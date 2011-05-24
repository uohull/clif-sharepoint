using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using System.Xml;
using System.Diagnostics;

namespace CLIF.Solutions.Code
{
    class WebConfigEntries : SPFeatureReceiver
    {
        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            try
            {             
                SPWebApplication webApp;
                WebConfigModifications webConfigMods;                

                webApp = (SPWebApplication)properties.Feature.Parent;
                webConfigMods = new WebConfigModifications();

                //load the web.config settings from the feature root  
                //xmlDoc = new XmlDataDocument();
                //xmlDoc.Load(string.Format("{0}\\{1}", properties.Feature.Definition.RootDirectory, "web.config"));

                //configuration/SharePoint/SafeControls  
                //xPath = "configuration/SharePoint/SafeControls";
                //AddWebConfigNodes(xPath, ref xmlDoc, ref webConfigMods, ref webApp);

                //configuration/appSettings  
               // xPath = "configuration/appSettings";
                //AddWebConfigNodes(xPath, ref xmlDoc, ref webConfigMods, ref webApp);

                //configuration/appSettings  
                //xPath = "configuration/configSections";
                //AddWebConfigNodes(xPath, ref xmlDoc, ref webConfigMods, ref webApp);


                //configuration  
                //xPath = "configuration/system.serviceModel";
                //AddWebConfigNodes(xPath, ref xmlDoc, ref webConfigMods, ref webApp);
                
                
                webApp.Farm.Servers.GetValue<SPWebService>().ApplyWebConfigModifications();
                webApp.Update(); 
                

            }
            catch (Exception ex)
            {
                WriteToEventLog(EventType.Error, ex.Message); 
            }
        }

        public override void FeatureInstalled(SPFeatureReceiverProperties properties)
        {
        }
        public override void FeatureUninstalling(SPFeatureReceiverProperties properties)
        {
        }
        
         private void AddWebConfigNodes(string xPath, ref XmlDataDocument xmlDoc, ref WebConfigModifications webConfigMods, ref SPWebApplication webApp)  
        {
            foreach (XmlNode node in xmlDoc.SelectSingleNode(xPath))
            {
                if (node.NodeType != XmlNodeType.Comment)
                {
                    webConfigMods.AddWebConfigNode(webApp, xPath, node, node.Attributes);
                }
            }
         }  
   
         public override void FeatureDeactivating(SPFeatureReceiverProperties properties)  
         {  
             SPWebApplication webApp = (SPWebApplication)properties.Feature.Parent;  
             WebConfigModifications webConfigMods = new WebConfigModifications();  
             webConfigMods.RemoveWebConfigNodes(webApp);  
             webApp.Farm.Servers.GetValue<SPWebService>().ApplyWebConfigModifications();  
             webApp.Update();  
         }
         public enum EventType
         {
             Error,
             Information
         }
         /// <summary>
         /// This Function Logs information/Error in the Event Log
         /// </summary>
         /// <param name="eType">EventType</param>
         /// <param name="Message">string</param>
         public static void WriteToEventLog(EventType eType, string Message)
         {
             EventLog ObjEventLog = new EventLog();
             if (!System.Diagnostics.EventLog.SourceExists("EMTrains"))
             {
                 System.Diagnostics.EventLog.CreateEventSource("EMTrains", "Application");
             }
             ObjEventLog.Source = "EMTrains";


             if (eType == EventType.Error)
             {
                 ObjEventLog.WriteEntry(Message, EventLogEntryType.Error);
             }
             else
             {
                 ObjEventLog.WriteEntry(Message, EventLogEntryType.Information);
             }
         }
    }
}
