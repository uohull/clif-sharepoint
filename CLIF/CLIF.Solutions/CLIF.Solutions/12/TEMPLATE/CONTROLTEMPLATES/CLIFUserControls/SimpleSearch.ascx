<%@ Register Tagprefix="PublishingWebControls" Namespace="Microsoft.SharePoint.Publishing.WebControls" Assembly="Microsoft.SharePoint.Publishing, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" Inherits="CLIF.Solutions.Code.SimpleSearch,CLIF.Solutions.Code, Version=1.0.0.0, Culture=neutral, PublicKeyToken=10028f2947c748cd" %>
<script language="Javascript">
 <!--

function doClear() 
{
   document.getElementById("ctl00$PlaceHolderSearchArea$SimpleSearch$txtSearchText").value=""
}
 //-->
 </script>
 
<table border="0" style="font-size:11px; color:#02569e">
  <tr>
    <td>
        Search 
    </td>  
    <td>
      <asp:TextBox ID="txtSearchText" BorderStyle="Solid" BorderWidth="1px" BorderColor="#02569e" Height="18px" runat="server" Width="100" />
    </td>    
    <td>    
      <asp:ImageButton ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" ImageUrl="~/_layouts/images/searchqry.gif" />
    </td>
    <td>
        &nbsp;&nbsp;</td>   
  </tr>
</table> 