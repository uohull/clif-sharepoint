<%@ Assembly Name="Microsoft.SharePoint.ApplicationPages, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#" Inherits="CLIF.Solutions.Code.RepositoryExplorer,CLIF.Solutions.Code, Version=1.0.0.0, Culture=neutral, PublicKeyToken=10028f2947c748cd"
    MasterPageFile="AddItemToRepository.master" EnableEventValidation="false" %>

<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Assembly="CLIF.Solutions.Code, Version=1.0.0.0, Culture=neutral, PublicKeyToken=10028f2947c748cd"
    Namespace="CLIF.Solutions.Code" TagPrefix="CLIFControls" %>
<%@ Register TagPrefix="CLIFControls" TagName="TreeView" Src="~/_controltemplates/CLIFUserControls/TreeView.ascx" %>
<asp:Content ID="Content2" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <asp:Panel runat="server" ID="pnlMain" Width="100%">
        <asp:Literal ID="litMessage" runat="server" />
        <div style="margin-left: 20px; width: 100%; background-color: Transparent; text-align: left">
            <br />
            <div class="ms-pagetitle">
                Repository Explorer
            </div>
            <table class="cf-table" cellspacing="5px" border="0" cellpadding="2px">
                <tr>
                    <td>
                        Select Root Folder:
                        <asp:DropDownList ID="ddlRoot" runat="server" BackColor="WhiteSmoke" AutoPostBack="true" OnSelectedIndexChanged="ddlRoot_SelectedIndexChanged">
                            <asp:ListItem Text="MySite Root" Value="0" />
                            <asp:ListItem Text="CLIF Root" Value="1" />
                            <asp:ListItem Text="Publishable Locations" Value="2" />
                        </asp:DropDownList>
                        <asp:DropDownList ID="ddlPublishableLocations" BackColor="WhiteSmoke" Visible="false" runat="server" />
                        <asp:Button ID="btnSubmit" runat="server" Text="Submit" Font-Size="XX-Small" OnClick="btnSearch_Click" />
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <strong>Folder(s):</strong>
                        <asp:Panel ID="Panel1" runat="server" ScrollBars="Auto" BackColor="WhiteSmoke" Height="250px"
                            Width="100%" BorderStyle="Inset">
                            <CLIFControls:TreeView ID="TreeView1" runat="server" service="proxies.TreeViewService.GetSubNodes"
                                title="Root Folder" />
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="Panel2" runat="server" ScrollBars="None" Width="100%" BorderStyle="None">
                        File(s) under
                            
                            <asp:ListBox ID="lstFiles" BackColor="WhiteSmoke" runat="server" Width="100%" Height="250px">
                            </asp:ListBox>
                            
                        </asp:Panel>
                        Selected Folder:<label id="lblSelectPID" name="lblSelectPID"  >(none)</label>
                    </td>
                </tr>
            </table>
        </div>
        <div style="text-align: center; font-size: x-small">
            <a href="javascript:window.close();">Close Window</a>
        </div>
    </asp:Panel>
    <%--<asp:Panel runat="server" ID="pnlConfirmation" Visible="false">
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        Thank you, request is sumbitted sucessfully!
        <br />
        <a href="javascript:refreshParent();">Close Window</a>
    </asp:Panel>--%>
</asp:Content>
