using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Web.UI.WebControls;
using Common.PolicyManagement;
//using Common.Web.Utils;
using System.IO;
using uk.ac.hull.repository.hydranet.hydracontent;
using uk.ac.hull.repository.hydranet.service;
using uk.ac.hull.repository.hydranet.fedora;
using uk.ac.hull.repository.hydranet.serviceref.fedoramanagement;
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

    public class ImportFromRepository : LayoutsPageBase
    {
        protected Button btnYes;
        protected Button btnNo;
        private string _itemId;
        private string _listId;
        private string _sourceUrl;
        protected Label lblTitle;
        protected Label lblContentSubject;
        protected Label lblContentMimeType;
        protected Label lblPersistentID;

        protected override void OnLoad(EventArgs e)
        {
            string _pid = string.Empty;
            SPWeb ObjWeb = SPContext.Current.Web;
            SPList ObjList = null;
            SPListItem item = null;

            if (Request.QueryString["item"] != null)
            {
                _itemId = Request.QueryString["item"].ToString();
            }
            if (Request.QueryString["listn"] == null)
            {
                if (Request.QueryString["list"] != null)
                {
                    _listId = Request.QueryString["list"].ToString();
                }                
                ObjList = ObjWeb.Lists[new Guid(_listId)];
                item = ObjList.Items.GetItemById(Convert.ToInt32(_itemId));
                _pid = item["Persistent ID"].ToString();
            }
            else
            {
                ObjList = ObjWeb.Lists[Request.QueryString["listn"]];
                item = ObjList.Items.GetItemById(Convert.ToInt32(_itemId));
                _pid = item["Persistent ID"].ToString();                
            }
            HydraServiceFedoraExt hydraService = new HydraServiceFedoraExt();
            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = item["Content MimeType"].ToString();
            Response.AddHeader("content-disposition", "attachment; filename=" + _pid + SPHelper.GetFileExtension(item["Content MimeType"].ToString()));
            Response.BinaryWrite(hydraService.GetObjectHydra(_pid));
            Response.Flush();
            Response.End();
        }
        private void DownloadFile(string URI,string type,string name)
        {

            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = @"application/pdf";
            System.IO.FileStream myFileStream = new System.IO.FileStream("Filename.PDF", System.IO.FileMode.Open);
            long FileSize = myFileStream.Length;
            byte[] Buffer = new byte[(int)FileSize];
            myFileStream.Read(Buffer, 0, (int)FileSize);
            myFileStream.Close();
            Response.BinaryWrite(Buffer);
            Response.Flush();
            Response.End();
        }
    }  
}
