using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.SharePoint.WebControls;
using System.Collections;
using Microsoft.SharePoint;


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
    public class LookupFieldWithPickerControlMulti : LookupFieldWithPickerControl
    {
        public override object Value
        {
            get
            {
                this.EnsureChildControls();
                ArrayList resolvedEntities = this.lookupEditor.ResolvedEntities;
                if (resolvedEntities.Count == 0)
                {
                    return null;
                }
                SPFieldLookupValueCollection lookups = new SPFieldLookupValueCollection();
                foreach (PickerEntity entity in resolvedEntities)
                {
                    lookups.Add(new SPFieldLookupValue(int.Parse(entity.Key), entity.DisplayText));
                }
                return lookups;
            }
            set
            {
                this.EnsureChildControls();
                this.SetFieldControlValue(value);
            }
        }
        private void SetFieldControlValue(object value)
        {
            LookupFieldWithPicker lookupFieldPicker = (LookupFieldWithPicker)this.Field;
            ArrayList alList = new ArrayList();
            PickerEntity ObjPickerEntity = new PickerEntity();

            //Clear all existing Entities from the lookup
            this.lookupEditor.Entities.Clear();
            
            ObjPickerEntity = this.lookupEditor.ValidateEntity(ObjPickerEntity);
            {
                SPFieldLookupValueCollection lookupValues = value as SPFieldLookupValueCollection;
                foreach (SPFieldLookupValue lookupValue in lookupValues)
                {
                    PickerEntity entity = this.lookupEditor.GetEntityById(lookupValue.LookupId);
                    if (entity != null)
                    {
                        alList.Add(entity);
                    }
                }
            }
            Context.Application["Picker_" + Web.CurrentUser.ID + this.List.Title + this.Field.InternalName] = alList;
            this.lookupEditor.UpdateEntities(alList);
        }
    }
}
