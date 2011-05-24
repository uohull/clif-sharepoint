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
     Title:              BulkCopy/Move
     Author:             Suresh Thampi
     Project:            CLIF.Solutions
     Date:               04/01/2011
     Copyright:          © Center for e-research(CeRch), King's College London 2010 All Rights Reserved.
     Description:     
                                
     ------------------------------------------------------------------------------
     Revision History
     Date             Ref       Author          Reason          Remarks        

     ***********************************************************************************/

    public class BulkCopy : LayoutsPageBase    
    {
        protected CheckBoxList chkFiles;
        protected Label lblPageTitle;
        protected Label lblFileSelectCount;
        protected Button btnContinue;
        string _listID = string.Empty;
        string _itemID = string.Empty;
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
                
                btnContinue.Enabled = false;                      
                SPListItemCollection items = ObjList.Items;
                chkFiles.Items.Clear();
                foreach (SPListItem item in items)
                {
                    chkFiles.Items.Add(new ListItem(item.Title.ToString(), item.ID.ToString()));
                }                
            }
            IEnumerable<string> allChecked = (from item in chkFiles.Items.Cast<ListItem>()
                                              where item.Selected
                                              select item.Value);
            int _count = allChecked.Count();
            lblFileSelectCount.Text = _count + " file(s) selected.";
            if (_count == 0)
            {
                btnContinue.Enabled = false;
            }
            else
            {
                btnContinue.Enabled = true;
            }

            
            base.OnLoad(e);
        }

        protected void btnContinue_Clicked(object sender, EventArgs e)
        {
            string _url = string.Empty;
            string _itemIds=string.Empty;

            //Getting all checked files
            IEnumerable<string> allChecked = (from item in chkFiles.Items.Cast<ListItem>() 
                               where item.Selected 
                               select item.Value);

            foreach(string id in allChecked)
            {
                if(_itemIds!="")
                {
                    _itemIds = _itemIds + " ," + id;
                }
                else
                {
                    _itemIds = id;
                }
            }
            if (_itemIds != "")
            {
                _url = string.Format("/_layouts/clifpages/bulkcopybasket.aspx?list={0}&itemIds={1}&move={2}", "{" + _listID + "}", _itemIds,Request.QueryString["move"].ToString());
                Response.Redirect(SPContext.Current.Site.Url +  _url);
            }
        }
    }
}
