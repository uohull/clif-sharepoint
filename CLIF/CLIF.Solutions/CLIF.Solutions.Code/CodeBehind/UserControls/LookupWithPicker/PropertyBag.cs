using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.SharePoint;

namespace CLIF.Solutions.Code
{
    public class LookupFieldWithPickerPropertyBag
    {


        private Guid _ListID, _FieldId, _WebId;
        private int _MaxSearchResults = 1000, _EntityEditorRows;
        private string _searchFields = "", _ListName = "",_FieldName="", _ListField;

        public string ListName
        {
            get { return _ListName; } 
            set { _ListName = value; }
        }
        public string FieldName
        {
            get { return _FieldName; }
            set { _FieldName = value; }
        }

        public Guid ListId { get { return _ListID; } set { _ListID = value; } }
        public string ListField
        {
            //get { return _ListName; } 
            get { return _ListField; }
            set { _ListField = value; }
        }
        public Guid FieldId { get { return _FieldId; } set { _FieldId = value; } }
        public int EntityEditorRows { get{ return _EntityEditorRows;} set{ _EntityEditorRows = value; } }

        public int MaxSearchResults { get { return _MaxSearchResults; } }
        public Guid WebId 
        { 
            get {
                    Guid ID = Guid.Empty;
                    SPSecurity.RunWithElevatedPrivileges(delegate() 
                    { 
                        ID = SPContext.Current.Web.Site.RootWeb.ID; 
                        
                    });
                    return ID;
                    
                 } 
            set { _WebId = value; } 
        }

        public List<string> SearchFields
        {
            get
            {
                return new List<string>(_searchFields.Split(','));
            }
            set
            {
                string str = "";
                foreach (string strField in value)
                {
                    if (str.Length > 0)
                        str += ",";
                    str += strField;
                }

                _searchFields = str;
            }
        }

        public LookupFieldWithPickerPropertyBag()
        {
        }

        public LookupFieldWithPickerPropertyBag(string value)
        {
            string[] tokens = value.Split(';');
            this.ListId = new Guid(tokens[0]);
            this.FieldId = new Guid(tokens[1]);
            this._searchFields = tokens[2];
            this.EntityEditorRows = int.Parse(tokens[3]);
          
 
            this.WebId = new Guid(tokens[4]);

            this.ListName = tokens[5];
            this.FieldName = tokens[6];
            //this.ListField = tokens[7];
        }

        public LookupFieldWithPickerPropertyBag(string ListName, string FieldName, Guid webId, Guid listId, Guid fieldId, List<string> searchFields, int entityEditorRows, string ListField)
        {
            this.ListId = listId;
            this.FieldId = fieldId;
            this.SearchFields = searchFields;
            this.EntityEditorRows = entityEditorRows;
            this.WebId = webId;
            this.ListName = ListName;
            this.FieldName = FieldName;
            //this.ListField = ListField;
        }
        public override string ToString()
        {
            return ListId.ToString() + ";" + FieldId.ToString() + ";" + _searchFields + ";" + EntityEditorRows + ";" + WebId.ToString() + ";" + ListName.ToString() + ";" + FieldName.ToString();
        }
    }

}
