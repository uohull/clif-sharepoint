<%--<%@ Assembly Name="Microsoft.SharePoint.ApplicationPages, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c"%> 
<%@ Page Language="C#" Inherits="Microsoft.SharePoint.ApplicationPages.WrkStatPage" MasterPageFile="~/_layouts/application.master"      %> 
--%>
<%@ Assembly Name="CLIF.Solutions.Code, Version=1.0.0.0, Culture=neutral, PublicKeyToken=10028f2947c748cd" %>
<%@ Page Language="C#" Inherits="CLIF.Solutions.Code.WorkflowStatus" MasterPageFile="~/_layouts/application.master" %>

<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %> <%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> <%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> <%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="wssuc" TagName="LinksTable" src="/_controltemplates/LinksTable.ascx" %> <%@ Register TagPrefix="wssuc" TagName="InputFormSection" src="/_controltemplates/InputFormSection.ascx" %> <%@ Register TagPrefix="wssuc" TagName="InputFormControl" src="/_controltemplates/InputFormControl.ascx" %> <%@ Register TagPrefix="wssuc" TagName="LinkSection" src="/_controltemplates/LinkSection.ascx" %> <%@ Register TagPrefix="wssuc" TagName="ButtonSection" src="/_controltemplates/ButtonSection.ascx" %> <%@ Register TagPrefix="wssuc" TagName="ActionBar" src="/_controltemplates/ActionBar.ascx" %> <%@ Register TagPrefix="wssuc" TagName="ToolBar" src="/_controltemplates/ToolBar.ascx" %> <%@ Register TagPrefix="wssuc" TagName="ToolBarButton" src="/_controltemplates/ToolBarButton.ascx" %> <%@ Register TagPrefix="wssuc" TagName="Welcome" src="/_controltemplates/Welcome.ascx" %>
<%@ Register Tagprefix="wssawc" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> <%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Workflow" Namespace="Microsoft.SharePoint.Workflow" Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<asp:Content ID="Content1" ContentPlaceHolderId="PlaceHolderPageTitle" runat="server">
	<SharePoint:EncodedLiteral ID="EncodedLiteral1" runat="server" text="<%$Resources:wss,WrkStat_PageTitle%>" EncodeMethod='HtmlEncode'/>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderId="PlaceHolderPageTitleInTitleArea" runat="server">
	<%SPHttpUtility.HtmlEncode(OnPageTitle,Response.Output);%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderId="PlaceHolderPageImage" runat="server">
	<IMG SRC="/_layouts/images/blank.gif" width=1 height=1 alt="">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderId="PlaceHolderMain" runat="server">
	<table width=100% border=0 cellspacing=0 cellpadding=0 style="padding-top:2px">
		 <tr><td colspan=2 class="ms-linksectionheader" style="padding: 4px;" valign="top">
				   <SharePoint:EncodedLiteral ID="EncodedLiteral2" runat="server" text="<%$Resources:wss,WrkStat_SectionTitle%>" EncodeMethod='HtmlEncode'/>
		 </td></tr>
	</table>
	<br>
	<TABLE border="0" cellspacing="0" cellpadding="0" class="ms-authoringcontrols" style="background-color: transparent">
		<TR height=20 valign=top>
			<TD width=10>&nbsp;</TD>
			<TD nowrap>
				<SharePoint:FormattedString ID="FormattedString1" FormatText="<%$Resources:wss,WrkStat_TextFormat%>" EncodeMethod="HtmlEncodeAllowSimpleTextFormatting" runat="server">
					<SharePoint:EncodedLiteral ID="EncodedLiteral3" runat="server" text="<%$Resources:wss,WrkStat_Owner%>" EncodeMethod='HtmlEncode'/>
				</SharePoint:FormattedString>
				<IMG SRC="/_layouts/images/blank.gif" width=4 height=1 alt="">
			</TD>
			<TD nowrap width=180>
				<asp:Literal ID="Literal_Owner" runat=server>
				</asp:Literal>
			</TD>
			<TD nowrap>
				<SharePoint:FormattedString ID="FormattedString2" FormatText="<%$Resources:wss,WrkStat_TextFormat%>" EncodeMethod="HtmlEncodeAllowSimpleTextFormatting" runat="server">
					<asp:Literal ID="Literal_ItemLabel" runat=server />
				</SharePoint:FormattedString>
				<IMG SRC="/_layouts/images/blank.gif" width=4 height=1 alt="">
			</TD>
			</TD>
			<TD nowrap width=180>
<%
	if (!string.IsNullOrEmpty(StrUrlItem))
	{
%>
		<a href=<%SPHttpUtility.AddQuote(SPHttpUtility.HtmlUrlAttributeEncode(StrUrlItem),Response.Output);%> onclick=<%SPHttpUtility.AddQuote(SPHttpUtility.HtmlEncode(StrOnClickItem),Response.Output);%>>
<%
	}
	SPHttpUtility.HtmlEncode(StrNameItem,Response.Output);
	if (!string.IsNullOrEmpty(StrUrlItem))
	{
%>
		</a>
<%
	}
%>
			</TD>
		</TR>
		<TR height=20 valign=top>
			<TD width=10>&nbsp;</TD>
			<TD nowrap>
				<SharePoint:FormattedString ID="FormattedString3" FormatText="<%$Resources:wss,WrkStat_TextFormat%>" EncodeMethod="HtmlEncodeAllowSimpleTextFormatting" runat="server">
					<SharePoint:EncodedLiteral ID="EncodedLiteral4" runat="server" text="<%$Resources:wss,WrkStat_Started%>" EncodeMethod='HtmlEncode'/>
				</SharePoint:FormattedString>
				<IMG SRC="/_layouts/images/blank.gif" width=4 height=1 alt="">
			</TD>
			<TD nowrap width=180>
				<asp:Literal ID="Literal_Started" runat=server>
				</asp:Literal>
			</TD>
			<TD nowrap>
				<SharePoint:FormattedString ID="FormattedString4" FormatText="<%$Resources:wss,WrkStat_TextFormat%>" EncodeMethod="HtmlEncodeAllowSimpleTextFormatting" runat="server">
					<SharePoint:EncodedLiteral ID="EncodedLiteral5" runat="server" text="<%$Resources:wss,WrkStat_Status%>" EncodeMethod='HtmlEncode'/>
				</SharePoint:FormattedString>
				<IMG SRC="/_layouts/images/blank.gif" width=4 height=1 alt="">
			</TD>
			<TD nowrap width=180>
				<asp:Literal ID="Literal_Status" runat=server>
				</asp:Literal>
			</TD>
		</TR>
		<TR height=20 valign=top>
			<TD width=10>&nbsp;</TD>
			<TD nowrap>
				<SharePoint:FormattedString ID="FormattedString5" FormatText="<%$Resources:wss,WrkStat_TextFormat%>" EncodeMethod="HtmlEncodeAllowSimpleTextFormatting" runat="server">
					<SharePoint:EncodedLiteral ID="EncodedLiteral6" runat="server" text="<%$Resources:wss,WrkStat_LastRun%>" EncodeMethod='HtmlEncode'/>
				</SharePoint:FormattedString>
				<IMG SRC="/_layouts/images/blank.gif" width=4 height=1 alt="">
			</TD>
			<TD nowrap width=180>
				<asp:Literal ID="Literal_LastRun" runat=server>
				</asp:Literal>
			</TD>
			<TD colspan=2>&nbsp;
			</TD>
		</TR>
		<TR height=10 valign=top>
			<TD colspan=5>&nbsp;</TD>
		</TR>
		<div ID="HG_Actions" runat=server>
		<%
		if (allowModifications)
		{
		string WorkflowModString;
		string WorkflowModUrl = Web.Url + "/"
								+ ModificationUrl
								+ "?ID=" + ListItem.ID
								+ "&List=" + Request.QueryString["List"]
								+ "&WorkflowInstanceID=" + StrGuidWorkflow
								+ "&Source=" + SPHttpUtility.UrlKeyValueEncode(Request.RawUrl);
		foreach (DictionaryEntry entry in Modifications)
		{
			SPWorkflowModification mod = (SPWorkflowModification)entry.Value;
			WorkflowModString = (string)WTBase["Modification_" + mod.Id.ToString() + "_Name"];
			if (string.IsNullOrEmpty(WorkflowModString))
				continue;
			string WorkflowModUrlWithSub = WorkflowModUrl + "&ModificationID=" + mod.Id.ToString();
		%>
		<TR height=20 valign=top>
			<TD width=10>&nbsp;</TD>
			<TD class=ms-propertysheet style="<SharePoint:EncodedLiteral runat='server' text='<%$Resources:wss,WrkStat_ActionStyleProperties%>' EncodeMethod='HtmlEncode'/>" nowrap colspan=4>
				<A href=<%SPHttpUtility.AddQuote(SPHttpUtility.HtmlEncode(WorkflowModUrlWithSub),Response.Output);%>>
				<IMG align="absmiddle" SRC="/_layouts/images/setrect.gif" alt="" style="border-width:0px;"/>&nbsp;<%SPHttpUtility.HtmlEncode(WorkflowModString,Response.Output);%></A>
			</TD>
		</TR>
		<%
		}
		}
		%>
		<%
		if (allowCancellation)
		{
		%>
		<TR height=20 valign=top>
			<TD width=10>&nbsp;</TD>
			<TD class=ms-propertysheet style="<SharePoint:EncodedLiteral runat='server' text='<%$Resources:wss,WrkStat_ActionStyleProperties%>' EncodeMethod='HtmlEncode'/>" nowrap colspan=4>
				<br>
				<SharePoint:EncodedLiteral ID="EncodedLiteral7" runat="server" text="<%$Resources:wss,WrkStat_TerminateWarning%>" EncodeMethod='HtmlEncode'/>
				<br>
				<script language="javascript">var endWorkflowConfirm = "<SharePoint:EncodedLiteral runat='server' text='<%$Resources:wss,endworkflow_confirm%>' EncodeMethod='HtmlEncode'/>"</script>
				<Sharepoint:SPLinkButton ID="HtmlAnchorEnd" ImageUrl="/_layouts/images/setrect.gif" OnClientClick="return confirm(endWorkflowConfirm);" runat=server />
			</TD>
		</TR>
		<br>
		<%
		}
		%>
		</div>
	</table>
	<br>
	<table width=100% border=0 cellspacing=0 cellpadding=0 style="padding-top:2px">
		<tr><td colspan=2 class="ms-linksectionheader" style="padding: 4px;" valign="top">
				<SharePoint:EncodedLiteral ID="EncodedLiteral8" runat="server" text="<%$Resources:wss,WrkStat_TaskStatus%>" EncodeMethod='HtmlEncode'/>
			</td>
		</tr>
		<tr><TD valign=top style="padding-left: 7px;padding-top: 0px;padding-bottom: 7px;" class=ms-descriptiontext>
				<div style="font-size: 2pt"><br></div>
				<asp:Label ID="labelTaskSection" runat=server />
			</td>
		</tr>
	</table>
	<asp:PlaceHolder id="idTasksViewError" runat="server" Visible="false">
		<SharePoint:EncodedLiteral ID="EncodedLiteral9" runat="server" text="<%$Resources:wss,WrkStat_TasksViewError%>" EncodeMethod='HtmlEncode'/>
		<br>
	</asp:PlaceHolder>
	<SharePoint:ListViewByQuery ID="idTasksView" DisableSort="true" DisableFilter="true" runat=server />
	<br>
	<table width=100% border=0 cellspacing=0 cellpadding=0 style="padding-top:2px">
		<tr><td colspan=2 class="ms-linksectionheader" style="padding: 4px;" valign="top">
				<SharePoint:EncodedLiteral ID="EncodedLiteral10" runat="server" text="<%$Resources:wss,WrkStat_WorkflowHistory%>" EncodeMethod='HtmlEncode'/>
			</td>
		</tr>
	     <tr><TD valign=top style="padding-left: 7px;padding-top: 0px;padding-bottom: 7px;" class=ms-descriptiontext>
				<SharePoint:FeatureLinks
					ID="ReportLinks"
					runat="server"
					Location="Microsoft.SharePoint.Workflows"
					GroupId="LeftNavBarLinks"
					OnAddLink="AddLink" />
				<div style="font-size: 2pt"><br></div>
				<asp:Label ID="labelHistorySection" runat=server />
			</td>
		</tr>
	</table>
	<asp:PlaceHolder id="idHistoryViewError" runat="server" Visible="false">
		<SharePoint:EncodedLiteral ID="EncodedLiteral11" runat="server" text="<%$Resources:wss,WrkStat_HistoryViewError%>" EncodeMethod='HtmlEncode'/>
		<br>
	</asp:PlaceHolder>
	<SharePoint:ListViewByQuery runat="server" DisableSort="true" DisableFilter="true" ID="idHistoryView" />
	<SharePoint:FormDigest ID="FormDigest1" runat=server/>
</asp:Content>
