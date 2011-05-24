using System;
using System.Web;
using System.ComponentModel;
using System.ServiceModel;
using uk.ac.hull.repository.hydranet.fedora;
using uk.ac.hull.repository.hydranet.hydracontent.metadata;
using uk.ac.hull.repository.hydranet.hydracontent;
using uk.ac.hull.repository.hydranet.service;
using System.Configuration;
using System.Linq;
using System.Xml.Linq;
using System.Net;
using System.IO;

namespace uk.ac.hull.repository.hydranet.service
{
    public class HydraServiceFedoraImpl : IHydraService
    {

        protected ContentFactory _contentFactory;
        internal IFedoraManagement _fedoraManagement;
        internal IFedoraAccess  _fedoraAccess;
        protected FedoraServer _fedoraServer;
        protected ResourceIndexClient _riClient;
        protected ContentObjectList _contentObjList;

        //--------------------------------------------------//
        //ObjectPID
        //--------------------------------------------------//
        private string _objectPID;
        public string ObjectPID
        {
            set{_objectPID = value;}
            get{return _objectPID;}
        }
        //--------------------------------------------------//
        public HydraServiceFedoraImpl()
        {
            _fedoraServer = new FedoraServer();
            this._objectPID = string.Empty;
        }        
        #region IHydraService Members
        /// <summary>
        ///     <para>This method will create an Hydra 'Implicit set object' with a fized (Singleton PID), and add three metadata datastreams:-
        ///     - descMetadata (for descriptive metadata) - Hydra suggests MODS XML
        ///     - contentMetadata (for content specific metadata)
        ///     - rightsMetadata (for rights specific metadata) - Loosely based on METS 
        ///     </para>
        ///     <para>For content model info - See http://www.fedora-commons.org/confluence/display/hydra/Hydra+content+models+and+disseminators</para>
        ///     <para>This method needs to be expanded to allow the setting of the metadata datastreams</para>
        /// </summary>
        /// <param name="nameSpace">Object namespace</param>
        /// <param name="label">Object label</param>
        /// <param name="parentID">Object parent (if it's a sub-set)</param>
        public string DepositSingletonSet(string nameSpace, string label, string parentID) 
        {
            string objectPID = string.Empty;
            //Create a fedoraManagement client
            _fedoraManagement = new FedoraManagementSOAPImpl(_fedoraServer);

            //--------------------------------------------------//
            // Modified By Suresh Thampi on 04/01/2011
            if (_objectPID == string.Empty)
            {
                //objectPID = nameSpace + ":1";
                objectPID = GetNextPID(nameSpace);
            }
            else
            {
                objectPID = _objectPID;
            }
            //--------------------------------------------------//

            //Use the content factory to create a new object
            ContentFactory contentFactory = new ContentFactory(objectPID, label, "A", "fedoraAdmin");

            DateTime dt = DateTime.Now;
            string timestamp = String.Format("{0:yy-MM-dd}", dt);

            //Create a basic DC Datastream
            contentFactory.AddMetadataDatastream("DC", "Dublin Core Metadata", BuildSomeDC(label, "", "", "", "fedoraAdmin", timestamp, "", "text/xml", objectPID, "en", "", "", "", "", ""), "text/xml");
       
            //Create the RELS-EXT XML, this contains the content model membership relationships etc...
            RelationshipMetadata relsMetadata = new RelationshipMetadata(objectPID);
            relsMetadata.AddRelationship(relsMetadata.HAS_MODEL_REL, "hydra-cModel:implicitSet");
            relsMetadata.AddRelationship(relsMetadata.HAS_MODEL_REL, "hydra-cModel:commonMetadata");
            relsMetadata.IsCollection = true;

            //Set parentID if its defined... 
            if (parentID != null)
            {
                relsMetadata.AddRelationship(relsMetadata.IS_MEMBER_REL, parentID);
            }
          
            //Creates the metadata datastream with the RelationshipsMetadata object created above
            contentFactory.AddMetadataDatastream("RELS-EXT", "RDF Statements about this object", relsMetadata, "application/rdf+xml");

            //Creates some sample DC - ***Need a MODS editor - Object model for insertion into objects***
            string descMetadata = BuildSomeDC(label, "", "", "", "fedoraAdmin", timestamp, "", "text/xml", objectPID, "en", "", "", "", "", "").Xml.ToString();


            //Creates a 'ManagedContentDatastream' for the descMetadata, using the sample DC created above
            contentFactory.AddManagedContentDatastream("descMetadata", "Descriptive metadata", "text/xml", 0, System.Text.Encoding.ASCII.GetBytes(descMetadata)); 
            // Creates a contentMetadata datastream...
            contentFactory.AddManagedContentDatastream("contentMetadata", "Content metadata", "text/xml", 0, System.Text.Encoding.ASCII.GetBytes("<adminMetadata />"));
            // Creates a rightsMetadata datastream...
            contentFactory.AddManagedContentDatastream("rightsMetadata", "Rights metadata", "text/xml", 0, System.Text.Encoding.ASCII.GetBytes("<rightsMetadata />"));

            //Once we have added all the datastreams, we can use the contentFactory.getContentAsByteArray() to get Byte[] rep of the FOXML object
            byte[] objectXML = contentFactory.GetContentAsByteArray();

            using (OperationContextScope scope = new OperationContextScope(_fedoraManagement.FedoraManagementProxy.InnerChannel))
            {
                try
                {
                    _fedoraManagement.ingest(objectXML, "info:fedora/fedora-system:FOXML-1.0", "Ingested by the .net hydra client", scope);  //Ingests into Fedora instance
                }
                catch (Exception ex)
                {
                    if (!ex.Message.Contains("ObjectExistsException"))
                    throw ex;
                }
            }
            return objectPID;
           
        }

        /// <summary>
        ///     <para>This example method will simple create an Hydra 'Implicit set object', and add three metadata datastreams:-
        ///     - descMetadata (for descriptive metadata) - Hydra suggests MODS XML
        ///     - contentMetadata (for content specific metadata)
        ///     - rightsMetadata (for rights specific metadata) - Loosely based on METS 
        ///     </para>
        ///     <para>For content model info - See http://www.fedora-commons.org/confluence/display/hydra/Hydra+content+models+and+disseminators</para>
        ///     <para>This method needs to be expanded to allow the setting of the metadata datastreams</para>
        /// </summary>
        /// <param name="nameSpace">Object namespace</param>
        /// <param name="label">Object label</param>
        /// <param name="parentID">Object parent (if it's a sub-set)</param>
        public void DepositSet(string nameSpace, string label, string parentID)
        {
            //Create a fedoraManagement client
            _fedoraManagement = new FedoraManagementSOAPImpl(_fedoraServer);

            string objectPID;

            objectPID = GetNextPID(nameSpace);
            //Use the content factory to create a new object
            ContentFactory contentFactory = new ContentFactory(objectPID, label, "A", "fedoraAdmin");

            DateTime dt = DateTime.Now;
            string timestamp = String.Format("{0:yy-MM-dd}", dt);

            //Create a basic DC Datastream
            contentFactory.AddMetadataDatastream("DC", "Dublin Core Metadata", BuildSomeDC(label, "", "", "", "fedoraAdmin", timestamp, "", "text/xml", objectPID, "en", "", "", "", "", ""), "text/xml");

            //Create the RELS-EXT XML, this contains the content model membership relationships etc...
            RelationshipMetadata relsMetadata = new RelationshipMetadata(objectPID);
            relsMetadata.AddRelationship(relsMetadata.HAS_MODEL_REL, "hydra-cModel:implicitSet");
            relsMetadata.AddRelationship(relsMetadata.HAS_MODEL_REL, "hydra-cModel:commonMetadata");
            relsMetadata.IsCollection = true;

            //Set parentID if its defined... 
            if (parentID != null)
            {
                relsMetadata.AddRelationship(relsMetadata.IS_MEMBER_REL, parentID);
            }

            //Creates the metadata datastream with the RelationshipsMetadata object created above
            contentFactory.AddMetadataDatastream("RELS-EXT", "RDF Statements about this object", relsMetadata, "application/rdf+xml");

            //Creates some sample DC - ***Need a MODS editor - Object model for insertion into objects***
            string descMetadata = BuildSomeDC(label, "", "", "", "fedoraAdmin", timestamp, "", "text/xml", objectPID, "en", "", "", "", "", "").Xml.ToString();


            //Creates a 'ManagedContentDatastream' for the descMetadata, using the sample DC created above
            contentFactory.AddManagedContentDatastream("descMetadata", "Descriptive metadata", "text/xml", 0, System.Text.Encoding.ASCII.GetBytes(descMetadata));
            // Creates a contentMetadata datastream...
            contentFactory.AddManagedContentDatastream("contentMetadata", "Content metadata", "text/xml", 0, System.Text.Encoding.ASCII.GetBytes("<adminMetadata />"));
            // Creates a rightsMetadata datastream...
            contentFactory.AddManagedContentDatastream("rightsMetadata", "Rights metadata", "text/xml", 0, System.Text.Encoding.ASCII.GetBytes("<rightsMetadata />"));

            //Once we have added all the datastreams, we can use the contentFactory.getContentAsByteArray() to get Byte[] rep of the FOXML object
            byte[] objectXML = contentFactory.GetContentAsByteArray();

            using (OperationContextScope scope = new OperationContextScope(_fedoraManagement.FedoraManagementProxy.InnerChannel))
            {
                 _fedoraManagement.ingest(objectXML, "info:fedora/fedora-system:FOXML-1.0", "Ingested by the .net hydra client", scope);  //Ingests into Fedora instance
            }

        }


        /// <summary>
        /// <para>This method creates a simple Hydra legal content object, with one
        /// binary content datastream.</para>
        /// <param>May be extended to enable flexible multiple DS ingests</param>
        /// </summary>
        /// <param name="nameSpace">Fedora namespace to be assigned to the document</param>
        /// <param name="label">Object label/filename</param>
        /// <param name="parentID">Object parent set if applicable</param>
        /// <param name="localFilePath">File path to the local file</param>
        /// <param name="mimeType">Content mimetype</param>
        /// <param name="ingestOwner">Id of the user assigned repository ownership rights over this document..if in doubt use fedoraAdmin</param>
        /// <param name="documentAuthor">Name of the person who authored the document ..e.g for copyright</param>/// 
        public void DepositSimpleContentObject(string nameSpace, string label, string parentID, string localFilePath, string mimeType, string ingestOwner, string documentAuthor)
        {
            _fedoraManagement = new FedoraManagementSOAPImpl(_fedoraServer);

            string objectPID;

            //Get the next available PID from fedora.
            objectPID = GetNextPID(nameSpace);
            //We do allow datastream versioning within Fedora, therefore we TimeStamp the target filename for easier verification
            string filename = TimeStampFilename(objectPID.Replace(":", "") + localFilePath.Substring(localFilePath.LastIndexOf('.')));
            long fileLength = 0;
            string contentLocation = UploadFile(localFilePath, filename, out fileLength); //Upload file to the FTP store 

            if (fileLength < 0)    // sometimes a zero length file is valid
                throw new OverflowException("Hydranet - unable to upload file invalid file size");
                
            if (fileLength > int.MaxValue)
                throw new OverflowException("Hydranet - unable to upload file (maximum size = 2gb)");

            //New instance of contentFactory with objectPID, label etc...    
            _contentFactory = new ContentFactory(objectPID, label, "A", ingestOwner);
            AddNonContentStreams(label, parentID, mimeType, objectPID, documentAuthor);
            _contentFactory.AddManagedContentDatastream("content", "Document", mimeType, (int)fileLength, contentLocation);
            DoIngest();
         }


        /// <summary>
        /// <para>This method creates a simple Hydra legal content object, with one
        /// binary content datastream.</para>
        /// <param>May be extended to enable flexible multiple DS ingests</param>
        /// </summary>
        /// <param name="nameSpace">Fedora namespace to be assigned to the document</param>
        /// <param name="label">Object label/filename</param>
        /// <param name="parentID">Object parent set if applicable</param>
        /// <param name="fileURL">Url to the file</param>
        /// <param name="mimeType">Content mimetype</param>
        /// <param name="ingestOwner">Id of the user assigned repository ownership rights over this document..if in doubt use fedoraAdmin</param>
        /// <param name="documentAuthor">Name of the person who authored the document ..e.g for copyright</param>
        public void DepositSimpleContentObject(string nameSpace, string label, string parentID, Uri fileURL, string mimeType, string ingestOwner, string documentAuthor)
        {
            _fedoraManagement = new FedoraManagementSOAPImpl(_fedoraServer);

            string objectPID;

            //Get the next available PID from fedora.
            objectPID = GetNextPID(nameSpace);
            //New instance of contentFactory with objectPID, label etc...    
            _contentFactory = new ContentFactory(objectPID, label, "A", ingestOwner);
            AddNonContentStreams(label, parentID, mimeType, objectPID, documentAuthor);
            _contentFactory.AddManagedContentDatastream("content", "Document", mimeType, 0, fileURL.ToString());
            DoIngest();
        }

        /// <summary>
        /// <para>This method creates a simple Hydra legal content object, with one
        /// binary content datastream.</para>
        /// <param>May be extended to enable flexible multiple DS ingests</param>
        /// </summary>
        /// <param name="nameSpace">Fedora namespace to be assigned to the document</param>/// 
        /// <param name="label">Object label/filename</param>
        /// <param name="parentID">Object parent set if applicable</param>
        /// <param name="content">content to embed</param>
        /// <param name="mimeType">Content mimetype</param>
        /// <param name="ingestOwner">Id of the user assigned repository ownership rights over this document..if in doubt use fedoraAdmin</param>
        /// <param name="documentAuthor">Name of the person who authored the document ..e.g for copyright</param>      
        public string DepositSimpleContentObject(string nameSpace, string label, string parentID, byte[] content, string mimeType, string ingestOwner, string documentAuthor)
        {
            if (content.Length < 0)    // sometimes a zero length file is valid
                throw new OverflowException("Hydranet - unable to upload file invalid file size");

            if (content.Length > int.MaxValue)
                throw new OverflowException("Hydranet - unable to upload file (maximum size = 2gb)");

            _fedoraManagement = new FedoraManagementSOAPImpl(_fedoraServer);

            string objectPID;

            //Get the next available PID from fedora.
            objectPID = GetNextPID(nameSpace);
            //New instance of contentFactory with objectPID, label etc...    
            _contentFactory = new ContentFactory(objectPID, label, "A", ingestOwner);
            AddNonContentStreams(label, parentID, mimeType, objectPID, documentAuthor);
            _contentFactory.AddManagedContentDatastream("content", "Document", mimeType, content.Length, content);
            DoIngest();
            return objectPID;
        }

        protected void AddNonContentStreams(string label, string parentID, string mimeType, string objectPID, string documentAuthor)
        {

            //Get date
            DateTime dt = DateTime.Now;
            string timestamp = String.Format("{0:yy-MM-dd}", dt);

           
            //Set Metadata datastream "DC" with some basic meta
            _contentFactory.AddMetadataDatastream("DC", "Dublin Core Metadata", BuildSomeDC(label, "", "", "", documentAuthor, timestamp, "", mimeType, objectPID, "en", "", "", "", "", ""), "text/xml");

            //Create the relationship metadata
            RelationshipMetadata relsMetadata = new RelationshipMetadata(objectPID);
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

            //Set the content datastream with the content

            //Creates some sample DC - ***Need a MODS editor - Object model for insertion into objects***
            string descMetadata = BuildSomeDC(label, "", "", "", documentAuthor, timestamp, "", "text/xml", objectPID, "en", "", "", "", "", "").Xml.ToString();

            //Creates a 'ManagedContentDatastream' for the descMetadata, using the sample DC created above
            _contentFactory.AddManagedContentDatastream("descMetadata", "MODS metadata", "text/xml", 0, System.Text.Encoding.ASCII.GetBytes(descMetadata));
            // Creates a contentMetadata datastream...
            _contentFactory.AddManagedContentDatastream("contentMetadata", "Content metadata", "text/xml", 0, System.Text.Encoding.ASCII.GetBytes("<adminMetadata />"));
            // Creates a rightsMetadata datastream...
            _contentFactory.AddManagedContentDatastream("rightsMetadata", "Rights metadata", "text/xml", 0, System.Text.Encoding.ASCII.GetBytes("<rightsMetadata />"));
      }

      protected void DoIngest()
      {
            //Gets the foxml content
            byte[] objectXML = _contentFactory.GetContentAsByteArray();

            using (OperationContextScope scope = new OperationContextScope(_fedoraManagement.FedoraManagementProxy.InnerChannel))
            {
                //Ingest the object into Fedora...
                _fedoraManagement.ingest(objectXML, "info:fedora/fedora-system:FOXML-1.0", "Ingested by the .net hydra client", scope);
            }
       }
     
       public byte[] GetObject(string pid, string webProtocol)
       {
            string url = webProtocol + "://" + _fedoraServer.ServerAddress + ":" + _fedoraServer.ServerPort + "/fedora/objects/" + pid + "/datastreams/content/content";
            WebRequest webRequest = WebRequest.Create(url);  
            WebResponse webResponse = webRequest.GetResponse(); 
            BinaryReader br = new BinaryReader(webResponse.GetResponseStream(),System.Text.Encoding.GetEncoding("UTF-8"));
            return br.ReadBytes(System.Convert.ToInt32(webResponse.ContentLength));
       }

       public byte[] GetObjectHydra(string pid)
       {
           return GetDisseminationOutput("hydra-sDef:genericContent", "getContent", pid);
       }

       public byte[] GetDisseminationOutput(string serviceDefinitionPid, string methodName, string pid)
       {
           _fedoraAccess = new FedoraAccessSOAPImpl(_fedoraServer);

           using (OperationContextScope scope = new OperationContextScope(_fedoraAccess.FedoraAccessProxy.InnerChannel))
           {
               return _fedoraAccess.getDissemination(pid, serviceDefinitionPid, methodName, null, null);
           }
       }

        /// <summary>
        ///   Upload a file to the FTP server
        /// </summary>
        /// <param name="localFilePath">Full local path to the file</param>
        /// <param name="remoteFileName">Target filename</param>
        /// <returns>ContentURL</returns>
       protected string UploadFile(string localFilePath, string remoteFileName, out long fileLength)
        {
            string hydranetFtpServerAddress = System.Configuration.ConfigurationManager.AppSettings["HydranetFtpServerAddress"];
            if (String.IsNullOrEmpty(hydranetFtpServerAddress))
                throw new FormatException("Invalid HydranetFtpServerAddress check the application or web config file");
            else
                hydranetFtpServerAddress = hydranetFtpServerAddress.Trim();

            int hydranetFtpServerPort = 0;
            if (!int.TryParse(System.Configuration.ConfigurationManager.AppSettings["HydranetFtpServerPort"], out hydranetFtpServerPort))
                throw new FormatException("Invalid HydranetFtpServerPort check the application or web config file");

            string hydranetFtpUsername = System.Configuration.ConfigurationManager.AppSettings["HydranetFtpUsername"];
            if (String.IsNullOrEmpty(hydranetFtpUsername))
                throw new FormatException("Invalid HydranetFtpUsername check the application or web config file");
            else
                hydranetFtpUsername = hydranetFtpUsername.Trim();

            string hydranetFtpPassword = System.Configuration.ConfigurationManager.AppSettings["HydranetFtpPassword"];
            if (String.IsNullOrEmpty(hydranetFtpPassword))
                throw new FormatException("Invalid HydranetFtpPassword check the application or web config file");
            else
                hydranetFtpPassword = hydranetFtpPassword.Trim();

            string hydranetFtpBaseFilePath = System.Configuration.ConfigurationManager.AppSettings["HydranetFtpBaseFilePath"];
            if (String.IsNullOrEmpty(hydranetFtpBaseFilePath))
                throw new FormatException("Invalid HydranetFtpBaseFilePath check the application or web config file");
            else
                hydranetFtpBaseFilePath = hydranetFtpBaseFilePath.Trim();

            string hydranetFtpHttpBaseUrl = System.Configuration.ConfigurationManager.AppSettings["HydranetFtpHttpBaseUrl"];
            if (String.IsNullOrEmpty(hydranetFtpHttpBaseUrl))
                throw new FormatException("Invalid HydranetFtpHttpBaseUrl check the application or web config file");
            else
                hydranetFtpHttpBaseUrl = hydranetFtpHttpBaseUrl.Trim();

            string hydranetFtpRemoteFilePath = System.Configuration.ConfigurationManager.AppSettings["HydranetFtpRemoteFilePath"];
            if (String.IsNullOrEmpty(hydranetFtpRemoteFilePath))
                throw new FormatException("Invalid HydranetFtpRemoteFilePath check the application or web config file");
            else
                hydranetFtpRemoteFilePath = hydranetFtpRemoteFilePath.Trim();

            FileTransfer fileTransfer = new FileTransfer(hydranetFtpServerAddress, hydranetFtpServerPort, hydranetFtpUsername, hydranetFtpPassword, hydranetFtpBaseFilePath, hydranetFtpHttpBaseUrl);
            fileLength = -1;
            return fileTransfer.Upload(localFilePath, hydranetFtpRemoteFilePath, remoteFileName, out fileLength);
        }

        /// <summary>
        /// Deletes content from the external FTP server
        /// </summary>
        /// <param name="objectPID">ObjectPID</param>
        /// <param name="dsId">DatastreamId of the content to delete</param>
        protected void DeleteExternalContent(string objectPID, string dsId)
        {

            string fileLocation;
            string filename;

            using (OperationContextScope scope = new OperationContextScope(_fedoraManagement.FedoraManagementProxy.InnerChannel))
            {
                uk.ac.hull.repository.hydranet.serviceref.fedoramanagement.Datastream datastream = _fedoraManagement.getDatastream(objectPID, dsId, "", scope);
                fileLocation = datastream.location;
                filename = fileLocation.Substring(fileLocation.LastIndexOf('/') + 1);
            }
      
            string ftpServerAddress = "rep1.adir.hull.ac.uk";
            int ftpServerPort = 2121;
            string ftpUsername = "fedora@adir.hull.ac.uk";
            string ftpPassword = "Loyalty is the best policy!";
            string ftpBaseFilePath = "test/";
            string httpBaseUrl = "http://rep1.adir.hull.ac.uk";

            FileTransfer fileTransfer = new FileTransfer(ftpServerAddress, ftpServerPort, ftpUsername, ftpPassword, ftpBaseFilePath, httpBaseUrl);

            fileTransfer.Delete("test/Users/", filename);

        }


        protected string TimeStampFilename(string inFileName)
        {
            DateTime dt = DateTime.Now;
            string timestamp = String.Format("{0:yyMMddHHmmss}", dt);
            string outFileName = inFileName.Insert(inFileName.LastIndexOf('.') , "-" + timestamp);

            return outFileName;
        }


        public DublinCoreMetadata BuildSomeDC(string title, string source, string contributor, string coverage, string creator, string date, string description,
            string format, string identifier, string language, string publisher, string relation, string rights, string subject, string type)
        {
            DublinCoreMetadata dcMetadata;
            dcMetadata = new DublinCoreMetadata();

            dcMetadata.Title = title;
            dcMetadata.Source = source;
            dcMetadata.Contributor = contributor;
            dcMetadata.Coverage =  coverage;
            dcMetadata.Creator = creator;
            dcMetadata.Date = date;
            dcMetadata.Description = description;
            dcMetadata.Format = format;
            dcMetadata.Identifier = identifier;
            dcMetadata.Language = language;
            dcMetadata.Publisher = publisher;
            dcMetadata.Relation = relation;
            dcMetadata.Rights = rights;
            dcMetadata.Subject = subject;
            dcMetadata.Type = type;
                        
            return dcMetadata;
        }


        public MODSMetadata  BuildSomeMODs(string documentAuthor, string timestamp)
        {
//            string modsXml = @"<ns1:modsCollection xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:ns1=""http://www.loc.gov/mods/v3"" xmlns:xlink=""http://www.w3.org/1999/xlink"">
//            return new MODSMetadata(modsXml);
            throw new NotImplementedException();
        }

        public ContentObjectList ListContentObjects(String parentId)
        {
            _riClient = new ResourceIndexClient();
            _contentObjList = _riClient.getSetChildrenObjects(parentId);

            return _contentObjList;

        }

        /// <summary>
        /// <para>DeleteObject deletes a singular or a set of objects
        /// from Fedora. If we choose to Delete a parent set, the 
        /// children set objets will also be deleted</para>
        /// 
        /// </summary>
        /// <param name="objectPid">ObjectPid to be deleted</param>
        /// <param name="isCollection">isCollection bool (if it is a set or not)</param>
        public void DeleteObject(string objectPID, bool isCollection)
        {
            _fedoraManagement = new FedoraManagementSOAPImpl(_fedoraServer);
        
            //First we need to determine if it is a collection
            if (isCollection)
            {
                //Instantiates the ResourceIndexClient
                _riClient = new ResourceIndexClient();
                _contentObjList = _riClient.getRecursiveSetChildrenObjects(objectPID); //Gets all set children recursively...

                int noOfChildren = _contentObjList.Count;

                //For each contentObj in the contentObjList
                foreach (ContentObject contentObj in _contentObjList)
                {
                    if (contentObj.IsCollection)
                    {
                        //If the contentObj is a collection/set then we only need to delete the object dr
                        using (OperationContextScope scope = new OperationContextScope(_fedoraManagement.FedoraManagementProxy.InnerChannel))
                        {
                            //Purge the set object, and force it.
                            _fedoraManagement.purgeObject(contentObj.ObjectPID, "Object deleted by .NET Client", false, scope);
                        }
                    }

                    else
                    {
                        DeleteExternalContent(contentObj.ObjectPID, "content");

                        using (OperationContextScope scope = new OperationContextScope(_fedoraManagement.FedoraManagementProxy.InnerChannel))
                        {
                            //Purge the set object, and force it.
                            _fedoraManagement.purgeObject(contentObj.ObjectPID, "Object deleted by .NET Client", false, scope);
                        }
                    }
                }
                //We then need to get a list of 'all' the child objects... 

                //Once we have list of the all the child objects, we need to delete them one by one.. 
            }
            else
            {
                DeleteExternalContent(objectPID, "content");

                using (OperationContextScope scope = new OperationContextScope(_fedoraManagement.FedoraManagementProxy.InnerChannel))
                {
                    //Purge the set object, and force it.
                    _fedoraManagement.purgeObject(objectPID, "Object deleted by .NET Client", false, scope);
                }
            }
          
        }

        public void ListObjects()
        {
            throw new NotImplementedException();
        }

       

        #endregion


     /// <summary>
     /// Uses the Fedora Management GetNextPID() management operation.
     /// This method simply returns a reserved singular ObjectPID
     /// </summary>
     /// <param name="objectNamespace">Namespace of the reserved object</param>
     /// <returns></returns>
        protected string GetNextPID(string objectNamespace)
        {
            string[] objectPIDList;
            string objectPID;

            using (OperationContextScope scope = new OperationContextScope(_fedoraManagement.FedoraManagementProxy.InnerChannel))
            {
                objectPIDList = _fedoraManagement.getNextPID("1", objectNamespace, scope);
            }

            int noOfPids = objectPIDList.GetLength(0);

            if (noOfPids > 0)
            {
               objectPID = objectPIDList[noOfPids - 1].ToString();

               return objectPID;
            }
            else
            {
                throw new Exception ("getNextPID failed, object pid not returned from Fedora");
            }
          
        }
       
    }
}
