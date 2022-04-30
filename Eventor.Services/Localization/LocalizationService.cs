using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Xml;
using Line.Data;

namespace Line.Services
{
    /// <summary>
    /// Provides information about localization
    /// </summary>
    public partial class LocalizationService : ILocalizationService
    {
       
        
       // private readonly ILogger _logger;
        private readonly ILanguageService _languageService;
        private readonly DBEntities _Repository;
        private readonly IWorkContext _WorkContext;


        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        public LocalizationService( ILanguageService languageService, IWorkContext workcontext)
        {
           
            this._languageService = languageService;
            _Repository = new DBEntities();
            _WorkContext = workcontext;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Deletes a locale string resource
        /// </summary>
        /// <param name="localeStringResource">Locale string resource</param>
        public virtual void DeleteLocaleStringResource(LocaleStringResource localeStringResource)
        {
            if (localeStringResource == null)
                throw new ArgumentNullException("localeStringResource");

            _Repository.EntityDeleted(localeStringResource);

            //cache

        }

        /// <summary>
        /// Gets a locale string resource
        /// </summary>
        /// <param name="localeStringResourceId">Locale string resource identifier</param>
        /// <returns>Locale string resource</returns>
        public virtual LocaleStringResource GetLocaleStringResourceById(int localeStringResourceId)
        {
            if (localeStringResourceId == 0)
                return null;

            return _Repository.LocaleStringResources.Find(localeStringResourceId);
        }

        /// <summary>
        /// Gets a locale string resource
        /// </summary>
        /// <param name="resourceName">A string representing a resource name</param>
        /// <returns>Locale string resource</returns>
        public virtual LocaleStringResource GetLocaleStringResourceByName(string resourceName)
        {
            if (_WorkContext.WorkingLanguage != null)
                return GetLocaleStringResourceByName(resourceName, _WorkContext.WorkingLanguage.Id);

            return null;
        }

        /// <summary>
        /// Gets a locale string resource
        /// </summary>
        /// <param name="resourceName">A string representing a resource name</param>
        /// <param name="languageId">Language identifier</param>
        /// <param name="logIfNotFound">A value indicating whether to log error if locale string resource is not found</param>
        /// <returns>Locale string resource</returns>
        public virtual LocaleStringResource GetLocaleStringResourceByName(string resourceName, int languageId,
            bool WriterIfNotFound = false)
        {
            //var query = from lsr in _Repository.LocaleStringResources.Table
            //            orderby lsr.ResourceName
            //            where lsr.LanguageId == languageId && lsr.ResourceName == resourceName
            //            select lsr;
            var localeStringResource = _Repository.LocaleStringResources.FirstOrDefault(d => d.LanguageId == languageId && d.ResourceName == resourceName);

            if (localeStringResource == null && WriterIfNotFound)
            {
                InsertLocaleStringResource(new LocaleStringResource { LanguageId = languageId, ResourceName = resourceName, ResourceValue = "" });
                return _Repository.LocaleStringResources.FirstOrDefault(d => d.LanguageId == languageId && d.ResourceName == resourceName);
            }
            //_Repository.LocaleStringResources.Add(new LocaleStringResource)
            //_logger.Warning(string.Format("Resource string ({0}) not found. Language ID = {1}", resourceName, languageId));

            return localeStringResource;
        }

        /// <summary>
        /// Gets all locale string resources by language identifier
        /// </summary>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Locale string resources</returns>
        public virtual IList<LocaleStringResource> GetAllResources(int languageId)
        {

            var locales = _Repository.LocaleStringResources.AsNoTracking().Where(d => d.LanguageId == languageId).ToList();

            return locales;
        }

        /// <summary>
        /// Inserts a locale string resource
        /// </summary>
        /// <param name="localeStringResource">Locale string resource</param>
        public virtual void InsertLocaleStringResource(LocaleStringResource localeStringResource)
        {
            if (localeStringResource == null)
                throw new ArgumentNullException("localeStringResource");

            var dbitem = _Repository.LocaleStringResources.Add(localeStringResource);
            _Repository.EntityAdded(dbitem);


        }

        /// <summary>
        /// Updates the locale string resource
        /// </summary>
        /// <param name="localeStringResource">Locale string resource</param>
        public virtual void UpdateLocaleStringResource(LocaleStringResource item)
        {
            if (item == null)
                throw new ArgumentNullException("localeStringResource");

            var dbitem = _Repository.LocaleStringResources.Find(item.Id);
             dbitem = item;
            //update language
            _Repository.EntityEdited(dbitem);
        }

        /// <summary>
        /// Gets all locale string resources by language identifier
        /// </summary>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Locale string resources</returns>
        public virtual Dictionary<string, KeyValuePair<int,string>> GetAllResourceValues(int languageId)
        {
           
                //we use no tracking here for performance optimization
                //anyway records are loaded only for read-only operations
       
                var locales = GetAllResources(languageId);
                //format: <name, <id, value>>
                var dictionary = new Dictionary<string, KeyValuePair<int, string>>();
                foreach (var locale in locales)
                {
                    var resourceName = locale.ResourceName.ToLowerInvariant();
                    if (!dictionary.ContainsKey(resourceName))
                        dictionary.Add(resourceName, new KeyValuePair<int, string>(locale.Id, locale.ResourceValue));
                }
                return dictionary;
            
        }

        /// <summary>
        /// Gets a resource string based on the specified ResourceKey property.
        /// </summary>
        /// <param name="resourceKey">A string representing a ResourceKey.</param>
        /// <returns>A string representing the requested resource string.</returns>
        public virtual string GetResource(string resourceKey)
        {
            if (_WorkContext.WorkingLanguage != null)
              return GetResource(resourceKey, _WorkContext.WorkingLanguage.Id);
            
            return "";
        }
        
        /// <summary>
        /// Gets a resource string based on the specified ResourceKey property.
        /// </summary>
        /// <param name="resourceKey">A string representing a ResourceKey.</param>
        /// <param name="languageId">Language identifier</param>
        /// <param name="logIfNotFound">A value indicating whether to log error if locale string resource is not found</param>
        /// <param name="defaultValue">Default value</param>
        /// <param name="returnEmptyIfNotFound">A value indicating whether an empty string will be returned if a resource is not found and default value is set to empty string</param>
        /// <returns>A string representing the requested resource string.</returns>
        public virtual string GetResource(string resourceKey, int languageId,
            bool logIfNotFound = true, string defaultValue = "", bool returnEmptyIfNotFound = false)
        {
            string result = string.Empty;
            if (resourceKey == null)
                resourceKey = string.Empty;
            resourceKey = resourceKey.Trim().ToLowerInvariant();
            /*if (_localizationSettings.LoadAllLocaleRecordsOnStartup)*/
            //{
            //    //load all records (we know they are cached)
            //    var resources = GetAllResourceValues(languageId);
            //    if (resources.ContainsKey(resourceKey))
            //    {
            //        result = resources[resourceKey].Value;
            //    }
            //}
            //else
            {
                try
                {
                    var x = _Repository.LocaleStringResources.FirstOrDefault(d => d.ResourceName == resourceKey && d.LanguageId == languageId);

                    if (x == null)
                    {
                        InsertLocaleStringResource(new LocaleStringResource { LanguageId = languageId,ResourceName = resourceKey, ResourceValue = resourceKey.Replace("."," ") });
                    }
                    return x != null ? x.ResourceValue : "";
                }
                catch (Exception)
                {

                    throw;
                }
               
                
                ////gradual loading
                //string key = string.Format(LOCALSTRINGRESOURCES_BY_RESOURCENAME_KEY, languageId, resourceKey);
                //string lsr = _cacheManager.Get(key, () =>
                //{
                //    var query = from l in _lsrRepository.Table
                //                where l.ResourceName == resourceKey
                //                && l.LanguageId == languageId
                //                select l.ResourceValue;
                //    return query.FirstOrDefault();
                //});

                //if (lsr != null) 
                //    result = lsr;
            }
            //if (String.IsNullOrEmpty(result))
            //{
            //    //if (logIfNotFound)
            //    //    _logger.Warning(string.Format("Resource string ({0}) is not found. Language ID = {1}", resourceKey, languageId));
                
            //    if (!String.IsNullOrEmpty(defaultValue))
            //    {
            //        result = defaultValue;
            //    }
            //    else
            //    {
            //        if (!returnEmptyIfNotFound)
            //            result = resourceKey;
            //    }
            //}
            //return result;
        }

        /// <summary>
        /// Export language resources to xml
        /// </summary>
        /// <param name="language">Language</param>
        /// <returns>Result in XML format</returns>
        public virtual string ExportResourcesToXml(Language language)
        {
            if (language == null)
                throw new ArgumentNullException("language");
            var sb = new StringBuilder();
            var stringWriter = new StringWriter(sb);
            var xmlWriter = new XmlTextWriter(stringWriter);
            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement("Language");
            xmlWriter.WriteAttributeString("Name", language.Name);
           // xmlWriter.WriteAttributeString("SupportedVersion", NopVersion.CurrentVersion);


            var resources = GetAllResources(language.Id);
            foreach (var resource in resources)
            {
                xmlWriter.WriteStartElement("LocaleResource");
                xmlWriter.WriteAttributeString("Name", resource.ResourceName);
                xmlWriter.WriteElementString("Value", null, resource.ResourceValue);
                xmlWriter.WriteEndElement();
            }

            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndDocument();
            xmlWriter.Close();
            return stringWriter.ToString();
        }

        /// <summary>
        /// Import language resources from XML file
        /// </summary>
        /// <param name="language">Language</param>
        /// <param name="xml">XML</param>
        /// <param name="updateExistingResources">A value indicating whether to update existing resources</param>
        public virtual void ImportResourcesFromXml(Language language, string xml, bool updateExistingResources = true)
        {
            if (language == null)
                throw new ArgumentNullException("language");

            if (String.IsNullOrEmpty(xml))
                return;
           
            else
            {
                //stored procedures aren't supported
                var xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xml);

                var nodes = xmlDoc.SelectNodes(@"//Language/LocaleResource");
                foreach (XmlNode node in nodes)
                {
                    string name = node.Attributes["Name"].InnerText.Trim();
                    string value = "";
                    var valueNode = node.SelectSingleNode("Value");
                    if (valueNode != null)
                        value = valueNode.InnerText;

                    if (String.IsNullOrEmpty(name))
                        continue;

                    //do not use "Insert"/"Update" methods because they clear cache
                    //let's bulk insert
                    var resource = language.LocaleStringResources.FirstOrDefault(x => x.ResourceName.Equals(name, StringComparison.InvariantCultureIgnoreCase));
                    if (resource != null)
                    {
                        if (updateExistingResources)
                        {
                            var r = _Repository.LocaleStringResources.Find(resource.Id);
                            r.ResourceValue = value;
                            UpdateLocaleStringResource(r);
                        }
                    }
                    else
                    {
                        var lang =
                            new LocaleStringResource
                            {
                                LanguageId = language.Id,
                                ResourceName = name,
                                ResourceValue = value
                            };

                        InsertLocaleStringResource(lang);
                    }
                }
                _languageService.UpdateLanguage(language);
            }

            //clear cache
           // _cacheManager.RemoveByPattern(LOCALSTRINGRESOURCES_PATTERN_KEY);
        }

        public void CreateWebsiteDefaulsForEnglish(int languageID)
        {
            
            InsertLocaleStringResource(new LocaleStringResource {
                ResourceName = @"event.create",
                ResourceValue = @"Create New Event", LanguageId = languageID });

            InsertLocaleStringResource(new LocaleStringResource
            {
                ResourceName = @"event.name.hint",
                ResourceValue = @"You can change the event's name later.",
                LanguageId = languageID
            });
            InsertLocaleStringResource(new LocaleStringResource
            {
                ResourceName = @"event.website",
                ResourceValue = @"Event Website",
                LanguageId = languageID
            });
            InsertLocaleStringResource(new LocaleStringResource
            {
                ResourceName = @"event.website.hint",
                ResourceValue = @"Your event Website will be availabe under this domain. Domain name may be formed from the set of alphanumeric ASCII characters (a-z, 0-9). In addition the hyphen is permitted if it is surrounded by characters.",
                LanguageId = languageID
            });
        }

        public void CreateWebsiteDefaulsForEnglish(Language language)
        {
            throw new NotImplementedException();
        }

        #endregion

        public List<string> GetCountryList(string country)
        {
            //var Countries = new List<SelectListItem>();
            List<string> CountryList = new List<string>();
            CultureInfo[] CInfoList = CultureInfo.GetCultures(CultureTypes.SpecificCultures);
            foreach (CultureInfo CInfo in CInfoList)
            {
                RegionInfo R = new RegionInfo(CInfo.LCID);
                if (!(CountryList.Contains(R.EnglishName)))
                {
                    CountryList.Add(R.EnglishName);
                }
            }

            CountryList.Sort();

            //foreach (var item in CountryList)
            //{
            //    var selected = item == country ? true : false;
            //    Countries.Add(new SelectListItem { Selected = selected, Text = item, Value = item });
            //}
            return CountryList;


        }
    }
}
