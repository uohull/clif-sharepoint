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
using System.Web;

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

    public class RepositoryExplorer : LayoutsPageBase
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
        protected DropDownList ddlRoot;
        protected DropDownList ddlPublishableLocations;
        string _rootFolder = string.Empty;
        protected override void OnLoad(EventArgs e)
        {
            if (!IsPostBack)
            {
                using (PublishableLocations ObjLocations = new PublishableLocations())
                {
                    ddlPublishableLocations.Items.Clear();
                    SPListItemCollection items = ObjLocations.GetPublishableLocations();
                    foreach (SPListItem item in items)
                    {
                        ddlPublishableLocations.Items.Add(new ListItem(item.Title, item["Persistent ID"].ToString()));
                    }
                    if (Request.QueryString["rf"] != null)
                    {
                        _rootFolder=Request.QueryString["rf"].ToString();
                        ddlRoot.SelectedValue = _rootFolder;
                        if (_rootFolder == "2")
                        {
                            ddlPublishableLocations.Visible = true;
                            if (Request.QueryString["pid"] != null)
                            {
                                ddlPublishableLocations.SelectedValue = Request.QueryString["pid"].ToString();
                            }
                        }
                        else
                        {
                            ddlPublishableLocations.Visible = false;
                        }
                    }
                }
            }
        }       
        /// <summary>
        /// This method populate treeview none on demand
        /// </summary>
        /// <param name="source">Object</param>
        /// <param name="e">TreeNodeEventArgs</param>
        public void PopulateNode(Object source, TreeNodeEventArgs e)
        {
            TreeNode node = e.Node;

            if (_rootFolder == "1")
            {
                return;
            }
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
        protected void ddlRoot_SelectedIndexChanged(object sender,EventArgs e)
        {
            if (ddlRoot.SelectedValue == "2")
            {
                ddlPublishableLocations.Visible = true;
            }
            else
            {
                ddlPublishableLocations.Visible = false;
            }
        }
        public void btnSearch_Click(object sender, EventArgs e)
        {
            string _rootFolder = string.Empty;
            string _rootPID = string.Empty;
            switch(ddlRoot.SelectedValue)
            {
                case "0":
                    using (MySiteRepositorySettings ObjSettings = new MySiteRepositorySettings())
                    {
                        SPListItem settings = ObjSettings.GetRepositorySettings(SPContext.Current.Web.Title);
                        if (ObjSettings != null)
                        {
                            _rootPID = settings["Default Root Object"].ToString();
                        }
                    }                    
                    break;
                case "1":
                    _rootPID = "CLIF:Root";
                    break;
                case "2":
                    _rootPID = ddlPublishableLocations.SelectedValue;
                    break;
            }
            _rootFolder = ddlRoot.SelectedValue;
            var nameValues = HttpUtility.ParseQueryString(Request.QueryString.ToString());
            nameValues.Set("pid", _rootPID);
            nameValues.Set("rf", _rootFolder);
            string url = SPContext.Current.Web.Url + "/" + Request.Url.AbsolutePath;
            string updatedQueryString = "?" + nameValues.ToString();
            Response.Redirect(url + updatedQueryString);
        }
    }    
}
