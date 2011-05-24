<%@ Assembly Name="Microsoft.SharePoint.ApplicationPages, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#" Inherits="CLIF.Solutions.Code.BulkCopy,CLIF.Solutions.Code, Version=1.0.0.0, Culture=neutral, PublicKeyToken=10028f2947c748cd"
    MasterPageFile="AddItemToRepository.master" %>

<%@ Register Assembly="CLIF.Solutions.Code, Version=1.0.0.0, Culture=neutral, PublicKeyToken=10028f2947c748cd"
    Namespace="CLIF.Solutions.Code" TagPrefix="CLIFControls" %>
<asp:Content ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <div style="width: 100%; text-align: center; font-size: x-small">
        <h4>
            <asp:Label ID="lblPageTitle" runat="server" Text="Copy multitple files to the repository"></asp:Label>
        </h4>
        <table border="0" cellpadding="0" cellspacing="0" width="95%">
            <tr>
                <td>
                    <span style="font-size: x-small"><strong>Please select files from the list:
                    </strong></span>
                </td>
            </tr>
            <tr>
                <td valign="top">
                    <div style="font-size: x-small; width:100%; text-align:right">
                        Select <a id="A3" href="#" onclick="javascript: CheckBoxListSelect ('<%= pnlFiles.ClientID %>',true)">
                            All</a> | <a id="A4" href="#" onclick="javascript: CheckBoxListSelect ('<%= pnlFiles.ClientID %>',false)">
                                None</a>
                    </div>
                    <asp:Panel ID="pnlFiles" runat="server" Width="100%" ScrollBars="Vertical" Height="500px"
                        BorderStyle="Inset" BackColor="WhiteSmoke">
                        <asp:CheckBoxList ID="chkFiles" AutoPostBack="true" runat="server" Font-Size="X-Small">
                        </asp:CheckBoxList>
                    </asp:Panel>
                    
                </td>
            </tr>
            <tr>
                <td>
            
                    <asp:Label ID="lblFileSelectCount" Font-Bold="true" Font-Size="XX-Small"
                        runat="server"></asp:Label>
                        
                        
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Button ID="btnContinue" runat="server" OnClick="btnContinue_Clicked" Text="Continue" />
                    <div style="text-align: center; font-size: x-small">
                        <a href="javascript:window.close();">Close Window</a>
                    </div>
                </td>
            </tr>
        </table>
    </div>

    <script language="javascript" type="text/javascript">
 
function CheckBoxListSelect(cbControl, state)
{    
                
       var chkBoxList = document.getElementById(cbControl);
        var chkBoxCount= chkBoxList.getElementsByTagName("input");
        for(var i=0;i<chkBoxCount.length;i++)
        {
            chkBoxCount[i].checked = state;
        }
        
      if(state=="1")
       {   
          document.getElementById('ctl00_PlaceHolderMain_btnContinue').disabled = false;      
          document.getElementById('ctl00_PlaceHolderMain_lblFileSelectCount').innerText=chkBoxCount.length + ' file(s) selected';
        
       }
       else
       {
          document.getElementById('ctl00_PlaceHolderMain_btnContinue').disabled = true; 
         document.getElementById('ctl00_PlaceHolderMain_lblFileSelectCount').innerText='0 file(s) selected';
               
       }     
        
             
        return false; 
}
 function disableButton(buttonId) 
 {
    if (document.all) 
    {
        var btn = document.all[buttonId]; 
        btn.disabled = 'true';
    }
    else 
    {
        var btn = document.getElementById('ctl00$PlaceHolderMain$btnContinue'); btn.disabled = 'true';
    }
}
    </script>

</asp:Content>
