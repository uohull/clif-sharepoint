using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Microsoft.SharePoint;
using Microsoft.Win32;
using uk.ac.hull.repository.hydranet.service;
using System.IO;
using System.Xml.Linq;
using uk.ac.hull.repository.hydranet.hydracontent.metadata;
using System.Xml.XPath;
using Common.PolicyManagement;
using Common.Web.Utils;
using System.Xml;
using uk.ac.hull.repository.utils;

namespace CLIF.Solutions.Code
{
    /**********************************************************************************
     Title:              SPHelper
     Author:             Suresh Thampi
     Project:            CLIF.Solutions.Code
     Date:               04/11/2010
     Copyright:          © Center for e-research(CeRch), King's College London 2010 All Rights Reserved.
     Description:     
                                
     ------------------------------------------------------------------------------
     Revision History
     Date             Ref       Author          Reason          Remarks        

     ***********************************************************************************/
    public static class SPHelper
    {
 
        /// <summary>
        /// This method returns the root url
        /// </summary>
        /// <param name="CurrentUrl">string</param>
        /// <returns>string</returns>
        public static string GetRootUrl(string CurrentUrl)
        {
            string _rootUrl = string.Empty;
            string[] _urlParts = CurrentUrl.Split('/');
            int _count = 0;
            foreach (string part in _urlParts)
            {
                _rootUrl += part + "/";
                if (_count == 2)
                {
                    break;
                }
                _count++;
            }
            return _rootUrl;
        }
        /// <summary>
        /// This method returns file extension from mimetype
        /// </summary>
        /// <param name="mimeType"></param>
        /// <returns></returns>
        public static string GetFileExtension(string mimeType)
        {
            string result;
            RegistryKey key;
            object value;
            key = Registry.ClassesRoot.OpenSubKey(@"MIME\Database\Content Type\" + mimeType, false);
            value = key != null ? key.GetValue("Extension", null) : null;
            result = value != null ? value.ToString() : string.Empty;
            return result;
        }
        /// <summary>
        /// Set specific list permissions on a site list or document library
        /// </summary>
        /// <param name="site">Site that contains the list or document library you want to set specific permissions for</param>
        /// <param name="listName">Name of the list that you want to set specific permissions on (e.g. "Commercial Documents")</param>
        /// <param name="roleNames">Names of the roles that you want to grant permissions to</param>
        public static void SetListPermissions(SPWeb site, string listName, List<string> roleNames)
        {
            SPList siteList = site.Lists[listName];
            siteList.BreakRoleInheritance(true);
            // Remove any roles that are not in list 
            for (int i = siteList.RoleAssignments.Count - 1; i >= 0; i--)
            {
                if (roleNames.IndexOf(siteList.RoleAssignments[i].Member.Name) == -1)
                {
                    siteList.RoleAssignments.Remove(i);
                }
            }
            siteList.Update();
        }
        /// <summary>
        /// This method assigns user or group with read permission on the current item/file.
        /// </summary>
        /// <param name="Item">SPListItem</param>
        /// <param name="Approver">string</param>
        /// <param name="Web">SPWeb</param>
        public static void AssignApproverReadPermission(SPListItem Item,SPUser Approver,SPWeb Web)
        {           
            bool _roleAssignmentExist = false;

            //Checking if role already exit for the current user or group.
            foreach(SPRoleAssignment rs in Item.RoleAssignments)
            {
                if (rs.Member == Approver)
                {
                    _roleAssignmentExist = true;
                }
            }
            //Add user or group role assignment.
            if (_roleAssignmentExist == false)
            {
               SPSecurity.RunWithElevatedPrivileges(delegate()
               {
                SPRoleAssignment roleAssignment = new SPRoleAssignment(Approver.LoginName, Approver.Email, Approver.Name, "Approver's file access permission.");
                SPRoleDefinition roleDefinition = Web.RoleDefinitions.GetByType(SPRoleType.Administrator);
                roleAssignment.RoleDefinitionBindings.Add(roleDefinition);
                if (!Item.HasUniqueRoleAssignments)
                {
                    // Ensure we don't inherit permissions from parent
                    Item.BreakRoleInheritance(true);
                }
                Item.Web.AllowUnsafeUpdates = true;
                Item.RoleAssignments.Add(roleAssignment);
                Item.SystemUpdate(false);
                Item.Web.AllowUnsafeUpdates = false;
               });
            }
        }
        public static void AssignPermission(SPListItem Item, string LoginName, string UserEmail, string UserNote, string UserName, SPWeb Web)
        {
            Web.AllowUnsafeUpdates = true;

            //Remove all existing RoleAssignments
            for (int i = Item.RoleAssignments.Count - 1; i >= 0; i--)
            {
                Item.RoleAssignments.Remove(Item.RoleAssignments[i].Member);
            }
            SPRoleAssignment roleAssignment = new SPRoleAssignment(LoginName, UserEmail, UserName, UserNote);
            SPRoleDefinition roleDefinition = Web.RoleDefinitions.GetByType(SPRoleType.Administrator);
            roleAssignment.RoleDefinitionBindings.Add(roleDefinition);
            if (!Item.HasUniqueRoleAssignments)
            {
                // Ensure we don't inherit permissions from parent
                Item.BreakRoleInheritance(true);
            }
            Item.RoleAssignments.Add(roleAssignment);
            Item.SystemUpdate(false);
            Web.Update();
            Web.AllowUnsafeUpdates = false;
        }
        public static void RemoveAllListPermissions(SPWeb site, string listName)
        {
            SPList siteList = site.Lists[listName];
            siteList.BreakRoleInheritance(true);
            // Remove any roles that are not in list 
            for (int i = siteList.RoleAssignments.Count - 1; i >= 0; i--)
            {
                siteList.RoleAssignments.Remove(i);
            }
            siteList.Update();
        }
        /// <summary>
        /// This method submit file to the repository
        /// </summary>
        /// <param name="Web">SPWeb</param>
        /// <param name="Item">SPListItem</param>
        /// <param name="TargetPID">TargetPID</param>
        /// <returns></returns>
        public static string SubmitItemToRepository(SPWeb Web, SPListItem Item, string TargetPID)
        {
            string _ObjectPID = string.Empty;
            SPListItem ObjItem = Item;
            //Getting the namespace format 
            string Namespace = string.Empty;
            using (MySiteRepositorySettings ObjSettings = new MySiteRepositorySettings())
            {
                SPListItem settings = ObjSettings.GetRepositorySettings(Web.Title,Web.Url);
                if (settings != null)
                {
                    Namespace = settings["Namespace Format"].ToString().Replace("{listname}", Item.ParentList.Title.Replace(" ", ""));
                }
            }

            //Add file to the repository --------------------------------------------------------------------------------
            SPFile file = ObjItem.File;
            if (file != null)
            {
/*
                //MEtabasepath
                string METABASEPATH = "IIS://" + Web.Site.HostName + "/MimeMap";
                //URL
                string url = Web.Url + '/' + file.Url;
                //MimeType
 */
                string mimeType = MimeTypeHelper.GetContentType(file.Name);//MimeType.GetMimeTypeForExtension(METABASEPATH, Path.GetExtension(file.Name));
                //Namespace
                string nameSpace = Web.Title + "-" + ObjItem.ParentList.Title;
                //Item Title
                string itemTitle = ObjItem.Name.Trim();
                //Skip Virus Scan
                byte[] content = file.OpenBinary(SPOpenBinaryOptions.SkipVirusScan);
                string author = "Unknown author";
                if ((file.SourceFile != null) && !String.IsNullOrEmpty(file.SourceFile.Name))
                {
                    author = file.SourceFile.Name.Trim();
                }
                else if ((file.Author != null) && !String.IsNullOrEmpty(file.Author.Name))
                {
                    author = file.Author.Name.Trim();
                }

                HydraServiceFedoraExt hydraService = new HydraServiceFedoraExt();
                _ObjectPID = hydraService.DepositSimpleContentObject(Namespace, itemTitle, TargetPID, content, mimeType, author, author);

                //PolicyDS policyDS = new PolicyDS();
                //policyDS.Xml = GetPolicyDS(hydraService.Pid,author);
                //hydraService.AppendMetaData("POLICY", "XACML Policy Datastream", policyDS);


                //Updating operation status for the exported list item
                UpdateSourceItem(ObjItem, Web, _ObjectPID);
                
                //Adding an entry in Published Documents Log
                UpdatePublishedDocumentsLog(Web, _ObjectPID, ObjItem);
            }
            return _ObjectPID;
        }

        private static void UpdatePublishedDocumentsLog(SPWeb Web, string _ObjectPID, SPListItem ObjItem)
        {
            using (DisabledItemEventsScope scope = new DisabledItemEventsScope())
            {
                Web.AllowUnsafeUpdates = true;
                SPList list = Web.Lists["Published Documents"];
                SPListItem newItem = list.Items.Add();
                newItem["Title"] = ObjItem.Title;
                newItem["Published Date"] = DateTime.Now;
                newItem["Persistent ID"] = _ObjectPID;
                newItem["Content MimeType"] = ObjItem["Content MimeType"].ToString();
                newItem.Update();
                Web.Update();
                Web.AllowUnsafeUpdates = false;
            }
        }
        /// <summary>
        /// This method adds files to the repository 
        /// </summary>
        /// <param name="Item">SPListItem</param>
        /// <param name="TargetPID">string</param>
        public static string AddItemToRepository(SPWeb Web, SPListItem Item, string TargetPID)
        {
            string _ObjectPID = string.Empty;
            SPListItem ObjItem = Item;
            //Getting the namespace format 
            string Namespace = string.Empty;
            using (MySiteRepositorySettings ObjSettings = new MySiteRepositorySettings())
            {
                SPListItem settings = ObjSettings.GetRepositorySettings(SPContext.Current.Web.Title);
                if (settings != null)
                {
                    Namespace = settings["Namespace Format"].ToString().Replace("{listname}",Item.ParentList.Title.Replace(" ", ""));
                }
            }

            //Add file to the repository --------------------------------------------------------------------------------
            SPFile file = ObjItem.File;
            if (file != null)
            {                

                //MEtabasepath
                //string METABASEPATH = "IIS://" + Web.Site.HostName + "/MimeMap";
                //URL
                //string url = Web.Url + '/' + file.Url;
                //MimeType
                string mimeType = MimeTypeHelper.GetContentType(file.Name);//MimeType.GetMimeTypeForExtension(METABASEPATH, Path.GetExtension(file.Name));
                //Namespace
                string nameSpace = Web.Title + "-" + ObjItem.ParentList.Title;
                //Item Title
                string itemTitle = ObjItem.Name.Trim();
                //Skip Virus Scan
                byte[] content = file.OpenBinary(SPOpenBinaryOptions.SkipVirusScan);
                string author = "Unknown author";
                if ((file.SourceFile != null) && !String.IsNullOrEmpty(file.SourceFile.Name))
                {
                    author = file.SourceFile.Name.Trim();
                }
                else if ((file.Author != null) && !String.IsNullOrEmpty(file.Author.Name))
                {
                    author = file.Author.Name.Trim();
                }

                HydraServiceFedoraExt hydraService = new HydraServiceFedoraExt();   
                _ObjectPID = hydraService.DepositSimpleContentObject(Namespace, itemTitle, TargetPID, content, mimeType, author, author);

                //PolicyDS policyDS = new PolicyDS();
                //policyDS.Xml = GetPolicyDS(hydraService.Pid,author);
                //hydraService.AppendMetaData("POLICY", "XACML Policy Datastream", policyDS);


                //Updating operation status for the exported list item
                UpdateSourceItem(ObjItem, Web, _ObjectPID);

                //Adding an entry in Published Documents Log
                UpdatePublishedDocumentsLog(Web, _ObjectPID, ObjItem);
            }
            return _ObjectPID;
        }
        private static string GetPolicyDS(string pid,string UserName)
        {
            ObjectPolicy policy = new ObjectPolicy(pid, true, UserName);

            return policy.Xml;
        }
        /// <summary>
        /// This method updates List item in the document library
        /// </summary>
        /// <param name="Item">SPListItem</param>
        /// <param name="Web">SPWeb</param>
        private static void UpdateSourceItem(SPListItem ObjItem, SPWeb Web, string PersistentId)
        {
            using (DisabledItemEventsScope scope = new DisabledItemEventsScope())
            {
                Web.AllowUnsafeUpdates = true;
                ObjItem["Persistent ID"] = PersistentId;
                ObjItem["Publishable Status"] = "Copied to repository";
                ObjItem.Update();
                Web.Update();
                Web.AllowUnsafeUpdates = false;
            }
        }
        /// <summary>
        /// This method marks the current Item as shared
        /// </summary>
        /// <param name="site">SPWeb</param>
        /// <param name="item">SPListItem</param>
        /// <param name="Shared">bool</param>
        public static void ShareItem(SPWeb site, SPListItem item, bool Shared)
        {
            item.BreakRoleInheritance(true);
            site.AllowUnsafeUpdates = true;
            SPFieldUrlValue value = new SPFieldUrlValue();
            //Remove all existing RoleAssignments
            for (int i = item.RoleAssignments.Count - 1; i >= 0; i--)
            {
                item.RoleAssignments.Remove(item.RoleAssignments[i].Member);
            }
            if (Shared == true)
            {
                SPList siteList = site.Lists["Shared Documents"];
                for (int i = siteList.RoleAssignments.Count - 1; i >= 0; i--)
                {
                    item.RoleAssignments.Add(siteList.RoleAssignments[i]);
                }
                value.Description = "Shared";
                value.Url = SPHelper.GetRootUrl(SPContext.Current.Web.Url) + "_layouts/IMAGES/sharedoc.gif";
            }
            else
            {
                value.Description = "Private";
                value.Url = SPHelper.GetRootUrl(SPContext.Current.Web.Url) + "_layouts/IMAGES/bizdataactionicon.gif";
            }
            item["Access Level"] = value;
            item.Update();
            site.AllowUnsafeUpdates = false;
            site.Update();
        }
        /// <summary>
        /// This method returns GUID for the root site
        /// </summary>
        /// <param name="RootUrl">string</param>
        /// <returns>Guid</returns>
        public static Guid GetRootWebId(string RootUrl)
        {
            using (SPSite ObjSite = new SPSite(RootUrl))
            {
                return ObjSite.OpenWeb().ID;
            }
        }
        /// <summary>
        /// This method returns SPWeb for the root site
        /// </summary>
        /// <param name="RootUrl">string</param>
        /// <returns>SPWeb</returns>
        public static SPWeb GetRootWeb(string RootUrl)
        {
            using (SPSite ObjSite = new SPSite(RootUrl))
            {
                return ObjSite.OpenWeb();
            }
        }
        /// <summary>
        /// This method returns SPSite for the root site
        /// </summary>
        /// <param name="RootUrl">string</param>
        /// <returns>SPSite</returns>
        public static SPSite GetRootSite()
        {
           return new SPSite(GetRootUrl(SPContext.Current.Site.Url));
        }
        /// <summary>
        /// This method return the root web Id of the landing Site
        /// </summary>
        /// <returns>Guid</returns>
        public static Guid GetLandingWebId()
        {
            using (SPSite ObjSite = new SPSite(SPHelper.GetRootUrl(SPContext.Current.Site.Url)))
            {
                return ObjSite.RootWeb.ID;
            }
        }
        /// <summary>
        /// This retuns root web object of the landing site.
        /// </summary>
        /// <returns>SPWeb</returns>
        public static SPWeb GetLandingWeb()
        {
            using (SPSite ObjSite = new SPSite(SPHelper.GetRootUrl(SPContext.Current.Site.Url)))
            {
                return ObjSite.RootWeb;
            }
        }
        /// <summary>
        /// This method set value for SPFieldUser
        /// </summary>
        /// <param name="item"></param>
        /// <param name="fieldName"></param>
        /// <param name="loginName"></param>
        public static void SetFieldValueUser(SPListItem item, string fieldName, string loginName)
        {
            if (item != null)
            {   
                item[fieldName] =item.Web.EnsureUser(loginName);                
            }
        }
        /// <summary>
        /// This Function Logs information/Error in the Event Log
        /// </summary>
        /// <param name="eType">EventType</param>
        /// <param name="Message">string</param>
        public static void WriteToEventLog(string Type, string Message)
        {
            EventLog ObjEventLog = new EventLog();
            if (!System.Diagnostics.EventLog.SourceExists("CLIF"))
            {
                System.Diagnostics.EventLog.CreateEventSource("CLIF", "Application");
            }
            ObjEventLog.Source = "CLIF";

            if (Type.ToLower() == "error")
            {
                ObjEventLog.WriteEntry(Message, EventLogEntryType.Error);
            }
            else
            {
                ObjEventLog.WriteEntry(Message, EventLogEntryType.Information);
            }
        }

        public static string GetFormatedAreaName(string AreaName)
        {
            return AreaName.Replace('-', '_');
        }

        public static string GetOriginalAreaName(string AreaName)
        {
            return AreaName.Replace('_', '-');
        }
    }
}
