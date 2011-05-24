
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.PolicyManagement
{
    public class ObjectPolicy
    {
        private string xml;
     

        PolicyProperties policyProp = new PolicyProperties();

        public ObjectPolicy(string pid, bool isPrivate, string userId)
        {
            policyProp.Pid = pid;
            policyProp.IsPrivate = isPrivate;
            policyProp.UserId = userId;
            policyProp.PolicyId = pidToPolicyId(pid);
        }

        public string PolicyId
        {
            get
            {
                return policyProp.PolicyId;
            }
        }

        public string Xml
        {
            get
            {
                ObjectPolicyContainer policy = new ObjectPolicyContainer(policyProp);
                this.xml = policy.Xml;
                return this.xml;
            }
        }

        private string pidToPolicyId(string pid)
        {
            char[] delimiter = {':'};
            string[] pidComp = pid.Split(delimiter);
            string policyId = pidComp[0] + "-" + pidComp[1]; 
            return policyId;
        }
    }
}
