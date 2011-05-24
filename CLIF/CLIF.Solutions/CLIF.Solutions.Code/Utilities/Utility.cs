/// ===========================================================================
/// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY
/// KIND, WHETHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
/// IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR
/// PURPOSE.
/// ===========================================================================
/// 
/// Project:        Connected Lookup
/// Author:         Leonid Lyublinski 
/// Date:           07/01/2007  Version:        1.0
///
/// ===========================================================================


using System;
using System.Xml;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Collections;
using System.Collections.Generic;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using Microsoft.SharePoint.Utilities;
using System.Globalization;

namespace CLIF.Solutions.Code.Common
{
    internal sealed class Utility
    {
        internal static Guid ParseGuid(string guidString)
        {
            Guid guid = Guid.Empty;
            if (string.IsNullOrEmpty(guidString))
            {
                return guid;
            }
            try
            {
                guid = new Guid(guidString);
            }
            catch (FormatException) 
            { 
                return guid; 
            }
            return guid;
        }

        internal static void AddFieldToDropDown(DropDownList showField,
            SPField field,
            string strSelectedField,
            string extraText)
        {
            ListItem item = new ListItem();
            item.Text = field.Title + " " + field.AuthoringInfo + " ";

            if (!string.IsNullOrEmpty(extraText))
                item.Text += extraText;

            item.Value = field.InternalName;
            if (field.InternalName == strSelectedField)
                item.Selected = true;

            showField.Items.Add(item);
        }

        internal static void PopulateShowFieldDropDown(
            DropDownList showField,
            SPList lookupTolist,
            Guid ownerListId,
            string strSelectedField,
            string[] excludedShowFields)
        {
            showField.Items.Clear();

            foreach (SPField field in lookupTolist.Fields)
            {
                if (field.Hidden == true)
                    continue; //do not show hidden fields
                if (excludedShowFields != null &&
                    Array.IndexOf<string>(excludedShowFields, field.InternalName) >= 0)
                    continue;
                if (field.Type == SPFieldType.Counter
                    || field.Type == SPFieldType.Text
                    || (field.Type == SPFieldType.Computed && ((SPFieldComputed)field).EnableLookup == true)
                    || (field.Type == SPFieldType.Calculated && ((SPFieldCalculated)field).OutputType == SPFieldType.Text))
                {
                    Common.Utility.AddFieldToDropDown(showField, field, strSelectedField, null);
                }
            }
        }
                     
        internal static string SetNamedStringItem(
            XmlDocument xdDoc,
            XmlNode node,
            string strName,
            string strValue)
        {
            // Check to see if strName is in the tree
            XmlNode nodeTemp = node.Attributes.GetNamedItem(strName);
            if (nodeTemp != null)            
                nodeTemp.Value = strValue;            
            else
            {
                XmlAttribute newAttr = xdDoc.CreateAttribute(strName);
                newAttr.Value = strValue;
                node.Attributes.SetNamedItem(newAttr);
            }
            return node.OuterXml;
        }
        
        internal static string GetNamedStringItem(XmlNode node, string strName)
        {
            XmlAttributeCollection attrColl = node.Attributes;
            XmlNode nodeTemp = attrColl.GetNamedItem(strName);
            return (nodeTemp != null) ? nodeTemp.Value : null;
        }

        internal static bool GetNamedBoolItem(XmlNode node, string strName)
        {
            // Get the attribute collection
            XmlAttributeCollection attrColl = node.Attributes;

            // Get the node from the Attributes Collection
            XmlNode nodeTemp = attrColl.GetNamedItem(strName);

            // Check for null and default to false
            if ((nodeTemp != null) &&
                (nodeTemp.Value == "-1"))
                return true;
            else
                return false;
        }

        internal static bool GetNamedIntItem(XmlNode node,
             string strName,
             out Int32 iValue)
        {
            // Get the attribute collection
            XmlAttributeCollection attrColl = node.Attributes;

            // Get the node from the Attributes Collection
            XmlNode nodeTemp = attrColl.GetNamedItem(strName);

            if (nodeTemp != null)
            {
                iValue = Convert.ToInt32(
                    nodeTemp.Value,
                    CultureInfo.InvariantCulture);
                return true;
            }
            else
            { 
                iValue = -1;
                return false;
            }
        }

        internal XmlNode Node(string schemaXml)
        {
            XmlElement el = null;
            XmlDocument field = new XmlDocument();
            try
            {
                field.LoadXml(schemaXml);
            }
            catch (Exception)
            {
                field.LoadXml(SPStringUtility.RemoveControlChars(schemaXml));
            }
            el = field.DocumentElement;     
            return el;           
        }

        internal static string GetDelimitedString(string delimiter, ArrayList al)
        {
            string result = string.Join(delimiter, (string[])al.ToArray(typeof(string)));
            return result;
        }

        internal static SPWeb GetWeb(HttpContext context, string siteUrl)
        {
            SPWeb web;
            if (string.IsNullOrEmpty(siteUrl))
            {
                web = SPControl.GetContextWeb(context);
            }
            else
            {
                SPSite site = new SPSite(siteUrl);
                web = site.OpenWeb();
            }
            return web;
        }


    }
}
