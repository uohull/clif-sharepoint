using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.SharePoint.WebControls;
using System.Xml;
using System.Xml.Xsl;
using System.Web.UI;
using Microsoft.SharePoint;
using System.Web;
using System.Diagnostics;
namespace CLIF.Solutions.Code
{
    public class XmlBasedControl : TemplateBasedControl, IXmlDocumentProvider
    {
        private XslTransformControl _transform;
        private XmlDocument _document;
        private bool _useDocumentCaching = false;
        private int _cachingTimeout = 5;
        private bool _cachePerUser = false;
        private bool _cahePerAuthenticated = false;
        private bool _showxml = false;
        public bool CahePerAuthenticated
        {
            get { return _cahePerAuthenticated; }
            set { _cahePerAuthenticated = value; }
        }
        public bool CachePerUser
        {
            get { return _cachePerUser; }
            set { _cachePerUser = value; }
        }

        public virtual int CachingTimeout
        {
            get { return _cachingTimeout; }
            set { _cachingTimeout = value; }
        }
        public bool UseDocumentCaching
        {
            get { return _useDocumentCaching; }
            set { _useDocumentCaching = value; }
        }
        public bool ShowXml
        {
            get
            {
                // Override setting if parameter present in querystring
                if (HttpContext.Current.Request.QueryString["showcontrolxml"] != null)
                {
                    string controlid = HttpContext.Current.Request.QueryString["showcontrolxml"];
                    if (controlid != null)
                    {
                        if (controlid.ToLower() == "all")
                        {
                            return true;
                        }

                        if (this.ID != null)
                        {
                            if (controlid.ToLower() == this.ID.ToLower())
                            {
                                return true;
                            }
                        }
                    }
                }
                return _showxml;
            }
            set 
            { 
                _showxml = value; 
            }
        }
        public XslTransformControl Transform
        {
            get 
            {
                if (_transform == null)
                {
                    _transform = new XslTransformControl(this.UniqueID);
                    _transform.ModifyXsltArguments += new ModifyXsltArgumentsEvent(OnModifyXsltArguments);
                }
                return _transform;
            }
        }

        public void EnsureDocument()
        {
            if (_document == null)
            {
                if (DocumentCached()) return;
                _document = BuildDocument();
                CacheDocument();
            }
        }

        protected virtual void CacheDocument()
        {
            if (UseDocumentCaching)
                HttpContext.Current.Cache.Add(CacheKey, _document, null, DateTime.Now.AddMinutes(CachingTimeout), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.Low, null);
        }
        protected virtual bool DocumentCached()
        {
            // Let Admins see immediate changes
            if (SPContext.Current.Web.UserIsSiteAdmin) return false;

            object o = HttpContext.Current.Cache[CacheKey];
            if (o != null && o is XmlDocument)
            {
                _document = (XmlDocument)o;
                return true;
            }

            return false;
        }

        public XmlDocument Document
        {
            get
            {
                EnsureDocument();
                return _document;
            }
            set
            {
                _document = value;
            }
        }

        protected void OnModifyXsltArguments(XsltArgumentList arguments)
        {
            ModifyXsltArguments(arguments);
        }

        protected virtual void ModifyXsltArguments(XsltArgumentList arguments)
        {
        }

        protected virtual string CacheKey
        {
            get
            {
                return "ACXml:" 
                       + UniqueID 
                       + ((CachePerUser) ? HttpContext.Current.User.Identity.Name : "") 
                       + ((CahePerAuthenticated) ? HttpContext.Current.User.Identity.IsAuthenticated.ToString() : "")
                       + HttpContext.Current.Request.Url.PathAndQuery;
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            try
            {
                EnsureDocument();

                Transform.Document = Document;
                Transform.RenderControl(writer);

                base.Render(writer);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
            }
        }

        protected virtual XmlDocument BuildDocument()
        {
            try
            {
                SecurityHelper.SetDocumentDelegate del = new SecurityHelper.SetDocumentDelegate(this.SetDocument);
                SecurityHelper.InvokeSetDocument(del);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
            }
            return Document;
        }
        protected virtual void SetDocument()
        {
        }
        protected void FillElementFromListItem(XmlElement element, SPListItem item)
        {
            foreach (SPField oField in item.Fields)
            {
                XmlElement oNew = element.OwnerDocument.CreateElement(oField.InternalName);
                oNew.InnerText = item[oField.Id].ToString();
                element.AppendChild(oNew);
            }
        }
    }
}
