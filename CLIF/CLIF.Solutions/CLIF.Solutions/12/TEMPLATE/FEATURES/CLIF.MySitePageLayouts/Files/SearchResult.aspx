<%@ Page Language="C#" MasterPageFile="KCL_MySite.Master" Inherits="CLIF.Solutions.Code.SearchResults,CLIF.Solutions.Code, Version=1.0.0.0, Culture=neutral, PublicKeyToken=10028f2947c748cd" %>

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
<asp:Content ID="Content1" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
    <asp:Literal ID="Literal1" runat="server" Text="<%$Resources:sps, Search_Results_Page_Title1%>" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
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
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PlaceHolderLeftNavBar" runat="server">
    <div height="100%" class="ms-pagemargin">
        <img src="/_layouts/images/blank.gif" width="8" height="1" alt=""></div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="PlaceHolderNavSpacer" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="PlaceHolderBodyLeftBorder" runat="server">
    <div height="100%" class="ms-pagemargin">
        <img src="/_layouts/images/blank.gif" width="6" height="1" alt=""></div>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="PlaceHolderPageImage" runat="server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="PlaceHolderTitleLeftBorder" runat="server">
</asp:Content>
<asp:Content ID="Content8" ContentPlaceHolderID="PlaceHolderSearchArea" runat="server">
</asp:Content>
<asp:Content ID="Content9" ContentPlaceHolderID="PlaceHolderTitleBreadcrumb" runat="server">
</asp:Content>
<asp:Content ID="Content10" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea"
    runat="server">
    <label class="ms-hidden">
        <asp:Literal ID="Literal2" runat="server" Text="<%$Resources:sps, Search_Results_Page_Title1%>" /></label>
</asp:Content>
<asp:Content ID="Content11" ContentPlaceHolderID="PlaceHolderTitleAreaSeparator"
    runat="server">
</asp:Content>
<asp:Content ID="Content12" ContentPlaceHolderID="PlaceHolderMain" runat="server">

    <script language="Javascript">
 <!--

 function doClear() 
{
   document.getElementById("ctl00$PlaceHolderMain$txtSearchText").value=""
 }
 //-->
    </script>


    <div>
        <div style="background-color: #596E9E; padding-left: 10px;">
            <h4>
                CLIF Document Search:</h4>
            Please enter a keyword in the textbox below to seach document(s)<br />
            <table border="0" style="font-size: small;">
                <tr>
                    <td>
                        <strong>Search Document:</strong>
                    </td>
                    <td>
                        <asp:TextBox ID="txtSearchText" runat="server" Width="120" Font-Size="Small" />
                    </td>
                    <td>
                        <asp:Button ID="Sds" runat="server" Text="Search" OnClick="btnSearch_Click" />
                    </td>
                </tr>
            </table>
            <br />
        </div>
        <div>
            <CLIFControls:DocumentSearch ID="DocumentSearch" runat="server" Transform-XslName="SearchResults.xsl" />
            <br />
            <br />
        </div>
    </div>
</asp:Content>
