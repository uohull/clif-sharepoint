// TreeView.js
// Javascript Behaviour for the TreeView Control
// Copyright (c) by Matthias Hertel, http://www.mathertel.de
// This work is licensed under a BSD style license. See http://www.mathertel.de/License.aspx
// ----- 
// 03.01.2006 created by Matthias Hertel
// 04.01.2006 FireFox compatible.
function querySt(ji) 
{
    hu = window.location.search.substring(1);
    gy = hu.split("&");
    for (i=0;i<gy.length;i++) 
    {
        ft = gy[i].split("=");
        if (ft[0] == ji) 
        {
            return ft[1];
        }
    }
}
var TreeViewBehaviour = {

  // ----- Properties -----
  service: "",
  title: "",
 
  // ----- Events -----
  onclick: function (evt) 
  {
   
    evt = evt || window.event;
    var src = evt.srcElement;
  
    if(src.className =="an")
    {
      var ret=confirm('Are you sure you want to create new folder under ' + src.attributes["name"].value);
     evt.cancelBubble = true;
     evt.returnValue = false;
      return;
    }
    if ((src.className != "do") && (src.className != "dc") && (src.className != "du"))
    {
      src = src.parentNode;  
       
    }
    if(src.attributes["name"].value!='/')
        {
          if(document.getElementById("btnAddNewFolder") !=null)
          {
           document.getElementById("lblSelectFolder").innerText=src.innerText; 
           document.getElementById("ctl00_PlaceHolderMain_hdnSelectPID").value=src.attributes["name"].value       
           document.getElementById("btnAddNewFolder").disabled=false;
          }
          document.getElementById("lblSelectPID").innerText=src.attributes["name"].value;  
          CallGetObjectFiles();
        }
        else
        {
           if(document.getElementById("btnAddNewFolder") !=null)
           {
             document.getElementById("btnAddNewFolder").disabled=true;
           }
        }
     
  
    if ((src.className == "do") || (src.className == "dc") || (src.className == "du")) 
    {
      var subf = src.nextSibling;
      if ((subf != null) && (subf.nodeName != "DIV"))
        subf = subf.nextSibling

      if ((subf == null) || (subf.nodeName != "DIV")) 
      {
        // this should never happen
      } else if (src.className == "do") 
      {
        src.className = "dc";
        subf.style.display = "none";
      } 
      else if (src.className == "dc") 
      {
        src.className = "do";
        subf.style.display = "block";
      } 
      else if (src.className == "du") 
      {
        if(querySt("rf")!="2")
        {
           src.className = "do";
           subf.innerText = "loading...";
           subf.style.display = "block";
           ajax.Start(jcl.FindBehaviourElement(src, TreeViewBehaviour).ExploreAction, src);
        }
        else
        {
           src.className = "do";
           subf.style.display = "block";
        }
      }
    }
    else
    {
        src.className = "de"         
    }
    
    evt.cancelBubble = true;
    evt.returnValue = false;
    

  }, // onchange


  // ----- Methods -----
  exploreTree: function (src) {
    var a = 0
  },
  
  // ----- AJAX Actions -----

  // Retrieve the sub-nodes of a given folder.
  ExploreAction: {
    delay: 10,
    queueMultiple: true,

    prepare:
      function(src) { 
        var path = "";
        var root = jcl.FindBehaviourElement(src, TreeViewBehaviour);
        while ((src != null) && (src != root)) 
        {
          if (src.nodeType == 3) 
          {
            src = src.previousSibling;
          } 
          else if (src.className == "subframe") 
          {
            src = src.previousSibling;
          } 
          else if (src.className == "do") 
          {
            path = "/" + src.attributes["name"].value + path;
            src = src.parentNode;
          }
        }
        while (path.substr(0,2) == "//")
        {
          path = path.substr(1);
        }
        
        return (path);
      },

    call: "",

    finish:
     function(data, src){
       jcl.FindBehaviourElement(src, TreeViewBehaviour).ExtendTree(src, data);
     },
    
    onException: proxies.alertException
  }, // FetchAction


//  xslt: "<?xml version='1.0' ?><xsl:stylesheet version='1.0' xmlns:xsl='http://www.w3.org/1999/XSL/Transform'>\
//    <xsl:template match='/tree'><xsl:apply-templates /></xsl:template>\
//    <xsl:template match='folder[@title]'><div class='du'><xsl:attribute name='name'><xsl:value-of select='@name' /></xsl:attribute><span class='ft'><xsl:value-of select='@title' /></span></div><div class='subframe' style='display:none'></div></xsl:template>\
//    <xsl:template match='folder'><div class='du'><xsl:attribute name='name'><xsl:value-of select='@name' /></xsl:attribute><span class='ft'><xsl:value-of select='@name' /></span></div><div class='subframe' style='display:none'><xsl:apply-templates /></div></xsl:template>\
//    <xsl:template match='file[@title]'><div class='fl'><span class='ft'><xsl:value-of select='@title' /></span></div></xsl:template>\
//    <xsl:template match='file'><div class='fl'><span class='ft'><xsl:value-of select='@name' /></span></div></xsl:template>\
//    </xsl:stylesheet>",
 
   xslt: "<?xml version='1.0' ?><xsl:stylesheet version='1.0' xmlns:xsl='http://www.w3.org/1999/XSL/Transform'>\
    <xsl:template match='/ArrayOfContentObject'><xsl:apply-templates /></xsl:template>\
    <xsl:template match='ContentObject[@Label]'><div class='du'><xsl:attribute name='name'><xsl:value-of select='@name' /></xsl:attribute><span class='ft'><xsl:value-of select='@Label' /></span></div><div class='subframe' style='display:none'></div></xsl:template>\
    <xsl:template match='ContentObject'><div class='du'><xsl:attribute name='name'><xsl:value-of select='@name' /></xsl:attribute><span class='ft'><xsl:value-of select='@name' /></span></div><div class='subframe' style='display:none'><xsl:apply-templates /></div></xsl:template>\
    <xsl:template match='file[@title]'><div class='fl'><span class='ft'><xsl:value-of select='@title' /></span></div></xsl:template>\
    <xsl:template match='file'><div class='fl'><span class='ft'><xsl:value-of select='@name' /></span></div></xsl:template>\
    </xsl:stylesheet>",
    
  ExtendTree: function (src, data) {
    var subf = src.nextSibling;
    if ((subf != null) && (subf.nodeName != "DIV"))
      subf = subf.nextSibling

    if (typeof(XSLTProcessor) != "undefined") {
      // Mozilla...
      
      if (typeof(this.xslt) == "string") {
        this.xslt = ajax._getXMLDOM(this.xslt);
      } // if

      // Finally import the .xsl
      var xsltProcessor = new XSLTProcessor();
      xsltProcessor.importStylesheet(this.xslt);

      var fragment = xsltProcessor.transformToFragment(data, window.document);
      subf.innerHTML = "";
      subf.appendChild(fragment);

    } else {
      // IE
      if (typeof(this.xslt) == "string") {
        var xsltDoc= new ActiveXObject("Msxml2.DOMDocument");
        xsltDoc.async = false;
        xsltDoc.loadXML(this.xslt);
        this.xslt = xsltDoc;
      } // if
      
      subf.innerHTML = data.transformNode(this.xslt);

    } // if
    
    if (subf.childNodes.length == 0) {
      src.className = "de";
      subf.style.display="none";
    } // if
      
  }, // ExtendTree
  
  init: function () {
    this.ExploreAction.call = this.service;
  } // init

} // TreeViewBehaviour


