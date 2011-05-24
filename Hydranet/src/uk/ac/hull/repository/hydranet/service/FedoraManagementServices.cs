using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;



namespace uk.ac.hull.repository.hydranet.service
{
    public class FedoraManagementServices
    {

     
        public FedoraManagementServices() {
             

        }
        
        public String getNextPID()
        {
            /*
            uk.ac.hull.adir.hydranet.webreference.FedoraAPIMService proxy = new Hydranet.uk.ac.hull.adir.hydranet.webreference.FedoraAPIMService();
            proxy.Credentials = new System.Net.NetworkCredential("fedoraAdmin", "pud6dini");
            proxy.PreAuthenticate = true;
 
           // FedoraManagementService.FedoraAPIM fedoraManagement = new FedoraManagementService.FedoraAPIMClient()
           string[] arrayOfPIDs =  proxy.getNextPID("1", "Simons-namespace");

           int pidLength = arrayOfPIDs.Length;

           if (pidLength > 0)
           {
               return arrayOfPIDs[0];
           }
           else
           {
               return "no pid reservered";
           }
             *
             * /
             */
            return "pid";
        
        }
       

    }
}
