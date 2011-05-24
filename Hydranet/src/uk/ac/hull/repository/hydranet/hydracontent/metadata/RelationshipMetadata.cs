using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;

namespace uk.ac.hull.repository.hydranet.hydracontent.metadata
{
    public class RelationshipMetadata : IMetadata
    {
        public const string HAS_MODEL_REL_CONSTANT = "hasModel";
        public const string IS_MEMBER_REL_CONSTANT = "isMemberOf";

        private const string HAS_MODEL_NS_CONSTANT = "xmlns=\"info:fedora/fedora-system:def/model#\"";
        private const string IS_MEMBER_NS_CONSTANT = "xmlns=\"info:fedora/fedora-system:def/relations-external#\"";

        private const int REL_PREDICATE_DIMENSION = 0;
        private const int TARGET_OBJECT_DIMENSION = 1;

        private const string FORMAT_URI = "info:fedora/fedora-system:FedoraRELSExt-1.0";

        private string[,] relationships;
        private int relationshipCount;
        private string objectPID;
        private string xml;
        private bool? isCollection = null;



        public string HAS_MODEL_REL
        {
            get
            {
                return HAS_MODEL_REL_CONSTANT;
            }
        }
        public string IS_MEMBER_REL
        {
            get
            {
                return IS_MEMBER_REL_CONSTANT;
            }
        }


        public string[,] Relationships
        {
            get
            {
                return relationships;
            }
            set
            {
                relationships = value;
            }
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
        }

        public RelationshipMetadata(string inObjectPID) {
            objectPID = inObjectPID;

            relationships = new string[20, 2];
            relationshipCount = 0;
        }
    
        public void AddRelationship(string relationshipPredicate, string targetObjectPID  )
        {
            //Add the two relationship elements to the array
            relationships[relationshipCount, REL_PREDICATE_DIMENSION] = relationshipPredicate;
            relationships[relationshipCount, TARGET_OBJECT_DIMENSION] = targetObjectPID;

            relationshipCount++; //Increment the relationshipCount

        }



        #region IMetadata Members

        public string Xml
        {
            get
            {

                xml = "<rdf:RDF xmlns:rdf=\"http://www.w3.org/1999/02/22-rdf-syntax-ns#\">" +
                "<rdf:Description rdf:about=\"info:fedora/" + objectPID + "\">";

                //Loop around the relationships added...
                for (int count = 0; count < relationshipCount; count++)
                {
                    string predicateNamespace = ""; 
                    
                    //Test to see what predicate it is...
                    if (relationships[count, REL_PREDICATE_DIMENSION].Equals(HAS_MODEL_REL_CONSTANT))
                    {
                        predicateNamespace = HAS_MODEL_NS_CONSTANT; //hasModel predicate
                    }
                    else if (relationships[count, REL_PREDICATE_DIMENSION].Equals(IS_MEMBER_REL_CONSTANT))
                    {
                        predicateNamespace = IS_MEMBER_NS_CONSTANT; //isMemberOf predicate
                    }
             
                    xml = xml + "<" + relationships[count, REL_PREDICATE_DIMENSION] + " " + predicateNamespace +
                            " rdf:resource=\"info:fedora/" + relationships[count, TARGET_OBJECT_DIMENSION] + "\"></" + relationships[count, REL_PREDICATE_DIMENSION] + ">"; 
                }

                //If the isCollection is set...
                if (isCollection != null)
                {
                    xml = xml + "<rel:isCollection xmlns:rel=\"info:fedora/fedora-system:def/relations-external#\">" + isCollection.ToString() + "</rel:isCollection>";
                }

                xml = xml + "</rdf:Description></rdf:RDF>";

                return xml;
 
            }
            set
            {
                xml = value;
            }
        }

        public string FormatURI
        {
            get {
                return FORMAT_URI;
            }
        }

        #endregion
    }
}
