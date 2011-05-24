using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.PolicyManagement
{
    public static class Dictionary
    {
        public enum Section : int
        {
            SUBJECTS,
            RESOURCES,
            ACTIONS
        }

        public const string LOGINID = "urn:fedora:names:fedora:2.1:subject:loginId";
        public const string SUBJECTREPRESENTED = "urn:fedora:names:fedora:2.1:subject:subjectRepresented";	
        public const string API = "urn:fedora:names:fedora:2.1:action:api";	
        public const string API_M = "urn:fedora:names:fedora:2.1:action:api-m";	
        public const string API_A = "urn:fedora:names:fedora:2.1:action:api-a";	
        public const string ID = "urn:fedora:names:fedora:2.1:action:id";
        public const string ID_ADDDATASTREAM = "urn:fedora:names:fedora:2.1:action:id-addDatastream";	
        public const string ID_EXPORT = "urn:fedora:names:fedora:2.1:action:id-export";	
        public const string ID_GETDATASTREAM = "urn:fedora:names:fedora:2.1:action:id-getDatastream";	
        public const string ID_GETDATASTREAMHISTORY = "urn:fedora:names:fedora:2.1:action:id-getDatastreamHistory";	
        public const string ID_GETDATASTREAMS = "urn:fedora:names:fedora:2.1:action:id-getDatastreams";	
        public const string ID_GETNEXTPID = "urn:fedora:names:fedora:2.1:action:id-getNextPid";	
        public const string ID_GETOBJECTXML = "urn:fedora:names:fedora:2.1:action:id-getObjectXML";	
        public const string ID_INGEST = "urn:fedora:names:fedora:2.1:action:id-ingest";	
        public const string ID_MODIFYDATASTREAMBYREFERENCE = "urn:fedora:names:fedora:2.1:action:id-modifyDatastreamByReference";	
        public const string ID_MODIFYDATASTREAMBYVALUE = "urn:fedora:names:fedora:2.1:action:id-modifyDatastreamByValue";	
        public const string ID_MODIFYOBJECT = "urn:fedora:names:fedora:2.1:action:id-modifyObject";	
        public const string ID_PURGEOBJECT = "urn:fedora:names:fedora:2.1:action:id-purgeObject";	
        public const string ID_PURGEDATASTREAM = "urn:fedora:names:fedora:2.1:action:id-purgeDatastream";	
        public const string ID_SETDATASTREAMDTATE = "urn:fedora:names:fedora:2.1:action:id-setDatastreamState";	
        public const string ID_SETDATASTREAMVERSIONABLE = "urn:fedora:names:fedora:2.1:action:id-setDatastreamVersionable";	
        public const string ID_COMPAREDATASTREAMCHECKSUM = "urn:fedora:names:fedora:2.1:action:id-compareDatastreamChecksum";	
        public const string ID_DESCRIBEREPOSITORY = "urn:fedora:names:fedora:2.1:action:id-describeRepository";	
        public const string ID_FINDOBJECTS = "urn:fedora:names:fedora:2.1:action:id-findObjects";	
        public const string ID_RIFINDOBJECTS = "urn:fedora:names:fedora:2.1:action:id-riFindObjects";	
        public const string ID_GETDATASTREAMDISSEMINATION = "urn:fedora:names:fedora:2.1:action:id-getDatastreamDissemination";	
        public const string ID_GETDISSEMINATION = "urn:fedora:names:fedora:2.1:action:id-getDissemination";	
        public const string ID_GETOBJECTHISTORY = "urn:fedora:names:fedora:2.1:action:id-getObjectHistory";	
        public const string ID_GETOBJECTPROFILE = "urn:fedora:names:fedora:2.1:action:id-getObjectProfile";	
        public const string ID_LISTDATASTREAMS = "urn:fedora:names:fedora:2.1:action:id-listDatastreams";	
        public const string ID_LISTMETHODS = "urn:fedora:names:fedora:2.1:action:id-listMethods";	
        public const string ID_LISTOBJECTINFIELDSEARCHRESULTS = "urn:fedora:names:fedora:2.1:action:id-listObjectInFieldSearchResults";	
        public const string ID_LISTOBJECTINRESOURCEINDEXRESULTS = "urn:fedora:names:fedora:2.1:action:id-listObjectInResourceIndexResults";	
        public const string ID_SERVERSTATUS = "urn:fedora:names:fedora:2.1:action:id-serverStatus";	
        public const string ID_OAI = "urn:fedora:names:fedora:2.1:action:id-oai";	
        public const string ID_UPLOAD = "urn:fedora:names:fedora:2.1:action:id-upload";	
        public const string ID_IDDSSTATE = "urn:fedora:names:fedora:2.1:action:id-dsstate";	
        public const string ID_RESOLVEDATASTREAM = "urn:fedora:names:fedora:2.1:action:id-resolveDatastream";	
        public const string ID_RELOADPOLICIES = "urn:fedora:names:fedora:2.1:action:id-reloadPolicies";	
        public const string ID_GETRELATIONSHIPS = "urn:fedora:names:fedora:2.1:action:id-getRelationships";	
        public const string ID_ADDRELATIONSHIP = "urn:fedora:names:fedora:2.1:action:id-addRelationship";	
        public const string ID_PURGERELATIONSHIP = "urn:fedora:names:fedora:2.1:action:id-purgeRelationship";	
        public const string ID_RETRIEVEFILE = "urn:fedora:names:fedora:2.1:action:id-retrieveFile";	
        public const string TICKETISSEDDATETIME = "urn:fedora:names:fedora:2.1:resource:ticketIssuedDateTime";	
        public const string PID = "urn:fedora:names:fedora:2.1:resource:object:pid";	
        public const string NAMESPACE = "urn:fedora:names:fedora:2.1:resource:object:namespace";	
        public const string STATE = "urn:fedora:names:fedora:2.1:resource:object:state";	
        public const string NEWSTATE = "urn:fedora:names:fedora:2.1:resource:object:newState";	
        public const string CONTROLGROUP = "urn:fedora:names:fedora:2.1:resource:object:controlGroup";	
        public const string OWNER = "urn:fedora:names:fedora:2.1:resource:object:owner";	
        public const string CREATEDDATE = "urn:fedora:names:fedora:2.1:resource:object:createdDate";	
        public const string LASTMODIFIEDDATE = "urn:fedora:names:fedora:2.1:resource:object:lastModifiedDate";	
        public const string OBJECTTYPE = "urn:fedora:names:fedora:2.1:resource:object:objectType";	
        public const string CONTEXT = "urn:fedora:names:fedora:2.1:resource:object:context";	
        public const string ENCODING = "urn:fedora:names:fedora:2.1:resource:object:encoding";	
        public const string FORMATURI = "urn:fedora:names:fedora:2.1:resource:object:formatUri";	
        public const string NPIDS = "urn:fedora:names:fedora:2.1:resource:object:nPids";	
        public const string ASOFDATETIME = "urn:fedora:names:fedora:2.1:resource:datastream:asOfDateTime";	
        public const string CONTENTLENGTH = "urn:fedora:names:fedora:2.1:resource:datastream:contentLength";	
        public const string DS_CONTROLGROUP = "urn:fedora:names:fedora:2.1:resource:datastream:controlGroup";	
        public const string NEWCONTROLGROUP = "urn:fedora:names:fedora:2.1:resource:datastream:newControlGroup";
        public const string DS_CREATEDDATE = "urn:fedora:names:fedora:2.1:resource:datastream:createdDate";	
        public const string DS_FORMATURI = "urn:fedora:names:fedora:2.1:resource:datastream:formatUri";	
        public const string NEWFORMATURI = "urn:fedora:names:fedora:2.1:resource:datastream:newFormatUri";	
        public const string DS_ID = "urn:fedora:names:fedora:2.1:resource:datastream:id";
        public const string INFOTYPE = "urn:fedora:names:fedora:2.1:resource:datastream:infoType";
        public const string LOCATION = "urn:fedora:names:fedora:2.1:resource:datastream:location";	
        public const string NEWLOCATION = "urn:fedora:names:fedora:2.1:resource:datastream:newLocation";	
        public const string FILEURI = "urn:fedora:names:fedora:2.1:resource:datastream:fileUri";	
        public const string LOCATIONTYPE = "urn:fedora:names:fedora:2.1:resource:datastream:locationType";	
        public const string MIMETYPE = "urn:fedora:names:fedora:2.1:resource:datastream:mimeType";	
        public const string NEWMIMETYPE = "urn:fedora:names:fedora:2.1:resource:datastream:newMimeType";	
        public const string DS_STATE = "urn:fedora:names:fedora:2.1:resource:datastream:state";	
        public const string DS_NEWSTATE = "urn:fedora:names:fedora:2.1:resource:datastream:newState";
        public const string NEWVERSIONABLE = "urn:fedora:names:fedora:2.1:resource:datastream:newVersionable";	
        public const string CHECKSUM = "urn:fedora:names:fedora:2.1:resource:datastream:checksum";	
        public const string NEWCHECKSUM = "urn:fedora:names:fedora:2.1:resource:datastream:newChecksum";	
        public const string CHECKSUMTYPE = "urn:fedora:names:fedora:2.1:resource:datastream:checksumType";	
        public const string NEWCHECKSUM_TYPE = "urn:fedora:names:fedora:2.1:resource:datastream:newChecksumType";	
        public const string ALTIDS = "urn:fedora:names:fedora:2.1:resource:datastream:altIds";	
        public const string NEWALTIDS = "urn:fedora:names:fedora:2.1:resource:datastream:newAltIds";	
        public const string DISS_ID = "urn:fedora:names:fedora:2.1:resource:disseminator:id";	
        public const string DISS_STATE = "urn:fedora:names:fedora:2.1:resource:disseminator:state";	
        public const string DISS_METHOD = "urn:fedora:names:fedora:2.1:resource:disseminator:method";	
        public const string DISS_ASOFDATETIME = "urn:fedora:names:fedora:2.1:resource:disseminator:asOfDateTime";	
        public const string DISS_NEWSTATE = "urn:fedora:names:fedora:2.1:resource:disseminator:newState";	
        public const string SDEF_PID = "urn:fedora:names:fedora:2.1:resource:sdef:pid";	
        public const string SDEF_NAMESPACE = "urn:fedora:names:fedora:2.1:resource:sdef:namespace";	
        public const string SDEF_LOCATION = "urn:fedora:names:fedora:2.1:resource:sdef:location";	
        public const string SDEF_STATE = "urn:fedora:names:fedora:2.1:resource:sdef:state";	
        public const string SDEP_PID = "urn:fedora:names:fedora:2.1:resource:sdep:pid";	
        public const string SDEP_NEWPID = "urn:fedora:names:fedora:2.1:resource:sdep:newPid";	
        public const string SDEP_NAMESPACE = "urn:fedora:names:fedora:2.1:resource:sdep:namespace";	
        public const string SDEP_NEWNAMESPACE = "urn:fedora:names:fedora:2.1:resource:sdep:newNamespace";
        public const string SDEP_LOCATION = "urn:fedora:names:fedora:2.1:resource:sdep:location";	
        public const string SDEP_STATE = "urn:fedora:names:fedora:2.1:resource:sdep:state";	
        public const string ENV_CURRENTDATE = "urn:fedora:names:fedora:2.1:environment:currentDate";	
        public const string ENV_CURRENTDATETIME = "urn:fedora:names:fedora:2.1:environment:currentDateTime";	
        public const string ENV_CURRENTTIME = "urn:fedora:names:fedora:2.1:environment:currentTime";	
        public const string AUTHTYPE = "urn:fedora:names:fedora:2.1:environment:httpRequest:authType";	
        public const string CLIENTFQDN = "urn:fedora:names:fedora:2.1:environment:httpRequest:clientFqdn";	
        public const string CLIENTIPADDRESS = "urn:fedora:names:fedora:2.1:environment:httpRequest:clientIpAddress";
        public const string HTTP_REQUEST_CONTENTLENGTH = "urn:fedora:names:fedora:2.1:environment:httpRequest:contentLength";	
        public const string CONTENTTYPE = "urn:fedora:names:fedora:2.1:environment:httpRequest:contentType";	
        public const string MESSAGEPROTOCOL = "urn:fedora:names:fedora:2.1:environment:httpRequest:messageProtocol";	
        public const string MESSAGEPROTOCOL_SOAP = "urn:fedora:names:fedora:2.1:environment:httpRequest:messageProtocol-soap";	
        public const string MESSAGEPROTOCOL_REST = "urn:fedora:names:fedora:2.1:environment:httpRequest:messageProtocol-rest";
        public const string HTTP_REQUEST_METHOD = "urn:fedora:names:fedora:2.1:environment:httpRequest:method";	
        public const string HTTP_REQUEST_PROTOCOL = "urn:fedora:names:fedora:2.1:environment:httpRequest:protocol";	
        public const string HTTP_REQUEST_SCHEME = "urn:fedora:names:fedora:2.1:environment:httpRequest:scheme";	
        public const string HTTP_REQUEST_SECURITY = "urn:fedora:names:fedora:2.1:environment:httpRequest:security";	
        public const string HTTP_REQUEST_SECURITY_SECURE = "urn:fedora:names:fedora:2.1:environment:httpRequest:security-secure";	
        public const string HTTP_REQUEST_SECURITY_INSECURE = "urn:fedora:names:fedora:2.1:environment:httpRequest:security-insecure";	
        public const string HTTP_REQUEST_SERVERFQN = "urn:fedora:names:fedora:2.1:environment:httpRequest:serverFqdn";	
        public const string HTTP_REQUEST_SERVERIPADDRESS = "urn:fedora:names:fedora:2.1:environment:httpRequest:serverIpAddress";	
        public const string HTTP_REQUEST_SERVERPORT = "urn:fedora:names:fedora:2.1:environment:httpRequest:serverPort";	
        public const string HTTP_REQUEST_SESSIONENCODING = "urn:fedora:names:fedora:2.1:environment:httpRequest:sessionEncoding";	
        public const string HTTP_REQUEST_SESSIONSTATUS = "urn:fedora:names:fedora:2.1:environment:httpRequest:sessionStatus";	

        public const string STRING_TYPE = "http://www.w3.org/2001/XMLSchema#string";
        public const string ANYURI_TYPE = "http://www.w3.org/2001/XMLSchema#anyURI";
        public const string INTEGER_TYPE = "http://www.w3.org/2001/XMLSchema#integer";
        public const string DATETIME_TYPE = "http://www.w3.org/2001/XMLSchema#dateTime";
        public const string BOOLEAN_TYPE = "http://www.w3.org/2001/XMLSchema#boolean";
        public const string DATE_TYPE = "http://www.w3.org/2001/XMLSchema#date";
        public const string TIME_TYPE = "http://www.w3.org/2001/XMLSchema#time";

        public const string STRING_EQ = "urn:oasis:names:tc:xacml:1.0:function:string-equal";

        public const string ALGO_FIRST_APPLICABLE = "urn:oasis:names:tc:xacml:1.0:rule-combining-algorithm:first-applicable";
        public const string NOT = "urn:oasis:names:tc:xacml:1.0:function:not";
        public const string STRING_AT_LEAST = "urn:oasis:names:tc:xacml:1.0:function:string-at-least-one-member-of";
        public const string STRING_BAG = "urn:oasis:names:tc:xacml:1.0:function:string-bag";

    }
}
