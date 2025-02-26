using Line.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Line.Integration
{
    public class DataCopier
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

        public DataCopier(ILanguageService languageService, ILocalizationService localizationService,
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

        public void CopyData()
        {
          
           
            using (ReservationsEntities db = new ReservationsEntities())
            {
                //get agency data



            }


            
        }
    }
}
