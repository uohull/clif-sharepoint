using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;

namespace CLIF.Solutions.Code
{
    public class DocumentApprovalTasks :IDisposable
    {
        /// <summary>
        /// This method adds tasks ref to the list in the root site.
        /// </summary>
        /// <param name="Task">SPListItem</param>
        public int AddTask(string Title, DateTime DueDate, string Approver, string DocumentAuthor, string TaskUrl, SPWeb Web, SPFieldLookupValue PublicationLocation)
        {
            SPList ObjTaskList = Web.Lists["Document Approval Tasks"];
            SPListItem _newTask = ObjTaskList.Items.Add();
            _newTask["Title"] = Title;
          
            _newTask["Document Approver"] = Web.SiteGroups[Approver];
            //SPHelper.SetFieldValueUser(_newTask, "Document Approver",Approver);
            SPHelper.SetFieldValueUser(_newTask, "Document Author",DocumentAuthor);
            _newTask["Due Date"] = DueDate;
            _newTask["Task Ref"] = TaskUrl;
            _newTask["Published Location"] = PublicationLocation;
            _newTask.SystemUpdate();
            Web.Update();
            return _newTask.ID;
        }
        /// <summary>
        /// This method update task ref in the root site.
        /// </summary>
        /// <param name="TaskId">string</param>
        /// <param name="IsComplete">bool</param>
        public void UpdateTask(int TaskId, string Status, SPWeb Web)
        {
            SPList ObjTaskList = Web.Lists["Document Approval Tasks"];
            SPListItem _task = ObjTaskList.Items.GetItemById(TaskId);
            _task["Status"] = Status;
            _task.SystemUpdate();
            Web.Update();
        }
        #region IDisposable Members

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
