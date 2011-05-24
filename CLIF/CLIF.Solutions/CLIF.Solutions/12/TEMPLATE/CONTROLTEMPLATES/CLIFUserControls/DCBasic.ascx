<%@ Assembly Name="CLIF.Solutions.Code, Version=1.0.0.0, Culture=neutral, PublicKeyToken=10028f2947c748cd"%>
<%@ Control Language="C#" AutoEventWireup="true" inherits="DCBasic" EnableViewState="true"%>
<%@ Register Assembly="AjaxControlToolkit, Version=3.5.40412.0, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e"
    Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<div>
    <h4>Dublic Core Metadata</h4>
    <div class="CssMDField3">
    <div class="CssInputFieldLgr"><div class="CssLabelCellSmlr"><label ID="lbl_title_dc" for="<%#txt_title_DCBasic.ClientID%>">Title</label></div><div class="CssStdFloatedCellLgr"><input id="txt_title_DCBasic" type="text" runat="server" class="DCBasic"/></div></div>    
    </div>
    <br />         
    <div class="CssMDField3">
    <div class="CssInputFieldSmlr"><div class="CssLabelCellStd"><label ID="lbl_creator_dc" for="<%#txt_creator_DCBasic.ClientID%>">Author/creator</label></div><div class="CssStdFloatedCellSmlr"><input id="txt_creator_DCBasic" type="text" runat="server" class="DCBasic validator_surname_forename"/></div></div>    
    <div class="CssInputFieldSmlr"><div class="CssLabelCellStd"><label ID="lbl_identifier_dc" for="<%#txt_identifier_DCBasic.ClientID%>">Identifier</label></div><div class="CssStdFloatedCellSmlr"><input id="txt_identifier_DCBasic" type="text" runat="server" class="DCBasic"/></div></div>    
    </div>    
    <br />    
    <div class="CssMDField3">
    <div class="CssInputFieldSmlr"><div class="CssLabelCellStd"><label ID="lbl_contributor_dc" for="<%#txt_contributor_DCBasic.ClientID%>">Contributor</label></div><div class="CssStdFloatedCellSmlr"><input id="txt_contributor_DCBasic" type="text" runat="server" class="DCBasic validator_surname_forename"/></div></div>    
    <div class="CssInputFieldSmlr"><div class="CssLabelCellStd"><label ID="lbl_coverage_dc" for="<%#txt_coverage_DCBasic.ClientID%>">Coverage</label></div><div class="CssStdFloatedCellSmlr"><input id="txt_coverage_DCBasic" type="text" runat="server" class="DCBasic"/></div></div>    
    </div> 
    <br />              
    <div class="CssMDField3">
    <div class="CssInputFieldSmlr"><div class="CssLabelCellStd"><label ID="lbl_language_dc" for="<%#ddl_language_DCBasic.ClientID%>">Language</label></div><div class="CssStdFloatedCellSmlr"><asp:DropDownList ID="ddl_language_DCBasic" CssClass="DCBasic" runat="server"/></div></div>    
    <div class="CssInputFieldSmlr"><div class="CssLabelCellStd"><label ID="lbl_format_dc" for="<%#txt_format_DCBasic.ClientID%>">Format</label></div><div class="CssStdFloatedCellSmlr"><input id="txt_format_DCBasic" type="text" runat="server" class="DCBasicHidden" readonly="true" style="color:gray"/></div></div>    
    </div>
    <br />            
    <div class="CssMDField3">
    <div class="CssInputFieldSmlr"><div class="CssLabelCellStd"><label ID="lbl_source_dc" for="<%#txt_source_DCBasic.ClientID%>">Source</label></div><div class="CssStdFloatedCellSmlr"><input id="txt_source_DCBasic" type="text" runat="server" class="DCBasic"/></div></div>    
    <div class="CssInputFieldSmlr"><div class="CssLabelCellStd"><label ID="lbl_subject_dc" for="<%#txt_subject_DCBasic.ClientID%>">Subject</label></div><div class="CssStdFloatedCellSmlr"><input id="txt_subject_DCBasic" type="text" runat="server" class="DCBasic"/></div></div>    
    </div>
    <br />
    <div class="CssMDField4">
    <div class="CssInputFieldSmlr"><div class="CssLabelCellStd"><label ID="lbl_date_dc" for="<%#txt_date_DCBasic.ClientID%>">Date issued</label></div><div class="CssStdFloatedCellSmlr"><asp:TextBox id="txt_date_DCBasic" runat="server" class="DCBasic" ReadOnly=true/><asp:Image
            ID="date_DCBasic_Image" runat="server" ImageUrl="/_ControlTemplates/ClifUserControls/images/Calendar-button.png"/></div></div><asp:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txt_date_DCBasic" PopupButtonID="date_DCBasic_Image" Format="yyyy-MM-dd"></asp:CalendarExtender>    
    </div>                            
    <div class="CssMDField3">
    <div class="CssInputFieldSmlr"><div class="CssLabelCellStd"><label ID="lbl_type_dc" for="<%#cmb_type_DCBasic.ClientID%>">Type</label></div><div class="CssStdFloatedCellSmlr"><asp:ComboBox ID="cmb_type_DCBasic" runat="server" class="DCAJAXBasic" DropDownStyle="DropDown" EnableViewState="true"/></div></div>    
    </div>
    <br />           
    <div class="CssMDField3">
    <div class="CssInputFieldLgr"><div class="CssLabelCellSmlr"><label ID="lbl_publisher_dc" for="<%#txt_publisher_DCBasic.ClientID%>">Publisher</label></div><div class="CssStdFloatedCellLgr"><input id="txt_publisher_DCBasic" type="text" runat="server" class="DCBasic"/></div></div>    
    </div>    
    <br />    
    <div class="CssMDField3" id="__DivDCRelation">
        <div class="RepeatSectionRoot">
	    <div class="RepeatSectionOptions">
        <a class="btn-add-cell"><img  src="/_ControlTemplates/ClifUserControls/images/addctrl.gif" alt="Add another field"/>Add another</a>&nbsp;
        <a class="btn-remove-cell" id="__DivTopicRepeatSectionRootRemove"><img  src="/_ControlTemplates/ClifUserControls/images/removectrl.gif" alt="Remove this field"/>Remove</a>	
	    </div>
	    <div class="RepeatSectionContentCell">	   
        <div class="CssInputFieldLgr"><div class="CssLabelCellRptd"><label for="<%#rptd_txt_relation_DCBasic.ClientID%>">Relation</label></div><div class="CssStdFloatedCellLgr"><input id="rptd_txt_relation_DCBasic" type="text" runat="server" class="DCBasic"/></div></div>
        </div>
	    <span class="RepeatSectionNextLevel"></span>
	    </div>
    </div>          
    <div class="CssMDField3">
        <div class="CssInputFieldLgr"><div class="CssLabelCellSmlr"><label ID="lbl_description_dc" for="<%#tar_description_DCBasic.ClientID%>">Description</label></div><div class="CssStdCellLgr"><textarea id="tar_description_DCBasic" rows="10" runat="server" class="DCBasic" style="width:100%"></textarea></div></div>
    </div>                   
    <h4>Rights</h4>
    <div class="CssMDField4">     
    <input type="checkbox" ID="main_ckbc_rights_DCBasic" runat="server" class="DCBasic"/><asp:Label ID="lbl_main_rights" runat="server"></asp:Label><br/>        
    <asp:PlaceHolder ID="rightsPlaceHolder" runat="server"></asp:PlaceHolder>
    </div>
    <br /> 
    <input id="hidden_txt_format_DCBasic" name="hidden_txt_format_DCBasic" type="hidden" value="" class="DCBasic" runat="server"/>           
    <input id="__BASIC_DC_XML_DATA" name="__BASIC_DC_XML_DATA" type="hidden" value=""/>    
</div>
