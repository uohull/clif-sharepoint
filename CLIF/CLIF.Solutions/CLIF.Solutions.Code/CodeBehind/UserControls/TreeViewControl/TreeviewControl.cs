using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace CLIF.Solutions.Code
{
    public class TreeviewControl : UserControl
    {
        public string title = "root";
        public string service = String.Empty;
        public string rootobject = string.Empty;
        protected Literal litDivTags;
        string _listId = String.Empty;
        string _rootNode = string.Empty;
        protected override void OnLoad(EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["pid"] != null)
                {
                    rootobject = Request.QueryString["pid"].ToString();
                }
                else
                {
                    SPListItem settings = null;
                    using (MySiteRepositorySettings ObjSettings = new MySiteRepositorySettings())
                    {
                        settings = ObjSettings.GetRepositorySettings(SPContext.Current.Web.Title);
                        if (ObjSettings != null)
                        {
                            rootobject = settings["Default Root Object"].ToString();
                        }
                    }
                }
                litDivTags.Text = "<div id=" + this.ClientID + " class=\"TreeView\"  service=" + this.service + ">";
                litDivTags.Text += "<div class=\"du\" name=" + rootobject + "><span class=\"ft\">" + rootobject + "</span></div><div class=\"subframe\" style=\"display: none\"><div class='fl'> </div></div>";
                litDivTags.Text += "</div>";
            }
            base.OnLoad(e);
        }


        public void RootNode(string Value)
        {
            _rootNode = Value;
        }
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (Page.Header == null)
                throw new Exception("The <head> element of this page is not marked with runat='server'.");

            // register the JavaScripts includes without need for a Form.
            if (!Page.ClientScript.IsClientScriptBlockRegistered(Page.GetType(), "CommonBehaviour"))
            {
                Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "CommonBehaviour", String.Empty);
                ((HtmlHead)Page.Header).Controls.Add(new LiteralControl("<script type='text/javascript' src='"
                  + Page.ResolveUrl("~/_layouts/CLIFPages/JS/jcl.js")
                  + "'><" + "/script>\n"));
            } // if

            if (!Page.ClientScript.IsClientScriptBlockRegistered(this.GetType(), "MyBehaviour"))
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "MyBehaviour", String.Empty);
                ((HtmlHead)Page.Header).Controls.Add(new LiteralControl("<script type='text/javascript' src='"
                  + Page.ResolveUrl("~/_layouts/CLIFPages/JS/TreeView.js")
                  + "'><" + "/script>\n"));
            } // if

        } // OnPreRender
    }
}
