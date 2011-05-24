using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace CLIF.Solutions.Code
{
    public class ShowUserDepartment : UserControl
    {
        protected Label lblDepartment;
        protected override void OnLoad(EventArgs e)
        {
            if (!IsPostBack)
            {
                SPWeb web = SPContext.Current.Web;
                SPUser spUser = web.CurrentUser;

                SPList userList = web.SiteUserInfoList;
                SPListItem user = userList.Items.GetItemById(spUser.ID);
                if (user["Department"] != null)
                {
                    lblDepartment.Text = user["Department"].ToString();
                }
                else
                {
                    lblDepartment.Text = "unknown";
                }
            }
            base.OnLoad(e);
        }
    }
}
