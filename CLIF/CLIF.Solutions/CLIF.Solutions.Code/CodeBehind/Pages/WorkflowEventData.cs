using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;

namespace CLIF.Solutions.Code
{
    /// <summary>
    /// Provides event data to a workflows in the format expected by workflows created in SharePoint Designer 2007.
    /// </summary>
    /// <remarks>
    /// Example usage:
    /// <code lang="c#">
    /// // Package up the workflow event data
    /// WorkflowEventData eventData = new WorkflowEventData();
    /// eventData.Add("Param1", "Value1");
    /// eventData.Add("Param2", "Value2");
    ///
    /// // Start the workflow for an item/association pair and provide the event data
    /// workflowManager.StartWorkflow(item, workflowAssoc, eventData.ToString());
    /// </code>
    /// </remarks>
    public class WorkflowEventData : NameValueCollection
    {
        public WorkflowEventData()
        {
        }

        /// <summary>
        /// Outputs the name/value collection in the format expected by SharePoint Designer workflows.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            // The output looks like:
            // <Data>
            //    <param1>value1</param1>
            //    <param2>value2</param2>
            // </Data>
            StringBuilder sb = new StringBuilder();
            sb.Append("<Data>");
            foreach (string key in this.Keys)
            {
                sb.Append(String.Format("<{0}>{1}</{2}>", key, this[key], key));
            }
            sb.Append("</Data>");
            return sb.ToString();
        }
    }

}
