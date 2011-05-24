using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using uk.ac.hull.repository.hydranet.hydracontent.metadata;

namespace uk.ac.hull.repository.hydranet.content
{


    public class InternalXMLObjectContent : IObjectContent
    {
        private string xmlContent;

        public InternalXMLObjectContent(String xmlContent)
        {
            this.xmlContent = xmlContent;
        }

        public InternalXMLObjectContent(IMetadata metadata)
        {
            this.xmlContent = metadata.ToString();
        }

        public string XmlContent
        {
            get
            {
                return xmlContent;
            }
            set
            {
                throw new NotImplementedException();
            }
           
        }

        public string getXML()
        {
            return "<foxml:xmlContent>" + xmlContent + "</foxml:xmlContent>";
        }
    }
}
