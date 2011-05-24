using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Drawing;
using System.Reflection;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;

namespace CLIF.DocumentApprovalWF
{
    public sealed partial class DAF
    {
        #region Designer generated code
        
        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCode]
        private void InitializeComponent()
        {
            this.CanModifyActivities = true;
            System.Workflow.Activities.Rules.RuleConditionReference ruleconditionreference1 = new System.Workflow.Activities.Rules.RuleConditionReference();
            System.Workflow.ComponentModel.ActivityBind activitybind1 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind2 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.Runtime.CorrelationToken correlationtoken1 = new System.Workflow.Runtime.CorrelationToken();
            System.Workflow.ComponentModel.ActivityBind activitybind3 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.Runtime.CorrelationToken correlationtoken2 = new System.Workflow.Runtime.CorrelationToken();
            System.Workflow.ComponentModel.ActivityBind activitybind4 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.Activities.Rules.RuleConditionReference ruleconditionreference2 = new System.Workflow.Activities.Rules.RuleConditionReference();
            System.Workflow.ComponentModel.ActivityBind activitybind5 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind6 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind7 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.ComponentModel.ActivityBind activitybind8 = new System.Workflow.ComponentModel.ActivityBind();
            this.CreateRejectedEmail = new System.Workflow.Activities.CodeActivity();
            this.CreateApprovalEmail = new System.Workflow.Activities.CodeActivity();
            this.IsRejected = new System.Workflow.Activities.IfElseBranchActivity();
            this.IsApproved = new System.Workflow.Activities.IfElseBranchActivity();
            this.onTaskChanged = new Microsoft.SharePoint.WorkflowActions.OnTaskChanged();
            this.log_TaskComplete = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.SendStatusEmail = new Microsoft.SharePoint.WorkflowActions.SendEmail();
            this.ifElseActivity = new System.Workflow.Activities.IfElseActivity();
            this.TaskComplete = new Microsoft.SharePoint.WorkflowActions.CompleteTask();
            this.WhileNotApproved = new System.Workflow.Activities.WhileActivity();
            this.log_TaskCreated = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            //this.SendApproverEmail = new Microsoft.SharePoint.WorkflowActions.SendEmail();
            this.onTaskCreated = new Microsoft.SharePoint.WorkflowActions.OnTaskCreated();
            this.CreateAppovalTask = new Microsoft.SharePoint.WorkflowActions.CreateTask();
            this.OnWorkFlowActivated = new Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated();
            // 
            // CreateRejectedEmail
            // 
            this.CreateRejectedEmail.Name = "CreateRejectedEmail";
            this.CreateRejectedEmail.ExecuteCode += new System.EventHandler(this.CreateRejectedEmail_ExecuteCode);
            // 
            // CreateApprovalEmail
            // 
            this.CreateApprovalEmail.Name = "CreateApprovalEmail";
            this.CreateApprovalEmail.ExecuteCode += new System.EventHandler(this.CreateApprovalEmail_ExecuteCode);
            // 
            // IsRejected
            // 
            this.IsRejected.Activities.Add(this.CreateRejectedEmail);
            this.IsRejected.Name = "IsRejected";
            // 
            // IsApproved
            // 
            this.IsApproved.Activities.Add(this.CreateApprovalEmail);
            ruleconditionreference1.ConditionName = "CheckApprovalStatus";
            this.IsApproved.Condition = ruleconditionreference1;
            this.IsApproved.Name = "IsApproved";
            // 
            // onTaskChanged
            // 
            activitybind1.Name = "DAF";
            activitybind1.Path = "onTaskChanged_AfterProperties";
            activitybind2.Name = "DAF";
            activitybind2.Path = "onTaskChanged_BeforeProperties";
            correlationtoken1.Name = "taskToken";
            correlationtoken1.OwnerActivityName = "DAF";
            this.onTaskChanged.CorrelationToken = correlationtoken1;
            this.onTaskChanged.Executor = null;
            this.onTaskChanged.Name = "onTaskChanged";
            activitybind3.Name = "CreateAppovalTask";
            activitybind3.Path = "TaskId";
            this.onTaskChanged.Invoked += new System.EventHandler<System.Workflow.Activities.ExternalDataEventArgs>(this.OnApprovalTaskChanged_Invoked);
            this.onTaskChanged.SetBinding(Microsoft.SharePoint.WorkflowActions.OnTaskChanged.TaskIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind3)));
            this.onTaskChanged.SetBinding(Microsoft.SharePoint.WorkflowActions.OnTaskChanged.AfterPropertiesProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind1)));
            this.onTaskChanged.SetBinding(Microsoft.SharePoint.WorkflowActions.OnTaskChanged.BeforePropertiesProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind2)));
            // 
            // log_TaskComplete
            // 
            this.log_TaskComplete.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.log_TaskComplete.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            this.log_TaskComplete.HistoryDescription = "";
            this.log_TaskComplete.HistoryOutcome = "";
            this.log_TaskComplete.Name = "log_TaskComplete";
            this.log_TaskComplete.OtherData = "";
            this.log_TaskComplete.UserId = -1;
            this.log_TaskComplete.MethodInvoking += new System.EventHandler(this.log_TaskComplete_Method);
            // 
            // SendStatusEmail
            // 
            this.SendStatusEmail.BCC = null;
            this.SendStatusEmail.Body = null;
            this.SendStatusEmail.CC = null;
            correlationtoken2.Name = "workflowToken";
            correlationtoken2.OwnerActivityName = "DAF";
            this.SendStatusEmail.CorrelationToken = correlationtoken2;
            this.SendStatusEmail.From = null;
            this.SendStatusEmail.Headers = null;
            this.SendStatusEmail.IncludeStatus = false;
            this.SendStatusEmail.Name = "SendStatusEmail";
            this.SendStatusEmail.Subject = null;
            this.SendStatusEmail.To = null;
            // 
            // ifElseActivity
            // 
            this.ifElseActivity.Activities.Add(this.IsApproved);
            this.ifElseActivity.Activities.Add(this.IsRejected);
            this.ifElseActivity.Name = "ifElseActivity";
            // 
            // TaskComplete
            // 
            this.TaskComplete.CorrelationToken = correlationtoken1;
            this.TaskComplete.Name = "TaskComplete";
            activitybind4.Name = "CreateAppovalTask";
            activitybind4.Path = "TaskId";
            this.TaskComplete.TaskOutcome = null;
            this.TaskComplete.SetBinding(Microsoft.SharePoint.WorkflowActions.CompleteTask.TaskIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind4)));
            // 
            // WhileNotApproved
            // 
            this.WhileNotApproved.Activities.Add(this.onTaskChanged);
            ruleconditionreference2.ConditionName = "WhileCheck";
            this.WhileNotApproved.Condition = ruleconditionreference2;
            this.WhileNotApproved.Name = "WhileNotApproved";
            // 
            // log_TaskCreated
            // 
            this.log_TaskCreated.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.log_TaskCreated.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            this.log_TaskCreated.HistoryDescription = "";
            this.log_TaskCreated.HistoryOutcome = "";
            this.log_TaskCreated.Name = "log_TaskCreated";
            this.log_TaskCreated.OtherData = "";
            this.log_TaskCreated.UserId = -1;
            this.log_TaskCreated.MethodInvoking += new System.EventHandler(this.logTaskCreated);
            // 
            // SendApproverEmail
            // 
            //this.SendApproverEmail.BCC = null;
            //this.SendApproverEmail.Body = null;
            //this.SendApproverEmail.CC = null;
            //this.SendApproverEmail.CorrelationToken = correlationtoken2;
            //this.SendApproverEmail.From = null;
            //this.SendApproverEmail.Headers = null;
            //this.SendApproverEmail.IncludeStatus = false;
            //this.SendApproverEmail.Name = "SendApproverEmail";
            //this.SendApproverEmail.Subject = null;
            //this.SendApproverEmail.To = null;
            // 
            // onTaskCreated
            // 
            activitybind5.Name = "DAF";
            activitybind5.Path = "onTaskCreated_AfterProperties";
            this.onTaskCreated.CorrelationToken = correlationtoken1;
            this.onTaskCreated.Executor = null;
            this.onTaskCreated.Name = "onTaskCreated";
            activitybind6.Name = "CreateAppovalTask";
            activitybind6.Path = "TaskId";
            this.onTaskCreated.SetBinding(Microsoft.SharePoint.WorkflowActions.OnTaskCreated.TaskIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind6)));
            this.onTaskCreated.SetBinding(Microsoft.SharePoint.WorkflowActions.OnTaskCreated.AfterPropertiesProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind5)));
            // 
            // CreateAppovalTask
            // 
            this.CreateAppovalTask.CorrelationToken = correlationtoken1;
            this.CreateAppovalTask.ListItemId = -1;
            this.CreateAppovalTask.Name = "CreateAppovalTask";
            this.CreateAppovalTask.SpecialPermissions = null;
            this.CreateAppovalTask.TaskId = new System.Guid("6d20c38a-2e72-4937-bced-9280e09fd45a");
            activitybind7.Name = "DAF";
            activitybind7.Path = "TaskProperties";
            this.CreateAppovalTask.MethodInvoking += new System.EventHandler(this.CreateAppovalTask_MethodInvoking);
            this.CreateAppovalTask.SetBinding(Microsoft.SharePoint.WorkflowActions.CreateTask.TaskPropertiesProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind7)));
            // 
            // OnWorkFlowActivated
            // 
            this.OnWorkFlowActivated.CorrelationToken = correlationtoken2;
            this.OnWorkFlowActivated.EventName = "OnWorkflowActivated";
            this.OnWorkFlowActivated.Name = "OnWorkFlowActivated";
            activitybind8.Name = "DAF";
            activitybind8.Path = "workflowProperties";
            this.OnWorkFlowActivated.SetBinding(Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated.WorkflowPropertiesProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind8)));
            // 
            // DAF
            // 
            this.Activities.Add(this.OnWorkFlowActivated);
            this.Activities.Add(this.CreateAppovalTask);
            this.Activities.Add(this.onTaskCreated);
            //this.Activities.Add(this.SendApproverEmail);
            this.Activities.Add(this.log_TaskCreated);
            this.Activities.Add(this.WhileNotApproved);
            this.Activities.Add(this.TaskComplete);
            this.Activities.Add(this.ifElseActivity);
            this.Activities.Add(this.SendStatusEmail);
            this.Activities.Add(this.log_TaskComplete);
            this.Name = "DAF";
            this.CanModifyActivities = false;

        }

        #endregion

        //private Microsoft.SharePoint.WorkflowActions.SendEmail SendApproverEmail;
        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity log_TaskComplete;
        private Microsoft.SharePoint.WorkflowActions.CreateTask CreateAppovalTask;
        private Microsoft.SharePoint.WorkflowActions.SendEmail SendStatusEmail;
        private WhileActivity WhileNotApproved;
        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity log_TaskCreated;
        private Microsoft.SharePoint.WorkflowActions.CompleteTask TaskComplete;
        private Microsoft.SharePoint.WorkflowActions.OnTaskChanged onTaskChanged;
        private Microsoft.SharePoint.WorkflowActions.OnTaskCreated onTaskCreated;
        private CodeActivity CreateRejectedEmail;
        private CodeActivity CreateApprovalEmail;
        private IfElseBranchActivity IsRejected;
        private IfElseBranchActivity IsApproved;
        private IfElseActivity ifElseActivity;
        private Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated OnWorkFlowActivated;
    }
}
