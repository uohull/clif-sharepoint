using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.SharePoint;
using System.Xml;
using System.Collections;
using System.Data;

namespace CLIF.Solutions.Code
{
    class Lists : SPFeatureReceiver
    {
        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            string RootPath = properties.Feature.Definition.RootDirectory;
         
            // feature is scoped at Site, so the parent is type SPSite rather than SPWeb..
            using (SPSite site = properties.Feature.Parent as SPSite)
            {
                SPWeb currentWeb = null;
                Guid gRootWebId = Guid.Empty;
                if (site != null)
                {
                    currentWeb = site.RootWeb;
                    gRootWebId = currentWeb.ID;
                }
                else
                {
                    currentWeb = properties.Feature.Parent as SPWeb;
                    gRootWebId = currentWeb.Site.RootWeb.ID;
                }

                using (currentWeb)
                {
                    string dataFile = RootPath + "\\" + properties.Feature.Properties["Data"].Value;
                    AddListData(currentWeb, dataFile);
                }
            }
        }
        private void AddListData(SPWeb web, string dataFileName)
        {
            XmlDocument dataDoc = new XmlDocument();
            dataDoc.Load(dataFileName);

            // process each list element
            foreach (XmlElement listElem in dataDoc.SelectNodes("Lists/List"))
            {
                string listName = listElem.Attributes["Name"].Value;
                SPList list = web.Lists[listName];


                //Clear all Existing Data
                ArrayList alExistingData = new ArrayList();
                foreach (SPItem item in list.Items)
                {
                    alExistingData.Add(item.ID);
                }
                foreach (int Id in alExistingData)
                {
                    list.Items.DeleteItemById(Id);
                }

                DataSet listData = new DataSet();
                bool hasItems = false;

                if (list.Items.Count > 0)
                {
                    hasItems = true;
                    listData.Tables.Add(list.Items.GetDataTable());
                }

                // process each row of data
                foreach (XmlElement dataRowElem in listElem.SelectNodes("Data/Rows/Row"))
                {
                    // find the row title so we can check if it exists already
                    string title = dataRowElem.SelectSingleNode("Field[@Name='Title']").InnerText;

                    if ((!hasItems) || listData.Tables[0].Select("Title='" + title + "'").Length == 0)
                    {
                        // row not already there so create it
                        SPListItem newItem = list.Items.Add();
                        newItem["Title"] = title;

                        // add remaining fields if any
                        foreach (XmlElement fieldElem in dataRowElem.SelectNodes("Field[@Name!='Title']"))
                        {
                            string fieldName = fieldElem.Attributes["Name"].Value;
                            string fieldVal = fieldElem.InnerText;
                            newItem[fieldName] = fieldVal;
                        }
                        newItem.Update();
                    }
                }
            }
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
