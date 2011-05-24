using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using  System.IO;
using System.Net;
using System.Xml;
using System.Xml.Xsl;
using System.Text.RegularExpressions;

namespace CLIF.Solutions.Code
{
    public class GetJavaScriptProxy : Page
    {
        protected override void OnLoad(EventArgs e)
        {
            string asText = Request.QueryString["html"];
            string fileName = Request.QueryString["service"];

            Response.Clear();
            if (asText != null)
            {
                Response.ContentType = "text/html";
                Response.Write("<pre>");
            }
            else
            {
                Response.ContentType = "text/text";
            }
            string ret = CreateClientProxies(fileName);
            ret = Regex.Replace(ret, @"\n *", "\n");
            ret = Regex.Replace(ret, @"\r\n *""", "\"");
            ret = Regex.Replace(ret, @"\r\n, *""", ",\"");
            ret = Regex.Replace(ret, @"\r\n\]", "]");
            ret = Regex.Replace(ret, @"\r\n; *", ";");
            Response.Write(ret);
            base.OnLoad(e);
        }

        /// <summary>
        /// This method creates a client proxy
        /// </summary>
        /// <param name="url">string</param>
        /// <returns>string</returns>
        private string CreateClientProxies(string url)
        {
            if ((url != null) && (url.StartsWith("~/")))
            {
                url = Request.ApplicationPath + url.Substring(1);
            }
            if (url.EndsWith(".asmx", StringComparison.InvariantCultureIgnoreCase))
            {
                url = url + "?WSDL";
            }
            Uri uri = new Uri(Request.Url, url);

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(uri);
            req.Credentials = CredentialCache.DefaultCredentials;
            req.Proxy = WebRequest.DefaultWebProxy; // running on the same server !
            req.Timeout = 6 * 1000; // 6 seconds

            WebResponse res = req.GetResponse();
            #if DOTNET11
                            XmlDocument data = new XmlDocument();
                            data.Load(res.GetResponseStream());

                            XslTransform xsl = new XslTransform();
                            xsl.Load(Server.MapPath("~/_layouts/CLIFPages/XSL/wsdl.xslt"));

                            System.IO.StringWriter sOut = new System.IO.StringWriter();
                            xsl.Transform(data, null, sOut, null);
              #else
            XmlReader data = XmlReader.Create(res.GetResponseStream());
                        XslCompiledTransform xsl = new XslCompiledTransform();
                        xsl.Load(Server.MapPath("~/_layouts/CLIFPages/XSL/wsdl.xslt"));
                        System.IO.StringWriter sOut = new System.IO.StringWriter();
                        xsl.Transform(data, null, sOut);
            #endif
            return (sOut.ToString());
        }
    }
}
