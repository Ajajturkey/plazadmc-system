using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Line.Services;
using System.Globalization;
using System.Threading;
using Line.Data;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Core.Lifetime;
using AutoMapper;
using Line.Models;

namespace Line
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private ILanguageService _LanguageService;
        private IWorkContext _WorkContext;


        public bool Systemintilized()
        {
            var languages = _LanguageService.GetAllLanguages();
            if (languages.Count == 0)
            {
                _LanguageService.InsertLanguage(new Language
                {
                    Name = "English",
                    FlagImageFileName = "",
                    LanguageCulture = "tr-tr",
                    Published = true,
                    Rtl = false,
                    UniqueSeoCode = "en",
                });
            }
            
            return true;
        }

        protected void Application_Start()
        {

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            DependencyManager.Confiqre();


            
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            try
            {
                if (HttpContext.Current != null)
                    DependencyManager.Scope = AutofacDependencyResolver.Current.RequestLifetimeScope;

                //when such lifetime scope is returned, you should be sure that it'll be disposed once used (e.g. in schedule tasks)
                DependencyManager.Scope.BeginLifetimeScope(MatchingScopeLifetimeTags.RequestLifetimeScopeTag);
            }
            catch (Exception)
            {
                //we can get an exception here if RequestLifetimeScope is already disposed
                //for example, requested in or after "Application_EndRequest" handler
                //but note that usually it should never happen

                //when such lifetime scope is returned, you should be sure that it'll be disposed once used (e.g. in schedule tasks)
                DependencyManager.Scope.BeginLifetimeScope(MatchingScopeLifetimeTags.RequestLifetimeScopeTag);
            }


            _LanguageService = new LanguageService(); //DependencyManager.Scope.Resolve<ILanguageService>();
            _WorkContext = DependencyManager.Scope.Resolve<IWorkContext>();
     
            //we don't do it in Application_BeginRequest because a user is not authenticated yet
            SetWorkingCulture();
        }

        protected void SetWorkingCulture()
        {
            if (Systemintilized())
            {
                //public store
                
                var culture = new CultureInfo(_WorkContext.WorkingLanguage.LanguageCulture);
                Thread.CurrentThread.CurrentCulture = culture;
                Thread.CurrentThread.CurrentUICulture = culture;
            }
        }
    }
}
