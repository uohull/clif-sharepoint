using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.PolicyManagement
{
    class PolicyRule
    {
        string xml;
        private int ruleNumber;
        private string pid;
        
        public PolicyRule(string pid, int num)
        {
            ruleNumber = num;
            this.pid = pid;
        }

        public string Xml
        {
            get
            {
                RuleTarget target = new RuleTarget(pid);
                RuleCondition condition = new RuleCondition("user");
                this.xml = "<Rule RuleId=\"" + ruleNumber + "\" Effect=\"Deny\">";
                this.xml += target.Xml;
                this.xml += condition.Xml;
                this.xml += "</Rule>" + "\n";
                return this.xml;
            }
        }

    }
}
