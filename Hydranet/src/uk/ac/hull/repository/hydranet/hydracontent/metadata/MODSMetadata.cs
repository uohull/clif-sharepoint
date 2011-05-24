using System;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Schema;
using System.Collections;
using System.Reflection;
using System.IO;

namespace uk.ac.hull.repository.hydranet.hydracontent.metadata
{
    public class MODSMetadata :IMetadata
    {
        private const string FORMAT_URI = "http://www.loc.gov/mods/v3";
        private Hashtable MandatoryValues = new Hashtable();

        #region IMetadata Members

        private string xml;

        public string Xml
        {
            get
            {
                return xml;
            }
            set
            {
                xml = value;
            }
        }
        
        public string FormatURI
        {
            get
            {
                 return FORMAT_URI;
            }
        }

        #endregion

        public MODSMetadata(string modsXml)
        {
            xml = modsXml;
            ValidateMODS();
        }

        private void  ValidateMODS()
        {
            ValidationEventHandler validationHandler = new ValidationEventHandler(XmlValidationError);
            XmlNameTable nameTable = new NameTable();

            XmlDocument xDoc = new XmlDocument(nameTable);
            xDoc.LoadXml(xml);

            XmlSchemaSet validationSchemas = new XmlSchemaSet();
            Assembly assembly = Assembly.GetExecutingAssembly();

            // following schemas edited to remove any external schemaLocation attributes in <import tags
            Stream stream = assembly.GetManifestResourceStream("uk.ac.hull.repository.hydranet.xlink.xsd");
            validationSchemas.Add("http://www.w3.org/1999/xlink", XmlReader.Create(new StreamReader(stream)));
            stream = assembly.GetManifestResourceStream("uk.ac.hull.repository.hydranet.mods-3-3.xsd");
            validationSchemas.Add("http://www.loc.gov/mods/v3", XmlReader.Create(new StreamReader(stream)));

            XPathNavigator xNavMain = xDoc.CreateNavigator();
            xNavMain.CheckValidity(validationSchemas,validationHandler);

            XmlNamespaceManager xNameSpaceMgr =
               new XmlNamespaceManager(new NameTable());

            xNameSpaceMgr.AddNamespace("valMODS", "http://www.loc.gov/mods/v3");
            xNameSpaceMgr.AddNamespace("valXlinq", "http://www.w3.org/1999/xlink");
                        
            string fieldName = "";
            try
            {
                fieldName = "version";
                MandatoryValues.Add(fieldName, xNavMain.SelectSingleNode(
                   "valMODS:modsCollection/valMODS:mods/@valMODS:version | valMODS:modsCollection/valMODS:mods/@version", xNameSpaceMgr).ToString());

                fieldName = "title/titleInfo";
                MandatoryValues.Add(fieldName, xNavMain.SelectSingleNode("valMODS:modsCollection/valMODS:mods/valMODS:titleInfo/valMODS:title", xNameSpaceMgr).ToString());

            }
            catch (Exception)
            {
                throw new XmlSchemaValidationException(String.Format("Missing mandatory field '{0}' in MODS metadata",fieldName));  
            }

            foreach (DictionaryEntry entry in MandatoryValues)
            {
                if (entry.Value.ToString().Trim() == "")
                    throw new XmlSchemaValidationException(String.Format("Missing mandatory field '{0}' in MODS metadata", entry.Key.ToString()));
            }

        }

        static void XmlValidationError(object sender, ValidationEventArgs ex)
        {
            if(ex.Severity == XmlSeverityType.Error)    // ignore warnings
                throw new XmlSchemaValidationException(ex.Message);  
        }


    }
}
