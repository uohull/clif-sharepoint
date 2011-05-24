using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;

namespace uk.ac.hull.repository.hydranet.hydracontent.metadata
{
    public interface IMetadata
    {
        string Xml
        {
            get;
            set;
        }
        string FormatURI
        {
            get;
        }

    }
}
