using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;

namespace uk.ac.hull.repository.hydranet.content
{
    public class ObjectDatastreamImpl : IObjectDatastream
    {
        public const string INTERNAL_XML_METADATA = "X";
        public const string MANAGED_CONTENT = "M";
        public const string EXTERNAL_REF_CONTENT = "E";
        public const string REDIRECTED_REF_CONTENT = "R";

        public const string ACTIVE_STATE = "A";
        public const string INACTIVE_STATE = "I";
        public const string DELETED_STATE = "D";
        
        private string dsId;
        private string label;
        private string mimeType;
        private string formatURI;
        private string[] altIds;
        private string checksum;
        private string state;
        private string controlGroup;
        private bool versionable;
        private IObjectContent content;
        private string contentURL;
        private string xmlContent;
        private byte[] dsContent;
        private int size = 0;

        #region IObjectDatastream Members

        public string DsId
        {
            get
            {
                return dsId;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public string Label
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public string MimeType
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public string FormatURI
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public string[] AltIds
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public string Checksum
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public int Size
        {
            get
            {
                return size;
            }
            set
            {
                size = value;
            }
        }

        public string State
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public string ControlGroup
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public bool Versionable
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public string ContentURL
        {
            get
            {
                return contentURL;
            }
        }

        public string XmlContent
        {
            get
            {
                return xmlContent;
            }
        }

        public IObjectContent Content
        {
            get
            {
                return content;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        public ObjectDatastreamImpl(string dsId, string label, string MIMEType, string formatURI, string[] altIds, string checksum,
                                        string state, string controlGroup, bool versionable, int size, string contentURL, string xmlContent, byte[] dsContent)
        {
            this.dsId = dsId;
            this.label = label;
            this.mimeType = MIMEType;
            this.formatURI = formatURI;
            this.altIds = altIds;
            this.checksum = checksum;
            this.state = state;
            this.controlGroup = controlGroup;
            this.versionable = versionable;
            this.size = size;
            this.contentURL = contentURL;

            this.dsContent = dsContent;
         
            if (controlGroup.Equals(INTERNAL_XML_METADATA))
            {
                this.xmlContent = xmlContent;
                content = new InternalXMLObjectContent(this.xmlContent);
            }
            else if (controlGroup.Equals(MANAGED_CONTENT))
            {
                if (dsContent != null)
                {
                    content = new ManagedObjectContent(dsContent);
                }
                else
                {
                    content = new ManagedObjectContent(this.contentURL);
                }
            }
            else if (controlGroup.Equals(EXTERNAL_REF_CONTENT))
            {
                content = new ExternalReferencedObjectContent(this.contentURL);
            }
            else if (controlGroup.Equals(REDIRECTED_REF_CONTENT))
            {
                content = new RedirectObjectContent(this.contentURL);
            }
        }


       

        public string getXML()
        {
            return "<foxml:datastream CONTROL_GROUP=\"" + controlGroup + "\" ID=\"" + dsId + "\" STATE=\"" + state + "\" VERSIONABLE=\"" + versionable.ToString().ToLower() + "\">" +
                        "<foxml:datastreamVersion ID=\"" + dsId + ".0\" LABEL=\"" + label + "\" SIZE=\"" + size.ToString() + "\" MIMETYPE=\"" + mimeType + "\">" +
                        content.getXML() +
                        "</foxml:datastreamVersion></foxml:datastream>";


        }

    }
}
