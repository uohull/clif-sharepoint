using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.PolicyManagement
{
    public class RuleCondition
    {
        private string user;

        public RuleCondition(string user)
        {
            this.user = user;
        }

        public string Xml
        {
            get
            {
                string xml = "<Condition FunctionId=\"" + Dictionary.NOT + "\">" + "\n"
                        + "<Apply FunctionId=\"" + Dictionary.STRING_AT_LEAST + "\">" + "\n"
                        + "<SubjectAttributeDesignator AttributeId=\"" + Dictionary.LOGINID + "\" MustBePresent=\"false\"" + " DataType=\"" + Dictionary.STRING_TYPE + "\" />"
                        + "<Apply FunctionId=\"" + Dictionary.STRING_BAG + "\">" + "\n"
                        + "<AttributeValue DataType=\"" + Dictionary.STRING_TYPE + "\">" + user + "</AttributeValue>" + "\n"
                        + "<AttributeValue DataType=\"" + Dictionary.STRING_TYPE + "\">" + "fedoraAdmin" + "</AttributeValue>" + "\n"
                        + "</Apply>" + "\n"
                        + "</Apply>" + "\n"
                        + "</Condition>" + "\n";

                return xml;
            }
        }

    }
}
