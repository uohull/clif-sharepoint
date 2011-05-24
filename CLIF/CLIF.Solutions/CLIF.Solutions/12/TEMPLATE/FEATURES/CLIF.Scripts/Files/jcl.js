// jcl.js: JavaScript Common behaviours Library
// -----
// Behaviour loading and DataConnections for AJAX Controls
// Copyright (c) by Matthias Hertel, http://www.mathertel.de
// This work is licensed under a BSD style license. See http://www.mathertel.de/License.aspx
// More information on: http://ajaxaspects.blogspot.com/ and http://ajaxaspekte.blogspot.com/
// -----
// 12.08.2005 created
// 31.08.2005 jcl object used instead of global methods and objects
// 04.09.2005 GetPropValue added.
// 15.09.2005 CloneObject added.
// 27.09.2005 nosubmit attribute without forms bug fixed.
// 29.12.2005 FindBehaviourElement added.
// 02.04.2005 term method added to release bound html objects.
// 07.05.2006 controlsPath added.
// 09.05.2006 Raise only changed values.
// 09.05.2006 Load() and RaiseAll() added to support default-values on page start.
// 03.06.2006 binding to the document enabled for FF.
// 13.08.2006 classname functions added.
// 16.09.2006 bind() function added to the Function.prototype.
//            The "on___" functions that are automatically attached to events
//            are now always executed in the context of the bound object.
// 24.11.2006 DataConnections without a name are ignored now.
// 09.01.2007 allow Array.prototype.extensions (prototype.js)
// 06.06.2007 including an Open Ajax hub specification compatible implementation

/// obsolete:
var isIE = (window.navigator.userAgent.indexOf("MSIE") > 0);

// -- OpenAjax hub --

if (typeof(window.OpenAjax) == "undefined") {
  // setup the OpenAjax framework - hub
  window.OpenAjax = {};
} // if

if (typeof(OpenAjax.hub) == "undefined") {
  // a hub implementation
  OpenAjax.hub = {
  implementer: "http://www.mathertel.de/OpenAjax",
  implVersion: "0.2",
  specVersion: "0.5",
  implExtraData: {},

  // ----- library management -----

  // the list of libraries that have registered
  libraries: {},

  // Registers an Ajax library with the OpenAjax Hub. 
  registerLibrary: function (p, u, v, e) {
    var entry = { prefix: p, namespaceURI: u, version: v, extraData:e };
    this.libraries[p] = entry;
    this.publish("org.openajax.hub.registerLibrary", entry);
  },

  // Unregisters an Ajax library with the OpenAjax Hub.
  unregisterLibrary: function (p) {
    var entry = this.libraries[p];
    this.publish("org.openajax.hub.unregisterLibrary", entry);
    if (entry != null)
      this.libraries[p] = null;
  },

  // ----- event management -----

  _regs: {},
  _regsId: 0,

  /// name, callback, scope, data, filter
  subscribe: function (n, c, s, d, f) {
    var h = this._regsId;

    s = s || window;

    // treating upper/lowercase equal is not clearly defined, but true with domain names.
    var rn = n.toLocaleLowerCase();

    // build a regexp pattern that will match the event names
    rn = rn.replace(/\*\*$/, "\S{0,}").replace(/\./g, "\\.").replace(/\*/g, "[^.]*");

    var entry = {id:h, n:rn, c:c, s:s, d:d, f:f};
    this._regs[h] = entry;

    this._regsId++;
    return(h);
  }, // subscribe


  unsubscribe: function (h) {
    this._regs[h] = null;
  }, // unsubscribe


  publish: function (n, data) {
    n = n.toLocaleLowerCase();
    for (var h in this._regs) {
      var r = this._regs[h];
      if (r && (n.match(r.n))) {
        var ff = r.f; if (typeof(ff) == "string") ff = r.s[ff];
        var fc = r.c; if (typeof(fc) == "string") fc = r.s[fc];
        if ((ff == null) || (ff.call(r.s, n, data, r.d)))
          fc.call(r.s, n, data, r.d)
      } // if
    } // for
  } // publish

  } // OpenAjax.hub
  OpenAjax.hub.registerLibrary("aoa", "http://www.mathertel.de/OpenAjax", "0.4", {});

} // if (! hub)

OpenAjax.hub.registerLibrary("jcl", "http://www.mathertel.de/Behavior", "1.2", {});

// -- Javascript Control Library (behaviors) --

var jcl = {

isIE: (window.navigator.userAgent.indexOf("MSIE") > 0),

// attach events, methods and default-values to a html object (using the english spelling)

LoadBehaviour: function (obj, behaviour) {
  if ((obj != null) && (obj.constructor == String))
    obj = document.getElementById(obj);

  if (obj == null) {
    alert("LoadBehaviour: obj argument is missing.");
  } else if (behaviour == null) {
    alert("LoadBehaviour: behaviour argument is missing.");

  } else {
    if (behaviour.inheritFrom != null) {
      this.LoadBehaviour(obj, behaviour.inheritFrom);
      this.List.pop();
    }
  
    if ((! jcl.isIE) && (obj.attributes != null)) {
      // copy all attributes to this.properties
      for (var n = 0; n < obj.attributes.length; n++)
        if (obj[obj.attributes[n].name] == null)
          obj[obj.attributes[n].name] = obj.attributes[n].value;
    } // if
    
    for (var p in behaviour) {
      if (p.substr(0, 2) == "on") {
        this.AttachEvent(obj, p, behaviour[p].bind(obj));
        
      } else if ((behaviour[p] == null) || (behaviour[p].constructor != Function)) {
        // set default-value
        if (obj[p] == null)
          obj[p] = behaviour[p];

      } else {
        // attach method
        obj[p] = behaviour[p];
      } // if
    } // for
    
    obj._attachedBehaviour = behaviour;
  } // if
  if (obj != null)
    this.List.push(obj);
}, // LoadBehaviour


/// Find the parent node of a given object that has the behavior attached.
FindBehaviourElement: function (obj, behaviourDef) {
  while ((obj != null) && (obj._attachedBehaviour != behaviourDef))
    obj = obj.parentNode;
  return(obj);
},


/// Find the child elements with a given className contained by obj.
getElementsByClassName: function (obj, cName) {
  var ret = new Array();
  var allNodes = obj.getElementsByTagName("*");
  for (var n = 0; n < allNodes.length; n++) {
    if (allNodes[n].className == cName)
      ret.push(allNodes[n]);
  }
  return(ret);
},


/// Find the child elements with a given name contained by obj.
getElementsByName: function (obj, cName) {
  var ret = new Array();
  var allNodes = obj.getElementsByTagName("*");
  for (var n = 0; n < allNodes.length; n++) {
    if (allNodes[n].name == cName)
      ret.push(allNodes[n]);
  }
  return(ret);
},


// cross browser compatible helper to register for events
AttachEvent: function (obj, eventname, handler) {
  if (isIE) {
    obj.attachEvent(eventname, handler);
  } else { 
    obj.addEventListener(eventname.substr(2), handler, false);
  } // if
}, // AttachEvent


// cross browser compatible helper to register for events
DetachEvent: function (obj, eventname, handler) {
  if (isIE) {
    obj.detachEvent(eventname, handler);
  } else { 
    obj.removeEventListener(eventname.substr(2), handler, false);
  } // if
}, // DetachEvent


/// Create a duplicate of a given JavaScript Object.
/// References are not duplicated !
CloneObject: function (srcObject) {
  var tarObject = new Object();
  for (p in srcObject)
    tarObject[p] = srcObject[p];
  return(tarObject);
}, // CloneObject


// Link all objects with behaviours
List: [],

// ----- Data connections between Controls on the client side. -----

DataConnections: {
  _ns: "de.mathertel.pp.",
  _consumers: { },
  _values: { },

  // remember an object to be a consumer
  RegisterConsumer: function (obj, propName) {
    OpenAjax.hub.subscribe(this._ns + propName.toLowerCase(), "_getValue", this, obj, null);
  },

  // remember an object to be a consumer
  _getValue: function (propName, propData, regData) {
    if (propName.substr(0, this._ns.length) == this._ns);
      regData.GetValue(propName.substr(this._ns.length), propData);
  },

  // Load a property but do not Raise an event.
  Load: function (propName, propValue) {
    propName = propName.toLowerCase();

    // store actual property value
    this._values[propName] = propValue;
  }, // Load
  
  
  // broadcast the change notification of a property
  // set force to true to raise an event even if the value has not changed.
  Raise: function (propName, propValue, force) {
    if ((propName == null) || (propName.length == 0))
      return;
      
    propName = propName.toLowerCase();
    if ((this._values[propName] != propValue) || (force == true)) {
      this._values[propName] = propValue;
      OpenAjax.hub.publish(this._ns + propName, propValue);
    } // if
  }, // Raise
  

  RaiseAll: function () {
    for (prop in this._values) {
      var val = this._values[prop];
      this.Raise(prop, val, true);
    }
  }, // RaiseAll


  // retrieve an actual property value
  GetPropValue: function (propName) {
    propName = propName.toLowerCase();
    return(this._values[propName]);
  }, // GetPropValue


  // persist an actual property value into a local cookie
  PersistPropValue: function (propName) {
    propName = propName.toLowerCase();
    window.document.cookie = "jcl." + propName + "=" + escape(this._values[propName]);
  } // PersistPropValue

}, // DataConnections


// find a relative link to the controls folder containing jcl.js
GetControlsPath: function () {
  var path = "../controls/"
  for (var n in document.scripts) {
    s = String(document.scripts[n].src);
    if ((s != null) && (s.length >= 6) && (s.substr(s.length -6).toLowerCase() == "jcl.js"))
      path = s.substr(0,s.length -6);
  } // for
  return(path);
}, // GetControlsPath


// init all objects when the page is loaded
onload: function(evt) {
  evt = evt || window.event;

  for (var n in jcl.List) {
    var obj = jcl.List[n];
    if ((obj != null) && (obj.init != null))
      obj.init();
  } // for
  
  // raise all persisted values
  var pv = document.cookie.replace(/; /g, ";").split(";");
  for (var n = 0; n < pv.length; n++) {
    if (pv[n].substr(0, 4) == "jcl.") {
      var p = pv[n].substr(4).split("=");
      jcl.DataConnections.Raise(p[0], p[1]);
    } // if
  } // for
}, // onload


// init all objects when the page is loaded
onunload: function(evt) {
  evt = evt || window.event;

  for (var n in jcl.List) {
    var obj = jcl.List[n];
    if ((obj != null) && (obj.term != null))
      obj.term();
  } // for
}, // onunload


// allow non-submitting input elements
onkeypress: function(evt) {
  evt = evt || window.event;
  
  if (evt.keyCode == 13) {
    var obj = document.activeElement;

    while ((obj != null) && (obj.nosubmit == null))
      obj = obj.parentNode;

    if ((obj != null) && ((obj.nosubmit == true) || (obj.nosubmit.toLowerCase() == "true"))) {
      // cancle ENTER / RETURN
      evt.cancelBubble = true;
      evt.returnValue = false;
    } // if
  } // if              
}, // onkeypress


init: function () {
  this.AttachEvent(window, "onload", this.onload);
  this.AttachEvent(window, "onunload", this.onunload);
  this.AttachEvent(document, "onkeypress", this.onkeypress);
}

} // jcl

// document.jcl_isinit (is not declared!) will be set to true to detect multiple jcl includes.
if (document.jcl_isinit != null)
  alert("multiple jcl includes detected !");
document.jcl_isinit = true;

jcl.init();

// ----- make FF more IE compatible -----
if (! jcl.isIE) {

  // ----- HTML objects -----

  HTMLElement.prototype.__defineGetter__("innerText", function () { return(this.textContent); });
  HTMLElement.prototype.__defineSetter__("innerText", function (txt) { this.textContent = txt; });

  HTMLElement.prototype.__defineGetter__("XMLDocument", function () { 
    return ((new DOMParser()).parseFromString(this.innerHTML, "text/xml"));
  });


  // ----- Event objects -----

  // enable using evt.srcElement in Mozilla/Firefox
  Event.prototype.__defineGetter__("srcElement", function () {
    var node = this.target;
    while (node.nodeType != 1) node = node.parentNode;
    // test this:
    if (node != this.target) alert("Unexpected event.target!")
    return node;
  });

  // enable using evt.cancelBubble=true in Mozilla/Firefox
  Event.prototype.__defineSetter__("cancelBubble", function (b) {
    if (b) this.stopPropagation();
  });

  // enable using evt.returnValue=false in Mozilla/Firefox
  Event.prototype.__defineSetter__("returnValue", function (b) {
    if (!b) this.preventDefault();
  });


  // ----- XML objects -----
  
  // select the first node that matches the XPath expression
  // xPath: the XPath expression to use
  XMLDocument.prototype.selectSingleNode = function(xPath) {
    var doc = this;
    if (doc.nodeType != 9)
      doc = doc.ownerDocument;
    if (doc.nsResolver == null) doc.nsResolver = function(prefix) { return(null); };
    var node = doc.evaluate(xPath, this, doc.nsResolver, XPathResult.ANY_UNORDERED_NODE_TYPE, null);
    if (node != null) node = node.singleNodeValue;
    return(node);
  }; // selectSingleNode


  // select the first node that matches the XPath expression
  // xPath: the XPath expression to use
  Node.prototype.selectSingleNode = function(xPath) {
    var doc = this;
    if (doc.nodeType != 9)
      doc = doc.ownerDocument;
    if (doc.nsResolver == null) doc.nsResolver = function(prefix) { return(null); };
    var node = doc.evaluate(xPath, this, doc.nsResolver, XPathResult.ANY_UNORDERED_NODE_TYPE, null);
    if (node != null) node = node.singleNodeValue;
    return(node);
  }; // selectSingleNode


  Node.prototype.__defineGetter__("text", function () {
    return(this.textContent);
  }); // text

}


// ----- Enable an easy attaching methods to events -----
// see http://digital-web.com/articles/scope_in_javascript/

Function.prototype.bind = function(obj) {
  var method = this, temp = function() {
    return method.apply(obj, arguments);
  }
  return(temp);
} // Function.prototype.bind


// ----- Cookies access -----

function _getCookie(aName) {
  var ret = "";
  var docCookie = this.element.document.cookie;
  var index = docCookie.indexOf(aName + "=");
  if (index >= 0)
    ret = docCookie.substring(index+7).split(';')[0];
  return (unescape(ret));
} // _getCookie


function _setCookie(aName, Props) {
  var p;
  try {
    p = String(window.location.href).split('/');
    p = p.slice(3, p.length-1).join('/');
    this.element.document.cookie = aName + "=" + Props + "; path=/" + p + "; expires=" + expire;
  } catch (e) {}
} // _setCookie


// ----- classname modifications -----

function addClassName (elem, className) {
  removeClassName (elem, className);
  if ((className != null) && (className.length > 0))
    elem.className = (elem.className + " " + className);
} // addClassName

function removeClassName (elem, className) {
  var cn = " " + elem.className + " ";
  if ((className != null) && (className.length > 0))
    cn = cn.replace(" " + className + " ", "");
  cn = cn.replace(/^\s+|\s+$/g, "");
  elem.className = cn;
} // removeClassName

// ----- End -----
