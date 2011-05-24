using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using System.Xml;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Publishing;
using System.Diagnostics;

namespace CLIF.Solutions.Code
{
    public class LeftNav : Menu, IXmlDocumentProvider
    {
        private XslTransformControl _transform;
        private bool _addToCache = false;
        private bool _cachePerAuthenticated = false;
        private bool _cachePerPage = true;
        private int _cacheTimeout = 1;
        private string _currentQuerystring = string.Empty;
        private string _qsParamsAdded = string.Empty;

        public bool AddToCache
        {
            get { return _addToCache; }
            set { _addToCache = value; }
        }

        // gets/sets cache timeout period
        public int CacheTimeout
        {
            get { return _cacheTimeout; }
            set { _cacheTimeout = value; }
        }

        // gets/sets whether to cache navigation for each page
        public bool CachePerPage
        {
            get { return _cachePerPage; }
            set { _cachePerPage = value; }
        }

        // gets/sets whether to cache navigation for all authenticated users
        public bool CachePerAuthenticated
        {
            get { return _cachePerAuthenticated; }
            set { _cachePerAuthenticated = value; }
        }

        // returns the XSLTTransformControl associated with this control
        public XslTransformControl Transform
        {
            get
            {
                if (_transform == null)
                {
                    _transform = new XslTransformControl(this.UniqueID);
                }
                return _transform;
            }
        }

        // configures any controls created by this control
        protected override void CreateChildControls()
        {
            Controls.Add(Transform);
            base.CreateChildControls();
        }

        // transforms the XML document using the XSLTransform control and renders it as HTML
        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            this.MaximumDynamicDisplayLevels = 7;
            try
            {
                _transform.Document = BuildXmlDocument();
                Transform.RenderControl(writer);
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Failed to render navigation: " + ex.Message);
                Trace.WriteLine(ex);
            }
        }

        protected override void OnMenuItemDataBound(MenuEventArgs e)
        {
            base.OnMenuItemDataBound(e);
        }

        // returns the key used for caching navigation
        protected virtual string CacheKey
        {
            get
            {
                return "CLIF_NAV"
                    + ((CachePerPage) ? HttpContext.Current.Request.Url.PathAndQuery.ToLower() : SPContext.Current.Web.Url.ToLower())
                    + ((CachePerAuthenticated) ? HttpContext.Current.User.Identity.IsAuthenticated.ToString() : "");
            }
        }

        // returns the control's XML document
        public XmlDocument Document
        {
            get { return BuildXmlDocument(); }
        }

        // builds the XML document which will be the input to the XSLT transformation
        protected XmlDocument BuildXmlDocument()
        {
            // Get the XML from cache, cached by region and area
            object cacheData = HttpContext.Current.Cache[CacheKey];

            if (cacheData != null && cacheData is XmlDocument && !SPContext.Current.Web.UserIsWebAdmin)
            {
                return (XmlDocument)cacheData;
            }

            XmlDocument document = new XmlDocument();
            document.LoadXml("<Navigation/>");

            // get URL of current site which will be used to determine path through the site
            Uri currentWebUrl = new Uri(SPContext.Current.Web.Url.ToLower());
            string absolutePath = currentWebUrl.AbsolutePath.ToLower();

            XmlAttribute absPathAttr = document.CreateAttribute("AbsolutePath");
            absPathAttr.InnerText = absolutePath;
            document.DocumentElement.Attributes.Append(absPathAttr);

            // add each navigation item from the navigation provider to the navigation XML
            foreach (MenuItem item in Items)
            {
                if (item.Text == "Lists")
                {
                    AddItemToXml(document.DocumentElement, item, absolutePath, string.Empty);
                }
            }

            // Cache the XML 
            if (_addToCache)
            {
                HttpContext.Current.Cache.Add(CacheKey, document, null, DateTime.Now.AddMinutes(CacheTimeout), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.AboveNormal, null);
            }
            return document;
        }

        // reads each navigation item from the navigation provider and builds up an XML structure
        // representing the navigation
        protected void AddItemToXml(XmlElement oParentElement, MenuItem oItem, string sAbsolutePath, string currentQueryString)
        {
            try
            {
                // create XML element for this navigation item
                XmlElement oNewItem = oParentElement.OwnerDocument.CreateElement("Item");
                oParentElement.AppendChild(oNewItem);

                if (oItem.Text.ToLower() == "error")
                {
                    _addToCache = false;
                }

                // store title and URL for use later
                string title = HttpUtility.HtmlDecode(oItem.Text);
                string url = oItem.NavigateUrl.ToLower();

                // keep track of current querystring
                string queryString = string.Empty;
                if (!string.IsNullOrEmpty(currentQueryString))
                    queryString = currentQueryString;

                // track querystring parameters that we add - we need to remove them again once we've
                // processed the current item's children, or they will be persisted throughout the
                // navigation tree
                string qsParamsAdded = string.Empty;

                // set the attributes of the XML element for the current item
                oNewItem.SetAttribute("Title", title);
                oNewItem.SetAttribute("Selected", oItem.Selected.ToString());
                oNewItem.SetAttribute("Url", url + queryString);
                bool isCurrent = sAbsolutePath.ToLower().StartsWith(oItem.DataPath.ToLower());
                oNewItem.SetAttribute("Current", isCurrent.ToString());

                // process the current item's children
                foreach (MenuItem i in oItem.ChildItems)
                {
                    AddItemToXml(oNewItem, i, sAbsolutePath, queryString);
                }
            }
            catch (Exception ex)
            {
                AddToCache = false;
                Trace.WriteLine("Failed to add menu item");
                Trace.WriteLine(ex);
            }
        }

        protected string AddToQueryString(string currentQueryString, ref string qsParamsAdded, string param, string value)
        {
            if (Page.Request.QueryString[param] != null)
            {
                if (currentQueryString.Length == 0)
                    currentQueryString = "?";
                else
                    currentQueryString += "&";
                currentQueryString += param + "=" + value;

                // keep track of parameters added to the querystring, as they will need to be removed
                // once we've processed the current item's children, or they will be persisted 
                // throughout the navigation tree
                if (qsParamsAdded.Length != 0)
                    qsParamsAdded = ",";
                qsParamsAdded += param;
            }
            return currentQueryString;
        }

        protected void RemoveQSParams(string qsParamsAdded)
        {
            char[] commaSep = { ',' };
            foreach (string paramName in qsParamsAdded.Split(commaSep, StringSplitOptions.RemoveEmptyEntries))
            {
                Page.Request.QueryString.Remove(paramName);
            }
        }
        protected void handleBadId()
        {
            HttpContext.Current.Response.Redirect("/aboutthissite/pages/pagenotfound.aspx");
        }
    }
}
