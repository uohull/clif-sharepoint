
using System;
using System.Text;
using System.Web;
using System.Web.Caching;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Web.Services.Description;
using System.Xml;
namespace CLIF.Solutions.Code
{
    [WebService(Namespace = TreeViewService.LOCALNAMESPACE,
      Description = "A WebService for getting hierarchical data for the AJAX Tree Control.")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class TreeViewService : WebService, ITreeViewDataService
    {
        // The namespace for the WebService and the passed xml documents.
        // "d" is always used as the the prefix in this class.
        private const string LOCALNAMESPACE = @"http://www.mathertel.de/TreeView/TreeView";


        /// <summary>Name of the file containing the tracking statistics.</summary>
        private const string FILENAME = @"\S03_AJAXControls\treeviewdata.xml";

        /// <summary>Name of the cache-entry for the tracking data.</summary>
        private const string FILENAMEKEY = @"mathertel.sampletreedata";


        /// <summary>Retrieve the sub nodes of a specific folder.</summary>
        /// <param name="path">a path to a folder.</param>
        /// <returns>A XML document (tree) containing folder and file nodes.</returns>
        [WebMethod(Description = "Get the subnodes of a given folder path.")]
        public XmlNode GetSubNodes(string path)
        {

            XmlNode xRet = null;            
            XMLData ObjData = new XMLData();
            XmlDocument result = null;

            if (path == null)
            {
                throw new ArgumentNullException("path");
            }
            if (!path.StartsWith("/"))
            {
                throw new ArgumentException("no valid path was given.", "path");
            }

            // build a correct XPath expression
            StringBuilder xpath = new StringBuilder();
            xpath.Append("/ArrayOfContentObject");

            string _nodeValue = String.Empty;
            string[] values = path.Split('/');
            foreach (string value in values)
            {
                _nodeValue = value;
            }
            _nodeValue=_nodeValue.Replace(@"\",".");
            result = ObjData.GetData(_nodeValue);

            // load the whole document
            XmlDocument xDoc = result;  //Load();
            
            // find the node pecified by the path
            XmlElement x = xDoc.SelectSingleNode(xpath.ToString()) as XmlElement;

            // do a shallow copy
            xRet = xDoc.CreateElement("ArrayOfContentObject");
            foreach (XmlNode xe in x.ChildNodes)
            {                
                foreach (XmlNode xee in xe.ChildNodes)
                {
                    AddAttribute(xe, xee.Name, xee.InnerText);
                    if (xee.Name == "ObjectPID")    
                    {
                        AddAttribute(xe, "name", xee.InnerText);
                    }
                }
                xRet.AppendChild(xe.CloneNode(false));

            }
            return (xRet);
        }
        public XmlAttribute CreateAttribute(XmlDocument xmlDocument, string attrName, string attrValue)
        {
            XmlAttribute oAtt = xmlDocument.CreateAttribute(attrName);
            oAtt.Value = attrValue;
            return oAtt;
        }
        public XmlAttribute CreateAttribute(XmlNode node, string attrName, string attrValue)
        {
            return (CreateAttribute(node.OwnerDocument, attrName, attrValue));
        }
        public void AddAttribute(XmlNode node, string attributeName, string attributeValue)
        {
            node.Attributes.Append(CreateAttribute(node, attributeName, attributeValue));
        }     
    }
}