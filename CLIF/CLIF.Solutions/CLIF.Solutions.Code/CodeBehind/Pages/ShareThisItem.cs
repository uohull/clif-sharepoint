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

    public class ShareThisItem : LayoutsPageBase
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

            if (item["Access Level"] != null)
            {
                if (item["Access Level"].ToString().Split(',')[1].Trim() == "Shared")
                {
                    lblAccessLevel.Text = "Shared";
                    lblAccessLevel.ForeColor = Color.Green;
                    btnYes.Text = "Do not share"; 
                }
                else
                {
                    lblAccessLevel.Text = "Private";
                    lblAccessLevel.ForeColor = Color.Red;
                    btnYes.Text = "Share it";
                }
            }
            else
            {
                lblAccessLevel.Text = "Private";
                lblAccessLevel.ForeColor = Color.Red;
                btnYes.Text = "Share it";
            }
            base.OnLoad(e);
        }
        public void btnYes_Clicked(Object sender, EventArgs e)
        {
            SPWeb ObjWeb = SPContext.Current.Web;
            ObjWeb.AllowUnsafeUpdates = true;
            SPList ObjList = ObjWeb.Lists[new Guid(_listId)];
            SPListItem item = ObjList.Items.GetItemById(Convert.ToInt32(_itemId));

            if (btnYes.Text == "Share it")
            {
                SPHelper.ShareItem(ObjWeb, item,true);                                
            }
            else
            {
                SPHelper.ShareItem(ObjWeb, item,false);
            }
            Response.Redirect(_sourceUrl);
        }
        public void btnNo_Clicked(Object sender, EventArgs e)
        {
            Response.Redirect(_sourceUrl);
        }
    }
}
