using System;
using System.Diagnostics;
using System.DirectoryServices;
using System.IO;
using System.Collections;
using System.Collections.Specialized;
using System.Reflection;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace Common.Web.Utils
{
    /// <summary>
    /// Set of utilities to do with Mime Types.
    /// </summary>
    public static class MimeType
    {
        #region public properties

        /// <summary>
        /// The interval in seconds after which cached data should be cleared (defaults to 300 seconds).
        /// </summary>
        public static int RefreshInterval
        {
            [DebuggerStepThrough]
            get
            {
                return _refreshInterval;
            }
            [DebuggerStepThrough]
            set
            {
                _refreshInterval = value;
            }
        }
        #endregion
        #region GetMimeTypeForExtension

        /// <summary>
        /// Returns a Mime type for an extension
        /// </summary>        
        /// <param name="extension">The file extension</param>
        /// <returns>Mime type (sourced from the machine's registry)
        /// or null if not found or an error occurs.</returns>
        public static string GetMimeTypeForExtension(string extension)
        {
            if (string.IsNullOrEmpty(extension))
            {
                Trace.WriteLine("GetMimeTypeForExtension: Passed extension is null or empty!");
                return null;
            }

            // Get a reference to the mime type mappings
            StringDictionary extensionMapping;
            lock (_mimeTypeLock)
            {
                if (_extensionToMimeTypeMappings == null)
                {
                    _extensionToMimeTypeMappings = new StringDictionary();
                }
                extensionMapping = _extensionToMimeTypeMappings;
            }

            string type = null;
            if (extensionMapping.ContainsKey(extension))
            {
                type = extensionMapping[extension];                
            }
            else
            {
                try
                {
                    RegistryKey extensionKey = Registry.ClassesRoot.OpenSubKey(extension, false);
                    if (extensionKey != null)
                    {
                        object contentValue = extensionKey.GetValue("Content Type");
                        if (contentValue != null)
                        {
                            type = contentValue.ToString();
                        }
                    }
                    extensionKey.Close();
                }
                catch (Exception ex)
                {
                    Trace.WriteLine("GetMimeTypeForExtension Exception : " + ex.Message);
                    return null;
                }
                
                // Add the new type to the set of mappings.
                lock (_mimeTypeLock)
                {
                    if (!_extensionToMimeTypeMappings.ContainsKey(extension))
                    {
                        _extensionToMimeTypeMappings.Add(extension, type);
                    }
                }
            }

            return type;
        }

        /// <summary>
        /// Returns a Mime type for an extension
        /// </summary>
        /// <param name="metabasePath">The metabase path of interest</param>
        /// <param name="extension">The file extension</param>
        /// <returns></returns>
        public static string GetMimeTypeForExtension(string metabasePath, string extension)
        {
            string type = null;
            string mappingKey = extension + " " + metabasePath;            
            string currentExtension;
            string currentMimeType;
            string currentMappingKey;
            bool wasFound = false;

            StringDictionary extensionMapping = GetMimeTypeCache();
        
            if (extensionMapping.ContainsKey(mappingKey))
            {
                type = extensionMapping[mappingKey];
                wasFound = true;
            }
            else
            {
                //  metabasePath is of the form "IIS://<servername>/<path>"
                //    for example "IIS://localhost/W3SVC/1/Root" 
                //  newExtension is of the form ".<extension>", for example, ".hlp"
                //  newMimeType is of the form "<application>", for example, "application/winhlp"
                
                // Look for the extension in the metabase data
                try
                {
                    DirectoryEntry path = new DirectoryEntry(metabasePath);
                    PropertyValueCollection propValues = path.Properties["MimeMap"];

                    if (propValues.Count > 0)
                    {
                        lock (_mimeTypeLock)
                        {
                            CheckCacheExpiration();

                            foreach (object value in propValues)
                            {
                                // IISOle requires a reference to the Active DS IIS Namespace Provider in Visual Studio .NET
                                IISOle.IISMimeType mimetypeObj = (IISOle.IISMimeType)value;

                                currentExtension = mimetypeObj.Extension;
                                currentMimeType = mimetypeObj.MimeType;

                                currentMappingKey = currentExtension + " " + metabasePath;
                                if (!_extensionToMimeTypeMappings.ContainsKey(currentMappingKey))
                                {
                                    _extensionToMimeTypeMappings.Add(currentMappingKey, currentMimeType);
                                }

                                if (extension == currentExtension)
                                {
                                    type = currentMimeType;
                                    wasFound = true;
                                }
                                System.Runtime.InteropServices.Marshal.ReleaseComObject(mimetypeObj);
                                mimetypeObj = null;
                            }
                        }
                    }                
                }
                catch (Exception ex)
                {
                    if ("HRESULT 0x80005006" == ex.Message)
                        Debug.Print(" Property MimeMap does not exist at {0}", metabasePath);
                    else
                        Debug.Print("Failed in GetMimeTypeForExtension with the following exception: \n{0}", ex.Message);
                }

                // If not found in the metabase, check the registry
                if (!wasFound)
                {
                    type = GetMimeTypeForExtension(extension);

                    // Add the new type to the set of mappings.
                    mappingKey = extension + " " + metabasePath;
                    AddMimeMappingToCache(mappingKey, type);
                }
            }

            return type;
        }
        
        #endregion
        #region Private implementation

        /// <summary>
        ///  Get a reference to the mime type mappings
        /// </summary>
        /// <returns></returns>
        private static StringDictionary GetMimeTypeCache()
        {
            StringDictionary extensionMapping;

            // Get a reference to the mime type mappings
            lock (_mimeTypeLock)
            {             
                // Check whether to clear the cache
                CheckCacheExpiration();

                extensionMapping = _extensionToMimeTypeMappings;
            }

            return extensionMapping;
        }

        /// <summary>
        /// Add a mapping to the cache
        /// </summary>
        private static void AddMimeMappingToCache(string extensionKey, string mimeType)
        {
            // Add the new type to the set of mappings.
            lock (_mimeTypeLock)
            {
                // Check whether to clear the cache
                CheckCacheExpiration();
                
                // Add the value;
                if (!_extensionToMimeTypeMappings.ContainsKey(extensionKey))
                {
                    _extensionToMimeTypeMappings.Add(extensionKey, mimeType);
                }
            }
        }

        /// <summary>
        /// Check for cache expiration (must execute this in a lock of _mimeTypeLock
        /// </summary>
        private static void CheckCacheExpiration()
        {
            DateTime now = new DateTime();

            // Check whether to clear the cache
            if (now.Ticks - _mappingClearedTime.Ticks > _refreshInterval * TimeSpan.TicksPerSecond)
            {
                lock (_mimeTypeLock)
                {
                    _mappingClearedTime = now;
                    _extensionToMimeTypeMappings = new StringDictionary();
                }
            }
        }
     
        #endregion
        #region Static data

        /// <summary>
        /// A cache of metabase/extension -> MIME type mappings.
        /// </summary>
        private static StringDictionary _extensionToMimeTypeMappings = new StringDictionary();
        /// <summary>
        /// An object to lock the cache with.
        /// </summary>
        private static object _mimeTypeLock = new object();
        /// <summary>
        /// The time the cache was last cleared.
        /// </summary>
        private static DateTime _mappingClearedTime = new DateTime();
        /// <summary>
        /// The interval after which the cache should be cleared.
        /// </summary>
        private static int _refreshInterval = 300; 

        #endregion
    }
}
   