using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;

namespace CLIF.Solutions.Code
{
    /**********************************************************************************
     Title:              AddRepositoryUsers
     Author:             Suresh Thampi
     Project:            CLIF.Solutions
     Date:               20/01/2011
     Copyright:          © Center for e-research(CeRch), King's College London 2010 All Rights Reserved.
     Description:     
                                
     ------------------------------------------------------------------------------
     Revision History
     Date             Ref       Author          Reason          Remarks        

     ***********************************************************************************/
    public class AddRepositoryUsers : SPFeatureReceiver
    {
        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            string _userName = SPContext.Current.Web.CurrentUser.ToString();
            string _defaultPassword = SPContext.Current.Web.CurrentUser.ToString();
            using (FedoraUserManagement ObjUserManagement = new FedoraUserManagement())
            {
                ObjUserManagement.AddUser(_userName, _defaultPassword);
            }
        }

        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            
        }

        public override void FeatureInstalled(SPFeatureReceiverProperties properties)
        {
//            throw new NotImplementedException();
        }

        public override void FeatureUninstalling(SPFeatureReceiverProperties properties)
        {
//            throw new NotImplementedException();
        }
    }
}
