using Line.Models;
using Line.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using Line.Helpers;

namespace Line.Controllers
{
    public class AccountingController : AdminController
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

        public AccountingController(ILanguageService languageService, ILocalizationService localizationService,
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
        // GET: Admin/Accounting


        public ActionResult Agencies(int Id = 0)
        {
            var Model = new AgencyViewModel();
            Model.Agency = _configurationService.GetAgencyById(Id);
            if (Model.Agency != null)
            {
                ViewBag.Agencies = new List<SelectListItem> { new SelectListItem { Text = Model.Agency.AgencyName, Value = Model.Id.ToString(), Selected = true } };
                var SearchModel = new ReservationSearchModel();
                SearchModel.CheckoutDatefrom = Model.CheckOutFrom;
                SearchModel.CheckoutDateto = Model.CheckOutTo;
                SearchModel.AgencyId = new List<int> { Model.Id };
                var result = _reservationService.SearchReservations(SearchModel, true).ToList();
                Model.Files = result;
            }
            else
            {
                ViewBag.Agencies = new List<SelectListItem>();
            }
            return View(Model);
        }

        public ActionResult DownloadExcel(int Id = 0)
        {
            var Model = _reportsService.GetDebtReport("");
            Export export = new Export();
            export.ToExcel(Response, Model,"Debts");

            return Content("");
        }

        public ActionResult DownloadAgencyExcel(int Id = 0)
        {

            var Model = new AgencyViewModel();
            var SearchModel = new ReservationSearchModel();

            Model.CheckOutFrom = DateTime.MinValue;
            Model.CheckOutTo = DateTime.MaxValue;

            SearchModel.AgencyId = new List<int> { Id };
            var result = _reservationService.SearchReservations(SearchModel, true).ToList();

            var ViewMode = result.OrderByDescending(d => d.ID).Select(d =>
            new
            {
                Id = d.ID,
                Voucher = d.vocher,
                Dates = d.checkindate.ToShortDateString() + " - " + d.balanceDate.ToShortDateString(),
                Guest = d.GetCustomers(),
                Debt = d.price,
                Payment = d.Payments.Sum(x => x.Payment),
                Balance = d.price - d.Payments.Sum(x => x.Payment)
            });

            var agency = _configurationService.GetAgencyById(Id);
            Export export = new Export();
            export.ToExcel(Response, ViewMode, agency.AgencyName);

            return Content("");
        }

        public ActionResult Statement(int Id, string Statement)
        {
            var model = new StatmentViewModel();
            var agency = _configurationService.GetAgencyById(Id);
            model.Id = agency.ID;
            model.City = agency.City;
            model.Country = agency.Country;
            model.name = agency.AgencyName;

            model.Statment = Statement.ToUpper();

            if (Statement == "A") //unpaid
            {
                var SearchModel = new ReservationSearchModel { AgencyId = new List<int> { model.Id } };

                var files = _reservationService.SearchReservations(SearchModel, true).Where(d => d.balance != 0);

                model.Files = files.Select(x => new DeptDateViewModel { Id = x.ID, name = x.vocher, Country = x.Agency.Country, City = x.Agency.City, DebtValue = x.price, BalanceValue = x.balance, PaymentsValue = x.Payments.Sum(p => p.Payment), Cutomer = x.GetCustomers(), date = x.checkindate.ToString("dd.MM.yyyy") + " - " + x.balanceDate.ToString("dd.MM.yyyy") }).ToList();

            }
            if (Statement == "B") //passed
            {
                var SearchModel = new ReservationSearchModel { AgencyId = new List<int> { model.Id } };
                SearchModel.CheckoutDateto = DateTime.Today.Date;
                SearchModel.CheckoutDatefrom = DateTime.MinValue.AddDays(1);

                var files = _reservationService.SearchReservations(SearchModel, true).Where(d => d.balance != 0);

                model.Files = files.Select(x => new DeptDateViewModel { Id = x.ID, name = x.vocher, Country = x.Agency.Country, City = x.Agency.City, DebtValue = x.price, BalanceValue = x.balance, PaymentsValue = x.Payments.Sum(p => p.Payment), Cutomer = x.GetCustomers(), date = x.checkindate.ToString("dd.MM.yyyy") + " - " + x.balanceDate.ToString("dd.MM.yyyy") }).ToList();

            }
            if (Statement == "C") //future
            {
                var SearchModel = new ReservationSearchModel { AgencyId = new List<int> { model.Id } };
                SearchModel.CheckoutDatefrom = DateTime.Today.AddDays(1).Date;
                SearchModel.CheckoutDateto = DateTime.MaxValue;

                var files = _reservationService.SearchReservations(SearchModel, true).Where(d => d.balance != 0);

                model.Files = files.Select(x => new DeptDateViewModel { Id = x.ID, name = x.vocher, Country = x.Agency.Country, City = x.Agency.City, DebtValue = x.price, BalanceValue = x.balance, PaymentsValue = x.Payments.Sum(p => p.Payment), Cutomer = x.GetCustomers(), date = x.checkindate.ToString("dd.MM.yyyy") + " - " + x.balanceDate.ToString("dd.MM.yyyy") }).ToList();

            }

            if (agency.Country.ToLower().Contains("kuwait") || agency.Country.ToLower().Contains("kw")
                || agency.Country.ToLower().Contains("lebanon")
                || agency.Country.ToLower().Contains("lb")

                )
            {
                return View("Statement2", model);
            }
            else
            {
                return View(model);
            }

        }
        [HttpPost]
        public ActionResult Agencies(AgencyViewModel Model)
        {
            return RedirectToAction("Agency", new { Id = Model.Id });
        }

        public ActionResult Agency(int Id, AgencyViewModel Model, int page = 1)
        {

            var SearchModel = new ReservationSearchModel();

            if (Request.HttpMethod == "POST") { page = 1; }
            if (Model.CheckOutFrom == Model.CheckOutTo) { Model.CheckOutFrom = DateTime.Now.AddMonths(-1); Model.CheckOutTo = DateTime.Now; }

            if (Model.Showpayment)
            {
                SearchModel.CheckoutDatefrom = Model.CheckOutFrom;
                SearchModel.CheckoutDateto = Model.CheckOutTo;
            }
            else
            {
                if (Model.FileDate == 0)
                {//future

                    SearchModel.CheckoutDatefrom = DateTime.Now.Date;
                    SearchModel.CheckoutDateto = DateTime.MaxValue;
                }
                else if (Model.FileDate == 1) // old
                {
                    SearchModel.CheckoutDatefrom = DateTime.MinValue;
                    SearchModel.CheckoutDateto = DateTime.Now.Date;
                }
            }

            SearchModel.AgencyId = new List<int> { Id };

            if (Model.FileStatus == 1)
            {
                SearchModel.OnlyPaidFiles = true;
            }
            else if (Model.FileStatus == 0)
            {
                SearchModel.OnlyUnpaidFiles = true;
            }

            var result = _reservationService.SearchReservations(SearchModel, false);


            var canceled = result.Where(d => d.status == ReservationStatus.Canceled.ToString()).ToList();
            result = result.Where(d => d.status == ReservationStatus.Confirmed.ToString() || d.status == ReservationStatus.ReConfirmed.ToString()).ToList();
            Model.Agency = _configurationService.GetAgencyById(Id);
            ViewBag.Canceled = canceled;

            ViewBag.Filter = Model;
            var income = Model.Agency.AdvancePayment.Where(d => !d.PayOut).Sum(d => d.Payment);
            var Outcome = Model.Agency.AdvancePayment.Where(d => d.PayOut).Sum(d => d.Payment);
            ViewBag.advancePayments = income - Outcome;

            return View(result.OrderByDescending(d => d.balanceDate).ToPagedList(page, PageSize));

        }
        public ActionResult Debts(int page = 1)
        {
            var Model = new ReservationSearchModel();
            Model.CheckoutDateto = DateTime.Today.Date;
            Model.CheckoutDatefrom = DateTime.MinValue.AddDays(1);
            Model.OnlyUnpaidFiles = true;
            var result = _reservationService.SearchReservations(Model, true);
            var Grouped = result.GroupBy(d => d.Agency.AgencyName).Select(x => new GroupDeptViewModel { Id = x.FirstOrDefault().Agency.ID, name = x.Key, Country = x.FirstOrDefault().Agency.Country, City = x.FirstOrDefault().Agency.City, DebtValue = x.Sum(d => d.price), BalanceValue = x.Sum(d => d.balance), count = x.Count() });
            return View(Grouped.OrderByDescending(d => d.BalanceValue).ToPagedList(page, PageSize));
        }
        public ActionResult Files(int page = 1)
        {
            var Model = new ReservationSearchModel();
            Model.CheckoutDateto = DateTime.Today.Date;
            Model.CheckoutDatefrom = DateTime.MinValue.AddDays(1);
            Model.OnlyUnpaidFiles = true;
            var result = _reservationService.SearchReservations(Model, true);
            var viewModel = result.Select(x => new DeptViewModel { Id = x.ID, name = x.Agency.AgencyName, Country = x.Agency.Country, City = x.Agency.City, DebtValue = x.price, BalanceValue = x.balance, PaymentsValue = x.Payments.Sum(p => p.Payment), Cutomer = x.GetCustomers(), date = x.balanceDate });
            return View(viewModel.OrderByDescending(d => d.date).ToPagedList(page, PageSize));
        }

        public ActionResult Payments(GenerecSearchModel SearchModel, int Id = 0, int page = 1)
        {
            var agency = _configurationService.GetAgencyById(Id);
            if (Request.HttpMethod == "POST") { page = 1; }
            if (agency != null)
            {
                ViewBag.Agencies = new List<SelectListItem> { new SelectListItem { Text = agency.AgencyName, Value = agency.ID.ToString(), Selected = true } };
                ViewBag.Agency = agency.AgencyName;
                ViewBag.AgencyId = agency.ID;
            }
            else
            {
                ViewBag.Agencies = new List<SelectListItem>();
            }

            if (SearchModel.Datefrom == SearchModel.Dateto) { SearchModel.Datefrom = DateTime.Now.AddMonths(-1); SearchModel.Dateto = DateTime.Now; }
            ViewBag.Filter = SearchModel;
            SearchModel.AgencyId.Add(Id);

            var Model = _accountingService.SearchPayment(SearchModel);

            return View(Model.ToPagedList(page, PageSize));
        }

        public ActionResult AdvancePayments(int Id, GenerecSearchModel SearchModel, int page = 1)
        {
            var agency = _configurationService.GetAgencyById(Id);
            if (agency == null)
            {
                return View();
            }

            ViewBag.Filter = SearchModel;
            ViewBag.Agency = agency.AgencyName;
            ViewBag.AgencyId = agency.ID;

            SearchModel.AgencyId.Add(Id);
            var Model = _accountingService.SearchAdvancePayments(SearchModel);

            return View(Model.ToPagedList(page, PageSize));
        }
        public ActionResult Payment(PaymentViewModel Model)
        {

            if (Model.type == "Balance")
            {
                var file = _reservationService.GetReservationById(Model.AId);
                _accountingService.InsertAdvancePayment(new Data.AdvancePayments
                {
                    Payment = (Model.PaymentsValue * -1),
                    Currency = "",
                    date = Convert.ToDateTime(Model.date),
                    type = Model.type,
                    note = "Closing File:" + Model.AId + " " + Model.description,
                    Member = _workContext.CurrentVisitor.GetFullName(),
                    PayOut = true,
                    ref_Agency = file.ref_agency.Value
                });
            }

            decimal rate = 0;
            var date = Date.GetDateTime(Model.date);
            //get xml bank price
            try
            {
                string srate = shared.getxmlprice(date, "EUR");
                rate = Convert.ToDecimal(srate);
            }
            catch (Exception ex)
            {
                ;
            }
            _accountingService.InsertPayment(new Data.Payments
            {
                Payment = Model.PaymentsValue,
                Currency = rate.ToString(),
                date = Date.GetDateTime(Model.date),
                RID = Model.AId,
                type = Model.type,
                note = Model.description,
                Member = _workContext.CurrentVisitor.GetFullName(),
                TotalPrice = Model.PaymentsValue * rate

            });

            //calculate balance
            _reservationService.CalculateReservation(Model.AId, false);

            return RedirectToAction("agency", new { Id = Model.Id });
        }
        public ActionResult AdvancePayment(PaymentViewModel Model)
        {

            var ads = _accountingService.InsertAdvancePayment(new Data.AdvancePayments
            {
                Payment = Model.PaymentsValue,
                Currency = "",
                date = Date.GetDateTime(Model.date),
                ref_Agency = Model.Id,
                type = Model.type,
                note = Model.description,
                Member = _workContext.CurrentVisitor.GetFullName()
            });

            if (!string.IsNullOrEmpty(Request.Form["autoclose"]))
            {
                var SearchModel = new ReservationSearchModel();
                var AgencyModel = new AgencyViewModel();
                SearchModel.OnlyUnpaidFiles = true;
                SearchModel.CheckoutDatefrom = DateTime.MinValue;
                SearchModel.CheckoutDateto = DateTime.Now.Date;
                SearchModel.AgencyId = new List<int> { ads.ref_Agency };

                var result = _reservationService.SearchReservations(SearchModel, true).ToList();

                var remainTotal = Model.PaymentsValue;
                foreach (var item in result)
                {
                    decimal rate = 0;
                    var date = Date.GetDateTime(Model.date);
                    //get xml bank price
                    try
                    {
                        string srate = shared.getxmlprice(date, "EUR");
                        rate = Convert.ToDecimal(srate);
                    }
                    catch (Exception ex)
                    {
                        ;
                    }
                    if (item.balance <= remainTotal)
                    {
                        remainTotal = remainTotal - item.balance;
                        _accountingService.InsertPayment(new Data.Payments
                        {
                            Payment = item.balance,
                            Currency = rate.ToString(),
                            date = Date.GetDateTime(Model.date),
                            RID = item.ID,
                            type = Model.type,
                            note = "Advance Payment ID:" + ads.ID + " " + Model.description,
                            Member = _workContext.CurrentVisitor.GetFullName(),
                            TotalPrice = (rate * item.balance),
                        });

                        _accountingService.InsertAdvancePayment(new Data.AdvancePayments
                        {
                            Payment = (item.balance * -1),
                            Currency = "",
                            date = Convert.ToDateTime(Model.date),
                            type = Model.type,
                            note = "Closing File:" + item.ID + " " + Model.description,
                            Member = _workContext.CurrentVisitor.GetFullName(),
                            PayOut = true,
                            ref_Agency = item.ref_agency.Value
                        });
                    }
                }

            }
            return RedirectToAction("AdvancePayments", new { Id = Model.Id });
        }

        public ActionResult DeletePayment(int Id)
        {
            var payment = _accountingService.GetPaymentById(Id);
            if (payment != null)
            {
                _accountingService.DeletePayment(Id);

                //calculate the balance
                _reservationService.CalculateReservation(payment.RID, false);
               
            }
            return Json(new { success = true, responseText = "Deleted" }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeleteAdvancePayment(int Id)
        {
            var ad = _accountingService.GetAdvancePaymentById(Id);
            _accountingService.DeleteAdvancePayment(Id);

            return RedirectToAction("AdvancePayments", new { Id = ad.ref_Agency });
        }
        public ActionResult FutureFiles(int page = 1)
        {
            var Model = new ReservationSearchModel();
            Model.CheckoutDatefrom = DateTime.Today.AddDays(1).Date;
            Model.CheckoutDateto = DateTime.MaxValue;
            Model.OnlyUnpaidFiles = true;
            var result = _reservationService.SearchReservations(Model, true);
            var viewModel = result.Select(x => new DeptViewModel { Id = x.ID, name = x.Agency.AgencyName, Country = x.Agency.Country, City = x.Agency.City, DebtValue = x.price, BalanceValue = x.balance, PaymentsValue = x.Payments.Sum(p => p.Payment), Cutomer = x.GetCustomers(), date = x.balanceDate });
            return View(viewModel.OrderBy(d => d.date).ToPagedList(page, PageSize));
        }
        public ActionResult FutureDebts(int page = 1)
        {
            var Model = new ReservationSearchModel();
            Model.CheckoutDatefrom = DateTime.Today.AddDays(1).Date;
            Model.CheckoutDateto = DateTime.MaxValue;
            Model.OnlyUnpaidFiles = true;
            var result = _reservationService.SearchReservations(Model, true);
            var Grouped = result.GroupBy(d => d.Agency.AgencyName).Select(x => new GroupDeptViewModel { Id = x.FirstOrDefault().Agency.ID, name = x.Key, Country = x.FirstOrDefault().Agency.Country, City = x.FirstOrDefault().Agency.City, DebtValue = x.Sum(d => d.price), BalanceValue = x.Sum(d => d.balance), count = x.Count() });
            return View(Grouped.OrderByDescending(d => d.BalanceValue).ToPagedList(page, PageSize));
        }

        public ActionResult Report(string name = "")
        {
            var Model = _reportsService.GetDebtReport(name);

            ViewBag.Countries = _configurationService.GetUsedCountries(name);

            return View(Model);
        }
    }
}