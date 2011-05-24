using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebPartPages;
using System.Diagnostics;
using uk.ac.hull.repository.hydranet.service;
using Microsoft.SharePoint.Workflow;

namespace CLIF.Solutions.Code
{
    /**********************************************************************************
     Title:              ConfigureDocumentLibrary
     Author:             Suresh Thampi
     Project:            CLIF.Solutions
     Date:               04/11/2010
     Copyright:          © Center for e-research(CeRch), King's College London 2010 All Rights Reserved.
     Description:     
                            
     ------------------------------------------------------------------------------
     Revision History
     Date             Ref       Author          Reason          Remarks        

     ***********************************************************************************/
    public class ConfigureDocumentLibrary : SPFeatureReceiver
    {
        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
              

        }
        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            //throw new Exception("The method or operation is not implemented.");
        }

        public override void FeatureInstalled(SPFeatureReceiverProperties properties)
        {
            //throw new Exception("The method or operation is not implemented.");
        }

        public override void FeatureUninstalling(SPFeatureReceiverProperties properties)
        {
            //throw new Exception("The method or operation is not implemented.");
        }
    }
}
