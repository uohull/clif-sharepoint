<%@ Assembly Name="Microsoft.SharePoint.ApplicationPages, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" Inherits="CLIF.Solutions.Code.SearchResults,CLIF.Solutions.Code, Version=1.0.0.0, Culture=neutral, PublicKeyToken=10028f2947c748cd"
   MasterPageFile="~/_layouts/application.master"  %>

<%@ Register Assembly="CLIF.Solutions.Code, Version=1.0.0.0, Culture=neutral, PublicKeyToken=10028f2947c748cd"
    Namespace="CLIF.Solutions.Code" TagPrefix="CLIFControls" %>
    
    <asp:Content ID="Content3" ContentPlaceHolderId="PlaceHolderPageTitle" runat="server">
	<SharePoint:EncodedLiteral runat="server" text="<%$Resources:wss,searchresults_pagetitle%>" EncodeMethod='HtmlEncode'/>
	
</asp:Content>
<asp:Content ID="Content4" ContentPlaceholderID="PlaceHolderAdditionalPageHead" runat="server">
   <link rel="stylesheet" type="text/css" href="/_layouts/CLIFPages/Styles/SearchResult.css"/>

  
</asp:Content>

    <asp:Content ID="Content2" ContentPlaceHolderId="PlaceHolderTitleAreaClass" runat="server">
ms-searchresultsareaSeparator
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderId="PlaceHolderNavSpacer" runat="server">
</asp:Content>
<asp:Content ContentPlaceHolderID="PlaceHolderMain" runat="server">
<h4>CLIF Document Search:</h4>
<div >

<div style="padding-left:10px; ">
<br />

Please enter a keyword in the textbox below to seach document(s)<br />
<table border="0" style="font-size:12px" >

  <tr>

  <td >
  Enter search text:
  </td>    
    <td>
      <asp:TextBox ID="txtSearchText" runat="server" Width="120" Font-Size="Small" />
    </td>    
    <td> 
    <asp:Button ID="Sds" runat="server" Text="Search" OnClick="btnSearch_Click"/>         
    </td>    
  </tr>
</table> 
<br />
</div>

<div>
    <CLIFControls:DocumentSearch Id="DocumentSearch" runat="server" Transform-XslName="SearchResults.xsl" />
    <br />
     <br />
</div>
</div>

</asp:Content>
