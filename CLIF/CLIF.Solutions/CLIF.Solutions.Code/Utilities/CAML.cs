using System;
using System.Collections.Generic;
using System.Text;

namespace CLIF.Solutions.Code
{
    class CAML
    {
        List<CAMLField> ObjList = new List<CAMLField>();
       
        public void OrCondition(CAMLField Value)
        {
            ObjList.Add(Value);
        }
        public List<CAMLField> GetFieldValues()
        {
            return ObjList;
        }
        public void Clear()
        {
            ObjList.Clear();
        }
        public string GetCAML()
        {
            string _numberOfDays = string.Empty;
            string strCAML = string.Empty;
            int _count = 0;
            switch (ObjList.Count)
            {
                case 1:
                    foreach (CAMLField item in ObjList)
                    {
                        if (item.FieldRef != "ID")
                        {
                            strCAML = strCAML + "<Eq><FieldRef Name='" + item.FieldRef + "' LookupId='TRUE'/>";
                            strCAML = strCAML + "<Value Type='LookupFieldWithPicker'>" + item.FieldValue + "</Value></Eq>";
                        }
                        else
                        {
                            strCAML = strCAML + "<Eq><FieldRef Name='" + item.FieldRef + "'/>";
                            strCAML = strCAML + "<Value Type='Text'>" + item.FieldValue + "</Value></Eq>";
                        }
                    }
                    break;
                case 2:
                    strCAML = strCAML + "<Or>";
                    foreach (CAMLField item in ObjList)
                    {
                        if (item.FieldRef != "ID")
                        {
                            strCAML = strCAML + "<Eq><FieldRef Name='" + item.FieldRef + "' LookupId='TRUE' />";
                            strCAML = strCAML + "<Value Type='LookupFieldWithPicker' >" + item.FieldValue + "</Value></Eq>";
                        }
                        else
                        {
                            strCAML = strCAML + "<Eq><FieldRef Name='" + item.FieldRef + "'/>";
                            strCAML = strCAML + "<Value Type='Text'>" + item.FieldValue + "</Value></Eq>";
                        }
                    }
                    strCAML = strCAML + "</Or>";
                    break;
                default:

                    foreach (CAMLField item in ObjList)
                    {
                        _count++;
                        if (_count == 1)
                        {
                            strCAML = strCAML + "<Or>";
                        }
                        if (_count > 2)
                        {
                            strCAML = "<Or>" + strCAML;
                        }

                        if (item.FieldRef != "ID")
                        {
                            strCAML = strCAML + "<Eq><FieldRef Name='" + item.FieldRef + "' LookupId='TRUE' />";
                            strCAML = strCAML + "<Value Type='LookupFieldWithPicker'>" + item.FieldValue + "</Value></Eq>";
                        }
                        else
                        {
                            strCAML = strCAML + "<Eq><FieldRef Name='" + item.FieldRef + "' />";
                            strCAML = strCAML + "<Value Type='Text'>" + item.FieldValue + "</Value></Eq>";
                        }
                        if (_count == 2)
                        {
                            strCAML = strCAML + "</Or>";
                        }
                        if (_count > 2)
                        {
                            strCAML = strCAML + "</Or>";
                        }
                    }
                    break;
            }

            return strCAML;
        }
        public string GetPickupCAML()
        {
            string _numberOfDays = string.Empty;
            string strCAML = string.Empty;
            int _count = 0;
            switch (ObjList.Count)
            {
                case 1:
                    foreach (CAMLField item in ObjList)
                    {   
                        if (item.FieldRef != "ID")
                        {
                            strCAML = strCAML + "<Eq><FieldRef Name='" + item.FieldRef + "'/>";
                            strCAML = strCAML + "<Value Type='LookupFieldWithPicker'>" + item.FieldValue + "</Value></Eq>";
                        }
                        else
                        {
                            strCAML = strCAML + "<Eq><FieldRef Name='" + item.FieldRef + "'/>";
                            strCAML = strCAML + "<Value Type='Text'>" + item.FieldValue + "</Value></Eq>";
                        }
                    }
                    break;
                case 2:
                    strCAML = strCAML + "<Or>";
                    foreach (CAMLField item in ObjList)
                    {                       
                        if (item.FieldRef != "ID")
                        {
                            strCAML = strCAML + "<Eq><FieldRef Name='" + item.FieldRef + "' />";
                            strCAML = strCAML + "<Value Type='LookupFieldWithPicker' >" + item.FieldValue + "</Value></Eq>";
                        }
                        else
                        {
                            strCAML = strCAML + "<Eq><FieldRef Name='" + item.FieldRef + "'/>";
                            strCAML = strCAML + "<Value Type='Text'>" + item.FieldValue + "</Value></Eq>";
                        }
                    }
                    strCAML = strCAML + "</Or>";
                    break;
                default:
                    
                    foreach (CAMLField item in ObjList)
                    {
                        _count++;
                        if (_count == 1)
                        {
                            strCAML = strCAML + "<Or>";
                        }
                        if (_count >2)
                        {
                            strCAML = "<Or>" + strCAML;
                        }
                       
                        if (item.FieldRef != "ID")
                        {
                            strCAML = strCAML + "<Eq><FieldRef Name='" + item.FieldRef + "' />";
                            strCAML = strCAML + "<Value Type='LookupFieldWithPicker'>" + item.FieldValue + "</Value></Eq>";
                        }
                        else
                        {
                            strCAML = strCAML + "<Eq><FieldRef Name='" + item.FieldRef + "' />";
                            strCAML = strCAML + "<Value Type='Text'>" + item.FieldValue + "</Value></Eq>";
                        }
                        if (_count == 2)
                        {
                            strCAML = strCAML + "</Or>";
                        }
                        if (_count > 2)
                        {
                            strCAML = strCAML + "</Or>";
                        }
                    }                    
                    break;
            }           
           
            return strCAML;
        }
    }
    class CAMLField
    {
        public CAMLField(string FieldRef,string FieldValue)
        {
            this._fieldRef = FieldRef;
            this._fieldValue = FieldValue;
        }
        private string _fieldRef;
        public string FieldRef
        {
            set
            {
                this._fieldRef = value;
            }
            get 
            {
                return this._fieldRef;
            }

        }
        private string _fieldValue;
        public string FieldValue
        {
            set
            {
                this._fieldValue = value;
            }
            get
            {
                return this._fieldValue;
            }

        }
    }
}
