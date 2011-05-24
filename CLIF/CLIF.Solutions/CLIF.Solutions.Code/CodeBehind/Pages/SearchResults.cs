using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint.WebControls;
using Microsoft.SharePoint;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Web;

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

    public class SearchResults : LayoutsPageBase    
    {   
        protected TextBox txtSearchText;
        private string _keyword;
        protected override void OnLoad(EventArgs e)
        {
            txtSearchText.Attributes.Add("onclick", "doClear();");
            if (!IsPostBack)
            {   
                if (Request.QueryString["k"] != null)
                {
                    txtSearchText.Text = Request.QueryString["k"].ToString();
                }
                if (Request.QueryString["p"] == null)
                {
                    var nameValues = HttpUtility.ParseQueryString(Request.QueryString.ToString());
                    nameValues.Add("p", "0");                    
                    string url = Request.Url.AbsolutePath;
                    string updatedQueryString = "?" + nameValues.ToString();
                    Response.Redirect(url + updatedQueryString);
                }
            }
            base.OnLoad(e);
        }
        public void btnSearch_Click(object sender, EventArgs e)
        {
            _keyword = txtSearchText.Text.Trim();
            if (_keyword != "")
            {
                var nameValues = HttpUtility.ParseQueryString(Request.QueryString.ToString());
                nameValues.Set("k", _keyword);
                string url = Request.Url.AbsolutePath;
                string updatedQueryString = "?" + nameValues.ToString();
                Response.Redirect(url + updatedQueryString);
            }
        }
    }
}
