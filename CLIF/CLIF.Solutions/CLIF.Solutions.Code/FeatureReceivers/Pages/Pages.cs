using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.SharePoint;
using System.Xml;
using System.Collections;
using System.Data;
using Microsoft.SharePoint.Workflow;
using Microsoft.SharePoint.Publishing;

namespace CLIF.Solutions.Code
{
    class Pages : SPFeatureReceiver
    {
        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            SPSite site = (SPSite)properties.Feature.Parent;
            SPWeb web = site.OpenWeb();

            PublishingPage pubPage = AddPageToWeb(web, "searchresults.aspx", "searchresult.aspx");          
            pubPage.CheckIn("CLIF - Search page.");
            pubPage.ListItem.File.Publish("CLIF search page published.");         
        }
        public PublishingPage AddPageToWeb(SPWeb web,string pageFileName,string pageLayoutName)
        {
            PublishingWeb publishingWeb = PublishingWeb.GetPublishingWeb(web);
            PageLayout[] layouts = publishingWeb.GetAvailablePageLayouts();
            PageLayout layout = null;
            for (int i = 0; i < layouts.Length; ++i)
            {
                if (layouts[i].Name.Equals(pageLayoutName, StringComparison.OrdinalIgnoreCase))
                {
                    layout = layouts[i];
                    break;
                }
            }
            if (null == layout)
            {
                throw new ApplicationException(String.Format("Cannot find page layout named '{0}'.",pageLayoutName));
            }           
            PublishingPage page = publishingWeb.GetPublishingPages().Add(pageFileName, layout);
            return page;
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
