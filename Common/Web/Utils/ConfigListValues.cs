//  A. Thompson 2010
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Common.Web.Utils
{
///  This module contains generic helper classes for reading custom configuration data (e.g from a web.config or app.config file)

/// <summary>
/// 
/// </summary>
/// <typeparam name="T"></typeparam>
[ConfigurationCollection(typeof(ConfigurationElement))]
    public class GenericConfigurationElementCollection<T> : ConfigurationElementCollection where T : ConfigurationElement, new()
    {
	    protected override ConfigurationElement CreateNewElement()
	    {
		    return new T();
	    }
	    protected override object GetElementKey(ConfigurationElement element)
	    {
		    return ((T)(element)).ToString();
	    }
	    public T this[int idx]
	    {
		    get { return (T)BaseGet(idx); }
	    }
    }

    public class GenericListOption : ConfigurationElement
    {
        public GenericListOption() { }

        [ConfigurationProperty("value", DefaultValue = "", IsKey = true, IsRequired = true)]
        public string OptionValue
        {
            get { return (string)(base["value"]); }
            set { base["value"] = value; }
        }

        public override string ToString()
        {
            return this.OptionValue;
        }
    }

    public class GenericValue : ConfigurationElement
    {
        public GenericValue() { }

        [ConfigurationProperty("value", DefaultValue = "", IsKey = true, IsRequired = true)]
        public string Value
        {
            get { return (string)(base["value"]); }
            set { base["value"] = value; }
        }

        public override string ToString()
        {
            return this.Value;
        }

    }

    public class GenericKeyValuePair : ConfigurationElement
    {
        public GenericKeyValuePair() { }

        [ConfigurationProperty("key", DefaultValue = "", IsKey = true, IsRequired = true)]
        public string Key
        {
            get { return (string)(base["key"]); }
            set { base["key"] = value; }
        }


        [ConfigurationProperty("value", DefaultValue = "", IsKey = true, IsRequired = true)]
        public string Value
        {
            get { return (string)(base["value"]); }
            set { base["value"] = value; }
        }

        public override string ToString()
        {
            return this.Key;
        }
    }

    public class GenericKeyValueGroupedPair : ConfigurationElement
    {
        public GenericKeyValueGroupedPair() { }

        [ConfigurationProperty("key", DefaultValue = "", IsKey = true, IsRequired = true)]
        public string Key
        {
            get { return (string)(base["key"]); }
            set { base["key"] = value; }
        }

        [ConfigurationProperty("group", DefaultValue = "", IsKey = false, IsRequired = true)]
        public string Group
        {
            get { return (string)(base["group"]); }
            set { base["group"] = value; }
        }

        [ConfigurationProperty("value", DefaultValue = "", IsKey = true, IsRequired = true)]
        public string Value
        {
            get { return (string)(base["value"]); }
            set { base["value"] = value; }
        }

        public override string ToString()
        {
            return this.Key;
        }
    }

}
