using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;

namespace CLIF.Solutions.Code
{
    class MySiteRepositorySettings:IDisposable
    {
        public SPListItem GetRepositorySettings(string CurrentSiteTitle)
        {
            SPListItem item=null;
            using (SPWeb ObjWeb = SPHelper.GetLandingWeb())
            {
                //Getting default destination repository path
                SPList ObjList = ObjWeb.Lists["MySite Repository Settings"];
                SPQuery ObjQuery = new SPQuery();
                string _query = string.Format("<Where><Eq><FieldRef Name=\"Title\" /><Value Type=\"Text\">{0}</Value></Eq></Where>", SPContext.Current.Web.Title);
                ObjQuery.Query = _query;

                //Getting list item collection
                item = ObjList.GetItems(ObjQuery)[0];
            }
            return item;
        }
        public SPListItem GetRepositorySettings(string CurrentSiteTitle,string SiteURL)
        {
            SPListItem item = null;
            using (SPWeb ObjWeb = SPHelper.GetRootWeb(SPHelper.GetRootUrl(SiteURL)))
            {
                //Getting default destination repository path
                SPList ObjList = ObjWeb.Lists["MySite Repository Settings"];
                SPQuery ObjQuery = new SPQuery();
                string _query = string.Format("<Where><Eq><FieldRef Name=\"Title\" /><Value Type=\"Text\">{0}</Value></Eq></Where>", CurrentSiteTitle);
                ObjQuery.Query = _query;

                //Getting list item collection
                item = ObjList.GetItems(ObjQuery)[0];
            }
            return item;
        }

        #region IDisposable Members

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
