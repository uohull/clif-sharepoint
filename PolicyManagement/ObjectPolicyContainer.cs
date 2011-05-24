using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.PolicyManagement
{
    public class ObjectPolicyContainer
    {
        private string xml;
        private int ruleCount;
        PolicyProperties policyProp;

        public ObjectPolicyContainer(PolicyProperties policyProp)
        {
            this.ruleCount = policyProp.NumRules;
            this.policyProp = policyProp;
        }

        public string Xml
        {
            get
            {
                PolicyWrapper wrapper = new PolicyWrapper(policyProp.Algorithm, policyProp.PolicyId, policyProp.Description);
                PolicyTarget target = new PolicyTarget(policyProp.Pid);
                PolicyRule rule1 = new PolicyRule(policyProp.Pid, ruleCount++);

                this.xml = wrapper.Header;
                this.xml += wrapper.Description;
                this.xml += target.Xml;
                this.xml += rule1.Xml;
                this.xml += wrapper.Footer;
                return xml;           
            }
        }
    }
}
