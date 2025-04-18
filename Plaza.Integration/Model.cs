﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace database
{
    using System;
    using System.Collections.Generic;
    
    public partial class Agency
    {
        public Agency()
        {
            this.Agents = new HashSet<Agents>();
            this.Reservations = new HashSet<Reservations>();
        }
    
        public int ID { get; set; }
        public string AgencyName { get; set; }
        public bool Active { get; set; }
        public string Market { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Zip { get; set; }
        public string Tel { get; set; }
        public string Fax { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public string Adress { get; set; }
        public decimal Commision { get; set; }
        public string Credit { get; set; }
        public string CreditAmount { get; set; }
        public string note { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string joindate { get; set; }
    
        public virtual ICollection<Agents> Agents { get; set; }
        public virtual ICollection<Reservations> Reservations { get; set; }
    }
}
namespace database
{
    using System;
    using System.Collections.Generic;
    
    public partial class Agents
    {
        public int ID { get; set; }
        public int AID { get; set; }
        public bool active { get; set; }
        public System.DateTime date { get; set; }
        public string name { get; set; }
        public string mobile { get; set; }
        public string email { get; set; }
        public string skype { get; set; }
        public string other { get; set; }
    
        public virtual Agency Agency { get; set; }
    }
}
namespace database
{
    using System;
    using System.Collections.Generic;
    
    public partial class ChangeRoom
    {
        public int ID { get; set; }
        public int HID { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public int count { get; set; }
        public decimal cost { get; set; }
        public decimal price { get; set; }
        public string board { get; set; }
        public string guset { get; set; }
        public Nullable<System.DateTime> checkin { get; set; }
        public Nullable<System.DateTime> checkout { get; set; }
    
        public virtual hotelchange hotelchange { get; set; }
    }
}
namespace database
{
    using System;
    using System.Collections.Generic;
    
    public partial class Coupun
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Email { get; set; }
        public string Total { get; set; }
        public string Rate { get; set; }
        public string ExpireDate { get; set; }
    }
}
namespace database
{
    using System;
    using System.Collections.Generic;
    
    public partial class DeadLine
    {
        public int id { get; set; }
        public int Ref_file { get; set; }
        public System.DateTime Expaire { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
    }
}
namespace database
{
    using System;
    using System.Collections.Generic;
    
    public partial class ExtraService
    {
        public int ID { get; set; }
        public int RID { get; set; }
        public string name { get; set; }
        public System.DateTime date { get; set; }
        public decimal Cost { get; set; }
        public decimal Total { get; set; }
        public string customer { get; set; }
        public string note { get; set; }
        public System.DateTime dateout { get; set; }
        public Nullable<int> Pax { get; set; }
    
        public virtual Reservations Reservations { get; set; }
    }
}
namespace database
{
    using System;
    using System.Collections.Generic;
    
    public partial class hotelchange
    {
        public hotelchange()
        {
            this.ChangeRoom = new HashSet<ChangeRoom>();
        }
    
        public int id { get; set; }
        public int RID { get; set; }
        public string name { get; set; }
        public Nullable<int> hid { get; set; }
        public System.DateTime checkout { get; set; }
        public System.DateTime checkin { get; set; }
        public decimal Cost { get; set; }
        public decimal Total { get; set; }
        public string customer { get; set; }
        public string note { get; set; }
    
        public virtual ICollection<ChangeRoom> ChangeRoom { get; set; }
        public virtual Reservations Reservations { get; set; }
    }
}
namespace database
{
    using System;
    using System.Collections.Generic;
    
    public partial class ininvoice
    {
        public int id { get; set; }
        public int RID { get; set; }
        public string no { get; set; }
        public string description { get; set; }
        public System.DateTime date { get; set; }
        public decimal curency { get; set; }
        public decimal price { get; set; }
        public decimal total { get; set; }
        public string customer { get; set; }
        public string url { get; set; }
        public string currency { get; set; }
        public Nullable<int> type { get; set; }
        public Nullable<int> typeid { get; set; }
    
        public virtual Reservations Reservations { get; set; }
    }
}
namespace database
{
    using System;
    using System.Collections.Generic;
    
    public partial class Marketman
    {
        public int Id { get; set; }
        public string name { get; set; }
        public string Email { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Gsm { get; set; }
    }
}
namespace database
{
    using System;
    using System.Collections.Generic;
    
    public partial class Members
    {
        public Members()
        {
            this.Reservations1 = new HashSet<Reservations>();
        }
    
        public int ID { get; set; }
        public bool active { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string name { get; set; }
        public string title { get; set; }
        public string email { get; set; }
        public bool manager { get; set; }
        public bool accountManager { get; set; }
        public bool reservationManager { get; set; }
        public bool doreservations { get; set; }
        public string tel { get; set; }
        public string emailPassword { get; set; }
    
        public virtual ICollection<Reservations> Reservations1 { get; set; }
    }
}
namespace database
{
    using System;
    using System.Collections.Generic;
    
    public partial class old
    {
        public int res_id { get; set; }
        public string market { get; set; }
        public string Hoteladi { get; set; }
        public string Hotelkod { get; set; }
        public Nullable<int> emailsay { get; set; }
        public Nullable<int> belgesay { get; set; }
        public string email { get; set; }
        public string aciklama { get; set; }
        public string dosyano { get; set; }
        public string Rezervlist { get; set; }
        public string Rezervkod { get; set; }
        public Nullable<System.Guid> Rezervkodmu { get; set; }
        public string Checkindate { get; set; }
        public string Checkoutdate { get; set; }
        public string Totalnights { get; set; }
        public string Board { get; set; }
        public string OdaTipi { get; set; }
        public string Sngfiyat { get; set; }
        public string Sngoda { get; set; }
        public string Sngmaliyet { get; set; }
        public string Sngtotal { get; set; }
        public string Doublefiyat { get; set; }
        public string Doubleoda { get; set; }
        public string Doublemaliyet { get; set; }
        public string Doubletotal { get; set; }
        public string Exbedfiyat { get; set; }
        public string Exbedadet { get; set; }
        public string Exbedtotal { get; set; }
        public string Chdfreeadet { get; set; }
        public string Chd50fiyat { get; set; }
        public string Chd50adet { get; set; }
        public string Chd50total { get; set; }
        public string Rezervtotal { get; set; }
        public string hotelmaliyet { get; set; }
        public Nullable<double> hoteltotal { get; set; }
        public string maliyettotal { get; set; }
        public string Grandtotal { get; set; }
        public string Komisyontotal { get; set; }
        public string netpayment { get; set; }
        public string Info { get; set; }
        public string Turadi { get; set; }
        public string Turpaxcins { get; set; }
        public string Turprice { get; set; }
        public string Turadet { get; set; }
        public Nullable<double> Turtotal { get; set; }
        public string Turmaliyet { get; set; }
        public Nullable<double> fullturtotal { get; set; }
        public string Turtarih { get; set; }
        public string TurRegion { get; set; }
        public string Transferadi { get; set; }
        public string Transferpaxcins { get; set; }
        public string Transferadet { get; set; }
        public string Transferprice { get; set; }
        public string Transfermaliyet { get; set; }
        public Nullable<int> Transfertotal { get; set; }
        public Nullable<int> fulltransfertotal { get; set; }
        public string Transfertarih { get; set; }
        public string TransferRegion { get; set; }
        public string Transferflightno { get; set; }
        public string Transferflighttime { get; set; }
        public string komisyon { get; set; }
        public string kulladi { get; set; }
        public bool reconfirm { get; set; }
        public bool confirm { get; set; }
        public bool cancel { get; set; }
        public bool onay { get; set; }
        public bool degisiklik { get; set; }
        public bool payment { get; set; }
        public bool otel { get; set; }
        public string otel_bilgi { get; set; }
        public string notlar { get; set; }
        public string otel_tarih { get; set; }
        public string yedek { get; set; }
        public string rezervdurum { get; set; }
        public Nullable<int> yedeksayi { get; set; }
        public bool partlyconfirm { get; set; }
        public bool tamamdir { get; set; }
        public string odeme { get; set; }
        public bool belge { get; set; }
        public bool iptal_belge { get; set; }
        public bool degisiklik_belge { get; set; }
        public string birim { get; set; }
        public string mudahale { get; set; }
        public Nullable<System.DateTime> mudahale_tarih { get; set; }
        public string bizimkiler { get; set; }
        public Nullable<System.DateTime> tarih { get; set; }
        public Nullable<System.DateTime> ilktarih { get; set; }
        public Nullable<int> Rezervkodu { get; set; }
        public Nullable<System.DateTime> iptal_tarih { get; set; }
        public Nullable<int> degisiklikfark { get; set; }
        public Nullable<int> kasa { get; set; }
        public bool kasahareketi { get; set; }
        public bool silinecek { get; set; }
        public bool online { get; set; }
        public bool onlineiptal { get; set; }
        public string provider { get; set; }
        public string onlineFilename { get; set; }
        public Nullable<System.DateTime> onlineiptaltarih { get; set; }
        public string cancelationtext { get; set; }
        public string Triplefiyat { get; set; }
        public string Tripleadet { get; set; }
        public string Tripletotal { get; set; }
        public string SENDMAIL { get; set; }
        public bool SENDPDF { get; set; }
        public string PDFFILE { get; set; }
    }
}
namespace database
{
    using System;
    using System.Collections.Generic;
    
    public partial class Payments
    {
        public int ID { get; set; }
        public int RID { get; set; }
        public System.DateTime date { get; set; }
        public string type { get; set; }
        public decimal Payment { get; set; }
        public string note { get; set; }
        public string Member { get; set; }
        public string Currency { get; set; }
        public decimal TotalPrice { get; set; }
    
        public virtual Reservations Reservations { get; set; }
    }
}
namespace database
{
    using System;
    using System.Collections.Generic;
    
    public partial class Reservations
    {
        public Reservations()
        {
            this.ExtraService = new HashSet<ExtraService>();
            this.hotelchange = new HashSet<hotelchange>();
            this.ininvoice = new HashSet<ininvoice>();
            this.Rhotel = new HashSet<Rhotel>();
            this.Tour = new HashSet<Tour>();
            this.Transfers = new HashSet<Transfers>();
            this.Payments = new HashSet<Payments>();
        }
    
        public int ID { get; set; }
        public string Code { get; set; }
        public string ref_marketman { get; set; }
        public int ref_member { get; set; }
        public Nullable<int> ref_agency { get; set; }
        public string status { get; set; }
        public Nullable<System.DateTime> date { get; set; }
        public Nullable<decimal> net { get; set; }
        public Nullable<decimal> commision { get; set; }
        public Nullable<decimal> discount { get; set; }
        public Nullable<int> discountid { get; set; }
        public Nullable<decimal> cost { get; set; }
        public Nullable<decimal> profit { get; set; }
        public decimal price { get; set; }
        public Nullable<decimal> priceTL { get; set; }
        public Nullable<decimal> costTL { get; set; }
        public string curency { get; set; }
        public Nullable<System.DateTime> finishDate { get; set; }
        public System.DateTime balanceDate { get; set; }
        public System.DateTime updatedate { get; set; }
        public string updateuser { get; set; }
        public bool reconfirmed { get; set; }
        public string vocher { get; set; }
        public bool sendmail { get; set; }
        public string note { get; set; }
        public string agencystaff { get; set; }
        public decimal balance { get; set; }
        public System.DateTime checkindate { get; set; }
    
        public virtual Agency Agency { get; set; }
        public virtual ICollection<ExtraService> ExtraService { get; set; }
        public virtual ICollection<hotelchange> hotelchange { get; set; }
        public virtual ICollection<ininvoice> ininvoice { get; set; }
        public virtual Members Members { get; set; }
        public virtual ICollection<Rhotel> Rhotel { get; set; }
        public virtual ICollection<Tour> Tour { get; set; }
        public virtual ICollection<Transfers> Transfers { get; set; }
        public virtual ICollection<Payments> Payments { get; set; }
    }
}
namespace database
{
    using System;
    using System.Collections.Generic;
    
    public partial class Rhotel
    {
        public Rhotel()
        {
            this.RHRoom = new HashSet<RHRoom>();
        }
    
        public int ID { get; set; }
        public int RID { get; set; }
        public string name { get; set; }
        public Nullable<int> hid { get; set; }
        public System.DateTime checkout { get; set; }
        public System.DateTime checkin { get; set; }
        public decimal Cost { get; set; }
        public decimal Total { get; set; }
        public string customer { get; set; }
        public string note { get; set; }
        public bool Confirmed { get; set; }
        public bool SendHotel { get; set; }
    
        public virtual ICollection<RHRoom> RHRoom { get; set; }
        public virtual Reservations Reservations { get; set; }
    }
}
namespace database
{
    using System;
    using System.Collections.Generic;
    
    public partial class RHRoom
    {
        public int ID { get; set; }
        public int HID { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public int count { get; set; }
        public decimal cost { get; set; }
        public decimal price { get; set; }
        public string board { get; set; }
        public string guset { get; set; }
        public Nullable<System.DateTime> checkin { get; set; }
        public Nullable<System.DateTime> checkout { get; set; }
    
        public virtual Rhotel Rhotel { get; set; }
    }
}
namespace database
{
    using System;
    using System.Collections.Generic;
    
    public partial class tahsilat
    {
        public int id { get; set; }
        public string kulladi { get; set; }
        public Nullable<System.DateTime> tarih { get; set; }
        public Nullable<double> toplam { get; set; }
        public string staff { get; set; }
        public string odeme_tipi { get; set; }
        public string orderid { get; set; }
        public string odeme_detay { get; set; }
        public Nullable<System.DateTime> odeme_tarihi { get; set; }
        public string ilgilidosya { get; set; }
    }
}
namespace database
{
    using System;
    using System.Collections.Generic;
    
    public partial class Tour
    {
        public int ID { get; set; }
        public int RID { get; set; }
        public string name { get; set; }
        public System.DateTime date { get; set; }
        public decimal Cost { get; set; }
        public decimal Total { get; set; }
        public string customer { get; set; }
        public string note { get; set; }
        public int Pax { get; set; }
        public string pickuptime { get; set; }
        public string pickup { get; set; }
        public string dropoff { get; set; }
        public string city { get; set; }
        public string car { get; set; }
        public string guide { get; set; }
    
        public virtual Reservations Reservations { get; set; }
    }
}
namespace database
{
    using System;
    using System.Collections.Generic;
    
    public partial class Transfers
    {
        public int ID { get; set; }
        public int RID { get; set; }
        public System.DateTime date { get; set; }
        public decimal Cost { get; set; }
        public decimal Total { get; set; }
        public string customer { get; set; }
        public string note { get; set; }
        public int pax { get; set; }
        public string city { get; set; }
        public string pickup { get; set; }
        public string dropoff { get; set; }
        public string flightCode { get; set; }
        public string flightTime { get; set; }
        public string car { get; set; }
        public string guide { get; set; }
    
        public virtual Reservations Reservations { get; set; }
    }
}
namespace database
{
    using System;
    using System.Collections.Generic;
    
    public partial class uyeler
    {
        public int ID { get; set; }
        public string kulladi { get; set; }
        public string sifre { get; set; }
        public bool onay { get; set; }
        public string market { get; set; }
        public string ulke { get; set; }
        public string email { get; set; }
        public string indirim { get; set; }
        public string kredi { get; set; }
        public string komisyon { get; set; }
        public string adi { get; set; }
        public string agent_type { get; set; }
        public string soyadi { get; set; }
        public string acente_code { get; set; }
        public string firma { get; set; }
        public string fax { get; set; }
        public string tel { get; set; }
        public string sehir { get; set; }
        public Nullable<System.DateTime> uyelik_tarihi { get; set; }
        public Nullable<System.DateTime> son_giris_tarihi { get; set; }
        public Nullable<double> giris_sayisi { get; set; }
        public string uye_ip { get; set; }
        public string cevap { get; set; }
        public string sifresoru { get; set; }
        public string adres { get; set; }
        public string IP { get; set; }
        public bool gecici_kredi { get; set; }
        public string cperson { get; set; }
        public string remarks { get; set; }
        public string czipcode { get; set; }
        public string iata { get; set; }
        public string iperson { get; set; }
        public string iemail { get; set; }
        public string iadres { get; set; }
        public string isehir { get; set; }
        public string izipcode { get; set; }
        public string iulke { get; set; }
        public string itel { get; set; }
        public string ifax { get; set; }
        public string logo { get; set; }
        public string ceptel { get; set; }
        public string iceptel { get; set; }
        public bool subagent { get; set; }
    }
}
