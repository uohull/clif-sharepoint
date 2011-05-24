using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using System.Xml;
using System.Diagnostics;

namespace CLIF.Solutions.Code
{
    class SiteMasterPage : SPFeatureReceiver
    {
        private const string MASTERPAGEFILE = "MasterPageFile";

        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            char[] slashes = { '/' };
            string _masterPagePath = string.Empty;
            try
            {
                SPSite ObjSite = properties.Feature.Parent as SPSite;
                
                //Getting the master page file path.
                SPFeatureProperty masterFile = properties.Definition.Properties[MASTERPAGEFILE];
                
                //Getting Site Ref
                foreach (SPWeb ObjWeb in ObjSite.AllWebs)
                {
                    if (ObjWeb.IsRootWeb)
                    {
                        _masterPagePath=ObjWeb.ServerRelativeUrl.TrimEnd(slashes) + "/_catalogs/masterpage/" + masterFile.Value;
                    }
                    //Updating master pages for all sites;
                    ObjWeb.CustomMasterUrl = _masterPagePath;
                    ObjWeb.MasterUrl = _masterPagePath;
                    ObjWeb.ApplyTheme("simple");
                    ObjWeb.Update();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }    
        public override void FeatureInstalled(SPFeatureReceiverProperties properties){}
        public override void FeatureUninstalling(SPFeatureReceiverProperties properties){}      
        public override void FeatureDeactivating(SPFeatureReceiverProperties properties){}
   }
}
