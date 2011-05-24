using System;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Services;
using System.Xml;
using AjaxControlToolkit;
using System.Web.Script.Services;
using System.Configuration;
using CLIF.Solutions.Code;
using System.Collections.Generic;
 
///<summary>
/// Class AJAXCtrlsHlpr. implements a web service for page controls requiring AJAX server calls
/// [WebService(Namespace = "http://tempuri.org/")]
/// [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
/// 
/// N.B can add exact same method to web page code behind if we make WebMethods static
///</summary>

[System.Web.Script.Services.ScriptService]
public class AJAXCtrlsHlpr : System.Web.Services.WebService
{
    /// <summary>
    /// GetResourceTypeGenres. Fetchs values for the ___ResourceTypes drop down list and its dependent genres drop down
    /// </summary>
    /// <param name="knownCategoryValues">the current parent control (ResourceType) value  format = name:value;</param>
    /// <param name="category">identifies the control requiring population ___ResourceTypes or __Genre</param>
    /// <returns></returns>
    [WebMethod]
    [System.Web.Script.Services.ScriptMethod(ResponseFormat=ResponseFormat.Json)]
    public CascadingDropDownNameValue[] GetResourceTypeGenres(string knownCategoryValues, string category)
    {
        CLIFConfigurationSettings configurationSettings =
           ConfigurationManager.GetSection("CLIFSendFedora") as CLIFConfigurationSettings;

        List<CascadingDropDownNameValue> retList = new List<CascadingDropDownNameValue>();

        if (category == "___ResourceTypes")
        {
            for (int i = 0; i < configurationSettings.ResourceTypes.Count; i++)
            {
                retList.Add(new CascadingDropDownNameValue() { isDefaultValue = false, name = configurationSettings.ResourceTypes[i].Key, value = configurationSettings.ResourceTypes[i].Value, optionTitle = configurationSettings.ResourceTypes[i].Key });
            }

        }
        else
        {
            for (int i = 0; i < configurationSettings.MODSGenres.Count; i++)
            {
                string[] parentCategory = knownCategoryValues.Split(':');
                string groupMatch = parentCategory[1].TrimEnd(new char[]{';'});
                if (configurationSettings.MODSGenres[i].Group == groupMatch)
                    retList.Add( new CascadingDropDownNameValue() { isDefaultValue = false, name = configurationSettings.MODSGenres[i].Key, value = configurationSettings.MODSGenres[i].Value, optionTitle = configurationSettings.MODSGenres[i].Key } );
            }
        }

        return retList.ToArray();
    }
}