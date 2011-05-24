<%@ Page Language="C#" MasterPageFile="~/_layouts/application.master" CodeBehind="cliferror.aspx.cs" Inherits="CLIF.Solutions.ApplicationPages.cliferror, CLIF.Solutions, Version=1.0.0.0, Culture=neutral, PublicKeyToken=e9e88222f01b7472" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<asp:Content ID="Content1" ContentPlaceHolderId ="PlaceHolderTitleLeftBorder" runat="server">
 <div class="ms-titleareaframe"><IMG SRC="/_layouts/images/blank.gif" width=1 height=100% alt=""></div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderId ="PlaceHolderTitleRightMargin" runat="server">
 <div style="height:100%;" class="ms-titleareaframe"><IMG SRC="/_layouts/images/blank.gif" width=1 height=1 alt=""></div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderId ="PlaceHolderBodyLeftBorder" runat="server">
 <div style="height:100%;" class="ms-pagemargin"><IMG SRC="/_layouts/images/blank.gif" width=10 height=1 alt=""></div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderId ="PlaceHolderBodyRightMargin" runat="server">
 <div style="height:100%;" class="ms-pagemargin"><IMG SRC="/_layouts/images/blank.gif" width=10 height=1 alt=""></div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderId="PlaceHolderTitleAreaClass" runat="server">ms-pagetitleareaframe</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderId="PlaceHolderBodyAreaClass" runat="server">ms-viewareaframe</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderId="PlaceHolderPageTitle" runat="server">
<SharePoint:EncodedLiteral ID="EncodedLiteral1" runat="server" text="Page title" EncodeMethod='HtmlEncode'/>
</asp:Content>
<asp:Content ID="Content8" ContentPlaceHolderId="PlaceHolderPageTitleInTitleArea" runat="server">
<SharePoint:EncodedLiteral ID="EncodedLiteral2" runat="server" text="Page title" EncodeMethod='HtmlEncode'/>
</asp:Content>
<asp:Content ID="Content9" ContentPlaceHolderId="PlaceHolderPageImage" runat="server"><SharePoint:AlphaImage ID="AlphaImage1" src="/_layouts/images/allcontents.png" Height="54" Width="145" Alt="" runat="server"/></asp:Content>
<asp:Content ID="Content10" ContentPlaceHolderId="PlaceHolderAdditionalPageHead" runat="server">
	<script>
		var navBarHelpOverrideKey = "ListOLists";
	</script>
</asp:Content>
<asp:Content ID="Content11" ContentPlaceHolderId="PlaceHolderLeftNavBar" runat="server">
				<div class="ms-quicklaunchouter">
				<div class="ms-quickLaunch" style="width:100%">
				<h3 class="ms-standardheader"><label class="ms-hidden"><SharePoint:EncodedLiteral ID="EncodedLiteral3" runat="server" text="<%$Resources:wss,quiklnch_pagetitle%>" EncodeMethod="HtmlEncode"/></label>
				<Sharepoint:SPSecurityTrimmedControl ID="SPSecurityTrimmedControl1" runat="server" PermissionsString="ViewFormPages">
				<div class="ms-quicklaunchheader"><SharePoint:SPLinkButton id="idNavLinkViewAll" runat="server" NavigateUrl="~site/_layouts/viewlsts.aspx" Text="<%$Resources:wss,quiklnch_allcontent%>" AccessKey="<%$Resources:wss,quiklnch_allcontent_AK%>"/></div>
				</SharePoint:SPSecurityTrimmedControl>
				</h3>
				<Sharepoint:SPNavigationManager
				id="QuickLaunchNavigationManager"
				runat="server"
				QuickLaunchControlId="QuickLaunchMenu"
				ContainedControl="QuickLaunch"
				EnableViewState="false"
				>
				<div>
					<SharePoint:DelegateControl ID="DelegateControl1" runat="server"
						ControlId="QuickLaunchDataSource">
					 <Template_Controls>
						<asp:SiteMapDataSource
						SiteMapProvider="SPNavigationProvider"
						ShowStartingNode="False"
						id="QuickLaunchSiteMap"
						StartingNodeUrl="sid:1025"
						runat="server"
						/>
					 </Template_Controls>
					</SharePoint:DelegateControl>
					<SharePoint:AspMenu
					id="QuickLaunchMenu"
					DataSourceId="QuickLaunchSiteMap"
					runat="server"
					Orientation="Vertical"
					StaticDisplayLevels="2"
					ItemWrap="true"
					MaximumDynamicDisplayLevels="0"
					StaticSubMenuIndent="0"
					SkipLinkText=""
					>
					<LevelMenuItemStyles>
						<asp:MenuItemStyle CssClass="ms-navheader"/>
						<asp:MenuItemStyle CssClass="ms-navitem"/>
					</LevelMenuItemStyles>
					<LevelSubMenuStyles>
						<asp:SubMenuStyle CssClass="ms-navSubMenu1"/>
						<asp:SubMenuStyle CssClass="ms-navSubMenu2"/>
					</LevelSubMenuStyles>
					<LevelSelectedStyles>
						<asp:MenuItemStyle CssClass="ms-selectednavheader"/>
						<asp:MenuItemStyle CssClass="ms-selectednav"/>
					</LevelSelectedStyles>
				</SharePoint:AspMenu>
				</div>
				</Sharepoint:SPNavigationManager>
				<Sharepoint:SPNavigationManager
				id="TreeViewNavigationManager"
				runat="server"
				ContainedControl="TreeView"
				>
				  <table class="ms-navSubMenu1" cellpadding="0" cellspacing="0" border="0">
					<tr>
					  <td>
						<table class="ms-navheader" width="100%" cellpadding="0" cellspacing="0" border="0">
						  <tr>
							<td nowrap id="idSiteHierarchy">
							  <SharePoint:SPLinkButton runat="server" NavigateUrl="~site/_layouts/viewlsts.aspx" id="idNavLinkSiteHierarchy" Text="<%$Resources:wss,treeview_header%>" AccessKey="<%$Resources:wss,quiklnch_allcontent_AK%>"/>
							</td>
						  </tr>
						</table>
					  </td>
					</tr>
				  </table>
				  <div class="ms-treeviewouter">
					<SharePoint:SPHierarchyDataSourceControl
					 runat="server"
					 id="TreeViewDataSource"
					 RootContextObject="Web"
					 IncludeDiscussionFolders="true"
					/>
					<SharePoint:SPRememberScroll runat="server" id="TreeViewRememberScroll" onscroll="javascript:_spRecordScrollPositions(this);" Style="overflow: auto;height: 400px;width: 150px; ">
					<Sharepoint:SPTreeView
						id="WebTreeView"
						runat="server"
						ShowLines="false"
						DataSourceId="TreeViewDataSource"
						ExpandDepth="0"
						SelectedNodeStyle-CssClass="ms-tvselected"
						NodeStyle-CssClass="ms-navitem"
						NodeStyle-HorizontalPadding="2"
						SkipLinkText=""
						NodeIndent="12"
						ExpandImageUrl="/_layouts/images/tvplus.gif"
						CollapseImageUrl="/_layouts/images/tvminus.gif"
						NoExpandImageUrl="/_layouts/images/tvblank.gif"
					>
					</Sharepoint:SPTreeView>
					</Sharepoint:SPRememberScroll>
				  </div>
				</Sharepoint:SPNavigationManager>
				<table width="100%" cellpadding="0" cellspacing="0" border="0">
				<tr><td>
		<table class="ms-recyclebin" width="100%" cellpadding="0" cellspacing="0" border="0">
		<tr><td nowrap>
		<SharePoint:SPLinkButton runat="server" NavigateUrl="~site/_layouts/recyclebin.aspx" id="idNavLinkRecycleBin" ImageUrl="/_layouts/images/recycbin.gif" Text="<%$Resources:wss,StsDefault_RecycleBin%>" PermissionsString="DeleteListItems" />
		</td></tr>
		</table>
		</td></tr></table>
				</div>
				</div>
</asp:Content>
<asp:Content ID="Content12" ContentPlaceHolderId="PlaceHolderPageDescription" runat="server">
	<asp:Label id="LabelPageDescription" runat="server"/>
</asp:Content>
<asp:Content ID="Content13" ContentPlaceHolderId="PlaceHolderPageDescriptionRowAttr" runat="server">
style="display:none;"
</asp:Content>
<asp:Content ID="Content14" ContentPlaceHolderId="PlaceHolderPageDescriptionRowAttr2" runat="server">
style="display:none;"
</asp:Content>
<asp:Content ID="Content15" ContentPlaceHolderId="PlaceHolderMain" runat="server">
</asp:Content>
