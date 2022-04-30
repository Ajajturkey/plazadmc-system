using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Line.Data;
using Line.Services;
using System.Collections;
using System.Collections.Specialized;
using System.Security.Principal;
using System.Web;
using Line.Models;

namespace Line.Services
{
    public partial class WebWorkContext : IWorkContext
    {
        #region Const

        private const string VisitorCookieName = ".visitor";

        private readonly HttpContextBase _httpContext;
        private readonly IVisitorService _visitorService;
        private readonly ILanguageService _languageService;
        //private readonly ICurrencyService _currencyService;
        //private readonly CurrencySettings _currencySettings;
        //private readonly LocalizationSettings _localizationSettings;
        //private readonly IUserAgentHelper _userAgentHelper;


        private Members _cachedVisitor;// { get; set; }
        private Language _cachedLanguage;
        private int _cachedEvent;
        private IAuthenticationService _authenticationService;

        #endregion
        public WebWorkContext(IVisitorService VisitorService, ILanguageService languageService, 
            IAuthenticationService authenticationService, HttpContextBase httpContext)
        {
            _httpContext = httpContext;
            _visitorService = new VisitorService();
            _languageService = new LanguageService();
            _authenticationService = new FormsAuthenticationService(_httpContext, _visitorService);
          
        }

        protected virtual HttpCookie GetVisitorCookie()
        {
            if (_httpContext == null || _httpContext.Request == null)
                return null;

            return _httpContext.Request.Cookies[VisitorCookieName];
        }

        protected virtual void SetVisitorCookie(Guid VisitorGuid)
        {
            if (_httpContext != null && _httpContext.Response != null)
            {
                var cookie = new HttpCookie(VisitorCookieName);
                cookie.HttpOnly = true;
                cookie.Value = VisitorGuid.ToString();
                if (VisitorGuid == Guid.Empty)
                {
                    cookie.Expires = DateTime.Now.AddMonths(-1);
                }
                else
                {
                    int cookieExpires = 24 * 365; //TODO make configurable
                    cookie.Expires = DateTime.Now.AddHours(cookieExpires);
                }

                _httpContext.Response.Cookies.Remove(VisitorCookieName);
                _httpContext.Response.Cookies.Add(cookie);
            }
        }

        protected virtual Language GetLanguageFromBrowserSettings()
        {
            if (_httpContext == null ||
                _httpContext.Request == null ||
                _httpContext.Request.UserLanguages == null)
                return null;

            var userLanguage = _httpContext.Request.UserLanguages.FirstOrDefault();
            if (String.IsNullOrEmpty(userLanguage))
                return null;

            var language = _languageService
                .GetAllLanguages()
                .FirstOrDefault(l => userLanguage.Equals(l.LanguageCulture, StringComparison.InvariantCultureIgnoreCase));
            if (language != null &&  language.Published )
            {
                return language;
            }

            return null;
        }

        public virtual Members CurrentVisitor
        {
            get
            {
                if (_cachedVisitor != null)
                    return _cachedVisitor;

                Members Visitor = null;

                //if (_httpContext == null || _httpContext is FakeHttpContext)
                //{
                //    //check whether request is made by a background task
                //    //in this case return built-in Visitor record for background task
                //    Visitor = _visitorService.GetVisitorBySystemName(SystemVisitorNames.BackgroundTask);
                //}

                //check whether request is made by a search engine
                //in this case return built-in Visitor record for search engines 
                //or comment the following two lines of code in order to disable this functionality
                //if (Visitor == null || Visitor.Deleted || !Visitor.Active)
                //{
                //    if (_userAgentHelper.IsSearchEngine())
                //        Visitor = _VisitorService.GetVisitorBySystemName(SystemVisitorNames.SearchEngine);
                //}

                //registered user
                if (Visitor == null || Visitor.Deleted || !Visitor.active)
                {
                    Visitor = _authenticationService.GetAuthenticatedCustomer();
                }


                //load guest Visitor
                if (Visitor == null || Visitor.Deleted || !Visitor.active)
                {
                    var VisitorCookie = GetVisitorCookie();
                    if (VisitorCookie != null && !String.IsNullOrEmpty(VisitorCookie.Value))
                    {
                        Guid VisitorGuid;
                        if (Guid.TryParse(VisitorCookie.Value, out VisitorGuid))
                        {
                            var VisitorByCookie = _visitorService.GetVisitorByGuid(VisitorGuid);
                            if (VisitorByCookie != null &&
                                //this Visitor (from cookie) should not be registered
                                !VisitorByCookie.IsRegistered())
                                Visitor = VisitorByCookie;
                        }
                    }
                }

                //create guest if not exists
                if (Visitor == null || Visitor.Deleted || !Visitor.active)
                {
                    Visitor = _visitorService.InsertGuestVisitor();
                }


                //validation
                if (!Visitor.Deleted && Visitor.active)
                {
                    SetVisitorCookie(Visitor.VisitorGuid.Value);
                    _cachedVisitor = Visitor;
                }

                return _cachedVisitor;
            }
            set
            {
                SetVisitorCookie(value.VisitorGuid.Value);
                _cachedVisitor = value;
            }
        }


        public virtual Language WorkingLanguage
        {
            get
            {
                if (_cachedLanguage != null)
                    return _cachedLanguage;

                Language detectedLanguage = null;
                //if (_localizationSettings.SeoFriendlyUrlsForLanguagesEnabled)
                //{
                //    //get language from URL
                //    detectedLanguage = GetLanguageFromUrl();
                //}
                //if (detectedLanguage == null /*&& _localizationSettings.AutomaticallyDetectLanguage*/)
                //{
                    //get language from browser settings
                    //but we do it only once
                    if (this.CurrentVisitor.languageId == 0)
                    {
                        detectedLanguage = GetLanguageFromBrowserSettings();
                        
                    }
                //}

                if (detectedLanguage != null)
                {
                    //the language is detected. now we need to save it
                    //if (this.CurrentCustomer.GetAttribute<int>(SystemCustomerAttributeNames.LanguageId,
                    //    _genericAttributeService, _storeContext.CurrentStore.Id) != detectedLanguage.Id)
                    //{
                    //    _genericAttributeService.SaveAttribute(this.CurrentCustomer, SystemCustomerAttributeNames.LanguageId,
                    //        detectedLanguage.Id, _storeContext.CurrentStore.Id);
                    //}
                    this.CurrentVisitor.languageId = detectedLanguage.Id;
                    //_visitorService.UpdateVisitor(CurrentVisitor);
                }

                var allLanguages = _languageService.GetAllLanguages();
                //find current customer language
                var languageId = this.CurrentVisitor.languageId;
                var language = allLanguages.FirstOrDefault(x => x.Id == languageId);
                //if (language == null)
                //{
                //    //it not found, then let's load the default currency for the current language (if specified)
                //    languageId = _storeContext.CurrentStore.DefaultLanguageId;
                //    language = allLanguages.FirstOrDefault(x => x.Id == languageId);
                //}
                if (language == null)
                {
                    //it not specified, then return the first (filtered by current store) found one
                    language = allLanguages.FirstOrDefault();
                }
                if (language == null)
                {
                    //it not specified, then return the first found one
                    language = _languageService.GetAllLanguages().FirstOrDefault();
                }

                //cache
                _cachedLanguage = language;
                return _cachedLanguage;
            }
            set
            {
                var languageId = value != null ? value.Id : 0;
                CurrentVisitor.languageId = languageId;
                _visitorService.UpdateVisitor(CurrentVisitor);
                //reset cache
                _cachedLanguage = null;
            }
        }

        public virtual bool IsAdmin { get {
                return CurrentVisitor.IsAdmin();
            } }

        public virtual bool IsMemeber
        {
            get
            {
                return CurrentVisitor.IsInCustomerRole(SystemVisitorTypeNames.Members, true);
            }
        }

        public bool IsReservation
        {
            get
            {
                return CurrentVisitor.IsInCustomerRole(SystemVisitorTypeNames.Reservations, true);
            }
        }
        

        public bool IsOperation
        {
            get
            {
                return CurrentVisitor.IsInCustomerRole(SystemVisitorTypeNames.Operations, true);
            }
        }

        public bool IsAccounts
        {
            get
            {
                return CurrentVisitor.IsInCustomerRole(SystemVisitorTypeNames.Accounting, true);
            }
        }

        public bool IsManagment
        {
            get
            {
                return CurrentVisitor.IsInCustomerRole(SystemVisitorTypeNames.Managing, true);
            }
        }
    }
}
