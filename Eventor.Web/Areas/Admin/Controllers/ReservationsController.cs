using Line.Data;
using Line.Models;
using Line.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using Line.Helpers;

namespace Line.Controllers
{
    public class ReservationsController : AdminController
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

        public ReservationsController(ILanguageService languageService, ILocalizationService localizationService,
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
        public ActionResult QuickSearch(string qid)

        {
            int value;
            if (int.TryParse(qid, out value))
                return RedirectToAction("OpenReservations", new { Id = value });

            return RedirectToAction("Search");
        }

        public JsonResult GetAgencies(string q)
        {
            if (q.Count() >= 3)
            {
                var contents = new SelectList(_configurationService.Agencies().Select(x => new { Id = x.ID, name = x.AgencyName + ", " + x.City + ", " + x.Country })
                    .Where(d => d.name.ToLower().Contains(q.ToLower())), "Id", "name");
                return Json(contents, JsonRequestBehavior.AllowGet);
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetHotels(string q)
        {
            if (q.Count() >= 3)
            {
                var contents = new SelectList(_configurationService.SearchHotels(0).Select(x => new { Id = x.Id, name = x.name + ", " + x.city + ", " + x.country })
                    .Where(d => d.name.ToLower().Contains(q.ToLower())), "Id", "name");
                return Json(contents, JsonRequestBehavior.AllowGet);
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }
        // GET: Admin/Reservations

        public ActionResult CreateReservations()
        {
            var selectagency = new List<SelectListItem>();
            ViewBag.Agencies = selectagency;
            return View(new ReservationControl());
        }
        [HttpPost]
        public ActionResult CreateReservations(ReservationControl Model)
        {
            ViewErrors errors = new ViewErrors();

            var agency = _configurationService.GetAgencyById(Model.AID);
            if (agency == null)
            {
                return PageNotFound();
            }

            var selectagency = new List<SelectListItem>();
            selectagency.Add(new SelectListItem { Text = agency.AgencyName, Value = Model.AID.ToString() });
            ViewBag.Agencies = selectagency;

            //we must create empty file to fill later 
            var reservation = CreateFileToAgency(agency);

            //check balance and limits
            var ress = agency.Reservations.Where(d => d.status == ReservationStatus.Confirmed.ToString() || d.status == ReservationStatus.ReConfirmed.ToString());
            var balance = ress.Sum(d => d.price) - ress.Sum(d=> d.Payments.Sum(x=> x.Payment)) - agency.AdvancePayment.Sum(d => d.Payment);
            var Credit = Convert.ToDecimal(agency.CreditAmount);
            //if (agency.Credit == CreditType.Cash.ToString())
            //{
            //    errors.Errors.Add("Cash");
            //    ViewBag.warning = errors;
            //    reservation.status = ReservationStatus.Locked.ToString();
            //    reservation.note = "Cash agency";

            //    if (!Model.CreateApproved)
            //    {
            //        return View(Model);
            //    }

            //}
            //else 
            if (agency.Credit == CreditType.Limited.ToString() && balance >= Credit)
            {
                errors.Errors.Add("Limited");
                errors.Errors.Add("Balance is : " + balance.ToString(true) + " Credit Ammount: " + Credit.ToString(true));
                ViewBag.warning = errors;
                reservation.status = ReservationStatus.Locked.ToString();
                reservation.note = "Balance is : " + balance.ToString(true) + " Credit Ammount: " + Credit.ToString(true);

                if (!Model.CreateApproved)
                {
                    return View(Model);
                }
            }
           


            var result = _reservationService.InsertReservation(reservation);
            if (result != null)
            {
                _reservationService.InsertFileNote(new Filelog { Action = "File Created: ", ref_file = result.ID, ref_member = result.ref_member, shortMessage = agency.AgencyName + " : New File" });
            }
            return RedirectToAction("OpenReservations", new { Id = result.ID });
        }


        public ActionResult LockedReservations()
        {
            var SearchModel = new ReservationSearchModel();

            SearchModel.Status.Add(ReservationStatus.Locked.ToString());
            if (!_workContext.IsAdmin)
            {
                SearchModel.CreatedBy.Add(_workContext.CurrentVisitor.Id);
            }
            var result = _reservationService.SearchReservations(SearchModel, false);

            return View(result);
        }

        public ActionResult Unlock(int Id)
        {
            var res = _reservationService.GetReservationById(Id);
            if (res == null)
            {
                return PageNotFound();
            }
            res.status = ReservationStatus.Request.ToString();
            _reservationService.UpdateReservation(res);
            _reservationService.InsertFileNote(new Filelog
            {
                Action = "File Unlocked",
                ref_file = res.ID,
                ref_member = _workContext.CurrentVisitor.Id,
                shortMessage = "File Unlocked by " + _workContext.CurrentVisitor.GetFullName()
            });
            return Json(new { success = true, responseText = "Unlocked" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ConfirmReservations(int cId, FormCollection form)
        {
            var note = form["cNote"];
            var email = form["cemail"];
            var cc = form["cemailcc"];
            var send = form["cSend"];


            var res = _reservationService.GetReservationById(cId);
            if (res == null)
            {
                return PageNotFound();
            }
            res.status = ReservationStatus.Confirmed.ToString();
            _reservationService.UpdateReservation(res);
            _reservationService.InsertFileNote(new Filelog
            {
                Action = "File Confirmed ",
                ref_file = res.ID,
                ref_member = _workContext.CurrentVisitor.Id,
                shortMessage = "File Confirmed by " + _workContext.CurrentVisitor.GetFullName()
            });


            if (!string.IsNullOrEmpty(send))
            {
                Mailing.SendMailToAgency(res.Agency.AgencyName, res.agencystaff, email, note, cc,
                    res.Agency.AgencyName, _workContext.CurrentVisitor.GetFullName(), _workContext.CurrentVisitor.title, _workContext.CurrentVisitor.email, res);
            }

            return RedirectToAction("OpenReservations", new { Id = cId });
        }
        public ActionResult ReConfirmReservations(int rcId, FormCollection form)
        {
            var note = form["rcNote"];
            var email = form["rcemail"];
            var cc = form["rcemailcc"];
            var send = form["rcSend"];
            var voucher = form["rcvocher"];
            var res = _reservationService.GetReservationById(rcId);
            if (res == null)
            {
                return PageNotFound();
            }
            res.vocher = Request.Form["rcvocher"];

            res.status = ReservationStatus.ReConfirmed.ToString();
            _reservationService.UpdateReservation(res);

            if (!string.IsNullOrEmpty(send))
            {
                Mailing.SendMailToAgency(res.Agency.AgencyName, res.agencystaff, email, note, cc,
                res.Agency.AgencyName, _workContext.CurrentVisitor.GetFullName(), _workContext.CurrentVisitor.title, _workContext.CurrentVisitor.email, res, voucher);

                _reservationService.InsertFileNote(new Filelog { Action = "File Sent ", ref_file = res.ID, ref_member = _workContext.CurrentVisitor.Id, shortMessage = "File ReConfirmed And sent to Agency by " + _workContext.CurrentVisitor.GetFullName() });
            }
            _reservationService.InsertFileNote(new Filelog { Action = "File ReConfirmed ", ref_file = res.ID, ref_member = _workContext.CurrentVisitor.Id, shortMessage = "File ReConfirmed by " + _workContext.CurrentVisitor.GetFullName() });

            return RedirectToAction("OpenReservations", new { Id = rcId });
        }
        public ActionResult CancelReservations(int Id)
        {
            var res = _reservationService.GetReservationById(Id);
            if (res == null)
            {
                return PageNotFound();
            }
            res.status = ReservationStatus.Canceled.ToString();
            _reservationService.UpdateReservation(res);
            _reservationService.InsertFileNote(new Filelog { Action = "File Canceled ", ref_file = res.ID, ref_member = _workContext.CurrentVisitor.Id, shortMessage = res.Agency.AgencyName + "File Canceled by " + _workContext.CurrentVisitor.GetFullName() });

            return RedirectToAction("OpenReservations", new { Id = res.ID });
        }

        public ActionResult Deadlines(int Id)
        {
            var dlines = _reservationService.SearchDeadlines(Id);

            return PartialView(dlines);
        }

        public ActionResult OpenInvoice(int Id)
        {
            var setting = _configurationService.GetSettings();
            ViewBag.header = _uploadService.GetPicture(setting.InvoiceHeader);
            ViewBag.footer = _uploadService.GetPicture(setting.InvoiceFooter);

            var res = _reservationService.GetReservationById(Id);
            if (res == null)
            {
                return PageNotFound();
            }
            if (res.Agency.Country.ToLower().Contains("kuwait") 
                || res.Agency.Country.ToLower().Contains("kw")
                || res.Agency.Country.ToLower().Contains("lebanon")
                || res.Agency.Country.ToLower().Contains("lb")

                
                )
            {
                return View("OpenInvoice2", res);
            }
            else
            {
                return View(res);
            }

        }
        public ActionResult OpenVocher(int Id)
        {
            var setting = _configurationService.GetSettings();
            ViewBag.header = _uploadService.GetPicture(setting.VoucherHeader);
            ViewBag.footer = _uploadService.GetPicture(setting.VoucherFooter);
            return OpenReservations(Id);
        }
        public ActionResult Settings(int Id)
        {
            var res = _reservationService.GetReservationById(Id);

            ViewBag.Agencies = new SelectList(_configurationService.SearchAgencies(0), "Id", "AgencyName");
            ViewBag.Members = new SelectList(_VisitorService.GetAllVisitors(true).Select(d => new { Id = d.Id, Name = d.FirstName + " " + d.LastName }), "Id", "Name");

            if (res == null)
            {
                return PageNotFound();
            }
            return View(res);
        }

        [HttpPost]
        public ActionResult ChangeOwner(int Id = 0)
        {
            var res = _reservationService.GetReservationById(Id);
            if (res == null)
            {
                return PageNotFound();
            }

            int mid = Convert.ToInt32(Request.Form["owner"]);
            string password = Request.Form["opass"];
            if (res.Visitor.password == password || res.Visitor.Id == mid)
            {
                res.ref_member = mid;
                _reservationService.UpdateReservation(res);
            }

            return RedirectToAction("Settings", new { Id = Id });
        }
        [HttpPost]
        public ActionResult ChangeAgency(int Id)
        {
            var res = _reservationService.GetReservationById(Id);
            if (res == null)
            {
                return PageNotFound();
            }
            string agencystaff = Request.Form["Agencystaff"];
            res.agencystaff = agencystaff;

            int aid = Convert.ToInt32(Request.Form["ref_agency"]);
            var agency = _configurationService.GetAgencyById(aid);
            if (agency != null)
            {
                res.ref_agency = aid;  
            }
            _reservationService.UpdateReservation(res);

            return RedirectToAction("Settings", new { Id = Id });
        }
        [HttpPost]
        public ActionResult UpdateNote(int Id)
        {
            var res = _reservationService.GetReservationById(Id);
            if (res == null)
            {
                return PageNotFound();
            }
            res.note = Request.Form["note"];
            _reservationService.UpdateReservation(res);

            return RedirectToAction("Settings", new { Id = Id });
        }
        [HttpPost]
        public ActionResult InsertDeadline(int Id)
        {
            var res = _reservationService.GetReservationById(Id);
            if (res == null)
            {
                return PageNotFound();
            }
            var date = Convert.ToDateTime(Request.Form["dldate"]);
            var dtype = Request.Form["dtype"];
            var dnote = Request.Form["dnote"];

            var x = _reservationService.InsertDeadline(new DeadLine
            {
                Category = dtype,
                Expaire = date,
                Ref_file = Id,
                Description = dnote
            });

            return RedirectToAction("Settings", new { Id = Id });
        }


        public ActionResult SendInvoice(int Id)
        {
            var res = _reservationService.GetReservationById(Id);
            if (res == null)
            {
                return PageNotFound();
            }
            return OpenReservations(Id);
        }
        public ActionResult SendVocher(int Id)
        {
            var res = _reservationService.GetReservationById(Id);
            if (res == null)
            {
                return PageNotFound();
            }
            return OpenReservations(Id);
        }

        public ActionResult SendHotel(FormCollection form)
        {
            var Id = Convert.ToInt32(form["shid"]);
            var note = form["hnote"];
            var contact = form["hcontact"];
            var email = form["hemail"];
            var name = form["hname"];
            var request = Convert.ToInt32(form["request"]);

            var hotel = _reservationService.GetHotelById(Id);
            if (hotel == null)
            {
                return PageNotFound();
            }
            if (!string.IsNullOrEmpty(form["updateconfirm"]))
            {
                var datahotel = _configurationService.GetHotelById(hotel.hid.Value);
                datahotel.rname = contact;
                datahotel.Email = email;
                datahotel.Tel = form["htel"];
                datahotel.Fax = form["hfax"];

                _configurationService.UpdateHotel(datahotel);
            }

            Mailing.SendMailToHotel(
                (MailType)request, name, email, note, contact, _workContext.CurrentVisitor.GetFullName()
                , _workContext.CurrentVisitor.title, _workContext.CurrentVisitor.email,
                hotel, hotel.RID.ToString());

            hotel.SendHotel = true;
            hotel.Confirmed = false;

            _reservationService.UpdateHotel(hotel);

            return RedirectToAction("OpenReservations", new { Id = hotel.RID });
        }

        public ActionResult ConfirmHotel(int Id)
        {
            var hotel = _reservationService.GetHotelById(Id);
            if (hotel == null)
            {
                return PageNotFound();
            }
            hotel.Confirmed = true;
            _reservationService.UpdateHotel(hotel);
            return Json(new { success = true, responseText = "Confirmed" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Preparehotel(int id)
        {
            var hotel = _configurationService.GetHotelById(id);

            if (hotel == null)
            {
                return Content("");
            }

            return Json(new
            {
                shid = hotel.Id,
                hname = hotel.name,
                hemail = hotel.Email,
                htel = hotel.Tel,
                hfax = hotel.Fax,
                hcontact = hotel.rname
            }, JsonRequestBehavior.AllowGet);
        }

        private Reservations CreateFileToAgency(Agency agency)
        {
            var reservation = new Reservations();
            reservation.ref_agency = agency.ID;
            reservation.Ref_CustomerId = 0;
            reservation.ref_marketman = "0";
            reservation.ref_member = _workContext.CurrentVisitor.Id;
            reservation.agencystaff = "";
            reservation.updateuser = "";

            reservation.price = 0;
            reservation.priceTL = 0;
            reservation.profit = 0;
            reservation.cost = 0;
            reservation.costTL = 0;
            reservation.net = 0;
            reservation.balance = 0;
            reservation.curency = Currency.USD.ToString();

            reservation.date = DateTime.Now;
            reservation.balanceDate = DateTime.Now;
            reservation.finishDate = DateTime.Now;
            reservation.updatedate = DateTime.Now;
            reservation.checkindate = DateTime.Now.Date;
            reservation.finishDate = DateTime.Now.Date;
            reservation.updateuser = _workContext.CurrentVisitor.username;

            reservation.vocher = "";
            reservation.status = ReservationStatus.Request.ToString();
            reservation.sendmail = false;
            reservation.reconfirmed = false;
            reservation.Code = "";

            reservation.note = "";
            reservation.IsB2B = true;

            return reservation;
        }

        public ActionResult PageNotFound()
        {
            throw new NotImplementedException();
        }

        public ActionResult ViewReservations(int Id)
        {
            var res = _reservationService.GetReservationById(Id);
            if (res == null)
            {
                return PageNotFound();
            }

            return View(res);
        }
        public ActionResult OpenReservations(int Id)
        {
            var res = _reservationService.GetReservationById(Id);
            if (res == null)
            {
                return PageNotFound();
            }

            if (res.status == ReservationStatus.Locked.ToString())
            {
                return RedirectToAction("ViewReservations", new { Id = Id });
            }

            return View(res);
        }


        public ActionResult Search()
        {

            ViewBag.Users = _VisitorService.GetMembers();
            ViewBag.Countries = _configurationService.GetUsedCountries("");
            ViewBag.FileStatus = new SelectList(Enum.GetNames(typeof(ReservationStatus)));
            ViewBag.FileDate = new SelectList(Enum.GetNames(typeof(ReservationDate)));
            var Model = new ReservationSearchModel();
            //var Result = _reservationService.SearchReservations(Model, true);
            ViewBag.Reseult = new List<Reservations>();

            return View(Model);
        }

        [HttpPost]
        public ActionResult Search(ReservationSearchModel Model)
        {
            ViewBag.Users = _VisitorService.GetMembers();


            ViewBag.Countries = _configurationService.GetUsedCountries("");
            ViewBag.FileStatus = new SelectList(Enum.GetNames(typeof(ReservationStatus)));
            ViewBag.FileDate = new SelectList(Enum.GetNames(typeof(ReservationDate)));


            //update files date
            var paymentdatetype = Request.Form["FileDate"];
            if (paymentdatetype == "Checkin")
            {
                Model.CheckinDatefrom = Model.CreateDate;
                Model.CheckinDateto = Model.CreateDateTo;

                Model.CreateDate = Model.CreateDateTo = new DateTime();
            }
            else if (paymentdatetype == "Checkout")
            {
                Model.CheckoutDatefrom = Model.CreateDate;
                Model.CheckoutDateto = Model.CreateDateTo;
                Model.CreateDate = Model.CreateDateTo = new DateTime();
            }

            var Result = _reservationService.SearchReservations(Model, false);
            ViewBag.Reseult = Result;

            return View(Model);
        }

        
        public ActionResult CustomerSearch(ReservationSearchModel Model)
        {
            if (Request.HttpMethod == "GET")
            {
                ViewBag.Reseult = new List<Reservations>();

                if (Model.CreateDate == new DateTime())
                {
                    Model.CreateDate = DateTime.Now;
                }
                if (Model.CreateDateTo == new DateTime())
                {
                    Model.CreateDateTo = DateTime.Now.AddDays(5);
                }
                return View(Model);
            }

            Model.CheckinDatefrom = Model.CreateDate;
            Model.CheckinDateto = Model.CreateDateTo;
            Model.CreateDate = Model.CreateDateTo = new DateTime();
            bool nodate = Model.OnlyPaidFiles;
            if (nodate)
            {
                Model.CheckinDatefrom = Model.CheckinDateto = new DateTime();
            }
            Model.OnlyPaidFiles = false;
            var Result = _reservationService.SearchReservations(Model, true);
            ViewBag.Reseult = Result;

            Model.CreateDate = Model.CheckinDatefrom;
            Model.CreateDateTo = Model.CheckinDateto;
            Model.OnlyPaidFiles = nodate;
            return View(Model);
        }

        
        public ActionResult UserSearch(ReservationSearchModel Model)
        {
            ViewBag.Users = _VisitorService.GetMembers();
            ViewBag.FileDate = new SelectList(Enum.GetNames(typeof(ReservationDate)), "0");
            if (Request.HttpMethod == "GET")
            {
                ViewBag.Reseult = new List<Reservations>();
                Model.Status.Add(ReservationStatus.Confirmed.ToString());
                Model.Status.Add(ReservationStatus.ReConfirmed.ToString());
                Model.Status.Add(ReservationStatus.Request.ToString());

                if (Model.CreateDate == new DateTime())
                {
                    Model.CreateDate = DateTime.Now;
                }
                if (Model.CreateDateTo == new DateTime())
                {
                    Model.CreateDateTo = DateTime.Now.AddDays(5);
                }
                return View(Model);
            }
            var paymentdatetype = Request.Form["FileDate"];
            if (paymentdatetype == "Checkin")
            {
                Model.CheckinDatefrom = Model.CreateDate;
                Model.CheckinDateto = Model.CreateDateTo;
                ViewBag.FileDate = new SelectList(Enum.GetNames(typeof(ReservationDate)), "Checkin");
                Model.CreateDate = Model.CreateDateTo = new DateTime();
            }
            else if (paymentdatetype == "Checkout")
            {
                Model.CheckoutDatefrom = Model.CreateDate;
                Model.CheckoutDateto = Model.CreateDateTo;
                ViewBag.FileDate = new SelectList(Enum.GetNames(typeof(ReservationDate)), "Checkout");
                Model.CreateDate = Model.CreateDateTo = new DateTime();
            }

            var Result = _reservationService.SearchReservations(Model, false);
            ViewBag.Reseult = Result;


            Model.CreateDate = Model.CheckinDatefrom;
            Model.CreateDateTo = Model.CheckinDateto;

            return View(Model);
        }

        
        public ActionResult ReservationSearch(ReservationSearchModel Model)
        {
            ViewBag.FileStatus = new SelectList(Enum.GetNames(typeof(ReservationStatus)));
            ViewBag.FileDate = new SelectList(Enum.GetNames(typeof(ReservationDate)));


            if (Request.HttpMethod == "GET")
            {
                ViewBag.Reseult = new List<Reservations>();
                Model.Status.Add(ReservationStatus.Confirmed.ToString());
                Model.Status.Add(ReservationStatus.ReConfirmed.ToString());
                if (Model.CreateDate == new DateTime())
                {
                    Model.CreateDate = DateTime.Now;
                }
                if (Model.CreateDateTo == new DateTime())
                {
                    Model.CreateDateTo = DateTime.Now.AddDays(5);
                }
                return View(Model);
            }
            
            //update files date
            var paymentdatetype = Request.Form["FileDate"];
            if (paymentdatetype == "Checkin")
            {
                Model.CheckinDatefrom = Model.CreateDate;
                Model.CheckinDateto = Model.CreateDateTo;
                Model.CreateDate = Model.CreateDateTo = new DateTime();
            }
            else if (paymentdatetype == "Checkout")
            {
                Model.CheckoutDatefrom = Model.CreateDate;
                Model.CheckoutDateto = Model.CreateDateTo;
                Model.CreateDate = Model.CreateDateTo = new DateTime();
            }

            var Result = _reservationService.SearchReservations(Model, false);
            ViewBag.Reseult = Result;

  

            return View(Model);
        }

        
        public ActionResult AgencySearch(ReservationSearchModel Model)
        {

            if (Request.HttpMethod == "GET")
            {
                ViewBag.Reseult = new List<Reservations>();
                Model.Status.Add(ReservationStatus.Confirmed.ToString());
                Model.Status.Add(ReservationStatus.ReConfirmed.ToString());
                if (Model.CreateDate == new DateTime())
                {
                    Model.CreateDate = DateTime.Now;
                }
                if (Model.CreateDateTo == new DateTime())
                {
                    Model.CreateDateTo = DateTime.Now.AddDays(5);
                }
                return View(Model);
            }

            Model.CheckinDatefrom = Model.CreateDate;
            Model.CheckinDateto = Model.CreateDateTo;
            Model.CreateDate = Model.CreateDateTo = new DateTime();

            var Result = _reservationService.SearchReservations(Model, false);
            ViewBag.Reseult = Result;

            Model.CreateDate = Model.CheckinDatefrom;
            Model.CreateDateTo = Model.CheckinDateto;

            return View(Model);
        }


        public ActionResult UnconfirmedHotels(string CheckinDatefrom, string CheckinDateto, int page = 1)
        {
            var searchModel = new HotelSearchModel();
            searchModel.UnConfirmed = true;
            searchModel.CheckinDatefrom = DateTime.Now.Date;
            searchModel.CheckinDateto = DateTime.Now.AddMonths(1);

            if (!string.IsNullOrEmpty(CheckinDatefrom))
            {
                searchModel.CheckinDatefrom = Date.GetDateTime(CheckinDatefrom);
            }
            if (!string.IsNullOrEmpty(CheckinDateto))
            {
                searchModel.CheckinDateto = Date.GetDateTime(CheckinDateto);
            }

            if (Request.HttpMethod == "POST") { page = 1; }
                var Model = _reservationService.SearchHotels(searchModel);
            ViewBag.Search = searchModel;
            return View(Model.ToPagedList(page, PageSize));
        }

        public ActionResult CashHotels(HotelSearchModel searchModel, int page = 1)
        {
            searchModel.IsCash = true;
            if (searchModel.Cashfrom == new DateTime())
            {
                searchModel.Cashfrom = DateTime.Now.Date;
            }
            if (searchModel.Cashto == new DateTime())
            {
                searchModel.Cashto = DateTime.Now.AddMonths(1);
            }
            
            ViewBag.Filter = searchModel;
            var Model = _reservationService.SearchHotels(searchModel);

            return View(Model.ToPagedList(page, PageSize));
        }

        public ActionResult ReservationHotels(int Id)
        {
            ViewBag.Hotels = new List<SelectListItem>();

            var model = new HotelsviewModel();

            var reservation = _reservationService.GetReservationById(Id);

            model.Agency = reservation.Agency.AgencyName;
            model.RID = Id;
            model.Total = reservation.Rhotel.Sum(d => d.Total).ToString();
            model.Owner = reservation.Visitor.GetFullName();
            model.Hotles = reservation.Rhotel.ToList();

            return View(model);
        }
        [HttpPost]
        public ActionResult ReservationHotels(HotelsviewModel model)
        {
            //Add hotel to reservation
            var hotel = _reservationService.InsertHotel(new Rhotel
            {
                ref_supplier = 1,
                note = "",
                name = _configurationService.GetHotelById(model.hotel).name,
                checkin = DateTime.Today.Date,
                checkout = DateTime.Today.Date,
                customer = "",
                Confirmed = false,
                Cost = 0,
                Total = 0,
                hid = model.hotel,
                SendHotel = false,
                RID = model.RID
            });

            if (hotel == null)
            {
                return RedirectToAction("ReservationHotels", new { Id = hotel.ID });
            }

            return RedirectToAction("ReservationRooms", new { Id = hotel.ID });

        }
        public void HotelchangedEvents(int Id)
        {
            var hotel = _reservationService.GetHotelById(Id);

            //clear old recoreds
            _reservationService.ClearHotelchangedRecoreds(Id);

            var hotelchanged = new hotelchange
            {
                hid = Id,
                checkin = hotel.checkin,
                checkout = hotel.checkout,
                Cost = hotel.Cost,
                Total = hotel.Total,
                customer = hotel.customer,
                name = hotel.name,
                note = hotel.note,
                RID = hotel.RID,
            };

            hotelchanged = _reservationService.InsertHotelChange(hotelchanged);

            foreach (var item in hotel.RHRoom)
            {
                var roomchanged = new ChangeRoom
                {
                    name = item.name,
                    board = item.board,
                    type = item.type,
                    checkin = item.checkin,
                    checkout = item.checkout,
                    cost = item.cost,
                    price = item.price,
                    count = item.count,
                    guset = item.guset,
                    HID = hotelchanged.id
                };
                _reservationService.InsertRoomChange(roomchanged);
            }

        }

        public void ClaculateChanges(int Id)
        {

            var hotel = _reservationService.GetHotelById(Id);
            if (hotel == null)
            {
                return;
            }

            if (hotel.RHRoom.Count == 0)
            {
                hotel.checkin = DateTime.Now.Date;
                hotel.checkout = DateTime.Now.Date;
                hotel.Confirmed = false;
                hotel.SendHotel = false;
                hotel.Total = 0;
                hotel.Cost = 0;
                if (_reservationService.UpdateHotel(hotel) != null)
                {
                    _reservationService.InsertFileNote(new Filelog { Action = "Hotel Updated , No More Rooms", ref_file = hotel.RID, ref_member = hotel.Reservations.ref_member, shortMessage = hotel.name + " has been updated" });
                }
                return;
            }

            hotel.checkin = hotel.RHRoom.Min(d => d.checkin).Value;
            hotel.checkout = hotel.RHRoom.Min(d => d.checkout).Value;
            hotel.Total = hotel.RHRoom.Sum(d => d.price * d.count * (d.checkout.Value - d.checkin.Value).Days);
            hotel.Cost = hotel.RHRoom.Sum(d => d.cost * d.count * (d.checkout.Value - d.checkin.Value).Days);
            hotel.Confirmed = false;
            hotel.SendHotel = false;

            if (_reservationService.UpdateHotel(hotel) != null)
            {
                _reservationService.InsertFileNote(new Filelog
                {
                    Action = "Hotel Updated",
                    ref_file = hotel.RID,
                    ref_member = _workContext.CurrentVisitor.Id,
                    shortMessage = hotel.name + " has been updated | New Total: " + hotel.Total.ToString(true)
                });
            }

        }
        public ActionResult ReservationRooms(int Id)
        {
            var List = Enum.GetNames(typeof(RoomTypes));
            List[(int)RoomTypes.TwinWithExtrabed] = "Twin + Extra bed";
            List[(int)RoomTypes.DoubleWithChild] = "Double + Child";
            List[(int)RoomTypes.DoubleWithExtrabed] = "Double + Extra bed";
            ViewBag.RoomTypes = new SelectList(List);
            ViewBag.RoomBoard = new SelectList(Enum.GetNames(typeof(RoomBoard)));

            var Hotel = _reservationService.GetHotelById(Id);
            if (Hotel == null)
            {
                return PageNotFound();
            }
            var datahotel = _configurationService.GetHotelById(Hotel.hid.Value);
            var reservation = _reservationService.GetReservationById(Hotel.RID);
            var model = new RoomsViewModel();
            model.Location = datahotel.city + ", " + datahotel.country;
            model.Owner = Hotel.Reservations.Visitor.GetFullName();
            model.RID = Hotel.Reservations.ID;
            model.Room.HID = Hotel.ID;
            model.Agency = reservation.Agency.AgencyName;
            model.ReservationDate = reservation.date.Value.ToFString();
            model.Total = Hotel.Total.ToString();
            model.Hotel = Hotel;
            model.Contact = datahotel.rname;
            model.Email = datahotel.Email;
            model.Owner = reservation.Visitor.GetFullName();
            model.Rooms = Hotel.RHRoom.ToList();
            model.Room.guset = reservation.GetCustomers();

            return View(model);
        }

        [HttpPost]
        public ActionResult ReservationRooms(int Id, FormCollection form)
        {

            var Hotel = _reservationService.GetHotelById(Id);
            if (Hotel == null)
            {
                return PageNotFound();
            }

            var isChash = form["Hotel.IsCash"];
            var date = form["Hotel.CashDate"];

            Hotel.IsCash = isChash == "Cash" ? true : false;
            if (Hotel.IsCash)
            {
                try
                {
                    Hotel.CashDate = Convert.ToDateTime(date);

                }
                catch (Exception ex)
                {
                    Hotel.CashDate = null;
                }
            }
            else
            {
                Hotel.CashDate = null;
            }
            _reservationService.UpdateHotel(Hotel);
            return ReservationRooms(Id);
        }

        public ActionResult PrepareRoom(int id)
        {
            var room = _reservationService.GetHotelRoomById(id);

            if (room == null)
            {
                return Content("");
            }

            return Json(new
            {
                ID = room.ID,
                HID = room.HID,
                name = room.name,
                type = room.type,
                price = room.price,
                cost = room.cost,
                checkin = room.checkin.Value.ToFString(),
                checkout = room.checkout.Value.ToFString(),
                board = room.board,
                count = room.count,
                guset = room.guset,
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult EditRoom(FormCollection form)
        {
            var RID = Convert.ToInt32(form["uID"]);
            var HID = Convert.ToInt32(form["uHID"]);
            var room = _reservationService.GetHotelRoomById(RID);
            if (room == null)
            {
                return PageNotFound();
            }

            room.name = form["uname"];
            room.type = form["utype"];
            room.price = Convert.ToDecimal(form["uprice"]);
            room.cost = Convert.ToDecimal(form["ucost"]);
            room.checkin = Convert.ToDateTime(form["ucheckin"]);
            room.checkout = Convert.ToDateTime(form["ucheckout"]);
            room.board = form["uboard"];
            room.count = Convert.ToInt32(form["ucount"]);
            room.guset = form["uguset"];


            //chnage Notifications
            var hotel = _reservationService.GetHotelById(room.HID);
            if (hotel.Confirmed || hotel.SendHotel)
            {
                HotelchangedEvents(hotel.ID);
            }

            var result = _reservationService.UpdateHotelRoom(room);

            if (result == null)
            {
                return View();
            }

            _reservationService.InsertFileNote(new Filelog
            {
                Action = "Hotel Room Edited",
                ref_file = result.Rhotel.RID,
                ref_member = _workContext.CurrentVisitor.Id,
                shortMessage = result.Rhotel.name + ": Hotel Room Edited, <br/> Price" + result.price + " Dates: " +
                result.checkin.Value.ToShortDateString() + " - " + result.checkout.Value.ToShortDateString()
            });

            //notifiy hotel change
            ClaculateChanges(room.HID);
            //calculate reservation
            _reservationService.CalculateReservation(hotel.RID);

            return RedirectToAction("ReservationRooms", new { Id = room.HID });
        }

        [HttpPost]
        public ActionResult CreateRoom(RoomsViewModel Model)
        {
            Model.Room.checkin = Convert.ToDateTime(Request.Form["Room.checkin"]);
            Model.Room.checkout = Convert.ToDateTime(Request.Form["Room.checkout"]);

            //chnage Notifications
            var hotel = _reservationService.GetHotelById(Model.Room.HID);
            if (hotel.Confirmed || hotel.SendHotel)
            {
                HotelchangedEvents(hotel.ID);
            }



            var result = _reservationService.InsertHotelRoom(Model.Room);
            if (result == null)
            {
                return View(); // error
            }
            _reservationService.InsertFileNote(new Filelog
            {
                Action = "Hotel Room Inserted",
                ref_file = result.Rhotel.RID,
                ref_member = _workContext.CurrentVisitor.Id,
                shortMessage = result.Rhotel.name + ": Hotel Room Inserted, <br/> Price" + result.price + " Dates: " +
                result.checkin.Value.ToShortDateString() + " - " + result.checkout.Value.ToShortDateString()
            });
            //notifiy hotel change
            ClaculateChanges(Model.Room.HID);
            //calculate reservation
            _reservationService.CalculateReservation(hotel.RID);

            return RedirectToAction("ReservationRooms", new { Id = result.HID });
        }
        public ActionResult DeleteRoom(int Id)
        {
            var room = _reservationService.GetHotelRoomById(Id);
            if (room == null)
            {
                return PageNotFound();
            }
            //chnage Notifications
            var hotel = _reservationService.GetHotelById(room.HID);
            if (hotel.Confirmed || hotel.SendHotel)
            {
                HotelchangedEvents(hotel.ID);
            }

            _reservationService.DeleteHotelRoom(Id);
            _reservationService.InsertFileNote(new Filelog
            {
                Action = "Hotel Room Deleted",
                ref_file = hotel.Reservations.ID,
                ref_member = _workContext.CurrentVisitor.Id,
                shortMessage = hotel.name + ": Hotel Room Deleted, <br/> Price" + room.price + " Dates: " +
                room.checkin.Value.ToShortDateString() + " - " + room.checkout.Value.ToShortDateString()
            });
            //notifiy hotel change
            ClaculateChanges(hotel.ID);
            //calculate reservation
            _reservationService.CalculateReservation(hotel.RID);

            return Json(new { success = true, responseText = "Deleted" }, JsonRequestBehavior.AllowGet);
        }



        public ActionResult ReservationFlights(int Id)
        {
            ViewBag.Cities = new SelectList(Enum.GetNames(typeof(TurkeyCities)), "Istanbul");
            ViewBag.Packages = new SelectList(_configurationService.SearchPackages(0), "name", "name");
            var model = new FlightsViewModel();

            var reservation = _reservationService.GetReservationById(Id);

            model.RID = Id;
            model.Agency = reservation.Agency.AgencyName;
            model.Total = reservation.Flight.Sum(d => d.Total * d.Pax).ToString();
            model.Owner = reservation.Visitor.GetFullName();
            model.Flights = reservation.Flight.ToList();

            return View(model);
        }

        public ActionResult PrepareFlights(int id)
        {
            var Flight = _reservationService.GetFlightById(id);

            if (Flight == null)
            {
                return Content("");
            }

            return Json(new
            {
                TID = Flight.Id,
                code = Flight.name,
                pax = Flight.Pax,
                note = Flight.note,
                date = Flight.date.ToFString(),
                dateout = Flight.dateout.ToFString(),
                buy = Flight.Cost,
                sell = Flight.Total,
                names = Flight.customer
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult EditFlight(FormCollection form)
        {
            var tID = Convert.ToInt32(form["TID"]);
            var Flight = _reservationService.GetFlightById(tID);
            if (Flight == null)
            {
                return PageNotFound();
            }
            Flight.name = form["code"];
            Flight.Pax = Convert.ToInt32(form["pax"]);
            Flight.date = Convert.ToDateTime(form["date"]); // check result
            Flight.dateout = Convert.ToDateTime(form["dateout"]); // check result
            Flight.Cost = Convert.ToDecimal(form["buy"]);
            Flight.Total = Convert.ToDecimal(form["sell"]);
            Flight.note = form["note"];
            Flight.customer = form["names"];

            var result = _reservationService.UpdateFlight(Flight);
            if (result == null)
            {
                return View();
            }

            _reservationService.InsertFileNote(new Filelog
            {
                Action = "Flight Edited",
                ref_file = result.RID,
                ref_member = _workContext.CurrentVisitor.Id,
                shortMessage = "Pax: " + result.Pax + " price: " + result.Total
            });
            //calculate reservation
            _reservationService.CalculateReservation(result.RID);
            return RedirectToAction("ReservationFlights", new { Id = Flight.RID });
        }

        [HttpPost]
        public ActionResult CreateFlight(FormCollection form)
        {
            var package = new Flight();

            package.name = form["package.name"];
            package.Pax = Convert.ToInt32(form["package.pax"]);
            package.date = Convert.ToDateTime(form["package.date"]); // check result
            package.dateout = Convert.ToDateTime(form["package.dateout"]); // check result
            package.Cost = Convert.ToDecimal(form["package.Cost"]);
            package.Total = Convert.ToDecimal(form["package.Total"]);
            package.note = form["package.note"];
            package.customer = form["package.customer"];
            package.RID = Convert.ToInt32(form["RID"]);
            package.SupplierCode = "";
            package.ref_supplier = 1;

            var result = _reservationService.InsertFlight(package);
            if (result == null)
            {
                return View();
            }

            _reservationService.InsertFileNote(new Filelog
            {
                Action = "Flight Inserted",
                ref_file = result.RID,
                ref_member = _workContext.CurrentVisitor.Id,
                shortMessage = "Pax: " + result.Pax + " price: " + result.Total
            });

            //calculate reservation
            _reservationService.CalculateReservation(result.RID);
            return RedirectToAction("ReservationFlights", new { Id = package.RID });
        }

        public ActionResult DeleteFlights(int Id)
        {
            var Package = _reservationService.GetFlightById(Id);
            if (Package == null)
            {
                return PageNotFound();
            }

            _reservationService.DeleteFlight(Id);
            //calculate reservation
            _reservationService.InsertFileNote(new Filelog
            {
                Action = "Flight Deleted",
                ref_file = Package.RID,
                ref_member = _workContext.CurrentVisitor.Id,
                shortMessage = "Pax: " + Package.Pax + " price: " + Package.Total
            });
            _reservationService.CalculateReservation(Package.RID);
            return Json(new { success = true, responseText = "Deleted" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ReservationTransfers(int Id)
        {
            ViewBag.Cities = new SelectList(Enum.GetNames(typeof(TurkeyCities)), "Istanbul");
            var model = new TransfersViewModel();

            var reservation = _reservationService.GetReservationById(Id);

            model.Agency = reservation.Agency.AgencyName;
            model.RID = Id;
            model.Total = reservation.Transfers.Sum(d => d.Total).ToString();
            model.Owner = reservation.Visitor.GetFullName();
            model.Transfers = reservation.Transfers.ToList();
            model.Transfer.customer = reservation.GetCustomers();


            return View(model);
        }

        public ActionResult PrepareTransfer(int id)
        {
            var transfer = _reservationService.GetTransferById(id);

            if (transfer == null)
            {
                return Content("");
            }

            return Json(new
            {
                TID = transfer.ID,
                City = transfer.city,
                code = transfer.flightCode,
                pickup = transfer.pickup,
                dropoff = transfer.dropoff,
                pax = transfer.pax,
                time = transfer.flightTime,
                date = transfer.date.ToFString(),
                buy = transfer.Cost,
                sell = transfer.Total,
                car = transfer.car,
                names = transfer.customer
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult EditTransfer(FormCollection form)
        {
            var tID = Convert.ToInt32(form["TID"]);
            var transfer = _reservationService.GetTransferById(tID);
            if (transfer == null)
            {
                return PageNotFound();
            }
            transfer.city = form["city"];
            transfer.flightCode = form["code"];
            transfer.pickup = form["pickup"];
            transfer.dropoff = form["dropoff"];
            transfer.pax = Convert.ToInt32(form["pax"]);
            transfer.flightTime = form["time"];
            transfer.date = Convert.ToDateTime(form["date"]); // check result
            transfer.Cost = Convert.ToDecimal(form["buy"]);
            transfer.Total = Convert.ToDecimal(form["sell"]);
            transfer.car = form["car"];
            transfer.customer = form["names"];

            var result = _reservationService.UpdateTransfer(transfer);
            if (result == null)
            {
                return View();
            }
            _reservationService.InsertFileNote(new Filelog
            {
                Action = "Transfer Edited",
                ref_file = result.RID,
                ref_member = _workContext.CurrentVisitor.Id,
                shortMessage = "Pax: " + result.pax + " price: " + result.Total
            });
            //calculate reservation
            _reservationService.CalculateReservation(result.RID);
            return RedirectToAction("ReservationTransfers", new { Id = transfer.RID });
        }

        [HttpPost]
        public ActionResult CreateTransfer(FormCollection form)
        {
            var transfer = new Transfers();

            transfer.city = form["Transfer.city"];
            transfer.flightCode = form["Transfer.flightCode"];
            transfer.pickup = form["Transfer.pickup"];
            transfer.dropoff = form["Transfer.dropoff"];
            transfer.pax = Convert.ToInt32(form["Transfer.pax"]);
            transfer.flightTime = form["Transfer.flightTime"];
            transfer.date = Convert.ToDateTime(form["Transfer.date"]); // check result
            transfer.Cost = Convert.ToDecimal(form["Transfer.Cost"]);
            transfer.Total = Convert.ToDecimal(form["Transfer.Total"]);
            transfer.car = form["Transfer.car"];
            transfer.customer = form["Transfer.customer"];
            transfer.RID = Convert.ToInt32(form["RID"]);
            transfer.ref_supplier = 1;

            var result = _reservationService.InsertTransfer(transfer);
            if (result == null)
            {
                return View();
            }
            _reservationService.InsertFileNote(new Filelog
            {
                Action = "Transfer Inserted",
                ref_file = result.RID,
                ref_member = _workContext.CurrentVisitor.Id,
                shortMessage = "Pax: " + result.pax + " price: " + result.Total
            });

            //calculate reservation
            _reservationService.CalculateReservation(result.RID);
            return RedirectToAction("ReservationTransfers", new { Id = transfer.RID });
        }

        public ActionResult DeleteTransfer(int Id)
        {
            var transfer = _reservationService.GetTransferById(Id);
            if (transfer == null)
            {
                return PageNotFound();
            }

            _reservationService.DeleteTransfer(Id);

            _reservationService.InsertFileNote(new Filelog
            {
                Action = "Transfer Deleted",
                ref_file = transfer.RID,
                ref_member = _workContext.CurrentVisitor.Id,
                shortMessage = "Pax: " + transfer.pax + " price: " + transfer.Total
            });
            //calculate reservation
            _reservationService.CalculateReservation(transfer.RID);
            return Json(new { success = true, responseText = "Deleted" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteHotel(int Id)
        {
            var hotel = _reservationService.GetHotelById(Id);
            if (hotel == null)
            {
                return PageNotFound();
            }

            _reservationService.DeleteHotel(Id);
            //calculate reservation
            _reservationService.CalculateReservation(hotel.RID);
            return Json(new { success = true, responseText = "Deleted" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ReservationTours(int Id)
        {
            ViewBag.Cities = new SelectList(Enum.GetNames(typeof(TurkeyCities)), "Istanbul");
            ViewBag.Tours = new SelectList(_configurationService.SearchTours(0), "name", "name");
            ViewBag.Suppliers = new SelectList(_configurationService.SearchSupplier(0));

            var model = new ToursViewModel();

            var reservation = _reservationService.GetReservationById(Id);

            model.Agency = reservation.Agency.AgencyName;
            model.RID = Id;
            model.Total = reservation.Tour.Sum(d => d.Total * d.Pax).ToString();
            model.Owner = reservation.Visitor.GetFullName();
            model.Tours = reservation.Tour.ToList();
            model.Tour.customer = reservation.GetCustomers();
            return View(model);
        }

        public ActionResult PrepareTour(int id)
        {
            var tour = _reservationService.GetTourById(id);

            if (tour == null)
            {
                return Content("");
            }

            return Json(new
            {
                TID = tour.ID,
                City = tour.city,
                code = tour.name,
                pickup = tour.pickup,
                dropoff = tour.dropoff,
                pax = tour.Pax,
                time = tour.pickuptime,
                date = tour.date.ToFString(),
                buy = tour.Cost,
                sell = tour.Total,
                car = tour.car,
                names = tour.customer
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult EditTour(FormCollection form)
        {
            var tID = Convert.ToInt32(form["TID"]);
            var tour = _reservationService.GetTourById(tID);
            if (tour == null)
            {
                return PageNotFound();
            }
            tour.city = form["city"];
            tour.name = form["code"];
            tour.pickup = form["pickup"];
            tour.dropoff = form["dropoff"];
            tour.Pax = Convert.ToInt32(form["pax"]);
            tour.pickuptime = form["time"];
            tour.date = Convert.ToDateTime(form["date"]); // check result
            tour.Cost = Convert.ToDecimal(form["buy"]);
            tour.Total = Convert.ToDecimal(form["sell"]);
            tour.car = form["car"];
            tour.customer = form["names"];

            var result = _reservationService.UpdateTour(tour);
            if (result == null)
            {
                return View();
            }

            _reservationService.InsertFileNote(new Filelog
            {
                Action = "Tour Edited",
                ref_file = result.RID,
                ref_member = _workContext.CurrentVisitor.Id,
                shortMessage = "Pax: " + result.Pax + " price: " + result.Total
            });
            //calculate reservation
            _reservationService.CalculateReservation(result.RID);
            return RedirectToAction("ReservationTours", new { Id = tour.RID });
        }

        [HttpPost]
        public ActionResult CreateTour(FormCollection form)
        {
            var tour = new Tour();

            tour.city = form["Tour.city"];
            tour.name = form["Tour.name"];
            tour.pickup = form["Tour.pickup"];
            tour.dropoff = form["Tour.dropoff"];
            tour.Pax = Convert.ToInt32(form["Tour.pax"]);
            tour.pickuptime = form["Tour.flightTime"];
            tour.date = Convert.ToDateTime(form["Tour.date"]); // check result
            tour.Cost = Convert.ToDecimal(form["Tour.Cost"]);
            tour.Total = Convert.ToDecimal(form["Tour.Total"]);
            tour.car = form["Tour.car"];
            tour.customer = form["Tour.customer"];
            tour.RID = Convert.ToInt32(form["RID"]);
            tour.ref_supplier = 1;

            var result = _reservationService.InsertTour(tour);
            if (result == null)
            {
                return View();
            }
            _reservationService.InsertFileNote(new Filelog
            {
                Action = "Tour Inserted",
                ref_file = result.RID,
                ref_member = _workContext.CurrentVisitor.Id,
                shortMessage = "Pax: " + result.Pax + " price: " + result.Total
            });
            //calculate reservation
            _reservationService.CalculateReservation(result.RID);

            return RedirectToAction("ReservationTours", new { Id = tour.RID });
        }

        public ActionResult DeleteTour(int Id)
        {
            var tour = _reservationService.GetTourById(Id);
            if (tour == null)
            {
                return PageNotFound();
            }

            _reservationService.DeleteTour(Id);

            _reservationService.InsertFileNote(new Filelog
            {
                Action = "Tour Deleted",
                ref_file = tour.RID,
                ref_member = _workContext.CurrentVisitor.Id,
                shortMessage = "Pax: " + tour.Pax + " price: " + tour.Total
            });
            //calculate reservation
            _reservationService.CalculateReservation(tour.RID);
            return Json(new { success = true, responseText = "Deleted" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ReservationPackages(int Id)
        {
            ViewBag.Cities = new SelectList(Enum.GetNames(typeof(TurkeyCities)), "Istanbul");
            ViewBag.Packages = new SelectList(_configurationService.SearchPackages(0), "name", "name");
            var model = new PackagesViewModel();

            var reservation = _reservationService.GetReservationById(Id);

            model.Agency = reservation.Agency.AgencyName;
            model.RID = Id;
            model.Total = reservation.ExtraService.Sum(d => d.Total * d.Pax).ToString();
            model.Owner = reservation.Visitor.GetFullName();
            model.Packages = reservation.ExtraService.ToList();
            model.Package.customer = reservation.GetCustomers();

            return View(model);
        }

        public ActionResult PreparePackage(int id)
        {
            var tour = _reservationService.GetPackageById(id);

            if (tour == null)
            {
                return Content("");
            }

            return Json(new
            {
                TID = tour.ID,
                code = tour.name,
                pax = tour.Pax,
                note = tour.note,
                date = tour.date.ToFString(),
                dateout = tour.dateout.ToFString(),
                buy = tour.Cost,
                sell = tour.Total,
                names = tour.customer
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult EditPackage(FormCollection form)
        {
            var tID = Convert.ToInt32(form["TID"]);
            var package = _reservationService.GetPackageById(tID);
            if (package == null)
            {
                return PageNotFound();
            }
            package.name = form["code"];
            package.Pax = Convert.ToInt32(form["pax"]);
            package.date = Convert.ToDateTime(form["date"]); // check result
            package.dateout = Convert.ToDateTime(form["dateout"]); // check result
            package.Cost = Convert.ToDecimal(form["buy"]);
            package.Total = Convert.ToDecimal(form["sell"]);
            package.note = form["note"];
            package.customer = form["names"];

            var result = _reservationService.UpdatePackage(package);
            if (result == null)
            {
                return View();
            }

            _reservationService.InsertFileNote(new Filelog
            {
                Action = "Package Edited",
                ref_file = result.RID,
                ref_member = _workContext.CurrentVisitor.Id,
                shortMessage = "Pax: " + result.Pax + " price: " + result.Total
            });
            //calculate reservation
            _reservationService.CalculateReservation(result.RID);
            return RedirectToAction("ReservationPackages", new { Id = package.RID });
        }

        [HttpPost]
        public ActionResult CreatePackage(FormCollection form)
        {
            var package = new ExtraService();

            package.name = form["package.name"];
            package.Pax = Convert.ToInt32(form["package.pax"]);
            package.date = Convert.ToDateTime(form["package.date"]); // check result
            package.dateout = Convert.ToDateTime(form["package.dateout"]); // check result
            package.Cost = Convert.ToDecimal(form["package.Cost"]);
            package.Total = Convert.ToDecimal(form["package.Total"]);
            package.note = form["package.note"];
            package.customer = form["package.customer"];
            package.RID = Convert.ToInt32(form["RID"]);
            package.ref_supplier = 1;

            var result = _reservationService.InsertPackage(package);
            if (result == null)
            {
                return View();
            }

            _reservationService.InsertFileNote(new Filelog
            {
                Action = "Package Inserted",
                ref_file = result.RID,
                ref_member = _workContext.CurrentVisitor.Id,
                shortMessage = "Pax: " + result.Pax + " price: " + result.Total
            });

            //calculate reservation
            _reservationService.CalculateReservation(result.RID);
            return RedirectToAction("ReservationPackages", new { Id = package.RID });
        }

        public ActionResult DeletePackage(int Id)
        {
            var Package = _reservationService.GetPackageById(Id);
            if (Package == null)
            {
                return PageNotFound();
            }

            _reservationService.DeletePackage(Id);
            //calculate reservation
            _reservationService.InsertFileNote(new Filelog
            {
                Action = "Package Deleted",
                ref_file = Package.RID,
                ref_member = _workContext.CurrentVisitor.Id,
                shortMessage = "Pax: " + Package.Pax + " price: " + Package.Total
            });
            _reservationService.CalculateReservation(Package.RID);
            return Json(new { success = true, responseText = "Deleted" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Dashboard()
        {
            var SearchModel = new ReservationSearchModel();
            SearchModel.CreateDate = DateTime.Today;
            SearchModel.CreateDateTo = DateTime.Today.AddDays(1);
            SearchModel.Status.Add(ReservationStatus.Request.ToString());
            SearchModel.Status.Add(ReservationStatus.Confirmed.ToString());
            SearchModel.Status.Add(ReservationStatus.ReConfirmed.ToString());
            SearchModel.Status.Add(ReservationStatus.Canceled.ToString());

            ViewBag.onlyEngin = false;
            if (_workContext.IsAdmin && _workContext.CurrentVisitor.email.ToLower().Contains("engin"))
            {
                ViewBag.onlyEngin = true;
            }
            //if (!_workContext.IsAdmin)
            //{
            //    SearchModel.CreatedBy.Add(_workContext.CurrentVisitor.Id);
            //}
            var results = _reservationService.SearchReservations(SearchModel, false);

            ViewBag.dashtotal = new DashTotal
            {
                Request = _workContext.CurrentVisitor.MReservations.Count(d=> d.status == ReservationStatus.Request.ToString()),
                Confirmed = _workContext.CurrentVisitor.MReservations.Count(d=> d.status == ReservationStatus.Confirmed.ToString()),
                Reconfirmed = _workContext.CurrentVisitor.MReservations.Count(d=> d.status == ReservationStatus.ReConfirmed.ToString()),
                Canceled = _workContext.CurrentVisitor.MReservations.Count(d=> d.status == ReservationStatus.Canceled.ToString()),
                Total =_workContext.CurrentVisitor.MReservations.Count
            };

            var Locked = _workContext.IsAdmin || _workContext.IsAccounts;
            ViewBag.Locked = Locked;

            SearchModel = new ReservationSearchModel();
            SearchModel.Status.Add(ReservationStatus.Locked.ToString());
            if (!(_workContext.IsAdmin || _workContext.IsAccounts))
            {
                SearchModel.CreatedBy.Add(_workContext.CurrentVisitor.Id);
            }
            ViewBag.LockedRes = _reservationService.SearchReservations(SearchModel, false);


            return View(results);
        }

        public ActionResult UserFiles(string query,int page)
        {
            var SearchModel = new ReservationSearchModel();
            SearchModel.CreateDate = DateTime.MinValue;
            SearchModel.CreateDateTo = DateTime.MaxValue;


            ReservationStatus myStatus;
            if (Enum.TryParse(query, out myStatus))
            {
                switch (myStatus)
                {
                    case ReservationStatus.Request:
                        SearchModel.Status.Add(ReservationStatus.Request.ToString());
                        break;
                    case ReservationStatus.Confirmed:
                        SearchModel.Status.Add(ReservationStatus.Confirmed.ToString());
                        break;
                    case ReservationStatus.ReConfirmed:
                        SearchModel.Status.Add(ReservationStatus.ReConfirmed.ToString());
                        break;
                    case ReservationStatus.Canceled:
                        SearchModel.Status.Add(ReservationStatus.Canceled.ToString());
                        break;
                    default:
                        SearchModel.Status.Add(ReservationStatus.Request.ToString());
                        SearchModel.Status.Add(ReservationStatus.Confirmed.ToString());
                        SearchModel.Status.Add(ReservationStatus.ReConfirmed.ToString());
                        SearchModel.Status.Add(ReservationStatus.Canceled.ToString());
                        break;
                }
            }

            SearchModel.CreatedBy.Add(_workContext.CurrentVisitor.Id);
            
            var results = _reservationService.SearchReservations(SearchModel, false);

            return View(results.OrderByDescending(d => d.ID).ToPagedList(page, 50));
        }
    }
}