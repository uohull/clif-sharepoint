// A library thats aids building of metadata
// Author Andy Thompson  2010  version 0.1

/// MDHLPRGetFieldAttributes. Etracts properties of the field from the field name, returning a JSON object that encapsulates them
///     
///
function MDHLPRGetFieldAttributes(ctrl,idArray)
{
    var i = 0;

    for(i=idArray.length -1;i > -1;i--)
    {
       switch(idArray[i].toLowerCase()) 
       {
            case "ckbc": case "ckbe":
            {            
                    return {"ctrl": ctrl, "repeated": false, "type": idArray[i].toLowerCase(), "id": idArray[i + 1], "checked": ctrl.checked };
            }
            case "cmb": case "ddl":
            {
                if(i > 0 && (idArray[i-1].toLowerCase() == "rptd"))
                {
                    return {"ctrl": ctrl, "repeated": true, "type": idArray[i].toLowerCase(), "id": idArray[i + 1], "fullid": idArray[i-1] + '_' + idArray[i] + '_' + idArray[i+1] + '_' + ( (i+2) >= idArray.length ? '' : idArray[i+2])};                        
                }
                else    
                    return {"ctrl": ctrl, "repeated": false, "type": idArray[i].toLowerCase(), "id": idArray[i + 1],"fullid": idArray[i] + '_' + idArray[i+1] + '_' + ( (i+2) >= idArray.length ? '' : idArray[i+2])};                       
            }            
            case "txt": case "tar": case "edt":
            {
                if(i > 0 && (idArray[i-1].toLowerCase() == "rptd"))
                    return {"ctrl": ctrl, "repeated": true, "type": idArray[i].toLowerCase(), "id": idArray[i + 1]};            
                else    
                    return {"ctrl": ctrl, "repeated": false, "type": idArray[i].toLowerCase(), "id": idArray[i + 1]};            
            }
       }    
    }
    
    return null;
}

/// MDHLPRGetFieldAttributes. Etracts properties of the field from the field name, returning a JSON object that encapsulates them
///     
///
function MDHLPRAddMetaDataField(mdtype,ctrl,markuptype)
{ 
    var id = '';

    if(ctrl.id.indexOf('__',0) < 0) // ctrl000_txt_text_MODSBasic   etc
    {
        attrs = MDHLPRGetFieldAttributes(ctrl,ctrl.id.split('_')); 
        if(attrs)
        { 
            if(attrs.type == "edt")
            {
                MDHLPRAppendField(mdtype,attrs,MDHLPREncodeElement($.trim(ctrl.control.get_content())));            
            }
            else if((attrs.type == "cmb")||(attrs.type == "ddl"))
            {
                var ctrlVal =  $.trim(ctrl.value);
                if(ctrlVal != "") 
                {
//                var hiddFieldId = '#' + ctrl.id.replace('_TextBox','_HiddenField');
//                var hiddenField = $(hiddFieldId)[0];
                    var cmbVal = ctrlVal;
                    if(attrs.type == "cmb")
                        selectedVal = '_!cmb_' + attrs.fullid + '=' + $.trim(ctrlVal);
                    else
                        selectedVal = '_!ddl_' + attrs.fullid + '=' + $.trim(ctrlVal);

                    MDHLPRAppendField(mdtype,attrs,MDHLPREncodeElement(selectedVal));
                 }
            }
            else            
                MDHLPRAppendField(mdtype,attrs,MDHLPREncodeElement($.trim(ctrl.value)));        
        }
    }

}


/// MDHLPREncodeAttribute. encodes a string for use as an xml attribute
///     
///
function MDHLPREncodeAttribute(txt)
{
    return txt.replace(/&/g,'&amp;').replace(/</g,'&lt;').replace(/>/g,'&gt;').replace(/"/g,'&quot;');
}

/// MDHLPREncodeElement. encodes a string for use as an xml element
///     
///
function MDHLPREncodeElement(txt)
{
    return txt.replace(/&/g,'&amp;').replace(/</g,'&lt;').replace(/>/g,'&gt;').replace(/"/g,'&quot;').replace(/'/g,'&apos;');
}

/// MDHLPREncodeElement. adds a field and its value to the $('form').data collection...repeated fields are stored as an array value type
///     
///
function MDHLPRAppendField(mdtype,attrs,value)
{
    if(value && typeof(value)!='undefined' && value!='')
    {
        var arrVals = new Array();
        var currAttrs = $('form').data(mdtype + attrs.id);
        if((attrs.repeated || (attrs.type == "ckbc")) && currAttrs)
            arrVals = currAttrs;   // ckbc = compound checkbox
                        
        if((attrs.type == "ckbe") || (attrs.type == "ckbc")) { // ckbc = exclusive checkbox
            if(attrs.checked) {        
                arrVals[arrVals.length] = value;
                $('form').data(mdtype + attrs.id,arrVals);
            }
        }        
        else
        {
            arrVals[arrVals.length] = value;
            $('form').data(mdtype + attrs.id,arrVals);
        }
        
        if((mdtype + attrs.id) == 'MODSlanguageTermType')
        {
            arrVals = new Array();
            arrVals[arrVals.length] = attrs.ctrl.value;
            $('form').data('MODSlanguageTermAuthority',arrVals);   // holds true for MODSlanguageTermType = Drop down list only                      
        }
        
    }
}

/// MDHLPREncodeElement. clears out the $('form').data collection ready for a fresh set of data
///     
///
function MDHLPRClearValues()
{
    $('form').removeData();
}