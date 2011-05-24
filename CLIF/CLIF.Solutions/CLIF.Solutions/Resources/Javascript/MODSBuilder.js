// A library thats builds up some Basic MODS XML
// Author Andy Thompson  2010  version 0.1

/// MODSBuildXML. builds an Xml fragment for MODS metadata from form data values held in the JQuery form.data object
///     
///
function MODSBuildXML()
{  
    var xml = '<modsCollection xmlns="http://www.loc.gov/mods/v3"><mods version="3.3">';
    var formData = $('form');
    
    if(formData.data('MODStitle'))
    {
        xml += '<titleInfo><title>' + formData.data('MODStitle')[0] + '</title></titleInfo>';
    }
        
    if(formData.data('MODSnamePart') || formData.data('MODSroleType'))
    {    
        $.each(formData.data('MODSnamePart'),function(index,value){xml += '<name type="personal"><namePart>' + formData.data('MODSnamePart')[index] + '</namePart><role><roleTerm type="text">' + formData.data('MODSroleType')[index] + '</roleTerm></role></name>'});    
    }
    
    if(formData.data('MODStypeOfResource'))
    {
        xml += '<typeOfResource>' + formData.data('MODStypeOfResource')[0] + '</typeOfResource>';    
    }
        
    if(formData.data('MODSgenre'))
    {
        xml += '<genre>' + formData.data('MODSgenre')[0] + '</genre>';    
    }
    else if(formData.data('MODStypeOfResource'))   // A Hull specific reauirement (for driving Blacklight facets)
    {
        xml += '<genre>' + formData.data('MODStypeOfResource')[0] + '</genre>';    
    }
    
    if(formData.data('MODSpublisher') || formData.data('MODSdateIssued'))
    {
        xml += '<originInfo>';
        if (formData.data('MODSpublisher'))
        {
            xml += '<publisher>' + formData.data('MODSpublisher')[0] + '</publisher>'; 
        }        
        if (formData.data('MODSdateIssued'))  
        {
            xml += '<dateIssued encoding="w3cdtf" keyDate="yes">' + formData.data('MODSdateIssued')[0] + '</dateIssued>';        
        }
        xml += '</originInfo>';
    }    
     
    if(formData.data('MODSlanguageTermType') || formData.data('MODSlanguageTermAuthority'))
    {
        xml += '<language>';
        
        if(formData.data('MODSlanguageTermType'))
        {        
             xml += '<languageTerm type="text">' + formData.data('MODSlanguageTermType')[0] + '</languageTerm>';             
        }
      
        if(formData.data('MODSlanguageTermAuthority'))
        {        
             xml += '<languageTerm authority="iso639-2b" type="code">' + formData.data('MODSlanguageTermAuthority')[0] + '</languageTerm>';
        }        
        
        xml += '</language>';        
    }
    
    if(formData.data('MODSextent') || formData.data('MODSinternetMediaType') || formData.data('MODSdigitalOrigin')) 
    {
        xml += '<physicalDescription>'
        if(formData.data('MODSextent'))
        {
            xml += '<extent>' + formData.data('MODSextent')[0] + '</extent>';
        }
        if(formData.data('MODSinternetMediaType'))
        {
            xml += '<internetMediaType>' + formData.data('MODSinternetMediaType')[0] + '</internetMediaType>';
        }
        if(formData.data('MODSdigitalOrigin'))
        {
            xml += '<digitalOrigin>' + formData.data('MODSdigitalOrigin')[0] + '</digitalOrigin>';
        }            
        
        xml += '</physicalDescription>'    
    }
    
    if(formData.data('MODSabstract')) 
    {
        xml += '<abstract>' + formData.data('MODSabstract')[0] + '</abstract>';        
    }    
    
    if(formData.data('MODStopic')) 
    {
        if(formData.data('MODSsubjectAuthority'))
        {
            xml += '<subject authority="' + formData.data('MODSsubjectAuthority')[0] + '">'
        }
        else
        {
            xml += '<subject>';
        }
       
        $.each(formData.data('MODStopic'),function(index,value){xml += '<topic>' + formData.data('MODStopic')[index] + '</topic>'});      
        xml += '</subject>';
    }
      
    if(formData.data('MODSidentifier'))
    {
        xml += '<identifier type="fedora">' + formData.data('MODSidentifier')[0] + '</identifier>';
    }  
            
    if(formData.data('MODSurl'))
    {
        xml += '<location>';                
        $.each(formData.data('MODSurl'),function(index,value){var firstUrl = '<url access="object in context" usage="primary display">';var otherUrls = '<url access="raw object">'; xml += (( index == 0 ? firstUrl : otherUrls ) + formData.data('MODSurl')[index] + '</url>')});                   
        xml += '</location>';        
    }
    
    if(formData.data('MODSaccessCondition'))
    {
        $.each(formData.data('MODSaccessCondition'),function(index,value){xml += '<accessCondition>' + formData.data('MODSaccessCondition')[index] + '</accessCondition>'});    
    }  
    
    if(formData.data('MODSnote')) 
    {
        $.each(formData.data('MODSnote'),function(index,value){xml += '<note>' + formData.data('MODSnote')[index] + '</note>'});      
    }     
               
    xml += '</mods></modsCollection>';
    return xml;
}

/// MODSBasicInsertXML. builds the MODS xml for the form and saves it in the supplied field
///     
///
function MODSBasicInsertXML(hiddenFormField)  
{
    hiddenFormField.value = MODSBuildXML();
}
