using Line.Models;
using Line.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Line.Controllers
{
    public class ReportsController : AdminController
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

        public ReportsController(ILanguageService languageService, ILocalizationService localizationService,
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


        // GET: Admin/Reports
        public ActionResult Dashboard()
        {
            //if (_reportsService.Getlastyear() < DateTime.Now.Year)
            //{
            //    if (_reportsService.Getlastyear() + 1 == DateTime.Now.Year)
            //    {

            //    }
            //    else
            //    {
            //        for (int i = _reportsService.Getlastyear(); i < DateTime.Now.Year - 1; i++)
            //        {
            //            _reportsService.UpdateReportTable(i + 1);
            //        }
            //    }
            //}

            var Search = new ReservationSearchModel();
            //get last 2 years,
            Search.CheckinDatefrom = new DateTime(DateTime.Now.Year - 1, 1, 1);
            Search.CheckinDateto = new DateTime(DateTime.Now.Year + 1, 1, 1);
            var Reservations = _reservationService.SearchReservations(Search, true);

            DashboardViewModel Model = new DashboardViewModel();

            Model.Sales = _reportsService.GetSalesByRegistrations(Reservations);
            Model.Years = _reportsService.GetYearsDashboard();

            Model.Agencies = _reportsService.GetAgencyReport(DateTime.Now.Year, Reservations);
            Model.AgenciesLastYear = _reportsService.GetAgencyReport(DateTime.Now.Year - 1, Reservations);


            var countries = _reportsService.GetCountriesReport(Reservations);
            Model.Countries = countries.Where(d => d.Year == DateTime.Now.Year).OrderByDescending(v => v.Sales).Take(15).ToList();
            Model.CountriesLastYear = countries.Where(d => d.Year == DateTime.Now.Year - 1).OrderByDescending(v => v.Sales).Take(15).ToList();

            Model.Hotels = _reportsService.GetHotelsReport(DateTime.Now.Year, Reservations);
            Model.HotelsLastYear = _reportsService.GetHotelsReport(DateTime.Now.Year - 1, Reservations);

            return View(Model);
        }


        public ActionResult Sales()
        {
            
            ViewBag.CountChart = _reportsService.GetSalesFilesCounters(DateTime.Now.Year);

            ViewBag.SalesChart = _reportsService.GetSales(DateTime.Now.Year - 1);

            var Model = _reportsService.GetSales(DateTime.Now.AddYears(-4).Year);

            return View(Model);
        }

        public ActionResult Profit()
        {
            ViewBag.CountChart = _reportsService.GetSalesFilesCounters(DateTime.Now.Year);

            ViewBag.SalesChart = _reportsService.GetProfits(DateTime.Now.Year - 1);

            var Model = _reportsService.GetProfits(DateTime.Now.AddYears(-4).Year);

            return View(Model);
        }

        public ActionResult Countries()
        {

            ViewBag.Countries = _configurationService.GetUsedCountries("");

            var Search = new ReservationSearchModel();
            //get last 4 years,
            Search.CheckinDatefrom = new DateTime(DateTime.Now.Year - 3, 1, 1);
            Search.CheckinDateto = new DateTime(DateTime.Now.Year + 1, 1, 1);
            var Reservations = _reservationService.SearchReservations(Search, true);


            var Model = Reservations.GroupBy(d => new { d.Agency.Country, d.balanceDate.Year })
            .Select(x => new ShortReport
            {
                Year = x.Key.Year,
                Name = x.Key.Country,
                Count = x.Count(),
                Sales = x.Sum(c => c.price)
            });
           

            return View(Model);
        }

        public ActionResult Agencies()
        {
            var country = Request.QueryString["country"];
            if (string.IsNullOrEmpty(country))
            {
                return RedirectToAction("Countries");
            }
            //ViewBag.Counters = _reportsService.GetCountryFilesCounters(DateTime.Now.Year, country);
            var Model = _reportsService.GetAgencyReportByCountry(DateTime.Now.Year - 4, country);
            return View(Model);

        }

        public ActionResult Agency()
        {
            var Id = Request.QueryString["AId"];
            var agency = _configurationService.GetAgencyById(Convert.ToInt32(Id));
            if (agency != null)
            {
                ViewBag.Counters = _reportsService.GetAgencyFilesCounters(DateTime.Now.Year, Convert.ToInt32(Id));
                var Model = _reportsService.GetAgencyReportById(2015, Convert.ToInt32(Id));
                ViewBag.Name = agency.AgencyName;
                return View(Model);

            }

            return RedirectToAction("Countries");


        }

        public ActionResult Member(int Id)
        {
            ViewBag.Counters = _reportsService.GetStaffFilesCounters(DateTime.Now.Year, Id);
            var Model = _reportsService.GetAgencyReportByMemberId(2015, Id);
            return View(Model);
        }

        public ActionResult Hotels()
        {
            //ViewBag.Graph = _reportsService.GetCountriesReport(DateTime.Now.Year);
            var Model = new List<ShortReport>();
            for (int i = DateTime.Now.Year - 1; i <= DateTime.Now.Year; i++)
            {
                Model.AddRange(
                    _reportsService.GetHotelsReport(i)
                );
            }
            return View(Model);

        }

        public ActionResult staff()
        {
            ViewBag.CountChart = _reportsService.GetSalesFilesCounters(DateTime.Now.Year);
            ViewBag.SalesChart = _reportsService.GetSales(DateTime.Now.Year-1);

            var Search = new ReservationSearchModel();
            //get Current year,
            Search.CreateDate = new DateTime(DateTime.Now.Year, 1, 1);
            Search.CreateDateTo = new DateTime(DateTime.Now.Year + 1, 1, 1);
            var Reservations = _reservationService.SearchReservations(Search, true);
            var model = _reportsService.GetSalesByStaff(Reservations);
            return View(model);
        }

        public ActionResult AgenciesInfo(string country = "")
        {
            ViewBag.Countries = _configurationService.GetUsedCountries(country);
            var List = _configurationService.SearchAgencies(0);
            if (country != null)
            {
                List = List.Where(d => d.Country == country).ToList();
            }
            return View(List);
        }

        public ActionResult DeptsCountries(string name = "")
        {
            return View();
        }

        public ActionResult DeptAgencies(int Id = 0)
        {
            return View();
        }

    }


}