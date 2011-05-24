using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.SharePoint.WebControls;
using System.Xml.Xsl;
using System.Xml;
using System.Web.UI;
using System.Web;
using Microsoft.SharePoint;

namespace CLIF.Solutions.Code
{
    public class XmlBaseFieldControl : BaseFieldControl, IXmlDocumentProvider
    {
        private XslTransformControl _transform;
        private XmlDocument _document;
        private bool _useSharePointCaching = true;

        public bool UseSharePointCaching
        {
            get { return _useSharePointCaching; }
            set { _useSharePointCaching = value; }
        }

        protected override void OnInit(EventArgs e)
        {
            CanCacheRenderedFieldValue = UseSharePointCaching;
            
            if (ControlMode != SPControlMode.Display)
                EnableViewState = true;
            else
                EnableViewState = false;

            base.OnInit(e);
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

        protected virtual ToolBar AddToolbar(Control parent)
        {
            ToolBar tb = (ToolBar)Page.LoadControl("~/_controltemplates/ToolBar.ascx"); tb.ID = "Toolbar";
            parent.Controls.Add(tb);

            return tb;
        }

        protected virtual ToolBarButton AddToolbarButton(ToolBar parent, string title, string ID)
        {
            ToolBarButton btn = (ToolBarButton)Page.LoadControl("~/_controltemplates/ToolBarButton.ascx");
            parent.Buttons.Controls.Add(btn);

            btn.Text = title;
            btn.ID = ID;
            btn.OnClientClick = "g_bWarnBeforeLeave = false";

            return btn;
        }

        protected virtual void OnModifyXsltArguments(XsltArgumentList arguments)
        {
        }

        protected override void RenderFieldForDisplay(System.Web.UI.HtmlTextWriter output)
        {
            //base.RenderFieldForDisplay(output);
            Transform.Document = Document;
            Transform.RenderControl(output);
        }

        protected virtual string CacheKey
        {
            get
            {
                return "ACFieldXml:" + UniqueID + HttpContext.Current.Request.Url.PathAndQuery;
            }
        }

        public void EnsureDocument()
        {
            if (_document == null)
            {
                if (DocumentCached()) return;
                _document = GetDocument();
                CacheDocument();
            }
        }

        protected virtual void CacheDocument()
        {
            HttpContext.Current.Cache.Add(CacheKey, _document, null, DateTime.Now.AddMinutes(5), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.Low, null);
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

        protected virtual XmlDocument GetDocument() { return null; }

        #region IXmlDocumentProvider Members

        public XmlDocument Document
        {
            get { EnsureDocument(); return _document; }
        }

        #endregion
    }
}
