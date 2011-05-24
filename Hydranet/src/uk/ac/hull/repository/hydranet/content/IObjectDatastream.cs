using System;
using System.Web;
using System.ComponentModel;

namespace uk.ac.hull.repository.hydranet.content
{
    public interface IObjectDatastream
    {

        string DsId
        {
            get;
            set;
        }

        string Label
        {
            get;
            set;
        }

        string MimeType
        {
            get;
            set;
        }

        string FormatURI
        {
            get;
            set;
        }

        string[] AltIds
        {
            get;
            set;
        }

        string Checksum
        {
            get;
            set;
        }

        string State
        {
            get;
            set;
        }

        string ControlGroup
        {
            get;
            set;
        }

        bool Versionable
        {
            get;
            set;
        }

        int Size
        {
            get;
            set;
        }

        IObjectContent Content
        {
            get;
        }

        string getXML();
    }
}
