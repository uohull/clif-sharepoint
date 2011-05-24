using System;
using System.Web.Script.Services;
using System.Web.Services;
using uk.ac.hull.repository.hydranet.service;
using CLIF.Solutions.Code.Utilities;
using uk.ac.hull.repository.hydranet.hydracontent;
using System.Collections.Generic;

namespace CLIF.Solutions.Code
{
    [ScriptService]
    [WebService(Namespace = "CLIF.Solutions.Code")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class HydranetService : WebService
    {
        [WebMethod]
        public string AddContentObject(string ObjectName, string ParentID, string PIDFormat, string NamespaceFormat, string LabelFormat)
        {
            string _returnValue = string.Empty;
            string _objectLabel = LabelFormat.Replace("{objectname}", ObjectName);
            try
            {
                HydraServiceFedoraImpl ObjService = new HydraServiceFedoraImpl();
                ObjService.DepositSingletonSet(NamespaceFormat, _objectLabel, ParentID);
                _returnValue = ObjService.ObjectPID;
                return _returnValue;
            }
            catch (Exception ex)
            {
                _returnValue = ex.Message;
            }
            return _returnValue;
        }
        [WebMethod]
        public ContentObjectList GetContentObjectFiles(string PID)
        {
            return RepositoryHelper.GetFilesInCollection(PID);
        }
    }
}