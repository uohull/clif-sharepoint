using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Workflow;
using Microsoft.SharePoint.ApplicationPages;

namespace CLIF.Solutions.Code
{
    /// <summary>
    /// Custom Workflow Status page to handle workflow cancellation event.
    /// WrkStat.aspx page in \TEMPLATE\LAYOUTS folder must inherit from this class
    /// so first two lines of the page should look similar to this: 
    /// <%@ Assembly Name="MyCustomWorkflow, Version=1.0.0.0, Culture=neutral, PublicKeyToken=1234526e108e148a"%>
    /// <%@ Page Language="C#" Inherits="MyCustomWorkflow.MyWrkStatPage" MasterPageFile="~/_layouts/application.master"%>
    /// </summary>
    public class WorkflowStatus : WrkStatPage
    {        
        private const string EVENT_TARGET = "__EVENTTARGET";
        /// <summary>
        /// Overrides base class event and adds custom code to remove items from
        /// My Submitted Documents list.
        /// Note. The protected OnClick_HtmlAnchorEnd() method is not virtual thus it won’t get called
        /// by the framework even if overriden.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            // Detect cancellation event
            if (Request.Form[EVENT_TARGET] != null && Request.Form[EVENT_TARGET].Contains("HtmlAnchorEnd"))
            {                
                Guid guid = new Guid(base.StrGuidWorkflow);
                SPWorkflow workflow = new SPWorkflow(base.Web, guid);
                using (DisabledItemEventsScope scope = new DisabledItemEventsScope())
                {
                    SPList ObjList = SPContext.Current.Web.Lists[new Guid(Request.QueryString["List"].ToString())];
                    SPListItem ObjItem = ObjList.Items.GetItemById(workflow.ItemId);
                    ObjItem["Publishable Status"] = "";
                    ObjItem.Update();
                }
            }
        }
    }
}
