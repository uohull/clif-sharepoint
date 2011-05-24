using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.SharePoint;
using System.Web;
using System.Collections;

namespace CLIF.Solutions.Code
{
    public class LookupFieldWithPickerHelper
    {
        public static bool IsSearchableField(SPField field)
        {
            return (field.Id == SPBuiltInFieldId.FileLeafRef || field.Hidden == false &&
                       (field.Type == SPFieldType.Counter
                        || field.Type == SPFieldType.Boolean
                        || field.Type == SPFieldType.Integer
                        || field.Type == SPFieldType.Currency
                        || field.Type == SPFieldType.DateTime
                        || field.Type == SPFieldType.Number
                        || field.Type == SPFieldType.Text
                        || field.Type == SPFieldType.URL
                        || field.Type == SPFieldType.User
                        || field.Type == SPFieldType.Choice
                        || field.Type == SPFieldType.MultiChoice
                        || field.Type == SPFieldType.Lookup
                        || (field.Type == SPFieldType.Calculated && ((SPFieldCalculated)field).OutputType == SPFieldType.Text))
                        || field.TypeAsString == "LookupFieldWithPicker"
                        );
        }

        public static string GetResourceString(string key)
        {
            string resourceClass = "CLIF.Solutions.Code.LookupFieldWithPicker";
            string value = HttpContext.GetGlobalResourceObject(resourceClass, key).ToString();
            return value;
        }        
    }

}
