using System;
using System.Web;
using System.ComponentModel;
using uk.ac.hull.repository.hydranet.serviceref.fedoramanagement;
using System.ServiceModel.Description;
using System.ServiceModel.Channels;
using System.ServiceModel;
using System.Text;

namespace uk.ac.hull.repository.hydranet.fedora
{
    public class FedoraManagementSOAPImpl : uk.ac.hull.repository.hydranet.fedora.IFedoraManagement
    {

        private FedoraAPIMClient fedoraManagementProxy;

        public FedoraAPIMClient FedoraManagementProxy
        {
            get { return fedoraManagementProxy; }
            set { fedoraManagementProxy = value; }
        }

        public FedoraManagementSOAPImpl (FedoraServer fedoraServer) {

                    
            if ((fedoraServer.ServerAddress.Length <= 0) || (fedoraServer.ServerAddress.Length <= 0)) {
               Console.Write("Server not set...."); 
            }
            else 
            {
                string fedoraManagementServiceURL = "http://" + fedoraServer.ServerAddress + ":" + fedoraServer.ServerPort + "/fedora/services/management";
                fedoraManagementProxy = new FedoraAPIMClient("management", fedoraManagementServiceURL); 
                //fedoraManagementProxy.ClientCredentials.Windows.ClientCredential = new System.Net.NetworkCredential(fedoraServer.AdminUsername, fedoraServer.AdminPassword);
                
                var defaultCredentials = fedoraManagementProxy.Endpoint.Behaviors.Find<ClientCredentials>();
                var loginCredentials = new ClientCredentials();
                loginCredentials.UserName.UserName = fedoraServer.AdminUsername;
                loginCredentials.UserName.Password = fedoraServer.AdminPassword;

                fedoraManagementProxy.Endpoint.Behaviors.Remove(defaultCredentials);
                fedoraManagementProxy.Endpoint.Behaviors.Add(loginCredentials);

            }
         
        }



        public string ingest(byte[] objectXML, string format, string logMessage, OperationContextScope scope)
        {
            AddBasicAuthHeader();

            string pid = fedoraManagementProxy.ingest(objectXML, format, logMessage);
            return pid;
        }

        public string modifyObject(string pid, string state, string label, string ownerId, string logMessage, OperationContextScope scope)
        {
            AddBasicAuthHeader();

            string timestamp = fedoraManagementProxy.modifyObject(pid, state, label, ownerId, logMessage);
            return timestamp;
        }

        public byte[] getObjectXML(string pid, OperationContextScope scope)
        {
            AddBasicAuthHeader();

            byte[] objectXML = fedoraManagementProxy.getObjectXML(pid);
            return objectXML;
        }

        public byte[] export(string pid, string format, string context, OperationContextScope scope)
        {
            AddBasicAuthHeader();

            byte[] objectXML = fedoraManagementProxy.export(pid, format, context);
            return objectXML;
        }

        public string purgeObject(string pid, string logMessage, bool force, OperationContextScope scope)
        {
            AddBasicAuthHeader();

            string timestamp = fedoraManagementProxy.purgeObject(pid, logMessage, force);
            return timestamp;
        }

        public string addDatastream(string pid, string dsID, string[] altIDs, string dsLabel, bool versionable, string MIMEType, string formatURI,
                                                string dsLocation, string controlGroup, string dsState, string checksumType, string checksum, string logMessage, OperationContextScope scope)
        {
            AddBasicAuthHeader();
          
            string datastreamID = fedoraManagementProxy.addDatastream(pid, dsID, altIDs, dsLabel, versionable, MIMEType, formatURI,
                                                    dsLocation, controlGroup, dsState, checksumType, checksum, logMessage);
            return datastreamID;
        }

        public string modifyDatastreamByReference(string pid, string dsID, string[] altIDs, string dsLabel, string MIMEType, string formatURI,
                                                             string dsLocation, string checksumType, string checksum, string logMessage, bool force, OperationContextScope scope)
        {
            AddBasicAuthHeader();

            string timestamp = fedoraManagementProxy.modifyDatastreamByReference(pid, dsID, altIDs, dsLabel, MIMEType, formatURI,                                                  
                                            dsLocation, checksumType, checksum, logMessage, force);
            return timestamp;
        }

        public string modifyDatastreamByValue(string pid, string dsID, string[] altIDs, string dsLabel, string MIMEType, string formatURI, byte[] dsContent,
                                                            string checksumType, string checksum, string logMessage, bool force, OperationContextScope scope)
        {
            AddBasicAuthHeader();

            string timestamp = fedoraManagementProxy.modifyDatastreamByValue(pid, dsID, altIDs, dsLabel, MIMEType, formatURI, dsContent,
                                                            checksumType, checksum, logMessage, force);
            return timestamp;
        }

        public string setDatastreamState(string pid, string dsID, string dsState, string logMessage, OperationContextScope scope)
        {
            AddBasicAuthHeader();

            string timestamp = fedoraManagementProxy.setDatastreamState(pid, dsID, dsState, logMessage);
            return timestamp;
        }

        public string setDatastreamVersionable(string pid, string dsID, bool versionable, string logMessage, OperationContextScope scope)
        {
            AddBasicAuthHeader();

            string timestamp = fedoraManagementProxy.setDatastreamVersionable(pid, dsID, versionable, logMessage);
            return timestamp;
        }

        public string compareDatastreamChecksum(string pid, string dsID, string versionDate, OperationContextScope scope)
        {
            AddBasicAuthHeader();

            string checksum = fedoraManagementProxy.compareDatastreamChecksum(pid, dsID, versionDate);
            return checksum;
        }

        public Datastream getDatastream(string pid, string dsID, string asOfDateTime, OperationContextScope scope)
        {
            AddBasicAuthHeader();

            Datastream ds = fedoraManagementProxy.getDatastream(pid, dsID, asOfDateTime);
            return ds;
        }

        public Datastream[] getDatastreams(string pid, string asOfDateTime, string dsState, OperationContextScope scope)
        {
            AddBasicAuthHeader();

            Datastream[] ds = fedoraManagementProxy.getDatastreams(pid, asOfDateTime, dsState);
            return ds;
        }

        public Datastream[] getDatastreamHistory(string pid, string dsID, OperationContextScope scope)
        {
            AddBasicAuthHeader();

            Datastream[] ds = fedoraManagementProxy.getDatastreamHistory(pid, dsID);
            return ds;
        }

        public string[] purgeDatastream(string pid, string dsID, string startDT, string endDT, string logMessage, bool force, OperationContextScope scope)
        {
           AddBasicAuthHeader();
           
            string[] timestamp = fedoraManagementProxy.purgeDatastream(pid, dsID, startDT, endDT, logMessage, force);
           return timestamp;
        }

        public string[] getNextPID(string numPIDs, string pidNamespace,OperationContextScope scope)
        {
            AddBasicAuthHeader();

            string[] pid =  fedoraManagementProxy.getNextPID(numPIDs, pidNamespace);

            return pid;

        }

        private void AddBasicAuthHeader()
        {

            var httpRequestProperty = new HttpRequestMessageProperty();
            httpRequestProperty.Headers[System.Net.HttpRequestHeader.Authorization] = "Basic " +
            Convert.ToBase64String(Encoding.ASCII.GetBytes(fedoraManagementProxy.ClientCredentials.UserName.UserName + ":" +
            fedoraManagementProxy.ClientCredentials.UserName.Password));
            OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] =
               httpRequestProperty;
        }
         
        public RelationshipTuple[] getRelationships(string pid, string relationship)
        {
            RelationshipTuple[] relationshipTuple = fedoraManagementProxy.getRelationships(pid, relationship);
            return relationshipTuple;
        }

        public bool addRelationship(string pid, string relationship, string @object, bool isLiteral, string datatype)
        {
           bool relationshipAdded = fedoraManagementProxy.addRelationship(pid, relationship, @object, isLiteral, datatype); 
           return relationshipAdded;
        }

        public bool purgeRelationship(string pid, string relationship, string @object, bool isLiteral, string datatype)
        {
          bool relationshipPurged = fedoraManagementProxy.purgeRelationship(pid, relationship, @object, isLiteral, datatype);
          return relationshipPurged;
        }




        #region IFedoraManagement Members

        public FedoraServer fedoraServer
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                fedoraServer = value;
            }
        }

        #endregion
    }
}
