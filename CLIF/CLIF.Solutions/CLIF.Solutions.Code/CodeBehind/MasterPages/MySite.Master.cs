using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.HtmlControls;
using Microsoft.SharePoint;
using System.Web.UI.WebControls;

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

    public class MySiteMaster : System.Web.UI.MasterPage
    {
        #region Private Variables & Properties
        protected Literal litBanner;
        #endregion

        protected override void OnLoad(EventArgs e)
        {
            string _banner = string.Empty;
            string _rootUrl = SPHelper.GetRootUrl(SPContext.Current.Site.Url);
            _banner = "<a href=\"" + _rootUrl + "\" >";
            _banner += "<img id=\"Img2\" border=\"0\" width=\"131\" height=\"87\" src=\"" + _rootUrl + "/SiteCollectionImages/CLIF/KCL/KingsLogo.jpg\" runat=\"server\" alt=\"Kings College London\" />";
            //_banner += "<img width=\"195\"  border=\"0\" height=\"58\" id=\"Img3\" src=\"" + _rootUrl + "/SiteCollectionImages/CLIF/KCL/OneSpaceLogo.jpg\" runat=\"server\" alt=\"OneSpace\" />";
            _banner += "</a>";
            litBanner.Text = _banner;
            base.OnLoad(e);
        }       
    }
}
