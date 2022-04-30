using System;
using System.Collections.Generic;
using System.Linq;
using Line.Data;
using System.Data;

namespace Line.Services
{
    /// <summary>
    /// Language service
    /// </summary>
    public partial class LanguageService : ILanguageService
    {
       

        private readonly DBEntities _Repository;

      

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="cacheManager">Cache manager</param>
        /// <param name="languageRepository">Language repository</param>
        /// <param name="storeMappingService">Store mapping service</param>
        /// <param name="settingService">Setting service</param>
        /// <param name="localizationSettings">Localization settings</param>
        /// <param name="eventPublisher">Event published</param>
        public LanguageService()
        {
            this._Repository  = new DBEntities();
         
        }

        #endregion
        
        #region Methods

        /// <summary>
        /// Deletes a language
        /// </summary>
        /// <param name="language">Language</param>
        public virtual void DeleteLanguage(Language language)
        {
            if (language == null)
                throw new ArgumentNullException("language");

            var collection = language.LocaleStringResources.ToList();
            foreach (var item in collection)
            {
                _Repository.EntityDeleted(item);

            }

            _Repository.Entry(language).State = System.Data.Entity.EntityState.Deleted;
            var dbitem =_Repository.Languages.Remove(language);
            _Repository.EntityDeleted(dbitem);



        }



        /// <summary>
        /// Gets all languages
        /// </summary>
        /// <param name="storeId">Load records allowed only in a specified store; pass 0 to load all records</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Languages</returns>
        public virtual List<Language> GetAllLanguages(bool showHidden = false)
        {
            if (showHidden)
            {
                return _Repository.Languages.AsNoTracking().ToList();
            }
            else
            {
                try
                {
                    return _Repository.Languages.AsNoTracking().ToList();
                }
                catch (Exception ex)
                {
                    return _Repository.Languages.AsNoTracking().ToList();
                }
            }
          
        }

        

        /// <summary>
        /// Gets a language
        /// </summary>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Language</returns>
        public virtual Language GetLanguageById(int languageId)
        {
            if (languageId == 0)
                return null;

           var languages = _Repository.Languages.FirstOrDefault(d => d.Published && d.Id == languageId);
            return languages;
        }

        /// <summary>
        /// Inserts a language
        /// </summary>
        /// <param name="language">Language</param>
        public virtual void InsertLanguage(Language language)
        {
            if (language == null)
                throw new ArgumentNullException("language");

            var dbitem =_Repository.Languages.Add(language);
            _Repository.EntityAdded(dbitem);


        }

        
        /// <summary>
        /// Updates a language
        /// </summary>
        /// <param name="language">Language</param>
        public virtual void UpdateLanguage(Language language)
        {
            if (language == null)
                throw new ArgumentNullException("language");

            var dbitem = _Repository.Languages.Find(language.Id);
            _Repository.Entry(dbitem).CurrentValues.SetValues(language);
            _Repository.EntityEdited(dbitem);
            //return dbitem;

            //var dblang = _Repository.Languages.AsNoTracking().FirstOrDefault(d=> d.Id ==language.Id);
            //dblang = language;
            ////update language
            //_Repository.EntityEdited(dblang);
            //cache

        }

       

        #endregion
    }
}
