using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Microsoft.Office.Server.Search.Query;
using System.Collections.Specialized;
using System.Collections;
using Microsoft.Office.Server;
using System.Data;
using System.Globalization;
using System.Text.RegularExpressions;
using Microsoft.SharePoint;

namespace CLIF.Solutions.Code
{
    public class DocumentSearch : XmlBasedControl
    {
        private string _scope="CLIF";        
        private string _documentAuthor;        
        public string FilterField
        {
            get
            {
                try
                {
                    return System.Web.HttpUtility.UrlDecode(Page.Request.QueryString.Get("f")).Trim().ToLower();
                }
                catch (Exception e)
                {
                    return "";
                }
            }
        }
        public string DocumentAuthor
        {
            set
            {
                this._documentAuthor = value;
            }
        }
        public string SearchScope
        {
            set 
            {
                this._scope = value;
            }
        }
        public string SearchText
        {
            get
            {
                try
                {
                    return System.Web.HttpUtility.UrlDecode(Page.Request.QueryString.Get("k")).Trim().ToLower();
                }
                catch (Exception e)
                {
                    return "";
                }
            }
        }
 
        protected override XmlDocument BuildDocument()
        {
            XmlDocument xmlResults = new XmlDocument();
            Hashtable argumentList = new Hashtable();
            StringBuilder BaseSqlStatement = new StringBuilder();
            
            NameValueCollection queryString = Page.Request.QueryString;
            try
            {
                FullTextSqlQuery query = new FullTextSqlQuery(ServerContext.Current);
                ResultTableCollection queryResults = null;
                query.TrimDuplicates = true;
                query.IgnoreAllNoiseQuery = true;
                query.ResultTypes = ResultType.RelevantResults;
                ParsePagingParameters(queryString, argumentList, query);

                //Building query text
                BaseSqlStatement.Append("SELECT Rank, Title, HitHighlightedSummary, LastModifiedTime, Path,PersistentID,ListId FROM portal..scope() WHERE \"SCOPE\" = 'CLIF'");                
                BaseSqlStatement.Append(" AND FREETEXT('" + SearchText + "')");                
                query.QueryText = BaseSqlStatement.ToString();
                xmlResults.LoadXml("<Xml/>");
                try
                {
                    queryResults = query.Execute();
                }
                catch (Exception ex)
                {
                    XmlElement errorElement = xmlResults.CreateElement("error");
                    XmlElement eMsgElement = xmlResults.CreateElement("message");
                    eMsgElement.InnerText = ex.Message;
                    errorElement.AppendChild(eMsgElement);
                    xmlResults.DocumentElement.AppendChild(errorElement);
                    Document = xmlResults;                    
                }
                ResultTable queryResultsTable = queryResults[ResultType.RelevantResults];
                XmlElement pElement = xmlResults.CreateElement("search_attributes");
                XmlElement startRowElement = xmlResults.CreateElement("start_row");
                startRowElement.InnerText = Convert.ToString(query.StartRow + 1);
                pElement.AppendChild(startRowElement);
                int endRow = (query.StartRow + query.RowLimit);
                if (queryResultsTable.TotalRows < endRow)
                {
                    endRow = queryResultsTable.TotalRows;
                }
                XmlElement endRowElement = xmlResults.CreateElement("end_row");
                endRowElement.InnerText = endRow.ToString();
                pElement.AppendChild(endRowElement);

                XmlElement countElement = xmlResults.CreateElement("total_results");
                countElement.InnerText = queryResultsTable.TotalRows.ToString();
                pElement.AppendChild(countElement);

                argumentList.Add("end_row", endRow.ToString());
                argumentList.Add("total_results", queryResultsTable.TotalRows);

                double iTotalPages = Math.Ceiling(Double.Parse(Convert.ToString(queryResultsTable.TotalRows / query.RowLimit) + "." + Convert.ToString(queryResultsTable.TotalRows % query.RowLimit)));

                XmlElement pagesElement = xmlResults.CreateElement("total_pages");
                pagesElement.InnerText = iTotalPages.ToString();
                pElement.AppendChild(pagesElement);

                argumentList.Add("total_pages", iTotalPages);

                DataTable queryDataTable = new DataTable();
                queryDataTable.Load(queryResultsTable, LoadOption.OverwriteChanges);
                query.Dispose();

                XmlElement textElement = xmlResults.CreateElement("searched_text");
                textElement.InnerText = SearchText;
                pElement.AppendChild(textElement);

                XmlElement curPageElement = xmlResults.CreateElement("current_page");
                curPageElement.InnerText = Convert.ToString(Convert.ToInt32(queryString["p"]) + 1);
                pElement.AppendChild(curPageElement);

                XmlElement pagePathElement = xmlResults.CreateElement("page_path");
                pagePathElement.InnerText = this.Page.Request.Url.AbsolutePath;
                pElement.AppendChild(pagePathElement);

                XmlElement preCSSElement = xmlResults.CreateElement("prev_class");
                if (queryString["p"] == "0")
                    preCSSElement.InnerText = "prevoff";
                else
                    preCSSElement.InnerText = "prev";

                pElement.AppendChild(preCSSElement);

                XmlElement nextCSSElement = xmlResults.CreateElement("next_class");
                if (queryString["p"] == Convert.ToString((iTotalPages - 1)))
                    nextCSSElement.InnerText = "nextoff";
                else
                    nextCSSElement.InnerText = "next";

                pElement.AppendChild(nextCSSElement);

                XmlElement startElement = xmlResults.CreateElement("starting_no");
                int startingPageNo = -1;
                if (queryString["p"] != "-1")
                {
                    int curPageNo = int.Parse(queryString["p"]);
                    startingPageNo = (curPageNo / 10) * 10 + 1;
                }
                startElement.InnerText = startingPageNo.ToString();
                pElement.AppendChild(startElement);

                XmlElement endElement = xmlResults.CreateElement("end_no");
                int endPageElement = 10;
                if (queryString["p"] != "-1")
                {
                    if ((startingPageNo + 9) <= iTotalPages)
                        endPageElement = startingPageNo + 9;
                    else
                        endPageElement = int.Parse(iTotalPages.ToString());
                }
                endElement.InnerText = endPageElement.ToString();
                pElement.AppendChild(endElement);

                XmlElement queryStringElement = xmlResults.CreateElement("query_string");
                string orgQuerystring = this.Page.Request.Url.Query;
                string queryAfterPageNo = orgQuerystring.Remove(orgQuerystring.LastIndexOf("p=") - 1);
                queryStringElement.InnerText = queryAfterPageNo;
                pElement.AppendChild(queryStringElement);

                XmlElement dSortElement = xmlResults.CreateElement("date_sort");
                if (orgQuerystring.IndexOf("ord=rank") != -1)
                    dSortElement.InnerText = orgQuerystring.Replace("ord=rank", "ord=date");
                else if (orgQuerystring.IndexOf("ord=title") != -1)
                    dSortElement.InnerText = orgQuerystring.Replace("ord=title", "ord=date");
                else
                    dSortElement.InnerText = "";
                pElement.AppendChild(dSortElement);

                XmlElement rSortElement = xmlResults.CreateElement("rank_sort");
                if (orgQuerystring.IndexOf("ord=date") != -1)
                    rSortElement.InnerText = orgQuerystring.Replace("ord=date", "ord=rank");
                else if (orgQuerystring.IndexOf("ord=title") != -1)
                    rSortElement.InnerText = orgQuerystring.Replace("ord=title", "ord=rank");
                else
                    rSortElement.InnerText = "";

                pElement.AppendChild(rSortElement);
                XmlElement tSortElement = xmlResults.CreateElement("title_sort");
                if (orgQuerystring.IndexOf("ord=date") != -1)
                    tSortElement.InnerText = orgQuerystring.Replace("ord=date", "ord=title");
                else if (orgQuerystring.IndexOf("ord=rank") != -1)
                    tSortElement.InnerText = orgQuerystring.Replace("ord=rank", "ord=title");
                else
                    tSortElement.InnerText = "";

                pElement.AppendChild(tSortElement);
                xmlResults.DocumentElement.AppendChild(pElement);
                string _listID = string.Empty;
                for (int i = 0; i < queryDataTable.Rows.Count; i++)
                {
                    _listID = queryDataTable.Rows[i].ItemArray[6].ToString();
                    string displayPath = queryDataTable.Rows[i].ItemArray[4].ToString();
                    XmlElement oElement = xmlResults.CreateElement("result");
                    XmlElement rankElement = xmlResults.CreateElement("Rank");
                    rankElement.InnerText = queryDataTable.Rows[i].ItemArray[0].ToString();
                    oElement.AppendChild(rankElement);
                    //XmlElement titleElement = xmlResults.CreateElement("Title");
                    //string title = queryDataTable.Rows[i].ItemArray[1].ToString();                   
                    //titleElement.InnerText = title;
                    //oElement.AppendChild(titleElement);

                    XmlElement desElement = xmlResults.CreateElement("Description");
                    desElement.InnerText = StripHtmlTag(queryDataTable.Rows[i].ItemArray[2].ToString());
                    oElement.AppendChild(desElement);

                    XmlElement modifiedElement = xmlResults.CreateElement("LastModifiedTime");
                    string modifiedDateOrg = queryDataTable.Rows[i].ItemArray[3].ToString();
                    string strTime = string.Empty;
                    try
                    {
                        strTime = modifiedDateOrg.Substring(modifiedDateOrg.IndexOf(" "));
                        strTime = strTime.Remove(strTime.LastIndexOf(":"));
                        modifiedElement.InnerText = strTime + " " + modifiedDateOrg.Remove(modifiedDateOrg.IndexOf(" "));
                    }
                    catch (Exception ex)
                    {}

                    oElement.AppendChild(modifiedElement);
                    XmlElement pathElement = xmlResults.CreateElement("Path");
                    

                    if (displayPath.Contains("/Lists/Archive/"))
                    {
                        XmlElement titleElement = xmlResults.CreateElement("Title");
                        string title = queryDataTable.Rows[i].ItemArray[1].ToString();
                        titleElement.InnerText = title + "<strong>(Archived)</strong>";
                        oElement.AppendChild(titleElement);

                        string _siteUrl = displayPath.Substring(0, displayPath.IndexOf("/Lists"));
                        //_layouts/clifpages/ImportFromRepository.aspx?list=
                        string _itemId = displayPath.Split('?')[1].Split('=')[1];
                        
                        pathElement.InnerText = _siteUrl + "/_layouts/clifpages/ImportFromRepository.aspx?listn=Archive&item=" + _itemId;
                    }
                    else if (displayPath.Contains("/Lists/Project Documents/") && displayPath.Contains("?ID="))
                    {
                        XmlElement titleElement = xmlResults.CreateElement("Title");
                        string title = queryDataTable.Rows[i].ItemArray[1].ToString();
                        titleElement.InnerText = title;
                        oElement.AppendChild(titleElement);
                        string _siteUrl = displayPath.Substring(0, displayPath.IndexOf("/Lists"));
                        string _itemId = displayPath.Split('?')[1].Split('=')[1];
                        using (SPSite ObjSite = new SPSite(_siteUrl))
                        {
                            using (SPWeb web = ObjSite.OpenWeb())
                            {
                                SPList list = web.Lists["Project Documents"];
                                SPListItem item = list.Items.GetItemById(Convert.ToInt32(_itemId));
                                pathElement.InnerText =_siteUrl + "/" + item.File.Url;
                            }
                        }
                    }
                    else
                    {
                        XmlElement titleElement = xmlResults.CreateElement("Title");
                        string title = queryDataTable.Rows[i].ItemArray[1].ToString();
                        titleElement.InnerText = title;
                        oElement.AppendChild(titleElement);
                        pathElement.InnerText = displayPath;
                    }                    
                    oElement.AppendChild(pathElement);

                    XmlElement pidElement = xmlResults.CreateElement("PID");
                    if (queryDataTable.Rows[i].ItemArray[5].ToString() == "")
                    {
                        pidElement.InnerText = "(none)";
                    }
                    else
                    {
                        pidElement.InnerText = queryDataTable.Rows[i].ItemArray[5].ToString();
                    }
                    oElement.AppendChild(pidElement);
                    xmlResults.DocumentElement.AppendChild(oElement);
                }                
            }
            catch (Exception ex)
            {
                SPHelper.WriteToEventLog("error", ex.Message);
            }
            return xmlResults;
        }
        private string StripHtmlTag(string text)
        {
            string _result = string.Empty;
            _result = Regex.Replace(text, "<c[^>]*>", "<strong>");
            _result = Regex.Replace(_result, "</c[^>]*>", "</strong>");         
            return _result;
        }     
        protected virtual void ParsePagingParameters(NameValueCollection queryString, Hashtable argumentList,Query query)
        {
            try
            {
                int currentPage = 0;

                if (queryString.Get("p") != null)
                {
                    currentPage = int.Parse(queryString["p"], CultureInfo.InvariantCulture);
                }

                // parse the results per page parameter
                if (!string.IsNullOrEmpty(queryString.Get("res")))
                {
                    int rowLimit = 0;
                    if (int.TryParse(queryString["res"], out rowLimit))
                        query.RowLimit = rowLimit;
                    else
                        throw new Exception("The number of results to display per page must be a valid numeric value.");
                }

                // update the query object
                query.StartRow = (currentPage * query.RowLimit);

                // update XSLT arguments
                argumentList.Add("cur_page", currentPage.ToString(CultureInfo.InvariantCulture));
                argumentList.Add("start_row", query.StartRow + 1);
                argumentList.Add("results_per_page", query.RowLimit.ToString(CultureInfo.InvariantCulture));
            }
            catch (Exception ex)
            {
                
            }
        }
        protected virtual void ParseSortParameters(NameValueCollection queryString, Hashtable argumentList, StringBuilder queryStatement)
        {
            // parse order by parameter from query string
            if (!string.IsNullOrEmpty(queryString.Get("ord")))
            {
                string sortOrder = queryString["ord"].ToUpperInvariant();
                switch (sortOrder)
                {

                    case "DATE":
                        queryStatement.Append(" ORDER BY WRITE DESC ");
                        break;
                    case "RANK":
                        queryStatement.Append(" ORDER BY \"Rank\" DESC ");
                        break;
                    case "TITLE":
                        queryStatement.Append(" ORDER BY \"Title\" ASC ");
                        break;
                    default:
                        // first check no extra values have been added
                        if (ParseExtraSortCriteria(queryString, argumentList, queryStatement) == false)
                            throw new Exception(string.Format("Invalid ORDER BY clause specified. Clause passed was {0}.", sortOrder));
                        break;
                }
            }
            else
            {
                queryStatement.Append(" ORDER BY \"Rank\" DESC ");
            }
        }
        protected virtual bool ParseExtraSortCriteria(NameValueCollection queryString, Hashtable argumentList,StringBuilder queryStatement)
        {
            return false;
        }
    }
}
