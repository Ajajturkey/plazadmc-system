using Line.Data;
using Line.Models;
using Line.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Line.Services.Seo;

namespace Line.Controllers
{
    public class AccountController : Controller
    {
        public ILanguageService _LanguageService;
        public ILocalizationService _LocalizationService;
        public IWorkContext _workContext;
        public IConfiqurationService _configurationService;
        public IUploadService _uploadService;
        public IVisitorService _VisitorService;
        public IAuthenticationService _AuthenticationService;


        public AccountController(ILanguageService languageService, ILocalizationService localizationService,
             IUploadService uploadService,
             IWorkContext workContext, IAuthenticationService authenticationService, IConfiqurationService configurationService, IVisitorService VisitorService 
            )
        {
            this._LanguageService = languageService;
            this._LocalizationService = localizationService;
            this._workContext = workContext;
            this._configurationService = configurationService;
            this._VisitorService = VisitorService;
            this._uploadService = uploadService;
            this._AuthenticationService = authenticationService;
        }


        // GET: Account
        public ActionResult login()
        {
            LoginModel modle = new LoginModel();

            var setting = _configurationService.GetSettings();
            ViewBag.header = _uploadService.GetPicture(setting.Logo);

            return View(modle);
        }
        [HttpPost]
        public ActionResult login(LoginModel model,string redirecturl = "")
        {
            var setting = _configurationService.GetSettings();
            ViewBag.header = _uploadService.GetPicture(setting.Logo);

            if (ModelState.IsValid)
            {
                _VisitorService.loginLog(new LoginLog
                {
                    LoginDate = DateTime.Now,
                    Username = model.Username,
                    Password = model.password,
                    IPAddress = Request.UserHostAddress,
                });

                var visitor = _VisitorService.GetVisitorByUsername(model.Username);

                if (visitor == null)
                {
                    ViewBag.Error = "User not exsist";
                    return View();
                }


                if (visitor.password != model.password)
                {
                    ViewBag.Error = "Wrong username or password";
                    return View();
                }

                //TODO
                //if (!visitor.active)
                //{
                //    ViewBag.Error = "User Not Active, Contact Admin to activate your panel";
                //    return View();
                //}

                if (visitor != null)
                {
                    _workContext.CurrentVisitor = visitor;

                    _AuthenticationService.SignIn(visitor, true);

                    if (_workContext.IsAdmin || _workContext.IsMemeber)
                    {
                        return RedirectToAction("Dashboard", "Reservations", new { area = "admin" });
                    }
                   

                    ViewBag.Error = "you are not authorized here";


                }
                else
                {
                    ViewBag.Error = "User not exsist";

                }

            }
            return View();
        }

        public ActionResult logout()
        {
            _workContext.CurrentVisitor = _VisitorService.InsertGuestVisitor();
            _AuthenticationService.SignOut();
            return RedirectToAction("login");
        }

        public ActionResult Register()
        {
            var visitor = _AuthenticationService.GetAuthenticatedCustomer();
            if (visitor!= null)
            {
                _AuthenticationService.SignOut();
            }
          
            RegisterModel model = new RegisterModel();
            return View();
        }
        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {

                if (_VisitorService.GetVisitorByEmail(model.Email) == null)
                {
                    var visitor = _VisitorService.InsertGuestVisitor();
                    visitor.FirstName = model.FirstName;
                    visitor.LastName = model.LastName;
                    visitor.email = model.Email;
                    visitor.username = model.Email;
                    visitor.password = model.Password;
                    visitor.lastActivityFromUtc = DateTime.UtcNow;
                    visitor.IPaddress = HttpContext.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null ? HttpContext.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] : HttpContext.Request.ServerVariables["REMOTE_ADDR"];
                    visitor.languageId = _workContext.WorkingLanguage.Id;
                   
                    visitor.active = true;

                    _VisitorService.InsertVisitor(visitor);


                    _VisitorService.InsertVisitorTypes(new VisitorTypes { Name = SystemVisitorTypeNames.Administrators, VisitorId = visitor.Id });
                    _VisitorService.InsertVisitorTypes(new VisitorTypes { Name = SystemVisitorTypeNames.Members, VisitorId = visitor.Id });

                    _AuthenticationService.SignIn(visitor, true);

                    _workContext.CurrentVisitor = visitor;
                }
                else
                {
                    ViewBag.isRegisterd = true;
                    return View(model);

                }

                return RedirectToAction("Users", "Confiquration", new {area = "Admin" });
            }
            return View(model);
        }

     

        
    }
}