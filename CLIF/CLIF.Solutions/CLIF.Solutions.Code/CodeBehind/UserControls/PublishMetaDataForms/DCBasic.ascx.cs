using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections.Specialized;
using CLIF.Solutions.Code;
using AjaxControlToolkit;
using Common.Web.Utils;

[assembly: System.Web.UI.WebResource("CLIF.Solutions.Code.Modules.PublishMetaDataForms.Resources.Javascript.DCBuilder.js", "text/javascript")]
/// <summary>
/// DCBasic. Code behind for the DCBasic user control.
/// </summary>
public partial class DCBasic : System.Web.UI.UserControl
{
    #region Event handlers
    /// <summary>
    /// Page_Load. loads any configurable control values (e.g lists for drop downs and combos) and required Helper javascripts
    /// </summary>
    /// <param name="sender">see MSDN documentation</param>
    /// <param name="e">see MSDN documentation</param>
    protected void Page_Load(object sender, EventArgs e)
    {

        Parent.Page.ClientScript.RegisterClientScriptInclude("DCBuilder", Parent.Page.ClientScript.GetWebResourceUrl(typeof(DCBasic), "CLIF.Solutions.Code.Modules.PublishMetaDataForms.Resources.Javascript.DCBuilder.js"));

        try
        {
            CLIFConfigurationSettings configurationSettings =
               ConfigurationManager.GetSection("CLIFSendFedora") as CLIFConfigurationSettings;

            if (configurationSettings == null)
                System.Diagnostics.Trace.WriteLine("Failed to load configurationSettings.");
            else
            {
                HtmlInputText tbPublisher = (HtmlInputText)this.FindControl("txt_publisher_DCBasic");
                tbPublisher.Value = configurationSettings.MODSPublisher.Value;

                HtmlInputCheckBox ckAccessCondition = (HtmlInputCheckBox)this.FindControl("main_ckbc_rights_DCBasic");
                ckAccessCondition.Value = String.Format(configurationSettings.RightsMainStatement.Value, DateTime.Now.Year);

                Label lblAccessCondition = (Label)this.FindControl("lbl_main_rights");
                lblAccessCondition.Text = ckAccessCondition.Value;

                PlaceHolder rightsPlaceHolder = (PlaceHolder)this.FindControl("rightsPlaceHolder");

                for (int i = 0; i < configurationSettings.RightsOptionalStatements.Count; i++)
                {
                    HtmlInputCheckBox ckBox = new HtmlInputCheckBox();
                    ckBox.ID = i + "_ckbc_rights_DCBasic";
                    ckBox.Value = configurationSettings.RightsOptionalStatements[i].OptionValue;
                    ckBox.Attributes.Add("class", "DCBasic");
                    rightsPlaceHolder.Controls.Add(ckBox);
                    rightsPlaceHolder.Controls.Add(new LiteralControl(configurationSettings.RightsOptionalStatements[i].OptionValue));
                    rightsPlaceHolder.Controls.Add(new LiteralControl("<br/>"));
                }

                AjaxControlToolkit.ComboBox cbResourcesTypes = (AjaxControlToolkit.ComboBox)this.FindControl("cmb_type_DCBasic");
                cbResourcesTypes.Items.Clear();
                for (int i = 0; i < configurationSettings.ResourceTypes.Count; i++)
                {
                    cbResourcesTypes.Items.Add(new ListItem(configurationSettings.ResourceTypes[i].Key, configurationSettings.ResourceTypes[i].Value));
                }

                DropDownList dlLanguageTerms = (DropDownList)this.FindControl("ddl_language_DCBasic");
                dlLanguageTerms.Items.Clear();
                for (int i = 0; i < configurationSettings.LanguageTerms.Count; i++)
                {
                    dlLanguageTerms.Items.Add(new ListItem(configurationSettings.LanguageTerms[i].Key, configurationSettings.LanguageTerms[i].Value));
                }
            }

        }
        catch (ConfigurationErrorsException err)
        {
            System.Diagnostics.Trace.WriteLine("ReadCustomSection(string): {0}", err.ToString());
        }

    }

    #endregion
}
