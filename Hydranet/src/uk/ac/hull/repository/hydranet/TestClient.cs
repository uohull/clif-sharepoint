using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using uk.ac.hull.repository.hydranet.service;

namespace uk.ac.hull.repository.hydranet
{
    class TestClient
    {
        public static void Main(string[] args)
        {

            HydraServiceFedoraImpl hydraService = new HydraServiceFedoraImpl();
            //hydraService.DepositSet("Test set", null );

            //hydraService.DepositSimpleContentObject("Report", "test:2", "C:\\tmp\\Report.rtf", "application/rtf");

            //hydraService.DeleteObject("test:1", true);
        }

    }
}
