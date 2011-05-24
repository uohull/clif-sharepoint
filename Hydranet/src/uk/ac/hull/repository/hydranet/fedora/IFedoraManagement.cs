using System;
using System.Web;
using System.ComponentModel;
using System.ServiceModel;
using uk.ac.hull.repository.hydranet.serviceref.fedoramanagement;

namespace uk.ac.hull.repository.hydranet.fedora
{
    internal interface IFedoraManagement
    {
        FedoraServer fedoraServer
        {
            get;
            set;
        }

        FedoraAPIMClient FedoraManagementProxy { get; set; }
        string ingest(byte[] objectXML, string format, string logMessage, OperationContextScope scope);

        string modifyObject(string pid, string state, string label, string ownerId, string logMessage, OperationContextScope scope);

        byte[] getObjectXML(string pid, OperationContextScope scope);

        byte[] export(string pid, string format, string context, OperationContextScope scope);

        string purgeObject(string pid, string logMessage, bool force, OperationContextScope scope);

        string addDatastream(string pid, string dsID, string[] altIDs, string dsLabel, bool versionable, string MIMEType, string formatURI,
                                string dsLocation, string controlGroup, string dsState, string checksumType, string checksum, string logMessage, OperationContextScope scope);
        
        string modifyDatastreamByReference(string pid, string dsID, string[] altIDs, string dsLabel, string MIMEType, string formatURI,
                                               string dsLocation, string checksumType, string checksum, string logMessage, bool force, OperationContextScope scope);

    
        string modifyDatastreamByValue(string pid, string dsID, string[] altIDs, string dsLabel, string MIMEType, string formatURI, byte[] dsContent,
                                                          string checksumType, string checksum, string logMessage, bool force, OperationContextScope scope);

        string setDatastreamState(string pid, string dsID, string dsState, string logMessage, OperationContextScope scope);

        string setDatastreamVersionable(string pid, string dsID, bool versionable, string logMessage, OperationContextScope scope);

        string compareDatastreamChecksum(string pid, string dsID, string versionDate, OperationContextScope scope);

        Datastream getDatastream(string pid, string dsID, string asOfDateTime, OperationContextScope scope);

        Datastream[] getDatastreams(string pid, string asOfDateTime, string dsState, OperationContextScope scope);

        Datastream[] getDatastreamHistory(string pid, string dsID, OperationContextScope scope);

        string[] purgeDatastream(string pid, string dsID, string startDT, string endDT, string logMessage, bool force, OperationContextScope scope);

        string[] getNextPID(string numPIDs, string pidNamespace, OperationContextScope scope);

        RelationshipTuple[] getRelationships(string pid, string relationship);
        
        bool addRelationship(string pid, string relationship, string @object, bool isLiteral, string datatype);
      
        bool purgeRelationship(string pid, string relationship, string @object, bool isLiteral, string datatype);
       
    }
}
