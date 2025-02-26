using Line.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Line.Web.Controllers
{
    public class OutController : Controller
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

        public OutController(ILanguageService languageService, ILocalizationService localizationService,
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
        // GET: Out
        public ActionResult Invoice(int Id = 0, int eref = 0)
        {
            var res = _reservationService.GetReservationById(Id);
            if (res != null && res.ref_agency.Value == eref)
            {
                var setting = _configurationService.GetSettings();
                ViewBag.header = _uploadService.GetPicture(setting.InvoiceHeader);
                ViewBag.footer = _uploadService.GetPicture(setting.InvoiceFooter);
                ViewBag.Settings = (setting);
                return View(res);
            }
            else
            {
                return RedirectToAction("login", "Account");
            }
        }

        public ActionResult Voucher(int Id = 0, int eref = 0)
        {
            var res = _reservationService.GetReservationById(Id);
            if (res != null && res.ref_agency.Value == eref)
            {
                var setting = _configurationService.GetSettings();
                ViewBag.header = _uploadService.GetPicture(setting.VoucherHeader);
                ViewBag.footer = _uploadService.GetPicture(setting.VoucherFooter);
                return View(res);
            }
            else
            {
                return RedirectToAction("login", "Account");
            }
        }
    }
}