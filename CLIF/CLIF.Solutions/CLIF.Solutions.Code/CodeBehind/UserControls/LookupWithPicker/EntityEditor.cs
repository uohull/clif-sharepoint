using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.SharePoint.WebControls;
using Microsoft.SharePoint;
using System.Collections;

namespace CLIF.Solutions.Code
{
    public class LookupFieldWithPickerEntityEditor : EntityEditorWithPicker
    {

        protected override bool DefaultPlaceButtonsUnderEntityEditor
        {
            get
            {
                return this.MultiSelect;
            }
        }


        protected override int DefaultRows
        {
            get
            {
                if (!this.MultiSelect)
                {
                    return base.DefaultRows;
                }

                return new LookupFieldWithPickerPropertyBag(this.CustomProperty).EntityEditorRows;
            }
        }


        public PickerEntity GetEntityById(int id)
        {
            PickerEntity entity = null;
            if (id > 0)
            {
                    LookupFieldWithPickerPropertyBag propertyBag = new LookupFieldWithPickerPropertyBag(this.CustomProperty);
                    SPWeb web = SPContext.Current.Site.OpenWeb(propertyBag.WebId);              
                    SPList list = web.Lists[propertyBag.ListId];
                    SPQuery queryById = new SPQuery();
                    queryById.ViewAttributes = "Scope=\"Recursive\"";
                    queryById.Query = string.Format("<Where><Eq><FieldRef Name=\"ID\"/><Value Type=\"Integer\">{0}</Value></Eq></Where>", id);
                    SPListItemCollection items = list.GetItems(queryById);
                    if (items.Count > 0)
                    {
                        entity = this.GetEntity(items[0]);
                    }         
                    web.Dispose();                
            }

            return entity;
        }

        public override PickerEntity ValidateEntity(PickerEntity needsValidation)
        {
            PickerEntity entity = needsValidation;

            LookupFieldWithPickerPropertyBag propertyBag = new LookupFieldWithPickerPropertyBag(this.CustomProperty);

            if (!string.IsNullOrEmpty(needsValidation.DisplayText))
            {
                    // Get reference to LookUp List 
                    SPList list = null;
                    SPWeb ObjWeb = SPContext.Current.Web;
                    list = ObjWeb.Lists[propertyBag.ListId];

                    SPQuery queryById = new SPQuery();
                    queryById.ViewAttributes = "Scope=\"Recursive\"";
                    queryById.Query = string.Format("<Where><Eq><FieldRef Name=\"ID\"/><Value Type=\"Integer\">{0}</Value></Eq></Where>", needsValidation.Key);
                    SPListItemCollection items = list.GetItems(queryById);
                    if (items.Count > 0)
                    {
                        entity = this.GetEntity(items[0]);
                    }
                    else
                    {
                        SPQuery queryByTitle = new SPQuery();
                        queryByTitle.Query = string.Format("<Where><Eq><FieldRef ID=\"{0}\"/><Value Type=\"Text\">{1}</Value></Eq></Where>", propertyBag.FieldId, needsValidation.DisplayText);
                        queryByTitle.ViewAttributes = "Scope=\"Recursive\"";
                        items = list.GetItems(queryByTitle);
                        if (items.Count > 0)
                        {
                            entity = this.GetEntity(items[0]);
                        }
                    }
                    if (this.Entities != null)
                    {
                        Context.Application["Picker_" + SPContext.Current.Web.CurrentUser.ID + propertyBag.ListName + propertyBag.FieldName] = this.Entities;
                        //AddToContext(propertyBag.ListName, propertyBag.FieldName, (PickerEntity)base.EditorControl.Entities[0]);                
                    }                
            }
            return entity;
        }

        public void AddToContext(string ListName, string FieldName, PickerEntity NewPicker)
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
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.PickerDialogType = typeof(LookupFieldWithPickerDialog);
        
        }

        protected override PickerEntity[] ResolveErrorBySearch(string unresolvedText)
        {
            List<PickerEntity> entities = new List<PickerEntity>();
            LookupFieldWithPickerPropertyBag propertyBag = new LookupFieldWithPickerPropertyBag(this.CustomProperty);

            SPWeb web = SPContext.Current.Site.OpenWeb(propertyBag.WebId);
          
                SPList list = web.Lists[propertyBag.ListId];
                SPQuery query = new SPQuery();
                query.ViewAttributes = "Scope=\"Recursive\"";
                query.Query = string.Format("<Where><Contains><FieldRef ID=\"{0}\"/><Value Type=\"Text\">{1}</Value></Contains></Where>", propertyBag.FieldId, unresolvedText);
                SPListItemCollection items = list.GetItems(query);

                foreach (SPListItem item in items)
                {
                    entities.Add(this.GetEntity(item));
                }

        
                web.Dispose();
            
            return entities.ToArray();
        }

        private PickerEntity GetEntity(SPListItem item)
        {
            LookupFieldWithPickerPropertyBag propertyBag = new LookupFieldWithPickerPropertyBag(this.CustomProperty);

            PickerEntity entity = new PickerEntity();
            string displayValue = null;
            try
            {
                displayValue = item[propertyBag.FieldId] as String;
            }
            catch
            {
                //field has been deleted
            }

            if (displayValue != null
                && item.Fields[propertyBag.FieldId].Type == SPFieldType.Calculated
                && item[propertyBag.FieldId] != null
                && item[propertyBag.FieldId].ToString().Contains("#"))
            {
                entity.DisplayText = displayValue.ToString().Split('#')[1];
            }
            else
                entity.DisplayText = displayValue ?? "";
            entity.Key = item.ID.ToString();
            entity.Description = entity.DisplayText;
            entity.IsResolved = true;

            return entity;
        }

        public override bool LoadPostData(string postDataKey, System.Collections.Specialized.NameValueCollection values)
        {
            return base.LoadPostData(postDataKey, values);
        }
        public override void RaisePostDataChangedEvent()
        {
            base.RaisePostDataChangedEvent();
        }
    }

}
