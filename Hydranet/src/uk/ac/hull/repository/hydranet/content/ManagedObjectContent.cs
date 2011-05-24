using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;

namespace uk.ac.hull.repository.hydranet.content
{

    public class ManagedObjectContent : IObjectContent
    {
        private string contentURL;
        private byte[] dsContent;
     
        public string ContentURL
        {
            get
            {
                return contentURL;
            }
            set
            {
                this.contentURL = ContentURL;
            }

        }

        public byte[] DsContent
        {
            get
            {
                return dsContent;
            }
            set
            {
                this.dsContent = DsContent;
            }
        }

     
        public ManagedObjectContent(string contentURL)
        {
            this.contentURL = contentURL;
        }

        public ManagedObjectContent(byte[] dsContent)
        {
            this.dsContent = dsContent;
        }

        public string getXML()
        {
            if (dsContent != null)
            {
                return "<foxml:binaryContent>" +  Convert.ToBase64String(dsContent) + "</foxml:binaryContent>";
            }
            else
            {
                return "<foxml:contentLocation REF=\"" + this.contentURL + "\" TYPE=\"URL\"/>";
            }
        }

    }
}
