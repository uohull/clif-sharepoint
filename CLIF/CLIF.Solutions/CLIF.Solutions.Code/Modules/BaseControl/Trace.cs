using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using System.Web;
using System.Web.UI;

namespace CLIF.Solutions.Code
{
    public class Trace
    {
        public static void WriteLine(object o)
        {
            string msg = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + "  ";

            string url = string.Empty;

            if (SPContext.Current != null)
            {
                if (SPContext.Current.Web != null)
                    url = SPContext.Current.Web.Url + "/";
                if (SPContext.Current.ListItem != null)
                    url += SPContext.Current.ListItem.Url;
            }
            if (!string.IsNullOrEmpty(url))
                msg += "in " + url + "  ";

            msg += o.ToString();

            WriteToPage(msg);
            System.Diagnostics.Trace.WriteLine(msg);
            System.Diagnostics.Trace.Flush();
        }

        public static void Write(object o)
        {
            string msg = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + "  ";

            string url = string.Empty;

            if (SPContext.Current != null)
            {
                if (SPContext.Current.Web != null)
                    url = SPContext.Current.Web.Url + "/";
                if (SPContext.Current.ListItem != null)
                    url += SPContext.Current.ListItem.Url;
            }
            if (!string.IsNullOrEmpty(url))
                msg += "in " + url + "  ";

            msg += o.ToString();

            System.Diagnostics.Trace.WriteLine(msg);
            System.Diagnostics.Trace.Flush();
        }

        private static void WriteToPage(object o)
        {
            if (HttpContext.Current != null)
            {
                if (HttpContext.Current.Handler != null && HttpContext.Current.Handler is Page)
                    ((Page)HttpContext.Current.Handler).Trace.Write("AC", o.ToString());
            }
        }
    }
}
