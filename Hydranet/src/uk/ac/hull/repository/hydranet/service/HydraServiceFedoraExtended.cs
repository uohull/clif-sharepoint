using System;
using System.Text;
using uk.ac.hull.repository.hydranet.hydracontent;
using uk.ac.hull.repository.hydranet.fedora;
using uk.ac.hull.repository.hydranet.hydracontent.metadata;
using uk.ac.hull.repository.hydranet.serviceref.fedoramanagement;

namespace uk.ac.hull.repository.hydranet.service
{
    /// <summary>
    /// This class is a specialisation of HydraServiceFedoraImpl which allows injection of additional metadata streams,
    /// prior to ingest and more control over existing metadata streams. Methods should be called in the specific order             
    /// [AppendBasicMetaData() | Append...MetaData()], DepositContentObject 
    /// </summary>
    public class HydraServiceFedoraExt : HydraServiceFedoraImpl
    {

        private string _label;
        private string _objectPID;

        //------------------------------------------------------------
        // Added by Suresh Thampi
        private string _subject;
        public string Subject
        {
            get
            {
                return _subject;
            }
            set
            {
                _subject = value;
            }
        }

        private string _source;
        public string Source
        {
            get
            {
                return _source;
            }
            set
            {
                _source = value;
            }
        }

        private string _languageCode;
        public string Language
        {
            get
            {
                return _languageCode;
            }
            set
            {
                _languageCode = value;
            }
        }
        //------------------------------------------------------------

        public Datastream GetData(string PID)
        {
            FedoraManagementSOAPImpl ObjFedora = new FedoraManagementSOAPImpl(new FedoraServer());
            Datastream ds = ObjFedora.getDatastream(PID, "DC", null, null);
            return ds;
        }
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="nameSpace">namespace for the new object</param>
        /// <param name="label">label for the new object</param>
        /// <param name="ingestOwner">name of the object owner</param>
        /// <param name="reservedPID">(optional) if the first generated PID ends with this string forces a second attempt to create a PID for the new object</param>
        public HydraServiceFedoraExt(string nameSpace, string label, string ingestOwner, string reservedPID) : base()
        {
           //Default Language Code - Suresh Thampi
           _languageCode = "en";
           _source = "unknown";
           _subject = "unknown";

            _fedoraManagement = new FedoraManagementSOAPImpl(_fedoraServer);            
            _label = label;

            //--------------------------------------------------//
            // Modified By Suresh Thampi on 04/01/2011
            //--------------------------------------------------//
            if (base.ObjectPID != string.Empty)
            {
                _objectPID = base.ObjectPID;
            }
            else
            {
                //Get the next available PID from fedora.
                _objectPID = GetNextPID(nameSpace);
                //--------------------------------------------------//
                if (!String.IsNullOrEmpty(reservedPID))
                {
                    if (_objectPID.EndsWith(reservedPID))    // this PID is reserved so lets start with the next
                        _objectPID = GetNextPID(nameSpace);
                }
            }
          
            //New instance of contentFactory with objectPID, label etc...    
            _contentFactory = new ContentFactory(_objectPID, _label, "A", ingestOwner);
        }
        
        /// <summary>
        /// Default Constructor
        /// </summary>
        public HydraServiceFedoraExt(): base()
        {
        }
        /// <summary>
        /// Adds some very basic (mandatory) metadata streams all in one go. Use instead the individual AppendXYZMetaData methods for finer control.
        /// </summary>
        /// <param name="parentID">ID of parent object</param>
        /// <param name="mimeType">Mime Type of Object</param>
        public void AppendBasicMetaData(string parentID, string mimeType, string documentAuthor)
        {
            AddNonContentStreams(_label, parentID, mimeType, _objectPID, documentAuthor);
        }

        public void AppendRightsMetaData(byte[] metadata)
        {
            // Creates a rightsMetadata datastream...
            _contentFactory.AddManagedContentDatastream("rightsMetadata", "Rights metadata", "text/xml", 0, metadata);
        }

        public void AppendContentMetaData(byte[] metadata)
        {
            // Creates a contentMetadata datastream...
            _contentFactory.AddManagedContentDatastream("contentMetadata", "Content metadata", "text/xml", 0, metadata);
        }

        /// <summary>
        /// Adds some very basic RDF parent child metadata
        /// </summary>
        /// <param name="parentID"></param>
        public void AddRelsExtMetaData(string parentID)
        {
            //Create the relationship metadata
            RelationshipMetadata relsMetadata = new RelationshipMetadata(_objectPID);
            relsMetadata.AddRelationship(relsMetadata.HAS_MODEL_REL, "hydra-cModel:commonMetadata");
            relsMetadata.AddRelationship(relsMetadata.HAS_MODEL_REL, "hydra-cModel:genericContent");

            relsMetadata.IsCollection = false;

            //Set parentID if its defined... 
            if (parentID != null)
            {
                relsMetadata.AddRelationship(relsMetadata.IS_MEMBER_REL, parentID);
            }

            //Add relationship metadata to the Object...
            _contentFactory.AddMetadataDatastream("RELS-EXT", "RDF Statements about this object", relsMetadata, "application/rdf+xml");
        }

        public void AddRelsExtMetaData(RelationshipMetadata relsMetadata)
        {
            _contentFactory.AddMetadataDatastream("RELS-EXT", "RDF Statements about this object", relsMetadata, "application/rdf+xml");
        }

        /// <summary>
        /// Adds some very basic Dublin Core metadata
        /// </summary>
        /// <param name="mimeType"></param>
        /// <param name="timestamp"></param>
        public void AppendDCMetaData(string mimeType, string timestamp, string documentAuthor)
        {
            //Set Metadata datastream "DC" with some basic meta
            _contentFactory.AddMetadataDatastream("DC", "Dublin Core Metadata", BuildSomeDC(_label, "", "", "", documentAuthor, timestamp, "", mimeType, _objectPID, "en", "", "", "", "", ""), mimeType);
        }

        public void AppendDCMetaData(DublinCoreMetadata metadata)
        {
            //Set Metadata datastream "DC" with some basic meta
            _contentFactory.AddMetadataDatastream("DC", "Dublin Core Metadata", metadata, "text/xml");
        }

        public void AppendDescriptionMetaData(string timestamp, string documentAuthor)
        {
            string modsMetadata = BuildSomeMODs(documentAuthor, timestamp).Xml;
            _contentFactory.AddManagedContentDatastream("descMetadata", "MODS metadata", "text/xml", 0, System.Text.Encoding.ASCII.GetBytes(modsMetadata));
        }

        public void AppendDescriptionMetaData(MODSMetadata metadata)
        {
            string modsMetadata = metadata.Xml;
            _contentFactory.AddManagedContentDatastream("descMetadata", "MODS metadata", "text/xml", 0, System.Text.Encoding.ASCII.GetBytes(modsMetadata));
        }

        /// <summary>
        /// Provides provision for adding a custom Metadata section
        /// </summary>
        /// <param name="dsId">Data stream Id</param>
        /// <param name="dsLabel">Data stream label</param>
        /// <param name="inMetadata">metadata to add</param>
        /// <param name="mimeType">mime type of the data stream e.g. "text/xml"</param>
        public void AppendMetaData(string dsId, string dsLabel, IMetadata inMetadata, string mimeType)
        {
            _contentFactory.AddMetadataDatastream(dsId, dsLabel, inMetadata, mimeType);
        }

        /// <summary>
        /// Appends the content data stream
        /// </summary>
        /// <param name="content"></param>
        /// <param name="mimeType"></param>
        public void AppendContent(byte[] content, string mimeType)
        {
            _contentFactory.AddManagedContentDatastream("content", "Document", mimeType, content.Length, content);
        }

        /// <summary>
        /// Ingests the completed object into the Fedora repository
        /// </summary>
        /// <param name="content"></param>
        /// <param name="mimeType"></param>
        public void DepositContentObject()
        {
            DoIngest();
        }

         
        public string Pid
        {
            get
            {
                return _objectPID;
            }
            set
            {
                _objectPID = value;
            }
        }
    }
}
