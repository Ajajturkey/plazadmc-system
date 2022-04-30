using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Line.Models;
using Line.Data;
using Line.Services;
using Autofac;
using Autofac.Integration.Mvc;
using Line.Helpers;
using Newtonsoft.Json;
using System.Text;
using System.Globalization;
using PagedList;

namespace Line.Controllers
{

    public class ConfigurationController : Controller
    {

        public ILanguageService _LanguageService;
        public ILocalizationService _LocalizationService;
        public IWorkContext _workContext;
        public IConfiqurationService _configurationService;
        public IUploadService _uploadService;
        public IVisitorService _VisitorService;
        private readonly int PageSize = 50;


        public ConfigurationController(ILanguageService languageService, ILocalizationService localizationService,
             IUploadService uploadService,
             IWorkContext workContext, IConfiqurationService configurationService, IVisitorService VisitorService)
        {
            this._LanguageService = languageService;
            this._LocalizationService = localizationService;
            this._workContext = workContext;
            this._configurationService = configurationService;
            this._VisitorService = VisitorService;
            this._uploadService = uploadService;
        }

        public ActionResult Settings()
        {
            var model = _configurationService.GetSettings();
            return View(model.ToModel());
        }
        [HttpPost]
        public ActionResult Settings(SettingViewModel model)
        {
            var dbitem = model.ToDBObject();
            var e = _configurationService.SetSettings(dbitem);
           
            return View(e.ToModel());
        }

        public ActionResult Agencies(string search = "",int page = 1)
        {
            var model = _configurationService.SearchAgencies(search);
            ViewBag.Search = search;
            if (Request.HttpMethod == "POST")
            {
                page = 1;
            }
            return View(model.ToPagedList(page, PageSize));
        }

        public ActionResult Agency(int Id = 0)
        {
            ViewBag.Countries = _configurationService.GetCountries("");
            ViewBag.Types = new SelectList (Enum.GetNames(typeof(CreditType)));

            if (Id  >  0)
            {
                var dbitem = _configurationService.GetAgencyById(Id);
                if (dbitem == null)
                {
                    return PageNotFound();
                }
                return View(dbitem);
            }

            return View(new Agency ());
        }
        [HttpPost]
        public ActionResult Agency(Agency model)
        {
            ViewBag.Countries = _configurationService.GetCountries("");
            ViewBag.Types = new SelectList(Enum.GetNames(typeof(CreditType)));

            if (model.ID > 0)
            {
                model.Latitude = "";
                model.Longitude = "";
                var dbitem = _configurationService.UpdateAgency(model);
                if (dbitem == null)
                {
                    ViewBag.error = "Not.Added";
                    return View(model);
                }
                return Agency(dbitem.ID);
            }

            
            model.joindate = DateTime.Today.ToShortDateString();
            model.Latitude = "";
            model.Longitude = "";
            var dbiitem = _configurationService.InsertAgency(model);
            if (dbiitem == null)
            {
                ViewBag.error = "Not.Added";
                return View(model);
            }
            return Agency(dbiitem.ID);
        }

        public ActionResult Hotels(string search = "",int page = 1)
        {
            var model = _configurationService.SearchHotels(search);
            ViewBag.Search = search;
            if (Request.HttpMethod == "POST")
            {
                page = 1;
            }
            return View(model.ToPagedList(page, PageSize));
        }

        public ActionResult hotel(int Id = 0)
        {
            ViewBag.Countries = _configurationService.GetCountries("");
            ViewBag.Cities = new SelectList(Enum.GetNames(typeof(TurkeyCities)), "Istanbul");

            if (Id > 0)
            {
                var dbitem = _configurationService.GetHotelById(Id);
                if (dbitem == null)
                {
                    return PageNotFound();
                }
                return View(dbitem);
            }

            return View(new DataHotel());
        }
        [HttpPost]
        public ActionResult hotel(DataHotel model)
        {
            ViewBag.Countries = _configurationService.GetCountries("");
            ViewBag.Cities = new SelectList(Enum.GetNames(typeof(TurkeyCities)), "Istanbul");

            if (model.Id > 0)
            {
                var dbitem = _configurationService.UpdateHotel(model);
                if (dbitem == null)
                {
                    ViewBag.error = "Not.Added";
                    return View(model);
                }
                return View(dbitem);
            }


            var dbiitem = _configurationService.InsertHotel(model);
            if (dbiitem == null)
            {
                ViewBag.error = "Not.Added";
                return View(model);
            }
            return hotel(dbiitem.Id);
        }

        public ActionResult OnlineRooms()
        {
            var model = _configurationService.SearchOnlineRooms(0);
            return View(model);
        }

        public ActionResult OnlineRoom(int Id = 0)
        {
            ViewBag.Countries = _configurationService.GetCountries("");
            ViewBag.Types = new SelectList(Enum.GetNames(typeof(CreditType)));

            if (Id > 0)
            {
                var dbitem = _configurationService.GetOnlineRoomById(Id);
                if (dbitem == null)
                {
                    return PageNotFound();
                }
                return View(dbitem);
            }

            return View(new DataHotel());
        }
        [HttpPost]
        public ActionResult OnlineRoom(OnlineRooms model)
        {
            ViewBag.Countries = _configurationService.GetCountries("");
            ViewBag.Types = new SelectList(Enum.GetNames(typeof(CreditType)));

            if (model.Id > 0)
            {
                var dbitem = _configurationService.UpdateOnlineRoom(model);
                if (dbitem == null)
                {
                    ViewBag.error = "Not.Added";
                    return View(model);
                }
                return Agency(dbitem.Id);
            }


            var dbiitem = _configurationService.InsertOnlineRoom(model);
            if (dbiitem == null)
            {
                ViewBag.error = "Not.Added";
                return View(model);
            }
            return hotel(dbiitem.Id);
        }

        public ActionResult OfflineRooms()
        {
            var model = _configurationService.SearchOfflineRooms(0);
            return View(model);
        }

        public ActionResult OfflineRoom(int Id = 0)
        {
            ViewBag.Countries = _configurationService.GetCountries("");
            ViewBag.Types = new SelectList(Enum.GetNames(typeof(CreditType)));

            if (Id > 0)
            {
                var dbitem = _configurationService.GetOfflineRoomById(Id);
                if (dbitem == null)
                {
                    return PageNotFound();
                }
                return View(dbitem);
            }

            return View(new DataHotel());
        }
        [HttpPost]
        public ActionResult OfflineRoom(OfflineRooms model)
        {
            ViewBag.Countries = _configurationService.GetCountries("");
            ViewBag.Types = new SelectList(Enum.GetNames(typeof(CreditType)));

            if (model.Id > 0)
            {
                var dbitem = _configurationService.UpdateOfflineRoom(model);
                if (dbitem == null)
                {
                    ViewBag.error = "Not.Added";
                    return View(model);
                }
                return Agency(dbitem.Id);
            }


            var dbiitem = _configurationService.InsertOfflineRoom(model);
            if (dbiitem == null)
            {
                ViewBag.error = "Not.Added";
                return View(model);
            }
            return hotel(dbiitem.Id);
        }


        public ActionResult Tours(int page = 1)
        {
            var model = _configurationService.SearchTours(0);
            return View(model.ToPagedList(page , PageSize));
        }

        public ActionResult Tour(int Id = 0)
        {
            ViewBag.Countries = _configurationService.GetCountries("");
            ViewBag.Cities = new SelectList(Enum.GetNames(typeof(TurkeyCities)), "Istanbul");
            if (Id > 0)
            {
                var dbitem = _configurationService.GetTourById(Id);
                if (dbitem == null)
                {
                    return PageNotFound();
                }
                return View(dbitem);
            }

            return View(new DataTour());
        }
        [HttpPost]
        public ActionResult Tour(DataTour model)
        {
            ViewBag.Countries = _configurationService.GetCountries("");
            ViewBag.Cities = new SelectList(Enum.GetNames(typeof(TurkeyCities)), "Istanbul");

            if (model.Id > 0)
            {
                var dbitem = _configurationService.UpdateTour(model);
                if (dbitem == null)
                {
                    ViewBag.error = "Not.Added";
                    return View(model);
                }
                return View(dbitem);
            }


            var dbiitem = _configurationService.InsertTour(model);
            if (dbiitem == null)
            {
                ViewBag.error = "Not.Added";
                return View(model);
            }
            return View(dbiitem);
        }


        public ActionResult Packages(int page = 1)
        {
            var model = _configurationService.SearchPackages(0);
            return View(model.ToPagedList(page,PageSize));
        }

        public ActionResult Package(int Id = 0)
        {
            ViewBag.Countries = _configurationService.GetCountries("");
            ViewBag.Cities = new SelectList(Enum.GetNames(typeof(TurkeyCities)), "Istanbul");
            if (Id > 0)
            {
                var dbitem = _configurationService.GetPackageById(Id);
                if (dbitem == null)
                {
                    return PageNotFound();
                }
                return View(dbitem);
            }

            return View(new DataPackage());
        }
        [HttpPost]
        public ActionResult Package(DataPackage model)
        {
            ViewBag.Countries = _configurationService.GetCountries("");
            ViewBag.Cities = new SelectList(Enum.GetNames(typeof(TurkeyCities)), "Istanbul");

            if (model.Id > 0)
            {
                var dbitem = _configurationService.UpdatePackage(model);
                if (dbitem == null)
                {
                    ViewBag.error = "Not.Added";
                    return View(model);
                }
                return View(dbitem);
            }


            var dbiitem = _configurationService.InsertPackage(model);
            if (dbiitem == null)
            {
                ViewBag.error = "Not.Added";
                return View(model);
            }
            return View(dbiitem);
        }


        public ActionResult Suppliers(int page = 1)
        {
            var model = _configurationService.SearchSupplier(0);
            return View(model.ToPagedList(page, PageSize));
        }

        public ActionResult Supplier(int Id = 0)
        {
            ViewBag.Countries = _configurationService.GetCountries("");
            ViewBag.Types = new SelectList(Enum.GetNames(typeof(CreditType)));

            if (Id > 0)
            {
                var dbitem = _configurationService.GetSupplierById(Id);
                if (dbitem == null)
                {
                    return PageNotFound();
                }
                return View(dbitem);
            }

            return View(new Supplier());
        }
        [HttpPost]
        public ActionResult Supplier(Supplier model)
        {
            
            if (string.IsNullOrEmpty(model.mobile))
            {
                model.mobile = "";
            }
            if (string.IsNullOrEmpty(model.fax))
            {
                model.fax = "";
            }
            if (string.IsNullOrEmpty(model.tel))
            {
                model.tel = "";
            }
            if (string.IsNullOrEmpty(model.contactPerson))
            {
                model.contactPerson = "";
            }
            if (string.IsNullOrEmpty(model.type))
            {
                model.type = "";
            }

            ViewBag.Countries = _configurationService.GetCountries("");
            ViewBag.Types = new SelectList(Enum.GetNames(typeof(CreditType)));

            if (model.Id > 0)
            {
                var dbitem = _configurationService.UpdateSupplier(model);
                if (dbitem == null)
                {
                    ViewBag.error = "Not.Added";
                    return View(model);
                }
                return Supplier(dbitem.Id);
            }

            model.JoinDate = DateTime.Today;
            
            var dbiitem = _configurationService.InsertSupplier(model);
            if (dbiitem == null)
            {
                ViewBag.error = "Not.Added";
                return View(model);
            }
            return RedirectToAction("Supplier", new { Id = dbiitem.Id }); 
        }


        public ActionResult Marketers()
        {
            var model = _configurationService.SearchMarketer(0);
            return View(model);
        }

        public ActionResult Marketer(int Id = 0)
        {
            ViewBag.Countries = _configurationService.GetCountries("");
            ViewBag.Types = new SelectList(Enum.GetNames(typeof(CreditType)));

            if (Id > 0)
            {
                var dbitem = _configurationService.GetMarketerById(Id);
                if (dbitem == null)
                {
                    return PageNotFound();
                }
                return View(dbitem);
            }

            return View(new DataTour());
        }
        [HttpPost]
        public ActionResult Marketer(Marketman model)
        {
            ViewBag.Countries = _configurationService.GetCountries("");
            ViewBag.Types = new SelectList(Enum.GetNames(typeof(CreditType)));

            if (model.Id > 0)
            {
                var dbitem = _configurationService.UpdateMarketer(model);
                if (dbitem == null)
                {
                    ViewBag.error = "Not.Added";
                    return View(model);
                }
                return Agency(dbitem.Id);
            }


            var dbiitem = _configurationService.InsertMarketer(model);
            if (dbiitem == null)
            {
                ViewBag.error = "Not.Added";
                return View(model);
            }
            return hotel(dbiitem.Id);
        }


        private ActionResult PageNotFound()
        {
            throw new NotImplementedException();
        }

       


        public ActionResult SocialMedia() {
            return View();
        }
        [HttpPost]

        public ActionResult SocialMedia(string facebook, string Twitter, string instagram, string youtube)
        {
            foreach (var item in _LanguageService.GetAllLanguages())
            {
                var localfacebook = _LocalizationService.GetLocaleStringResourceByName("facebook",item.Id,true);
                var localTwitter = _LocalizationService.GetLocaleStringResourceByName("Twitter", item.Id, true);
                var localinstagram = _LocalizationService.GetLocaleStringResourceByName("instagram", item.Id, true);
                var localyouTube = _LocalizationService.GetLocaleStringResourceByName("youtube", item.Id, true);

                localfacebook.ResourceValue = facebook;
                localTwitter.ResourceValue = Twitter;
                localinstagram.ResourceValue = instagram;
                localyouTube.ResourceValue = youtube;

                _LocalizationService.UpdateLocaleStringResource(localfacebook);
                _LocalizationService.UpdateLocaleStringResource(localTwitter);
                _LocalizationService.UpdateLocaleStringResource(localinstagram);
                _LocalizationService.UpdateLocaleStringResource(localyouTube);
            }
            return View();
        }

        public ActionResult ExportResourcesToXml(int id)
        {
            //if (_workContext.IsAdmin)
            {
                var language = _LanguageService.GetLanguageById(id);
                var xmlFile = _LocalizationService.ExportResourcesToXml(language);
                return File(Encoding.UTF8.GetBytes(xmlFile),
                 "application/xml",
                  string.Format("{0}.xml", language.Name));
            }
            //return View();
        }

        [HttpPost]
        public ActionResult ImportResourcesToXml(HttpPostedFileBase file, int id)
        {
            if (file != null && file.ContentLength > 0)
            {
                string xml = (new System.IO.StreamReader(file.InputStream)).ReadToEnd();
                var language = _LanguageService.GetLanguageById(id);
                _LocalizationService.ImportResourcesFromXml(language, xml);

            }
            return RedirectToAction("Languages");
        }

        public ActionResult CreateOrUpdateUser(int id = 0)
        {
            if (id == 0)
            {
                return View(new VisitorModel());
            }
            else
            {
                var visitor = _VisitorService.GetVisitorById(id);

                var visitorModel = new VisitorModel
                {
                    Active = visitor.active,
                    Deleted = visitor.Deleted,
                    FirstName = visitor.FirstName,
                    LastName = visitor.LastName,
                    Email = visitor.email,
                    Phone = visitor.tel,
                    Password = visitor.password,
                    Id = visitor.Id,
                    EmailPassword = visitor.emailPassword,
                    Title = visitor.title,
                    username = visitor.username,
                    IsAccounting = false,
                    IsManager = false,
                    IsOperation = false,
                    IsReservation = false,
                    IsAdmin = false
                };
                if (visitor.IsAdmin())
                {
                    visitorModel.IsAdmin = true;
                }
                //TODO Roles
                if (visitor.IsInCustomerRole(SystemVisitorTypeNames.Reservations))
                {
                    visitorModel.IsReservation = true;
                }
                if (visitor.IsInCustomerRole(SystemVisitorTypeNames.Managing))
                {
                    visitorModel.IsManager = true;
                }
                if (visitor.IsInCustomerRole(SystemVisitorTypeNames.Accounting))
                {
                    visitorModel.IsAccounting = true;
                }
                if (visitor.IsInCustomerRole(SystemVisitorTypeNames.Operations))
                {
                    visitorModel.IsOperation = true;
                }
                if (visitor.IsInCustomerRole(SystemVisitorTypeNames.Administrators))
                {
                    visitorModel.IsAdmin = true;
                }
                return View(visitorModel);
            }

        }
        [HttpPost]
        public ActionResult CreateUser(VisitorModel model)
        {

            if (model.Id > 0)
            {
                var visitor = _VisitorService.GetVisitorById(model.Id);

                if (Request.Form["updatepassword"] != null)
                {
                    visitor.password = model.Password;
                    _VisitorService.UpdateVisitor(visitor);
                    return RedirectToAction("Users");
                }

                if (Request.Form["UpdateEpass"] != null)
                {
                    visitor.emailPassword = model.EmailPassword;
                    _VisitorService.UpdateVisitor(visitor);
                    return RedirectToAction("Users");
                }

                visitor.active = model.Active;
                visitor.FirstName = model.FirstName;
                visitor.LastName = model.LastName;
                visitor.email = model.Email;
                visitor.tel = model.Phone;
                visitor.title = model.Title;

                if (visitor.VisitorGuid == null)
                {
                    visitor.VisitorGuid = Guid.NewGuid();
                }

                if (model.IsAdmin && !visitor.IsAdmin())
                {
                    _VisitorService.InsertVisitorTypes(new VisitorTypes { Name = SystemVisitorTypeNames.Administrators, VisitorId = visitor.Id });
                }
                else
                {
                    if (!model.IsAdmin && visitor.IsAdmin())
                    {
                        _VisitorService.DeleteVisitorTypes(visitor.VisitorTypes.FirstOrDefault(d => d.Name == SystemVisitorTypeNames.Administrators));
                    }
                }

                if (visitor.VisitorTypes.FirstOrDefault(d => d.Name == SystemVisitorTypeNames.Reservations) == null && model.IsReservation)
                {
                    _VisitorService.InsertVisitorTypes(new VisitorTypes { Name = SystemVisitorTypeNames.Reservations, VisitorId = visitor.Id });
                }
                else
                {
                    if (visitor.VisitorTypes.FirstOrDefault(d => d.Name == SystemVisitorTypeNames.Reservations) != null && !model.IsReservation)
                    {
                        _VisitorService.DeleteVisitorTypes(visitor.VisitorTypes.FirstOrDefault(d => d.Name == SystemVisitorTypeNames.Reservations));
                    }
                }
                if (visitor.VisitorTypes.FirstOrDefault(d => d.Name == SystemVisitorTypeNames.Operations) == null && model.IsOperation)
                {
                    _VisitorService.InsertVisitorTypes(new VisitorTypes { Name = SystemVisitorTypeNames.Operations, VisitorId = visitor.Id });
                }
                else
                {
                    if (visitor.VisitorTypes.FirstOrDefault(d => d.Name == SystemVisitorTypeNames.Operations) != null && !model.IsOperation)
                    {
                        _VisitorService.DeleteVisitorTypes(visitor.VisitorTypes.FirstOrDefault(d => d.Name == SystemVisitorTypeNames.Operations));
                    }
                }


                if (visitor.VisitorTypes.FirstOrDefault(d => d.Name == SystemVisitorTypeNames.Managing) == null && model.IsManager)
                {
                    _VisitorService.InsertVisitorTypes(new VisitorTypes { Name = SystemVisitorTypeNames.Managing, VisitorId = visitor.Id });
                }
                else
                {
                    if (visitor.VisitorTypes.FirstOrDefault(d => d.Name == SystemVisitorTypeNames.Managing) != null && !model.IsManager)
                    {
                        _VisitorService.DeleteVisitorTypes(visitor.VisitorTypes.FirstOrDefault(d => d.Name == SystemVisitorTypeNames.Managing));
                    }
                }

                if (visitor.VisitorTypes.FirstOrDefault(d => d.Name == SystemVisitorTypeNames.Accounting) == null && model.IsAccounting)
                {
                    _VisitorService.InsertVisitorTypes(new VisitorTypes { Name = SystemVisitorTypeNames.Accounting, VisitorId = visitor.Id });
                }
                else
                {
                    if (visitor.VisitorTypes.FirstOrDefault(d => d.Name == SystemVisitorTypeNames.Accounting) != null && !model.IsAccounting)
                    {
                        _VisitorService.DeleteVisitorTypes(visitor.VisitorTypes.FirstOrDefault(d => d.Name == SystemVisitorTypeNames.Accounting));
                    }
                }

                _VisitorService.UpdateVisitor(visitor);
                return RedirectToAction("Users");
            }
            else
            {

                //check user name
                var user = _VisitorService.GetVisitorByUsername(model.username);
                if (user != null)
                {
                    ModelState.AddModelError(nameof(VisitorModel.username), "username is already exist");
                    return View("CreateOrUpdateUser", model);
                }
            
            var visitor = new Members();
            visitor.active = model.Active;
            visitor.FirstName = model.FirstName;
            visitor.LastName = model.LastName;
            visitor.tel = model.Phone;
            visitor.Created_utc = DateTime.UtcNow;
            visitor.lastActivityFromUtc = DateTime.UtcNow;
            visitor.VisitorGuid = Guid.NewGuid();
            visitor.email = model.Email;
            visitor.password = model.Password;
            visitor.emailPassword = model.EmailPassword;
            visitor.username = model.username;
            visitor.title = model.Title;
            visitor.name = model.FirstName + " " + model.LastName;
            
            if (model.IsAdmin)
            {
                visitor.VisitorTypes.Add(new VisitorTypes { Name = SystemVisitorTypeNames.Administrators });
            }
            if (model.IsReservation)
            {
                visitor.VisitorTypes.Add(new VisitorTypes { Name = SystemVisitorTypeNames.Reservations });
            }
            if (model.IsOperation)
            {
                visitor.VisitorTypes.Add(new VisitorTypes { Name = SystemVisitorTypeNames.Operations });
            }
            if (model.IsAccounting)
            {
                visitor.VisitorTypes.Add(new VisitorTypes { Name = SystemVisitorTypeNames.Accounting });
            }
            if (model.IsManager)
            {
                visitor.VisitorTypes.Add(new VisitorTypes { Name = SystemVisitorTypeNames.Managing });
            }

            visitor.VisitorTypes.Add(new VisitorTypes { Name = SystemVisitorTypeNames.Members });

            var evisitor = _VisitorService.InsertVisitor(visitor);

            if (evisitor == null)
            {
                    return View("CreateOrUpdateUser", evisitor);
            }


            }
            

            return RedirectToAction("Users");
        }
       

        public ActionResult Users(Members model)
        {
            {
                var members = _VisitorService.GetAllVisitors(false);
                return View(members);

            }
        }

        public ActionResult DeleteUser(int id)
        {
            var visitor = _VisitorService.GetVisitorById(id);
            visitor.Deleted = true;
            _VisitorService.UpdateVisitor(visitor);
            return Json(new { success = true, responseText = "Deleted" }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult DeleteAgency(int id)
        {
                if (_configurationService.DeleteAgency(id))
                {
                    return Json(new { success = true, responseText = "Deleted" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false, responseText = "Not Found" }, JsonRequestBehavior.AllowGet);
                };

        }

        public ActionResult DeactivateUser(int id)
        {
            var visitor = _VisitorService.GetVisitorById(id);
            visitor.active = false;
            _VisitorService.UpdateVisitor(visitor);
            return RedirectToAction("Users");
        }

        public ActionResult ActivateUser(int id)
        {
            var visitor = _VisitorService.GetVisitorById(id);
            visitor.active = true;
            _VisitorService.UpdateVisitor(visitor);
            return RedirectToAction("Users");
        }

         
        public ActionResult AccessDenied()
        {

            return View("AccessDenied");
        }

        public ActionResult Logs()
        {
            var model = _VisitorService.GetSystemLogs();
            return View(model);
        }

        public ActionResult ClearLog()
        {
            _VisitorService.ClearLog();

            return RedirectToAction("Logs");
        }

        public ActionResult Languages()
        {
            List<Language> model = _LanguageService.GetAllLanguages();
            return View(model);
        }

        public ActionResult LanguageAddOrUpdate(int? id)
        {
            Language model;
            if (id.HasValue && id != 0)
            {
                //
                model = _LanguageService.GetLanguageById(id.Value); ;
            }
            else
            {
                model = new Language();
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult LanguageAddOrUpdate(Language model)
        {

            if (model.Id == 0)
            {
                //
                _LanguageService.InsertLanguage(model);
                return RedirectToAction("languages");
            }
            else
            {
                _LanguageService.UpdateLanguage(model);
                return RedirectToAction("languages");
            }
        }

        public ActionResult Languagedelete(int? id)
        {
            Language model;
            if (id.HasValue && id != 0)
            {
                //
                model = _LanguageService.GetLanguageById(id.Value);
                _LanguageService.DeleteLanguage(model);
                ViewBag.success = true;
            }
            else
            {
                ViewBag.success = false;
            }
            return RedirectToAction("languages");
        }

        public ActionResult LanguageRecurces(int? id, string name, string value)
        {
            LocaleStringResourcesModel model = new LocaleStringResourcesModel();
            if (id.HasValue && id != 0)
            {
                //
                var query = _LocalizationService.GetAllResources(id.Value).AsQueryable();

                if (!string.IsNullOrWhiteSpace(name))
                {
                    query = query.Where(d => d.ResourceName.Contains(name.ToLower()));
                }

                if (!string.IsNullOrWhiteSpace(value))
                {
                    query = query.Where(d => d.ResourceValue.Contains(value));
                }

                query = query.Take(20);

                model.LanguageId = id.Value;
                model.Resources = query.ToList();
            }
            else
            {
                return RedirectToAction("Languages");
            }

            return View(model);
        }
        [HttpPost]
        public ActionResult LanguageRecurcesCreate(LocaleStringResourcesModel model)
        {
            if (model.LanguageId != 0)
            {
                //
                LocaleStringResource lsr = new LocaleStringResource();
                lsr.ResourceName = model.Name.ToLower();
                lsr.ResourceValue = model.Value;
                lsr.LanguageId = model.LanguageId;
                _LocalizationService.InsertLocaleStringResource(lsr);

            }

            return RedirectToAction("LanguageRecurces", new { id = model.LanguageId });
        }
        [HttpPost]
        public ActionResult LanguageRecurcesUpdate(int id, string name, string value)
        {
            LocaleStringResource x;
            if (id != 0)
            {

                x = _LocalizationService.GetLocaleStringResourceById(id);
                x.ResourceName = name;
                x.ResourceValue = value;

                _LocalizationService.UpdateLocaleStringResource(x);

                return Json(new { success = true, responseText = "resurce Updated" }, JsonRequestBehavior.AllowGet);

            }

            return Json(new { success = false, responseText = "resurce Updated" }, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public ActionResult LanguageRecurcesDelete(int id)
        {
            LocaleStringResource x;
            if (id != 0)
            {

                x = _LocalizationService.GetLocaleStringResourceById(id);
                _LocalizationService.DeleteLocaleStringResource(x);

                return Json(new { success = true, responseText = "resurce Updated" }, JsonRequestBehavior.AllowGet);

            }

            return Json(new { success = false, responseText = "resurce Updated" }, JsonRequestBehavior.AllowGet);

        }
    }
}