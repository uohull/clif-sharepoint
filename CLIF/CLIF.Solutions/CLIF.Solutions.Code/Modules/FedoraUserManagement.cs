using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Configuration;
using Microsoft.SharePoint;

namespace CLIF.Solutions.Code
{
    class FedoraUserManagement : IDisposable
    {
        string _userConfigFilePath = string.Empty;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public FedoraUserManagement()
        {
  
            //Getting the user config file path from the root site
            using (SPWeb ObjWeb = SPHelper.GetLandingWeb())
            {
                //Getting default destination repository path
                SPList ObjList = ObjWeb.Lists["Repository Config Files"];
                SPQuery ObjQuery = new SPQuery();
                string _query = string.Format("<Where><Eq><FieldRef Name=\"Title\" /><Value Type=\"Text\">{0}</Value></Eq></Where>", "Users");
                ObjQuery.Query = _query;

                //Getting list item collection
                SPListItem item = ObjList.GetItems(ObjQuery)[0];
                _userConfigFilePath = item["File Path"].ToString();
            }
            
        }
        /// <summary>
        /// This method adds fedora users
        /// </summary>
        /// <param name="UserName">string</param>
        /// <param name="Password">string</param>
        public void AddUser(string UserName,string Password)
        {            
            XElement doc = XElement.Load(_userConfigFilePath);

            //Checking if fedora user already exists.
            var existingusers = from existinguser in XElement.Load(_userConfigFilePath).Elements("user")
                                where existinguser.Attribute("name").Value==UserName
                                select existinguser;
            if (existingusers == null)
            {
                //Add new fedora user
                XElement user = new XElement("user",
                     new XAttribute("name", UserName),
                     new XAttribute("password", Password),
                          new XElement("attribute",
                             new XAttribute("name", "fedoraRole"),
                                 new XElement("value", "administrator")));
                //Add new element
                doc.Add(user);

                //Save XML file
                doc.Save(_userConfigFilePath);
            }
        }
        #region IDisposable Members

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
