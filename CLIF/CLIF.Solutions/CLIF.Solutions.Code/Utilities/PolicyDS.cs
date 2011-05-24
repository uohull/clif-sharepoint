using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CLIF.Solutions.Code
{
    public class PolicyDS
    {
        public static string POLICY_DS_NS = "urn:CLIF.hull.ac.uk:PolicyDS";
        private string _xml;

        public string Xml
        {
            get
            {
                return _xml;
            }
            set
            {
                _xml = value;
            }
        }

        public string FormatURI
        {
            get
            {
                return POLICY_DS_NS;
            }
        }
    }
}
