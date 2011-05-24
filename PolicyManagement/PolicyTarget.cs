using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.PolicyManagement
{
    class PolicyTarget : ITarget
    {
        private string pid;

        public PolicyTarget(string pid)
        {
            this.pid = pid;
        }

        public string Xml
        {
            get
            {
                PolicySection subjects = new PolicySection(Dictionary.Section.SUBJECTS, pid, true);
                PolicySection resources = new PolicySection(Dictionary.Section.RESOURCES, pid, false);
                PolicySection actions = new PolicySection(Dictionary.Section.ACTIONS, pid, true);

                string xml = "<Target>" + "\n";
                xml += subjects.Xml + "\n";
                xml += resources.Xml + "\n";
                xml += actions.Xml + "\n";
                xml += "</Target>" + "\n";

                return xml;
            }
        }
    }
}
