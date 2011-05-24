<%@ Assembly Name="Microsoft.SharePoint.ApplicationPages, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#" Inherits="CLIF.Solutions.Code.PublishDocument,CLIF.Solutions.Code, Version=1.0.0.0, Culture=neutral, PublicKeyToken=10028f2947c748cd"
    MasterPageFile="~/_layouts/simple.master" %>

<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="wssuc" TagName="ToolBar" Src="~/_controltemplates/ToolBar.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="ToolBarButton" Src="~/_controltemplates/ToolBarButton.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="InputFormSection" Src="~/_controltemplates/InputFormSection.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="InputFormControl" Src="~/_controltemplates/InputFormControl.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="ButtonSection" Src="~/_controltemplates/ButtonSection.ascx" %>
<%@ Register TagPrefix="wssawc" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<asp:Content ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">

</asp:Content>
<asp:Content ContentPlaceHolderID="PlaceHolderTitleBreadcrumb" runat="server">
    &nbsp;
</asp:Content>
<asp:Content ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server">

 
</asp:Content>
<asp:Content ContentPlaceHolderID="PlaceHolderSiteName" runat="server" />
<asp:Content ContentPlaceHolderID="PlaceHolderMain" runat="server">


<asp:Panel ID="pnlMain" runat="server" >
<div>
 <div  class="ms-pagetitle">               
    Publish this document
</div>
<asp:Label ID="lblMessage" runat="server" ForeColor="Red" Visible="false" Font-Bold="true" /> 
<table class="ms-input" border="0" width="100%" cellpadding="2" cellspacing="3" style="border-color:Black; border:1px; border-style:solid; border-bottom-width:thin; " >
<tr>
<td style="width:150px" >
<strong> Title:</strong>
</td>
<td>
<asp:Label ID="lblTitle" runat="server" />
</td>
</tr>
<tr>
<td>
<strong>Content Subject :</strong>
</td>
<td>
<asp:Label ID="lblContentSubject" runat="server" />
</td>
</tr>

<tr>
<td>
<strong>Content MimeType :</strong>
</td>
<td>
<asp:Label ID="lblContentMimeType" runat="server" />
</td>
</tr>

<tr>
<td>
<strong> Persistent ID :</strong>
</td>
<td>
<asp:Label ID="lblPersistentID" runat="server" />
</td>
</tr>
<tr>
<td>
<strong> Require Approval:</strong>
</td>
<td>
<asp:RadioButtonList ID="rbtRequireApproval" runat="server" Font-Size="11px" RepeatDirection="Horizontal" >
    <asp:ListItem Text="Yes" Value="1" Selected="True" />
    <asp:ListItem Text="No" Value="2" />
</asp:RadioButtonList>
</td>
</tr>
<tr>
<td>
<strong>Publish To :</strong>
</td>
<td>
<asp:DropDownList ID="ddlPublishTo" runat="server" />
</td>
</tr>
<tr>
<td colspan="2">
<asp:RadioButtonList ID="rbtMode" runat="server" Font-Size="11px" BackColor="WhiteSmoke" RepeatDirection="Horizontal" >
    <asp:ListItem Text="Overrite if exists" Value="1" Selected="True" />
    <asp:ListItem Text="Add as new Version" Value="2" />
</asp:RadioButtonList>
</td>
</tr>
<tr>
    <td colspan="2" >
    <br />
    <br />
    <asp:Button ID="btnYes" runat="server" Text="Submit" OnClick="btnYes_Clicked" />
    <asp:Button ID="btnNo" runat="server" Text="Cancel" OnClick="btnNo_Clicked" />
 
    </td>
</tr>
</table>
</div>
<br />
<br />

<br /><br />
</asp:Panel>
</asp:Content>



