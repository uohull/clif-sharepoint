using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Microsoft.SharePoint;
using System.Collections;
using CLIF.Solutions.Code;
using System.Threading;

namespace CLIF.Solutions.Code
{
    /**********************************************************************************
    Title:              LookUpWithPickerFields
    Author:             Suresh Thampi
    Project:            CLIF.Solutions
    Date:               04/11/2010
    Copyright:          © Center for e-research(CeRch), King's College London 2010 All Rights Reserved.
    Description:     
                            
    ------------------------------------------------------------------------------
    Revision History
    Date             Ref       Author          Reason          Remarks        

    ***********************************************************************************/
    public class LookUpWithPickerFields : SPFeatureReceiver
    {
        string RootPath = string.Empty;
        string PathValue = string.Empty;
        /// <summary>
        /// This event fires when the feature is activated.
        /// </summary>
        /// <param name="properties">SPFeatureReceiverProperties</param>
        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            RootPath = properties.Feature.Definition.RootDirectory;
            PathValue = properties.Feature.Properties["XmlFields"].Value;
            using (SPSite site   = (SPSite)properties.Feature.Parent)
            {
                using (SPWeb web = site.OpenWeb())
                {
                    ThreadPool.QueueUserWorkItem(new WaitCallback(RunProcess), web.Url);
                }
            }
        }
        private void RunProcess(object state)
        {
            Thread.Sleep(25000);

            string url = (string)state;
            Guid RootGUID = Guid.Empty;
            string FieldElement = string.Empty;
            string ColumnName = string.Empty;
            string SearchFields = string.Empty;
            string ListName = string.Empty;
            string ContentTypes = string.Empty;
            //int Position = 1;
            string PositionString = string.Empty;
            string XmlFieldPath = string.Empty;

            string ResultCAML = string.Empty;
            string ListField = string.Empty;
            //int EntityEditorRows = 0;
            XmlTextReader ObjReader = null;
            ArrayList alContentTypes = new ArrayList();

            //Custom Lookup Variables
            string _lookUpListName = string.Empty;
            string _lookUpListField = string.Empty;
            int _lookUpEntityEditorRows = 0;
            List<string> _lookUpSearchFields = new List<string>();

            try
            {
                //Getting the XML files with all LookUp Fields
                XmlFieldPath = RootPath + "\\" + PathValue;

                //Getting reference to the SPSite Object
                using (SPSite ObjSite = new SPSite(url))
                {
                    using (SPWeb ObjWeb = ObjSite.OpenWeb())
                    {
                        ObjReader = new XmlTextReader(XmlFieldPath);
                        while (ObjReader.Read())
                        {
                            if (ObjReader.LocalName == "Field")
                            {
                                ColumnName = string.Empty;
                                ListName = string.Empty;
                                ContentTypes = string.Empty;

                                //Getting the Column Name from the XML File
                                if (ObjReader.MoveToAttribute("LookUp:ListName"))
                                {
                                    _lookUpListName = ObjReader.Value;
                                    ObjReader.MoveToElement();
                                }
                                if (ObjReader.MoveToAttribute("LookUp:ListField"))
                                {
                                    _lookUpListField = ObjReader.Value;
                                    ObjReader.MoveToElement();
                                }
                                if (ObjReader.MoveToAttribute("LookUp:SearchFields"))
                                {
                                    _lookUpSearchFields.Clear();
                                    string[] values = ObjReader.Value.Split(',');
                                    foreach (string value in values)
                                    {
                                        _lookUpSearchFields.Add(value);
                                    }
                                    ObjReader.MoveToElement();
                                }
                                if (ObjReader.MoveToAttribute("LookUp:EntityEditorRows"))
                                {
                                    _lookUpEntityEditorRows = Convert.ToInt32(ObjReader.Value);
                                    ObjReader.MoveToElement();
                                }

                                //Getting the Column Name from the XML File
                                if (ObjReader.MoveToAttribute("Name"))
                                {
                                    ColumnName = ObjReader.Value;
                                    ObjReader.MoveToElement();
                                }

                                //Getting the List Name from the XML File
                                if (ObjReader.MoveToAttribute("List"))
                                {
                                    ListName = ObjReader.Value;
                                    ObjReader.MoveToElement();
                                }

                                //Getting the Content Type(s) from the XML File
                                if (ObjReader.MoveToAttribute("LookUp:ContentTypes"))
                                {

                                    ContentTypes = ObjReader.ReadOuterXml();
                                    ObjReader.MoveToElement();
                                }

                                FieldElement = ObjReader.ReadOuterXml();


                                // Refine XML String to make the CAML Conform to the Schema
                                ResultCAML = RefineXML(ListName, ContentTypes, FieldElement, ObjWeb);

                                // now create the column
                                LookupFieldWithPicker ObjLookUpFieldWithPicker = CreateLookUpWithPickerColumn(ObjWeb, ResultCAML, ColumnName, SearchFields);

                                // add the column to appropriate content types
                                AddColumnToContentTypes(ObjWeb, ColumnName, ContentTypes);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new SPException("The feature not be activated. [Error]:" + ex.Message);
            }
            finally
            {
                ObjReader.Close();
            }
        }
        /// <summary>
        /// This Method Refines XML String 
        /// </summary>
        /// <param name="LookUpListName">string</param>
        /// <param name="LookUpList">SPList</param>
        /// <param name="InvalidAttribute">string</param>
        /// <param name="ElementString">string</param>
        /// <returns>string</returns>
        private string RefineXML(string LookUpListName, string InvalidAttributes, string ElementString, SPWeb Web)
        {
            string _refinedXML = string.Empty;
            string _listWithName = string.Empty;
            string _listWithGuid = string.Empty;
            string _lookupWebId = string.Empty;

            // Get reference to LookUp List 
            _lookupWebId = Web.ID.ToString();
            SPList ObjLookUpList = Web.Lists[LookUpListName];
            _listWithName = string.Format("List=\"{0}\"", LookUpListName);
            _listWithGuid = string.Format("List=\"{0}\"", "{" + ObjLookUpList.ID + "}");

            //Replace List Name With GUID
            _refinedXML = ElementString.Replace(_listWithName, _listWithGuid);

            //Remove Any Invalid Attribute
            _refinedXML = _refinedXML.Replace(InvalidAttributes, string.Empty);

            //Setting the LookupWebId
            _refinedXML = _refinedXML.Replace("{LookupWebId}", "{" + _lookupWebId + "}");

            //Setting SourceID
            _refinedXML = _refinedXML.Replace("{SourceID}", "{" + _lookupWebId + "}");

            return _refinedXML;
        }

        /// <summary>
        /// Attempt to delete the column. Note that this will fail if the column is in use, 
        /// i.e. it is used in a content type or list. I prefer to not catch the exception 
        /// (though it may be useful to add extra logging), hence feature deactivation/re-activation 
        /// will fail. This effectively means this feature cannot be deactivated whilst the column 
        /// is in use.
        /// </summary>
        /// <param name="column">Column to delete.</param>
        private void AttemptColumnDelete(SPWeb web, string ColumnName)
        {
            if (web.Fields.ContainsField(ColumnName))
            {
                web.Fields.GetFieldByInternalName(ColumnName).Delete();
            }
        }

        /// <summary>
        /// This Method Creates LookUp with picker Field from the XML file
        /// </summary>
        /// <param name="Web">SPWeb</param>
        /// <param name="ColumnDefinitionXml">string</param>
        /// <param name="ColumnName">string</param>
        private LookupFieldWithPicker CreateLookUpWithPickerColumn(SPWeb Web, string ColumnDefinitionXml, string ColumnName, string SearchFields)
        {
            LookupFieldWithPicker ObjLookUpFieldWithPicker = null;
            string CreatedColumnName = string.Empty;

            //Delete Field
            AttemptColumnDelete(Web, ColumnName);

            //fix
            ColumnDefinitionXml = ColumnDefinitionXml.Replace("LookUp:", "");

            // Create Column from the CAML definition..
            CreatedColumnName = Web.Fields.AddFieldAsXml(ColumnDefinitionXml);

            //Getting Reference to the newly created LookUp With Picker Field
            ObjLookUpFieldWithPicker = Web.Fields.GetFieldByInternalName(CreatedColumnName) as LookupFieldWithPicker;

            return ObjLookUpFieldWithPicker;
        }

        /// <summary>
        /// This Method Adds the Field to Content Tyee(s)
        /// </summary>
        /// <param name="Web">SPWeb</param>
        /// <param name="ColumnName">string</param>
        /// <param name="ContentTypes">string</param>
        private void AddColumnToContentTypes(SPWeb Web, string ColumnName, string ContentTypes)
        {
            ArrayList alFields = new ArrayList();
            int Position = 0;
            string[] _attributes;
            char[] commaSep = { ',' };
            SPField lookupColumn = Web.Fields.GetField(ColumnName);
            string contentTypeNames = ContentTypes.Replace("\"", "").Substring(ContentTypes.IndexOf('=') + 1);
            if (lookupColumn != null)
            {
                foreach (string contentTypeName in contentTypeNames.Split(commaSep, StringSplitOptions.RemoveEmptyEntries))
                {
                    _attributes = contentTypeName.Split('|');
                    Position = Convert.ToInt32(_attributes[2]);

                    SPContentType ObjContentTypes = Web.ContentTypes[_attributes[0]];
                    if (ObjContentTypes != null)
                    {
                        if (ObjContentTypes.FieldLinks[lookupColumn.Id] == null)
                        {
                            SPFieldLink _flLookUpColumn = new SPFieldLink(lookupColumn);
                            _flLookUpColumn.DisplayName = _attributes[1];
                            ObjContentTypes.FieldLinks.Add(_flLookUpColumn);
                            ObjContentTypes.Update(true);

                            //ReOrdering Site Column in the ContentType
                            //alFields.Clear();

                            //foreach (SPField field in ObjContentTypes.Fields)
                            //{
                            //    if (field.InternalName != "ContentType")
                            //    {
                            //        if (field.InternalName == lookupColumn.InternalName)
                            //        {
                            //            alFields.Insert(Position, field.InternalName);
                            //        }
                            //        else
                            //        {
                            //            alFields.Add(field.InternalName);
                            //        }
                            //    }
                            //}
                            //ObjContentTypes.FieldLinks.Reorder(alFields.ToArray(typeof(string)) as string[]);
                            //ObjContentTypes.Update(true);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// This event fires when the feature is Deactivating.
        /// </summary>
        /// <param name="properties">SPFeatureReceiverProperties</param>
        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
        }
        /// <summary>
        /// This event fires when the feature is installed.
        /// </summary>
        /// <param name="properties">SPFeatureReceiverProperties</param>
        public override void FeatureInstalled(SPFeatureReceiverProperties properties)
        {
        }
        /// <summary>
        /// This event fires when the feature is Uninstalling.
        /// </summary>
        /// <param name="properties">SPFeatureReceiverProperties</param>
        public override void FeatureUninstalling(SPFeatureReceiverProperties properties)
        {
        }
    }
}
