using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace uk.ac.hull.repository.hydranet.service
{
    public class Utilities
    {
        public static string GetFormatedNamespace(string value)
        {
            string _namespace = value;
            _namespace=_namespace.Replace(" ", "");
            _namespace=_namespace.Replace("(", "");
            _namespace=_namespace.Replace(")", "");
            return _namespace;
        }
    }
}
