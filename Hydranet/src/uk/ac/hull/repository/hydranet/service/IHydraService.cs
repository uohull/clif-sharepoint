using System;
using System.Web;
using System.ComponentModel;

namespace uk.ac.hull.repository.hydranet.service
{
    public interface IHydraService
    {
        void DepositSimpleContentObject(string nameSpace, string label, string foxml, string contentURL, string mimeType, string ingestOwner, string documentAuthor);

        void DeleteObject(string objectPid, bool isCollection);

        byte[] GetObjectHydra(string objectPid);

        byte[] GetDisseminationOutput(string serviceDefinitionPid, string methodName, string pid);

        void ListObjects();
    }


}
