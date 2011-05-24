using System.Xml;
using System.Text;
using Microsoft.SharePoint.Administration;
using System.Collections.ObjectModel;


namespace CLIF.Solutions.Code
{
    public class WebConfigModifications
    {

        public string Owner
        {
            get
            {
                return this.GetType().FullName;
            }
        }

        private string GetWebConfigModName(string nodeName, XmlAttributeCollection attributes)
        {
            StringBuilder webConfigModName = new StringBuilder(nodeName);


            foreach (XmlAttribute attribute in attributes)
            {
                webConfigModName.Append(string.Format("[@{0}=\"{1}\"]", attribute.Name, attribute.Value));
            }


            return webConfigModName.ToString();
            //return string.Format("add[@key=\"{0}\"]", key);

        }

        private string GetWebConfigModValue(string nodeName, XmlAttributeCollection attributes)
        {
            XmlDataDocument xDoc = new XmlDataDocument();
            XmlAttribute newAttribute;
            XmlNode modValueNode = xDoc.AppendChild(xDoc.CreateElement(nodeName));
            foreach (XmlAttribute attribute in attributes)
            {
                newAttribute = xDoc.CreateAttribute(attribute.Name);
                newAttribute.Value = attribute.Value;
                modValueNode.Attributes.Append(newAttribute);
            }
            return string.Format("{0}\n", modValueNode.OuterXml);
        }

        private string GetFirstNodeName(ref XmlDataDocument xDoc)
        {
            string xPath = xDoc.FirstChild.Name;

            //we don't want to process the xml declaration node
            if (xPath == "xml")
            {
                if (xDoc.FirstChild.NextSibling != null)
                {
                    xPath = xDoc.FirstChild.NextSibling.Name;
                }
                else
                {
                    xPath = string.Empty;
                }
            }

            return xPath;
        }




        /// <summary>
        /// Adds the key/value pair as an appSettings entry in the web application's 
        /// SPWebConfigModification collection
        /// </summary>
        /// <param name="webApp">Current web application context</param>
        /// <param name="key">appSettings node key</param>
        /// <param name="value">appSettings node value</param>
        public void AddWebConfigNode(SPWebApplication webApp, string webConfigModxPath, XmlNode node, XmlAttributeCollection attributes)
        {
            SPWebConfigModification webConfigMod;
            string webConfigModName;

            webConfigModName = GetWebConfigModName(node.Name, attributes);
            webConfigMod = new SPWebConfigModification(webConfigModName, webConfigModxPath);
            webConfigMod.Owner = this.Owner;
            webConfigMod.Sequence = 0;
            webConfigMod.Type = SPWebConfigModification.SPWebConfigModificationType.EnsureChildNode;
            webConfigMod.Value = node.OuterXml;

            webApp.WebConfigModifications.Add(webConfigMod);
            webApp.Update();
        }

        /// <summary>
        /// Removes the key from the appSettings the web application's 
        /// SPWebConfigModification collection
        /// </summary>
        /// <param name="webApp">Current web application context</param>
        /// <param name="key">appSettings node key</param>        
        public void RemoveWebConfigNodes(SPWebApplication webApp)
        {
            Collection<SPWebConfigModification> collection;
            SPWebConfigModification[] tempCollection;
            //string webConfigModName;
            int iStartCount;
            SPWebConfigModification webConfigMod;

            collection = webApp.WebConfigModifications;
            tempCollection = new SPWebConfigModification[collection.Count];
            collection.CopyTo(tempCollection, 0);
            iStartCount = collection.Count;

            // Remove any modifications that were originally created by the owner.
            for (int c = iStartCount - 1; c >= 0; c--)
            {
                webConfigMod = collection[c];

                if (webConfigMod.Owner == this.Owner)
                    collection.Remove(webConfigMod);
            }
        }
    }
}
