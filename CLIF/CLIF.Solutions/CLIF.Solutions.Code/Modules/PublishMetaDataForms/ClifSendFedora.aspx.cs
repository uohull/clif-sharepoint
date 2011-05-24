using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.Office.InfoPath.Server.Controls;
using System.Xml;
using uk.ac.hull.repository.hydranet.hydracontent.metadata;
using uk.ac.hull.repository.hydranet.hydracontent;
using uk.ac.hull.repository.hydranet.service;
using System.Xml.Schema;
using Microsoft.SharePoint;
using System.IO;
using System.Xml.XPath;
using Common.Web.Utils;
using System.Text.RegularExpressions;
using System.Diagnostics;
using Microsoft.SharePoint.WebControls;
using System.Collections.Specialized;
using CLIF.Solutions.Code;
using uk.ac.hull.repository.utils;
using System.Drawing;
using System.Xml.Linq;
using System.Web.Services;
using AjaxControlToolkit;
using System.Web.Script.Services;
//using System.Diagnostics;

[assembly: System.Web.UI.WebResource("CLIF.Solutions.Code.Modules.PublishMetaDataForms.Resources.Javascript.MDHelper.js", "text/javascript")]

namespace CLIF.Solutions.Code
{
    /// <summary>
    /// ClifSendFedora. The basic design philosophy behind this sharepoint code behind class is to load User Controls (or infopath forms) dynamically onto the page
    /// depending on the meta data option checkboxes the user selects. Each User control builds a complete MetaData form. JQuery helper javascript libraries build the 
    /// complete XML for each form. User Controls can utilise tha full range of avaiable ajax controls (e.g DatePickers / ComboBoxes). 
    /// Prior to page postback some JQuery local field validation is performed for certain fields (e.g name fields).
    /// Initial postback is done asynchronously (via ajax/ASP.NET client script postback) and validates all metadata fields against any relevant XSD schemas (See Hydranet IMetadata constructors).  
    /// Validation errors attempt to highlight the relevant form fields (infopath forms contain their own office server routines for highlighting errors).
    /// If all forms validate ok a 2nd full page postback occurs and the DepositDocument method gets invoked to send the document to the Fedora repository along with the metadata streams.
    /// </summary>
    public partial class ClifSendFedora : LayoutsPageBase, ICallbackEventHandler
    {
        #region EventHandlers

        /// <summary>
        /// OnPreInit. Page pre-initialisation actions. Gets Sharepoint context values and adds the required metadata form 
        /// user controls.
        /// </summary>
        /// <param name="e">see MSDN documentation</param>
        protected override void OnPreInit(EventArgs e)
        {
            try
            {

                GetSPValues();
                AddBasicDC();
                if (PostbackMDCtrls.Contains("ckshowIPMODs"))
                {
                    PopulateMDFields(false);
                    AddXmlFormView("~sitecollection/FormServerTemplates/Mods.xsn");  //  infopath form view is populated in XmlFormView1_Initialize
                }
                else
                {
                    AddBasicMods();
                    PopulateMDFields(true);
                }

            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("CLIFSendFedora.aspx: OnPreInit:" + ex.ToString());
            }
            base.OnPreInit(e);
        }

        /// <summary>
        /// OnPreInit. Page on initialisation actions. Adds any javascript libraries required by the page. 
        /// user controls.
        /// </summary>
        /// <param name="e">see MSDN documentation</param>
        protected override void OnInit(EventArgs e)
        {
            this.ClientScript.RegisterClientScriptInclude("MDHelper", this.ClientScript.GetWebResourceUrl(typeof(ClifSendFedora), "CLIF.Solutions.Code.Modules.PublishMetaDataForms.Resources.Javascript.MDHelper.js"));
            base.OnInit(e);
        }

        /// <summary>
        /// Page_Load. Page on load actions. Defines the main page flow 
        /// user controls.
        /// </summary>
        /// <param name="e">see MSDN documentation</param>
        /// <param name="sender">see MSDN documentation</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            SetCallBackScripts();
            if (!Request.Browser.Cookies)
                throw new BrowserSupportException("enable browser cookie support");

            if (Request.Browser.EcmaScriptVersion.Major < 1)
                throw new BrowserSupportException("enable browser javascript support");

            Page.DataBind();
            if (!IsPostBack)
            {
                pnlAddDC.Attributes.Add("style", " visibility:hidden");
                Session["SPListId"] = Request.QueryString["ListId"];
                Session["SPItemId"] = Request.QueryString["ItemId"];
            }

            else
            {
                // string ctrl = Request.Params["__EVENTTARGET"];
                //if (!CheckNotRepeated())
                //    return;   // just return 200 OK might be a repeated AJAX request or DOS attack
                if (PostbackMDCtrls.Contains("ckshowIPMODs"))
                {
                    ckShowBasicMODs.Checked = false;
                }
                else if (PostbackMDCtrls.Contains("ckShowBasicMODs"))
                {
                    ckshowIPMODs.Checked = false;
                }

                if (IsSubmitRepository)
                {
                    GatherAndSendMetaData(false);
                }
            }
        }

        /// <summary>
        /// OnPreRender. Page pre render actions. Binds page data variables and registers any normal page javascript functions to 
        /// allow the page to refresh normally.
        /// user controls.
        /// </summary>
        /// <param name="e">see MSDN documentation</param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (!_successfulUpload)
            {
                ((HtmlInputHidden)FindCtrl("txtPostBackControl")).Value = "";
                Page.DataBind();
                string script = "javascript:setTimeout('RestoreVisibleForms()',100);";
                ClientScript.RegisterStartupScript(this.GetType(), "PostBackReturnFunc", script, true);

            }
        }

        /// <summary>
        /// RaiseCallbackEvent. Receives any data from the .NET ClientScript (AJAX) callback function 'CallServer'.
        /// </summary>
        /// <param name="eventArgument">the form post data array from the client page</param>
        void ICallbackEventHandler.RaiseCallbackEvent(string eventArgument)
        {
            System.Diagnostics.Trace.WriteLine("ICallbackEventHandler.RaiseCallbackEvent");
            _clientPostBackFormData = eventArgument;
            _clientReturnFunc = "";
            GatherAndSendMetaData(true);
        }

        /// <summary>
        /// GetCallbackResult. Responds back to the .NET ClientScript (AJAX) callback function 'CallServer' with 
        /// any required client javascript e.g. error message routines.
        /// </summary>
        /// <returns></returns>
        string ICallbackEventHandler.GetCallbackResult()
        {
            return _clientReturnFunc;
        }

        /// <summary>
        /// XmlFormView1_Initialize. Populates the XMLFormView Webpart with initial document metadata obtained from sharepoint 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void XmlFormView1_Initialize(object sender, InitializeEventArgs e)
        {
            XPathNavigator xNavMain = XmlFormView1.XmlForm.MainDataSource.CreateNavigator();
            XmlNamespaceManager xNameSpace = new XmlNamespaceManager(new NameTable());

            xNameSpace.AddNamespace("mods", "http://www.loc.gov/mods/v3");
            xNameSpace.AddNamespace("xlinq", "http://www.w3.org/1999/xlink");

            XPathNavigator fTitle = xNavMain.SelectSingleNode(
                "mods:modsCollection[1]/mods:mods/mods:titleInfo/mods:title", xNameSpace);

            fTitle.SetValue(_itemTitle);

            if (!String.IsNullOrEmpty(_mimeType))
            {
                XPathNavigator fmediaType = xNavMain.SelectSingleNode(
                    "mods:modsCollection[1]/mods:mods/mods:physicalDescription/mods:internetMediaType", xNameSpace);
                if (fmediaType != null)
                    fmediaType.SetValue(_mimeType);
            }

            XPathNavigator fextent = xNavMain.SelectSingleNode(
                "mods:modsCollection[1]/mods:mods/mods:physicalDescription/mods:extent", xNameSpace);
            if (fextent != null)
                fextent.SetValue(_file.TotalLength.ToString() + " bytes");

            string dateIssued = "";
            if (_fileDate != "")
                dateIssued = _fileDate;
            else
                dateIssued = DateTime.Now.ToString("yyyy-MM-dd");

            XPathNavigator fdateIssued = xNavMain.SelectSingleNode(
                "mods:modsCollection[1]/mods:mods/mods:originInfo/mods:dateIssued", xNameSpace);
            if (fdateIssued != null)
                fdateIssued.SetValue(dateIssued);

            XPathNavigator fnamePart = xNavMain.SelectSingleNode(
                "mods:modsCollection[1]/mods:mods/mods:name/mods:namePart", xNameSpace);
            if (fnamePart != null)
                fnamePart.SetValue(_author);
        }

        /// <summary>
        /// XmlFormView1_SubmitToHost. On Submit event handler for the XmlFormView control..Does a MODS build validation just to be doubly sure
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void XmlFormView1_SubmitToHost(object sender,
           SubmitToHostEventArgs e)
        {
            // N.B possible to also trigger this process by server button on click()...XmlFormView1.XmlForm.Save() or XmlFormView1.SubmitToHost() ?)

            GetMODS();
        }

        #endregion

        #region Private implementation

        //private const int REPEAT_LOCKOUT_INTERVAL_SEC = 3;
        private SPFile _file = null;
        private string _listTitle = "";
        private string _itemTitle = "";
        private string _siteUrl = "";
        private string _author = "";
        private string _fileDate = "";
        private ImageInfo _imageInfo;
        private bool _isImage = false;
        private bool _successfulUpload = false;
        private SPListItem _item;
        private string _clientReturnFunc = "";
        private string _clientPostBackFormData = "";
        private NameValueCollection _clientPostBackForm;
        private string _mimeType = "";
        protected Control postBackCtrl;
        private EventHandler<InitializeEventArgs> initHandler;

        #region DataTypes

        /// <summary>
        /// ImageInfo. a data object to hold information about photographic images.
        /// </summary>
        private struct ImageInfo
        {
            public int Width;
            public int Height;
        }

        #endregion

        #region Accessors
        /// <summary>
        /// ShowDC. An accessor that determines whether the user has presently elected to show DC metadata 
        /// (complicated by the fact that it needs to work in clientscript postback mode also)
        /// </summary>
        private bool ShowDC
        {
            get
            {
                bool clientDCChecked = false;
                if (ClientPostBackFormData != null)
                {
                    Control ctrl = FindCtrl(ckShowDC.ID);
                    if (ctrl != null)
                    {
                        clientDCChecked = ClientPostBackFormData[ctrl.ClientID.Replace("_", "$")] == "on";
                    }

                    return clientDCChecked;
                }
                return ckShowDC.Checked;
            }
        }

        /// <summary>
        /// ShowMODs. An accessor that determines whether the user has presently elected to show MODS metadata 
        /// (complicated by the fact that it needs to work in clientscript postback mode also)
        /// </summary>
        private bool ShowMODs
        {
            get
            {
                bool clientBasicMODSChecked = false;
                bool clientIPMODSChecked = false;

                if (ClientPostBackFormData != null)
                {
                    Control ctrl = FindCtrl(ckShowBasicMODs.ID);
                    if (ctrl != null)
                    {
                        clientBasicMODSChecked = ClientPostBackFormData[ctrl.ClientID.Replace("_", "$")] == "on";
                    }

                    ctrl = FindCtrl(ckshowIPMODs.ID);
                    if (ctrl != null)
                    {
                        clientIPMODSChecked = ClientPostBackFormData[ctrl.ClientID.Replace("_", "$")] == "on";
                    }

                    return clientBasicMODSChecked || clientIPMODSChecked;
                }

                return ckShowBasicMODs.Checked || ckshowIPMODs.Checked;
            }
        }

        /// <summary>
        /// ClientPostBackFormData. An accessor for the client script post back form data
        /// </summary>
        private NameValueCollection ClientPostBackFormData
        {
            get
            {
                if (_clientPostBackForm == null && _clientPostBackFormData != "")
                {
                    _clientPostBackForm = HttpUtility.ParseQueryString(_clientPostBackFormData);
                }
                return _clientPostBackForm;
            }
        }

        /// <summary>
        /// PostbackMDCtrls. accessor that reads the value of the MD form Options data value
        /// </summary>
        private string PostbackMDCtrls
        {
            get
            {
                string formMDCtrslValue = ((Request.Form[FindCtrl("txtPostbackCtrlIds").ClientID.Replace("_", "$")] ?? "").ToString());
                return formMDCtrslValue;
            }
        }

        /// <summary>
        /// IsSubmitRepository. accessor that returns whether the Copy to repository button initiated the postback
        /// </summary>
        private bool IsSubmitRepository
        {
            get
            {
                bool normalPostBack = txtPostBackControl == null ? !String.IsNullOrEmpty((Request.Form[FindCtrl("txtPostBackControl").ClientID.Replace("_", "$")] ?? "").ToString()) : txtPostBackControl.Value != "";
                return normalPostBack;
            }
        }


        private System.Web.UI.WebControls.PlaceHolder _placeHolderMODs;

        /// <summary>
        /// 
        /// </summary>
        private System.Web.UI.WebControls.PlaceHolder _PlaceHolderMODs
        {
            get
            {
                if (_placeHolderMODs == null)
                {
                    Control ctrl = FindCtrl("PlaceHolderMODs");
                    _placeHolderMODs = ctrl == null ? null : (System.Web.UI.WebControls.PlaceHolder)ctrl;
                }

                return _placeHolderMODs;
            }

        }

        private System.Web.UI.WebControls.PlaceHolder _placeHolderDC;

        protected System.Web.UI.WebControls.PlaceHolder _PlaceHolderDC
        {
            get
            {
                if (_placeHolderDC == null)
                {
                    Control ctrl = FindCtrl("PlaceHolderDC");
                    _placeHolderDC = ctrl == null ? null : (System.Web.UI.WebControls.PlaceHolder)ctrl;
                }

                return _placeHolderDC;
            }

        }

        private XmlFormView xmlFormView1;

        /// <summary>
        /// XmlFormView1. The page XmlFormView.
        /// </summary>
        protected XmlFormView XmlFormView1
        {
            get
            {
                if (xmlFormView1 == null)
                {
                    Control ctrl = FindCtrl("XmlFormView1");
                    xmlFormView1 = ctrl == null ? null : (XmlFormView)ctrl;
                }

                return xmlFormView1;
            }

        }

        #endregion

        #region Methods
        /// <summary>
        /// BrowserSupportException. Custom exception class for browser support incompatibilities
        /// </summary>
        private class BrowserSupportException : Exception
        {
            public BrowserSupportException(string msg) : base(msg) { ;}
        }

        /// <summary>
        /// EscapeJavascriptStringLiteral. Encodes a string for embedding into a javascript literal
        /// </summary>
        /// <param name="str">string to be encoded</param>
        /// <returns></returns>
        private String EscapeJavascriptStringLiteral(String str)
        {
            str = str.Replace("\\", "\\\\");
            str = str.Replace("'", "\\'");
            str = str.Replace("\"", "\\\"");
            str = str.Replace("<", "\\<");
            str = str.Replace(">", "\\>");
            str = str.Replace("\n", " ");
            str = str.Replace("\r", " ");
            return str;
        }

        /// <summary>
        /// EncoodeXMLAttribute. Encodes an xml attribute string for embedding into an XML document
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        private String EncoodeXMLAttribute(string xml)
        {
            return xml.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;");
        }

        /// <summary>
        /// EncoodeXMLElement.  Encodes an xml element string for embedding into an XML document
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        private String EncoodeXMLElement(string xml)
        {
            return xml.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;").Replace("'", "&apos;");
        }

        /// <summary>
        /// PopulateMDFields. Populates any initial form values with information gathered about the document from Sharepoint.
        /// </summary>
        /// <param name="modsBasic"></param>
        private void PopulateMDFields(bool modsBasic)
        {
            DateTime now = DateTime.Now;
            ((HtmlInputText)ctrlBasicDC.FindControl("txt_title_DCBasic")).Value = _itemTitle;
            ((HtmlInputText)ctrlBasicDC.FindControl("txt_creator_DCBasic")).Value = _author;
            TextBox tbDateIssued = ((TextBox)ctrlBasicDC.FindControl("txt_date_DCBasic"));

            if (_fileDate != "")
                tbDateIssued.Text = _fileDate;
            else
                tbDateIssued.Text = now.ToString("yyyy-MM-dd");

            HtmlInputText dcMediaType = null;
            if (!String.IsNullOrEmpty(_mimeType))
            {
                dcMediaType = (HtmlInputText)ctrlBasicDC.FindControl("txt_format_DCBasic");
                string[] mediaTypeArr = _mimeType.Split('/');
                dcMediaType.Value = mediaTypeArr[mediaTypeArr.Length - 1];
                string[] mediaSubTypeArr = mediaTypeArr[mediaTypeArr.Length - 1].Split('.');
                if (mediaSubTypeArr.Length > 1)
                {
                    int nVal = 0;    // now try and make some of the MS office types more friendly e.g application/vnd.ms-excel.sheet.binary.macroEnabled.12
                    if (!Int32.TryParse(mediaSubTypeArr[mediaSubTypeArr.Length - 1], out nVal))   // doesnt  end in a numeric  e.g  .12
                        dcMediaType.Value = mediaSubTypeArr[mediaSubTypeArr.Length - 1];
                    else if (mediaSubTypeArr.Length > 3)
                        dcMediaType.Value = mediaSubTypeArr[mediaSubTypeArr.Length - 4] + "." + mediaSubTypeArr[mediaSubTypeArr.Length - 3];      // none of our present MimeTypeHelper values has 2nd but last part  as a numeric 
                    else
                        dcMediaType.Value = mediaSubTypeArr[mediaSubTypeArr.Length - 2];
                }

                HtmlInputHidden dcMediaTypeHidden = (HtmlInputHidden)ctrlBasicDC.FindControl("hidden_txt_format_DCBasic");
                dcMediaTypeHidden.Value = _mimeType;

            }

            if (modsBasic)
            {
                if ((dcMediaType != null) && !String.IsNullOrEmpty(_mimeType))
                {
                    HtmlInputText modsFormat = (HtmlInputText)ctrlBasicMODs.FindControl("txt_internetMediaType_MODSBasic");
                    modsFormat.Value = dcMediaType.Value;
                    HtmlInputHidden modsFormatHidden = (HtmlInputHidden)ctrlBasicMODs.FindControl("hidden_txt_internetMediaType_MODSBasic");
                    modsFormatHidden.Value = _mimeType;
                }

                HtmlInputText modsExtent = (HtmlInputText)ctrlBasicMODs.FindControl("txt_extent_MODSBasic");
                modsExtent.Value = _file.TotalLength.ToString() + " bytes";

                TextBox dateIssued = (TextBox)ctrlBasicMODs.FindControl("txt_dateIssued_MODSBasic");
                if (_fileDate != "")
                    dateIssued.Text = _fileDate;
                else
                    dateIssued.Text = now.ToString("yyyy-MM-dd");

                HtmlInputText modsTitle = (HtmlInputText)ctrlBasicMODs.FindControl("txt_title_MODSBasic");
                modsTitle.Value = _itemTitle;
                HtmlInputText modsCreator = (HtmlInputText)ctrlBasicMODs.FindControl("rptd_txt_namePart_MODSBasic");
                modsCreator.Value = _author;
            }
        }

        private Control ctrlBasicDC;

        /// <summary>
        /// AddBasicDC. Adds page controls for basic Dublin Core metadata
        /// </summary>
        private void AddBasicDC()
        {
            ctrlBasicDC = Page.LoadControl(@"~/_controltemplates/CLIFUserControls/DCBasic.ascx");
            _PlaceHolderDC.Controls.Add(ctrlBasicDC);
            postBackCtrl = ctrlBasicDC;
        }

        private Control ctrlBasicMODs;

        /// <summary>
        /// AddBasicMods. Adds page controls for basic MODs metadata
        /// </summary>
        private void AddBasicMods()
        {
            HtmlControl ctrl = (HtmlControl)FindCtrl("DivMiddle");
            ctrl.Attributes["class"] = "DivMiddle";
            ctrl = (HtmlControl)FindCtrl("DivTop");
            ctrl.Attributes["class"] = "DivTop";
            ctrl = (HtmlControl)FindCtrl("DivBottom");
            ctrl.Attributes["class"] = "DivBottom";
            _PlaceHolderMODs.Controls.Clear();
            ctrlBasicMODs = Page.LoadControl(@"~/_controltemplates/CLIFUserControls/MODSBasic.ascx");
            _PlaceHolderMODs.Controls.Add(ctrlBasicMODs);
            postBackCtrl = ctrlBasicMODs;
        }

        /// <summary>
        /// AddXmlFormView. Adds XmlFormView webpart to the page for loading Infopath forms into
        /// </summary>
        /// <param name="templatePath"></param>
        private void AddXmlFormView(string templatePath)
        {
            HtmlControl ctrl = (HtmlControl)FindCtrl("DivMiddle");
            ctrl.Attributes["class"] = "DivMiddleIP";
            ctrl = (HtmlControl)FindCtrl("DivTop");
            ctrl.Attributes["class"] = "DivTopIP";
            ctrl = (HtmlControl)FindCtrl("DivBottom");
            ctrl.Attributes["class"] = "DivBottomIP";
            _PlaceHolderMODs.Controls.Clear();
            _PlaceHolderMODs.Controls.Add(FormView(templatePath));
            postBackCtrl = ctrlXmlFormView;
        }

        /// <summary>
        /// GetControl. Recurively searches a control's child controls for a specific ctrl.
        /// </summary>
        /// <param name="ctrl">the control to search</param>
        /// <param name="ctrlId">Id of the control to find</param>
        /// <returns>the found control or null</returns>
        private Control GetControl(Control ctrl, string ctrlId)
        {
            foreach (Control c in ctrl.Controls)
            {
                if (c.ID == ctrlId)
                    return c;
                else
                {
                    if (c.HasControls())
                    {
                        Control c2 = GetControl(c, ctrlId);
                        if (c2 != null)
                            return c2;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// FindCtrl. Finds a control in the page regardless of whether it is part of a Master Page layout or not.
        /// </summary>
        /// <param name="id">Id of the control to find</param>
        /// <returns>the found control or null<</returns>
        private Control FindCtrl(string id)
        {
            Control ctrl = null;
            if (this.Master != null)
            {
                ctrl = this.Master.FindControl("PlaceHolderMain");
                if (ctrl != null)
                {
                    ctrl = GetControl(ctrl, id);
                }
                else
                {
                    ctrl = GetControl((Control)(this.Master), id);
                }
            }
            if (ctrl == null)
            {
                ctrl = GetControl((Control)this, id);
            }

            return ctrl;
        }

        private XmlFormView ctrlXmlFormView;

        /// <summary>
        /// FormView. Returns a populated XmlFormView based on a given path to an Infopath template
        /// </summary>
        /// <param name="templatePath">File path to an infopath template</param>
        /// <returns>The XmlFormView</returns>
        private XmlFormView FormView(string templatePath)
        {
            initHandler = this.XmlFormView1_Initialize;
            if (ctrlXmlFormView == null)
            {
                ctrlXmlFormView = new XmlFormView();
                ctrlXmlFormView.ID = "XmlFormView1";
                ctrlXmlFormView.Height = new Unit("100%");
                ctrlXmlFormView.Width = new Unit("100%");
                ctrlXmlFormView.XsnLocation = templatePath;
                ctrlXmlFormView.ScrollBars = ScrollBars.None;
                ctrlXmlFormView.ShowFooter = false;
                ctrlXmlFormView.ShowHeader = false;
                ctrlXmlFormView.Initialize += initHandler;
            }
            return ctrlXmlFormView;
        }

        /// <summary>
        /// SetCallBackScripts. Sets up a .NET ClientScript (AJAX) callback function in the client script named 'CallServer'.
        /// After the call returns the client script ReceiveServerData() function gets called.
        /// </summary>
        protected void SetCallBackScripts()
        {
            ClientScriptManager cm = Page.ClientScript;
            String cbReference = cm.GetCallbackEventReference(this, "arg",
                "ReceiveServerData", "");
            String callbackScript = "function CallServer(arg, context) {" +
                cbReference + "; }";
            cm.RegisterClientScriptBlock(this.GetType(),
                "CallServer", callbackScript, true);
        }

        /// <summary>
        /// ReportSuccess. informs the user that the document was successfully deposited
        /// </summary>
        private void ReportSuccess()
        {
            Response.Clear();
            HtmlControl DivTop = (HtmlControl)this.FindCtrl("DivTop");
            DivTop.Controls.Clear();
            HtmlControl DivMiddle = (HtmlControl)this.FindCtrl("DivMiddle");
            DivMiddle.Controls.Clear();
            DivTop.Controls.Add(new LiteralControl("<br/><br/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<h3>Item successfully copied to the repository</h3>"));
        }

        /// <summary>
        /// IdentifyErroredField. given a system error message (e.g from xsd validation) tries to infer the name of field that caused the error.
        /// </summary>
        /// <param name="error"></param>
        /// <returns></returns>
        private String IdentifyErroredField(string error)
        {
            string matchExpr = @".*: invalid value for .*?_(?<fieldName>[a-zA-Z]*)_.*?.*";
            Regex regEx = new Regex(matchExpr, RegexOptions.Multiline | RegexOptions.ExplicitCapture);
            MatchCollection matches = regEx.Matches(error);

            if (matches.Count > 0)
                return matches[0].Groups["fieldName"].Value;


            matchExpr = @".*mods/v3:(?<fieldName>[a-zA-Z]*)' element is invalid.*";
            regEx = new Regex(matchExpr, RegexOptions.Multiline | RegexOptions.ExplicitCapture);
            matches = regEx.Matches(error);

            if (matches.Count > 0)
                return matches[0].Groups["fieldName"].Value;


            matchExpr = @"Missing mandatory field '(?<fieldName>[a-zA-Z]*)/?.*";
            regEx = new Regex(matchExpr, RegexOptions.Multiline | RegexOptions.ExplicitCapture);
            matches = regEx.Matches(error);

            if (matches.Count > 0)
                return matches[0].Groups["fieldName"].Value;

            return "";
        }

        /// <summary>
        /// ResetErrorFields. adds client script to reset any presently displayed form errors.
        /// </summary>
        protected void ResetErrorFields()
        {
            _clientReturnFunc += "$(':input[type!=\"hidden\"][type!=\"button\"]').each(function(){$(this).removeClass('errHighlight')});";
        }

        /// <summary>
        /// GetImageInfo. Gathers extra information from image files e.g Height ,Width of the image
        /// </summary>
        /// <returns>an ImageInfo containing the required information</returns>
        private ImageInfo GetImageInfo()
        {
            ImageInfo retVal;
            retVal.Height = 0;
            retVal.Width = 0;

            try
            {
                using (StreamReader strm = new StreamReader(_file.OpenBinaryStream()))
                {
                    System.Drawing.Image img = System.Drawing.Image.FromStream(strm.BaseStream);
                    retVal.Height = img.Height;
                    retVal.Width = img.Width;
                    System.Diagnostics.Trace.WriteLine(img.PropertyItems.ToString());
                }

            }
            catch { }
            return retVal;
        }

        /// <summary>
        /// GatherAndSendMetaData. Builds the required metadata in a pre-check. 
        /// If the build succeeds forces a real page postback rather than AJAX or if 2nd postback invokes the method
        /// to actually deposit the document.
        /// </summary>
        /// <param name="ajaxPostBack"></param>
        protected void GatherAndSendMetaData(bool ajaxPostBack)
        {
            try
            {
                //clear any previous highlighted fields
                ResetErrorFields();

                if (ShowDC)
                {
                    GetDC();
                }

                if (ShowMODs)
                {
                    GetMODS();
                }

                if (ajaxPostBack)   // and no validation exception thrown
                {
                    _clientReturnFunc += "setTimeout('SubmitPage()',200);";
                }
                else DepositDocument();
            }

            catch (Exception ex)
            {
                _clientReturnFunc += "MDHLPRClearValues();";

                if (ex is BrowserSupportException)
                {
                    Response.Clear();
                    Response.Write(string.Format("Please correct the following: {0}", String.IsNullOrEmpty(ex.Message) ? ex.ToString() : ex.Message));
                    Response.End();
                }
                else
                {
                    string exMessage = ex.Message;
                    string errorField = "";
                    if ((!String.IsNullOrEmpty(exMessage)) && exMessage.StartsWith("Missing mandatory field"))
                    {
                        errorField = IdentifyErroredField(ex.Message);
                        if (!String.IsNullOrEmpty(errorField))
                        {
                            exMessage = "Missing mandatory field ";
                            if (errorField.Contains("/"))
                            {
                                string[] arrField = errorField.Split('/');
                                exMessage += arrField[0];
                            }
                            else
                                exMessage += errorField;
                        }
                    }
                    string errorMsg = String.IsNullOrEmpty(exMessage) ? ex.ToString() : exMessage;
                    errorMsg = EscapeJavascriptStringLiteral(string.Format("Please correct the following: {0}", errorMsg));
                    _clientReturnFunc += "window.alert('" + errorMsg + "');";

                    errorField = "";

                    try
                    {
                        errorField = IdentifyErroredField(ex.Message);
                        if (!String.IsNullOrEmpty(errorField))
                        {
                            if (!ex.ToString().Contains(" DC "))
                            {
                                _clientReturnFunc += "$(':input[type!=\"hidden\"][type!=\"button\"]').filter(function(){return (($(this)[0].id + '').indexOf('" + errorField + "_MODSBasic',2) > -1 ? true : false ) }).each(function(){$(this).addClass('errHighlight')});";
                            }
                            if (!ex.ToString().Contains(" MODS "))
                            {
                                _clientReturnFunc += "$(':input[type!=\"hidden\"][type!=\"button\"]').filter(function(){return (($(this)[0].id + '').indexOf('" + errorField + "_DCBasic',2) > -1 ? true : false ) }).each(function(){$(this).addClass('errHighlight')});";
                            }
                        }
                    }
                    catch (Exception) { ;}
                    // now for an ordinary postback include same return script
                    ClientScript.RegisterStartupScript(this.GetType(), "PostBackReturnMsg", _clientReturnFunc, true);

                }
            }

        }

        /// <summary>
        /// CheckNotRepeated() : checks wether or not this request occurs within a predefined interval of the previous request
        /// Uses Session rather than Viewstate to make it more hacker proof
        /// </summary>
        /// <returns></returns>
        //private bool CheckNotRepeated()
        //{

        //    string listId = Request.QueryString["ListId"];
        //    string itemId = Request.QueryString["ItemId"];
        //    DateTime lastAttempt = DateTime.MinValue.ToUniversalTime();
        //    DateTime minDateTime = TimeZoneInfo.ConvertTimeToUtc(DateTime.MinValue);

        //    if (Session["Hydranet_LastAttempt"] != null)
        //        lastAttempt = TimeZoneInfo.ConvertTimeToUtc(new DateTime((long)Session["Hydranet_LastAttempt"]));

        //    DateTime timeNow = DateTime.UtcNow;
        //    if ((lastAttempt != minDateTime) && (listId == (Session["Hydranet_ListId"] ?? "").ToString()) && (itemId == (Session["Hydranet_ItemId"] ?? "").ToString()) && (timeNow.Subtract(lastAttempt).TotalSeconds < REPEAT_LOCKOUT_INTERVAL_SEC))
        //    {
        //        Session["Hydranet_LastAttempt"] = timeNow.ToString();
        //        return false;
        //    }

        //    Session["Hydranet_ListId"] = Request.QueryString["ListId"];
        //    Session["Hydranet_ItemId"] = Request.QueryString["ItemId"];
        //    Session["Hydranet_LastAttempt"] = timeNow.Ticks;

        //    return true;
        //}

        /// <summary>
        /// GetSPValues. Gets sharepoint values and derived values and assigns these to page level variables 
        /// </summary>
        private void GetSPValues()
        {
            using (SPSite siteCollection = SPContext.Current.Site)
            {
                using (SPWeb site = SPContext.Current.Web)
                {

                    string listId = "";
                    string itemId = "";

                    if ((Request.QueryString["ItemId"] ?? "").ToString() == "")
                    {
                        listId = (Session["SPListId"] ?? "").ToString();
                        itemId = (Session["SPItemId"] ?? "").ToString();
                    }

                    if (listId == "")
                    {
                        listId = Request.QueryString["ListId"];
                    }

                    if (itemId == "")
                    {
                        itemId = Request.QueryString["ItemId"];
                    }

                    SPList list = null;

                    try
                    {
//                      list = site.Lists[new Guid(listId)];  pre KINGS integration
                        SPWeb ObjWeb = SPContext.Current.Web;
                        list = ObjWeb.Lists[new Guid(listId)];
                        _siteUrl = site.Url;
                    }
                    catch (Exception ex){

                        Trace.Write(ex.ToString());
                        
                    }

                    if (list == null)
                    {
                        foreach (SPWeb subWeb in site.GetSubwebsForCurrentUser())
                        {
                            try
                            {
                                list = subWeb.Lists[new Guid(listId)];
                                _siteUrl = subWeb.Url;
                            }
                            catch { ;}
                            if (list != null)
                                break;
                        }
                    }

                    _item = list.Items.GetItemById(Convert.ToInt32(itemId));

                    _listTitle = list.Title;
                    _itemTitle = _item.Title.Trim().Length == 0 ? Path.GetFileNameWithoutExtension(_item.Name.Trim()) : _item.Title.Trim();
                    _file = _item.File;

                    _mimeType = MimeTypeHelper.GetContentType(_file.Name);

                    if (_file.TimeCreated != null)
                        _fileDate = _file.TimeCreated.ToString("yyyy-MM-dd");

                    if (_file.Properties["_Author"] != null && (!String.IsNullOrEmpty((string)(_file.Properties["_Author"]))) && (((string)(_file.Properties["_Author"])).Trim() != ""))
                    {
                        _author = ((string)(_file.Properties["_Author"])).Trim();
                    }
                    else if ((_file.Author != null) && !String.IsNullOrEmpty(_file.Author.Name))
                    {
                        _author = _file.Author.Name.Trim();
                    }
                    else
                    {
                        _author = site.CurrentUser.Name;
                    }

                    string[] arrAuthor = _author.Split(' ');
                    if (arrAuthor.Length > 1)
                    {
                        _author = arrAuthor[arrAuthor.Length - 1] + ", ";
                        for (int i = 0; i < arrAuthor.Length - 1; i++)
                        {
                            _author += arrAuthor[i];
                        }
                    }

                    ExtractImageInfo();
                }
            }
        }

        /// <summary>
        /// ExtractImageInfo. Determines if the file is an image and if so extracts addtional information
        /// </summary>
        private void ExtractImageInfo()
        {
            switch (Path.GetExtension(_file.Name).ToLower())
            {
                case ".png":
                case ".emf":
                case ".wmf":
                case ".gif":
                case ".jpeg":
                case ".tiff":
                case ".jpg":
                case ".bmp":
                    {
                        _imageInfo = GetImageInfo();
                        _isImage = true;
                        break;
                    }
            }
        }

        private string _evaluatorPrefix = "";

        /// <summary>
        /// ComboEvaluator. Used in a RegEx replace. Finds the control matching the identifier contained in the Match object and returns its value 
        /// </summary>
        /// <param name="m">Match object from a RegEx operation</param>
        /// <returns>The Combo or Drop down list value corresponding to the disaply text in the match</returns>
        private string ComboEvaluator(Match m)
        {
            string retValue = "";
            string[] nameValue = m.Value.Split('=');
            string ctrlId = nameValue[0].Replace(_evaluatorPrefix, "");
            string value = nameValue[1].Replace("<", "");
            try
            {
                Control combo;
                ListItemCollection items;
                retValue = "";
                if (_evaluatorPrefix.Contains("ddl"))
                {
                    retValue = value;
                }
                else
                {
                    combo = (AjaxControlToolkit.ComboBox)FindCtrl(ctrlId);
                    items = ((AjaxControlToolkit.ComboBox)combo).Items;
                    if (value != "")
                    {
                        retValue = value;
                        try
                        {
                            retValue = items.FindByText(value).Value;
                        }
                        catch { ;}
                    }
                }

                return retValue + "<";
            }
            catch (Exception)
            {
                throw new ArgumentOutOfRangeException("invalid value for " + ctrlId.Replace("rptd_", ""));
            }
        }

        /// <summary>
        /// ProcessComboValues. Any Combo boxes or Drop down lists values in the XML are encoded with the name of the 
        /// field followed by the display text data.
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        private string ProcessComboValues(string xml)
        {
            string comboRegEx = "_!cmb(_|_rptd_)cmb_.*?=.*?<";
            string dropDownListRegEx = "_!ddl(_|_rptd_)ddl_.*?=.*?<";

            MatchEvaluator evaluator = new MatchEvaluator(ComboEvaluator);
            _evaluatorPrefix = "_!cmb_";
            xml = Regex.Replace(xml, comboRegEx, evaluator);

            evaluator = new MatchEvaluator(ComboEvaluator);
            _evaluatorPrefix = "_!ddl_";

            xml = Regex.Replace(xml, dropDownListRegEx, evaluator);

            return xml;
        }

        /// <summary>
        /// GetMODS. Builds a MODs data object from posted back form data (either Infopath or basic MODs form)
        /// </summary>
        /// <returns>a MODSMetadata object</returns>
        private MODSMetadata GetMODS()
        {
            string xml = "";
            if (ckshowIPMODs.Checked)
            {
                XPathNavigator xNavMain =
                   XmlFormView1.XmlForm.MainDataSource.CreateNavigator();

                // strip off the InfoPath header part..leaving pure MODS xml
                int lenPRefix = "<ns1:".Length;
                xml = xNavMain.InnerXml.Substring(xNavMain.InnerXml.IndexOf("modsCollection ", 0) - lenPRefix, xNavMain.InnerXml.Length - xNavMain.InnerXml.IndexOf("modsCollection ", 0) + lenPRefix);
            }
            else if (ckShowBasicMODs.Checked)
            {
                xml = (Request.Form["__BASIC_MODS_XML_DATA"] ?? "").ToString();
                if (xml == "")
                    xml = (ClientPostBackFormData["__BASIC_MODS_XML_DATA"] ?? "").ToString();
                xml = ProcessComboValues(xml);
            }

            if (_isImage)
            {
                XDocument modsDoc = XDocument.Parse(xml);

                XmlNamespaceManager nsmgr = new XmlNamespaceManager(new NameTable());
                nsmgr.AddNamespace("mods", "http://www.loc.gov/mods/v3");
                XNamespace modsNS = "http://www.loc.gov/mods/v3";

                XElement imageFormatNode = new XElement(modsNS + "extent");

                XText imageData2 = new XText(String.Format("Digitised image:{0} x {1} pixels", _imageInfo.Width, _imageInfo.Height));
                imageFormatNode.Add(imageData2);

                XElement extentNode = modsDoc.Root.XPathSelectElement("//mods:physicalDescription/mods:extent[last()]", nsmgr);
                if (extentNode == null)
                {
                    XElement modsNode = modsDoc.Root.XPathSelectElement("//mods:mods[1]", nsmgr);
                    XElement physicalDescNode = modsNode.XPathSelectElement("mods:physicalDescription[1]", nsmgr);
                    if (physicalDescNode == null)
                    {
                        physicalDescNode = new XElement(modsNS + "physicalDescription");
                        physicalDescNode.Add(imageFormatNode);
                        if (!String.IsNullOrEmpty(_mimeType))
                        {
                            XElement internetMediaTypeNode = new XElement(modsNS + "internetMediaType");
                            XText imageData3 = new XText(_mimeType);
                            internetMediaTypeNode.Add(imageData3);
                            physicalDescNode.Add(internetMediaTypeNode);
                        }
                        modsNode.Add(physicalDescNode);
                    }
                    else
                    {
                        physicalDescNode.Add(imageFormatNode);
                    }

                }
                else
                {
                    extentNode.AddAfterSelf(imageFormatNode);
                }

                xml = modsDoc.ToString();
            }


            return new MODSMetadata(xml);

        }

        /// <summary>
        /// GetDC. Builds a DC data object from posted back form data
        /// </summary>
        /// <returns>a DublinCoreMetadata object</returns>
        private DublinCoreMetadata GetDC()
        {
            string xml = (Request.Form["__BASIC_DC_XML_DATA"] ?? "").ToString();
            if (xml == "")
                xml = (ClientPostBackFormData["__BASIC_DC_XML_DATA"] ?? "").ToString();

            xml = ProcessComboValues(xml);

            if (!xml.Contains(":title>"))
            {
                throw new XmlSchemaValidationException(String.Format("Missing mandatory field 'title' in DC metadata", "Title"));
            }

            if (_isImage)
            {
                XDocument modsDoc = XDocument.Parse(xml);

                XmlNamespaceManager nsmgr = new XmlNamespaceManager(new NameTable());
                nsmgr.AddNamespace("dc", "http://purl.org/dc/elements/1.1/");
                XNamespace modsNS = "http://purl.org/dc/elements/1.1/";

                XElement imageFormatNode = new XElement(modsNS + "format");

                XText imageData2 = new XText(String.Format("Digitised image:{0} x {1} pixels", _imageInfo.Width, _imageInfo.Height));
                imageFormatNode.Add(imageData2);

                XElement formatNode = modsDoc.Root.XPathSelectElement("dc:format[last()]", nsmgr);
                if (formatNode == null)
                {
                    formatNode = modsDoc.Root;
                    formatNode.Add(imageFormatNode);
                }
                else
                {
                    formatNode.AddAfterSelf(imageFormatNode);
                }

                xml = modsDoc.ToString();
            }

            return new DublinCoreMetadata(xml);

        }

        /// <summary>
        /// DepositDocument. Uses the Hydranet code library to construct a Hydra profile Fedora repository object
        /// and uses the same library to send it to the repository.
        /// </summary>
        private void DepositDocument()
        {
            if (_file != null)
            {
                using (SPWeb site = SPContext.Current.Web)
                {
                    string url = site.Url + '/' + _file.Url;
                    string nameSpace = System.Configuration.ConfigurationManager.AppSettings["FedoraObjectNamespace"];

                    HydraServiceFedoraExt hydraService = new HydraServiceFedoraExt(nameSpace, _itemTitle, HttpContext.Current.User.Identity.Name, ":1");
                    byte[] content = null;
                    content = _file.OpenBinary(SPOpenBinaryOptions.SkipVirusScan);  // virusscan could cain the server on a large file, files containing viruses should never have been allowed onto sharepoint in the first place anyway
                    if (ckShowDC.Checked)
                    {
                        hydraService.AppendDCMetaData(GetDC());
                    }

                    if (ckShowBasicMODs.Checked || ckshowIPMODs.Checked)
                    {
                        hydraService.AppendDescriptionMetaData(GetMODS());
                    }

                    hydraService.AppendContent(content, _mimeType);
                    hydraService.AddRelsExtMetaData(System.Configuration.ConfigurationManager.AppSettings["FedoraPublishQueuePID"]);
                    hydraService.DepositContentObject();

                    _successfulUpload = true;
                    ReportSuccess();
                }
            }

        }
        #endregion

        #endregion

    }
}
