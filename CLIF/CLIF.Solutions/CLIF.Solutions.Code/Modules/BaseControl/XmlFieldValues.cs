using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Diagnostics;

namespace CLIF.Solutions.Code
{
    public class XmlFieldValues : XmlBasedControl
    {
        private string _fields;
        private char[] commaSep = { ',' };

        public string Fields
        {
            get { return _fields; }
            set { _fields = value; }
        }

        protected override XmlDocument BuildDocument()
        {
            XmlDocument document = new XmlDocument();
            XmlElement rootElem = document.CreateElement("Values");

            foreach (string field in Fields.Split(commaSep, StringSplitOptions.RemoveEmptyEntries))
            {
                XmlElement fieldElem = document.CreateElement(field);
                try
                {
                    fieldElem.InnerText = (string)SPContext.Current.ListItem[field];
                }
                catch (Exception ex)
                {
                    Trace.WriteLine("Error reading column " + field + " from list item " 
                        + SPContext.Current.ListItem.Title + ". Original exception was : " + ex.Message);
                }

                rootElem.AppendChild(fieldElem);
            }
            document.AppendChild(rootElem);
            return document;
        }
    }
}
