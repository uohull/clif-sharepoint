using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web;
using System.IO;
using System.Reflection;

//[assembly: TagPrefix("Westwind.Web.Controls", "ww")]

[assembly: WebResource("jQueryDatePicker.Resources.jquery.js", "text/javascript")]
[assembly: WebResource("jQueryDatePicker.Resources.calendar.gif", "image/gif")]
[assembly: WebResource("jQueryDatePicker.Resources.ui.datepicker.js", "text/javascript")]
[assembly: WebResource("jQueryDatePicker.Resources.ui.datepicker.css", "text/css")]


namespace Common.Web.Utils
{ 
    /// <summary>
    /// Class is used as to consolidate access to resources
    /// </summary>
    public class ControlResources
    {
        public const string JQUERY_SCRIPT_RESOURCE = "jQueryDatePicker.Resources.jquery.js";
        public const string CALENDAR_ICON_RESOURCE = "jQueryDatePicker.Resources.calendar.gif";

        public const string JQUERY_CALENDAR_SCRIPT_RESOURCE = "jQueryDatePicker.Resources.ui.datepicker.js";
        public const string JQUERY_CALENDAR_CSS_RESOURCE = "jQueryDatePicker.Resources.ui.datepicker.css";        

        /// <summary>
        /// Loads the appropriate jScript library out of the scripts directory
        /// </summary>
        /// <param name="control"></param>
        public static void LoadjQuery(Control control, string jQueryUrl)
        {
            ClientScriptProxy.ClientScriptProxy p = ClientScriptProxy.ClientScriptProxy.Current;
            //p.RegisterClientScriptResource(control, typeof(ControlResources), ControlResources.PROTOTYPE_SCRIPT_RESOURCE);
            p.RegisterClientScriptResource(control, typeof(ControlResources), ControlResources.JQUERY_SCRIPT_RESOURCE, Common.Web.Utils.ClientScriptProxy.ScriptRenderModes.HeaderTop);

            return;
            
            //ClientScriptProxy p = ClientScriptProxy.Current;

            //if (string.IsNullOrEmpty(jQueryUrl))
            //    jQueryUrl = "~/scripts/jquery.js";

            //string script;
            //if (HttpContext.Current.IsDebuggingEnabled)
            //    script = control.ResolveUrl(jQueryUrl);
            //else
            //    script = control.ResolveUrl(jQueryUrl.Replace(".js",".min.js"));

            //p.RegisterClientScriptInclude(control, typeof(ControlResources), "__jquery", script);            

#if (false)  // *** Embed in header
            
//            StringBuilder sb = new StringBuilder(128);
//            sb.Append(@"
//<script src=""" + script +
//@""" type=""text/javascript""></script>
//");

//            <script type=""text/javascript"">
//jQuery.noConflict();
//</script>
            //control.Page.Header.Controls.AddAt(0, new LiteralControl(sb.ToString()));

            //p.RegisterClientScriptBlock(control,typeof(ControlResources),
            //                            "__jqueryinit",
            //                            "jQuery.noConflict",true);                                                    
#endif
        }

        /// <summary>
        /// Loads the jQuery component uniquely into the page
        /// </summary>
        /// <param name="control"></param>
        /// <param name="jQueryUrl">Optional Url to the jQuery Library. NOTE: Should also have a .min version in place</param>
        public static void LoadjQuery(Control control)
        {
            LoadjQuery(control,null);
        }

        /// <summary>
        /// Simplified Helper function that is used to add script files to the page
        /// This version adds scripts to the top of the page in the 'normal' position
        /// immediately following the form tag.
        /// </summary>
        /// <param name="script"></param>
        public static void IncludeScriptFile(Control control, string scriptFile)
        {
            scriptFile = control.ResolveUrl(scriptFile);
            ClientScriptProxy.ClientScriptProxy.Current.RegisterClientScriptInclude(control, typeof(ControlResources), 
                                          Path.GetFileName(scriptFile).ToLower(), 
                                          scriptFile);            
        }

        /// <summary>
        /// Simplified Helper function that is used to add script files to the page
        /// This version adds scripts to the bottom of the page just before the
        /// Form tag is ended. This ensures that other libraries have loaded
        /// earlier in the page.
        /// 
        /// This may be required for 'manually' adding script code that relies
        /// on other dependencies. For example a jQuery library that depends
        /// on jQuery or wwScriptLIbrary that is implicitly loaded.
        /// </summary>
        /// <param name="control"></param>
        /// <param name="script"></param>
        public static void IncludeScriptFileBottom(Control control, string scriptFile)
        {
            scriptFile = control.ResolveUrl(scriptFile);
            ClientScriptProxy.ClientScriptProxy.Current.RegisterClientScriptBlock(control, typeof(ControlResources),
                            Path.GetFileName(scriptFile).ToLower(),
                            "<script src='" + scriptFile + "' type='text/javascript'></script>",
                            false);            
        }


        /// <summary>
        /// Returns a string resource from a given assembly.
        /// </summary>
        /// <param name="assembly">Assembly reference (ie. typeof(ControlResources).Assembly) </param>
        /// <param name="ResourceName">Name of the resource to retrieve</param>
        /// <returns></returns>
        public static string GetStringResource(Assembly assembly, string ResourceName)
        {            
            Stream st = assembly.GetManifestResourceStream(ResourceName);
            StreamReader sr = new StreamReader(st);
            string content = sr.ReadToEnd();            
            st.Close();
            return content;
        }

        /// <summary>
        /// Returns a string resource from the from the ControlResources Assembly
        /// </summary>
        /// <param name="ResourceName"></param>
        /// <returns></returns>
        public static string GetStringResource(string ResourceName)
        {
            return GetStringResource(typeof(ControlResources).Assembly, ResourceName);
        }

    }    

    

}
