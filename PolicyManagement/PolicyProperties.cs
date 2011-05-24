using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.PolicyManagement
{
    public class PolicyProperties
    {      
        private string pid, policyId, userId, algorithm, description;
        private int numRules, numUsers;
        bool isPrivate;

        private const string ALGORITHM = Dictionary.ALGO_FIRST_APPLICABLE;
        private const string DESCRIPTION = "This is an object-specific policy. "
            + "It could be stored inside the digital object itself in the POLICY datastream "
            + "OR in the directory for object-specific policies. (The directory location is set in the "
            + "Authorization module configuration in the Fedora server configuration file (fedora.fcfg)."
            + "This policy shows how to deny access to all raw datastreams in the object except to "
            + "particular users (e.g., the object owners). ";
        private const int NUMRULES = 1;


        public PolicyProperties()
        {
            this.algorithm = ALGORITHM;
            this.description = DESCRIPTION;
            this.numRules = 1;
        }

        public string Pid
        {
            get
            {
                return pid;
            }
            set
            {
                pid = value;
            }
        }

        public string PolicyId
        {
            get
            {
                return policyId;
            }
            set
            {
                policyId = value;
            }
        }

        public string UserId
        {
            get
            {
                return userId;
            }
            set
            {
                userId = value;
            }
        }

        public int NumRules
        {
            get
            {
                return numRules;
            }
            set
            {
                numRules = value;
            }
        }

        public bool IsPrivate
        {
            get
            {
                return isPrivate;
            }
            set
            {
                isPrivate = value;
            }
        }

        public string Algorithm
        {
            get
            {
                return algorithm;
            }
            set
            {
                algorithm = value;
            }
        }

        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                description = value;
            }
        }

        public int NumUsers
        {
            get
            {
                return numUsers;
            }
            set
            {
                numUsers = value;
            }
        }
    }
}
