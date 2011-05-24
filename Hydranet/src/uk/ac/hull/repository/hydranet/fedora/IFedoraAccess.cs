using System;
using System.Web;
using System.ComponentModel;
using uk.ac.hull.repository.hydranet.serviceref.fedoraaccess;

namespace uk.ac.hull.repository.hydranet.fedora
{
    internal interface IFedoraAccess
    {
    
        FedoraServer fedoraServer
        {
            get;
            set;
        }

        FedoraAPIAClient FedoraAccessProxy { get; set; }
        byte[] getDissemination(string pid, string serviceDefinitionPid, string methodName, string versionDateTime, Property[] parameters);

     }
}
