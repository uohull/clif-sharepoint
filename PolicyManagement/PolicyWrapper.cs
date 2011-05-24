using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.PolicyManagement
{
    class PolicyWrapper
    {
        private string algorithm;
        private string policyId;
        private string desc;

        public PolicyWrapper(string algorithm, string policyId, string desc)
        {
            this.algorithm = algorithm;
            this.policyId = policyId;
            this.desc = desc;
        }

        public string Header
        {
            get
            {
                string header = "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>" + "\n";
                header += "<Policy xmlns=\"urn:oasis:names:tc:xacml:1.0:policy\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" PolicyId=\"";
                header += policyId;
                header += "\" RuleCombiningAlgId=\"";
                header += algorithm + "\">" + "\n";

                return header;
            }
        }

        public string Description
        {
            get
            {
                return "<Description>" + "\n" + desc + "\n" + "</Description>" + "\n";
            }
        }

        public string Footer
        {
            get
            {
                return "</Policy>" + "\n";
            }
        }
    }
}
