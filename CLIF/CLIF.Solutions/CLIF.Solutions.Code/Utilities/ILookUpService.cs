// ILookUpService.cs
// Interface for WebServices serving data for the ALAX Lookup Control.
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

namespace CLIF.Solutions.Code
{
 

  /// <summary>
  /// Summary description for ILookUpService
  /// </summary>
  public interface ILookUpService {

    /// <summary>Return Lookup entries that start with the given prefix.</summary>
    /// <param name="prefix">A prefix string</param>
    /// <returns>Semikolon separated list of entries.</returns>
    [WebMethod(Description = "Return Lookup entries that start with the given prefix.")]
    string GetPrefixedEntries(string Prefix);


    /// <summary>Supports the server side verification if a given entry really exists
    /// and returns the correctly writing and full entry.</summary>
    /// <param name="Entry">The given value.</param>
    /// <returns>The correctly written entry.</returns>
    string GetEntry(string Entry);

  } // ILookUpService

} // namespace
