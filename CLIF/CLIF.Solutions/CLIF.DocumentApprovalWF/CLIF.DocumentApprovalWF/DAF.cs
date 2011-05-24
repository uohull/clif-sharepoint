using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Drawing;
using System.Linq;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Workflow;
using Microsoft.SharePoint.WorkflowActions;
using Microsoft.Office.Workflow.Utility;
using CLIF.Solutions.Code;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Xml.Linq;
namespace CLIF.DocumentApprovalWF
{
    public sealed partial class DAF : SequentialWorkflowActivity
    {
        /**********************************************************************************
         Title:              DAF
         Author:             Suresh Thampi
         Project:            CLIF.DocumentApprovalWF
         Date:               04/02/2011
         Copyright:          © Center for e-research(CeRch), King's College London 2010 All Rights Reserved.
         Description:                                         
         ------------------------------------------------------------------------------
         Revision History
         Date             Ref       Author          Reason          Remarks        
         ***********************************************************************************/

        //==========================================================================================================================================
       //Private Variables
        string _approverNotes;
        string _publishableStatus = string.Empty;
        int _documentApprovalTaskId = 0;
        bool _approvalTaskCompleted = false;
        bool _approvalStatus = false;        
        public Guid workflowId = default(System.Guid);
        public SPWorkflowActivationProperties DAworkflowProperties = new Microsoft.SharePoint.Workflow.SPWorkflowActivationProperties();
        public SPWorkflowActivationProperties workflowProperties = new Microsoft.SharePoint.Workflow.SPWorkflowActivationProperties();
        public SPWorkflowTaskProperties TaskProperties = new Microsoft.SharePoint.Workflow.SPWorkflowTaskProperties();
        public SPWorkflowTaskProperties onTaskCreated_AfterProperties = new Microsoft.SharePoint.Workflow.SPWorkflowTaskProperties();
        public SPWorkflowTaskProperties onTaskChanged_AfterProperties = new Microsoft.SharePoint.Workflow.SPWorkflowTaskProperties();
        public SPWorkflowTaskProperties onTaskChanged_BeforeProperties = new Microsoft.SharePoint.Workflow.SPWorkflowTaskProperties();
        //==========================================================================================================================================

        /// <summary>
        /// Default constructor
        /// </summary>
        public DAF()
        {
            InitializeComponent();
        }
        /// <summary>
        /// This Method handles CreateApprovalEmail code activity.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        private void CreateApprovalEmail_ExecuteCode(object sender, EventArgs e)
        {
            if (workflowProperties.Item.File.ModifiedBy.Email != null)
            {
                SendStatusEmail.To = workflowProperties.Item.File.ModifiedBy.Email.ToString();
                SendStatusEmail.From = CreateAppovalTask.TaskProperties.AssignedTo;
                SendStatusEmail.Subject = "Document Approved - CLIF";
                SendStatusEmail.Body = "Your Expense Report: " +
                                 "<a href=\"" +
                                    workflowProperties.SiteUrl + workflowProperties.Item.File.ServerRelativeUrl +
                                  "\">" + workflowProperties.Item.File.Name + "</a>" +
                                  " has been Approved!" +
                                  "<p><p>" +
                                  "<b><u>Approve Notes</u></b><p><p>" +
                                  this._approverNotes;
            }
        }
        /// <summary>
        /// This Method handles CreateRejectedEmail code activity.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        private void CreateRejectedEmail_ExecuteCode(object sender, EventArgs e)
        {
            if (workflowProperties.Item.File.ModifiedBy.Email != null)
            {
                SendStatusEmail.To = workflowProperties.Item.File.ModifiedBy.Email.ToString();
                SendStatusEmail.From = CreateAppovalTask.TaskProperties.AssignedTo;
                SendStatusEmail.Subject = "Document Rejected";
                SendStatusEmail.Body = "Your document: " +
                                 "<a href=\"" +
                                    workflowProperties.SiteUrl + workflowProperties.Item.File.ServerRelativeUrl +
                                  "\">" + workflowProperties.Item.File.Name + "</a>" +
                                  " has been Rejected!" +
                                  "<p><p>" +
                                  "<b><u>Approve Notes</u></b><p><p>" +
                                  this._approverNotes;
            }
        }
        private SPGroup GetSiteGroup(SPWeb web, string name)
        {
            foreach (SPGroup group in web.Groups)
            {
                if (group.Name.ToLower() == name.ToLower())
                {
                    return group;
                }
            }
            return null;
        }
        /// <summary>
        /// This Method handles CreateAppovalTask
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        private void CreateAppovalTask_MethodInvoking(object sender, EventArgs e)
        {
            string strApprovers = string.Empty;        
            
            //Setting approvers for the document approval.
            SPSecurity.RunWithElevatedPrivileges(delegate()
             {

                 int _groupID = Convert.ToInt32(workflowProperties.Item["Document Approver"].ToString().Split(';')[0]);
                 using (SPWeb ObjRootWeb = SPHelper.GetRootWeb(SPHelper.GetRootUrl(workflowProperties.SiteUrl)))
                 {
                     SPGroup _approverGroup = ObjRootWeb.SiteGroups.GetByID(_groupID);                     
                     try
                     {
                         workflowProperties.Web.SiteGroups.Add(_approverGroup.Name, workflowProperties.Web.CurrentUser, null, _approverGroup.Description);
                     }
                     catch { }

                     foreach (SPUser user in _approverGroup.Users)
                     {
                         SPUser _user= workflowProperties.Web.EnsureUser(user.LoginName);
                         workflowProperties.Web.SiteGroups[_approverGroup.Name].AddUser(_user);
                     }
                     //SPFieldUserValue groupvalueGet = new SPFieldUserValue(group.ParentWeb, group.ID, group.Name);
                     CreateAppovalTask.TaskProperties.AssignedTo = _approverGroup.Name;
                 }
             });
            // Define a HybridDictionary object
            HybridDictionary permsCollection = new HybridDictionary();

            // Give Administrator rights to the user to whom the task has been assigned
            permsCollection.Add(CreateAppovalTask.TaskProperties.AssignedTo, SPRoleType.Administrator);

            // SpecialPermissions -the SpecialPermissions property  in your code will strip out all existing permissions inherited from
            // the parent list(Workflow Task List) and only adds permissions for each pair you added to the hashtable
            CreateAppovalTask.SpecialPermissions = permsCollection;

            //Setting the due date for approval
            CreateAppovalTask.TaskProperties.DueDate = DateTime.Now.AddDays(7);
               
            // Setting the title for the task
            CreateAppovalTask.TaskProperties.Title = workflowProperties.Item.Title;

            // Sending an Outlook Email and Outlook Task
            CreateAppovalTask.TaskProperties.SendEmailNotification = true;

            SPListItem ObjItem = workflowProperties.Item;
            using (DisabledItemEventsScope scope = new DisabledItemEventsScope())
            {
                ObjItem["Publishable Status"] = "Awaiting for approval";
                ObjItem.SystemUpdate();
            }            
        }
        /// <summary>
        /// This Method handles TaskChanged
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        private void OnApprovalTaskChanged_Invoked(object sender, ExternalDataEventArgs e)
        {            
            // Get the Approval Status (Approved or Declined) from the Workflow Form
            this._approvalStatus = System.Convert.ToBoolean(onTaskChanged_AfterProperties.ExtendedProperties["IsApproved"]);
            
            //Get approver's comments
            this._approverNotes = onTaskChanged_AfterProperties.ExtendedProperties["Comments"].ToString();
            
            // Set the Approval Task as completed
            this._approvalTaskCompleted = true;
            using (SPSite CurrentSite = new SPSite(workflowProperties.Web.Url))
            {
                using (SPWeb CurrentWeb = CurrentSite.OpenWeb())
                {
                    if (this._approvalStatus == true)
                    {
                        _publishableStatus = "Published";

                        //Submit file to the repository
                        SubmitFileToRepository(CurrentWeb);
                    }
                    else
                    {
                        _publishableStatus = "Publication request rejected";
                    }

                    //Update approval status
                    UpdateApprovalStatus(workflowProperties.Item.ID, workflowProperties.List.Title,CurrentWeb, _publishableStatus);
                }
            }
        }
        /// <summary>
        /// This method copies file to the repository
        /// </summary>
        /// <param name="CurrentWeb">SPWeb</param>
        private void SubmitFileToRepository(SPWeb CurrentWeb)
        {
            string initiationData = workflowProperties.InitiationData;

            // Getting Publishbale Location ID 
            var Publish_Location_ID = from _value in XElement.Load(new StringReader(EscapeXml(initiationData))).Elements("Publish_Location_ID")
                                      select _value;
            string _locationID = string.Empty;
            foreach (var ID in Publish_Location_ID)
            {
                _locationID = ID.Value;
            }

            string _locationPID = string.Empty;
            using (PublishableLocations ObjLocation = new PublishableLocations())
            {
                _locationPID = ObjLocation.GetPublishableLocationPID(_locationID, workflowProperties.Web.Url);
            }
            //Publish this document to fedora
            SPHelper.SubmitItemToRepository(CurrentWeb, workflowProperties.Item, _locationPID);
        }
        /// <summary>
        /// This method updates approval Status
        /// </summary>
        /// <param name="ItemID">int</param>
        /// <param name="ListName">string</param>
        /// <param name="CurrentWeb">SPWeb</param>
        /// <param name="Status">string</param>
        private void UpdateApprovalStatus(int ItemID,string ListName,SPWeb CurrentWeb,string Status)
        {  
           using (DisabledItemEventsScope scope = new DisabledItemEventsScope())
            {
                CurrentWeb.AllowUnsafeUpdates = true;
                SPList list = CurrentWeb.Lists[ListName];
                SPListItem item = list.Items.GetItemById(ItemID);
                item["Publishable Status"] = Status;
                item.SystemUpdate();
                CurrentWeb.AllowUnsafeUpdates = false;
            }

            //Add Task Refs in the root site.
            using (DocumentApprovalTasks tasks = new DocumentApprovalTasks())
            {
                using (SPWeb ObjRootWeb = SPHelper.GetRootWeb(SPHelper.GetRootUrl(workflowProperties.SiteUrl)))
                {
                    tasks.UpdateTask(_documentApprovalTaskId, "Complete", ObjRootWeb);
                }
            }
        }
        /// <summary>
        /// This method handle create task event
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        private void logTaskCreated(object sender, EventArgs e)
        {
            //Add Task Refs in the root site.
            using (DocumentApprovalTasks tasks = new DocumentApprovalTasks())
            {
                using (SPWeb ObjRootWeb = SPHelper.GetRootWeb(SPHelper.GetRootUrl(workflowProperties.SiteUrl)))
                {
                    //Getting Created By - Login Name
                    SPFieldUserValue val = new SPFieldUserValue(workflowProperties.Item.Web, workflowProperties.Item["Created By"] as string);
                    SPUser usr = val.User;
                    string _documentAuthor = usr.LoginName;                    
                    string initiationData = workflowProperties.InitiationData;                 
                 
                    // Getting Publishbale Location ID 
                    var Publish_Location_ID = from _value in XElement.Load(new StringReader(EscapeXml(initiationData))).Elements("Publish_Location_ID")
                                select _value;
                    string _locationID = string.Empty;
                    foreach (var ID in Publish_Location_ID)
                    {
                        _locationID = ID.Value;
                    }
                    

                    // Getting Publishbale Location Name
                    var Publish_Location_Name = from _value in XElement.Load(new StringReader(EscapeXml(initiationData))).Elements("Publish_Location_Name")
                                              select _value;                    
                    string _locationName = string.Empty;
                    foreach (var Name in Publish_Location_Name)
                    {
                        _locationName = Name.Value;
                    }

                    SPFieldLookupValue _publishableLocation = new SPFieldLookupValue(Convert.ToInt32(_locationID), _locationName); ;

                    //_publishableLocation=new SPFieldLookupValue(location
                    //Getting Approver - Login Name          
                    string _taskUrl = workflowProperties.Item.Web.Url + "/Lists/DocumentApprovalTasks/DispForm.aspx?ID=" + CreateAppovalTask.ListItemId.ToString();
                                        
                    //Add Task Ref in the root site  
                    _documentApprovalTaskId = tasks.AddTask(CreateAppovalTask.TaskProperties.Title, CreateAppovalTask.TaskProperties.DueDate, CreateAppovalTask.TaskProperties.AssignedTo, _documentAuthor, _taskUrl, ObjRootWeb, _publishableLocation);

                    //int _groupID = Convert.ToInt32(CreateAppovalTask.TaskProperties.AssignedTo.ToString().Split(';')[0]);
                    SPGroup _approverGroup = ObjRootWeb.Groups[CreateAppovalTask.TaskProperties.AssignedTo];
      
                    //Send Email notification to the approver
                    SendApproverEmail(_approverGroup, "CLIF");
                }
            }
            LogToHistoryListActivity log = (LogToHistoryListActivity)sender;
            if (log != null)
            {
                log.HistoryDescription = "Document approval task created.";
            }
        }
        public string EscapeXml(string input)
        {
            string output = string.Empty;
            output = input.Replace("&", "&amp;");
            return output;
        }
        private List<string> getEmailAddressesFromGroup(SPGroup group)
        {
            List<string> addresses = new List<string>();
            foreach (SPUser user in group.Users)
            {
                addresses.Add(user.Email);
            }
            return addresses;
        }
        /// <summary>
        /// This method send approver email notifications
        /// </summary>
        /// <param name="To">string</param>
        /// <param name="From">string</param>
        private void SendApproverEmail(SPGroup ApproverGroup,string From)
        {
            string _toAddress=string.Empty;
            List<string> addresses=getEmailAddressesFromGroup(ApproverGroup);
            foreach (string address in addresses)
            {
                if (_toAddress != "")
                {
                    _toAddress = _toAddress + address;
                }
                else
                {
                    _toAddress = address;
                }
            }
            //SendApproverEmail.To = _toAddress;
            //SendApproverEmail.From = From;
            //SendApproverEmail.Subject = "Document approval request - CLIF";
            //SendApproverEmail.Body = "<p><strong>Document approval request</strong></p>" +
            //                 "Please review this document <a href=\"" +
            //                    workflowProperties.SiteUrl + workflowProperties.Item.File.ServerRelativeUrl +
            //                  "\">" + workflowProperties.Item.File.Name + "</a>";
        }
        /// <summary>
        /// This method handle task complete event
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        private void log_TaskComplete_Method(object sender, EventArgs e)
        {
            string _message = string.Empty;
            _message = "Status : " + this._publishableStatus + " Details : " + this._approverNotes;
            LogToHistoryListActivity log = (LogToHistoryListActivity)sender;
            if (log != null)
            {
                log.HistoryDescription = _message;
            }
        }       
    }
}
