using System;
using System.Collections;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Schema;
using System.Reflection;
using System.IO;

namespace uk.ac.hull.repository.hydranet.hydracontent.metadata
{
    public class DublinCoreMetadata : IMetadata
    {
        private Hashtable dublinCore;
        private Hashtable MandatoryValues = new Hashtable();
        private string xml;

        private const string FORMAT_URI = "http://www.openarchives.org/OAI/2.0/oai_dc/";

        public DublinCoreMetadata()
        {
            dublinCore = new Hashtable();
        }

        public DublinCoreMetadata(string xml)
        {
            Xml = xml;
            ValidateDC();
        }

        static void XmlValidationError(object sender, ValidationEventArgs ex)
        {
            if (ex.Severity == XmlSeverityType.Error)    // ignore warnings
                throw new XmlSchemaValidationException(ex.Message);
        }

        private void ValidateDC()
        {
            ValidationEventHandler validationHandler = new ValidationEventHandler(XmlValidationError);
            XmlNameTable nameTable = new NameTable();

            XmlDocument xDoc = new XmlDocument(nameTable);
            xDoc.LoadXml(xml);

            XmlSchemaSet validationSchemas = new XmlSchemaSet();
            Assembly assembly = Assembly.GetExecutingAssembly();

            // following schemas edited to remove any external schemaLocation attributes in <import tags
            Stream stream = assembly.GetManifestResourceStream("uk.ac.hull.repository.hydranet.xml.xsd");
            validationSchemas.Add("http://www.w3.org/XML/1998/namespace", XmlReader.Create(new StreamReader(stream)));

            stream = assembly.GetManifestResourceStream("uk.ac.hull.repository.hydranet.simpledc20021212.xsd");
            validationSchemas.Add("http://purl.org/dc/elements/1.1/", XmlReader.Create(new StreamReader(stream)));

            stream = assembly.GetManifestResourceStream("uk.ac.hull.repository.hydranet.oai_dc.xsd");
            validationSchemas.Add("http://www.openarchives.org/OAI/2.0/oai_dc/", XmlReader.Create(new StreamReader(stream)));

            XPathNavigator xNavMain = xDoc.CreateNavigator();
            xNavMain.CheckValidity(validationSchemas, validationHandler);

            XmlNamespaceManager xNameSpaceMgr =
               new XmlNamespaceManager(new NameTable());

            xNameSpaceMgr.AddNamespace("valDC", "http://purl.org/dc/elements/1.1/");

            string fieldName = "";
            try
            {
                fieldName = "title";
                MandatoryValues.Add(fieldName, xNavMain.SelectSingleNode("//valDC:title[1]", xNameSpaceMgr).ToString());

            }
            catch (Exception)
            {
                throw new XmlSchemaValidationException(String.Format("Missing mandatory field '{0}' in DC metadata", fieldName));
            }

            foreach (DictionaryEntry entry in MandatoryValues)
            {
                if (entry.Value.ToString().Trim() == "")
                    throw new XmlSchemaValidationException(String.Format("Missing mandatory field '{0}' in DC metadata", entry.Key.ToString()));
            }

        }


        #region IMetadata Members

        public string Xml
        {
            get
            {
                if (!String.IsNullOrEmpty(xml))
                    return xml;

                xml = "<oai_dc:dc xmlns:dc=\"http://purl.org/dc/elements/1.1/\" xmlns:oai_dc=\"http://www.openarchives.org/OAI/2.0/oai_dc/\" " +
                      "xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:schemaLocation=\"http://www.openarchives.org/OAI/2.0/oai_dc/ http://www.openarchives.org/OAI/2.0/oai_dc.xsd\">";

                IDictionaryEnumerator _enumerator = dublinCore.GetEnumerator();

                while (_enumerator.MoveNext())
                {
                    string dcElement = (string)_enumerator.Key;
                    string dcValue = (string)_enumerator.Value;

                    xml = xml + "<dc:" + dcElement + ">" + dcValue + "</dc:" + dcElement + ">";
                }

                xml = xml + "</oai_dc:dc>";

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

        public Hashtable DublinCore
        {
            set
            {
                dublinCore = value;
            }
        }

        public string Title
        {
            set
            {
                dublinCore.Add("title", value);
            }
        }
       
        public string Creator
        {
            set
            {
                dublinCore.Add("creator", value);
            }
        }

        public string Subject
        {
            set
            {
                dublinCore.Add("subject", value);
            }
        }

        public string Description
        {
            set
            {
                dublinCore.Add("description", value);
            }
        }


        public string Publisher
        {
            set
            {
                dublinCore.Add("publisher", value);
            }
        }
        public string Contributor
        {
            set
            {
                dublinCore.Add("contributor", value);
            }
        }

        public string Date
        {
              set
            {
                dublinCore.Add("date", value);
            }
        }

        public string Type
        {
            set
            {
                dublinCore.Add("type", value);
            }
        }
        public string Format
        {
            set
            {
                dublinCore.Add("format", value);
            }
        }
       
        public string Source
        {
            set
            {
                dublinCore.Add("source", value);
            }
        }
        public string Language
        {
            set
            {
                dublinCore.Add("language", value);
            }
        }

        public string Identifier
        {
            set
            {
                dublinCore.Add("identifier", value);
            }
        }
        public string Relation
        {
            set
            {
                dublinCore.Add("relation", value);
            }
        }
        public string Coverage
        {
            set
            {
                dublinCore.Add("coverage", value);
            }
        }

        public string Rights
        {
            set
            {
                dublinCore.Add("rights", value);
            }
        }
       
       }
         
}
