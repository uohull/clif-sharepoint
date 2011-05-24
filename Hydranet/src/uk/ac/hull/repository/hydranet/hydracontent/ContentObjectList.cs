using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using System.Collections;

namespace uk.ac.hull.repository.hydranet.hydracontent
{
    public class ContentObjectList : CollectionBase
    {
        #region Constructors
        public ContentObjectList() { }
        #endregion

        #region Indexer
        /// <summary>
        /// Indexer for our collection
        /// </summary>
        /// <param name="idx">Index of the collection</param>
        /// <returns>ContentObject at that Index</returns>
        public ContentObject this[int idx]
        {
            get
            {
                //return the ContentObject at the specified index
                return (ContentObject)this.InnerList[idx];
            }
        }
        #endregion

        #region Add
        /// <summary>
        /// Method for adding an ContentObject to the list
        /// </summary>
        /// <param name="contentObj">ContentObject to add</param>
        public void Add(ContentObject contentObj)
        {
            //add this item to the list
            this.InnerList.Add(contentObj);
        }
        #endregion


        #region Remove
        /// <summary>
        /// Method for removing a specified ContentObject from the list
        /// </summary>
        /// <param name="contentObj">ContentObject to remove</param>
        public void Remove(ContentObject contentObj)
        {
            //check to see if the list contains this item, if not - exit the procedure
            if (!((IList)this).Contains(contentObj)) return;

            
            //loop through all the items in the list
            for (int i = 0; i <= this.InnerList.Count; i++)
            {
                //create an instance of the ContentObject
                ContentObject item = (ContentObject)this.InnerList[i];

                //check to see if its in the list
                if ((item.CompareTo(contentObj) == 0))
                {
                    //if its in the list remove it
                    this.RemoveAt(i);
                    return;
                }

            }
        }
        #endregion

        #region OnValidate
        protected override void OnValidate(object value)
        {
            //first validate the object
            base.OnValidate(value);

            //if the object isn't of the correct type
            //throw an ArgumentException
            if (!(value is ContentObject))
            {
                throw new ArgumentException("Collection only supports of type ContentObject");
            }
        }
        #endregion




    }
}
