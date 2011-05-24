using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Common.Web.Utils;

namespace CLIF.Solutions.Code
{
/// <summary>
/// CLIFConfigurationSettings. A utility class for reading configuration values (e.g from web.config)
/// </summary>
    public class CLIFConfigurationSettings : ConfigurationSection
    {
#region Accessors
	    [ConfigurationProperty("CLIF_MODS_Publisher", IsRequired = true)]
        public GenericValue MODSPublisher
        {
            get
            {
                return (GenericValue)this["CLIF_MODS_Publisher"];
            }
        }

        [ConfigurationProperty("CLIF_MODS_Role_Types", IsRequired = true),
        ConfigurationCollection(typeof(GenericConfigurationElementCollection<GenericListOption>), AddItemName = "option")]
        public GenericConfigurationElementCollection<GenericListOption> MODSRoleTypes
        {
            get 
            {
                return (GenericConfigurationElementCollection<GenericListOption>) this["CLIF_MODS_Role_Types"]; 
            }
        }

        [ConfigurationProperty("CLIF_Rights_Main_Statement", IsRequired = true)]
        public GenericValue RightsMainStatement
        {
            get
            {
                return (GenericValue)this["CLIF_Rights_Main_Statement"];
            }
        }

        [ConfigurationProperty("CLIF_Rights_Optional_Statements", IsRequired = true),
        ConfigurationCollection(typeof(GenericConfigurationElementCollection<GenericListOption>), AddItemName = "option")]
        public GenericConfigurationElementCollection<GenericListOption> RightsOptionalStatements
        {
            get
            {
                return (GenericConfigurationElementCollection<GenericListOption>)this["CLIF_Rights_Optional_Statements"];
            }
        }

        [ConfigurationProperty("CLIF_Language_Terms", IsRequired = true),
        ConfigurationCollection(typeof(GenericConfigurationElementCollection<GenericKeyValuePair>), AddItemName = "option")]
        public GenericConfigurationElementCollection<GenericKeyValuePair> LanguageTerms
        {
            get 
            {
                return (GenericConfigurationElementCollection<GenericKeyValuePair>)this["CLIF_Language_Terms"]; 
            }
        }

        [ConfigurationProperty("CLIF_MODS_Genre", IsRequired = true),
        ConfigurationCollection(typeof(GenericConfigurationElementCollection<GenericKeyValueGroupedPair>), AddItemName = "option")]
        public GenericConfigurationElementCollection<GenericKeyValueGroupedPair> MODSGenres
        {
            get
            {
                return (GenericConfigurationElementCollection<GenericKeyValueGroupedPair>)this["CLIF_MODS_Genre"];
            }
        }

        [ConfigurationProperty("CLIF_Resource_Types", IsRequired = true),
        ConfigurationCollection(typeof(GenericConfigurationElementCollection<GenericKeyValuePair>), AddItemName = "option")]
        public GenericConfigurationElementCollection<GenericKeyValuePair> ResourceTypes
        {
            get 
            {
                return (GenericConfigurationElementCollection<GenericKeyValuePair>) this["CLIF_Resource_Types"]; 
            }
        }

        [ConfigurationProperty("CLIF_MODS_Authority", IsRequired = true)]
        public GenericValue MODSAuthority
        {
            get
            {
                return (GenericValue)this["CLIF_MODS_Authority"];
            }
        }

 
	#endregion    
    }
}
