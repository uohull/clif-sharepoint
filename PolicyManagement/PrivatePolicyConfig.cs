using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.PolicyManagement
{
    public static class PrivatePolicyConfig
    {
        public const string ALGORITHM = Dictionary.ALGO_FIRST_APPLICABLE;
        public const string DESCRIPTION = "This is an object-specific policy. " 
            + "It could be stored inside the digital object itself in the POLICY datastream "
            + "OR in the directory for object-specific policies. (The directory location is set in the "
            + "Authorization module configuration in the Fedora server configuration file (fedora.fcfg)."
            + "This policy shows how to deny access to all raw datastreams in the object except to "
            + "particular users (e.g., the object owners). "; 

    }
}
