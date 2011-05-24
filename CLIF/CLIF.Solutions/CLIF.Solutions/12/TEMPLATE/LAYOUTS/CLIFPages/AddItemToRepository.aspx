<%@ Assembly Name="Microsoft.SharePoint.ApplicationPages, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#" Inherits="CLIF.Solutions.Code.AddItemToRepository,CLIF.Solutions.Code, Version=1.0.0.0, Culture=neutral, PublicKeyToken=10028f2947c748cd"
    MasterPageFile="AddItemToRepository.master" %>

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
                Add to repository
            </div>
            <table class="cf-table" cellspacing="5px" border="0" cellpadding="2px">
                <tr>
                    <td class="cf-td-Label" colspan="2">
                        <strong>Source List:</strong> <br /><asp:Label ID="lblSourceList" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td class="cf-td-Label" colspan="2">
                        <strong>Source File(s) :</strong> <br /><asp:Label ID="lblSourceFileName" runat="server" />
                        <asp:Panel ID="pnlSourceFiles" runat="server" Width="100%">
                            
                             <asp:ListBox ID="lstSourceFiles" BackColor="WhiteSmoke" runat="server" Width="99%"></asp:ListBox>
                        </asp:Panel>
                        
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <strong>Destination folder:</strong>
                        <asp:Panel ID="Panel1" runat="server" ScrollBars="Auto" BackColor="WhiteSmoke" Height="200px" Width="260px"
                            BorderStyle="Inset">
                            <!-- List Repository Settings Values -->
                            <asp:HiddenField ID="hdnNamespaceFormat" runat="server" />
                            <asp:HiddenField ID="hdnLabelFormat" runat="server" />
                            <asp:HiddenField ID="hdnPIDFormat" runat="server" />
                            <CLIFControls:TreeView ID="TreeView1" runat="server" service="proxies.TreeViewService.GetSubNodes"
                                title="Root Folder" />
                        </asp:Panel>
                    </td>
                    <td valign="top">
                        <asp:Panel ID="Panel2" runat="server" ScrollBars="None" Width="260px" BorderStyle="None">
                            <strong>Existing Files</strong>
                            <asp:ListBox ID="lstFiles" BackColor="WhiteSmoke" runat="server" Width="100%" Height="203px"></asp:ListBox>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <strong>Target Object:</strong>
                        <br />
                        <table border="0" cellpadding="2" width="100%" cellspacing="2" style="background-color: #DBDBDB;
                            font-size: xx-small">
                            <tr>
                                <td class="cf-td-Label">
                                    PID:
                                </td>
                                <td>
                                    <label id="lblSelectPID" name="lblSelectPID">
                                    </label>
                                    <input id="hdnSelectPID" name="hdnSelectPID" type="Hidden" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="cf-td-Label">
                                    Label:
                                </td>
                                <td>
                                    <label id="lblSelectFolder" name="lblSelectFolder">
                                    </label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <div style="text-align: left; margin-left: 20px; font-size: x-small">
                            <br />
                            <asp:CheckBox ID="chkIsPrivate" runat="server" Text="Is Private" />
                            <br />
                            <div style="float: right">
                                <asp:Button ID="btnBack" runat="server"  OnClick="btnBack_Clicked" Text="Back" Visible="false" />
                                <input type="button" id="btnAddNewFolder" value="Add New Object/Folder" disabled="disabled"
                                    onclick="CallAddObjectService();" />
                                <asp:Button ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" Text="Submit" />
                            </div>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
        <div style="text-align: center; font-size: x-small">
            <a href="javascript:window.close();">Close Window</a>
        </div>
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlConfirmation" Visible="false">
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        Thank you, request is sumbitted sucessfully!
        <br />
        <a href="javascript:refreshParent();">Close Window</a>
    </asp:Panel>
</asp:Content>
