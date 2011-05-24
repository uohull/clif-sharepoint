using System;
using System.Collections.Generic;
using System.Text;


using uk.ac.hull.repository.hydranet.hydracontent;
using uk.ac.hull.repository.hydranet.fedora;

namespace CLIF.Solutions.Code.Utilities
{
    /**********************************************************************************
     Title:              RepositoryHelper
     Author:             Suresh Thampi
     Project:            CLIF.Solutions
     Date:               04/11/2010
     Copyright:          © Center for e-research(CeRch), King's College London 2010 All Rights Reserved.
     Description:     
                            
     ------------------------------------------------------------------------------
     Revision History
     Date             Ref       Author          Reason          Remarks        

     ***********************************************************************************/

    class RepositoryHelper
    {

        /// <summary>
        /// This method return all files in a collection.
        /// </summary>
        /// <param name="parentID">string</param>
        /// <returns>ContentObjectList</returns>
        public static ContentObjectList GetFilesInCollection(string parentID)
        {
            ContentObjectList contentObjList = new ContentObjectList();
            ResourceIndexClient resIndClient = new ResourceIndexClient();
            contentObjList = resIndClient.getSetChildrenObjects(parentID, false);
            return contentObjList;
        }

        /// <summary>
        /// This method return all collections in a collection.
        /// </summary>
        /// <param name="parentID">string</param>
        /// <returns>ContentObjectList</returns>
        public static ContentObjectList GetCollectionsInCollection(string parentID)
        {
            ContentObjectList contentObjList = new ContentObjectList();
            ResourceIndexClient resIndClient = new ResourceIndexClient();
            contentObjList = resIndClient.getSetChildrenObjects(parentID, true);
            return contentObjList;
        }
    }
}
