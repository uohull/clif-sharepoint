using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CLIF.Solutions.Code
{
    public class SPWorkflowHistoryMetadata
    {
        public static string SP_WFHIST_NS = "urn:CLIF.hull.ac.uk:SPWorkflowHistory";
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
                return SP_WFHIST_NS;
            }
        }
    }
}
