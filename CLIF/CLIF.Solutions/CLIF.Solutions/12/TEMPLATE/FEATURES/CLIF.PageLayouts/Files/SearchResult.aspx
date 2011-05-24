<%@ Page Language="C#" MasterPageFile="KCL.Master" Inherits="CLIF.Solutions.Code.SearchResults,CLIF.Solutions.Code, Version=1.0.0.0, Culture=neutral, PublicKeyToken=10028f2947c748cd" %>

<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages"
    Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="OSRVWC" Namespace="Microsoft.Office.Server.WebControls" Assembly="Microsoft.Office.Server, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SPSWC" Namespace="Microsoft.SharePoint.Portal.WebControls"
    Assembly="Microsoft.SharePoint.Portal, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SEARCHWC" Namespace="Microsoft.Office.Server.Search.WebControls"
    Assembly="Microsoft.Office.Server.Search, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="PublishingWebControls" Namespace="Microsoft.SharePoint.Publishing.WebControls"
    Assembly="Microsoft.SharePoint.Publishing, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Assembly="CLIF.Solutions.Code, Version=1.0.0.0, Culture=neutral, PublicKeyToken=10028f2947c748cd"
    Namespace="CLIF.Solutions.Code" TagPrefix="CLIFControls" %>
    
<asp:content ID="Content1" contentplaceholderid="PlaceHolderPageTitle" runat="server">
	<asp:literal ID="Literal1" runat="server" Text="<%$Resources:sps, Search_Results_Page_Title1%>" />
</asp:content>
<asp:content ID="Content2" contentplaceholderid="PlaceHolderAdditionalPageHead" runat="server">
<style type="text/css">
    TD.ms-titleareaframe, Div.ms-titleareaframe, .ms-pagetitleareaframe
    {
        height: 85px;
        text-align: center;
    }
    .ms-pagetitleareaframe table
    {
        background-position: 400px 36px;
        height: 0px;
    }
    .ms-bodyareaframe
    {
        padding-right: 0px;
        padding-left: 18px;
        padding-bottom: 0px;
        padding-top: 0px;
    }
</style>  

</asp:content>
<asp:content ID="Content3" contentplaceholderid="PlaceHolderLeftNavBar" runat="server">
<div height=100% class="ms-pagemargin"><IMG SRC="/_layouts/images/blank.gif" width=8 height=1 alt=""></div>
</asp:content>
<asp:content ID="Content4" contentplaceholderid="PlaceHolderNavSpacer" runat="server">
</asp:content>
<asp:content ID="Content5" contentplaceholderid="PlaceHolderBodyLeftBorder" runat="server">
<div height=100% class="ms-pagemargin"><IMG SRC="/_layouts/images/blank.gif" width=6 height=1 alt=""></div>
</asp:content>
<asp:content ID="Content6" contentplaceholderid="PlaceHolderPageImage" runat="server">
</asp:content>
<asp:content ID="Content7" contentplaceholderid="PlaceHolderTitleLeftBorder" runat="server">
</asp:content>
<asp:content ID="Content8" contentplaceholderid="PlaceHolderSearchArea" runat="server">
</asp:content>
<asp:content ID="Content9" contentplaceholderid="PlaceHolderTitleBreadcrumb" runat="server">
</asp:content>
<asp:content ID="Content10" contentplaceholderid="PlaceHolderPageTitleInTitleArea" runat="server">
<label class="ms-hidden"><asp:literal ID="Literal2" runat="server" Text="<%$Resources:sps, Search_Results_Page_Title1%>" /></label>
</asp:content>
<asp:content ID="Content11" contentplaceholderid="PlaceHolderTitleAreaSeparator" runat="server">
</asp:content>
<asp:content ID="Content12" contentplaceholderid="PlaceHolderMain" runat="server">
<script language="Javascript">
 <!--

 function doClear() 
{
   document.getElementById("ctl00$PlaceHolderMain$txtSearchText").value=""
 }
 //-->
 </script>
<br />
<div >

<div style="background-color:#596E9E; padding-left:10px;">
<br />
<h4>CLIF Document Search:</h4>
Please enter a keyword in the textbox below to seach document(s)<br />
<table border="0" style="font-size:small;">

  <tr>

  <td >
   <strong>Search Document:</strong>
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


</asp:content>
