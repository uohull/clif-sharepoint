using System;
using System.Web;
using System.Net;
using System.ComponentModel;
using System.IO;
using System.Collections.Generic;
using System.Text;

using uk.ac.hull.repository.hydranet.hydracontent;

namespace uk.ac.hull.repository.hydranet.fedora
{
    public class ResourceIndexClient
    {
        private FedoraServer fedoraServer;
        private ContentObjectList contentObjList;


        public ContentObjectList getSetChildrenObjects(string parentId)
        {
            string query = "select $member $isCollection $label $mimetype " +
                            "from <#ri> " +
                             "where $member <fedora-rels-ext:isMemberOf> <info:fedora/" + parentId + "> " +
                                "and $member <fedora-rels-ext:isCollection> $isCollection " +
                                 "and $member <info:fedora/fedora-system:def/model#label> $label " +
                                  "and $member <dc:format> $mimetype " + 
                               "order by $isCollection asc;";

            string response = QueryIndex("tuples", "itql", "CSV", int.MaxValue.ToString(), query);
            return ParseResult(response, 4);
        }

        public ContentObjectList getRecursiveSetChildrenObjects(string parentId)
        {
          string query = "select $member $subMember $isCollection $label $mimetype " +
                            "from <#ri> " +
                             "where walk($member <fedora-rels-ext:isMemberOf> <info:fedora/" + parentId + "> " +
                               "and $subMember <fedora-rels-ext:isMemberOf> $member) " +
                                "and $subMember <fedora-rels-ext:isCollection> $isCollection " +
                                 "and $subMember <info:fedora/fedora-system:def/model#label> $label " +
                                   "and $subMember <dc:format> $mimetype " +
                                    "order by $isCollection asc;";

          string response = QueryIndex("tuples", "itql", "CSV", int.MaxValue.ToString(), query);
          return ParseResult(response, 5);
        }

        private ContentObjectList ParseResult(string content, int numOfCols) 
        {

            //Get the lines of the content
            string[] lines = content.Split('\n');
            int noOfLines = lines.Length;
            //If empty query
            if (noOfLines == 1) 
            {
                return null;
            }
            else 
            {
                ContentObject contentObj;
                contentObjList = new ContentObjectList();

                for (int i = 1; i < noOfLines - 1; i++)
                {
                    string [] tokens = lines[i].Split(',');

                    if (numOfCols == 4)
                    {
                        //Hard coded to deal with member/isCollection/label/mimetype format of result
                        string member = tokens[0].Substring(tokens[0].LastIndexOf('/') + 1);
                        string isCollection = tokens[1];
                        string label = tokens[2];
                        string mimetype = tokens[3];

                        contentObj = new ContentObject(member, bool.Parse(isCollection), label, mimetype);

                        contentObjList.Add(contentObj);
                    }
                    if (numOfCols == 5)
                    {
                        //Hard coded to deal with member/isCollection/label/mimetype format of result
                        string member = tokens[0].Substring(tokens[0].LastIndexOf('/') + 1);
                        string subMember = tokens[1].Substring(tokens[1].LastIndexOf('/') + 1);
                        string isCollection = tokens[2];
                        string label = tokens[3];
                        string mimetype = tokens[4];

                        contentObj = new ContentObject(subMember, bool.Parse(isCollection), label, mimetype);

                        contentObjList.Add(contentObj);

                    }

                 
                                      
                }

                return contentObjList;
            }
       }


        public static Dictionary<string, string> postStringToDictionary(string postString)
        {
            char[] delimiters = { '&' };
            string[] postPairs = postString.Split(delimiters);

            Dictionary<string, string> postVariables = new Dictionary<string, string>();
            foreach (string pair in postPairs)
            {
                char[] keyDelimiters = { '=' };
                string[] keyAndValue = pair.Split(keyDelimiters);
                if (keyAndValue.Length > 1)
                {
                    postVariables.Add(HttpUtility.UrlDecode(keyAndValue[0]),
                        HttpUtility.UrlDecode(keyAndValue[1]));
                }
            }

            return postVariables;
        }

        public static string dictionaryToPostString(Dictionary<string, string> postVariables)
        {
            string postString = "";
            foreach (KeyValuePair<string, string> pair in postVariables)
            {
                postString += HttpUtility.UrlEncode(pair.Key) + "=" +
                    HttpUtility.UrlEncode(pair.Value) + "&";
            }

            return postString;
        }

        private string QueryIndex(string type, string lang, string format, string limit, string query)
        {
            fedoraServer = new FedoraServer();

            string resourceIndexURIString = "http://" + fedoraServer.ServerAddress + ":" + fedoraServer.ServerPort.ToString() + "/fedora/risearch";
            Uri resourceIndexURI = new Uri(resourceIndexURIString);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(resourceIndexURI);

            Dictionary<string,string> postVariables  = new Dictionary<string,string>();
            postVariables.Add("type", type);
            postVariables.Add("lang", lang);
            postVariables.Add("format", format);
            postVariables.Add("limit", limit);
            postVariables.Add("query", query);
                     
            string postString = dictionaryToPostString(postVariables);
            byte[] postBytes  = Encoding.ASCII.GetBytes(postString);

            request.Method = "POST";
            request.ContentLength = postBytes.Length;
            request.AllowAutoRedirect = true;
            request.ContentType = "application/x-www-form-urlencoded";
                     
            Stream postStream = request.GetRequestStream();
            postStream.Write(postBytes,0,postBytes.Length);
            postStream.Close();

            HttpWebResponse response = (HttpWebResponse) request.GetResponse();
            //WebResponse response = request.GetResponse();

            Stream responseStream = response.GetResponseStream();
            StreamReader responseStreamReader = new StreamReader(responseStream);

            string result = responseStreamReader.ReadToEnd();

            response.Close();
            responseStream.Close();
            responseStreamReader.Close();

            return result;
        }

 //////////////////////////////////////////// NEW METHODS /////////////////////////////////////

        public ContentObjectList getSetChildrenObjects(string parentId, bool isCollectionType)
        {
            /*
                        string query = "select $member $isCollection $label $mimetype " +
                                        "from <#ri> " +
                                         "where $member <fedora-rels-ext:isMemberOf> <info:fedora/" + parentId + "> " +
                                            "and $member <fedora-rels-ext:isCollection> $isCollection " +
                                             "and $member <info:fedora/fedora-system:def/model#label> $label " +
                                              "and $member <dc:format> $mimetype " + 
                                           "order by $isCollection asc;";

                        string response = QueryIndex("tuples", "itql", "CSV", int.MaxValue.ToString(), query);
             */

/* 
            string query = "select $member $isCollection $label $mimetype " +
                            "from <#ri> " +
                             "where $member <fedora-rels-ext:isCollection> $isCollection " +
                                 "and $member <info:fedora/fedora-system:def/model#label> $label " +
                                  "and $member <dc:format> $mimetype " +
                               "order by $isCollection asc;";

*/
            string query = "select $member $isCollection $label $mimetype " +
                            "from <#ri> " +
                             "where $member <fedora-rels-ext:isMemberOf> <info:fedora/" + parentId + "> " +
                                "and $member <fedora-rels-ext:isCollection> $isCollection " +
                                 "and $member <info:fedora/fedora-system:def/model#label> $label " +
                                  "and $member <dc:format> $mimetype;";

            string response = QueryIndex("tuples", "itql", "CSV", int.MaxValue.ToString(), query);
            return ParseResultByType(response, 4, isCollectionType);
        }

        private ContentObjectList ParseResultByType(string content, int numOfCols, bool isCollectionType)
        {

            //Get the lines of the content
            string[] lines = content.Split('\n');
            int noOfLines = lines.Length;
            //If empty query
            if (noOfLines == 1)
            {
                return null;
            }
            else
            {
                ContentObject contentObj;
                contentObjList = new ContentObjectList();

                for (int i = 1; i < noOfLines - 1; i++)
                {
                    string[] tokens = lines[i].Split(',');

                    if (numOfCols == 4)
                    {
                        //Hard coded to deal with member/isCollection/label/mimetype format of result
                        string member = tokens[0].Substring(tokens[0].LastIndexOf('/') + 1);
                        string isCollection = tokens[1];
                        bool isCollectionBool = bool.Parse(isCollection);
                        string label = tokens[2];
                        string mimetype = tokens[3];

                        if (isCollectionBool == isCollectionType)
                        {
                            contentObj = new ContentObject(member, isCollectionBool, label, mimetype);
                            contentObjList.Add(contentObj);
                        }
                    }
                    if (numOfCols == 5)
                    {
                        //Hard coded to deal with member/isCollection/label/mimetype format of result
                        string member = tokens[0].Substring(tokens[0].LastIndexOf('/') + 1);
                        string subMember = tokens[1].Substring(tokens[1].LastIndexOf('/') + 1);
                        string isCollection = tokens[2];
                        bool isCollectionBool = bool.Parse(isCollection);
                        string label = tokens[3];
                        string mimetype = tokens[4];

                        if (isCollectionBool == isCollectionType)
                        {
                            contentObj = new ContentObject(subMember, isCollectionBool, label, mimetype);
                            contentObjList.Add(contentObj);
                        }
                    }

                }

                return contentObjList;
            }
        }

    }
}
