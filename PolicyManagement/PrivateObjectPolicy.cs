using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.PolicyManagement
{
    class PrivateObjectPolicy
    {
        private string xml, policyId, userId, pid;
        private int ruleCount;

        public PrivateObjectPolicy(string pid, string policyId, string userId)
        {
            this.ruleCount = 1;
            this.pid = pid;
            this.policyId = policyId;
            this.userId = userId;
        }

        public string Xml
        {
            get
            {
                PolicyWrapper wrapper = new PolicyWrapper(PrivatePolicyConfig.ALGORITHM, policyId, PrivatePolicyConfig.DESCRIPTION);
                PolicyTarget target = new PolicyTarget(pid);
                PolicyRule rule1 = new PolicyRule(pid, ruleCount++);

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
