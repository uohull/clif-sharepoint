<%@ Assembly Name="Microsoft.SharePoint,Version=12.0.0.0,Culture=neutral,PublicKeyToken=71E9BCE111E9429C"%>
<%@ Assembly Name="Hydranet,Version=1.0.0.0,Culture=neutral,PublicKeyToken=1f09f77877e589fb"%>
<%@ Assembly Name="WebUtils,Version=1.0.0.0,Culture=neutral,PublicKeyToken=d7a37793abe7fa2c"%>
<%@ Assembly Name="Microsoft.Office.InfoPath,Version=12.0.0.0,Culture=neutral,PublicKeyToken=71e9bce111e9429c"%>
<%@ Assembly Name="Microsoft.Office.InfoPath.Server,Version=12.0.0.0,Culture=neutral,PublicKeyToken=71e9bce111e9429c"%>
<%@ Page Language="C#" MasterPageFile="~/_layouts/application.master"  Inherits="CLIF.Solutions.Code.ClifSendFedora,CLIF.Solutions.Code, Version=1.0.0.0, Culture=neutral, PublicKeyToken=10028f2947c748cd" EnableSessionState="True" EnableViewState="True" EnableViewStateMac="True" EnableEventValidation="False"%>   
<%@ Register Assembly="AjaxControlToolkit, Version=3.5.40412.0, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register assembly="Microsoft.Office.InfoPath.Server, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" namespace="Microsoft.Office.InfoPath.Server.Controls" tagprefix="cc1" %>

<asp:Content ID="PageTitle" ContentPlaceHolderId="PlaceHolderPageTitleInTitleArea" runat="server">
	Copy to repository
</asp:Content>
<asp:Content ID="PageHeader" ContentPlaceHolderId="PlaceHolderAdditionalPageHead" runat="server">
<link href="css/styles.css" rel="stylesheet" type="text/css" />
<script type="text/javascript" src="js/jQuery-1.4.1.js"></script>
<script type="text/javascript" src="js/plugins/validate/jquery.validate.js"></script>

<script type="text/javascript">

    /// ShowHideElement. Shows or hides a DOM element by positioning it off the pages or back into its previous position
    ///     
    ///
    function ShowHideElement(elemId)
    {
        var elem = document.getElementById(elemId);
        if(elem)
        {
            if((!elem.style.visibility) || (elem.style.visibility=="") || (elem.style.visibility=="visible"))        
            {
                elem.style.visibility="hidden";

		        if(elem.style.left)
		        {
		           var newPos = (0 + parseInt(elem.style.left)) - 2000;
		           elem.style.left= newPos + "px"; 
	            }
		        else
		            elem.style.left="-2000px";

		        elem.style.position = "absolute";

                return false;
            }
            else
            {
                elem.style.visibility="visible"; 

		        if(elem.style.left)
		        {
		           var newPos = (0 + parseInt(elem.style.left)) + 2000;
		           elem.style.left= newPos + "px"; 
	            }
		        else
		            elem.style.left="0px";

		        elem.style.position = "static";        
                return true;
            }
        }

    }

    /// HideElement. Hides a DOM element by positioning it off the page
    ///     
    ///
    function HideElement(elemId)
    {
        var elem = document.getElementById(elemId);
        if(elem)
        {
		    if(elem.style.position != "absolute")
		    {
                elem.style.visibility="hidden";

			    if(elem.style.left)
			    {
		 	       var newPos = (0 + parseInt(elem.style.left)) - 2000;
		 	       elem.style.left= newPos + "px"; 
	       	 	}
			    else
		    	   elem.style.left="-2000px";

			    elem.style.position = "absolute";
		    }             
	    }

	    return false;
    }

    /// GetDocElementById. Returns the DOM element matching the given Id
    ///     
    ///
    function GetDocElementById(id)
    {
        return $("*[id*='" + id + "']")[0];
    }

    /// ShowIPMODs. Performs actions to cause the infopath MODS metadata form to appear (Full page postback required) 
    ///     
    ///
    function ShowIPMODs()
    {     
        postbackctrl = GetDocElementById('<%#txtPostbackCtrlIds.ClientID%>');
        
        if(postbackctrl.value.indexOf('ckshowIPMODs',0) < 0)
        {
            SetPostBackMDCtrls('ckShowBasicMODs');
            SubmitPage();        
        }
        else
        {             
            ShowHideElement('<%#pnlAddMODs.ClientID%>');
        }           
    }
    
    /// ShowBasicMODs. Performs actions to cause the cut-down MODS metadata form to appear
    ///     
    ///    
    function ShowBasicMODs()
    {
        postbackctrl = GetDocElementById('<%#txtPostbackCtrlIds.ClientID%>');

        if((postbackctrl.value == '') || postbackctrl.value.indexOf('ckShowBasicMODs',0) < 0)
        {
            SetPostBackMDCtrls('ckshowIPMODs');           
            SubmitPage();        
        }
        else
        {          
            ShowHideElement('<%#pnlAddMODs.ClientID%>');
        }
    }

    /// ShowDC. Performs actions to cause the DC metadata form to appear
    ///     
    /// 
    function ShowDC()
    {  
	    var ckshowipmods = GetDocElementById('<%#ckshowIPMODs.ClientID%>');
	    var ckshowbasicmods = GetDocElementById('<%#ckShowBasicMODs.ClientID%>'); 
	    if(!(ckshowbasicmods.checked || ckshowipmods.checked))
		    HideElement('<%#pnlAddMODs.ClientID%>')
       
        ShowHideElement('<%#pnlAddDC.ClientID%>');
    }
  
    /// RestoreVisibleForms. Following a page postback restores the previously visible forms
    ///     
    ///  
    function RestoreVisibleForms()	
    {
        var ckshowdc = GetDocElementById('<%#ckShowDC.ClientID%>');       

	    if(ckshowdc.checked)
		    ShowDC();
    }  

    /// SetPostBackMDCtrls. Stores the user's forms selections
    ///     
    ///  
    function SetPostBackMDCtrls(ignoreCtrl)
    {
        var postbackctrl = GetDocElementById('<%#txtPostbackCtrlIds.ClientID%>');
        var ckshowipmods = GetDocElementById('<%#ckshowIPMODs.ClientID%>');    
        var ckshowbasicmods = GetDocElementById('<%#ckShowBasicMODs.ClientID%>');
        var ckshowdc = GetDocElementById('<%#ckShowDC.ClientID%>');       
         
        postbackctrl.value = '';         
        postbackarray = new Array();
                    
        if(ckshowdc.checked)
            postbackarray[postbackarray.length] = 'ckShowDC';
           
        if(ckshowipmods.checked && ((!ignoreCtrl) || (ignoreCtrl && ignoreCtrl != 'ckshowIPMODs')))
            postbackarray[postbackarray.length] = 'ckshowIPMODs';
            
        if(ckshowbasicmods.checked && ((!ignoreCtrl) || (ignoreCtrl && ignoreCtrl != 'ckShowBasicMODs')))
            postbackarray[postbackarray.length] = 'ckShowBasicMODs';
                                   
        postbackctrl.value = postbackarray.join('*');
    }

    /// PageAddMDField. Reads and stores the value of a metadata field (into JQuery $('form').data object) so that it can be built into XML at some later stage
    ///     
    /// 
    function PageAddMDField(elem,type)
    {
	    if(elem && elem[0])// && elem[0].onchange) 
	    {
	        //elem[0].onchange();	
		    MDHLPRAddMetaDataField(type,elem[0],null);
	    }
    }
     
    /// PageAddMDHiddenField. Reads and stores the value of a hidden metadata field (into JQuery $('form').data object) so that it can be built into XML at some later stage
    ///     
    ///         
    function PageAddMDHiddenField(elem,type,fieldType)
    {            
	    if(elem && elem[0])
	    {	
            var fieldId= elem[0].id.replace('_' + fieldType + '_','_hidden' + '_' + fieldType + '_');    
            var hiddenField = document.getElementById(fieldId); 	    
		    MDHLPRAddMetaDataField(type,hiddenField,null);
	    }        
    }
     
    var _noValidationErrors = true;

    /// ValidateFields. Perfoms actions to facilitate client side form field validation
    ///     
    /// 
    function ValidateFields(field)
    {
        var pnlMODs = document.getElementById('<%#pnlAddMODs.ClientID%>');
        var pnlDC = document.getElementById('<%#pnlAddDC.ClientID%>');

	    _noValidationErrors = true;
        if(pnlDC && pnlDC.style.visibility!="hidden")
        {
		    $('.DCBasic').each(function(){if(_noValidationErrors){$(this).trigger('jqvalidator')}});
	    }

        if(pnlMODs && pnlMODs.style.visibility!="hidden")
        {
		    $('.MODSBasic').each(function(){if(_noValidationErrors){$(this).trigger('jqvalidator')}});
        }
	    return _noValidationErrors;
    }

    /// PageSubmit. Handles a request by the user to submit the page
    ///     
    /// 
    function PageSubmit(sendBtn)
    {       
	    if(!ValidateFields())
		    return false;
		    
        var pnlMODs = document.getElementById('<%#pnlAddMODs.ClientID%>');
        var pnlDC = document.getElementById('<%#pnlAddDC.ClientID%>');
        
        SetPostBackMDCtrls();
        
        var basicMODSXML = document.getElementById('__BASIC_MODS_XML_DATA');
        var basicDCXML = document.getElementById('__BASIC_DC_XML_DATA');
        
        var postBackControl = GetDocElementById('<%#txtPostBackControl.ClientID%>');     
        postBackControl.value = 'btnSubmitPage';
        
        try
        {
            if(pnlDC && pnlDC.style.visibility!="hidden")
            {
                $('.DCBasicHidden').each(function(){PageAddMDHiddenField($(this),'MODS','txt')});    
                $('.DCBasic').each(function(){PageAddMDField($(this),'DC')}); 
                $('.DCAJAXEditorBasic').each(function(){PageAddMDField($(this),'DC')});                        
                $("*.DCAJAXBasic input[type!='hidden']").each(function(){PageAddMDField($(this),'DC')});         
                DCBasicInsertXML(basicDCXML);       
            }
            
            if(pnlMODs)
            {
                $(sendBtn).css('disabled','true');
                if(pnlMODs.style.visibility!="hidden")
                {
                    if(!basicMODSXML)
                    {
                        ShowBusy();
                        eval(<%#postBackCtrl == null ? "no__Ctrl" : postBackCtrl.ClientID.Replace("_","$")%>.strPostbackCall);  // got this by examining generated page;                                       
                        // alternative from core.js could do PostbackBody_Submit(boolForceFullPostback); 
                    }
                    else
                    {
                        //$("*.MODSAJAXBasic input[type!='hidden']").each(function(){$(this).blur()});   // gather any of the combo box data from our custom User Controls
                        $('.MODSBasicHidden').each(function(){PageAddMDHiddenField($(this),'MODS','txt')});                   
                        $('.MODSBasic').each(function(){PageAddMDField($(this),'MODS')}); // gather standard MD fields
                        $('.MODSAJAXEditorBasic').each(function(){PageAddMDField($(this),'MODS')});                        
                        $("*.MODSAJAXBasic input[type!='hidden']").each(function(){PageAddMDField($(this),'MODS')}); // gather standard MD fields
                    
                        MODSBasicInsertXML(basicMODSXML);               
                        AsyncPostback();       

                    }
                }
                else if(pnlDC && pnlDC.style.visibility!="hidden")
                    AsyncPostback();
		    else
		        SubmitPage();
            }
            else if(pnlDC && pnlDC.style.visibility!="hidden")
                AsyncPostback();
	    else
		    SubmitPage();
        }
        catch (e)
        {
            window.alert(e.message);
        }
    }

    /// AsyncPostback. Performs an ASP.NET ajax framework postback (Client script postback) containing all form values (including built up metadata XML in the hidden field for each form user control)
    ///     
    /// 
    function AsyncPostback()
    {
        ShowBusy()
        CallServer($('form').serialize());
    }

    /// SubmitPage. Performs a full page postback
    ///     
    ///
    function SubmitPage()
    {
        var theform = document.forms[0];
        if(theform)
        {
            ShowBusy()
            theform.submit();   
        }
    }
    
    /// ReceiveServerData. Performs any server side javascript returned following an async (client script) postback..e.g code to highlight errored fields
    ///     
    ///    
    function ReceiveServerData(arg, context)
    {
        HideBusy();
        if(arg != '')
            eval(arg);
    }
 
    /// GetElementsByAttribute. Given an element attribute name and tagname returns an array of DOM elements that contain that attribute
    ///     
    ///               
    function GetElementsByAttribute(oElm, strTagName, strAttributeName, strAttributeValue){
        var arrElements = (strTagName == "*" && oElm.all)? oElm.all : oElm.getElementsByTagName(strTagName);
        var arrReturnElements = new Array();
        var oAttributeValue = (typeof strAttributeValue != "undefined")? new RegExp("(^|\\s)" + strAttributeValue + "(\\s|$)", "i") : null;
        var oCurrent;
        var oAttribute;
        for(var i=0; i<arrElements.length; i++){
            oCurrent = arrElements[i];
            oAttribute = oCurrent.getAttribute && oCurrent.getAttribute(strAttributeName);
            if(typeof oAttribute == "string" && oAttribute.length > 0){
                if(typeof strAttributeValue == "undefined" || (oAttributeValue && oAttributeValue.test(oAttribute))){
                    arrReturnElements.push(oCurrent);
                }
            }
        }
        return arrReturnElements;
    }
              
    /// ShowBusy. Displays the busy animation
    ///     
    ///                     
    function ShowBusy() 
    {
        GetDocElementById('progressIndicator').style.display=""; 
        setTimeout('document.images["progressImage"].src="/_ControlTemplates/ClifUserControls/images/ajax-loader.gif"', 200); 
    } 
     
    /// HideBusy. Perfoms actions to facilitate client side form field validation
    ///     
    ///     
    function HideBusy()
    {
        GetDocElementById('progressIndicator').style.display="none";    
    }
     
    /// ValidatorSurnameForename. validates the surname, forename field
    ///     
    ///      
    function ValidatorSurnameForename(field)
    {
	    if(field.value != "")
	    {
		    var regExp = new RegExp("^(([A-z]+)|([A-z]+'[A-z]+)|([A-z]+\-[A-z]+))(\-?([A-z]+'[A-z]+)|([A-z]+)|([A-z]+\-[A-z]+))*(([ |\-][A-z]+'[A-z]+)|( [A-z]+)|( [A-z]+\-[A-z]+))*, (([A-z]+)|([A-z]+'[A-z]+)|([A-z]+\-[A-z]+))(([A-z]+'[A-z]+)|([A-z]+)|([A-z]+\-[A-z]+))*(( [A-z]+'[A-z]+)|( [A-z]+)|( [A-z]+\-[A-z]+))*$",""); // every conceivable foreign name format e.g an arabic and irish mixture  g k o'donnovan-o'reilly e fe o'keefe-o'connell, e'uy h-j iu g-gjh fk-gh o'e 
		    //  intentionally not allowing bracketed names e.g Xiow (clarke)
		    var valid = regExp.test(field.value);
		    if(!valid)
		    {		
			    var attrs = MDHLPRGetFieldAttributes(field,field.id.split('_'));
		        $(field).addClass('errHighlight');
			    alert(attrs.id + ':invalid name format (surname, forename)');
		    }
		    _noValidationErrors = _noValidationErrors && valid;		    
	    }
    }
           
    /// ready. attaches event handlers on page initialization
    ///     
    ///           
    $(document).ready( function() {
        $('.validator_surname_forename').live('jqvalidator',function(){return ValidatorSurnameForename(this);});        
        $(':input[type!=\"hidden\"]').live('mousedown',function(){$(this).removeClass('errHighlight')});  // keypress doesnt work in IE
	    $('.btn-add-cell').live('click',function() {
	        if(!this.disabled) {	
                var btnRemCell = $(this).parent().find('.btn-remove-cell');	
    	        var row = $(this).parent().parent().find('span.RepeatSectionNextLevel');	
		        var cloned = $(this).parent().parent().clone();
		        cloned.find('input').each(function(){this.value=''});
		        cloned.find('textarea').each(function(){this.value=''});		    
		        var clonedRemBtn = cloned.find('.btn-remove-cell');
		        clonedRemBtn[0].id='';
	            clonedRemBtn[0].disabled=false;
		        cloned.appendTo(row);
		        this.disabled=true;
	            btnRemCell[0].disabled=true;
	        }	    
	        return false;  // cancel event just in case 
        });
	    $('.btn-remove-cell').live('click',function() {
	        if(!this.disabled) {
	            var parentOptionsDiv = $(this).parent().parent().parent().parent().children().first();		
       	        var remBtn = parentOptionsDiv.find('.btn-remove-cell');
       	        parentOptionsDiv.find('.btn-add-cell')[0].disabled=false;    	    
    		    if(!(remBtn[0].id.indexOf('RootRemove') > -1)) {
           	        remBtn[0].disabled=false;
    		    }
	            $(this).parent().parent().parent().empty();
	        }
            return false; 
        });        
    });
</script>

</asp:Content>

<asp:Content ID="Main" contentplaceholderid="PlaceHolderMain" runat="server">   
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>    
   <!--[if IE]>   
    <div class="IEroot" style="position:relative;height:100%;width:100%;margin:0px;border:0px;padding:0px;">
    <![endif]-->      
    <div class="NonIEroot" style="position:relative;width:100%;margin:0px;border:0px;padding:0px;">  
    <div id="DivTop" class="DivTop" runat="server">
    <span class="CssPageHdr">
    <span class="CssDownSmallRight">   
    <input class="ClsSubmitBtn" id="btnSubmitPage" type="button" value="Copy to repository" onclick="PageSubmit(this)" title="press this key to copy the document to the repository (no further cancellation possible)"/>      
    </span>
    </span>
    <span style="width:99%"><div style="position:relative;width:600px;top:3em;left:30px;font:1.5em verdana">Please add any metadata you require to describe the object in the repository. Not displaying a form will result in that type of metadata NOT being added. The options panel enables you to display more than one metadata capture form (you may need to scroll to the bottom of the page to find the last form).</div>
    <span class="OptionCtrlsSpan">
    <asp:Panel ID="pnlOptionCtrls" ScrollBars="None" 
            BorderStyle="Dashed" BackColor="White" height="37%"
             CssClass="CssOptionCtrls" runat="server">
    Options :<br/><br/>
    <input id="ckShowBasicMODs" type="checkbox" runat="server" onclick="ShowBasicMODs()"  title="Add Basic MODS Metadata (Optional)" checked="checked"/>Include Basic MODS<br/><br/>
    <input id="ckshowIPMODs" type="checkbox" runat="server" onclick="ShowIPMODs()"  title="Add Full MODS Metadata (Optional)"/>Include Full MODS<br/><br/>
    <input id="ckShowDC" type="checkbox" runat="server" onclick="ShowDC()" title="Add DC Metadata (Optional)"/>Include DC
    <span id="progressIndicator" style="display: none;position:relative;top:-30px;left:-135px">
    <img src="/_ControlTemplates/ClifUserControls/images/ajax-loader.gif" id="progressImage" align="middl"/>
    </span>     
    </asp:Panel>
    </span>
    </span>
    </div>
    <br/>
    <div id="DivMiddle" class="DivMiddle" runat="server">                
    <div class="DivMetaDataForm">
    <asp:Panel ID="pnlAddMODs" runat="server" ScrollBars="None" BorderStyle="Outset" BackColor="#CCCCFF" CssClass="CssDownIndentHigh CssFormMargin">
    <h4>MODS Metadata</h4><asp:PlaceHolder ID="PlaceHolderMODs" runat="server"></asp:PlaceHolder>
    </asp:Panel>
    </div>
    </div>
    <div id="DivBottom" class="DivBottom" runat="server">
    <asp:Panel ID="pnlAddDC" runat="server" ScrollBars="None" 
            BackColor="#CCCCFF" BorderStyle="Outset" CssClass="DivMetaDataForm CssFormMargin">
    <asp:PlaceHolder ID="PlaceHolderDC" runat="server"></asp:PlaceHolder>            
    </asp:Panel>
    </div>
    </div>
    <!--[if IE]>
    </div>
    <![endif]-->    
    <input id="txtPostbackCtrlIds" type="hidden" value="ckShowBasicMODs" runat="server" enableviewstate="false"/>  
    <input id="txtPostBackControl" type="hidden" value="" runat="server" enableviewstate="false"/>
</asp:Content>