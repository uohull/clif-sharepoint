using System;
using System.Web;
using System.ComponentModel;

namespace uk.ac.hull.repository.hydranet.content
{
    public interface IFedoraObject
    {
        string Pid
        {
            get;
            set;
        }

        string Label
        {
            get;
            set;

        }

        string State
        {
            get;
            set;

        }

        string OwnerId
        {
            get;
            set;

        }

        IObjectDatastream[] DatastreamList
        {
            get;
            set;

        }

        string GetXML();

        void AddDatastream(IObjectDatastream datastream);
    }
}
