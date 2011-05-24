using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
namespace CLIF.Solutions.Code
{
    public interface IXmlDocumentProvider
    {
        XmlDocument Document 
        {
            get;
        }
    }
}