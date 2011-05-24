using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Web.UI.WebControls;
using Common.PolicyManagement;
using Common.Web.Utils;
using System.IO;
using uk.ac.hull.repository.hydranet.hydracontent.metadata;
using uk.ac.hull.repository.hydranet.service;
using uk.ac.hull.repository.hydranet.hydracontent;
using uk.ac.hull.repository.utils;

namespace CLIF.Solutions.Code
{
    /**********************************************************************************
     Title:              ArchiveThisItem
     Author:             Suresh Thampi
     Project:            CLIF.Solutions
     Date:               04/11/2010
     Copyright:          © Center for e-research(CeRch), King's College London 2010 All Rights Reserved.
     Description:     
                            
     ------------------------------------------------------------------------------
     Revision History
     Date             Ref       Author          Reason          Remarks        

     ***********************************************************************************/

    public class ArchiveThisItem : LayoutsPageBase
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
            SPWeb ObjWeb = SPContext.Current.Web;
            SPList ObjList = ObjWeb.Lists[new Guid(_listId)];
            SPListItem item = ObjList.Items.GetItemById(Convert.ToInt32(_itemId));
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
                lblPersistentID.Text = "-";
            }
            base.OnLoad(e);
        }
        public void btnYes_Clicked(Object sender, EventArgs e)
        {
            SPWeb ObjWeb = SPContext.Current.Web;
            ObjWeb.AllowUnsafeUpdates = true;
            SPList ObjList = ObjWeb.Lists[new Guid(_listId)];
            SPListItem item = ObjList.Items.GetItemById(Convert.ToInt32(_itemId));
            
            //Creating Archive Object List
            SPList ObjArchiveList = ObjWeb.Lists["Archive"];
            SPListItem _archive = ObjArchiveList.Items.Add();
            _archive["Title"] = item.Title;
            _archive["Created By"] = SPContext.Current.Web.CurrentUser; 
            if (item["Persistent ID"] == null)
            {
                SPListItem settings = null;
                using (MySiteRepositorySettings ObjSettings = new MySiteRepositorySettings())
                {
                    settings = ObjSettings.GetRepositorySettings(SPContext.Current.Web.Title);

                    string _nameSpace = settings["Namespace Format"].ToString();

                    string _selectedDestinationPID = settings["Archive PID"].ToString();
                    //Add file to the repository --------------------------------------------------------------------------------
                    SPFile file = item.File;
                    if (file != null)
                    {
                        SPWeb spWeb = ObjWeb;

                        //MEtabasepath
                        //string METABASEPATH = "IIS://" + spWeb.Site.HostName + "/MimeMap";

                        //URL
                        //string url = spWeb.Url + '/' + file.Url;

                        //MimeType
                        string mimeType = MimeTypeHelper.GetContentType(file.Name);//MimeType.GetMimeTypeForExtension(METABASEPATH, Path.GetExtension(file.Name));

                        //Namespace
                        string nameSpace = ObjWeb.Title + "-" + ObjList.Title;

                        //Item Title
                        string itemTitle = item.Title.Trim().Length == 0 ? Path.GetFileNameWithoutExtension(item.Name.Trim()) : item.Title.Trim();

                        byte[] content = file.OpenBinary(SPOpenBinaryOptions.SkipVirusScan);
                        string author = "Unknown author";
                        if ((file.SourceFile != null) && !String.IsNullOrEmpty(file.SourceFile.Name))
                        {
                            author = file.SourceFile.Name.Trim();
                        }
                        else if ((file.Author != null) && !String.IsNullOrEmpty(file.Author.Name))
                        {
                            author = file.Author.Name.Trim();
                        }
                        _nameSpace = _nameSpace.Replace("{listname}", ObjList.Title.Replace(" ", ""));
                        HydraServiceFedoraImpl hydraService = new HydraServiceFedoraImpl();
                        _archive["Persistent ID"] = hydraService.DepositSimpleContentObject(_nameSpace, itemTitle, _selectedDestinationPID, content, mimeType, author, author);
                    }
                }
            }
            else
            {
                _archive["Persistent ID"] = item["Persistent ID"].ToString();
            }
            _archive["Document Library"] = ObjList.Title;
            _archive["Project Title"] = item["Project Title"];
            _archive["Content Subject"] = item["Content Subject"];
            _archive["Content MimeType"] = item["Content MimeType"];
            _archive["Content Coverage"] = item["Content Coverage"];
            _archive.Update();
            
            //Delete item from the "Project Documents" document library
            ObjList.Items.DeleteItemById(Convert.ToInt32(_itemId));
            ObjWeb.AllowUnsafeUpdates = false;
            Response.Redirect(ObjList.DefaultViewUrl);
        }
        public void btnNo_Clicked(Object sender, EventArgs e)
        {
            Response.Redirect(_sourceUrl);
        }
    }
}
