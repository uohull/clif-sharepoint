using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using uk.ac.hull.repository.hydranet.content;
using uk.ac.hull.repository.hydranet.hydracontent.metadata;

namespace uk.ac.hull.repository.hydranet.hydracontent
{
    public class ContentFactory

    {
        private IFedoraObject genericObject;

        /// <summary>
        /// Constructor for the ContentFactory class
        /// </summary>
        /// <param name="pid">The new ObjectPID</param>
        /// <param name="label">Label</param>
        /// <param name="state">Active state</param>
        /// <param name="ownerId">Owner/User of object</param>
        public ContentFactory(string pid, string label, string state, string ownerId)
        {
            genericObject = new FedoraObjectImpl(pid, label, state, ownerId);
        }

        //public void BuildGenericContentObject(string pid, string label, string state, string ownerId)
        //{
        //    genericObject = new FedoraObjectImpl(pid, label, state, ownerId);
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Returns the ObjectXML as a string</returns>
        public string GetContentAsString()
        {
            string objectXML = genericObject.GetXML();
            
            return objectXML;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Returns the ObjectXML as a Byte[]</returns>
        public byte[] GetContentAsByteArray()
        {
            string objectXML = genericObject.GetXML();
            byte[] objectXMLDataB = System.Text.Encoding.ASCII.GetBytes(objectXML);

            return objectXMLDataB;            
        }

        /// <summary>
        /// Adds a Metadata (XML) datastream, takes IMetadata object as the metadata
        /// </summary>
        /// <param name="dsId">DatastreamID</param>
        /// <param name="dsLabel">Datastream Label</param>
        /// <param name="inMetadata">Metadata object</param>
        /// <param name="mimeType">The mime type (usually "text/xml") but could be different e.g application/rdf+xml</param>
        public void AddMetadataDatastream (string dsId, string dsLabel, IMetadata inMetadata, string mimeType ) 
        {
            IMetadata metadata = inMetadata;
            
            //Creates a new ObjectDatastream 
            IObjectDatastream objectDatastream = new ObjectDatastreamImpl(dsId, dsLabel, mimeType, metadata.FormatURI, null, "DISABLED", "A", "X", true, 0, null, metadata.Xml, null);
            genericObject.AddDatastream(objectDatastream); //Adds to the genericObject
        }

        /// <summary>
        /// Adds an inline-XML datastream.
        /// </summary>
        /// <param name="dsId">DatastreamID</param>
        /// <param name="dsLabel">Datastream Label</param>
        /// <param name="xml">XML content of datastream</param>
        public void AddXMLDatastream(string dsId, string dsLabel, string xml)
        {
            //Creates a new ObjectDatastream 
            IObjectDatastream objectDatastream = new ObjectDatastreamImpl(dsId, dsLabel, "text/xml", "", null, "DISABLED", "A", "X", true, 0, null, xml, null);
            genericObject.AddDatastream(objectDatastream); //Adds to the genericObject
        }

        /// <summary>
        /// Adds a ManagedContentDatastream, which Fedora will retrieve from the URL location.
        /// </summary>
        /// <param name="dsId">DatastreamID</param>
        /// <param name="dsLabel">Datastream Label</param>
        /// <param name="contentMimeType">Content Mimetype</param>
        /// <param name="location">URL location of content</param>
        /// <param name="size">Size of the datastream content in bytes</param>
        public void AddManagedContentDatastream(string dsId, string dsLabel, string contentMimeType, int size, string location)
        {
            AddContentDatastream(dsId, dsLabel, contentMimeType, size, location, "M");
        }
        /// <summary>
        /// Adds a ManagedContentDatastream with the content as a Byte[]
        /// </summary>
        /// <param name="dsId">DatastreamID</param>
        /// <param name="dsLabel">Datastream Label</param>
        /// <param name="contentMimeType">Content Mimetype</param>
        /// <param name="content">The content</param>
        /// <param name="size">Size of the datastream content in bytes</param>
        public void AddManagedContentDatastream(string dsId, string dsLabel, string contentMimeType, int size, byte[] content)
        {
            IObjectDatastream objectDatastream = new ObjectDatastreamImpl(dsId, dsLabel, contentMimeType, "", null, "DISABLED", "A", "M", true, size, null, null, content);
            genericObject.AddDatastream(objectDatastream);
        }

        /// <summary>
        /// Adds a Externally referenced content datastream
        /// </summary>
        /// <param name="dsId">DatastreamID</param>
        /// <param name="dsLabel">Datastream Label</param>
        /// <param name="contentMimeType">Content Mimetype</param>
        /// <param name="location">URL location of the content</param>
        /// <param name="size">Size of the datastream content in bytes</param>
        public void AddExternalRefContentDatastream(string dsId, string dsLabel, string contentMimeType, int size, string location)
        {
            AddContentDatastream(dsId, dsLabel, contentMimeType, size, location, "E");
        }

        /// <summary>
        /// Add a Redirect-referenced content datastream
        /// </summary>
        /// <param name="dsId">DatastreamID</param>
        /// <param name="dsLabel">Datastream Label</param>
        /// <param name="contentMimeType">Content Mimetype</param>
        /// <param name="location">URL location for redirection</param>
        /// <param name="size">Size of the datastream content in bytes</param>
        public void AddRedirectRefContentDatastream(string dsId, string dsLabel, string contentMimeType, int size, string location)
        {
            AddContentDatastream(dsId, dsLabel, contentMimeType, size, location, "R");
        }

        
        private void AddContentDatastream(string dsId, string dsLabel, string contentMimeType, int size, string location, string controlGroup)
        {
            IObjectDatastream objectDatastream = new ObjectDatastreamImpl(dsId, dsLabel, contentMimeType, "", null, "DISABLED", "A", controlGroup, true, size, location, null, null);
            genericObject.AddDatastream(objectDatastream);
        }
        
    }
}
