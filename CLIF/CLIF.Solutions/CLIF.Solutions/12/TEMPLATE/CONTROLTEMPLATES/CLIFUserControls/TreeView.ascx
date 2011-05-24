<%@ Control Language="C#" Inherits="CLIF.Solutions.Code.TreeviewControl,CLIF.Solutions.Code, Version=1.0.0.0, Culture=neutral, PublicKeyToken=10028f2947c748cd"   %>

<%--<!-- Here comes the HTML code that builds the Control. -->
<div id="<%=this.ClientID %>" class="TreeView" 
  service="<%=this.service %>">
  <div class="du" name="/">
    <span class="ft">
      <%=this.title %>
    </span>
  </div>
  <div class="subframe" style="display: none">
    <div class='fl'>
    </div>
  </div>
</div>
--%>
<asp:Literal ID="litDivTags" runat="server" />
<!-- Here comes the script code to bind the behaviour. -->
<script defer="defer" type="text/javascript">
  jcl.LoadBehaviour("<%=this.ClientID %>", TreeViewBehaviour);
</script>
