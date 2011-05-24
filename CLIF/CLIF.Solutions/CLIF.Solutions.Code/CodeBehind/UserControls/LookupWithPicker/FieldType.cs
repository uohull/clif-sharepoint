using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.SharePoint;
using System.Xml;
using System.Reflection;
using Microsoft.SharePoint.WebControls;
using System.Runtime.Remoting.Contexts;
namespace CLIF.Solutions.Code
{
    /*
*************************************************************************************
   Title:              LookupFieldWithPicker Class       
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
 
    public class LookupFieldWithPicker : SPFieldLookup
    {
        #region Constructors
        public LookupFieldWithPicker(SPFieldCollection fields, string fieldName)
            : base(fields, fieldName)
        {
        }
        public LookupFieldWithPicker(SPFieldCollection fields, string typeName, string displayName)
            : base(fields, typeName, displayName)
        {
        }
        #endregion

        public string CustomDefaultValue
        {
            get
            {
                object obj = this.GetFieldAttribute("CustomDefaultValue");
                if (obj == null)
                    return "";
                else
                    return obj.ToString();
            }
            set
            {
                if (value == null)
                    SetFieldAttribute("CustomDefaultValue", "");
                else
                    SetFieldAttribute("CustomDefaultValue", value.ToString());
            }
        }

        public int MaxSearchResults
        {
            get
            {
                object obj = this.GetFieldAttribute("MaxSearchResults");
                if (obj == null)
                    return 100;
                else
                {
                    string str = obj.ToString();
                    int result = default(Int32);
                    int.TryParse(str, out result);
                    return result;
                }
            }
            set
            {
                if (value < 1)
                    throw new Exception("MaxSearchResults must be a positive number");

                SetFieldAttribute("MaxSearchResults", value.ToString());
            }
        }

        /// <summary>
        /// Entity Editor Rows
        /// </summary>
        public int EntityEditorRows
        {
            get
            {
                object obj = this.GetFieldAttribute("EntityEditorRows");
                if (obj == null)
                {
                    return 1;
                }
                else
                {
                    string str = obj.ToString();
                    int result = default(Int32);
                    int.TryParse(str, out result);
                    return result;
                }
            }
            set
            {
                if (value < 1)
                    throw new Exception("EntityEditorRows must be greater or equals than 1");

                SetFieldAttribute("EntityEditorRows", value.ToString());
            }
        }
        /// <summary>
        /// Lookup List Name
        /// </summary>
        public string ListName
        {
            get
            {
                object ObjList = this.GetFieldAttribute("ListName");
                if (ObjList == null)
                {
                    return "Areas";
                }
                else
                {
                    return ObjList.ToString();
                }
            }
            set
            {
                SetFieldAttribute("ListName", value.ToString());
            }
        }
        /// <summary>
        /// List Field Name
        /// </summary>
        public string ListField
        {
            get
            {
                object obj = this.GetFieldAttribute("ListField");
                if (obj == null)
                    return "Title";
                else
                {
                    string str = obj.ToString();
                    return str;
                }
            }
            set
            {
                SetFieldAttribute("ListField", value.ToString());
            }
        }
        /// <summary>
        /// Search Fields
        /// </summary>
        public List<string> SearchFields
        {
            get
            {
                List<string> searchFields = new List<string>();

                string strSearchFields = this.GetFieldAttribute("SearchFields");

                //if (String.IsNullOrEmpty(strSearchFields) != true)
                //    searchFields = new List<string>(strSearchFields.Split(','));

                searchFields.Add("Title");

                return searchFields;
            }
            set
            {
                if (value.Count == 0)
                    throw new Exception("One search field is required.");

                string str = "";
                foreach (string strField in value)
                {
                    if (str.Length > 0)
                        str += ",";
                    str += strField;
                }

                this.SetFieldAttribute("SearchFields", str);
            }
        }
        /// <summary>
        /// Allow Multiple Values 
        /// </summary>
        public override bool AllowMultipleValues
        {
            get
            {
                return base.AllowMultipleValues;
            }
            set
            {
                base.AllowMultipleValues = value;
                this.SetFieldAttribute("Type", "LookupFieldWithPicker");
            }
        }
        /// <summary>
        /// Validation String
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override string GetValidatedString(object value)
        {
            if (value == null)
            {
                throw new SPFieldValidationException(SPResource.GetString("MissingRequiredField", new object[0]));
            }

            return base.GetValidatedString(value);
        }
        /// <summary>
        /// Render Multi/Single LookUp Control
        /// </summary>
        public override BaseFieldControl FieldRenderingControl
        {
            get
            {
                BaseFieldControl control = null;
                if (AllowMultipleValues)
                {
                    control = new LookupFieldWithPickerControlMulti();
                }
                else
                {
                    control = new LookupFieldWithPickerControl();
                }
                control.FieldName = this.InternalName;

                return control;
            }
        }


        /// <summary>
        /// Setting the Attribute Values
        /// </summary>
        /// <param name="attribute">string</param>
        /// <param name="value">string</param>
        private void SetFieldAttribute(string attribute, string value)
        {
            //Invokes an internal method from the base class
            Type baseType = typeof(LookupFieldWithPicker);
            MethodInfo mi = baseType.GetMethod("SetFieldAttributeValue", BindingFlags.Instance | BindingFlags.NonPublic);
            mi.Invoke(this, new object[] { attribute, value });
        }
        /// <summary>
        /// Getting the Field Attributes
        /// </summary>
        /// <param name="attribute">string</param>
        /// <returns>string</returns>
        private string GetFieldAttribute(string attribute)
        {
            //Invokes an internal method from the base class
            Type baseType = typeof(LookupFieldWithPicker);
            MethodInfo mi = baseType.GetMethod("GetFieldAttributeValue", BindingFlags.Instance | BindingFlags.NonPublic, null, new Type[] { typeof(String) }, null);
            object obj = mi.Invoke(this, new object[] { attribute });
            if (obj == null)
            {
                return "";
            }
            else
            {
                return obj.ToString();
            }
        }
    }

}
