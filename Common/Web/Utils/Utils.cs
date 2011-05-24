using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace Common.Web.Utils
{
    /// <summary>
    /// Class HtmlUtils. contains miscellaneous utilities
    /// </summary>
    public static class HtmlUtils
    {
        /// <summary>
        /// Encodes a string value in say code behind to be valid in a client javascript string literal;
        /// </summary>
        /// <param name="valueToEncode">the string To Encode</param>
        /// <returns></returns>
        static public string EncodeJavascriptStringLiteral(string valueToEncode)
        {
            string retVal = valueToEncode;

            retVal = retVal.Replace("\\", "\\\\");
            retVal = retVal.Replace("\"", "\\\"");
            retVal = retVal.Replace("'", "\\\'");
            retVal = retVal.Replace("\r", "\\\r");
            retVal = retVal.Replace("\n", "\\\n");
            retVal = retVal.Replace("\t", "\\\t");

            return retVal;
        }


        /// <summary>
        /// Used for example in content disposition header values e.g filename = XXX
        /// invented from no real specification ..tested working in IE7, Firefox 2.0 and safari..good luck
        /// </summary>
        /// <param name="context">a HttpContext</param>
        /// <param name="headerValue">the value to encode</param>
        /// <returns></returns>
        static public string EncodeHTMLHeaderValue(HttpContext context, string headerValue)
        {
            string encoded = "";

            if (context.Request.Browser.Browser == "IE")
            {
                encoded = Uri.EscapeDataString(headerValue);
            }
            else
            {
                encoded = "\"" + headerValue + "\"";
            }

            return encoded;

        }

        /// <summary>/// 
        /// Returns a site relative HTTP path from a partial path starting out with a ~./// 
        /// Same syntax that ASP.Net internally supports but this method can be used/// 
        /// outside of the Page framework./// 
        /// Works like Control.ResolveUrl including support for ~ syntax///
        /// but returns an absolute URL.///
        /// </summary>/// 
        /// <param name="originalUrl">Any Url including those starting with ~</param>/// 
        /// <returns>relative url</returns>/// 
        public static string ResolveUrl(string originalUrl)
        {    
            if (originalUrl == null)        
                return null;     // *** Absolute path - just return    
            if (originalUrl.IndexOf("://") != -1)        
                return originalUrl;     // *** Fix up image path for ~ root app dir directory    
            if (originalUrl.StartsWith("~"))    
            {        
                string newUrl = "";        
                if (HttpContext.Current != null)            
                    newUrl = HttpContext.Current.Request.ApplicationPath +  originalUrl.Substring(1).Replace("//", "/");        
                else            // *** Not context: assume current directory is the base directory            
                    throw new ArgumentException("Invalid URL: Relative URL not allowed.");   // *** Just to be sure fix up any double slashes        
                return newUrl;    
            }     
            return originalUrl;
        } //You call it just like you would Control.ResolveUrl() and it will return an Application relative path. 

        //Another related scenario is to resolve URLs into fully qualified absolute URLs. For example, in several applications I have callback URLs that get passed to various services like PayPal and Trackback services, that are supposed to call back your application. These pages need to get a fully qualified URL so they can find your page on the Web. Another scenario where I found this necessary is when you need to switch URLs into ssl as part of your application. For example, in my store you enter as a non-SSL URL until you hit the order pages at which point the page switches to SSL and you have to provide a fully qualified URL to do so.

        /// <summary>/// 
        /// This method returns a fully qualified absolute server Url which includes///
        /// the protocol, server, port in addition to the server relative Url.///
        /// Works like Control.ResolveUrl including support for ~ syntax/// 
        /// but returns an absolute URL./// 
        /// </summary>/// 
        /// <param name="ServerUrl">Any Url, either App relative or fully qualified</param>///
        /// <param name="forceHttps">if true forces the url to use https</param>/// 
        /// <returns></returns>
        public static string ResolveServerUrl(string serverUrl, bool forceHttps)
        {    // *** Is it already an absolute Url?    
            if (serverUrl.IndexOf("://") > -1)        
                return serverUrl;     // *** Start by fixing up the Url an Application relative Url    
            string newUrl = ResolveUrl(serverUrl);     
            Uri originalUri = HttpContext.Current.Request.Url;    
            newUrl = (forceHttps ? "https" : originalUri.Scheme) + "://" + originalUri.Authority + newUrl;     
            return newUrl;
        }  

        /// <summary>/// 
        /// This method returns a fully qualified absolute server Url which includes///
        /// the protocol, server, port in addition to the server relative Url.///
        /// It work like Page.ResolveUrl, but adds these to the beginning./// 
        /// This method is useful for generating Urls for AJAX methods/// 
        /// </summary>/// 
        /// <param name="ServerUrl">Any Url, either App relative or fully qualified</param>///
        /// <returns></returns>
        public static string ResolveServerUrl(string serverUrl)
        {    
            return ResolveServerUrl(serverUrl, false);
        }

    }
}
