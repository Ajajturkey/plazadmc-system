using Line.Data;
using Line.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Line.Services
{
    public partial interface IReportsService
    {
        int Getlastyear();

        List<Unpaid> SearchUnpaidFiles(int FileId);
        Unpaid GetUnpaidFileById(int id);
        Unpaid InsertUnpaidFile(Unpaid value);
        Unpaid UpdateUnpaidFile(Unpaid value);
        void DeleteUnpaidFile(int id);

        List<Report> SearchReportBases(int FileId);
        Report GetReportBaseById(int id);
        Report InsertReportBase(Report value);
        Report UpdateReportBase(Report value);
        void DeleteReportBase(int id);

        void UpdateReportTable(int Year);

        List<MonthSales> GetSales(int startYear);
        List<MonthSales> GetSalesByStaff(IEnumerable<Reservations> Items);
        List<MonthSales> GetProfits(int startYear);
        List<YearSales> GetYearsDashboard();

        List<ShortReport> GetAgencyReport(int year);
        List<ShortReport> GetAgencyReport(int year,IEnumerable<Reservations> Items);

        List<ShortReport> GetAgencyReportByCountry(int year, string name = "");
        List<MonthSales> GetAgencyReportById(int year, int Id);
        List<ShortReport> GetAgencyReportByMemberId(int year, int Id);
        

        List<ShortReport> GetCountriesReport(int year);
        List<ShortReport> GetCountriesReport(IEnumerable<Reservations> Items);

        List<ShortReport> GetHotelsReport(int year);
        List<ShortReport> GetHotelsReport(int year, IEnumerable<Reservations> Items);

        List<ShortReport> GetReports(ReportType Type, ReportCategory Category);
        FileCounters GetSalesFilesCounters(int year);
        FileCounters GetCountryFilesCounters(int year, string country);
        FileCounters GetAgencyFilesCounters(int year, int AgencyId);
        FileCounters GetStaffFilesCounters(int year, int MId);

        IEnumerable<AccountingReport> GetDebtReport(string country = "");
        List<MonthSales> GetSalesByRegistrations(IEnumerable<Reservations> reservations);
    }
}
