using System;
using System.Web;
using System.ComponentModel;

namespace uk.ac.hull.repository.hydranet.content
{
    public class ExternalReferencedObjectContent : IObjectContent
    {
        private string contentURL;

        public string ContentURL
        {
            get
            {
                return contentURL;
            }
            set
            {
                throw new NotImplementedException();
            }

        }

        public ExternalReferencedObjectContent(string contentURL)
        {
            this.contentURL = contentURL;
        }

        public string getXML()
        {
            return "<foxml:contentLocation REF=\"" + contentURL + "\" TYPE=\"URL\" />";
        }
    }
}
