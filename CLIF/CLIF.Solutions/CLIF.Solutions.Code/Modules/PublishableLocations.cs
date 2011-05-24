using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;

namespace CLIF.Solutions.Code
{
    public class PublishableLocations:IDisposable
    {
   
        /// <summary>
        /// This method returns all publication locations
        /// </summary>
        /// <returns>SPListItemCollection</returns>
        public SPListItemCollection GetPublishableLocations()
        {
            SPListItemCollection items = null;
            using (SPWeb ObjWeb = SPHelper.GetLandingWeb())
            {
                //Getting default destination repository path
                SPList ObjList = ObjWeb.Lists["Publishable Locations"];
                items = ObjList.Items;                
            }
            return items;
        }
        /// <summary>
        /// This method returns a publication location detail from the root site
        /// </summary>
        /// <param name="ID">string</param>
        /// <returns>SPListItem</returns>
        public SPListItem GetPublishableLocationDetails(string ID)
        {
            SPListItem item = null;
            using (SPWeb ObjWeb = SPHelper.GetLandingWeb())
            {
                //Getting default destination repository path
                SPList ObjList = ObjWeb.Lists["Publishable Locations"];
                item = ObjList.Items.GetItemById(Convert.ToInt32(ID));                           
            }
            return item;
        }
        /// <summary>
        /// This method gets the publishable location by ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public string GetPublishableLocationPID(string ID, string CurrentSiteURL)
        {
            string _pID = string.Empty;
            using (SPWeb ObjWeb = SPHelper.GetRootWeb(SPHelper.GetRootUrl(CurrentSiteURL)))
            {
                //Getting default destination repository path
                SPList ObjList = ObjWeb.Lists["Publishable Locations"];
                SPListItem item = ObjList.Items.GetItemById(Convert.ToInt32(ID));
                if (item != null)
                {
                    _pID = item["Persistent ID"].ToString();
                }
            }
            return _pID;
        }

        #region IDisposable Members

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
