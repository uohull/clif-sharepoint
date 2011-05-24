using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using System.Collections;

namespace uk.ac.hull.repository.hydranet.hydracontent
{
    public class ContentObject : IComparable
    {
        private string objectPID = "";
        private bool isCollection = false;
        private string label = "";
        private string mimeType = "";


        public ContentObject()
        {
            this.objectPID = string.Empty;
            this.isCollection = false;
            this.label = string.Empty;
            this.mimeType = string.Empty;
        }

        public ContentObject(string objectPID, bool isCollection, string label, string mimeType)
        {
            this.objectPID = objectPID;
            this.isCollection = isCollection;
            this.label = label;
            this.mimeType = mimeType;
        }
    
    public string ObjectPID 
    {
        set
        {
            objectPID = value;
        }
        get
        {
            return objectPID;
        }
    }

    public bool IsCollection
    {
        set
        {
            isCollection = value;
        }
        get
        {
            return isCollection;
        }
    }

    public string Label
    {
        set
        {
            label = value;
        }
        get
        {
            return label;
        }
    }
    public string MimeType
    {
        set
        {
            mimeType = value;
        }
        get
        {
            return mimeType;
        }

    }

    public int CompareTo(object obj)
    {
        //check to see if the type being passed is of 
        //Content object type

        if (!(obj is ContentObject))
        {
            //sinces its not, we throw an ArgumentException
            throw new ArgumentException("Object provided is of the wrong type");
        }

        //converts the object being passed to the type ContentObject
        ContentObject contentObject = (ContentObject)obj;

        //execute CompareTo and set its return
        //value to our int variable
        int cmpl = this.ObjectPID.CompareTo(contentObject.ObjectPID);
        //check the value of the CompareTo method
        if (!(cmpl == 0))
        {
            //not equal to zero, negative comparision
            return cmpl;
        }

        return this.ObjectPID.CompareTo(contentObject.ObjectPID);
    }
             
    }
}