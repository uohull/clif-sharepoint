using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;

namespace uk.ac.hull.repository.hydranet.content
{
    public class RedirectObjectContent : IObjectContent
    {
        public string redirectURL
        {
            get
            {
                return redirectURL;
            }
            set
            {
                throw new NotImplementedException();
            }

        }

        public RedirectObjectContent(string redirectURL)
        {
            this.redirectURL = redirectURL;
        }

        public string getXML()
        {
            return "<foxml:contentLocation REF=\"" + redirectURL + "\" TYPE=\"URL\"/>";
        }
    }
}
