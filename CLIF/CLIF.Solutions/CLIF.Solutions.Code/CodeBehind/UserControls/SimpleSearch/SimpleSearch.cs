using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using Microsoft.Office.Server.Search.Query;
using System.Data;

namespace CLIF.Solutions.Code
{
    public class SimpleSearch : UserControl
    {
        private string _goImageUrl;
        private string _goImageUrlRTL;
        protected TextBox txtSearchText;
        protected ImageButton btnSearch;
        protected override void OnLoad(EventArgs e)
        {
            txtSearchText.Attributes.Add("onclick", "doClear();");
            base.OnLoad(e);
        }
        public string GoImageUrl
        {
            set
            {
                this._goImageUrl = value;
            }
        }
        public string GoImageUrlRTL
        {
            set
            {
                this._goImageUrlRTL = value;
            }
        }

        public void btnSearch_Click(object sender, ImageClickEventArgs e)
        {
            if (txtSearchText.Text.Trim() != "")
            {
                Response.Redirect(SPHelper.GetRootUrl(SPContext.Current.Site.Url) + "/pages/searchresults.aspx?k=" + txtSearchText.Text.Trim() + "&p=0");
            }
        }       
    }    
}
