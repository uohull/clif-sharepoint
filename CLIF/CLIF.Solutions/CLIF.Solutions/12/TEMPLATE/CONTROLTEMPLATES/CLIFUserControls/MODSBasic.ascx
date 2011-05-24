<%@ Assembly Name="CLIF.Solutions.Code, Version=1.0.0.0, Culture=neutral, PublicKeyToken=10028f2947c748cd"%>
<%@ Control Language="C#" AutoEventWireup="true" inherits="MODSBasic" EnableViewState="true"%>
<%@ Register Assembly="AjaxControlToolkit, Version=3.5.40412.0, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e"
    Namespace="AjaxControlToolkit" TagPrefix="asp" %> 
<div>
    <h5>Title Info</h5>
    <div class="CssMDField3">
    <div class="CssInputFieldLgr"><div class="CssLabelCellSmlr"><label ID="lbl_title" for="<%#txt_title_MODSBasic.ClientID%>">Title</label></div><div class="CssStdFloatedCellLgr"><input id="txt_title_MODSBasic" type="text" runat="server" class="MODSBasic"/></div></div>    
    </div>           
    <br />
    <h5>Author/creator</h5>    
    <div class="CssMDField3" id="_DivAuthor">
        <div class="RepeatSectionRoot">
	    <div class="RepeatSectionOptions">
        <a class="btn-add-cell"><img  src="/_ControlTemplates/ClifUserControls/images/addctrl.gif" alt="Add another field"/>Add another</a>&nbsp;
        <a class="btn-remove-cell" id="_DivAuthorRepeatSectionRootRemove"><img  src="/_ControlTemplates/ClifUserControls/images/removectrl.gif" alt="Remove this field"/>Remove</a>	
	    </div>
	    <div class="RepeatSectionContentCell">
            <div class="CssInputFieldSmlr"><div class="CssLabelCellStd"><label ID="lbl_namePart" for="<%#rptd_txt_namePart_MODSBasic.ClientID%>">Author/creator</label></div><div class="CssStdFloatedCellSmlr"><input id="rptd_txt_namePart_MODSBasic" type="text" runat="server" class="MODSBasic validator_surname_forename"/></div></div>
            <div class="CssInputFieldSmlr"><div class="CssLabelCellStd"><label ID="lbl_roleType" for="<%#rptd_ddl_roleType_MODSBasic.ClientID%>">Role type</label></div><div class="CssStdFloatedCellSmlr"><asp:DropDownList ID="rptd_ddl_roleType_MODSBasic" runat="server" CssClass="MODSBasic"></asp:DropDownList></div></div>        
            <br/>
            </div>
	    <span class="RepeatSectionNextLevel"></span>
	 </div>
    </div>
    <div class="CssMDField3">
    <div class="CssInputFieldSmlr"><div class="CssLabelCellStd"><label ID="lbl_typeOfResource" for="<%#ddl_typeOfResource_MODSBasic.ClientID%>">Type of resource</label></div><div class="CssStdFloatedCellSmlr"><asp:DropDownList ID="ddl_typeOfResource_MODSBasic" runat="server" class="MODSBasic"></asp:DropDownList></div></div>    
    <div class="CssInputFieldSmlr"><div class="CssLabelCellStd"><label ID="lbl_genre" for="<%#ddl_genre_MODSBasic.ClientID%>">Genre</label></div><div class="CssStdFloatedCellSmlr"><asp:DropDownList ID="ddl_genre_MODSBasic" runat="server" CssClass="MODSBasic"></asp:DropDownList></div></div>    
    </div>
    <br/>    
    <div class="CssMDField4">
    <div class="CssInputFieldSmlr"><div class="CssLabelCellStd"><label ID="lbl_dateIssued" for="<%#txt_dateIssued_MODSBasic.ClientID%>">Date issued</label></div><div class="CssStdFloatedCellSmlr"><asp:TextBox id="txt_dateIssued_MODSBasic" runat="server" class="MODSBasic" ReadOnly=true/><asp:Image
            ID="dateIssued_MODSBasic_Image" runat="server" ImageUrl="/_ControlTemplates/ClifUserControls/images/Calendar-button.png"/></div></div><asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txt_dateIssued_MODSBasic" PopupButtonID="dateIssued_MODSBasic_Image" Format="yyyy-MM-dd"></asp:CalendarExtender>    
    </div>            
    <br/>           
    <div class="CssMDField3">
    <div class="CssInputFieldSmlr"><div class="CssLabelCellStd"><label ID="lbl_langugageTermType" for="<%#ddl_languageTermType_MODSBasic.ClientID%>">Language term type</label></div><div class="CssStdFloatedCellSmlr"><asp:DropDownList ID="ddl_languageTermType_MODSBasic" runat="server" CssClass="MODSBasic"></asp:DropDownList></div></div>    
    <div class="CssInputFieldLgr"><div class="CssLabelCellSmlr"><label ID="lbl_publisher" for="<%#txt_publisher_MODSBasic.ClientID%>">Publisher</label></div><div class="CssStdFloatedCellLgr"><input id="txt_publisher_MODSBasic" type="text" runat="server" class="MODSBasic"/></div></div>    
    </div>           
    <br />    
    <h5>Physical Description</h5>    
    <div class="CssMDField3">
    <div class="CssInputFieldSmlr"><div class="CssLabelCellStd"><label ID="lbl_extent" for="<%#txt_extent_MODSBasic.ClientID%>">Extent</label></div><div class="CssStdFloatedCellSmlr"><input id="txt_extent_MODSBasic" type="text" runat="server" class="MODSBasic" style="color:gray" readonly="readonly"/></div></div>    
    <div class="CssInputFieldSmlr"><div class="CssLabelCellStd"><label ID="lbl_internetMediaType" for="<%#txt_extent_MODSBasic.ClientID%>">Internet media type</label></div><div class="CssStdFloatedCellSmlr"><input id="txt_internetMediaType_MODSBasic" type="text" runat="server" class="MODSBasicHidden" style="color:gray" readonly="readonly"/></div></div>    
    </div>    
    <br />    
    <div class="CssMDField3">
    <div class="CssInputFieldLgr"><div class="CssLabelCellSmlr"><label ID="lbl_digitalOrigin" for="<%#txt_digitalOrigin_MODSBasic.ClientID%>">Digital origin</label></div><div class="CssStdFloatedCellLgr"><input id="txt_digitalOrigin_MODSBasic" type="text" runat="server" class="MODSBasic" style="color:gray" readonly="readonly" text="born digital" value="born digital"/></div></div>    
    </div>        
    <br />
    <div class="CssMDField3">
        <div class="CssInputFieldLgr"><div class="CssLabelCellSmlr"><label ID="lbl_abstract" for="<%#tar_abstract_MODSBasic.ClientID%>">Abstract</label></div><div class="CssStdCellLgr"><textarea id="tar_abstract_MODSBasic" rows="10" runat="server" class="MODSBasic" style="width:100%"></textarea></div></div>
    </div>
    <br />
    <h5>Subject</h5>
    <div class="CssMDField3">    
    <div class="CssInputFieldSmlr"><div class="CssLabelCellStd"><label ID="lbl_subjectAuthority" for="<%#txt_subjectAuthority_MODSBasic.ClientID%>">Authority</label></div><div class="CssStdFloatedCellSmlr"><input id="txt_subjectAuthority_MODSBasic" type="text" runat="server" class="MODSBasic"/></div></div>    
    </div>
    <br />            
    <div class="CssMDField3" id="__DivTopic">
        <div class="RepeatSectionRoot">
	    <div class="RepeatSectionOptions">
        <a class="btn-add-cell"><img  src="/_ControlTemplates/ClifUserControls/images/addctrl.gif" alt="Add another field"/>Add another</a>&nbsp;
        <a class="btn-remove-cell" id="__DivTopicRepeatSectionRootRemove"><img  src="/_ControlTemplates/ClifUserControls/images/removectrl.gif" alt="Remove this field"/>Remove</a>	
	    </div>
	    <div class="RepeatSectionContentCell">	   
        <div class="CssInputFieldLgr"><div class="CssLabelCellRptd"><label for="<%#rptd_txt_topic_MODSBasic.ClientID%>">Topic</label></div><div class="CssStdFloatedCellLgr"><input id="rptd_txt_topic_MODSBasic" type="text" runat="server" class="MODSBasic"/></div></div>
        </div>
	    <span class="RepeatSectionNextLevel"></span>
	    </div>
    </div>
    <h5>Identifier</h5> 
    <div class="CssMDField3">
    <div class="CssInputFieldLgr"><div class="CssLabelCellSmlr"><label ID="lbl_identifier" for="<%#txt_identifier_MODSBasic.ClientID%>">Identifier</label></div><div class="CssStdFloatedCellLgr"><input id="txt_identifier_MODSBasic" type="text" runat="server" class="MODSBasic"/></div></div>    
    </div>
    <h5>Location</h5>
    <div class="CssMDField3" id="__DivUrl">
        <div class="RepeatSectionRoot">
	    <div class="RepeatSectionOptions">
        <a class="btn-add-cell"><img  src="/_ControlTemplates/ClifUserControls/images/addctrl.gif" alt="Add another field"/>Add another</a>&nbsp;
        <a class="btn-remove-cell" id="__DivUrlRepeatSectionRootRemove"><img  src="/_ControlTemplates/ClifUserControls/images/removectrl.gif" alt="Remove this field"/>Remove</a>	
	    </div>
	    <div class="RepeatSectionContentCell">
        <div class="CssInputFieldLgr"><div class="CssLabelCellRptd"><label for="<%#rptd_txt_url_MODSBasic.ClientID%>">Url</label></div><div class="CssStdFloatedCellLgr"><input id="rptd_txt_url_MODSBasic" type="text" runat="server" class="MODSBasic"/></div></div>        
        </div>
	    <span class="RepeatSectionNextLevel"></span>
	    </div>
    </div>
    <div class="CssMDField3" id="__DivNote">
        <div class="RepeatSectionRoot">
	    <div class="RepeatSectionOptions">
        <a class="btn-add-cell"><img  src="/_ControlTemplates/ClifUserControls/images/addctrl.gif" alt="Add another field"/>Add another</a>&nbsp;
        <a class="btn-remove-cell" id="__DivNoteRepeatSectionRootRemove"><img  src="/_ControlTemplates/ClifUserControls/images/removectrl.gif" alt="Remove this field"/>Remove</a>	
	    </div>
	    <div class="RepeatSectionContentCell">  
        <div class="CssInputFieldLgr"><div class="CssLabelCellRptd"><label for="<%#rptd_tar_note_MODSBasic.ClientID%>">Note/citation</label></div><div class="CssStdCellLgrTrow"><textarea id="rptd_tar_note_MODSBasic" rows="10" runat="server" class="MODSBasic" style="width:100%"></textarea></div></div>        
        </div>
	    <span class="RepeatSectionNextLevel"></span>
	    </div>
    </div>
    <h5>Rights</h5>
    <div class="CssMDField4">    
    <input type="checkbox" ID="main_ckbc_accessCondition_MODSBasic" runat="server" class="MODSBasic"/><asp:Label ID="lbl_main_accessCondition" runat="server"></asp:Label>    
    <br/>
    <asp:PlaceHolder ID="accessConditionsPlaceHolder" runat="server"></asp:PlaceHolder>
    </div>
    <br />       
    <input id="hidden_txt_internetMediaType_MODSBasic" name="hidden_txt_internetMediaType_MODSBasic" type="hidden" value="" class="MODSBasic CssCheckBox" runat="server"/>    
    <input id="__BASIC_MODS_XML_DATA" name="__BASIC_MODS_XML_DATA" type="hidden" value=""/>    
<asp:CascadingDropDown runat="server" ID="CascadingDropDown1"   
                       Category="___ResourceTypes"
                       TargetControlID="ddl_typeOfResource_MODSBasic"
                       LoadingText="Acquiring ..."
                       PromptText="Select type of resource"
                       ServiceMethod="GetResourceTypeGenres"
                       ServicePath="/_controltemplates/CLIFUserControls/AJAXCtrlsHlpr.asmx">
</asp:CascadingDropDown>
<asp:CascadingDropDown runat="server" ID="CascadingDropDown2"
                       Category="___"
                       TargetControlID="ddl_genre_MODSBasic"
                       ParentControlID="ddl_typeOfResource_MODSBasic" 
                       LoadingText="Acquiring ..."
                       PromptText="Select genre"
                       ServiceMethod="GetResourceTypeGenres"
                       ServicePath="/_controltemplates/CLIFUserControls/AJAXCtrlsHlpr.asmx">
</asp:CascadingDropDown>
</div>
