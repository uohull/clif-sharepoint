using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.SharePoint;
using System.Xml;
using System.Collections;
using System.Data;
using Microsoft.SharePoint.Workflow;

namespace CLIF.Solutions.Code
{
    class ProjectDocuments : SPFeatureReceiver
    {
        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
           // feature is scoped at Site, so the parent is type SPSite rather than SPWeb..
            SPSite site = properties.Feature.Parent as SPSite;            
            SPWeb _ObjWeb = site.OpenWeb();

            //Remove all permission to make it private document library
            SPHelper.RemoveAllListPermissions(_ObjWeb, "Project Documents");

            //Remove all permission to make it Archive document library
            SPHelper.RemoveAllListPermissions(_ObjWeb, "Archive");

            //Remove all permission to make it Archive document library
            SPHelper.RemoveAllListPermissions(_ObjWeb, "Tasks");
                     
        }
        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            //throw new Exception("The method or operation is not implemented.");
        }

        public override void FeatureInstalled(SPFeatureReceiverProperties properties)
        {
        }

        public override void FeatureUninstalling(SPFeatureReceiverProperties properties)
        {
        }
    }
}
