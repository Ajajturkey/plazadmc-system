using Line.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Line.Models
{
    public class DashTotal
    {
        public int Request { get; set; }
        public int Confirmed { get; set; }
        public int Reconfirmed { get; set; }
        public int Canceled { get; set; }
        public int Total { get; set; }
    }
    public class MonthRows
    {
        public MonthRows()
        {
            years = new List<decimal>();
        }
        public int Id { get; set; }
        public string name { get; set; }
        public List<decimal> years { get; set; }
    }
    public class ViewErrors
    {
        public ViewErrors()
        {
            Errors = new List<string>();
        }
        public List<string> Errors { get; set; }
    }

    public class TransfersViewModel
    {
        public TransfersViewModel()
        {
            Transfers = new List<Transfers>();
            Transfer = new Transfers();
        }
        public int RID { get; set; }
        public string  Owner { get; set; }
        public string  Agency { get; set; }
        public string  Total { get; set; }
        public Transfers Transfer { get; set; }
        public List<Transfers> Transfers { get; set; }
    }
    public class ToursViewModel
    {
        public ToursViewModel()
        {
            Tours = new List<Tour>();
            Tour = new Tour();
        }
        public int RID { get; set; }
        public string Owner { get; set; }
        public string Agency { get; set; }
        public string Total { get; set; }
        public Tour Tour { get; set; }
        public List<Tour> Tours { get; set; }

    }
    public class AgencyViewModel
    {
        public AgencyViewModel()
        {
            Files = new List<Reservations>();
            Agency = new Agency();
            FileStatusList = new List<SelectListItem>();
            FileStatusList.Add(new SelectListItem { Text = "Paid", Value = "1" });
            FileStatusList.Add(new SelectListItem { Text = "UnPaid", Value = "0" });
            FileStatusList.Add(new SelectListItem { Text = "All.Files", Value = "2", Selected = true });

            FileDateList = new List<SelectListItem>();
            FileDateList.Add(new SelectListItem { Text = "Future", Value = "0" });
            FileDateList.Add(new SelectListItem { Text = "Passed", Value = "1" });
            FileDateList.Add(new SelectListItem { Text = "All.Files", Value = "2", Selected = true });

            FileStatus = 2;
            FileDate = 2;
        }
        public int Id { get; set; }
        public int FileStatus { get; set; }
        public List<SelectListItem> FileStatusList { get; set; }
        public int FileDate { get; set; }
        public List<SelectListItem> FileDateList { get; set; }

        public bool Showpayment { get; set; }
        public Agency Agency { get; set; }
        public List<Reservations> Files { get; set; }
        public DateTime CheckOutFrom { get;  set; }
        public DateTime CheckOutTo { get;  set; }
    }
    
    public class DeptViewModel
    {
        public int Id { get; set; }
        public string name { get; set; }
        public DateTime date { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Cutomer { get; set; }
        public decimal DebtValue { get; set; }
        public decimal PaymentsValue { get; set; }
        public decimal BalanceValue { get; set; }
    }

    public class DeptDateViewModel
    {
        public int Id { get; set; }
        public string name { get; set; }
        public string date { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Cutomer { get; set; }
        public decimal DebtValue { get; set; }
        public decimal PaymentsValue { get; set; }
        public decimal BalanceValue { get; set; }
    }
    public class GroupDeptViewModel
    {
        public int Id { get; set; }
        public string name { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public decimal DebtValue { get; set; }
        public decimal BalanceValue { get; set; }
        public int count { get; set; }
    }

    public class PaymentsViewModel
    {
        public PaymentsViewModel()
        {
            Payments = new List<PaymentViewModel>();
            Agency = new Agency();
        }
        public int Id { get; set; }

        public Agency Agency { get; set; }
        public List<PaymentViewModel> Payments { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
    }

    public class PaymentViewModel
    {
        public int AId { get; set;}
        public int Id { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string date { get; set; }
        public string description { get; set; }
        public decimal PaymentsValue { get; set; }
    }
    public class PackagesViewModel
    {
        public PackagesViewModel()
        {
            Packages = new List<ExtraService>();
            Package = new ExtraService();
        }
        public int RID { get; set; }
        public string Owner { get; set; }
        public string Agency { get; set; }
        public string Total { get; set; }
        public ExtraService Package { get; set; }
        public List<ExtraService> Packages { get; set; }
    }
    public class FlightsViewModel
    {
        public FlightsViewModel()
        {
            Flights = new List<Flight>();
        }
        public int RID { get; set; }
        public string Owner { get; set; }
        public string Agency { get; set; }
        public string Total { get; set; }
        public ExtraService Package { get; set; }
        public List<Flight> Flights { get; set; }
    }
    public class HotelsviewModel
    {
        public HotelsviewModel()
        {
            Hotles = new List<Rhotel>();
        }
        public int RID { get; set; }
        public string Owner { get; set; }
        public string Agency { get; set; }
        public string Total { get; set; }
        public int hotel { get; set; }
        public List<Rhotel> Hotles { get; set; }
    }
    public class RoomsViewModel
    {
        public RoomsViewModel()
        {
            Room = new RHRoom();
            Room.board = "BB";
            Rooms = new List<RHRoom>();
        }
        public int RID { get; set; }
        public string Owner { get; set; }
        public string Agency { get; set; }
        public string Total { get; set; }
        public Rhotel Hotel { get; set; }
        public string Location { get; set; }
        public string Contact { get; set; }
        public string Email { get; set; }
        public string ReservationDate { get; set; }
        public RHRoom Room { get; set; }
        public List<RHRoom> Rooms { get; set; }
    }
    public class ReservationControl
    {
        public int RID { get; set; }
        public int AID { get; set; }
        public bool CreateApproved { get; set; }
    }

    public class StatmentViewModel
    {
        public int Id { get; set; }
        public string name { get; set; }
        public string Statment { get; set; }
        public string date { get; set; }
        public string Country { get; set; }
        public string City { get; set; }

        public List<DeptDateViewModel> Files { get; set; }
       
    }
    



    public class SearchModel
    {
       
        public string wieght { get; set; }
        public string wieghtto { get; set; }
        
        public string price { get; set; }
        public string priceto { get; set; }

        public string Currency { get; set; }

        public string CreateDate { get; set; }
        public string CreateDateTo { get; set; }

        public string ReferanceCode { get; set; }
        public string Description { get; set; }

        public string CreatedBy { get; set; }
        public string Status { get; set; }

        public string ReciverName { get; set; }
        public string Reciverphone { get; set; }
        public string ReciverCountry { get; set; }
        public string ReciverCity { get; set; }

        public string senderName { get; set; }
        public string senderphone { get; set; }

        public string CarrierId { get; set; }
        public string CarrierName { get; set; }
        public string CarrierRefrence { get; set; }



    }

   public class VisitorModel
   {
        public int Id { get; set; }
        public string username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string EmailPassword { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Title { get; set; }
        public bool Active { get; set; }
        public bool Deleted { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsReservation { get; set; }
        public bool IsOperation { get; set; }
        public bool IsAccounting { get; set; }
        public bool IsManager { get; set; }
    }


    public enum RoomTypes
    {
        Single,
        Double,
        DoubleWithExtrabed,
        DoubleWithChild,
        Twin,
        TwinWithExtrabed,
        Triple,
        Quad
    }
    public enum RoomBoard
    {
        BB = 0,
        HB = 1,
        FB = 2,
        AI = 3,
        SC = 4,
        RO = 5
    }

    public enum Currency
    {
        TRY = 0,
        USD = 1,
        EUR = 2
    }

    public enum DemoAgencies
    {
        ALBAREKEH_TRAVELS = 0,
        ALMOHAD_TRAVELS = 1,
        EMARATTOUR_TRAVELS = 2
    }

    public enum DemoStatus
    {
        Request = 0,
        Confiremed = 1,
        ReConfiremed = 2,
        Canceled = 3
    }

    public enum DemoDates
    {
        ByFileCreate = 0,
        ByFileCheckin = 1,
        ByFileCheckOut = 3
    }

    public enum DemoUsers
    {
        AHMAD = 0,
        MOHAMAD = 1,
        HASAN = 2
    }

    public enum CreditType
    {
        Cash,
        Limited,
        Unlimited
        
    }
    

    public enum DemoCounties
    {
        Turkey = 0,
        Syria = 1,
        London = 2,
        USA = 3
    }

    public enum TurkeyCities
    {
        Adana = 0,
        Adiyaman = 1,
        Afyonkarahisar = 2,
        Agri = 3,
        Aksaray = 4,
        Amasya = 5,
        Ankara = 6,
        Antalya = 7,
        Ardahan = 8,
        Artvin = 9,
        Aydin = 10,
        Balikesir = 11,
        Bartin = 12,
        Batman = 13,
        Bayburt = 14,
        Bilecik = 15,
        Bingol = 16,
        Bitlis = 17,
        Bolu = 18,
        Burdur = 19,
        Bursa = 20,
        Canakkale = 21,
        Cankiri = 22,
        Corum = 23,
        Denizli = 24,
        Diyarbakir = 25,
        Duzce = 26,
        Edirne = 27,
        Elazig = 28,
        Erzincan = 29,
        Erzurum = 30,
        Eskisehir = 31,
        Gaziantep = 32,
        Giresun = 33,
        Gumushane = 34,
        Hakkari = 35,
        Hatay = 36,
        Igdir = 37,
        Isparta = 38,
        Istanbul = 39,
        Izmir = 40,
        Kahramanmaras = 41,
        Karabuk = 42,
        Karaman = 43,
        Kars = 44,
        Kastamonu = 45,
        Kayseri = 46,
        Kirikkale = 47,
        Kirklareli = 48,
        Kirsehir = 49,
        Kilis = 50,
        Kocaeli = 51,
        Konya = 52,
        Kutahya = 53,
        Malatya = 54,
        Manisa = 55,
        Mardin = 56,
        Mersin = 57,
        Mugla = 58,
        Mus = 59,
        Nevsehir = 60,
        Nigde = 61,
        Ordu = 62,
        Osmaniye = 63,
        Rize = 64,
        Sakarya = 65,
        Samsun = 66,
        Siirt = 67,
        Sinop = 68,
        Sivas = 69,
        Sirnak = 70,
        Tekirdag = 71,
        Tokat = 72,
        Trabzon = 73,
        Tunceli = 74,
        Sanliurfa = 75,
        Usak = 76,
        Van = 77,
        Yalova = 78,
        Yozgat = 79,
        Zonguldak = 80
    }
}