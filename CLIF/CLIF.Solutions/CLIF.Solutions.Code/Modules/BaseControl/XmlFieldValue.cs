using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace CLIF.Solutions.Code
{
    public class XmlFieldValue : XmlBasedControl
    {
        private string _field;
        public string Field
        {
            get 
            { 
                return _field; 
            }
            set 
            { 
                _field = value; 
            }
        }
        protected override XmlDocument BuildDocument()
        {
            XmlDocument document = new XmlDocument();
            XmlElement fieldElem = document.CreateElement(Field);
            fieldElem.InnerText = (string)SPContext.Current.ListItem[Field];
            document.AppendChild(fieldElem);
            return document;
        }
    }
}
