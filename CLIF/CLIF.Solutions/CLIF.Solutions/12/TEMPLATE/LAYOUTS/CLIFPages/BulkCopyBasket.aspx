<%@ Assembly Name="Microsoft.SharePoint.ApplicationPages, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#" Inherits="CLIF.Solutions.Code.BulkCopyBasket,CLIF.Solutions.Code, Version=1.0.0.0, Culture=neutral, PublicKeyToken=10028f2947c748cd"
    MasterPageFile="AddItemToRepository.master" %>

<%@ Register Assembly="CLIF.Solutions.Code, Version=1.0.0.0, Culture=neutral, PublicKeyToken=10028f2947c748cd"
    Namespace="CLIF.Solutions.Code" TagPrefix="CLIFControls" %>
   
    
<asp:Content ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <div style="width:100%; text-align:center; font-size:x-small">
    <br />
    <h4>    
    <asp:Label ID="lblPageTitle" runat="server"  ForeColor="Gray" Text="Copy multitple files to the repository"></asp:Label>
    </h4>
        <table border="0" cellpadding="0" cellspacing="0" width="95%" style="font-size:x-small">            
            <tr>
                <td>
                <strong>
                    List of select file(s):
                    
                </strong>
                </td>
               
            </tr>
            <tr>
                <td>
                <table style="font-size:x-small">
                  <tr>
                      <td valign="top">
                      <asp:ListBox ID="lstFiles" SelectionMode="Single" OnSelectedIndexChanged="lstFiles_OnSelectedIndexChanged" AutoPostBack="true" runat="server" Width="270" Height="450">
                    </asp:ListBox>                  
                      </td>
                      <td style="width:10px">
                      </td>
                      <td valign="top">
                      <div style="background-color:Silver; width:280px; padding-left:10px; padding-bottom:10px; padding-top:10px; height:450">
                      <strong>Title:</strong><br /><asp:Label ID="lblTitle" runat="server" Text=""></asp:Label><br />
                      <br /><strong>Subject:</strong><br /><asp:Label ID="lblContentSubject" runat="server" Text=""></asp:Label><br />
                      <br /><strong>MimeType:</strong><br /><asp:Label ID="lblMimeType" runat="server" Text=""></asp:Label><br />                                                                  
                      <br /><strong>Language:</strong><br /><asp:Label ID="lblContentLanguage" runat="server" Text=""></asp:Label><br />
                      <br /><strong>Source:</strong><br /><asp:Label ID="lblContentSource" runat="server" Text=""></asp:Label><br />
                      <br /><strong>Persistent ID:</strong><br /><asp:Label ID="lblPID" runat="server" Text=""></asp:Label><br />
                      <br /><strong>Publishable Status:</strong><br /><asp:Label ID="lblPublishableStatus" runat="server" Text=""></asp:Label><br />
                      <br />
                      
                      </div>
                      <br />
                      <asp:Button ID="btnRemove" runat="server"  OnClick="btnRemove_Clicked" Text="Remove" />
                      </td>
                  </tr>
                </table>
                 
                </td>
                
            </tr>            
            <tr>
               <td align="left">              
                  <asp:Button ID="btnBack" runat="server"  OnClick="btnBack_Clicked" Text="Back" />
                   <asp:Button ID="btnAdd" runat="server"  OnClick="btnAdd_Clicked" Text="Proceed" />
               </td>
            </tr>
        </table>
         <div style="text-align: center; font-size:x-small">
                        <a href="javascript:window.close();">Close Window</a>
                    </div>
    </div>
</asp:Content>
