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

namespace CLIF.Solutions.Code
{
    /**********************************************************************************
     Title:              AddItemToRepository
     Author:             Suresh Thampi
     Project:            CLIF.Solutions
     Date:               04/11/2010
     Copyright:          © Center for e-research(CeRch), King's College London 2010 All Rights Reserved.
     Description:     
                            
     ------------------------------------------------------------------------------
     Revision History
     Date             Ref       Author          Reason          Remarks        

     ***********************************************************************************/

    public class AddItemToRepository : LayoutsPageBase
    {
        protected Literal litTree;
        protected Label lblSelectPID;
        protected TreeView trwDestinationFolder;
        protected Label lblSelectFolder;
        protected Label lblSourceList;
        protected Label lblSourceFileName;
        protected Button btnSubmit;
        protected Panel pnlMain;
        protected Panel pnlConfirmation;
        protected Literal litMessage;
        protected HiddenField hdnNamespaceFormat;
        protected HiddenField hdnLabelFormat;
        protected HiddenField hdnPIDFormat;
        protected TreeviewControl tvwControl;
        protected ListBox lstSourceFiles;
        protected Panel pnlSourceFiles;
        protected Button btnBack;
        protected override void OnLoad(EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                try
                {
                    string _listId = GetCurrentListID();
                    int _itemId = GetCurrentListItemID();

                    SPList ObjCurrentList = SPContext.Current.Web.Lists[new Guid(_listId)];

                    lstSourceFiles.Items.Clear();
                    if (Request.QueryString["itemIds"] == null)
                    {
                        pnlSourceFiles.Visible = false;
                        btnBack.Visible = false;
                        SPListItem ObjItem = ObjCurrentList.Items.GetItemById(GetCurrentListItemID());
                        lblSourceList.Text = ObjCurrentList.Title;
                        lblSourceFileName.Text = ObjItem.File.Name;
                    }
                    else
                    {
                        btnBack.Visible = true;
                        pnlSourceFiles.Visible = true;
                        lblSourceList.Text = ObjCurrentList.Title;
                        string[] _itemIds = Request.QueryString["itemIds"].ToString().Split(',');                        
                        foreach (string itemid in _itemIds)
                        {
                            if (itemid != "")
                            {
                                lstSourceFiles.Items.Add(new ListItem(ObjCurrentList.Items.GetItemById(Convert.ToInt32(itemid)).File.Name, itemid));
                            }
                        }
                    }

                    SPListItem settings = null;
                    using (MySiteRepositorySettings ObjSettings = new MySiteRepositorySettings())
                    {
                        settings = ObjSettings.GetRepositorySettings(SPContext.Current.Web.Title);
                        if (settings != null)
                        {
                            hdnPIDFormat.Value = settings["PID Format"].ToString();
                            hdnNamespaceFormat.Value = settings["Namespace Format"].ToString();
                            hdnLabelFormat.Value = settings["Label Format"].ToString();
                        }
                        else
                        {
                            litMessage.Text = "Could not find default repository destination";
                        }
                    }
                    //Setting Namespace value
                    if (hdnNamespaceFormat.Value != null)
                    {
                        string _namespaceValue = hdnNamespaceFormat.Value;

                        //Setting {site} value
                        _namespaceValue = _namespaceValue.Replace("{site}", SPContext.Current.Web.Title);

                        //Setting {listname} value
                        _namespaceValue = _namespaceValue.Replace("{listname}", ObjCurrentList.Title.Replace(" ", ""));

                        hdnNamespaceFormat.Value = _namespaceValue;
                    }

                    //Setting ObjectLabel value
                    if (hdnLabelFormat.Value != null)
                    {
                        string _labelValue = hdnLabelFormat.Value;

                        //Setting {listname} value
                        _labelValue = _labelValue.Replace("{listname}", ObjCurrentList.Title.Replace(" ", ""));

                        hdnLabelFormat.Value = _labelValue;
                    }

                    //Setting PID value
                    if (hdnPIDFormat.Value != null)
                    {
                        string _pidValue = hdnPIDFormat.Value;

                        //Setting {listname} value
                        _pidValue = _pidValue.Replace("{listname}", ObjCurrentList.Title.Replace(" ", ""));

                        hdnPIDFormat.Value = _pidValue;
                    }
                }
                catch (Exception ex)
                {
                    litMessage.Text = ex.Message;
                }
            }
        }
        /// <summary>
        /// This method return current item ID from query string
        /// </summary>
        /// <returns>int</returns>
        private int GetCurrentListItemID()
        {
            int _itemId = 0;
            if (Request.QueryString["item"] != null)
            {
                _itemId = Convert.ToInt32(Request.QueryString["item"]);
            }
            return _itemId;
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
        protected void btnBack_Clicked(object sender, EventArgs e)
        {
            string _url = string.Format("/_layouts/clifpages/bulkcopy.aspx?itemIds={0}", Request["itemIds"].ToString());
            Response.Redirect(SPContext.Current.Site.Url + _url);
        }      
        public void btnSubmit_Click(object sender, System.EventArgs e)
        {
            string _selectedDestinationPID = string.Empty;
            string _nameSpace = hdnNamespaceFormat.Value;
            string _label = string.Empty;

            //Getting the selected repository PID                        
            if (Request.Form["ctl00$PlaceHolderMain$hdnSelectPID"] != null)
            {
                _selectedDestinationPID = Request.Form["ctl00$PlaceHolderMain$hdnSelectPID"].ToString();
            }
            else
            {
                litMessage.Text = "please select a destination";
                return;
            }
            
            //Getting the current site reference
            SPWeb ObjWeb = SPControl.GetContextWeb(Context);

            //Getting the source list
            SPList ObjCurrentList = ObjWeb.Lists[new Guid(GetCurrentListID())];

            //Check if Bulk Copy
            if (pnlSourceFiles.Visible == true)
            {
                bool _doCopy = false;
                if (Request.QueryString["move"] != null)
                {
                    if (Request.QueryString["move"] == "1")
                    {
                        _doCopy = false;
                    }
                    else
                    {
                        _doCopy = true;
                    }
                }
                //looping through each file in the list(Bulk Copy)
                foreach (ListItem li in lstSourceFiles.Items)
                {
                    SPListItem ObjItem = ObjWeb.Lists[new Guid(GetCurrentListID())].Items.GetItemById(Convert.ToInt32(li.Value));

                    //Add file to the repository
                    string _newPID= SPHelper.AddItemToRepository(SPControl.GetContextWeb(Context), ObjItem, _selectedDestinationPID);

                    if (_doCopy==false)
                    {                      
                       
                        //Creating Archive Object List
                        SPList ObjArchiveList = ObjWeb.Lists["Archive"];
                        SPListItem _archive = ObjArchiveList.Items.Add();
                        ObjWeb.AllowUnsafeUpdates = true;
                        _archive["Title"] = ObjItem.Title;
                        _archive["Created By"] = SPContext.Current.Web.CurrentUser;
                        _archive["Document Library"] = ObjItem.ParentList.Title;
                        _archive["Persistent ID"] = _newPID;
                        _archive.Update();     
                   
                        //Delete item from the "Project Documents" document library
                        ObjWeb.Lists[new Guid(GetCurrentListID())].Items.DeleteItemById(Convert.ToInt32(li.Value));
                        ObjWeb.AllowUnsafeUpdates = false;
                    }
                }
            }
            else
            {
                //Getting the list item
                SPListItem ObjItem = ObjWeb.Lists[new Guid(GetCurrentListID())].Items.GetItemById(GetCurrentListItemID());
 
                //Add file to the repository
                SPHelper.AddItemToRepository(SPControl.GetContextWeb(Context), ObjItem, _selectedDestinationPID);              
            }                       
            pnlConfirmation.Visible = true;
            pnlMain.Visible = false;
        }     
        /// <summary>
        /// This method populate treeview none on demand
        /// </summary>
        /// <param name="source">Object</param>
        /// <param name="e">TreeNodeEventArgs</param>
        public void PopulateNode(Object source, TreeNodeEventArgs e)
        {
            TreeNode node = e.Node;

            using (SPWeb web = SPControl.GetContextWeb(Context))
            {
                ContentObjectList _collectionList = null;

                //Getting all collections under current Collection
                _collectionList = Utilities.RepositoryHelper.GetCollectionsInCollection(node.Value);

                foreach (ContentObject contentObj in _collectionList)
                {
                    TreeNode newNode = new TreeNode(contentObj.Label, contentObj.ObjectPID);
                    newNode.ImageUrl = "~/_layouts/images/folder.gif";
                    if (Utilities.RepositoryHelper.GetCollectionsInCollection(node.Value).Count != 0)
                    {
                        newNode.PopulateOnDemand = true;
                    }
                    node.ChildNodes.Add(newNode);
                }
            }
        }
    }    
}
