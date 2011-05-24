using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebPartPages;
using System.Diagnostics;
using uk.ac.hull.repository.hydranet.service;

namespace CLIF.Solutions.Code
{
    /**********************************************************************************
     Title:              MySite
     Author:             Suresh Thampi
     Project:            CLIF.Solutions
     Date:               04/11/2010
     Copyright:          © Center for e-research(CeRch), King's College London 2010 All Rights Reserved.
     Description:     
                            
     ------------------------------------------------------------------------------
     Revision History
     Date             Ref       Author          Reason          Remarks        

     ***********************************************************************************/
    public class MySite : SPFeatureReceiver
    {
        private const string MASTERPAGEFILE = "MasterPageFile";

		public override void FeatureActivated(SPFeatureReceiverProperties properties)
		{
            char[] slashes = { '/' };
            string _siteName = string.Empty;
            string _masterPagePath = string.Empty;
            try
            {
                
                SPSite ObjSite = properties.Feature.Parent as SPSite;

                //Getting the master page file path.
                SPFeatureProperty masterFile = properties.Definition.Properties[MASTERPAGEFILE];

                //Getting Site Ref
                SPWeb ObjWeb =ObjSite.OpenWeb();
                _siteName = ObjWeb.Title;
                _masterPagePath = ObjWeb.ServerRelativeUrl.TrimEnd(slashes) + "/_catalogs/masterpage/" + masterFile.Value;
                
                //Updating master pages for all sites;
                ObjWeb.CustomMasterUrl = _masterPagePath;
                ObjWeb.MasterUrl = _masterPagePath;

                ObjWeb.ApplyTheme("simple");
                ObjWeb.Update();
                
                //Update MySite Repositry Settings List in the Root site
                UpdateRepositoryListSettings(_siteName,ObjWeb.Url,"System Account");
            }
            catch (Exception ex)
            {
                //UpdateLog(ex.Message, EventLogEntryType.Error);
                throw ex;
            }
		}
        /// <summary>
        /// This method added messages to the event log.
        /// </summary>
        /// <param name="Message">string</param>
        /// <param name="msgType">EventLogEntryType</param>
		private void UpdateLog(string Message, EventLogEntryType msgType)
		{
			try
			{
				System.Diagnostics.EventLog.WriteEntry("CLIF", Message, msgType);
			}
			catch
			{
                //ignore
			}
		}

       private void RenameDefaultLibraries(SPWeb MySite)
        {
            try
            {
                //Personal Documents
                SPList ObjPersonalDocumentsList = MySite.Lists["Personal Documents"];
                ObjPersonalDocumentsList.Title = "Private Project Documents";
                ObjPersonalDocumentsList.Update();

                //Shared Documents
                SPList ObjSharedDocumentsList = MySite.Lists["Shared Documents"];
                ObjSharedDocumentsList.Title = "Shared Project Documents";
                ObjSharedDocumentsList.Update();
            }
            catch { }

        }
       
       private void UpdateRepositoryListSettings(string SiteName,string SiteUrl,string CurrentUser)
        {
            string _formatedSiteName = SiteName.Replace(@"\", "-");;
            _formatedSiteName = _formatedSiteName.Replace(" ", "-");

            string _defaultRootObject = "CLIF:" + _formatedSiteName;
            string _labelFormat = "{objectname}";
            string _namespaceFormat = "CLIF-" + _formatedSiteName + "-{listname}";
            string _pidFormat = "CLIF:" + _formatedSiteName + "-{listname}:{n}";


            //Create CLIF Root Folder 
            HydraServiceFedoraImpl ObjService = new HydraServiceFedoraImpl();
            //if (ObjService.GetObjectHydra("CLIF:Root") == null)
            //{
            //    ObjService.ObjectPID = "CLIF:Root";
            //    ObjService.DepositSingletonSet("CLIF:Root", "CLIF:Root", null);
            //}

            //Create Site Root Folder for the current Site in Fedora
            ObjService = null;
            ObjService = new HydraServiceFedoraImpl();
            ObjService.ObjectPID = _defaultRootObject;
            ObjService.DepositSingletonSet(_defaultRootObject, "CLIF:" + _formatedSiteName,"CLIF:Root");

            ObjService = null;
            ObjService = new HydraServiceFedoraImpl();
            ObjService.DepositSingletonSet("CLIF-" + _formatedSiteName, "_private", _defaultRootObject);

            ObjService = null;
            ObjService = new HydraServiceFedoraImpl();
            string _archivePID=ObjService.DepositSingletonSet("CLIF-" + _formatedSiteName, "_archive", _defaultRootObject);
           
            using (SPSite ObjSite = new SPSite(SPHelper.GetRootUrl(SiteUrl)))
            {
                using (SPWeb ObjWeb = ObjSite.OpenWeb())
                {
                    SPList ObjList = ObjWeb.Lists["MySite Repository Settings"];
                    SPQuery ObjQuery = new SPQuery();
                    string _query = string.Format("<Where><Eq><FieldRef Name=\"Title\" /><Value Type=\"Text\">{0}</Value></Eq></Where>", SiteName);
                    ObjQuery.Query = _query;

                    //Getting list item collection
                    SPListItemCollection items = ObjList.GetItems(ObjQuery);
                    if (items.Count == 0)
                    {
                        SPItem _newItem = ObjList.Items.Add();
                        _newItem["Site Name"] = SiteName;
                        _newItem["Default Root Object"] = _defaultRootObject;
                        _newItem["Label Format"] = _labelFormat;
                        _newItem["Namespace Format"] = _namespaceFormat;
                        _newItem["PID Format"] = _pidFormat;
                        _newItem["Archive PID"] = _archivePID;
                        _newItem.Update();
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
			//throw new Exception("The method or operation is not implemented.");
		}

		public override void FeatureUninstalling(SPFeatureReceiverProperties properties)
		{
			//throw new Exception("The method or operation is not implemented.");
		}
	}
}
