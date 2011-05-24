using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.SharePoint.WebControls;
using System.Web.UI;
using System.Collections;
using Microsoft.SharePoint;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Web;


namespace CLIF.Solutions.Code
{
    public class LookupFieldWithPickerControl : BaseFieldControl
    {
        protected LookupFieldWithPickerEntityEditor lookupEditor;


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if ((this.ControlMode == SPControlMode.New) && this.Page.IsPostBack == false)
            {
                ClearContext();
                if (this.ControlMode == SPControlMode.New)
                {
                    this.SetFieldControlValue(null);
                }
            }
        }
        private void ClearContext()
        {
            string _key = "Picker_" + SPContext.Current.Web.CurrentUser.ID + this.List.Title + this.FieldName;

            if (this.Context.Application[_key] != null)
            {
                this.Context.Application[_key] = null;
            }
        }
        public override object Value
        {
            get
            {
                this.EnsureChildControls();

                ArrayList resolvedEntities = this.lookupEditor.ResolvedEntities;
                if (resolvedEntities.Count == 0)
                    return null;

                if (resolvedEntities.Count == 1)
                {
                    PickerEntity entity = (PickerEntity)resolvedEntities[0];
                    return new SPFieldLookupValue(int.Parse(entity.Key), entity.DisplayText);
                }
                else
                    throw new IndexOutOfRangeException();
            }

            set
            {
                this.EnsureChildControls();
                this.SetFieldControlValue(value);
            }
        }


        protected override void CreateChildControls()
        {
            LookupFieldWithPicker lookupFieldPicker = (LookupFieldWithPicker)this.Field;

            SPWeb web = Web.Site.OpenWeb(lookupFieldPicker.LookupWebId);

            SPList lookupList = web.Lists[new Guid(lookupFieldPicker.LookupList)];
            SPField lookupField = null;
            try
            {
                lookupField = lookupList.Fields.GetFieldByInternalName(lookupFieldPicker.LookupField);
            }
            catch
            {
                //field has been deleted, fallback is the id field
                web = Web.Site.OpenWeb(lookupFieldPicker.LookupWebId);
                lookupField = lookupList.Fields[SPBuiltInFieldId.ID];
                this.List.ParentWeb.AllowUnsafeUpdates = true;
                lookupFieldPicker = (LookupFieldWithPicker)this.List.Fields[Field.Id];
                lookupFieldPicker.LookupField = lookupField.InternalName;
                lookupFieldPicker.Update(true);
            }


            this.lookupEditor = new LookupFieldWithPickerEntityEditor();
            this.lookupEditor.CustomProperty = new LookupFieldWithPickerPropertyBag(this.List.Title, this.FieldName, lookupFieldPicker.LookupWebId, (new Guid(lookupFieldPicker.LookupList)), lookupField.Id, lookupFieldPicker.SearchFields, lookupFieldPicker.EntityEditorRows, lookupFieldPicker.ListField).ToString();
            this.lookupEditor.MultiSelect = lookupFieldPicker.AllowMultipleValues;
            this.Controls.Add(lookupEditor);

            base.CreateChildControls();
        }

        private void SetFieldControlValue(object value)
        {
            string _areaName = string.Empty;
            this.lookupEditor.Entities.Clear();

            ArrayList list = new ArrayList();

            LookupFieldWithPicker lookupFieldPicker = (LookupFieldWithPicker)this.Field;
            if (this.ControlMode == SPControlMode.New && this.Page.IsPostBack == false && (lookupFieldPicker.ListName == "Areas" || lookupFieldPicker.ListName == "Area"))
            {
                if (Context.Request.QueryString["Source"] != null)
                {
                    string _url = Context.Request.QueryString["Source"].ToString();
                    _url = HttpUtility.UrlDecode(_url);

                    string[] strUrl = _url.Split('/');
                    _areaName = strUrl[strUrl.Length - 2];

                    //Getting the Original Area Name with '-'
                    _areaName = SPHelper.GetOriginalAreaName(_areaName);

                    SPQuery ObjQuery = new SPQuery();
                    ObjQuery.Query = "<Where><Eq><FieldRef Name='Title' /><Value Type='Text'>" + _areaName + "</Value></Eq></Where>";
                    SPListItemCollection items = this.Web.Lists["Areas"].GetItems(ObjQuery);
                    if (items.Count != 0)
                    {
                        //Setting the Default Area
                        SPFieldLookupValue AreaLookUpValue = new SPFieldLookupValue(items[0].ID, items[0].Title);
                        value = AreaLookUpValue;
                    }
                }

            }
            if (this.ControlMode == SPControlMode.New && lookupEditor.Entities.Count == 0
                && String.IsNullOrEmpty(lookupFieldPicker.CustomDefaultValue) == false)
            {
                string strValue = ParseDefaultValue(lookupFieldPicker.CustomDefaultValue);

                if (strValue == null)
                    return;

                PickerEntity defaultEntity = new PickerEntity();
                defaultEntity.Key = strValue;
                defaultEntity.DisplayText = strValue;
                defaultEntity = this.lookupEditor.ValidateEntity(defaultEntity);
                if (defaultEntity != null)
                    list.Add(defaultEntity);
            }
            else
            {
                if (value == null || value.ToString() == "")
                    return;

                SPFieldLookupValue lookupValue = value as SPFieldLookupValue;
                PickerEntity entity = this.lookupEditor.GetEntityById(lookupValue.LookupId);
                if (entity != null)
                {
                    list.Add(entity);
                }
            }
            Context.Application["Picker_" + Web.CurrentUser.ID + this.List.Title + this.Field.InternalName] = list;
            this.lookupEditor.UpdateEntities(list);

        }


        protected string ParseDefaultValue(object value)
        {
            string strValue = (string)value;

            if (strValue == "[CurrentUserId]")
            {
                strValue = Web.CurrentUser.ID.ToString();
            }
            else
            {
                Match m = Regex.Match(strValue, @"^\[UrlParam:(\w+)\]");
                if (m.Success)
                {
                    strValue = this.Context.Request.QueryString[m.Groups[1].Value];
                }
            }
            return strValue;
        }

        public override void Validate()
        {
            if (base.ControlMode != SPControlMode.Display)
            {
                EnsureChildControls();

                base.Validate();

                object val = this.Value;
                if (base.IsValid == true && val == null)
                {
                    if (base.Field.Required)
                    {
                        IsValid = false;
                        ErrorMessage = SPResource.GetString("MissingRequiredField", new object[0]);
                    }
                }
            }
        }
    }
}
