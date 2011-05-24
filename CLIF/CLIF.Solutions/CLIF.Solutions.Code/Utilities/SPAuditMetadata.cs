using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CLIF.Solutions.Code
{
    public class SPAuditMetadata
    {
        public static string SP_AUDIT_NS = "urn:CLIF.hull.ac.uk:SPAudit";
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
                return SP_AUDIT_NS;
            }
        }
    }
}
