using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.SharePoint.WebControls;
using System.Collections;
using Microsoft.SharePoint;


namespace CLIF.Solutions.Code
{
    public class LookupFieldWithPickerDialog : PickerDialog
    {
        public LookupFieldWithPickerDialog(): base(new LookupFieldWithPickerQuery(), new TableResultControl(), new LookupFieldWithPickerEntityEditor())
        {
        }
        protected override void OnLoad(EventArgs e)
        {
            ArrayList columnDisplayNames = ((TableResultControl)base.ResultControl).ColumnDisplayNames;
            columnDisplayNames.Clear();

            ArrayList columnNames = ((TableResultControl)base.ResultControl).ColumnNames;
            columnNames.Clear();

            ArrayList columnWidths = ((TableResultControl)base.ResultControl).ColumnWidths;
            columnWidths.Clear();

            LookupFieldWithPickerPropertyBag propertyBag = new LookupFieldWithPickerPropertyBag(this.CustomProperty);

            //Get reference to LookUp List 
            SPList list = null;
            SPWeb ObjWeb = SPContext.Current.Web;
            list = ObjWeb.Lists[propertyBag.ListId];

            List<string> searchFields = propertyBag.SearchFields;

            foreach (SPField field in list.Fields)
            {
                //Add 'ID' Columns in the SearchFields/FieldId Parameter
                if (propertyBag.SearchFields.Contains(field.Id.ToString()) || propertyBag.FieldId == field.Id)
                {
                    if (columnDisplayNames.Contains(field.Title) == false)
                    {
                        columnDisplayNames.Add(field.Title);
                        columnNames.Add(field.Id.ToString());
                    }
                }

                //Add Columns in the the SearchFields Parameter
                if (propertyBag.SearchFields.Contains(field.InternalName))
                {
                    if (columnDisplayNames.Contains(field.Title) == false)
                    {
                        columnDisplayNames.Add(field.Title);
                        columnNames.Add(field.Id.ToString());
                    }
                }

                //Add Organisation(s) Column in the Popup List if exists
                if (field.InternalName.ToLower() == "organisation" || field.InternalName.ToLower() == "organisations")
                {
                    if (field.InternalName.ToLower() == "organisations")
                    {
                        columnDisplayNames.Insert(1, "Organisation(s)");
                        columnNames.Insert(1, field.Id.ToString());
                    }
                    else
                    {
                        columnDisplayNames.Insert(1, field.Title);
                        columnNames.Insert(1, field.Id.ToString());
                    }
                }
                //Add Area/Areas Column in the Popup List if exists
                if (field.InternalName.ToLower() == "areas" || field.InternalName.ToLower() == "area")
                {
                    columnDisplayNames.Add(field.Title);
                    columnNames.Add(field.Id.ToString());
                }
            }

            //Adjust Column Widths
            if (columnNames.Count > 0)
            {
                int width = (int)(100 / columnNames.Count);
                for (int i = 0; i < columnNames.Count; i++)
                {
                    columnWidths.Add(width.ToString() + "%");
                }
            }
            base.OnLoad(e);
        }

        protected override System.Web.HttpContext Context
        {
            get
            {
                return base.Context;
            }
        }

        protected override void OnUnload(EventArgs e)
        {
            base.OnUnload(e);
            if (base.EditorControl.Entities.Count > 0)
            {
                LookupFieldWithPickerPropertyBag propertyBag = new LookupFieldWithPickerPropertyBag(this.CustomProperty);
 
                AddToContext(propertyBag.ListName,propertyBag.FieldName,(PickerEntity)base.EditorControl.Entities[0]);                
            }
        }

        public void AddToContext(string ListName, string FieldName,PickerEntity NewPicker)
        {
            string _key = "Picker_" + SPContext.Current.Web.CurrentUser.ID + ListName + FieldName;
            ArrayList alOptions = new ArrayList();
            if (Context.Application[_key] != null)
            {
                alOptions = (ArrayList)Context.Application[_key];
            }
            alOptions.Add(NewPicker);
            Context.Application[_key] = alOptions;
        }
    }
}
