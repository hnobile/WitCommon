///////////////////////////////////////////////////////////////////////
//               Settings for the HandlersAndModules                 //
//            Written by: Miron Abramson. Date: 04-10-07             //
//         See the attached readme.txt file for instructions         //
//                    Last updated: 05-05-2008                       //
///////////////////////////////////////////////////////////////////////

#region Using
using System;
using System.Collections.Specialized;
using System.Text;
using System.Configuration;
using System.Xml;
using System.Xml.XPath;
#endregion

namespace Miron.Web.MbCompression
{
    internal sealed class Settings
    {
        #region // Constants
        // Name of the settings section in the web.config file
        private const string CONFIGURATION_SECTION_NAME = "Miron.web/CompressorSettings";

        // Configuration attributes
        private const string CONFIGURATION_EXCLUDE_TYPES_NODE_NAME = "excludeTypes";
        private const string CONFIGURATION_EXCLUDE_PATHS_NODE_NAME = "excludePaths";

        private const string CONFIGURATION_DAYS_IN_CACHE_ATTRIBUTE = "daysInCahe";
        private const string CONFIGURATION_COMPRESS_CSS_ATTRIBUTE = "compressCSS";
        private const string CONFIGURATION_COMPRESS_JS_ATTRIBUTE = "compressJavaScript";
        private const string CONFIGURATION_COMPRESS_PAGE_ATTRIBUTE = "compressPage";
        private const string CONFIGURATION_OPTIMIZE_HTML_ATTRIBUTE = "optimizeHtml";
        private const string CONFIGURATION_COMPRESS_WEBRESOURCE_ATTRIBUTE = "compressWebResource";
        private const string CONFIGURATION_REFLECTION_ALLOWEDED_ATTRIBUTE = "reflectionAlloweded";
        private const string CONFIGURATION_PARSE_THIRD_PARTY_SCRIPTS = "compressThirdParityScripts";

        #endregion


        #region // Private members
        private StringDictionary _excludeTypes;
        private StringDictionary _excludePathes;
        private static Settings settings;
        private int _daysInCache = 30;
        private bool _compressCSS = true;
        private bool _compressJavaScript = true;
        private bool _compressPage = true;
        private bool _compressWebResource = true;
        private bool _reflectionAlloweded = true;
        private bool _optimizeHtml;
        private bool _compressThirdPartyScripts;
        #endregion


        #region // Properties

        internal int DaysInCache
        {
            get { return _daysInCache; }
        }
        internal bool CompressCSS
        {
            get { return _compressCSS; }
        }
        internal bool CompressJavaScript
        {
            get { return _compressJavaScript; }
        }
        internal bool CompressThirdPartyJavaScript
        {
            get { return _compressThirdPartyScripts; }
        }
        internal bool CompressPage
        {
            get { return _compressPage; }
        }
        internal bool CompressWebResource
        {
            get { return _compressWebResource; }
        }
        internal bool ReflectionAlloweded
        {
            get { return _reflectionAlloweded; }
        }
        internal bool OptimizeHtml
        {
            get { return _optimizeHtml; }
        }
        #endregion


        #region // Constractors

        /// <summary>
        /// Private constructor
        /// </summary>
        private Settings()
        {
            Initialize();
        }


        #endregion


        // Create singleton object of the settings
        internal static Settings Instance
        {
            get
            {
                if (settings == null)
                {
                    settings = new Settings();
                }
                return settings;
            }
        }


        #region // Initialize

        private void Initialize()
        {
            _excludeTypes = new StringDictionary();
            _excludePathes = new StringDictionary();

            // Load the configuration settings from the web.config
            SettingsConfigSection configSection = (SettingsConfigSection)ConfigurationManager.GetSection(CONFIGURATION_SECTION_NAME);

            // No configuration was setting
            if (configSection == null)
            {
                return;
            }

            // Load the extra properties for the compressos
            LoadCompressorProperties(configSection.Config);

            // Add the exluded mime types from the configSection
            LoadExcludedTypes(configSection.Config.SelectSingleNode("excludeTypes"));

            // Add the exluded paths from the configSection
            LoadExcludedPaths(configSection.Config.SelectSingleNode("excludePaths"));
        }


        // Add the excluded mime types from the configSection
        private void LoadExcludedTypes(XPathNavigator node)
        {
            if (node == null)
                return;

            XPathNodeIterator childrens = node.SelectChildren(XPathNodeType.All);
            foreach (XPathNavigator child in childrens)
            {
                if (!string.IsNullOrEmpty(child.GetAttribute("key", string.Empty)))
                    _excludeTypes.Add(child.GetAttribute("key", string.Empty), null);
            }
        }

        // Add the excluded paths from the configSection
        private void LoadExcludedPaths(XPathNavigator node)
        {
            if (node == null)
                return;

            XPathNodeIterator childrens = node.SelectChildren(XPathNodeType.All);
            foreach (XPathNavigator child in childrens)
            {
                if (!string.IsNullOrEmpty(child.GetAttribute("key", string.Empty)))
                    _excludePathes.Add(child.GetAttribute("key", string.Empty), null);
            }
        }

        // Load the extra properties for the compressos
        private void LoadCompressorProperties(XPathNavigator node)
        {
            if (node == null)
                return;

            bool tmpBool;

            if (bool.TryParse(node.GetAttribute(CONFIGURATION_COMPRESS_CSS_ATTRIBUTE, string.Empty), out tmpBool))
                _compressCSS = tmpBool;

            if (bool.TryParse(node.GetAttribute(CONFIGURATION_COMPRESS_JS_ATTRIBUTE, string.Empty), out tmpBool))
                _compressJavaScript = tmpBool;

            if (bool.TryParse(node.GetAttribute(CONFIGURATION_COMPRESS_PAGE_ATTRIBUTE, string.Empty), out tmpBool))
                _compressPage = tmpBool;

            if (bool.TryParse(node.GetAttribute(CONFIGURATION_COMPRESS_WEBRESOURCE_ATTRIBUTE, string.Empty), out tmpBool))
                _compressWebResource = tmpBool;

            if (bool.TryParse(node.GetAttribute(CONFIGURATION_REFLECTION_ALLOWEDED_ATTRIBUTE, string.Empty), out tmpBool))
                _reflectionAlloweded = tmpBool;

            if (bool.TryParse(node.GetAttribute(CONFIGURATION_OPTIMIZE_HTML_ATTRIBUTE, string.Empty), out tmpBool))
                _optimizeHtml = tmpBool;

            if (bool.TryParse(node.GetAttribute(CONFIGURATION_PARSE_THIRD_PARTY_SCRIPTS, string.Empty), out tmpBool))
                _compressThirdPartyScripts = tmpBool;

            int tmpInt;
            if (int.TryParse(node.GetAttribute(CONFIGURATION_DAYS_IN_CACHE_ATTRIBUTE, string.Empty), out tmpInt))
                _daysInCache = tmpInt;
            if (_daysInCache < 0)
                _daysInCache = 0;
        }

        #endregion


        // Check if the gives type is valid to be compressed
        internal bool IsValidType(string type)
        {
            return !_excludeTypes.ContainsKey(type);
        }


        // Check if the gives path is valid to be compressed
        internal bool IsValidPath(string path)
        {
            return !_excludePathes.ContainsKey(path);
        }
    }

    #region // Class to hold the configuration section from the web.config
    public class SettingsConfigSection : IConfigurationSectionHandler
    {
        private XPathNavigator _config;

        public XPathNavigator Config
        {
            get { return _config; }
        }

        protected SettingsConfigSection() { }

        public object Create(object parent, object configContext, XmlNode section)
        {
            if (section == null)
            {
                return null;
            }
            _config = section.CreateNavigator();
            return this;
        }
    }
    #endregion
}