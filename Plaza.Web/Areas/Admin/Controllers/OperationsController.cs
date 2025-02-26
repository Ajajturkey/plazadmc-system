using Line.Models;
using Line.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;


namespace Line.Controllers
{
    public class OperationsController : AdminController
    {
        public ILanguageService _LanguageService;
        public ILocalizationService _LocalizationService;
        public IWorkContext _workContext;
        public IConfiqurationService _configurationService;
        public IReservationService _reservationService;
        public IReportsService _reportsService;
        public IAccountingService _accountingService;
        public IVisitorService _VisitorService;
        public IUploadService _uploadService;
        private readonly int PageSize = 50;

        public OperationsController(ILanguageService languageService, ILocalizationService localizationService,
             IWorkContext workContext, IConfiqurationService configurationService, IVisitorService VisitorService,
            IReservationService reservationService, IReportsService reportsService, IAccountingService accountingService, IUploadService uploadService)
        {
            this._LanguageService = languageService;
            this._LocalizationService = localizationService;
            this._workContext = workContext;
            this._VisitorService = VisitorService;
            this._uploadService = uploadService;
            this._configurationService = configurationService;
            this._reservationService = reservationService;
            this._accountingService = accountingService;
            this._reportsService = reportsService;
        }

        public ActionResult Hotels()
        {
            var SearchModel = new HotelSearchModel();
            SearchModel.CheckinDatefrom = DateTime.Today;
            SearchModel.CheckinDateto = DateTime.Today.AddDays(7);
            var results = _reservationService.SearchHotels(SearchModel);
            ViewBag.Filter = SearchModel;
            ViewBag.hotel = new List<SelectListItem>();
            return View(results.OrderBy(d => d.checkin).ToList());
        }
        [HttpPost]
        public ActionResult Hotels(HotelSearchModel Model)
        {
            var results = _reservationService.SearchHotels(Model);

            var hotel = _configurationService.GetHotelById(Model.hotel);
            if (hotel != null)
            {
                ViewBag.hotel = new List<SelectListItem> { new SelectListItem { Text = hotel.name, Value = hotel.Id.ToString(), Selected = true } };
            }
            else
            {
                ViewBag.hotel = new List<SelectListItem> ();
            }

                ViewBag.Filter = Model;
            return View(results.OrderBy(d => d.checkin).ToList());
        }
        public JsonResult Gethotels(string q)
        {

        
            if(q.Count() >= 3)
            {
               var contents = new SelectList(_configurationService.SearchHotels(0).Select(x => new { Id = x.Id, name = x.name + ", " + x.city + ", " + x.country })
                   .Where(d=> d.name.ToLower().Contains(q.ToLower())), "Id", "name");
                return Json(contents,JsonRequestBehavior.AllowGet);
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }
        

        public ActionResult Checkin(ReservationSearchModel Model, int page = 1)
        {
            if (Request.HttpMethod == "POST")
            {
                page = 1;
            }
            if (Model.CheckinDatefrom == new DateTime())
            {
                Model.CheckinDatefrom = DateTime.Today;
                Model.CheckinDateto = DateTime.Today.AddDays(2);
            }
            ViewBag.Filter = Model;
            var results = _reservationService.SearchReservations(Model, true);
            return View(results.OrderBy(d=> d.checkindate).ToPagedList(page , PageSize));
        }

        public ActionResult Checkout(ReservationSearchModel Model = null,int page = 1)
        {

            if (Request.HttpMethod == "POST")
            {
                page = 1;
            }

            if (Model.CheckoutDatefrom == new DateTime())
            {
                Model = new ReservationSearchModel();
                Model.CheckoutDatefrom = DateTime.Today;
                Model.CheckoutDateto = DateTime.Today.AddDays(5);
            }
            ViewBag.Filter = Model;
            var results = _reservationService.SearchReservations(Model, true);
            return View(results.OrderBy(d => d.balanceDate).ToPagedList(page,PageSize));
        }
        
        
        public ActionResult Tours(GenerecSearchModel Model)
        {
            ViewBag.Suppliers = new SelectList(_configurationService.SearchSupplier(0) , "Id", "name");
            if (Model.Datefrom == new DateTime())
            {
                Model.Datefrom = DateTime.Today;
                Model.Dateto = DateTime.Today.AddDays(7);
            }
            var results = _reservationService.SearchTours(Model);
            return View(results);
        }
        [HttpPost]
        public ActionResult Tour(int Id, string guide = "",string note = "",int Sid = 0)
        {
            var tur = _reservationService.GetTourById(Id);
            if (tur != null)
            {
                if (!string.IsNullOrEmpty(guide))
                {
                    tur.guide = guide;
                }
                if (!string.IsNullOrEmpty(note))
                {
                    tur.note = note;
                }

                if (Sid > 0)
                {
                    tur.ref_supplier = Sid;
                }

                var result =  _reservationService.UpdateTour(tur);

                return Json(new { success = true, Guide = tur.guide, Note = tur.note }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false, Guide = "", Note = "" }, JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult packages(GenerecSearchModel Model)
        {
                if (Model.Datefrom == new DateTime())
                {
                    Model.Datefrom = DateTime.Today;
                    Model.Dateto = DateTime.Today.AddDays(3);
                }
                var results = _reservationService.SearchPackages(Model);
                return View(results);
        }
        
        public ActionResult Transfers(GenerecSearchModel Model)
        {
            ViewBag.Suppliers = new SelectList(_configurationService.SearchSupplier(0), "Id", "Name");
            if (Model.Datefrom == new DateTime())
            {
                Model.Datefrom = DateTime.Today;
                Model.Dateto = DateTime.Today.AddDays(3);
            }
            var results = _reservationService.SearchTransfer(Model);
            return View(results);
        }
        [HttpPost]
        public ActionResult Transfer(int Id, string guide = "", string note = "", int Sid = 0)
        {
            var item = _reservationService.GetTransferById(Id);
            if (item != null)
            {
                if (!string.IsNullOrEmpty(guide))
                {
                    item.guide = guide;
                }
                if (!string.IsNullOrEmpty(note))
                {
                    item.note = note;
                }
                if (Sid > 0)
                {
                    item.ref_supplier = Sid;
                }

                var result = _reservationService.UpdateTransfer(item);

                return Json(new { success = true, Guide = item.guide, Note = item.note,Supplier = item.Supplier.name }, JsonRequestBehavior.AllowGet);

            }
            return Json(new { success = false, Guide = "", Note = "" }, JsonRequestBehavior.AllowGet);
        }

        
        public ActionResult Flights(GenerecSearchModel Model, int page)
        {
            if (Model.Datefrom == new DateTime())
            {
                Model.Datefrom = DateTime.Today;
                Model.Dateto = DateTime.Today.AddDays(3);
            }
            var results = _reservationService.SearchPackages(Model);
            return View(results);
        }
    }
}