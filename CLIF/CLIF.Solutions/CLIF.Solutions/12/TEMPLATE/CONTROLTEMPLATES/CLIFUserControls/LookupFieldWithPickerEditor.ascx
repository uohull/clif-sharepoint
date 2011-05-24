<%@ Control Language="C#" Inherits="Microsoft.SharePoint.ApplicationPages.LookupFieldEditor,Microsoft.SharePoint.ApplicationPages"     compilationMode="Always" %>
<%@ Register TagPrefix="wssuc" TagName="InputFormControl" src="~/_controltemplates/InputFormControl.ascx" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> <%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> <%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Implements Interface="Microsoft.SharePoint.WebControls.IFieldEditor,Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Assembly Name="CLIF.Solutions.Code, Version=1.0.0.0, Culture=neutral, PublicKeyToken=10028f2947c748cd" %>
<%@ Import Namespace="CLIF.Solutions.Code" %>
<%@ Import Namespace="System.Collections.Generic" %>
<wssuc:InputFormControl runat="server"
	LabelText="<%$Resources:wss,fldedit_getinfofrom%>"
	>
	<Template_Control>
		<asp:Label id="LabelLookupFieldTargetListTitle" runat="server" Visible="False"/>
		<asp:DropDownList id="DdlLookupFieldTargetList" runat="server"
			AutoPostBack="True"
			OnSelectedIndexChanged="DdlLookupFieldTargetList_Changed"
			Title = "<%$Resources:wss,fldedit_getinfofrom%>"
			Visible="False"
			>
		</asp:DropDownList>
	</Template_Control>
</wssuc:InputFormControl>
<wssuc:InputFormControl runat="server"
	LabelText="<%$Resources:wss,fldedit_inthiscolumn%>"
	>
	<Template_Control>
		<asp:DropDownList id="DdlLookupFieldTargetField" runat="server"
			AutoPostBack="True"
			Title="<%$Resources:wss,fldedit_inthiscolumn%>"
			OnSelectedIndexChanged="DdlLookupFieldTargetField_Changed" />
		<asp:Label id="LabelLookupFieldTargetField" runat="server" Visible="False"/>
		<SCRIPT>
		var bConfirmed = false;
		var bClicked = false;
		function ConfirmConvert(event)
		{
			var cbx = event.srcElement;
			if (cbx == null)
				cbx = event.target;
			if (!bClicked && cbx.checked)
			{
				bConfirmed = true;
			}
			if (!cbx.checked && !bConfirmed)
			{
				var msg = "<SharePoint:EncodedLiteral runat='server' text='<%$Resources:wss,fldedit_warn_turnoffmultilookup%>' EncodeMethod='HtmlEncode'/>";
				bConfirmed = confirm(msg);
				cbx.checked = !bConfirmed;
			}
			bClicked = true;
			UpdateDocLibWarning();
			UpdateLengthWarning();
		}
		function UpdateDocLibWarning()
		{
			var cbx = (document.getElementById("<%= cbxAllowMultiValue.ClientID %>"));
			var spanDocLibWarning = (document.getElementById("<%= SpanDocLibWarning.ClientID %>"));
			if (spanDocLibWarning != null)
			{
				if (cbx.checked)
				{
					spanDocLibWarning.style.display = "";
				}
				else
				{
					spanDocLibWarning.style.display = "none";
				}
			}
		}
		function UpdateLengthWarning()
		{
			var cbx = (document.getElementById("<%= cbxUnlimitedLengthInDocLib.ClientID %>"));
			var spanDocLibWarning = (document.getElementById("<%= SpanLengthWarning.ClientID %>"));
			if (spanDocLibWarning != null)
			{
				if (cbx.checked)
				{
					spanDocLibWarning.style.display = "";
				}
				else
				{
					spanDocLibWarning.style.display = "none";
				}
			}
		}
		</SCRIPT>
		<BR/>
		<asp:CheckBox id="cbxAllowMultiValue"
			Text="<%$Resources:wss,fldedit_allowmultivalue%>"
			ToolTip="<%$Resources:wss,fldedit_allowmultivalue%>"
			onClick="ConfirmConvert(event)"
			runat="server" />
		<span class="ms-formvalidation" id="SpanDocLibWarning" runat="server" Visible="false"><br><SharePoint:EncodedLiteral ID="EncodedLiteral1" runat="server" text="<%$Resources:wss,fldedit_MultiLookupWarningForDocLibSupport%>" EncodeMethod='HtmlEncode'/></span>
		<BR/>
		<asp:CheckBox id="cbxUnlimitedLengthInDocLib"
			Text="<%$Resources:wss,fldedit_UnlimitedLengthInDocumentLibrary2%>"
			ToolTip="<%$Resources:wss,fldedit_UnlimitedLengthInDocumentLibrary2%>"
			onClick="UpdateLengthWarning()"
			runat="server" />
		<span class="ms-formvalidation" id="SpanLengthWarning" runat="server" Visible="false"><br><SharePoint:EncodedLiteral ID="EncodedLiteral2" runat="server" text="<%$Resources:wss,fldedit_WarningForUnlimitedLengthInDocumentLibrar%>" EncodeMethod='HtmlEncode'/></span>
	</Template_Control>
</wssuc:InputFormControl>
<wssuc:InputFormControl runat="server" LabelText="<%$Resources:wss,fldedit_defaultvalue%>" >
	<Template_Control>
        <asp:TextBox ID="txtDefault" runat="server"  /><br />
        <span>[CurrentUserId],[UrlParam:<i>paramName</i>]</span>
	</Template_Control>
</wssuc:InputFormControl>
<wssuc:InputFormControl runat="server" LabelText="Filter:">
	<Template_Control>
	    Column from lookup:<asp:DropDownList ID="drpLookupList" runat="server" class="ms-authoringcontrols" />
    	<br />
    	Column in this list:<asp:TextBox ID="txtListColumn" runat="server" CssClass="ms-authoringcontrols" />
    	<br />
    	<asp:DropDownList ID="drpListColumn1" runat="server" class="ms-authoringcontrols" />
	</Template_Control>
</wssuc:InputFormControl>
<wssuc:InputFormControl runat="server" LabelText="Parent Columns:" >
	<Template_Control>
    	
	</Template_Control>
</wssuc:InputFormControl>
<script language="javascript">
	UpdateDocLibWarning();
	UpdateLengthWarning();
</script>

<script runat="server">
    
    private LookupFieldWithPicker lookupField;
      
    protected new void DdlLookupFieldTargetList_Changed(object sender, EventArgs e)
    {          
        base.DdlLookupFieldTargetList_Changed(sender, e);
        //FillDrpdFieldList(this.DdlLookupFieldTargetList.SelectedValue); 
    }

    private void FillDrpdFieldList(string lookupListId)
    {
        EnsureChildControls();

        SPWeb lookupWeb=null;
        SPList lookupList = null;
        if (lookupField != null && lookupField.LookupWebId != null)
        {
           using (SPWeb ObjWeb = SPHelper.GetRootWeb(SPHelper.GetRootUrl(SPContext.Current.Site.Url)))
           {
                lookupList = ObjWeb.Lists[new Guid(lookupListId)];
                drpLookupList.Items.Clear();
                foreach (SPField field in lookupList.Fields)
                {
                    if (LookupFieldWithPickerHelper.IsSearchableField(field))
                    {
                        drpLookupList.Items.Add(new ListItem(field.Title + " " + field.AuthoringInfo, field.Id.ToString()));
                    }
                }
            }            
        }
        else
        {
            lookupWeb = SPContext.Current.Web;
            lookupList = lookupWeb.Lists[new Guid(lookupListId)];
            drpLookupList.Items.Clear();
            foreach (SPField field in lookupList.Fields)
            {
                if (LookupFieldWithPickerHelper.IsSearchableField(field))
                {
                    drpLookupList.Items.Add(new ListItem(field.Title + " " + field.AuthoringInfo, field.Id.ToString()));
                }
            }
        
        }

        //drpListColumn.Items.Clear();
        //SPContentType lookupContent = lookupWeb.ContentTypes[this.];
        ////lookupList = lookupWeb.Lists[SPContext.Current.ListId];
        //foreach (SPField field in lookupContent.Fields)
        //{
        //    if (LookupFieldWithPickerCommon.IsSearchableField(field))
        //    {
        //        drpListColumn.Items.Add(new ListItem(field.Title + " " + field.AuthoringInfo, field.Id.ToString()));
        //    }
        //}

    }

    public new void InitializeWithField(SPField field)
    {
        lookupField = field as LookupFieldWithPicker;
        
        base.InitializeWithField(field);
        
        SPList userInfoList = SPContext.Current.Web.SiteUserInfoList;
        if (DdlLookupFieldTargetList.Items.FindByText(userInfoList.Title) == null)
            DdlLookupFieldTargetList.Items.Add(new ListItem(userInfoList.Title, userInfoList.ID.ToString("B")));
        
        if (this.IsPostBack == false && field==null)
            FillDrpdFieldList(this.DdlLookupFieldTargetList.SelectedValue);

        if (field == null)
            return;

        if (this.IsPostBack == false)
        {
            FillDrpdFieldList(lookupField.LookupList);
            
            List<string> searchFields = lookupField.SearchFields;
            foreach (string searchField in searchFields)
            {
                ListItem item = drpLookupList.Items.FindByValue(searchField);
                if (item != null)
                    item.Selected = true;
            }
            //txtDefault.Text = lookupField.CustomDefaultValue;     
        }
    }
    
    public new void OnSaveChange(SPField field, bool isNewField)
    {
        lookupField = field as LookupFieldWithPicker;

        List<string> searchFields = new List<string>();
        foreach (ListItem item in drpLookupList.Items)
        {
            if (item.Selected)
                searchFields.Add(item.Value);
        }

        lookupField.SearchFields = searchFields;
        lookupField.EntityEditorRows = 1;
       
        base.OnSaveChange(field, isNewField); 
    }
</script>