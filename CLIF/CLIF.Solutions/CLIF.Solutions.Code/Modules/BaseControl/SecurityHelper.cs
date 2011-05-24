using System;
using System.Web;
using System.Collections.Generic;
using System.Text;
using System.Xml;

using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;

namespace CLIF.Solutions.Code
{
    public static class SecurityHelper
    {
        public delegate void SetDocumentDelegate();

        public static bool UserCanViewDraftItems()
        {
            try
            {
               // SPGroup viewDraftsGroup = SPContext.Current.Site.RootWeb.Groups["OnePlace View Draft Items"];
                if (System.Configuration.ConfigurationManager.AppSettings["StagingSite"].ToUpper()=="TRUE")
                //if (viewDraftsGroup.ContainsCurrentUser)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Exception " + ex.Message + " in UserCanViewDraftItems()");
                return false;
            }
        }

        // setDocMethod is a delegate pointing to the SetDocument() method of the XmlBasedControl
        // being executed
        public static void InvokeSetDocument(SetDocumentDelegate setDocMethod)
        {
            // check if the user is in the SharePoint group allowed to view draft items
            if (UserCanViewDraftItems())
            {
                // yes - elevate security so they have 'edit' permissions on the list items
                // which will enable them to view drafts
                SPSecurity.CodeToRunElevated elevatedSetDoc = new SPSecurity.CodeToRunElevated(setDocMethod);
                SPSecurity.RunWithElevatedPrivileges(elevatedSetDoc);
            }
            else
            {
                // no - so execute in the user's current context
                setDocMethod();
            }
        }

        public static SPList GetSpecifiedList(string listName)
        {
            // get fresh SPSite and SPWeb objects so they run in the correct context
            // depending on whether we have elevated privileges to view draft items
            // or not

            //Not sure if caching a good idea here or not...
            //object cacheData = HttpContext.Current.Cache["ListCache" + listName];

            //if (cacheData != null)
            //    return (SPList)cacheData;

            using (SPSite site = new SPSite(SPContext.Current.Site.ID))
            {
                if (listName.StartsWith("/"))
                {
                //    SPList lst=site.RootWeb.Lists[listName.Substring(1)];
                //    HttpContext.Current.Cache.Add("ListCache" + listName, lst, null, DateTime.Now.AddHours(12), System.Web.Caching.Cache.NoSlidingExpiration,
                //System.Web.Caching.CacheItemPriority.Default, null);
                    return site.RootWeb.Lists[listName.Substring(1)]; 
                }
                else
                {
                    using (SPWeb web = site.OpenWeb(SPContext.Current.Web.ID))
                    {
                    //    SPList lst = web.Lists[listName];
                    //    HttpContext.Current.Cache.Add("ListCache" + listName, lst, null, DateTime.Now.AddHours(12), System.Web.Caching.Cache.NoSlidingExpiration,
                    //System.Web.Caching.CacheItemPriority.Default, null);

                        return web.Lists[listName]; 
                    }
                }
            }
        }

        public static SPFile GetFile(string fileName)
        {
            SPFile file=null;
            SPListItem listItem=null;
            using (SPSite site = new SPSite(SPContext.Current.Site.ID))
            {
                fileName = site.Url + fileName;
                try
                {
                    listItem = site.RootWeb.GetListItem(fileName);
                }
                catch
                { }
                if (listItem != null)
                {
                    file = listItem.File;
                }
            }
            return file;
        }
    }
}
