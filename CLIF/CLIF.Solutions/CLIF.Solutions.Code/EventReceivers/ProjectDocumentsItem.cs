using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.SharePoint;

namespace CLIF.Solutions.Code
{
    class ProjectDocumentsItemEventReceiver : SPItemEventReceiver
    {
        public override void ItemCheckedIn(SPItemEventProperties properties)
        {           
            this.DisableEventFiring();
            try
            {
                //properties.ListItem["Publishable Status"] = "Draft";
                //properties.ListItem.Update();
            }
            catch (Exception ex)
            {
                properties.Cancel = true;
                properties.ErrorMessage = "Unable attach metadata to this item " + ex.Message;
            }
            finally
            {
                this.EnableEventFiring();
                base.ItemCheckedIn(properties);
            }
        }
        public override void ItemDeleting(SPItemEventProperties properties)
        {
            if (IsWorkflowNotRunning(properties)==false)
            {
                properties.Status = SPEventReceiverStatus.CancelWithError;
                properties.Cancel = true;
                properties.ErrorMessage = "CLIF Error - Cannot delete this item. Sorry, document approval request for publication already submitted.";
            }
            else
            {
                base.ItemDeleting(properties);
            }
        }
        public override void ItemAdded(SPItemEventProperties properties)
        {
            this.DisableEventFiring();
            try
            {
                string _mimeType = GetMimeType(properties.ListItem.File.Name);
                if (_mimeType != null)
                {
                    this.DisableEventFiring();
                    properties.ListItem["Content MimeType"] = _mimeType;
                    properties.ListItem["Document Author(s)"] = properties.ListItem.File.Properties["vti_author"];
                    properties.ListItem["Publishable Status"] = "Draft";

                    SPFieldUrlValue value = new SPFieldUrlValue();
                    value.Description = "Private";
                    value.Url = SPHelper.GetRootUrl(SPContext.Current.Web.Url) + "_layouts/IMAGES/bizdataactionicon.gif";
                    properties.ListItem["Access Level"] = value;
                    properties.ListItem.Update();
                }
            }
            catch (Exception ex)
            {
                properties.Cancel = true;
                properties.ErrorMessage = "Unable attach metadata to this item " + ex.Message;
            }
            finally
            {
                this.EnableEventFiring();
                base.ItemAdded(properties);
            }
        }
        public override void ItemCheckingOut(SPItemEventProperties properties)
        {
            if (IsWorkflowNotRunning(properties))
            {
                properties.Status = SPEventReceiverStatus.CancelWithError;
                properties.Cancel = true;
                properties.ErrorMessage = "CLIF Error - Checkout failed <br>Sorry, document approval request for publication already submitted.";
            }
            else
            {
                base.ItemCheckingOut(properties);
            }
        }
        public override void ItemUpdating(SPItemEventProperties properties)
        {
            if (IsWorkflowNotRunning(properties) == false)
            {
                properties.Status = SPEventReceiverStatus.CancelWithError;
                properties.Cancel = true;
                properties.ErrorMessage = "CLIF Error - Update failed. Sorry, document approval request for publication already submitted.";
            }
            else
            {
                base.ItemUpdating(properties);            
            }
        }
        public override void ItemUpdated(SPItemEventProperties properties)
        {
            if (IsWorkflowNotRunning(properties))
            {
                this.DisableEventFiring();
                try
                {
                    string _mimeType = GetMimeType(properties.ListItem.File.Name);
                    if (_mimeType != null)
                    {
                        this.DisableEventFiring();
                        //properties.ListItem["Publishable Status"] = "Draft";
                        properties.ListItem["Content MimeType"] = _mimeType;
                        properties.ListItem["Document Author(s)"] = properties.ListItem.File.Properties["vti_author"];
                        properties.ListItem.SystemUpdate();
                        this.EnableEventFiring();
                        base.ItemUpdated(properties);
                    }
                }
                catch (Exception ex)
                {
                    properties.Cancel = true;
                    properties.ErrorMessage = "Unable attach metadata to this item " + ex.Message;
                }
                finally
                {
                    this.EnableEventFiring();                   
                }
            }
            else
            {
                properties.Status = SPEventReceiverStatus.CancelWithError;
                properties.Cancel = true;
                properties.ErrorMessage = "CLIF Error - Update failed. Sorry, document approval request for publication already submitted.";                
           
            }
        } 
        /// <summary>
        /// This merthod returns MIME Type for a file
        /// </summary>
        /// <param name="fileName">string</param>
        /// <returns>string</returns>
        private string GetMimeType(string fileName)
        {
            string mimeType = "application/unknown";
            string ext = System.IO.Path.GetExtension(fileName).ToLower();
            Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);
            if (regKey != null && regKey.GetValue("Content Type") != null)
            {
                mimeType = regKey.GetValue("Content Type").ToString();
            }
            return mimeType;
        }
        /// <summary>
        /// This method checks if workflow in runnning for the item.
        /// </summary>
        /// <returns></returns>
        private bool IsWorkflowNotRunning(SPItemEventProperties properties)
        {
            bool _isNotRunning=true;
            if (properties.ListItem.Workflows.Count != 0)            
            {
                if (properties.ListItem.Workflows[properties.ListItem.Workflows.Count - 1].InternalState == Microsoft.SharePoint.Workflow.SPWorkflowState.Running)
                {
                    _isNotRunning = false;
                }
            }
            return _isNotRunning;
        }
    }
}
