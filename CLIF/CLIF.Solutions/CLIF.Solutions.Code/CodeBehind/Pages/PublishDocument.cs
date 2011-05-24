using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Web.UI.WebControls;
using Common.PolicyManagement;
//using Common.Web.Utils;
using System.IO;
using uk.ac.hull.repository.hydranet.hydracontent.metadata;
using uk.ac.hull.repository.hydranet.service;
using uk.ac.hull.repository.hydranet.hydracontent;
using System.Drawing;
using Microsoft.SharePoint.Workflow;

namespace CLIF.Solutions.Code
{
    /**********************************************************************************
     Title:              PublishDocument
     Author:             Suresh Thampi
     Project:            CLIF.Solutions
     Date:               04/11/2010
     Copyright:          © Center for e-research(CeRch), King's College London 2010 All Rights Reserved.
     Description:     
                            
     ------------------------------------------------------------------------------
     Revision History
     Date             Ref       Author          Reason          Remarks        

     ***********************************************************************************/

    public class PublishDocument : LayoutsPageBase
    {
        protected Button btnYes;
        protected Button btnNo;
        private string _itemId;
        private string _listId;
        private string _sourceUrl;
        protected Label lblTitle;
        protected Label lblContentSubject;
        protected Label lblContentMimeType;
        protected Label lblPersistentID;
        protected Label lblAccessLevel;
        protected DropDownList ddlPublishTo;
        protected RadioButtonList rbtRequireApproval;
        protected RadioButtonList rbtMode;
        protected Panel pnlMain;
        protected Label lblMessage;
        protected override void OnLoad(EventArgs e)
        {

           
            if (Request.QueryString["SourceURL"] != null)
            {
                _sourceUrl = Request.QueryString["SourceURL"].ToString();
            }
            if (Request.QueryString["ID"] != null)
            {
                _itemId = Request.QueryString["ID"].ToString();
            }
            if (Request.QueryString["List"] != null)
            {
                _listId = Request.QueryString["List"].ToString();
            }
            if (!IsPostBack)
            {
                SPWeb ObjWeb = SPContext.Current.Web;
//                btnYes.PostBackUrl = ObjWeb.Site.Url + "/_layouts/CLIFPages/ClifSendFedora.aspx?ListId=" + _listId + "&ItemId=" + _itemId;
                SPList ObjList = ObjWeb.Lists[new Guid(_listId)];
                SPListItem item = ObjList.Items.GetItemById(Convert.ToInt32(_itemId));

                if (item.Workflows.Count == 0 )
                {
                    AttacheWorkflow(ObjWeb);
                    lblMessage.Visible = false;
                    btnYes.Enabled = true;
                }
                else
                {
                    if (item.Workflows[item.Workflows.Count-1].InternalState==SPWorkflowState.Running)
                    {
                        lblMessage.Visible = true;
                        lblMessage.Text = "Document approval request for publication already submitted.";
                        btnYes.Enabled = false;
                    }
                }

                            
                lblTitle.Text = item.Title;
                lblContentSubject.Text = item["Content Subject"].ToString();
                if (item["Content MimeType"] != null)
                {
                    lblContentMimeType.Text = item["Content MimeType"].ToString();
                }
                if (item["Persistent ID"] != null)
                {
                    lblPersistentID.Text = item["Persistent ID"].ToString();
                }
                else
                {
                    lblPersistentID.Text = "(auto-generated)";
                }

                //Populates all publication locations
                using (PublishableLocations ObjLocations = new PublishableLocations())
                {
                    SPListItemCollection _locations = ObjLocations.GetPublishableLocations();
                    int i = 0;
                    foreach (SPListItem _location in _locations)
                    {
                        i++;
                        ddlPublishTo.Items.Add(new ListItem(_location.Title, _location.ID.ToString()));
                    }
                }
            }
            base.OnLoad(e);            
        }
        /// <summary>
        /// This method return current item ID from query string
        /// </summary>
        /// <returns>int</returns>
        private int GetCurrentListItemID()
        {
            int _itemId = 0;
            if (Request.QueryString["ID"] != null)
            {
                _itemId = Convert.ToInt32(Request.QueryString["ID"]);
            }
            return _itemId;
        }
        /// <summary>
        /// This method attaches document approval workflow to the current list.
        /// </summary>
        /// <param name="_ObjWeb">SPWeb</param>
        private void AttacheWorkflow(SPWeb _ObjWeb)
        {
            //Attaching workflow to the "Project Documents" List.
            SPWorkflowTemplate baseTemplate = null;
            _ObjWeb.AllowUnsafeUpdates = true;
            for (int iCount = 0; iCount <= _ObjWeb.WorkflowTemplates.Count - 1; iCount++)
            {
                if (_ObjWeb.WorkflowTemplates[iCount].Id.ToString() == "51d62087-34c8-4a38-82d8-93ebe8f0c894")
                {
                    baseTemplate = _ObjWeb.WorkflowTemplates[iCount];
                    break;
                }
            }

            if (baseTemplate != null)
            {
                SPList _ObjList = _ObjWeb.Lists["Project Documents"];
                SPList tasks = _ObjWeb.Lists["Tasks"];
                //Checking if document approval workflow is already attached!
                if (_ObjList.WorkflowAssociations.Count == 0)
                {
                    _ObjWeb.AllowUnsafeUpdates = true;
                    SPList workflowhistory = null;
                    try
                    {
                        workflowhistory = _ObjWeb.Lists["Workflow History"];
                    }
                    catch
                    {
                        Guid HistoryListId = _ObjWeb.Lists.Add("Workflow History", "Workflow History", SPListTemplateType.WorkflowHistory);
                        workflowhistory = _ObjWeb.Lists[HistoryListId];
                    }
                    SPWorkflowAssociation newAssociation = null;
                    newAssociation = SPWorkflowAssociation.CreateListAssociation(baseTemplate, "Document Approval Workflow - (CLIF)", tasks, workflowhistory);
                    newAssociation.AutoStartChange =false;
                    newAssociation.AutoStartCreate = false;                   
                    _ObjList.AddWorkflowAssociation(newAssociation);
                    _ObjList.Update();
                    _ObjWeb.AllowUnsafeUpdates = false;
                }
            }
        }
        /// <summary>
        /// This method return current List ID from query string
        /// </summary>
        /// <returns>string</returns>
        private string GetCurrentListID()
        {
            string _listId = string.Empty;

            if (Request.QueryString["list"] != null)
            {
                _listId = Request.QueryString["list"];
            }
            return _listId;
        }
        /// <summary>
        /// This method updates List item in the document library
        /// </summary>
        /// <param name="Item">SPListItem</param>
        /// <param name="Web">SPWeb</param>
        private void UpdateSourceItem(SPListItem Item, SPWeb Web, string PID)
        {
            Web.AllowUnsafeUpdates = true;
            Item["Persistent ID"] = PID;
            Item["Publishable Status"] = "Published";
            Item.Update();
        }       
        public void btnYes_Clicked(Object sender, EventArgs e)
        {
            string _parentId = string.Empty;
            int _groupID = 0;
            lblMessage.Text = "";
            //SPFieldUserValue _approver = null;
            SPGroup _approverGroup=null;
            //Getting Publishing location details
            using (PublishableLocations ObjLocations = new PublishableLocations())
            {
                SPListItem _location = ObjLocations.GetPublishableLocationDetails(ddlPublishTo.SelectedValue);
                _groupID = Convert.ToInt32(_location["Document Approver"].ToString().Split(';')[0]);                
                if (_location["Persistent ID"] != null)
                {
                    _parentId = _location["Persistent ID"].ToString();
                }
            }

            if (_parentId == string.Empty)
            {
                lblMessage.Text = "Sorry, cannot publish this document.<br/> publishable location 'Persistent ID' not set for this location, please contact your system administrator.";
                return;
            }    
            //Check if Approval is Required
            if (rbtRequireApproval.SelectedValue == "1")
            {
                if (_groupID == null)
                {
                    lblMessage.Text = "Sorry, cannot publish this document.<br/> document approver(s) not set for this location, please contact your system administrator.";
                    return;
                }

                string siteUrl = SPControl.GetContextSite(Context).Url;
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    _approverGroup = SPHelper.GetLandingWeb().SiteGroups.GetByID(_groupID);              
                    using (SPSite ObjSite = new SPSite(siteUrl))
                    {
                        using (SPWeb ObjWeb = ObjSite.OpenWeb())
                        {
                            SPList ObjCurrentList = ObjWeb.Lists[new Guid(GetCurrentListID())];
                            SPListItem ObjItem = ObjCurrentList.Items.GetItemById(GetCurrentListItemID());
                            ObjWeb.AllowUnsafeUpdates = true;
                            using (DisabledItemEventsScope scope = new DisabledItemEventsScope())
                            {

                                ObjItem["Publishable Status"] = "Awaiting for approval";
                                foreach (SPUser _approver in _approverGroup.Users)
                                {
                                    ObjWeb.EnsureUser(_approver.LoginName);

                                    //Assign approver read permission to this file
                                    SPHelper.AssignApproverReadPermission(ObjItem, _approver, ObjWeb);
                                    ObjWeb.AllowUnsafeUpdates = true;
                                }
                                //SPHelper.SetFieldValueUser(ObjItem, "Document Approver", _approver.LoginName);

                                SPFieldUserValue groupvalueGet = new SPFieldUserValue(_approverGroup.ParentWeb, _approverGroup.ID, _approverGroup.Name);
                                ObjItem["Document Approver"] = groupvalueGet;
                                ObjItem.Update();
                            }

                            //If Approval Required Activate the Workflow
                            ObjWeb.AllowUnsafeUpdates = true;
                            SPWorkflowManager objWorkflowManager = ObjItem.Web.Site.WorkflowManager;
                            SPWorkflowAssociationCollection objWorkflowAssociationCollection = ObjCurrentList.WorkflowAssociations;
                            foreach (SPWorkflowAssociation objWorkflowAssociation in objWorkflowAssociationCollection)
                            {
                                if (String.Compare(objWorkflowAssociation.BaseId.ToString("B"), "{51d62087-34c8-4a38-82d8-93ebe8f0c894}", true) == 0)
                                {
                                    WorkflowEventData eventData = new WorkflowEventData();
                                    eventData.Add("Publish_Location_ID", ddlPublishTo.SelectedValue);
                                    eventData.Add("Publish_Location_Name", ddlPublishTo.SelectedItem.Text);
                                    objWorkflowManager.StartWorkflow(ObjItem, objWorkflowAssociation, eventData.ToString(), true);
                                    break;
                                }
                            }
                            ObjWeb.AllowUnsafeUpdates = false;
                            ObjWeb.Update();
                        }
                    }
                });
            }
            else
            {
                //If Approval NOT Required Upload the file directly
                using (SPWeb ObjWeb = SPControl.GetContextWeb(Context))
                {                    
                    
                    SPList ObjCurrentList = ObjWeb.Lists[new Guid(GetCurrentListID())];
                    SPListItem ObjItem = ObjCurrentList.Items.GetItemById(GetCurrentListItemID());
                    
                    //Add file to the repository
                    SPHelper.AddItemToRepository(ObjWeb, ObjItem, _parentId);
                }
            } 
            Response.Redirect(_sourceUrl);           
        }
        
        public void btnNo_Clicked(Object sender, EventArgs e)
        {
            Response.Redirect(_sourceUrl);
        }
    }
}
