using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.SharePoint.WebControls;
using System.Data;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using System.Globalization;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using System.Web;
using System.Collections;

namespace CLIF.Solutions.Code
{
    /*
     *************************************************************************************
     Title:              LookupFieldWithPickerControlMulti Class       
     Author:             Suresh Thampi
     Project:            CAA JAS Version 1.2
     Date:               19/06/2009
     Copyright:          © Audit Commission 2009 All Rights Reserved.
     Description:        
     ------------------------------------------------------------------------------
     Revision History
     Date             Ref       Author          Reason          Remarks        

     ************************************************************************************
     */

    public class LookupFieldWithPickerQuery : SimpleQueryControl, ICallbackEventHandler
    {
        private LookupFieldWithPickerPropertyBag propertyBag = null;
        string searchField = null;
        string searchOperator = null;
        
        protected HtmlSelect drpdSearchOperators;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            propertyBag = new LookupFieldWithPickerPropertyBag(this.PickerDialog.CustomProperty);
        }

        protected override void CreateChildControls()
        {
            //Copied from base class
            SPWeb contextWeb = SPControl.GetContextWeb(this.Context);

            Table child = new Table();
          
            child.Width = Unit.Percentage(100.0);
            child.Attributes.Add("cellspacing", "0");
            child.Attributes.Add("cellpadding", "0");
            TableRow row = new TableRow();
            row.Width = Unit.Percentage(100.0);
            Label label = new Label();
            TableCell cell = new TableCell();
            cell.CssClass = "ms-descriptiontext";
            cell.Attributes.Add("style", "white-space:nowrap");
            string str = SPHttpUtility.HtmlEncode(SPResource.GetString("PickerDialogDefaultSearchLabel", new object[0]));
            str = string.Format(CultureInfo.InvariantCulture, "<b>{0}</b>&nbsp;", new object[] { str });
            label.Text = str;
            cell.Controls.Add(label);
            this.ColumnList = new DropDownList();
            this.ColumnList.ID = "columnList";
            this.ColumnList.CssClass = "ms-pickerdropdown";
            cell.Controls.Add(this.ColumnList);
            row.Cells.Add(cell);

            //Punches-in the search operators dropdown
            cell = new TableCell();
            drpdSearchOperators = new HtmlSelect();
            drpdSearchOperators.ID = "queryOperators";
            drpdSearchOperators.Attributes.Add("class", "ms-pickerdropdown");

            cell.Controls.Add(drpdSearchOperators);
            row.Cells.Add(cell);

            cell = new TableCell();
            cell.Width = Unit.Percentage(100.0);
            this.QueryTextBox = new InputFormTextBox();
            this.QueryTextBox.ID = "queryTextBox";
            this.QueryTextBox.CssClass = "ms-pickersearchbox";
            this.QueryTextBox.AccessKey = SPResource.GetString("PickerDialogSearchAccessKey", new object[0]);
            this.QueryTextBox.Width = Unit.Percentage(100.0);
            this.QueryTextBox.MaxLength = 0x3e8;
            this.QueryTextBox.Text = QueryText;
            cell.Controls.Add(this.QueryTextBox);
            row.Cells.Add(cell);
            label.AssociatedControlID = "queryTextBox";
            cell = new TableCell();
            this.QueryButton = new ImageButton();
            this.QueryButton.ID = "queryButton";
            this.QueryButton.OnClientClick = "executeQuery();return false;";
            this.QueryButton.ToolTip = SPResource.GetString("PickerDialogSearchToolTip", new object[0]);
            this.QueryButton.AlternateText = SPResource.GetString("PickerDialogSearchToolTip", new object[0]);
            if (!contextWeb.RegionalSettings.IsRightToLeft)
            {
                this.QueryButton.ImageUrl = "/_layouts/images/gosearch.gif";
            }
            else
            {
                this.QueryButton.ImageUrl = "/_layouts/images/gortl.gif";
            }
            HtmlGenericControl control = new HtmlGenericControl("div");
            control.Attributes.Add("class", "ms-searchimage");
            control.Controls.Add(this.QueryButton);
            cell.Controls.Add(control);
            row.Cells.Add(cell);
            child.Rows.Add(row);


            //Add Message 
            Table tbMessage = new Table();           
            tbMessage.Width = Unit.Percentage(100.0);
            tbMessage.Attributes.Add("cellspacing", "0");
            tbMessage.Attributes.Add("cellpadding", "0");
            TableRow rowInfo = new TableRow();
            rowInfo.Width = Unit.Pixel(100);
            Label lblMessage = new Label();            
            lblMessage.Width = Unit.Pixel(15);
            TableCell cellMessage = new TableCell();          
            cellMessage.CssClass = "ms-descriptiontext";
            cellMessage.Attributes.Add("style", "white-space:nowrap");            
            lblMessage.Text = "This picker will only display the first 50 matches it finds. If you cannot see the record you were expecting, you</br> may wish to use the filter at the top of the window to reduce the number of matches.";
            cellMessage.Controls.Add(lblMessage);
            rowInfo.Cells.Add(cellMessage);
            tbMessage.Rows.Add(rowInfo);
            
            this.Controls.Add(child);
            this.Controls.Add(tbMessage);

            //fills the search fields initially
            if (!Page.IsPostBack)
            {

                // Get reference to LookUp List 
                SPList list = null;
                SPWeb ObjWeb = SPContext.Current.Web;
                list = ObjWeb.Lists[propertyBag.ListId];
                 
                List<string> searchFields = propertyBag.SearchFields;                
                foreach (SPField field in list.Fields)
                {
                    if (searchFields.Contains(field.InternalName))
                    {
                        mColumnList.Items.Add(new ListItem(field.Title, field.Id.ToString()));
                        
                    }
                    if (field.InternalName.ToLower() == "areas" || field.InternalName.ToLower() == "areas" || field.InternalName.ToLower() == "organisation" || field.InternalName.ToLower() == "organisations")
                    {
                        mColumnList.Items.Add(new ListItem(field.Title, field.Id.ToString()));
                    }
                }
            
                //fills the search operators initally
                FillSearchOperators(ColumnList.SelectedValue);
                base.GetCallbackResult();
            }
            
        }

        protected override void OnPreRender(EventArgs e)
        {
            //generate callback script for search field changes
            ClientScriptManager cs = Page.ClientScript;
            string context = GenerateContextScript();
            string cbr = cs.GetCallbackEventReference(this, "searchField", "SearchFieldChangedResult", context, false);
            String callbackScript = "function SearchFieldChanged() {"
               + "var searchField= 'searchFieldChangedTo:' + document.getElementById('" + SPHttpUtility.EcmaScriptStringLiteralEncode(this.ColumnList.ClientID) + "').value;"
               + cbr + "; }";

            cs.RegisterClientScriptBlock(this.GetType(), "SearchFieldChanged",
                callbackScript, true);

            ColumnList.Attributes.Add("onchange", "SearchFieldChanged();");

            //HACK: fragment from the base class with query operators hack
            string str = this.Page.ClientScript.GetCallbackEventReference(this, "search", "PickerDialogHandleQueryResult", "ctx", "PickerDialogHandleQueryError", true);

            this.Page.ClientScript.RegisterClientScriptBlock(base.GetType(), "__SEARCH__", "<script>\r\n                function executeQuery()\r\n                {\r\n   var operators=document.getElementById('" + SPHttpUtility.EcmaScriptStringLiteralEncode(this.drpdSearchOperators.ClientID) + "');                 var query=document.getElementById('" + SPHttpUtility.EcmaScriptStringLiteralEncode(this.QueryTextBox.ClientID) + "');\r\n                    var list=document.getElementById('" + SPHttpUtility.EcmaScriptStringLiteralEncode(this.ColumnList.ClientID) + "');\r\n                    var search='';\r\n                    var multiParts = new Array();\r\n       multiParts.push(operators.value);\r\n             if(list!=null)\r\n                        multiParts.push(list.value);\r\n                    else\r\n                        multiParts.push('');\r\n                    multiParts.push(query.value);\r\n\r\n                    search = ConvertMultiColumnValueToString(multiParts);\r\n                    PickerDialogSetClearState();\r\n                    \r\n                    var ctx = new PickerDialogCallbackContext();\r\n                    ctx.resultTableId = 'resultTable';\r\n                    ctx.queryTextBoxElementId = '" + SPHttpUtility.EcmaScriptStringLiteralEncode(this.QueryTextBox.ClientID) + "';\r\n                    ctx.errorElementId = '" + SPHttpUtility.EcmaScriptStringLiteralEncode(this.PickerDialog.ErrorLabel.ClientID) + "';\r\n                    ctx.htmlMessageElementId = '" + SPHttpUtility.EcmaScriptStringLiteralEncode(this.PickerDialog.HtmlMessageLabel.ClientID) + "';\r\n                    ctx.queryButtonElementId = '" + SPHttpUtility.EcmaScriptStringLiteralEncode(this.QueryButton.ClientID) + "';\r\n                    PickerDialogShowWait(ctx);\r\n                    " + str + ";\r\n                }\r\n                </script>");
            this.QueryTextBox.Text = this.QueryText;
            this.QueryTextBox.Attributes.Add("onKeyDown", "var e=event; if(!e) e=window.event; if(!browseris.safari && e.keyCode==13) { document.getElementById('" + SPHttpUtility.EcmaScriptStringLiteralEncode(this.QueryButton.ClientID) + "').click(); return false; }");
            if ((this.QueryTextBox.Text.Length > 0) && !this.Page.IsPostBack)
            {
                string group = string.Empty;
                if (this.ColumnList.SelectedItem != null)
                {
                    group = this.ColumnList.SelectedItem.Value;
                }
                this.ExecuteQuery(group, this.QueryText);
            }
            this.Page.ClientScript.RegisterStartupScript(base.GetType(), "SetFocus", "<script>\r\n                    var objQueryTextBox = document.getElementById('" + SPHttpUtility.EcmaScriptStringLiteralEncode(this.QueryTextBox.ClientID) + "'); \r\n                    if (objQueryTextBox != null)\r\n                    {\r\n                        try\r\n                        {\r\n                            objQueryTextBox.focus();\r\n                        }\r\n                        catch(e)\r\n                        {\r\n                        }\r\n                    }\r\n                  </script>");
        }

        private string GenerateContextScript()
        {
            StringBuilder context = new StringBuilder();
            context.Append("function SearchFieldChangedResult(searchOperators, context)");
            context.Append("{");
            context.Append("var drpdSearchOperators = document.getElementById('" + SPHttpUtility.EcmaScriptStringLiteralEncode(this.drpdSearchOperators.ClientID) + "');");
            context.Append("drpdSearchOperators.length=0;");
            context.Append("var operators = searchOperators.split(';');");
            context.Append("for(op=0;op<operators.length;op++)");
            context.Append("{");
            context.Append("var operator = operators[op].split(',');");
            context.Append("var option = document.createElement('option');");
            context.Append("option.text = operator[0];");
            context.Append("option.value = operator[1];");
            context.Append("drpdSearchOperators.add(option);");
            context.Append("}");
            context.Append("}");

            return context.ToString();
        }

        private void FillSearchOperators(string searchField)
        {
            drpdSearchOperators.Items.Clear();

            // Get reference to LookUp List 
            SPList list = null;
            SPWeb ObjWeb = SPContext.Current.Web;
            list = ObjWeb.Lists[propertyBag.ListId];
            
            SPField queryField = list.Fields[new Guid(searchField)];

            drpdSearchOperators.Items.Add(new ListItem("equals", "Eq"));
            drpdSearchOperators.Items.Add(new ListItem("not equal", "Neq"));

            if (queryField.Type == SPFieldType.Counter || queryField.Type == SPFieldType.Integer
                || queryField.Type == SPFieldType.Number || queryField.Type == SPFieldType.Currency
                || queryField.Type == SPFieldType.DateTime)
            {
                drpdSearchOperators.Items.Add(new ListItem("less then", "Lt"));
                drpdSearchOperators.Items.Add(new ListItem("less than or equal to", "Leq"));
                drpdSearchOperators.Items.Add(new ListItem("greater than", "Gt"));
                drpdSearchOperators.Items.Add(new ListItem("greater than or equal to", "Geq"));
            }
            else
            {
                if (queryField.Type != SPFieldType.Boolean && queryField.Type != SPFieldType.DateTime)
                {
                    drpdSearchOperators.Items.Insert(0, new ListItem("contains", "Contains"));
                }

                drpdSearchOperators.Items.Insert(1, new ListItem("begins with", "BeginsWith"));
            }

            //web.Dispose();
        }

        public new void RaiseCallbackEvent(string eventArgument)
        {
            //Wraps the base method to cut the hacked-in search operator

            if (eventArgument.StartsWith("searchFieldChangedTo:"))
            {
                searchField = eventArgument.Replace("searchFieldChangedTo:", "");
                return;
            }
            else
            {
                SPFieldMultiColumnValue multiVal = new SPFieldMultiColumnValue(eventArgument);
                if (multiVal.Count == 3)
                {
                    searchOperator = multiVal[0];
                    eventArgument = eventArgument.Replace(";#" + searchOperator, "");
                    base.RaiseCallbackEvent(eventArgument);
                }
                else
                    base.RaiseCallbackEvent(eventArgument);
            }
        }

        public new string GetCallbackResult()
        {
            if (String.IsNullOrEmpty(searchField))
            {
                return base.GetCallbackResult();
            }

            FillSearchOperators(searchField);

            string operators = "";
            foreach (ListItem item in drpdSearchOperators.Items)
            {
                if (operators.Length >= 1)
                    operators += ";";
                operators += item.Text + "," + item.Value;
            }
            return operators;
        }


        public override PickerEntity GetEntity(DataRow row)
        {
            PickerEntity entity = new PickerEntity();
            entity.DisplayText = row[propertyBag.FieldId.ToString()].ToString();
            entity.Key = row[SPBuiltInFieldId.ID.ToString()].ToString();
            entity.Description = entity.DisplayText;
            entity.IsResolved = true;

            foreach (DataColumn col in row.Table.Columns)
            {
                entity.EntityData.Add(col.Caption, row[col]);
            }            
            return entity;
        }


        protected override int IssueQuery(string search, string groupName, int pageIndex, int pageSize)
        {
            DataTable table = this.GetListTable(search, groupName);
            PickerDialog.Results = table;
            PickerDialog.ResultControl.PageSize = table.Rows.Count;
            return table.Rows.Count;
        }

        private ArrayList GetPagePickerEntities(string FieldName)
        {
            ArrayList alPickerEntities = new ArrayList();
            if (FieldName.ToLower().StartsWith("area"))
            {
                using (SPWeb ObjWeb = SPContext.Current.Site.OpenWeb(propertyBag.WebId))
                {
                    SPList ObjCurrentList = ObjWeb.Lists[propertyBag.ListName];
                    if (ObjCurrentList.Fields.ContainsField(FieldName)==false)
                    {
                        if (FieldName.ToLower() == "areas")
                        {
                            FieldName = FieldName.Substring(0, FieldName.Length - 1);
                        }
                    }
                }
            }           

            string _pickerName="Picker_" + SPContext.Current.Web.CurrentUser.ID + this.propertyBag.ListName + FieldName;
            if(Context.Application[_pickerName]!=null)
            {
                alPickerEntities = (ArrayList)Context.Application[_pickerName];
            }
            return alPickerEntities;
        }
        private DataTable GetListTable(string search, string groupName)
        {
            DataTable table = new DataTable();

            
            if (groupName == "")
            {
                groupName = this.ColumnList.SelectedValue;
            }
                SPWeb ObjWeb = SPContext.Current.Web;
            
                SPList ObjLookUpList = ObjWeb.Lists[propertyBag.ListId];
                SPField searchField = ObjLookUpList.Fields[new Guid(groupName)];
                
                SPListItemCollection items = null;                
                foreach (SPField field in ObjLookUpList.Fields)
                {
                    if (propertyBag.SearchFields.Contains(field.Id.ToString()) || propertyBag.FieldId == propertyBag.FieldId || field.Id == SPBuiltInFieldId.ID)
                    {
                        table.Columns.Add(field.Id.ToString());
                    }                    
                }

                SPQuery ObjQuery = new SPQuery();
                string strQuery = string.Empty;                
                ObjQuery.ViewAttributes = "Scope=\"Recursive\"";
                ArrayList FilterFields = new ArrayList();

                if (ObjLookUpList.Fields.ContainsField("Areas") && searchField.InternalName!="Areas")
                {
                    FilterFields.Add("Areas");
                }
                if (ObjLookUpList.Fields.ContainsField("Area") && searchField.InternalName != "Area")
                {
                    FilterFields.Add("Area");
                }

                if (ObjLookUpList.Fields.ContainsField("Organisation"))
                {
                    FilterFields.Add("Organisation");
                }

                if (ObjLookUpList.Fields.ContainsField("Organisations"))
                {
                    FilterFields.Add("Organisations");
                }

                strQuery = GetCAMLQueryString(propertyBag.ListName, strQuery, FilterFields);

                if (!string.IsNullOrEmpty(search))
                {
                    if (searchField.Type == SPFieldType.DateTime)
                    {
                        search = SPUtility.CreateISO8601DateTimeFromSystemDateTime(DateTime.Parse(search));
                    }

                    string valueType = searchField.TypeAsString;
                    if (searchField.Type == SPFieldType.Calculated)
                    {
                        valueType = "Text";
                    }
                    if (strQuery != "")
                    {
                        strQuery = "<And>" + strQuery + string.Format("<{0}><FieldRef ID=\"{1}\"/><Value Type=\"{2}\">{3}</Value></{0}></And>"
                            , searchOperator ?? "Eq"
                            , searchField.Id
                            , valueType
                            , search);
                    }
                    else
                    {
                        strQuery = string.Format("<{0}><FieldRef ID=\"{1}\"/><Value Type=\"{2}\">{3}</Value></{0}>"
                            , searchOperator ?? "Eq"
                            , searchField.Id
                            , valueType
                            , search);
                    }
                }
                if (strQuery != "")
                {
                    strQuery = "<Where>" + strQuery + "</Where>";
                }
                strQuery = strQuery + "<OrderBy><FieldRef Name=\"Title\" /></OrderBy>";
                ObjQuery.Query = strQuery;
                ObjQuery.RowLimit = 50;
                items = ObjLookUpList.GetItems(ObjQuery);


                if (items.Count > propertyBag.MaxSearchResults)
                {
                    this.PickerDialog.ErrorMessage = LookupFieldWithPickerHelper.GetResourceString("lookupWithPickerSearchResultExceededMessage");
                    return table;
                }

                foreach (SPListItem item in items)
                {
                    DataRow row = table.NewRow();
                    foreach (DataColumn col in table.Columns)
                    {
                        SPField field = item.Fields[new Guid(col.Caption)];
                        row[col] = field.GetFieldValueAsText(item[field.Id]);
                    }
                    table.Rows.Add(row);
                }
            
            return table;
        }

        private string GetCAMLQueryString(string ListName, string strQuery,ArrayList FilterFields)
        {
            CAML ObjCAML = new CAML();
            foreach (string FilterField in FilterFields)
            {
                ArrayList _alPickerEntities = GetPagePickerEntities(FilterField);

                ArrayList alValueTrack = new ArrayList();
                foreach (PickerEntity picker in _alPickerEntities)
                {

                    if (picker != null)
                    {
                        if (alValueTrack.Contains(picker.DisplayText) == false)
                        {
                            alValueTrack.Add(picker.DisplayText);
                            ObjCAML.OrCondition(new CAMLField(FilterField, picker.DisplayText));
                        }
                    }
                }
            }
            strQuery = ObjCAML.GetPickupCAML();
            return strQuery;
        }
    }
}
