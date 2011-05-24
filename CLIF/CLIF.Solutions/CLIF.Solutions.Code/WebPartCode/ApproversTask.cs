using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Web.UI;
using System.Web.UI.WebControls.WebParts;
using System.Data;
using Microsoft.SharePoint.WebControls;
using Microsoft.SharePoint;
using CLIF.Solutions.Code;
using System.Web.UI.WebControls;

namespace CLIF.Solutions.Code
{
    [Guid("7a6f18b1-c4ad-4597-9581-8d4a1a3dbe16")]
    public class ApproversTask : Microsoft.SharePoint.WebPartPages.WebPart
    {
        private bool _error = false;
        private DataTable tbTaskList;  
        private SPGridView gvTaskList;
        private DataView vwTaskList;
        MenuTemplate TaskListMenu;
        private string query = string.Empty;

        public ApproversTask()
        {
            this.ExportMode = WebPartExportMode.All;
        }
        /// <summary>
        /// This Function return a Public URL for the Current Site.
        /// </summary>
        /// <returns>string</returns>
        private string GetPublicUrl()
        {
            string publicUrl = string.Empty;
            string _message = string.Empty;
            using (SPSite ObjSite = new SPSite(SPContext.Current.Site.ID, SPContext.Current.Site.Zone))
            {
                publicUrl = ObjSite.MakeFullUrl(SPContext.Current.Site.ServerRelativeUrl);
            }
            return publicUrl;
        }
        /// <summary>
        /// Create all your controls here for rendering.
        /// Try to avoid using the RenderWebPart() method.
        /// </summary>
        protected override void CreateChildControls()
        {
            if (!_error)
            {
                try
                {
                    //Getting the Public URL to handle http & https url issues
                    string _publicURL = GetPublicUrl();
                    _publicURL = "";

                    base.CreateChildControls();

                    gvTaskList = new SPGridView();
                    gvTaskList.Width = Unit.Percentage(100);
                    gvTaskList.AutoGenerateColumns = false;
                    gvTaskList.BorderStyle = BorderStyle.Dotted;
                 
                    //Adding Title Field
                    SPMenuField nameMenu = new SPMenuField();
                    nameMenu.HeaderText = "Title";
                    nameMenu.TextFields = "Title";
                    nameMenu.MenuTemplateId = "TaskListMenu";
                    nameMenu.NavigateUrlFields = "Task Ref";
                    nameMenu.TokenNameAndValueFields = "Task Ref=Task Ref";

                    //nameMenu.NavigateUrlFormat = _publicURL + "/{0}";
                    nameMenu.SortExpression = "Due Date";

                    //Adding Due Date Field
                    BoundField dueDate = new BoundField();
                    dueDate.DataField = "Due Date";
                    dueDate.HeaderText = "Due Date";
                    dueDate.SortExpression = "Due Date";

                    //Adding Due Date Field
                    BoundField documentAuthor = new BoundField();
                    documentAuthor.DataField = "Document Author";
                    documentAuthor.HeaderText = "Document Author";
                    documentAuthor.SortExpression = "Document Author";

                    //Adding DropDown Menu for 'Evidence Log'
                    TaskListMenu = new MenuTemplate();
                    TaskListMenu.ID = "TaskListMenu";
                    MenuItemTemplate evidenceLogMenu = new MenuItemTemplate("Open", "/_layouts/images/actionseditpage.gif");
                    evidenceLogMenu.ClientOnClickNavigateUrl = "%Task Ref%"; 
  
                    //Adding Controls to Control Collection
                    TaskListMenu.Controls.Add(evidenceLogMenu);                   

                    this.Controls.Add(TaskListMenu);
                    gvTaskList.Columns.Add(nameMenu);
                    gvTaskList.Columns.Add(dueDate);
                    gvTaskList.Columns.Add(documentAuthor);
                    this.Controls.Add(gvTaskList);
                    gvTaskList.PagerTemplate = null;               
                }
                catch (Exception ex)
                {
                    HandleException(ex);
                }
            }
        }
        public void PopulateApproversTask()
        {
            // Create a new DataTable.
            tbTaskList = new DataTable("WebInfo");
            // Declare variables for DataColumn and DataRow objects.
            DataColumn column;
            DataRow row;
            // Create new DataColumn, set DataType,
            // ColumnName and add to DataTable.
            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "Title";
            column.ReadOnly = true;
            column.Unique = false;
            // Add the Column to the DataColumnCollection.
            tbTaskList.Columns.Add(column);

            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "Document Author";
            column.ReadOnly = true;
            column.Unique = false;
            // Add the Column to the DataColumnCollection.
            tbTaskList.Columns.Add(column);


            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "Task Ref";
            column.ReadOnly = true;
            column.Unique = false;
            // Add the Column to the DataColumnCollection.
            tbTaskList.Columns.Add(column);

            // Create new DataColumn, set DataType,
            // ColumnName and add to DataTable.
            column = new DataColumn();
            column.DataType = System.Type.GetType("System.DateTime");
            column.ColumnName = "Due Date";
            column.ReadOnly = true;
            column.Unique = false;
            // Add the Column to the DataColumnCollection.
            tbTaskList.Columns.Add(column);

            using (SPWeb ObjRootWeb = SPHelper.GetRootWeb(SPHelper.GetRootUrl(SPContext.Current.Site.Url)))
            {
                //Getting Areas List Ref.
                SPList ObjList = ObjRootWeb.Lists["Document Approval Tasks"];
                SPQuery ObjQuery = new SPQuery();                
                ObjQuery.Query = "<Where><And><Eq><FieldRef Name=\"Document_x0020_Approver\" /><Value Type=\"User\">Approvers</Value></Eq><Neq><FieldRef Name=\"Status\" /><Value Type=\"Text\">Complete</Value></Neq></And></Where>";
                SPListItemCollection _tasks = ObjList.GetItems(ObjQuery);
                tbTaskList.Rows.Clear();
                for (int i = 0; i < _tasks.Count; i++)
                {
                    row = tbTaskList.NewRow();
                    row["Title"] = _tasks[i].Title.ToString();
                    row["Document Author"] =_tasks[i]["Document Author"].ToString().Split('#')[1];
                    row["Due Date"] = _tasks[i]["Due Date"];
                    row["Task Ref"] = _tasks[i]["Task Ref"].ToString().Split(',')[0];
                    tbTaskList.Rows.Add(row);
                }

                //Adding Data to a DataView for Sorting feature
                vwTaskList = new DataView(tbTaskList);
            }
        }
        /// <summary>
        /// This Method Renders the Actual Content to the Page
        /// </summary>
        /// <param name="writer">HtmlTextWriter</param>
        protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
        {
            try
            {
                //Polulating Area List
                PopulateApproversTask();
                vwTaskList.Sort = "Due Date asc";
                gvTaskList.DataSource = vwTaskList;

                //Binding GridView 
                gvTaskList.DataBind();
                writer.Write("<table border=\"0\" width=\"100%\">");
                writer.Write("<tr><td>");
                gvTaskList.RenderControl(writer);
                writer.Write("</td></tr>");
                writer.Write("</table>");
                TaskListMenu.RenderControl(writer);
            }
            catch (Exception x)
            {
                writer.Write("<p>" + x.Message + "</p>");
            }
        }
        /// <summary>
        /// Ensures that the CreateChildControls() is called before events.
        /// Use CreateChildControls() to create your controls.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            if (!_error)
            {
                try
                {
                    base.OnLoad(e);
                    this.EnsureChildControls();

                    // Your code here...
                }
                catch (Exception ex)
                {
                    HandleException(ex);
                }
            }
        }

        /// <summary>
        /// Clear all child controls and add an error message for display.
        /// </summary>
        /// <param name="ex"></param>
        private void HandleException(Exception ex)
        {
            this._error = true;
            this.Controls.Clear();
            this.Controls.Add(new LiteralControl(ex.Message));
        }
    }
}
