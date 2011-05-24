using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.PolicyManagement
{
    public class RuleTarget : ITarget
    {
        private string pid;

        public RuleTarget(string pid)
        {
            this.pid = pid;
        }

        public string Xml
        {
            get
            {
                RuleSection subjects = new RuleSection(Dictionary.Section.SUBJECTS, pid, true);
                RuleSection resources = new RuleSection(Dictionary.Section.RESOURCES, pid, true);
                RuleSection actions = new RuleSection(Dictionary.Section.ACTIONS, pid, false);

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
