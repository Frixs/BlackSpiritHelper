using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Xml.Linq;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using System.Diagnostics;

namespace BlackSpiritHelper.Core
{
    /// <summary>
    /// Custom settings provider.
    /// </summary>
    public class CustomSettingsProvider : SettingsProvider
    {
        #region Constants

        const string NAME = "name";
        const string SERIALIZE_AS = "serializeAs";
        const string CONFIG = "configuration";
        const string USER_SETTINGS = "userSettings";
        const string SETTING = "setting";
        const string VALUE = "value";
        const string XML = "Xml";

        #endregion

        #region Structs

        /// <summary>
        /// Helper struct.
        /// </summary>
        internal struct SettingStruct
        {
            internal string name;
            internal string serializeAs;
            internal Type type;
            internal object value;
        }

        #endregion

        #region Private Members

        /// <summary>
        /// The settings file folder path.
        /// </summary>
        private string mUserConfigFolderPath
        {
            get
            {
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), ApplicationName.Split('.')[0]);
            }
        }

        /// <summary>
        /// The setting file path.
        /// </summary>
        private string mUserConfigPath
        {
            get
            {
                return Path.Combine(
                    mUserConfigFolderPath, 
                    Debugger.IsAttached ? "user.debug.config" : "user.config"
                    );
            }

        }

        /// <summary>
        /// In memory storage of the settings values.
        /// </summary>
        private Dictionary<string, SettingStruct> mSettingsDictionary { get; set; }

        /// <summary>
        /// Says if the settings file is already loaded.
        /// </summary>
        private bool mLoaded;

        #endregion

        #region Public Properties

        /// <summary>
        /// Override.
        /// </summary>
        public override string ApplicationName
        {
            get => System.Reflection.Assembly.GetExecutingAssembly().ManifestModule.Name;
            set
            {
                // Do nothing.
            }
        }

        #endregion

        #region Constructor / Initialize

        /// <summary>
        /// Loads the file into memory.
        /// </summary>
        public CustomSettingsProvider()
        {
            mSettingsDictionary = new Dictionary<string, SettingStruct>();
        }

        /// <summary>
        /// Override.
        /// </summary>
        public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config)
        {
            base.Initialize(ApplicationName, config);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Must override this, this is the bit that matches up the designer properties to the dictionary values.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="collection"></param>
        /// <returns></returns>
        public override SettingsPropertyValueCollection GetPropertyValues(SettingsContext context, SettingsPropertyCollection collection)
        {
            // Load all the values from the file.
            if (!mLoaded)
            {
                mLoaded = true;
                LoadValuesFromFile();
            }

            // Collection that will be returned.
            SettingsPropertyValueCollection values = new SettingsPropertyValueCollection();

            // Itterate thought the properties we get from the designer, checking to see if the setting is in the dictionary.
            foreach (SettingsProperty setting in collection)
            {
                SettingsPropertyValue value = new SettingsPropertyValue(setting);
                value.IsDirty = false;

                // Need the type of the value for the strong typing.
                var type = Type.GetType(setting.PropertyType.FullName);
                
                // Get value if already exists in the memory.
                if (mSettingsDictionary.ContainsKey(setting.Name))
                {
                    value.SerializedValue = mSettingsDictionary[setting.Name].value;
                    value.PropertyValue = Convert.ChangeType(mSettingsDictionary[setting.Name].value, type);
                }
                // Use defaults in the case where there are no settings yet.
                else
                {
                    value.SerializedValue = setting.DefaultValue;
                    value.PropertyValue = Convert.ChangeType(setting.DefaultValue, type);
                }

                // Add value.
                values.Add(value);
            }

            return values;
        }

        /// <summary>
        /// Must override this, this is the bit that does the saving to file. 
        /// Called when Settings.Save() is called.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="collection"></param>
        public override void SetPropertyValues(SettingsContext context, SettingsPropertyValueCollection collection)
        {
            // Grab the values from the collection parameter and update the values in our dictionary.
            foreach (SettingsPropertyValue value in collection)
            {
                if (value.SerializedValue == null)
                    continue;

                var setting = new SettingStruct()
                {
                    name = value.Name,
                    serializeAs = value.Property.SerializeAs.ToString(),
                    type = value.PropertyValue.GetType(),
                    value = value.PropertyValue == null ? string.Empty : value.PropertyValue,
                };

                // Add a new one, if does not exist.
                if (!mSettingsDictionary.ContainsKey(value.Name))
                {
                    mSettingsDictionary.Add(value.Name, setting);
                }
                // Update the existing one.
                else
                {
                    mSettingsDictionary[value.Name] = setting;
                }
            }

            // Now that our local dictionary is up-to-date, save it to disk.
            SaveValuesToFile();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Loads all the values of the file into memory.
        /// </summary>
        private void LoadValuesFromFile()
        {
            if (!File.Exists(mUserConfigPath))
                // If the config file is not where it's supposed to be, create a new one.
                CreateEmptyConfig();

            // Load the xml.
            var configXml = XDocument.Load(mUserConfigPath);

            // Get all of the <setting name="..." serializeAs="..."> elements.
            var settingElements = configXml.Element(CONFIG).Element(USER_SETTINGS).Element(typeof(Properties.Settings).FullName).Elements(SETTING);

            // Iterate through all the setting elements... check for nulls and add them to the dictionary.
            // Using "String" as default serializeAs.
            foreach (var element in settingElements)
            {
                // Add the struct into our list of loaded settings.
                mSettingsDictionary.Add(element.Attribute(NAME).Value, InitValueFromFile(element));
            }
        }

        /// <summary>
        /// Init a value from the file.
        /// </summary>
        /// <param name="element">Element where the value is stored.</param>
        /// <returns></returns>
        private SettingStruct InitValueFromFile(XElement element)
        {
            // Name.
            string name = element.Attribute(NAME) == null ? string.Empty : element.Attribute(NAME).Value;

            // SerializeAs.
            string serializeAs = element.Attribute(SERIALIZE_AS) == null ? nameof(String) : element.Attribute(SERIALIZE_AS).Value;

            // Value's type.
            Type type = serializeAs.Equals(XML) ? Type.GetType(GetType().Namespace + "." + element.Element(VALUE).Descendants().First().Name.ToString()) : typeof(string);

            // Value.
            object value;
            // Deserialize Xml.
            if (serializeAs.Equals(XML))
            {
                XmlSerializer xs = new XmlSerializer(type);
                value = xs.DeserializeAsObject(element.Element(VALUE).Descendants().First().ToString());
            }
            // Rest is string.
            else
            {
                value = element.Element(VALUE).Value ?? string.Empty;
            }

            // Create a struct with these properties.
            return new SettingStruct()
            {
                name = name,
                serializeAs = serializeAs,
                type = type,
                value = value,
            };
        }

        /// <summary>
        /// Saves the in memory dictionary to the user config file.
        /// </summary>
        private void SaveValuesToFile()
        {
            // Load the current xml from the file.
            var import = XDocument.Load(mUserConfigPath);

            // Get the settings group (e.g. <Company.Project.Desktop.Settings>).
            var settingsSection = import.Element(CONFIG).Element(USER_SETTINGS).Element(typeof(Properties.Settings).FullName);

            // Iterate though the dictionary, either updating a value or adding a new setting.
            foreach (var entry in mSettingsDictionary)
            {
                var setting = settingsSection.Elements().FirstOrDefault(e => e.Attribute(NAME).Value == entry.Key);
                // This can happen if a new setting is added via the .settings designer.
                if (setting == null)
                {
                    // Setting element.
                    var newSetting = new XElement(SETTING);
                    newSetting.Add(new XAttribute(NAME, entry.Value.name));
                    newSetting.Add(new XAttribute(SERIALIZE_AS, entry.Value.serializeAs));

                    // Value child element.
                    var newSettingValue = new XElement(VALUE);
                    // Serialize Xml.
                    if (entry.Value.serializeAs.Equals(XML))
                    {
                        XmlSerializer xs = new XmlSerializer(entry.Value.type);
                        newSettingValue.Add(xs.SerializeAsXElement(entry.Value.value));
                    }
                    // Rest is string.
                    else
                    {
                        newSettingValue.Value = (entry.Value.value ?? string.Empty).ToString();
                    }

                    // Add value child to the setting element.
                    newSetting.Add(newSettingValue);

                    // Add the setting element into the setting section.
                    settingsSection.Add(newSetting);
                }
                // Update the value if it exists.
                else
                {
                    // Serialize Xml.
                    if (entry.Value.serializeAs.Equals(XML))
                    {
                        // Remove the old value, first.
                        setting.Element(VALUE).Descendants().Remove();
                        // Add a new value.
                        XmlSerializer xs = new XmlSerializer(entry.Value.type);
                        setting.Element(VALUE).Add(xs.SerializeAsXElement(entry.Value.value));
                    }
                    // Rest is string.
                    else
                    {
                        setting.Element(VALUE).Value = (entry.Value.value ?? string.Empty).ToString();
                    }
                }
            }

            // Save to the file.
            import.Save(mUserConfigPath);
        }

        /// <summary>
        /// Creates an empty user.config file.
        /// </summary>
        private void CreateEmptyConfig()
        {
            var doc = new XDocument();
            var declaration = new XDeclaration("1.0", "utf-8", "true");
            var config = new XElement(CONFIG);
            var userSettings = new XElement(USER_SETTINGS);
            var group = new XElement(typeof(Properties.Settings).FullName);
            userSettings.Add(group);
            config.Add(userSettings);
            doc.Add(config);
            doc.Declaration = declaration;
            Directory.CreateDirectory(mUserConfigFolderPath);
            doc.Save(mUserConfigPath);
        }

        #endregion
    }

    /// <summary>
    /// Helper class.
    /// </summary>
    static class XmlSerializerExtension
    {
        /// <summary>
        /// Serialize into XElement.
        /// </summary>
        /// <param name="xs"></param>
        /// <param name="o"></param>
        /// <returns></returns>
        public static XElement SerializeAsXElement(this XmlSerializer xs, object o)
        {
            XDocument d = new XDocument();
            using (XmlWriter w = d.CreateWriter()) xs.Serialize(w, o);
            XElement e = d.Root;
            e.Remove();
            return e;
        }

        /// <summary>
        /// Deserialize into object.
        /// </summary>
        /// <param name="xs"></param>
        /// <param name="str"></param>
        /// <returns></returns>
        public static object DeserializeAsObject(this XmlSerializer xs, string str)
        {
            XDocument d = XDocument.Parse(str);
            object o = null;
            using (XmlReader w = d.CreateReader()) o = xs.Deserialize(w);
            return o;
        }
    }
}
