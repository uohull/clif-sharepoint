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

[assembly: System.Web.UI.WebResource("CLIF.Solutions.Code.Modules.PublishMetaDataForms.Resources.Javascript.MODSBuilder.js", "text/javascript")]

/// <summary>
/// MODSBasic. Code behind for the MODSBasic user control.
/// </summary>
public partial class MODSBasic : System.Web.UI.UserControl
{
    #region Event handlers

    /// <summary>
    /// Page_Load. loads any configurable control values (e.g lists for drop downs and combos) and required Helper javascripts
    /// </summary>
    /// <param name="sender">see MSDN documentation</param>
    /// <param name="e">see MSDN documentation</param>
    protected void Page_Load(object sender, EventArgs e)
    {

        Parent.Page.ClientScript.RegisterClientScriptInclude("MODSBuilder", Parent.Page.ClientScript.GetWebResourceUrl(typeof(MODSBasic), "CLIF.Solutions.Code.Modules.PublishMetaDataForms.Resources.Javascript.MODSBuilder.js"));

        try
        {
            CLIFConfigurationSettings configurationSettings =
               ConfigurationManager.GetSection("CLIFSendFedora") as CLIFConfigurationSettings;

            if (configurationSettings == null)
                System.Diagnostics.Trace.WriteLine("Failed to load configurationSettings.");
            else
            {
                HtmlInputText modsAuthority = (HtmlInputText)this.FindControl("txt_subjectAuthority_MODSBasic");
                modsAuthority.Value = configurationSettings.MODSAuthority.Value;

                HtmlInputCheckBox ckAccessCondition = (HtmlInputCheckBox)this.FindControl("main_ckbc_accessCondition_MODSBasic");
                ckAccessCondition.Value = String.Format(configurationSettings.RightsMainStatement.Value, DateTime.Now.Year);

                Label lblAccessCondition = (Label)this.FindControl("lbl_main_accessCondition");
                lblAccessCondition.Text = ckAccessCondition.Value;

                PlaceHolder rightsPlaceHolder = (PlaceHolder)this.FindControl("accessConditionsPlaceHolder");

                for (int i = 0; i < configurationSettings.RightsOptionalStatements.Count; i++)
                {
                    HtmlInputCheckBox ckBox = new HtmlInputCheckBox();
                    ckBox.ID = i + "_ckbc_accessCondition_MODSBasic";
                    ckBox.Value = configurationSettings.RightsOptionalStatements[i].OptionValue;
                    ckBox.Attributes.Add("class", "MODSBasic");
                    rightsPlaceHolder.Controls.Add(ckBox);
                    rightsPlaceHolder.Controls.Add(new LiteralControl(configurationSettings.RightsOptionalStatements[i].OptionValue));
                    rightsPlaceHolder.Controls.Add(new LiteralControl("<br/>"));
                }

                HtmlInputText tbPublisher = (HtmlInputText)this.FindControl("txt_publisher_MODSBasic");
                tbPublisher.Value = configurationSettings.MODSPublisher.Value;

                DropDownList cbMODSRoleTypes = (DropDownList)this.FindControl("rptd_ddl_roleType_MODSBasic");

                cbMODSRoleTypes.Items.Clear();
                for (int i = 0; i < configurationSettings.MODSRoleTypes.Count; i++)
                {
                    cbMODSRoleTypes.Items.Add(new ListItem(configurationSettings.MODSRoleTypes[i].OptionValue, configurationSettings.MODSRoleTypes[i].OptionValue));
                }

                DropDownList dlLanguageTerms = (DropDownList)this.FindControl("ddl_languageTermType_MODSBasic");
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
