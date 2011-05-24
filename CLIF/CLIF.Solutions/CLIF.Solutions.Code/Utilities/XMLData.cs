using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Serialization;
using System.Xml;
using System.IO;
using uk.ac.hull.repository.hydranet.hydracontent;
using uk.ac.hull.repository.hydranet.fedora;


namespace CLIF.Solutions.Code
{
    /**********************************************************************************
     Title:              AddItemToRepository
     Author:             Suresh Thampi
     Project:            CLIF.Solutions
     Date:               04/11/2010
     Copyright:          © Center for e-research(CeRch), King's College London 2010 All Rights Reserved.
     Description:     
                                
     ------------------------------------------------------------------------------
     Revision History
     Date             Ref       Author          Reason          Remarks        

     ***********************************************************************************/
    public class XMLData
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public XMLData()
        { }

        /// <summary>
        /// This method returns repository data in XML format.
        /// </summary>
        /// <param name="ObjectPID">string</param>
        /// <returns>XMLDocument</returns>
        public XmlDocument GetData(string ObjectPID)
        {
            string _parentId = ObjectPID;

            ResourceIndexClient ObjRIC = new ResourceIndexClient();
            ContentObjectList ObjCOL = new ContentObjectList();
            ObjCOL = ObjRIC.getSetChildrenObjects(_parentId, true);

            //Converting data to XML format
            XmlSerializer ObjXmlSerializer = new XmlSerializer(ObjCOL.GetType());

            StringWriter ObjStringWriter = new StringWriter();
            ObjXmlSerializer.Serialize(ObjStringWriter, ObjCOL);

            XmlDocument ObjXmldocument = new XmlDocument();
            ObjXmldocument.LoadXml(ObjStringWriter.ToString());
            return ObjXmldocument;
        }
        /// <summary>
        /// This method return files in an Object
        /// </summary>
        /// <param name="ObjectPID">string</param>
        /// <returns></returns>
        public ContentObjectList GetFiles(string ObjectPID)
        {
            string _parentId = ObjectPID;

            ResourceIndexClient ObjRIC = new ResourceIndexClient();
            ContentObjectList ObjCOL = new ContentObjectList();
            ObjCOL = ObjRIC.getSetChildrenObjects(_parentId,false);                       
            return ObjCOL;
        }
    }
}
