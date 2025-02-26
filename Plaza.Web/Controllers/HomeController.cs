using Newtonsoft.Json;
using Line.Data;
using Line.Models;
using Line.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Mvc;

namespace Line.Controllers
{
    public class HomeController : Controller
    {
        public ILanguageService _LanguageService;
        public ILocalizationService _LocalizationService;
        public IWorkContext _workContext;
        public IConfiqurationService _configurationService;
        public IVisitorService _VisitorService;
        public IAuthenticationService _AuthenticationService;
        public IUploadService _UploadService;
        public ISendEmailService _SendEmailService;


        public HomeController(ILanguageService languageService, ILocalizationService localizationService,
         IWorkContext workContext, IConfiqurationService configurationService, IVisitorService VisitorService, 
         IUploadService uploadService, IAuthenticationService authenticationService,
         ISendEmailService SendEmailService
        )
        {
            this._LanguageService = languageService;
            this._LocalizationService = localizationService;
            this._workContext = workContext;
            this._configurationService = configurationService;
            this._VisitorService = VisitorService;
            this._AuthenticationService = authenticationService;
            _UploadService = uploadService;
            _SendEmailService = SendEmailService;
        }

        public ActionResult Setup()
        {
            var Members = _VisitorService.GetAllVisitors(false);

            if (Members.Count == 0)
            {
                // Create Admin //TODO
            }

            foreach (var item in Members)
            {
                if (item.VisitorGuid == null)
                {
                     item.VisitorGuid = Guid.NewGuid();
                     item.FirstName = item.name;
                    _VisitorService.UpdateVisitor(item);
                }
               
               
                // Update 
                if (item.IsGuest())
                {
                    continue;
                }

                if (!item.IsInCustomerRole(SystemVisitorTypeNames.Members))
                {
                    _VisitorService.InsertVisitorTypes(new VisitorTypes
                    {
                        Name = SystemVisitorTypeNames.Members.ToString(),
                        VisitorId = item.Id
                    });
                }

                if (item.accountManager && !item.IsInCustomerRole(SystemVisitorTypeNames.Accounting))
                {
                    _VisitorService.InsertVisitorTypes(new VisitorTypes
                    {
                        Name = SystemVisitorTypeNames.Accounting.ToString(),
                        VisitorId = item.Id
                    });
                }
                if (item.reservations && !item.IsInCustomerRole(SystemVisitorTypeNames.Reservations))
                {
                    _VisitorService.InsertVisitorTypes(new VisitorTypes
                    {
                        Name = SystemVisitorTypeNames.Reservations.ToString(),
                        VisitorId = item.Id
                    });
                    _VisitorService.InsertVisitorTypes(new VisitorTypes
                    {
                        Name = SystemVisitorTypeNames.Operations.ToString(),
                        VisitorId = item.Id
                    });
                }
                if (item.reservationManager && !item.IsInCustomerRole(SystemVisitorTypeNames.Managing))
                {
                    _VisitorService.InsertVisitorTypes(new VisitorTypes
                    {
                        Name = SystemVisitorTypeNames.Managing.ToString(),
                        VisitorId = item.Id
                    });
                }
                if (item.manager && !item.IsInCustomerRole(SystemVisitorTypeNames.Administrators))
                {
                    _VisitorService.InsertVisitorTypes(new VisitorTypes
                    {
                        Name = SystemVisitorTypeNames.Administrators.ToString(),
                        VisitorId = item.Id
                    });
                }

             
            }


            if (_configurationService.GetSupplierById(1) == null)
            {
                var supplier = new Supplier
                {
                    Active = true,
                    Country = "Turkey",
                    City = "Istanbul",
                    type = "-",
                    contactPerson = "Admin",
                    Email = "info@email.com",
                    fax = "",
                    tel = "",
                    name = "Default",
                    JoinDate = DateTime.Now,
                };
                _configurationService.InsertSupplier(supplier);
            }
            

            return RedirectToAction("Index");
            }

            public ActionResult Index()
        {


            return RedirectToAction("Dashboard", "reservations", new { @Area = "admin" });
            //var model = new HomePageModel();
            //model.Webslider = _configurationService.SearchWebSlider(UploadLocation.MainSlider.ToString());
            //model.Commentslider = _configurationService.SearchWebSlider(UploadLocation.CustomerReview.ToString());
            //return View(model);
        }
        public ActionResult Pricing()
        {
            return View();
        }
        public ActionResult ContactUs()
        {
            return View("contact-us");
        }
        [HttpPost]
        public ActionResult ContactUs(FormCollection form)
        {
            var Name = form["your-name"] + form["last-name"] + form["your-phone"];
            var Email = form["your-email"];
            var Subject = form["your-subject"];
            var Message = form["your-message"];

            var EmailSetting = EmailSettings.PrepareEmailAccount("CompaneyMatcher.com | ContactUs");
            var body = EmailSettings.PrepareContactUsMessage(Subject , Message, Name);
            _SendEmailService.SendEmail(EmailSetting, Email, body, "info@companeymatcher.com", "Contact US", "info@globalmark.com.tr", "info@globalmark.com.tr");

            return View("contact-us");
        }


        public ActionResult AboutUs()
        {
            return View("about-us");
        }

        public ActionResult FAQ()
        {
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }
        [System.Web.Mvc.HttpPost]
        public ActionResult Register(LoginModel model)
        {
            if(ModelState.IsValid)
            {

                if (_VisitorService.GetVisitorByEmail(model.Username) == null)
                {
                    var visitor =  _VisitorService.InsertGuestVisitor();
               
                    visitor.email = model.Username;
                    visitor.username = model.Username;
                    visitor.FirstName = model.FirstName;
                    visitor.LastName = model.LastName;
                    visitor.password = model.password;
                    visitor.Country = "Turkey";
                    visitor.lastActivityFromUtc = DateTime.UtcNow;
                    visitor.IPaddress = HttpContext.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null ? HttpContext.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] : HttpContext.Request.ServerVariables["REMOTE_ADDR"];
                    visitor.languageId = _workContext.WorkingLanguage.Id;

                    visitor.active = true;

                    Guid rtoken =  Guid.NewGuid();
                    visitor.RecoveryToken = rtoken.ToString();
                    visitor.RecoveryDate = DateTime.Now.AddDays(1);

                    _VisitorService.InsertVisitor(visitor);

                    _VisitorService.InsertVisitorTypes(new VisitorTypes { Name = SystemVisitorTypeNames.Members, VisitorId = visitor.Id });

                    _AuthenticationService.SignIn(visitor, true);

                    _workContext.CurrentVisitor = visitor;

                   

                    var EmailSetting = EmailSettings.PrepareEmailAccount("CompaneyMatcher.com | Registoration");

                    var body = EmailSettings.PrepareRegistrationMessage("Reigistration Completed","You can update you profile and start join events","Check New Event",Request.Url.Host +Url.Action("login",new {area = "", token = rtoken }));

                    try
                    {
                        _SendEmailService.SendEmail(EmailSetting, "Registoration Complete at CompaneyMatcher.com", body, "info@companeymatcher.com", "Registration", visitor.email, visitor.email);
                    }
                    catch (Exception ex)
                    {
                        _VisitorService.Log("SendEmail","Faild to send complete email " +ex.Message);
                    }
                }
                else
                {
                    ViewBag.isRegisterd = true;
                    return View("login",model);

                }

                return RedirectToAction("profile", "Participants", new { area = "Profile" });
            }
            return View(model);
        }
        public ActionResult Login()
        {
            LoginModel model = new LoginModel();
            return View(model);
        }

        public ActionResult Reset()
        {
            LoginModel model = new LoginModel();
            return View(model);
        }

        public ActionResult Password(int Id, string token)
        {
            var visitor = _VisitorService.GetVisitorById(Id);
            if (visitor != null && token == visitor.RecoveryToken)
            {
                if (DateTime.UtcNow.Date > visitor.RecoveryDate.Value.AddDays(3).Date)
                {
                    ViewBag.Error = "Link Expired";
                }
            }
            else
            {
                ViewBag.Error = "Link is not working";
            }
            PasswordRecoverModel model = new PasswordRecoverModel { Id= visitor.Id,token = visitor.RecoveryToken};
            return View(model);
        }
        [HttpPost]
        public ActionResult Password(PasswordRecoverModel reset)
        {
            if (reset.Password != reset.ConfirmPassword)
            {
                ViewBag.Error = "Password not match";
                return View(reset);
            }
            var visitor = _VisitorService.GetVisitorById(reset.Id);
            if (visitor != null && reset.token == visitor.RecoveryToken)
            {
                if (DateTime.UtcNow.Date > visitor.RecoveryDate.Value.AddDays(3).Date)
                {
                    ViewBag.Error = "Link Expired";
                    return View(reset);
                }
                else
                {
                   
                    visitor.password = reset.Password;
                    visitor.RecoveryToken = Guid.NewGuid().ToString();
                    visitor.active = true;
                    _VisitorService.UpdateVisitor(visitor);
                }
            }
            else
            {
                ViewBag.Error = "Link is not working";
                return View(reset);
            }
            return RedirectToAction("login");
        }
        [HttpPost]
        public ActionResult Reset(LoginModel item)
        {
            var email = _VisitorService.GetVisitorByEmail(item.Username);
            if (email != null)
            {
                email.RecoveryToken = Guid.NewGuid().ToString();
                
                email.RecoveryDate = DateTime.UtcNow.Date;
                _VisitorService.UpdateVisitor(email);

                var EmailSetting = EmailSettings.PrepareEmailAccount("CompanyMatcher.com | Reset Password");

                var body = EmailSettings.PrepareRegistrationMessage("Password Reset", "Click on the following link to reset your passowrd, this link will be valid for 3 days", "Reset Now", Request.Url.Host + Url.Action("Password", new { area = "", token = email.RecoveryToken , Id = email.Id}));
                try
                {
                    _SendEmailService.SendEmail(EmailSetting, "Password Reset | CompaneyMatcher.com", body, "info@companeymatcher.com", "Reset Password", email.email, email.email);

                }
                catch (Exception ex)
                {
                    _VisitorService.Log("SendEmail", "Faild to send password reset " + ex.Message);
                }

            }
            else
            {
                ViewBag.Error = "There is no such email in our database";
            }
            return View(item);
        }
        [System.Web.Mvc.HttpPost]
        public ActionResult Login(LoginModel model, string redirecturl = "")
        {
            if (ModelState.IsValid)
            {
                var visitor = _VisitorService.GetVisitorByEmail(model.Username);

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


                if (visitor != null)
                {

                    if (!visitor.IsInCustomerRole(SystemVisitorTypeNames.Members))
                    {
                        ViewBag.Error = "There is no Member access found, are you an Administor";
                        return View(); 
                    }
                    if (!visitor.active)
                    {
                        ViewBag.Error = "The User is not yet activated, check your email";
                        return View();
                    }
                    _workContext.CurrentVisitor = visitor;

                    _AuthenticationService.SignIn(visitor, true);

                   
                    return RedirectToAction("Events", "home", new { area = "" });
                    
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
    }
}
