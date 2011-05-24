using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Xsl;

using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Diagnostics;

namespace CLIF.Solutions.Code
{
    public delegate void ModifyXsltArgumentsEvent(XsltArgumentList arguments);

    public class XslTransformControl : SPControl
    {
        private string _xslName;
        private XmlDocument _document;
        private string _xslLibrary = "/Style Library/CLIF/KCL/XSL/";
        private string _namedParameters;
        private bool _cachePerUser = false;
        private int _cacheDuration = 2;
        private string _hideFromAgents = "";
        private bool _showxml = false;

        public string HideFromAgents
        {
            get { return _hideFromAgents; }
            set { _hideFromAgents = value; }
        }

        public int CacheDuration
        {
            get { return _cacheDuration; }
            set { _cacheDuration = value; }
        }

        private bool _cachePerAuthenticated = false;

        public bool CachePerAuthenticated
        {
            get { return _cachePerAuthenticated; }
            set { _cachePerAuthenticated = value; }
        }

        public bool CachePerUser
        {
            get { return _cachePerUser; }
            set { _cachePerUser = value; }
        }

        private bool _cacheOutput = false;

        public bool CacheOutput
        {
            get { return _cacheOutput; }
            set { _cacheOutput = value; }
        }

        public string NamedParameters
        {
            get { return _namedParameters; }
            set { _namedParameters = value; }
        }

        public bool ShowXml
        {
            get { return _showxml; }
            set { _showxml = value; }
        }

        public XslTransformControl(string id)
        {
            this.ID = id;
        }

        public ModifyXsltArgumentsEvent ModifyXsltArguments;

        public string XslLibrary
        {
            get { return _xslLibrary; }
            set { _xslLibrary = value; }
        }

        public XmlDocument Document
        {
            get { return _document; }
            set { _document = value; }
        }

        public string XslName
        {
            get { return _xslName; }
            set { _xslName = value; }
        }

        protected string XslCacheKey
        {
            get
            {
                return "ACXsl:" + XslLibrary + XslName;
            }
        }

        protected string CacheKey
        {
            get
            {
                return "ACHtmlFragment:"
                       + UniqueID + XslLibrary + XslName
                       + ((CachePerUser) ? HttpContext.Current.User.Identity.Name : "")
                       + ((CachePerAuthenticated) ? HttpContext.Current.User.Identity.IsAuthenticated.ToString() : "")
                       + HttpContext.Current.Request.Url.PathAndQuery;
            }
        }

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            try
            {
                if (Utility.HtmlIsHidden(HideFromAgents)) 
                    return;
                if (WriteCachedOuput(writer)) 
                    return;
                
                if (string.IsNullOrEmpty(XslName))
                {
                    Trace.WriteLine("No XSL name specified");
                    return;
                }

                if (XslName.ToLower() == "blank.xsl") 
                    return;

                if (Document == null)
                {
                    Trace.WriteLine(CacheKey + ": Document is NULL...Unable to render");
                    return;
                }

                if (this.ShowXml)
                {
                    TextBox showxml = new TextBox();
                    showxml.EnableViewState = false;
                    showxml.ID = this.UniqueID + "_showxml";
                    showxml.TextMode = TextBoxMode.MultiLine;
                    showxml.Wrap = true;
                    showxml.Rows = 10;
                    showxml.Columns = 30;
                    XmlDocument showXmlDoc = (XmlDocument)this.Document;
                    showxml.Text = showXmlDoc.InnerXml;
                    showxml.CssClass = "showxml";
                    showxml.RenderControl(writer);
                }
                else
                {
                    XmlDocument oXsl = new XmlDocument();
                    object oXslTransform = null;
                    
                    if (!SPContext.Current.Web.UserIsWebAdmin)
                        oXslTransform = HttpContext.Current.Cache[XslCacheKey];

                    XslCompiledTransform oTransform = null;

                    if (oXslTransform == null)
                    {
                        string sXSLLibrary = XslLibrary;
                        string sXslString = SPContext.Current.Site.RootWeb.GetFileAsString(sXSLLibrary + XslName);
                        oXsl.LoadXml(sXslString);

                        oTransform = new XslCompiledTransform();
                        oTransform.Load(oXsl);

                        HttpContext.Current.Cache.Add(XslCacheKey, oTransform, null, 
                            DateTime.Now.AddMinutes(this.CacheDuration), 
                            System.Web.Caching.Cache.NoSlidingExpiration, 
                            System.Web.Caching.CacheItemPriority.AboveNormal, null);
                    }
                    else
                        oTransform = (XslCompiledTransform)oXslTransform;

                    if (CacheOutput)
                        TransformAndCache(writer, oTransform);
                    else
                    {
                        if (System.Configuration.ConfigurationManager.AppSettings["EncryptXSL"] != null && System.Configuration.ConfigurationManager.AppSettings["EncryptXSL"].ToString().Contains(XslName.ToLower()))
                        {
                            System.IO.StringWriter sw = new System.IO.StringWriter();
                            HtmlTextWriter tw = new HtmlTextWriter(sw);

                            oTransform.Transform(Document, BuildArguments(), tw);

                            //Encrypt
                            string enc = Utility.EncryptText(sw.ToString());
                            writer.Write(enc);
                        }
                        else
                        {
                            oTransform.Transform(Document, BuildArguments(), writer);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(XslCacheKey);
                Trace.Write(ex);
            }
            
            base.Render(writer);
        }

       
        private void TransformAndCache(System.Web.UI.HtmlTextWriter writer, XslCompiledTransform oTransform)
        {
            System.IO.StringWriter sw = new System.IO.StringWriter();
            HtmlTextWriter tw = new HtmlTextWriter(sw);

            oTransform.Transform(Document, BuildArguments(), tw);

            string sResponse = "<!-- Cached at: " + DateTime.Now.ToString() + " -->" + Environment.NewLine;
            if (System.Configuration.ConfigurationManager.AppSettings["EncryptXSL"] != null && System.Configuration.ConfigurationManager.AppSettings["EncryptXSL"].ToString().Contains(XslName.ToLower()))
            {
                string enc = Utility.EncryptText(sw.ToString());
                sResponse += enc;
            }
            else
            {
                sResponse += sw.ToString();
            }
                //Encrypt
                //End encryption
                HttpContext.Current.Cache.Add(CacheKey, sResponse, null, DateTime.Now.AddMinutes(CacheDuration), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.Normal, null);
            writer.Write(sResponse);
        }

        private bool WriteCachedOuput(System.Web.UI.HtmlTextWriter writer)
        {
            if (CacheOutput && !SPContext.Current.ListItem.DoesUserHavePermissions(SPBasePermissions.EditListItems))
            {
                string sResponse = (string)HttpContext.Current.Cache[CacheKey];
                if (sResponse != null)
                {
                    writer.Write(sResponse);
                    Trace.WriteLine("Using Cached Content");
                    return true;
                }
            }

            return false;
        }

        protected virtual XsltArgumentList BuildArguments()
        {
            XsltArgumentList args = new XsltArgumentList();

            args.AddExtensionObject("http://exslt.org/dates-and-times", new Mvp.Xml.Exslt.ExsltDatesAndTimes());
            args.AddExtensionObject("http://exslt.org/strings", new Mvp.Xml.Exslt.ExsltStrings());
            args.AddExtensionObject("http://exslt.org/math", new Mvp.Xml.Exslt.ExsltMath());
            args.AddExtensionObject("http://exslt.org/sets", new Mvp.Xml.Exslt.ExsltSets());

            if (!string.IsNullOrEmpty(NamedParameters))
            {
                string[] arItems = NamedParameters.Split(',');
                foreach (string sParameter in arItems)
                {
                    string[] arBits = sParameter.Split('=');
                    args.AddParam(arBits[0], "", arBits[1]);
                }
            }

            //args.AddParam("exclude", "", SPContext.Current.ListItem.UniqueId.ToString());

            try
            {
                foreach (string sKey in HttpContext.Current.Request.QueryString.AllKeys)
                    args.AddParam("qs-" + sKey, "", HttpContext.Current.Request[sKey]);
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Failed to add QS parameter: " + ex.Message);
            }

            HttpContext.Current.Items["ACXslBaseArgs"] = args;

            if (ModifyXsltArguments != null) ModifyXsltArguments(args);

            return args;
        } 

        public static void CleanNamespaces(XmlDocument xml)
        {
            XslCompiledTransform oTransform = (XslCompiledTransform)HttpContext.Current.Cache["AC-CleanNamespaces"];

            if (oTransform == null)
            {
                string sXslString = SPContext.Current.Site.RootWeb.GetFileAsString("/Style Library/XSL Style Sheets/Controls/CleanNamespaces.xsl");
                XmlDocument oXsl = new XmlDocument();
                oXsl.LoadXml(sXslString);

                oTransform = new XslCompiledTransform();
                oTransform.Load(oXsl);

                // Cache for 10 hours...this never really changes
                HttpContext.Current.Cache.Add("AC-CleanNamespaces", oTransform, null, DateTime.Now.AddMinutes(600), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.AboveNormal, null);
            }

            System.IO.StringWriter sw = new System.IO.StringWriter();
            
            XmlTextWriter writer = new XmlTextWriter(sw);
            oTransform.Transform(xml, null, writer);
            xml.LoadXml(sw.ToString()); 
        }
    }
}
