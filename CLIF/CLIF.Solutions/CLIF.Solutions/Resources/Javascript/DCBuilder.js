// A library thats builds up some Basic Dublin Core XML
// Author Andy Thompson  2010  version 0.1

/// DCBuildXML. builds an Xml fragment for Dublin Core metadata from form data values held in the JQuery form.data object
///     
///
function DCBuildXML()
{  
    xml = "<oai_dc:dc xmlns:dc=\"http://purl.org/dc/elements/1.1/\" xmlns:oai_dc=\"http://www.openarchives.org/OAI/2.0/oai_dc/\" " +
          "xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:schemaLocation=\"http://www.openarchives.org/OAI/2.0/oai_dc/ http://www.openarchives.org/OAI/2.0/oai_dc.xsd\">";
  
    
    var formData = $('form');
    
    if(formData.data('DCtitle'))
    {
        xml += '<dc:title>' + formData.data('DCtitle')[0] + '</dc:title>';
    }
     
    if(formData.data('DCidentifier'))
    {
        xml += '<dc:identifier>' + formData.data('DCidentifier')[0] + '</dc:identifier>';
    }     
    
    if(formData.data('DCcoverage'))
    {
        xml += '<dc:coverage>' + formData.data('DCcoverage')[0] + '</dc:coverage>';
    } 
    
    if(formData.data('DCcreator'))
    {
        xml += '<dc:creator>' + formData.data('DCcreator')[0] + '</dc:creator>';
    } 
    
    if(formData.data('DCcontributor'))
    {
        xml += '<dc:contributor>' + formData.data('DCcontributor')[0] + '</dc:contributor>';
    } 
        
    if(formData.data('DCsubject'))
    {
        xml += '<dc:subject>' + formData.data('DCsubject')[0] + '</dc:subject>';
    } 
            
    if(formData.data('DCdate'))
    {
        xml += '<dc:date>' + formData.data('DCdate')[0] + '</dc:date>';
    }                 
      
    if(formData.data('DCformat'))
    {
        xml += '<dc:format>' + formData.data('DCformat')[0] + '</dc:format>';
    } 

    if(formData.data('DCsource'))
    {
        xml += '<dc:source>' + formData.data('DCsource')[0] + '</dc:source>';
    }

    if(formData.data('DClanguage'))
    {
        xml += '<dc:language>' + formData.data('DClanguage')[0] + '</dc:language>';
    }
    
    if(formData.data('DCtype'))
    {
        xml += '<dc:type>' + formData.data('DCtype')[0] + '</dc:type>';
    }
        
    if(formData.data('DCrelation'))
    {
        $.each(formData.data('DCrelation'),function(index,value){xml += '<dc:relation>' + formData.data('DCrelation')[index] + '</dc:relation>'});        
    }

    if(formData.data('DCrights'))
    {
        $.each(formData.data('DCrights'),function(index,value){xml += '<dc:rights>' + formData.data('DCrights')[index] + '</dc:rights>'});        
    }

    if(formData.data('DCdescription'))
    {
        xml += '<dc:description>' + formData.data('DCdescription')[0] + '</dc:description>';
    }
                                   
    xml += '</oai_dc:dc>';
    return xml;
}

/// DCBasicInsertXML. builds the Dublin Core xml for the form and saves it in the supplied field
///     
///
function DCBasicInsertXML(hiddenFormField)  
{
    hiddenFormField.value = DCBuildXML();
}
