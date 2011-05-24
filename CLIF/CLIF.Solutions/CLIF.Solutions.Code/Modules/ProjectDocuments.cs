using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;

namespace CLIF.Solutions.Code
{
    public class ProjectDocument:IDisposable
    {
   
        /// <summary>
        /// This method returns all publication locations
        /// </summary>
        /// <returns>SPListItemCollection</returns>
        public SPListItem GetProjectDetails(string PID,SPWeb Web)
        {
            SPList ObjList = Web.Lists["Project Documents"];
            SPQuery _query=new SPQuery();
            _query.Query="<Where><Eq><FieldRef Name=\"Persistent_x0020_ID\" /><Value Type=\"Text\">" + PID +  "</Value></Eq></Where>";
            return ObjList.GetItems(_query)[0];
        }

        #region IDisposable Members

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
