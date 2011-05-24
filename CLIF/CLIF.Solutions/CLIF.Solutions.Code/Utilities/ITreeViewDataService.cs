// ITreeViewDataService.cs
// Interface for WebServices serving data for a AJAX TreeView Control.
// Copyright (c) by Matthias Hertel, http://www.mathertel.de
// This work is licensed under a BSD style license. See http://www.mathertel.de/License.aspx
// ----- 
// 07.01.2006 created by Matthias Hertel

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
  /// Definition of the methods that must be available in a WebService
  /// for a AJAX TreeView Control.
  /// </summary>
  public interface ITreeViewDataService {

    /// <summary>Retrieve the sub nodes of a specific folder.</summary>
    /// <param name="path">a path to a folder.</param>
    /// <returns>A XML document (tree) containing folder and file nodes.</returns>
    [WebMethod(Description = "Get the subnodes of a given folder path.")]
    XmlNode GetSubNodes(string path);

  } // ITreeViewDataService

} // interface
