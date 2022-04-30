using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Line.Data;
namespace Line.Models
{
    public class YearCountries
    {

        public string Country { get; set; }
        public decimal Year1 { get; set; }
        public decimal Year2 { get; set; }
        public decimal Year3 { get; set; }
        public decimal Year4 { get; set; }
        public decimal Year5 { get; set; }
    }

public partial class SettingViewModel
    {
        public int Id { get; set; }
        [UIHint("Panner")]
        public int Logo { get; set; }
        [UIHint("Panner")]
        public int InvoiceHeader { get; set; }
        [UIHint("Panner")]
        public int InvoiceFooter { get; set; }
        [UIHint("Panner")]
        public int VoucherHeader { get; set; }
        [UIHint("Panner")]
        public int VoucherFooter { get; set; }
        [UIHint("Panner")]
        public int HotelRequest { get; set; }
        [UIHint("Panner")]
        public int HotelChange { get; set; }
        [UIHint("Panner")]
        public int HotelCancel { get; set; }
        [UIHint("Panner")]
        public int HotelFooter { get; set; }

        public string EmailSMTP { get; set; }
        public string EmailPort { get; set; }
        public bool EmailUseSSL { get; set; }
        public bool EmailUseCredentials { get; set; }
        public string EmailDefaultUser { get; set; }
        public string EmailDefaultPass { get; set; }

        public string AccountingName { get; set; }
        public string AccountingTitle { get; set; }
        public string BankName { get; set; }
        public string BankAccount { get; set; }
        public string BankSwift { get; set; }
        public string BackIBAN { get; set; }
        public string BankAccountNo { get; set; }
        public string BankAddress { get; set; }

        public int DefaultCurrency { get; set; }
    }

    public class AccountingReport
    {
        public string Country { get; set; }
        public int AgencyId { get; set; }
        public string AgencyName { get; set; }
        public decimal Balance { get; set; }
        public decimal LastBalance { get; set; }
        public decimal CurrentBalance { get; set; }
        public decimal LateBalance30 { get; set; }
        public decimal LateBalance60 { get; set; }
        public decimal LateBalance90 { get; set; }
        public decimal LateBalance120 { get; set; }
        public decimal Future { get; set; }
    }
    public enum ReportType
    {
        Sales,
        Prfit,
        Cost
    }
    public enum ReportCategory
    {
        Countries,
        Agencies,
        Hotels,
        Staff,
        Years
    }
    public enum TurkeyCities
    {
        Adana = 1,
        Adiyaman = 2,
        Afyonkarahisar = 3,
        Agri = 4,
        Aksaray = 5,
        Amasya = 6,
        Ankara = 7,
        Antalya = 8,
        Ardahan = 9,
        Artvin = 10,
        Aydin = 11,
        Balikesir = 12,
        Bartin = 13,
        Batman = 14,
        Bayburt = 15,
        Bilecik = 16,
        Bingol = 17,
        Bitlis = 18,
        Bolu = 19,
        Burdur = 20,
        Bursa = 21,
        Canakkale = 22,
        Cankiri = 23,
        Corum = 24,
        Denizli = 25,
        Diyarbakir = 26,
        Duzce = 27,
        Edirne = 28,
        Elazig = 29,
        Erzincan = 30,
        Erzurum = 31,
        Eskisehir = 32,
        Gaziantep = 33,
        Giresun = 34,
        Gumushane = 35,
        Hakkâri = 36,
        Hatay = 37,
        Igdir = 38,
        Isparta = 39,
        Istanbul = 40,
        Izmir = 41,
        Kahramanmaras = 42,
        Karabuk = 43,
        Karaman = 44,
        Kars = 45,
        Kastamonu = 46,
        Kayseri = 47,
        Kirikkale = 48,
        Kirklareli = 49,
        Kirsehir = 50,
        Kilis = 51,
        Kocaeli = 52,
        Konya = 53,
        Kutahya = 54,
        Malatya = 55,
        Manisa = 56,
        Mardin = 57,
        Mersin = 58,
        Mugla = 59,
        Mus = 60,
        Nevsehir = 61,
        Nigde = 62,
        Ordu = 63,
        Osmaniye = 64,
        Rize = 65,
        Sakarya = 66,
        Samsun = 67,
        Siirt = 68,
        Sinop = 69,
        Sivas = 70,
        Sanliurfa = 71,
        Sirnak = 72,
        Tekirdag = 73,
        Tokat = 74,
        Trabzon = 75,
        Tunceli = 76,
        Usak = 77,
        Van = 78,
        Yalova = 79,
        Yozgat = 80,
        Zonguldak = 81
    }
    public class DashboardViewModel
    {
        public List<YearSales> Years { get; set; }
        public List<MonthSales> Sales { get; set; }
        public List<ShortReport> Agencies { get; set; }
        public List<ShortReport> AgenciesLastYear { get; set; }
        public List<ShortReport> Countries { get; set; }
        public List<ShortReport> CountriesLastYear { get; set; }
        public List<ShortReport> Hotels { get; set; }
        public List<ShortReport> HotelsLastYear { get; set; }
    }

    public class MonthSales
    {
        public int Year { get; set; }
        public string name { get; set; }
        public decimal Jan { get; set; }
        public decimal Feb { get; set; }
        public decimal Mar { get; set; }
        public decimal Apr { get; set; }
        public decimal May { get; set; }
        public decimal Jun { get; set; }
        public decimal Jul { get; set; }
        public decimal Aug { get; set; }
        public decimal Sep { get; set; }
        public decimal Oct { get; set; }
        public decimal Nov { get; set; }
        public decimal Dec { get; set; }

        public decimal Total { get; set; }
    }

    public class YearSales
    {
        public int Year { get; set; }
        public decimal Sales { get; set; }
        public decimal Profit { get; set; }
    }

    public class FileCounters
    {
        public int Confirmed { get; set; }
        public decimal Canceled { get; set; }
        public decimal Requests { get; set; }
    }

    public class ShortReport
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Year { get; set; }
        public int AgencyId { get; set; }
        public int Count { get; set; }
        public decimal Sales { get; set; }
        public decimal Profit { get; set; }
    }

    public enum ReservationStatus
    {
        Request,
        Confirmed,
        ReConfirmed,
        Canceled,
        Completed,
        Locked
    }

    public enum ReservationDate
    {
        Creation,
        Checkin,
        Checkout
    }

    public class ReservationSearchModel
    {
        public ReservationSearchModel()
        {
            Currency = new List<int>();
            CreatedBy = new List<int>();
            AgencyId = new List<int>();
            Status = new List<string>();
        }
        public string price { get; set; }
        public string priceto { get; set; }

        public List<int> Currency { get; set; }

        public DateTime CreateDate { get; set; }
        public DateTime CreateDateTo { get; set; }

        public DateTime CheckinDatefrom { get; set; }
        public DateTime CheckinDateto { get; set; }

        public DateTime CheckoutDatefrom { get; set; }
        public DateTime CheckoutDateto { get; set; }

        public List<int> CreatedBy { get; set; }
        public List<string> Status { get; set; }

        public string Customer { get; set; }

        public string Country { get; set; }
        public List<int> AgencyId { get; set; }

        public bool OnlyUnpaidFiles { get; set; }
        public bool OnlyPaidFiles { get; set; }
    }

    public class HotelSearchModel
    {
        public HotelSearchModel()
        {
            Currency = new List<int>();
            CreatedBy = new List<int>();
            AgencyId = new List<int>();
        }

        public int hotel { get; set; }

        public string price { get; set; }
        public string priceto { get; set; }

        public List<int> Currency { get; set; }

        public DateTime Cashfrom { get; set; }
        public DateTime Cashto { get; set; }

        public DateTime CheckinDatefrom { get; set; }
        public DateTime CheckinDateto { get; set; }

        public DateTime CheckoutDatefrom { get; set; }
        public DateTime CheckoutDateto { get; set; }

        public List<int> CreatedBy { get; set; }

        public bool Confirmed { get; set; }
        public bool UnConfirmed { get; set; }
        public bool MessageSend { get; set; }
        public bool IsCash { get; set; }


        public string Country { get; set; }
        public List<int> AgencyId { get; set; }
    }

    public class GenerecSearchModel
    {
        public GenerecSearchModel()
        {
            Currency = new List<int>();
            CreatedBy = new List<int>();
            AgencyId = new List<int>();
        }

        public string price { get; set; }
        public string priceto { get; set; }

        public List<int> Currency { get; set; }

        public DateTime Datefrom { get; set; }
        public DateTime Dateto { get; set; }

        public List<int> CreatedBy { get; set; }

        public string Country { get; set; }

        public string City { get; set; }

        public List<int> AgencyId { get; set; }

        public DateTime CheckOutFrom { get; internal set; }
        public DateTime CheckOutTo { get; internal set; }
        public string Type { get; internal set; }
    }

    public class AgencySearchModel
    {
        public AgencySearchModel()
        {
            Currency = new List<int>();
            CreatedBy = new List<int>();
            AgencyId = new List<int>();
        }

        public string price { get; set; }
        public string priceto { get; set; }

        public List<int> Currency { get; set; }

        public DateTime Datefrom { get; set; }
        public DateTime Dateto { get; set; }

        public List<int> CreatedBy { get; set; }

        public string Country { get; set; }

        public string City { get; set; }

        public List<int> AgencyId { get; set; }

        public DateTime CheckOutFrom { get; internal set; }
        public DateTime CheckOutTo { get; internal set; }

        public List<int> PaymentStatus { get; set; }
    }
}
