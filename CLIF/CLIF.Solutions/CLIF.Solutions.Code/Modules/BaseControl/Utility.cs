using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web;

using Microsoft.SharePoint.Publishing;
using Microsoft.SharePoint;
using System.Net.Mail;
using System.Net;
using System.Web.Security;
using System.Web.Profile;
using System.Xml;
using System.IO;

namespace CLIF.Solutions.Code
{
    public class Utility
    {
        public static string AddKey(string Url, string Key, string Value)
        {
            if (Url.IndexOf("?") > 0)
                return Url + "&" + Key + "=" + Value;
            else
                return Url + "?" + Key + "=" + Value;
        }

        public static string RemoveKey(string Url, string Key)
        {
            int start = Url.IndexOf(Key + "=");
            int end = 0;
            if (start > 0)
            {
                end = Url.IndexOf("&", start);
                if (end <= 0) end = Url.Length;
                
                string keyWithValue = Url.Substring(start,end - start);
                Url = Url.Replace(keyWithValue + "&", "").Replace("&" + keyWithValue, "").Replace(keyWithValue, "");
                if (Url.Substring(Url.Length - 1) == "?")
                    return Url.Replace("?", "");
                else
                    return Url;
            }
            return Url;
        }

        public static string GetQSValue(string Url, string Key)
        {
            int start = Url.IndexOf(Key + "=");
            int end = 0;
            if (start > 0)
            {
                end = Url.IndexOf("&", start);
                if (end <= 0) end = Url.Length;

                string keyWithValue = Url.Substring(start, end - start);
                return keyWithValue.Replace(Key + "=", "");
            }
            return "";
        }

        public static string GetCookie(string cookiename, HttpContext ctx)
        {
            try
            {
                if (HttpContext.Current.Request.Cookies[cookiename] != null)
                {
                    string cookieValue = HttpUtility.UrlDecode(HttpContext.Current.Request.Cookies[cookiename].Value);
                    return cookieValue;
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
            }
            return "";
        }

        public static void SetCookie(string cookiename, string cookievalue, int iDaysToExpire, HttpContext ctx)
        {
            try
            {
                HttpCookie objCookie = new HttpCookie(cookiename);
                String encodedValue = HttpUtility.UrlEncode(cookievalue);
                objCookie.Value = encodedValue;
                objCookie.Expires = DateTime.Now.AddDays(iDaysToExpire);
                ctx.Response.Cookies.Add(objCookie);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
            }
        }
        
        public static Control FindControl(Control oControl, string id)
        {
            try
            {
                Control c = oControl.NamingContainer.FindControl(id);

                if (c != null) return c;

                // Incase we are in a container ourselves!!
                c = oControl.NamingContainer.Parent.NamingContainer.FindControl(id);

                if (c != null) return c;

                // Give up...look in the entire form!!
                System.Collections.Queue q = new System.Collections.Queue();
                q.Enqueue(oControl.Page.Form);

                while (q.Count > 0)
                {
                    Control oParent = (Control)q.Dequeue();
                    c = oParent.NamingContainer.FindControl(id);
                    if (c != null) return c;

                    foreach (Control oChild in oParent.Controls)
                        q.Enqueue(oChild);
                }
            }
            catch (Exception ex)
            {
                Trace.Write(ex);
            }

            return null;
        }

        // lists of links stored in Publishing HTML fields are retrieved in an escaped form
        // this function expands them back out into child elements
        public static XmlElement ExpandXmlElement(XmlElement xmlElement)
        {
            string text = xmlElement.InnerText;
            if (text.Length > 0)
            {
                xmlElement.InnerText = "";
                try
                {
                    xmlElement.InnerXml = text;
                }
                catch (XmlException)
                {
                    // usually this is down to SharePoint storing HTML with unquoted attributes, so attempt to fix
                    // them up
                    text = FixLinksBug(text);
                    try
                    {
                        xmlElement.InnerXml = text;
                    }
                    catch (XmlException ex)
                    {
                        throw new XmlException("Field: " + xmlElement.OuterXml + " not valid Xml: '" + text + "'", ex);
                    }
                }
            }
            return xmlElement;
        }

        // SharePoint stores HTML content such as links and images with attributes inconsistently
        // quoted and unquoted. Unquoted attributes cause problems manipulating the HTML content as XML, 
        // so this function attempts to fix them up by looking for unquoted attributes. 
        public static string FixLinksBug(string value)
        {
            string work = value;
            string result = "";
            while (work.Length > 0)
            {
                // look for an equals sign - beginning of an attribute
                int pos = work.IndexOf("=");

                if (pos != -1)
                {
                    // check if char after = is a quote
                    if (work[pos + 1] != '\"')
                    {
                        // possible unquoted attribute - try and find the end, could either be a space
                        // or a > or / to close a tag
                        string leftPart = work.Substring(0, pos + 1);
                        string rightPart = work.Substring(pos + 1, work.Length - pos - 1);

                        int endOfAttrib = FindEndOfAttribute(rightPart);
                        string attribValue = rightPart.Substring(0, endOfAttrib);
                        work = rightPart.Substring(endOfAttrib, rightPart.Length - endOfAttrib);
                        result += leftPart + "\"" + attribValue + "\"";
                    }
                    else
                    {
                        // attribute was quoted so copy the string out untouched
                        result += work.Substring(0, pos + 1);
                        work = work.Substring(pos + 1, work.Length - pos - 1);
                    }
                }
                else
                {
                    result += work;
                    work = "";
                }
            }
            return result;
        }

        private static int FindEndOfAttribute(string value)
        {
            // attempt to find either a space, self closing tag or end tag which could close an attribute
            // use whichever is found first
            int nextSpace = value.IndexOf(" ");
            int nextSlash = value.IndexOf("/");
            int nextBracket = value.IndexOf(">");

            int endOfAttrib = value.Length + 2;

            if (nextSpace != -1)
                endOfAttrib = nextSpace;

            if (nextSlash != -1 && nextSlash < endOfAttrib)
                endOfAttrib = nextSlash;

            if (nextBracket != -1 && nextBracket < endOfAttrib)
                endOfAttrib = nextBracket;

            return endOfAttrib;
        }

        public static bool sendEmail(string from, string to, string subject, string body)
        {
            try
            {
                MailMessage ob = new MailMessage();
                MailMessage objCon = new MailMessage(from, to, subject, body);
                objCon.Priority = MailPriority.Normal;

                SmtpClient client = new SmtpClient();
                client.Credentials = CredentialCache.DefaultNetworkCredentials;
                client.Send(objCon);

                return true;
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
                return false;
            }
        }

        public static bool HtmlIsHidden(string UserAgent)
        {
            try
            {
                if (string.IsNullOrEmpty(UserAgent)) return false;

                string[] arAgents = UserAgent.ToLower().Split('~');
                string sAgent = HttpContext.Current.Request.UserAgent.ToLower();

                foreach (string sCheck in arAgents)
                    if (sAgent.Contains(sCheck)) return true;
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
            }

            return false;
        }
        public static string cleanFName(string fName)
        {
            fName = fName.Replace(" ", "");
            fName = fName.Replace("-", "");
            return fName.Replace("&","_");
        }
        public static void RegisterScript(Page page, string key, Type type, string filename)
        {
            string sFilePath = page.Server.MapPath("/scripts/" + filename);

            if (File.Exists(sFilePath))
                page.ClientScript.RegisterClientScriptInclude(type, key, "/scripts/" + filename);
            else
                page.ClientScript.RegisterClientScriptResource(type, "Parity.CEHR." + filename);
        }

        public static void AddStyleSheet(Page page, Type type, string styleSheet)
        {
            LiteralControl l = new LiteralControl(String.Format("<link rel='stylesheet' text='text/css' href='{0}' />", styleSheet));

            page.Header.Controls.Add(l);
        }

        public static string GetColValFromListId(int listId,string listName,string colName)
        {
            SPList listCheck = SecurityHelper.GetSpecifiedList("/" + listName);
            SPListItem item = listCheck.GetItemById(listId);
            if (item != null)
            {
                return Utility.GetListItemValue(item, colName);
            }
            return string.Empty;
        }

        public static XmlElement AddElementToDocument(XmlDocument document, XmlElement parentElement, string elementName,string elementValue)
        {
            XmlElement newElement = document.CreateElement(elementName);
            newElement.InnerText = elementValue;
            parentElement.AppendChild(newElement);
            return newElement;
        }

        public static string GetListItemValue(SPListItem item, string fieldName)
        {
            string value = string.Empty;
            if (item != null)
            {
                if (item[fieldName] != null)
                {
                    value = item[fieldName].ToString();
                }
            }
            return value;
        }

        public static XmlElement CreateXmlElement(XmlDocument parentDoc, XmlElement parentElement, string elementName,SPListItem item, string fieldName)
        {
            string valString = GetListItemValue(item, fieldName);
            XmlElement newElement = parentDoc.CreateElement(elementName);
            newElement.InnerText = valString;
            parentElement.AppendChild(newElement);
            return parentElement;
        }

        public static XmlCDataSection CreateCDataSection(XmlDocument parentDoc, XmlElement parentElement,SPListItem item, string fieldName)
        {
            string valString = GetListItemValue(item, fieldName);
            XmlCDataSection newSection = parentDoc.CreateCDataSection(valString);
            parentElement.AppendChild(newSection);
            return newSection;
        }
        public static string EncryptText(string TextToEncrypt)
        {
            string _AtoZselection = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            Random rnd = new System.Random();
            string _rnd_text = string.Empty;
            bool _inTag = false;
            for (int i = 0; i < TextToEncrypt.Length; i++)
            {
                if (TextToEncrypt.Substring(i, 1) == "<")
                {
                    _inTag = true;
                }
                if (!_inTag && TextToEncrypt.Substring(i, 1) != " ")
                {
                    _rnd_text += _AtoZselection.Substring(rnd.Next(0, _AtoZselection.Length - 1), 1);
                }
                else
                {
                    _rnd_text += TextToEncrypt.Substring(i, 1);
                }

                if (TextToEncrypt.Substring(i, 1) == ">")
                {
                    _inTag = false;
                }
            }
            return _rnd_text;
        }        
    }

}
