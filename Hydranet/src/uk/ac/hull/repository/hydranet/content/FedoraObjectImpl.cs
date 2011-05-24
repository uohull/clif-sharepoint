using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using System.Collections;
using System.Text;

namespace uk.ac.hull.repository.hydranet.content
{
    public class FedoraObjectImpl : IFedoraObject
    {

        protected const string W3_TYPE = "FedoraObject";

        private string pid;
        private string label;
        private string state;
        private string ownerId;
        private IObjectDatastream[] datastreamList;
        private Hashtable datastreams;

        #region IFedoraObject Members

        public string Pid
        {
            get
            {
                return pid;
            }

            set
            {
                pid = value;
            }
     
        }

        public string Label
        {
            get
            {
                return label;
            }
            set
            {
                label = value;
            }
        }

        public string State
        {
            get
            {
                return state;
            }
            set
            {
                state = value;
            }
       }

        public string OwnerId
        {
            get
            {
                return ownerId;
            }
            set
            {
                ownerId = value;
            }
          
        }

        public IObjectDatastream[] DatastreamList
        {
            get
            {
                return datastreamList;
            }
            set
            {
                datastreamList = value;
            }
          
        }

        public Hashtable Datastreams
        {
            get
            {
                return datastreams;
            }
            set
            {
                datastreams = value;
            }
        }

        #endregion

        /// <summary>
        /// Constructor for FedoraObjectImpl, this creates a Foxml Object skeleton.
        /// </summary>
        /// <param name="pid">ObjectPID</param>
        /// <param name="label">Object label</param>
        /// <param name="state">Active state</param>
        /// <param name="ownerId">User/owner of object</param>
        public FedoraObjectImpl(string pid, string label, string state, string ownerId)
            : this (pid, label, state, ownerId, null)
        { }
        /// <summary>
        /// Overloaded FedoraObjectImpl constructor, to enable the addition of 
        /// a Hashtable of datastreams
        /// </summary>
        /// <param name="pid">ObjectPID</param>
        /// <param name="label">Object label</param>
        /// <param name="state">Active state</param>
        /// <param name="ownerId">User/owner of object</param>
        /// <param name="datastreams"></param>
        public FedoraObjectImpl(string pid, string label, string state, string ownerId, Hashtable datastreams)
        {
            this.pid = pid;
            this.label = label;
            this.state = state;
            this.ownerId = ownerId;

            //Creates a reference to the datastreams hashtable if its not being passed in...
            if (datastreams == null) 
            {
               this.datastreams = new Hashtable();
            }
            else {
                this.datastreams = datastreams;
            }

        }
            
        /// <summary>
        /// Add a 'IObjectDatastream' type object to the object.
        /// </summary>
        /// <param name="datastream"></param>
        public void AddDatastream(IObjectDatastream datastream)
        {
            datastreams.Add(datastream.DsId, datastream);  //Adds another element to the hashtable with dsID as the key, and datastream object as value
        }

        /// <summary>
        /// getXML() - Returns the full FOXML object xml (including added datastreams)
        /// </summary>
        /// <returns></returns>
        public string GetXML()
        {

            string xml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                    "<foxml:digitalObject PID=\"" + pid + "\" " +
                        "fedoraxsi:schemaLocation=\"info:fedora/fedora-system:def/foxml# http://www.fedora.info/definitions/1/0/foxml1-0.xsd\" " +
                    "xmlns:audit=\"info:fedora/fedora-system:def/audit#\" xmlns:fedoraxsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:foxml=\"info:fedora/fedora-system:def/foxml#\">" +
                    "<foxml:objectProperties>" +
                "<foxml:property NAME=\"http://www.w3.org/1999/02/22-rdf-syntax-ns#type\" VALUE=\"" + W3_TYPE + "\"/>" +
                                "<foxml:property NAME=\"info:fedora/fedora-system:def/model#state\" VALUE=\"" + state + "\"/>" +
                                "<foxml:property NAME=\"info:fedora/fedora-system:def/model#label\" VALUE=\"" + label + "\"/>" +
                                "<foxml:property NAME=\"info:fedora/fedora-system:def/model#ownerId\" VALUE=\"" + ownerId + "\"/></foxml:objectProperties>";

            IDictionaryEnumerator _enumerator = datastreams.GetEnumerator();

            while (_enumerator.MoveNext())
            {
                IObjectDatastream datastream = (IObjectDatastream)_enumerator.Value;
                xml = xml + datastream.getXML();
          
            }

            xml = xml + "</foxml:digitalObject>";

            return xml;

        }


      
    }
}
