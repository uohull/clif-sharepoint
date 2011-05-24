using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint.WebControls;
using Microsoft.SharePoint;
using System.Web.UI.WebControls;

namespace CLIF.Solutions.Code
{
    /**********************************************************************************
     Title:              BulkCopyBasket
     Author:             Suresh Thampi
     Project:            CLIF.Solutions
     Date:               04/01/2011
     Copyright:          © Center for e-research(CeRch), King's College London 2010 All Rights Reserved.
     Description:     
                                
     ------------------------------------------------------------------------------
     Revision History
     Date             Ref       Author          Reason          Remarks        

     ***********************************************************************************/

    public class BulkCopyBasket : LayoutsPageBase    
    {       
        protected ListBox lstFiles;
        protected Label lblTitle;
        protected Label lblFileSelectCount;
        protected Button btnContinue;
        string _listID = string.Empty;
        string _itemID = string.Empty;
        protected Label lblPageTitle;
        protected Label lblContentSubject;
        protected Label lblMimeType;
        protected Label lblAccessPermission;
        protected Label lblPublishableStatus;
        protected Label lblContentLanguage;
        protected Label lblContentSource;
        protected Button btnRemove;
        protected Button btnAdd;
        protected Label lblPID;

        protected override void OnLoad(EventArgs e)
        {
            
            SPList ObjList = SPContext.Current.Web.Lists["Project Documents"];
            _listID = ObjList.ID.ToString();  
 
            if (!IsPostBack)
            {
                //Setting the page title
                if (Request.QueryString["move"] != null)
                {
                    if (Request.QueryString["move"] == "1")
                    {
                        lblPageTitle.Text = "Move multitple files to the repository.";
                    }
                    else
                    {
                        lblPageTitle.Text = "Copy multitple files to the repository.";
                    }
                }
                lstFiles.Items.Clear();
                if (Request.QueryString["itemIds"] != null)
                {
                  string[] _ids = Request.QueryString["itemIds"].ToString().Split(',');
                  foreach (string _itemId in _ids)
                  {
                      if (_itemId != "")
                      {
                          SPListItem ObjItem = ObjList.Items.GetItemById(Convert.ToInt32(_itemId));
                          lstFiles.Items.Add(new ListItem(ObjItem.Title,ObjItem.ID.ToString()));
                      }
                  }
                  if (lstFiles.Items.Count != 0)
                  {
                      lstFiles.SelectedIndex = 0;
                      GetDetails(Convert.ToInt32(lstFiles.SelectedValue));
                  }                  
                }         
            }                         
            base.OnLoad(e);
        }

        protected void btnRemove_Clicked(object sender, EventArgs e)
        {
            lstFiles.Items.Remove(lstFiles.SelectedItem);
            if (lstFiles.Items.Count != 0)
            {
                lstFiles.SelectedIndex = 0;
                GetDetails(Convert.ToInt32(lstFiles.SelectedValue));
            }
            else
            {
                lblTitle.Text = "";
                lblContentSubject.Text = "";
                lblMimeType.Text = "";
                lblContentLanguage.Text = "";
                lblContentSource.Text = "";
                lblPublishableStatus.Text = "";
                lblPID.Text = "";
                //btnRemove.Enabled = false;
                //btnAdd.Enabled = false;
            }
        }
        protected void btnBack_Clicked(object sender, EventArgs e)
        {
            string _url = string.Format("/_layouts/clifpages/bulkcopy.aspx?itemIds={0}",Request["itemIds"].ToString());
            Response.Redirect(SPContext.Current.Site.Url + _url);
        }
        protected void lstFiles_OnSelectedIndexChanged(Object sender, EventArgs e)
        {
            GetDetails(Convert.ToInt32(lstFiles.SelectedValue));
             
        }
        private void GetDetails(int ItemId)
        {

            lblTitle.Text = "(unknown)";
            lblContentSubject.Text = "(unknown)";
            lblMimeType.Text = "(unknown)";
            lblContentLanguage.Text = "(unknown)";
            lblContentSource.Text = "(unknown)";
            lblPublishableStatus.Text = "(unknown)";
            lblPID.Text = "(unknown)";
            SPList ObjList = SPContext.Current.Web.Lists["Project Documents"];
            SPListItem ObjItem = ObjList.Items.GetItemById(ItemId);
            lblTitle.Text = ObjItem.Title;
            lblContentSubject.Text = ObjItem["Content Subject"].ToString();
            if (ObjItem["Content MimeType"] != null)
            {
                lblMimeType.Text = ObjItem["Content MimeType"].ToString();
            }
            if (ObjItem["Content Language(s)"] != null)
            {
                lblContentLanguage.Text = ObjItem["Content Language(s)"].ToString();
            }
            if (ObjItem["Content Source(s)"] != null)
            {
                lblContentSource.Text = ObjItem["Content Source(s)"].ToString();
            }
            if (ObjItem["Publishable Status"] != null)
            {
                lblPublishableStatus.Text = ObjItem["Publishable Status"].ToString();
            }
            if (ObjItem["Persistent ID"] != null)
            {
                lblPID.Text = ObjItem["Persistent ID"].ToString();
            }
            
        }
        protected void btnAdd_Clicked(object sender, EventArgs e)
        {   
            if(Request.QueryString["move"]!=null)
            {
                string _itemIDs = string.Empty;
                foreach (ListItem item in lstFiles.Items)
                {
                    if (_itemIDs != "")
                    {
                        _itemIDs = _itemIDs + "," + item.Value;
                    }
                    else
                    {
                        _itemIDs = item.Value;
                    }
                }
                string _url = string.Format("/_layouts/clifpages/additemtorepository.aspx?move=" + Request.QueryString["move"].ToString() + "&itemIds={0}&list={1}", _itemIDs, _listID.ToString());
                Response.Redirect(SPContext.Current.Site.Url + _url);
            }
        }
    }
}
