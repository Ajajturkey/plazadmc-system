using Autofac;
using Autofac.Integration.Mvc;
using AutoMapper;
using Line.Controllers;
using Line.Data;
using Line.Models;
using Line.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Line
{
    public static class DependencyManager
    {
        public static IContainer _Container;
        public static ILifetimeScope Scope;
        public static MapperConfiguration AMapper;

        

        public static void Confiqre() {

            var builder = new ContainerBuilder();

            //HTTP context and other related stuff
            builder.Register(c =>
                //register FakeHttpContext when HttpContext is not available
                HttpContext.Current != null ?
                (new HttpContextWrapper(HttpContext.Current) as HttpContextBase) :
                //(new FakeHttpContext("~/") as HttpContextBase)
                null)
                .As<HttpContextBase>()
                .InstancePerLifetimeScope();
            builder.Register(c => c.Resolve<HttpContextBase>().Request)
                .As<HttpRequestBase>()
                .InstancePerLifetimeScope();
            builder.Register(c => c.Resolve<HttpContextBase>().Response)
                .As<HttpResponseBase>()
                .InstancePerLifetimeScope();
            builder.Register(c => c.Resolve<HttpContextBase>().Server)
                .As<HttpServerUtilityBase>()
                .InstancePerLifetimeScope();
            builder.Register(c => c.Resolve<HttpContextBase>().Session)
                .As<HttpSessionStateBase>()
                .InstancePerLifetimeScope();


            // Register types that expose interfaces...
            builder.RegisterType<LanguageService>().As<ILanguageService>().InstancePerDependency();
            builder.RegisterType<LocalizationService>().As<ILocalizationService>().InstancePerDependency();
            builder.RegisterType<FormsAuthenticationService>().As<IAuthenticationService>().InstancePerDependency();
            builder.RegisterType<VisitorService>().As<IVisitorService>();
            builder.RegisterType<UploadService>().As<IUploadService>();
            builder.RegisterType<SendEmailService>().As<ISendEmailService>();
            builder.RegisterType<ConfiqurationService>().As<IConfiqurationService>();
            builder.RegisterType<ReservationService>().As<IReservationService>();
            builder.RegisterType<ReportsService>().As<IReportsService>();
            builder.RegisterType<AccountingService>().As<IAccountingService>();



            builder.RegisterType<WebWorkContext>().As<IWorkContext>();

            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            _Container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(_Container));
            Scope = _Container.BeginLifetimeScope();
        }
    }
}