using System;
using System.Web;
using System.ComponentModel;
using uk.ac.hull.repository.hydranet.serviceref.fedoraaccess;
using System.ServiceModel.Description;
using System.ServiceModel.Channels;
using System.ServiceModel;
using System.Text;
using System.IO;

namespace uk.ac.hull.repository.hydranet.fedora {

    class FedoraAccessSOAPImpl : uk.ac.hull.repository.hydranet.fedora.IFedoraAccess
    {
        #region IFedoraAccess Members

        private FedoraAPIAClient fedoraAccessProxy;

        private void AddBasicAuthHeader()
        {
            var httpRequestProperty = new HttpRequestMessageProperty();
            httpRequestProperty.Headers[System.Net.HttpRequestHeader.Authorization] = "Basic " +
            Convert.ToBase64String(Encoding.ASCII.GetBytes(fedoraAccessProxy.ClientCredentials.UserName.UserName + ":" +
            fedoraAccessProxy.ClientCredentials.UserName.Password));
            OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] =
               httpRequestProperty;
        }

        public FedoraAPIAClient FedoraAccessProxy
        {
            get { return fedoraAccessProxy; }
            set { fedoraAccessProxy = value; }
        }


        public FedoraAccessSOAPImpl (FedoraServer fedoraServer) {

                    
            if ((fedoraServer.ServerAddress.Length <= 0) || (fedoraServer.ServerAddress.Length <= 0)) {
               Console.Write("Server not set...."); 
            }
            else 
            {
                string fedoraAccessServiceURL = "http://" + fedoraServer.ServerAddress + ":" + fedoraServer.ServerPort + "/fedora/services/access";
                fedoraAccessProxy = new FedoraAPIAClient("access", fedoraAccessServiceURL); 
                
                var defaultCredentials = fedoraAccessProxy.Endpoint.Behaviors.Find<ClientCredentials>();
                var loginCredentials = new ClientCredentials();
                loginCredentials.UserName.UserName = fedoraServer.AdminUsername;
                loginCredentials.UserName.Password = fedoraServer.AdminPassword;

                fedoraAccessProxy.Endpoint.Behaviors.Remove(defaultCredentials);
                fedoraAccessProxy.Endpoint.Behaviors.Add(loginCredentials);

            }
         
        }

        public FedoraServer fedoraServer
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
        /*
        public RepositoryInfo describeRepository()
        {
           //RepositoryInfo repoInfo = fedoraAccessProxy.describeRepository();
          // return repoInfo;
        }
        */
        public void getObjectProfile()
        {
            throw new NotImplementedException();
        }

        public void listMethods()
        {
            throw new NotImplementedException();
        }

        public void listDatastreams()
        {
            throw new NotImplementedException();
        }

        public void getDatastreamDissemination()
        {
            throw new NotImplementedException();
        }

        public byte[] getDissemination(string pid, string serviceDefinitionPid, string methodName, string versionDateTime, Property[] parameters)
        {
            AddBasicAuthHeader();
            string version = null;

            if (versionDateTime != String.Empty)
                version = versionDateTime;

            MIMETypedStream disseminationStream = fedoraAccessProxy.getDissemination(pid, serviceDefinitionPid, methodName, parameters, version);
            return disseminationStream.stream;
            //return Convert.FromBase64String(sr.ReadToEnd());

        }

        public void findObjects()
        {
            throw new NotImplementedException();
        }

        public void resumeFindObjects()
        {
            throw new NotImplementedException();
        }

        public void getObjectHistory()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
