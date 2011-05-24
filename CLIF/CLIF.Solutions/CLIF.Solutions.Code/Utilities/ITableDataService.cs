// ITableDataService.cs
// Interface for WebServices serving data for a AJAX TableData Control.
// Data provider to a list of city names for the Lookup AJAX Control
// Copyright (c) by Matthias Hertel, http://www.mathertel.de
// This work is licensed under a BSD style license. See http://www.mathertel.de/License.aspx
// ----- 
// 01.09.2005 created by Matthias Hertel
// 27.09.2005 ILookUpService Interface extended.
// 29.09.2005 namespaces rearranged.

using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml;

namespace CLIF.Solutions.Code
{
 

  /// <summary>
  /// Summary description for ITableDataService
  /// </summary>
  public interface ITableDataService {
    /// <summary>Return the ids of all found rows in a sorted order.</summary>
    /// <param name="filter">The filter condition.</param>
    /// <param name="order">The order condition.</param>
    /// <returns></returns>
    [WebMethod(Description = "Return the ids of all found rows in a sorted order.")]
    string Select(string filter, string order);


    /// <summary>Return a single row, identified by the id.</summary>
    /// <param name="id">The id that is unique for the record.</param>
    /// <returns>A record of data as XML Node.</returns>
    [WebMethod(Description = "Return a single row, identified by the id.")]
    XmlNode Fetch(string id);

  } // ITableDataService

} // interface
