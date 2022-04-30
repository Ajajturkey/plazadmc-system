using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Xml;
using Autofac;
using Line.Data;
using Line.Models;
using Line.Services;

namespace Line.Helpers
{
    public static class shared
    {
        public static DateTime GetLastdates(Reservations item)
        {
            DateTime checkin = DateTime.Now.AddYears(-15);
            DateTime checkout = DateTime.Now.AddYears(15);


            if (item.Rhotel.Count > 0)
            {

                checkout = item.Rhotel.Max(d => d.checkout);

                return checkout;
            }
            else if (item.Transfers.Count > 0)
            {
                checkout = item.Transfers.Max(d => d.date);
                return checkout;
            }
            else if (item.Tour.Count > 0)
            {
                checkout = item.Tour.Max(d => d.date);
                return checkout;
            }
            else if (item.ExtraService.Count > 0)
            {
                checkout = item.ExtraService.Max(d => d.date);
                return checkout;
            }

            return DateTime.Now.Date;
        }

        public static string GetCustomer(Reservations item)
        {
            if (item.Rhotel.Count > 0)
            {
                return item.Rhotel.FirstOrDefault().customer;
            }
            else if (item.Transfers.Count > 0)
            {
                return item.Transfers.FirstOrDefault().customer;
            }
            else if (item.Tour.Count > 0)
            {
                return item.Tour.FirstOrDefault().customer;
            }
            else if (item.ExtraService.Count > 0)
            {
                return item.ExtraService.FirstOrDefault().customer;
            }

            return "";
        }

        public static string ToString(this decimal some, bool compactFormat)
        {
            if (compactFormat)
            {
                return String.Format("{0:0,0}", some) + " €";
            }
            else
            {
                return some.ToString();
            }
        }

        public static string DoFormat(decimal myNumber)
        {
            return myNumber.ToString(true);
            var s = string.Format("{0:0,0.00}", myNumber);

            if (s.EndsWith("00"))
            {
                if (s.Contains("."))
                {
                    return s.Remove(s.IndexOf('.'), 3) + ".-Euro";
                }
                else
                {
                    return s.Remove(s.Length - 3, 3) + ".-Euro";
                }

            }
            else
            {
                return s + ".-Euro";
            }
        }


        public static string CellFormat(decimal myNumber)
        {
            string specifier = "N";
            var culture = CultureInfo.CreateSpecificCulture("tr-TR");

            var str = myNumber.ToString(specifier, culture);

            if (str.EndsWith(",00"))
            {
                str = str.Remove(str.IndexOf(','), 3);
            }
            return "<table style='width:100%; ' <tr><td> " + str + " </td ><td style='width:15px; background-color:#eee;text-align:center;'>€</td></tr></tbody></table>";



        }

        public static string DoFormat(decimal myNumber, int i)
        {
            string specifier = "N";
            var culture = CultureInfo.CreateSpecificCulture("tr-TR");

            var str = myNumber.ToString(specifier, culture);

            if (str.EndsWith(",00"))
            {
                str = str.Remove(str.IndexOf(','), 3);
            }
            return str;
            //    {
            //        return s.Remove(s.IndexOf(','), 3) + ".-€";
            //    }
            //    else
            //    {
            //        return s + ".-€";
            //    }
            //var s = string.Format("{0:0,0.000}", myNumber);
            //if (i == 2)
            //{
            //    s = string.Format("{0:0.0,00}", myNumber);
            //    if (s.EndsWith(",00"))
            //    {
            //        return s.Remove(s.IndexOf(','), 3) + ".-€";
            //    }
            //    else
            //    {
            //        return s + ".-€";
            //    }
            //}
            //else
            //{
            //    if (s.EndsWith("00"))
            //    {
            //        return s.Remove(s.IndexOf('.'), 3) + ".-€";
            //    }
            //    else
            //    {
            //        return s + ".-€";
            //    }
            //}



        }

        public static string getxmlprice(DateTime date, string currency)
        {
            //DateTime rdate;
            //if (DateTime.TryParseExact(date, "dd-MM-yy hh:mm:ss tt",
            //                           CultureInfo.InvariantCulture,
            //                           DateTimeStyles.None,
            //                           out rdate))
            //{
            bool Change = false;
            if (date.DayOfWeek == DayOfWeek.Saturday)
            {
                date = date.AddDays(-1);
                Change = true;
            }
            else if (date.DayOfWeek == DayOfWeek.Sunday)
            {
                Change = true;
                date = date.AddDays(-2);
            }
            if (date != DateTime.Now.Date)
            {
                Change = true;
            }
            //}
            string m_strFilePath = "";
            //string ddate = date.Split(' ')[0];
            //var rdate = ddate.Split('-');
            if (Change)
            {
                m_strFilePath = "http://www.tcmb.gov.tr/kurlar/" + date.ToString("yyyyMM") + "/" + date.ToString("ddMMyyyy") + ".xml";
            }
            else
            {
                m_strFilePath = "http://www.tcmb.gov.tr/kurlar/today.xml";
            }


            string Result = "0";

            switch (currency)
            {
                case "EURO":
                    currency = "EUR";
                    break;
                case "POUND":
                    currency = " GBP";
                    break;
                default:
                    break;
            }

            XmlDocument myXmlDocument = new XmlDocument();

            using (WebClient client = new WebClient())
            {
                string xml = client.DownloadString(m_strFilePath);
                XmlDocument doc = new XmlDocument();
                myXmlDocument.LoadXml(xml);
            }


            //try
            //{
            //     myXmlDocument.LoadXml(m_strFilePath);
            //}
            //catch (Exception)
            //{
            //    string day = (Convert.ToInt32(rdate[0])+ 1).ToString("00");
            //    m_strFilePath = "http://www.tcmb.gov.tr/kurlar/20" + rdate[2] + rdate[1] + "/" + rdate[0] + rdate[1] + "20" + rdate[2] + ".xml";
            //    try
            //    {
            //        myXmlDocument.LoadXml(m_strFilePath);
            //    }
            //    catch (Exception)
            //    {
            //        day = (Convert.ToInt32(rdate[0])).ToString("00");
            //        m_strFilePath = "http://www.tcmb.gov.tr/kurlar/20" + rdate[2] + rdate[1] + "/" + rdate[0] + rdate[1] + "20" + rdate[2] + ".xml";

            //    }
            //}


            foreach (XmlNode RootNode in myXmlDocument.ChildNodes[2].ChildNodes)
            {
                if (RootNode.Attributes["CurrencyCode"].InnerText == currency)
                {
                    Result = Convert.ToDecimal(RootNode.ChildNodes[6].InnerText).ToString("#.###");
                    break;
                }
            }

            return Result;
        }
    }
    public enum MailType
    {
        Confirm = 0,
        Cancel = 1,
        Change = 2
    }

    public static class Mailing
    {
        public static string centertablestart = "<table bgcolor='EFEFEF' cellspacing='0' cellpadding='0' border='0' width='100%' height='100%'><tbody><tr><td width='100%' align='center' valign='top'><table cellspacing='0' cellpadding='0' border='0' width='730' style='width:600px;min-width:650px'><tbody><tr><td>";
        public static string centertableend = "</td></tr></tbody></table></td></tr></tbody></table>      ";
        public static string bodystart = "<table cellspacing='0' cellpadding='15' border='0' width='100%' style='background-color:#ffffff;'><tbody><tr><td><table cellspacing='0' cellpadding='0' border='0' width='100%'><tbody><tr><td>";
        public static string bodyend = "</td></tr></tbody></table></td></tr></tbody></table>	";


        public static int SendMailToHotel(MailType type, string HotelName, string email, string Note, string name, string member, string title, string MemberEmail, Rhotel Hotel, string id)
        {
            var _Settings = DependencyManager.Scope.Resolve<IConfiqurationService>();
            var _Upload = DependencyManager.Scope.Resolve<Line.Services.IUploadService>();
            var settings = _Settings.GetSettings();

            string ME = MemberEmail;
            string Header = "", Body = "", Footer = "";
            string Subject = "";
            switch (type)

            {
                case MailType.Confirm:

                    Header = _Upload.GetPicture(settings.HotelRequest);// "http://login.plazadmc.com/static/img/system/new-reservation.png";
                    Footer = _Upload.GetPicture(settings.HotelFooter);//"http://login.plazadmc.com/static/img/system/footer.png";

                    Body +=
                         "<meta http-equiv = \"Content-Type\" content = \"text/html; charset=iso-8859-3\" > " +
                          centertablestart +

                    "    <img  src='" + Header + "'   width='730' height='132' alt='header' />              " +

                    bodystart +

                    "    <h3> Sayın " + name + "  </h3>                                                          " +
                    "    <p>Aşağıda detayları verilen rezervasyonumuzu konfirme etmenizi rica ederiz..</p>                               " +
                    "    <h3 style='color:#57a2ce'> Rezervasyon No : " + id + " </h3>         " +

                    "    <h3>" + Hotel.name + " </h3>                                                          ";
                    Body += CreateHotels(Hotel);

                    Body +=
                "                                                                                                                    " +
                "    <br />                                                                                                          " +
                "    <p>                                                                                                             " +
                "        <span style='color:#57a2ce' >ÖNEMLİ NOT:</span><br />                                              " +
                "        Tarafımıza fatura etmenizi,                                                                       " +
                "        <br />                                                                                                      " +
                "        Yabancı acente voucherının check-in anında istenmesini,                                                     " +
                "        <br />                                                                                                      " +
                "        Aslının veya kopyasının faturanız ile birlikte mutlaka tarafımıza gönderilmesini<br />                      " +
                "        Önemle rica ederiz.                                                                                        " +
                "    </p>                                                                                                            ";

                    if (Note.Trim() != "")
                    {
                        Body +=
                                "     <p style='color:#d9534f' >                                                                              " +
                        "         NOT: " + Note + "               " +
                            "     </p>                                                                                                           ";
                    }


                    Body +=

                    "    <p  >Saygılarımızla,</p>                                                                     " +
                    "    <p>                                                                                                             " +
                    "        <span style=\"font-weight: 900\">" + member + "</span>                             " +
                    "        <br />                                                                                                      " +
                    "        <span> " + title + "</span>                                             " +
                    "      </p>                                                                                                          " +

                    bodyend +

                    "    <img src ='" + Footer + "'   width='730' height='132' alt='footer'  />        " +

                    centertableend;

                    Subject = "Rezervasyon Talebi - PlazaDMC Res ID: " + id + " ";
                    break;



                case MailType.Cancel:

                    Header = _Upload.GetPicture(settings.HotelCancel);
                    Footer = _Upload.GetPicture(settings.HotelFooter);
                    //Header = "http://login.plazadmc.com/static/img/system/cancel.png";
                    //Footer = "http://login.plazadmc.com/static/img/system/footer.png";

                    Body +=
                       "<meta http-equiv = \"Content-Type\" content = \"text/html; charset=iso-8859-3\" > " +
                        centertablestart +

                        "    <img  src='" + Header + "'    width='730' height='132' alt='header' />              " +
                        "    <br />                                                                                                          " +
                        bodystart +

                    "    <h3> Sayın " + name + "</h3>                                                          " +
                    "    <p>Aşağıda detayları verilen rezervasyonumuzu İPTAL etmenizi rica ederiz..</p>                               " +
                    "    <h3 style='color:#57a2ce'>Rezervasyon No : <span>" + id + "</span> </h3>         " +
                    "    <br />                                                                                                          ";

                    Body += CreateHotels(Hotel);

                    Body +=
      "                                                                                                                    ";

                    if (Note != "")
                    {
                        Body +=
                               "     <p style='color:#d9534f'>                                                                              " +
                               "         NOT: " + Note + "               " +
                               "     </p>                                                                                                           ";
                    }


                    Body +=

                "    <p  >Saygılarımızla,</p>                                                                     " +
                "    <p>                                                                                                             " +
                "        <span  style=\"font-weight: 900\">" + member + "</span>                             " +
                "        <br />                                                                                                      " +
                "        <span> " + title + "</span>                                             " +
                "      </p>                                                                                                          " +

                bodyend +
                    "    <img src ='" + Footer + "'   width='730' height='132' alt='footer'  />        " +
                    centertableend;
                    Subject = "Rezervasyon İPTAL Talebi - PlazaDMC Res ID: " + id + " ";
                    break;


                case MailType.Change:


                    string Hotels = "";
                    var _reservationService = DependencyManager.Scope.Resolve<IReservationService>();

                   hotelchange changed = _reservationService.GetHotelChangeByHotelId(Hotel.ID);
                       

                        if (changed != null)
                        {

                            Hotels += CreateHotels(changed);
                            Hotels +=
                                "<h3 style='color:#d9534f;text-align:center;'> Daha önce yukarıdaki şekilde detayları verilen rezervasyonumuzu <br/> " +
                                        "AŞAĞIDAKİ ŞEKİLDE KONFİRME ETMENİZİ RİCA EDERİZ. </h3>";

                            Hotels += CreateHotels(Hotel);

                        }
                        else
                        {
                            Hotels = CreateHotels(Hotel);
                        }



                    Header = _Upload.GetPicture(settings.HotelChange);
                    Footer = _Upload.GetPicture(settings.HotelFooter);


                    Body +=
                       "<meta http-equiv = \"Content-Type\" content = \"text/html; charset=iso-8859-3\" > " +
                        centertablestart +

                        "    <img  src='" + Header + "'   width='730' height='132' alt='header' />              " +
                        "    <br />                                                                                                          " +
                        bodystart +

                    "    <h3> Sayın " + name + "</h3>                                                          " +
                    "    <p>Rezervasyonumuz ile ilgili değişiklik talebimizin detayları aşağıda dikkatinize sunulmuştur.</p>                               " +
                    "    <h3 style='color:#57a2ce' >Rezervasyon No : <span runat = \"server\" id=\"Rid\">" + id + "</span> </h3>         " +
                    "    <h3>" + Hotel.name + " </h3>                                                          "+
                    "    <h3 style='color:#d9534f' > REZERVASYON DEĞİŞİKLİK TALEBİ   </h3>         ";

                    Body += Hotels;


                    Body +=
      "                                                                                                                    " +
        "    <br />                                                                                                          " +
        "    <p>                                                                                                             " +
        "        <span style='color:#57a2ce' >ÖNEMLİ NOT:</span><br />                                              " +
        "        Tarafımıza fatura etmenizi,                                                                       " +
        "        <br />                                                                                                      " +
        "        Yabancı acente voucherının check-in anında istenmesini,                                                     " +
        "        <br />                                                                                                      " +
        "        Aslının veya kopyasının faturanız ile birlikte mutlaka tarafımıza gönderilmesini<br />                      " +
        "        Önemle rica ederiz.                                                                                        " +
        "    </p>                                                                                                            ";

                    if (Note != "")
                    {
                        Body +=
                               "     <p style='color:#d9534f'>                           ";


                        if (!string.IsNullOrWhiteSpace(Note))
                        {
                            Body += "         NOT: " + Note + "               ";
                        }
                        Body +=
    "     </p>                                                                                                           ";
                    }


                    Body +=

        "    <p >Saygılarımızla,</p>                                                                     " +
        "    <p>                                                                                                             " +
        "        <span  style=\"font-weight: 900\">" + member + "</span>                             " +
        "        <br />                                                                                                      " +
        "        <span > " + title + "</span>                                             " +
        "      </p>                                                                                                          " +

        bodyend +

                    "    <img src ='" + Footer + "'   width='730' height='132' alt='footer'  />        " +
                    centertableend;
                    Subject = "Rezervasyon DEGISIKLIK Talebi - PlazaDMC Res ID: " + id + " ";

                    break;
                default:
                    break;
            }

            SendMessage(email, Body, "", ME, Subject);

            return 0;
        }

        private static string CreateHotels(hotelchange Hotel)
        {
            string table = "";

            table +=
                 " <table cellspacing='0' cellpadding='0' border='1'  style='font-size: 11px;width:700px;border: 1px solid #ddd'>  <tbody>" +

                   "    <tr  color: #FFF;background: #57a2ce;' >                                                                                                       " +
                   "        <th style='width: 175px;height: 20px'>Misafir ismi</th>                                                                                                                  " +
                   "        <th>Oda kategorisi</th>                                                                                                                  " +
                   "        <th>Oda tipi</th>                                                                                                                       " +
                   "        <th>Board</th>                                                                                                                          " +
                   "       <th style='width: 60px;'>Oda sayısı</th>                                                                                                                          " +
                   "       <th>check-in</th>                                                                                                                        " +
                   "       <th>check-out</th>                                                                                                                      " +
                   "                                                                                                                                                " +
                   "        </tr>                                                                                                                                   ";
            foreach (var item in Hotel.ChangeRoom)
            {
                table +=
              "        <tr>                                                                                                           " +
                    "        <th>" + item.guset + " </th>                                                                                                                   " +
                    "        <th>" + item.name + " </th>                                                                                                                   " +
                    "        <th>" + item.type + "</th>                                                                                                                         " +
                    "        <th>  " + item.board + "      </th>                                                                                                                       " +
                    "        <th>  " + item.count + "      </th>                                                                                                                       " +
                    "        <th>" + item.checkin.Value.ToString("dd.MM.yyyy") + "</th>                                                                                                                     " +
                    "        <th>" + item.checkout.Value.ToString("dd.MM.yyyy") + "</th>                                                                                                                     " +
                    "        </tr>                                                                                                                                   " +
                    "        ";

            }

            table +=
            "           </tbody></table>                 ";

            return table;
        }

        private static string CreateHotels(Rhotel Hotel)
        {
            string table = "";

            table +=

                   " <table cellspacing='0' cellpadding='0' border='1'  style='font-size: 11px;width:700px;border: 1px solid #ddd'>  <tbody>" +

                   "    <tr  color: #FFF;background: #57a2ce;' >                                                                                                       " +
                   "        <th style='width: 175px;height: 20px'>Misafir ismi</th>                                                                                                                  " +
                   "        <th>Oda kategorisi</th>                                                                                                                  " +
                   "        <th>Oda tipi</th>                                                                                                                       " +
                   "        <th>Board</th>                                                                                                                          " +
                   "       <th style='width: 60px;'>Oda sayısı</th>                                                                                                                          " +
                   "       <th>check-in</th>                                                                                                                        " +
                   "       <th>check-out</th>                                                                                                                      " +
                   "                                                                                                                                                " +
                   "        </tr>                                                                                                                                   ";
            foreach (var item in Hotel.RHRoom)
            {
                table +=
              "        <tr>                                                                                                           " +
                    "        <th>" + item.guset + " </th>                                                                                                                   " +
                    "        <th>" + item.name + " </th>                                                                                                                   " +
                    "        <th>" + item.type + "</th>                                                                                                                         " +
                    "        <th>  " + item.board + "      </th>                                                                                                                       " +
                    "        <th>  " + item.count + "      </th>                                                                                                                       " +
                    "        <th>" + item.checkin.Value.ToString("dd.MM.yyyy") + "</th>                                                                                                                     " +
                    "        <th>" + item.checkout.Value.ToString("dd.MM.yyyy") + "</th>                                                                                                                     " +
                    "        </tr>                                                                                                                                   " +
                    "        ";

            }

            table +=
            "           </tbody></table>                 ";

            return table;
        }

        public static string GetCssStyle()
        {
            //string url = "https://maxcdn.bootstrapcdn.com/bootstrap/3.3.6/css/bootstrap.min.css";
            //var textFromFile = (new WebClient()).DownloadString(url);

            return


         "<style> " +
    "    body {                                             " +
    "            color: #171313;                            " +
    "    font-size: 13px;                                 " +
    "        }                                              " +
    ".myContainer{                                          " +
    "            width: 730px;                              " +
    "            height: auto;                              " +
    "            margin: 0 auto;                            " +
    "            padding:0 15px;                             " +
    "                                                       " +
    "        }                                              " +
    ".text-header {                                       " +
    "            font-weight:800;                         " +
    "            font-size:15px;                          " +
    "        }                                              " +
    ".header, .Footer{                                      " +
    "            width: 700px;                              " +
    "            height: auto;                              " +
    "        }                                              " +
    ".Blueheader{                                           " +
    "            font-size: 14px;                         " +
    "            color: #58AAE0;                            " +
    "    font-weight:900                                  " +
    "}                                                      " +
    ".Redheader{                                            " +
    "            font-size: 14px;                         " +
    "            padding-right:15px;                      " +
    "            color: #c23535;                            " +
    "}                                                      " +
    ".table {                                               " +
    "            margin-bottom: 0px;                      " +
    "        }                                              " +
    ".table-responsive {                                  " +
    "            min-height: .01 %;                       " +
    "            overflow-x: auto;                        " +
    "        }                                              " +
    ".table-bordered {                                    " +
    "            border: 1px solid #ddd;                    " +
    "}                                                      " +
    ".table {                                               " +
    "            width: 100 %;                              " +
    "            max-width: 100 %;                        " +
    "            margin-bottom: 0px;                      " +
    "        }                                              " +
    ".table {                                               " +
    "            background-color: transparent;           " +
    "        }                                              " +
    ".table {                                               " +
    "            border-spacing: 0;                       " +
    "            border-collapse: collapse;               " +
    "        }                                              " +
    "        * {                                            " +
    "            -webkit-box-sizing: border-box;      " +
    "            -moz-box-sizing: border-box;         " +
    "            box-sizing: border-box;                " +
    "        }                                              " +
    "        user agent stylesheet                          " +
    "        .table {                                       " +
    "            display: table;                            " +
    "            border-collapse: separate;               " +
    "            border-spacing: 1px;                     " +
    "            border-color: grey;                      " +
    "        }                                              " +
    ".bg-info {                                           " +
    "            background-color: #337ab7; color: white ;             " +
    "}                                                      " +
    ".bg-danger { " +
    "            background-color: #f2dede; " +
    "}" +
    ".bg-success  { " +
    "            background-color: #dff0d8; " +
    "}" +
            "        p {                              " +
    "            font-size:12px;                          " +
    "        }                                              " +

            "</style> ";
        }

        public static bool SendMessage(string Email, string Body, string CC, string ME, string Subject)
        {
            var _WorkContext = DependencyManager.Scope.Resolve<IWorkContext>();
            var member = _WorkContext.CurrentVisitor;

            if (member != null)
            {
                string From = member.email;//"Register@plazatur.com";
                string Password = member.emailPassword; //"PLaZa-5858";

                System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
                mail.To.Add(Email);
                mail.From = new MailAddress(From, member.GetFullName() + "/ PlazaDMC", System.Text.Encoding.UTF8);
                mail.Subject = Subject;
                mail.SubjectEncoding = System.Text.Encoding.UTF8;
                mail.Body = Body;
                mail.BodyEncoding = System.Text.Encoding.UTF8;
                mail.IsBodyHtml = true;
                mail.Priority = MailPriority.High;

                if (CC != "")
                {
                    mail.CC.Add(CC);

                }
                if (ME != "")
                {
                    mail.CC.Add(ME);

                }

                var _Settings = DependencyManager.Scope.Resolve<IConfiqurationService>();
                var settings = _Settings.GetSettings();
                SmtpClient client = new SmtpClient();

                if (settings.EmailUseCredentials)
                {
                client.Credentials = new System.Net.NetworkCredential(From, Password, From);
                }

                client.Port = Convert.ToInt32(settings.EmailPort);// 587; // Gmail works on this port
                client.Host = settings.EmailSMTP; //"smtp.office365.com";//"smtp.gmail.com"; 
                client.EnableSsl = settings.EmailUseSSL; //true; //Gmail works on Server Secured Layer  "QQaa8840"
                try
                {
                    client.Send(mail);
                    return true;
                }
                catch (Exception ex)
                {
                    client.Credentials = new System.Net.NetworkCredential(settings.EmailDefaultUser, settings.EmailDefaultPass, settings.EmailDefaultUser);
                    try
                    {
                        client.Send(mail);

                    }
                    catch (Exception exc)
                    {
                        SendUsingGmail(Email, Body, CC, ME, Subject);
                    }

                    //return true;


                    Exception ex2 = ex;
                    string errorMessage = string.Empty;
                    while (ex2 != null)
                    {
                        errorMessage += ex2.ToString();
                        ex2 = ex2.InnerException;
                    }
                    HttpContext.Current.Response.Write(errorMessage);
                    return false;
                }

            }
            else
            {
                HttpContext.Current.Response.Write("Please Login Again");
                return false;
            }

        }

        private static void SendUsingGmail(string email, string mbody, string cC, string mE, string msubject)
        {
            string smtpAddress = "smtp.gmail.com";
            int portNumber = 587;
            bool enableSSL = true;

            string emailFrom = "ahmadturkey88@gmail.com";
            string password = "1Blacklove!";
            string emailTo = email;
            string subject = msubject;
            string body = mbody;

            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(emailFrom);
                mail.To.Add(emailTo);
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;
                // Can set to false, if you are sending pure text.

                if (!string.IsNullOrEmpty(cC))
                {
                    mail.CC.Add(cC);

                }

                mail.CC.Add(mE);

                //mail.Attachments.Add(new Attachment("C:\\SomeFile.txt"));
                //mail.Attachments.Add(new Attachment("C:\\SomeZip.zip"));

                using (SmtpClient smtp = new SmtpClient(smtpAddress, portNumber))
                {
                    smtp.Credentials = new NetworkCredential(emailFrom, password);
                    smtp.EnableSsl = enableSSL;
                    smtp.Send(mail);
                }
            }
        }

        public static void SendMailToAgency(string Agency, string rsponsiple, string Email, string Note, string CC, string agencyName, string name, string title, string Memail, Reservations reservation, string vocher = "")
        {
            var _Settings = DependencyManager.Scope.Resolve<IConfiqurationService>();
            var _Upload = DependencyManager.Scope.Resolve<Line.Services.IUploadService>();
            var settings = _Settings.GetSettings();

            string css = GetCssStyle();
            string Header = "", Body = "", Footer = "";
            string Subject = "", reconfirm = "";
            if (vocher != "")
            {
                reconfirm = "    <h3> Your voucher No: <span>" + vocher + "</span> </h3>         ";
            }

            Header = _Upload.GetPicture(settings.InvoiceHeader);
            Footer = _Upload.GetPicture(settings.InvoiceFooter);

            //Header = "http://login.plazadmc.com/static/img/system/confirm.png";
            //Footer = "http://login.plazadmc.com/static/img/system/footer.png";

            Body +=
                 "<meta http-equiv = \"Content-Type\" content = \"text/html; charset=iso-8859-3\" > " +
                  centertablestart +

            "    <img  src='" + Header + "'   width='730' height='132' alt='header' />              " +

            bodystart +

            "    <h3> Dear " + rsponsiple + " / " + agencyName + "</h3>                                                          " +
            "    <p>Thank you for choosing PlazaDMC & Incentives <br/> <b/> This e-mail is automatically generated and posted by the online reservation system.</p>                               " +
            "    <p>All details are listed in the form below..</p>                               " +
            "    <h3 style='color:#57a2ce'>Your reservation ID  : <span>" + reservation.ID + "</span> </h3>         " +
            reconfirm;


            // Body += BuildCustomerspart(reservation);


            Body += BuildMailConfirmation(reservation);

            //CreateReservation(reservation);

            Body +=
                "                                                                                                                    " +
                "    <br />                                                                                                          ";

            if (Note != "")
            {
                Body += "    <p>                                                                                                             " +
            "        <span style='color:#57a2ce' >RESERVATION NOTE:</span><br />                                      " +


            "     <span style='color:#d9534f'>                                                                              " +
            "        " + Note + "               " +
            "     </span>                                                                                                           ";
            }


            Body +=

    "    <p>Regards,</p>                                                                     " +
    "    <p>                                                                                                             " +
    "        <span style=\"font-weight: 900\">" + name + "</span>                             " +
    "        <br />                                                                                                      " +
    "        <span> " + title + "</span>                                             " +
    "      </p>                                                                                                          " +
    bodyend +

        "    <img src ='" + Footer + "'   width='730' height='132' alt='footer'  />        " +
        centertableend;
            if (vocher != "")
            {
                Subject = "RECONFIRMED ONLINE: Your Reservation ID: " + reservation.ID + " With Voucher NO:" + vocher + "  ";
            }
            else
            {
                Subject = "CONFIRMED ONLINE: Your Reservation ID: " + reservation.ID + " ";
            }


            SendMessage(Email, Body, CC, Memail, Subject);

        }

        private static string BuildCustomerspart(Reservations reservation)
        {
            string customers = "<h4 style='font-weight: 700;'>Guests: ";

            if (reservation.Rhotel.Count > 0)
            {
                foreach (var item in reservation.Rhotel)
                {
                    // "<p><b>" + item.name + "</b>: ";
                    foreach (var room in item.RHRoom)
                    {
                        if (!customers.Contains(room.guset))
                        {
                            customers += room.guset + ", ";
                        }

                    }
                    customers = customers.Remove(customers.Count() - 2);

                }
            }
            else
            {

                customers += shared.GetCustomer(reservation);

            }

            customers += " </h4>";
            return customers;
        }

        private static string CreateReservation(Reservations reservation)
        {
            return CreateHotels(reservation) + CreateOthers(reservation) +
                "<table  cellspacing='0' cellpadding='0' border='1'  style='font-size: 11px;width:600px;border: 1px solid #ddd'text-align:righ' > <tr> <th> </th> <th style='text-align:left;border: 1px solid #ddd; width:70px'> "
                + reservation.price + ".Euro" +
                "</th> </tr> </table> "
                ;
        }

        private static string CreateHotels(Reservations res)
        {
            // string table = " <span class=\"Blueheader text-header\" >Hotels </span><span />";

            string table = "";

            foreach (var Hotel in res.Rhotel)
            {
                table +=
                  "<p>Hotel: " + Hotel.name + " </p>" +
                 "<table  style=\"width: 600px;border: 1px solid #ddd;text-align:left\"  class=\"table table-responsive table-striped table-bordered\"><tbody> " +

                "    <tr class='bg-info text-header'>                                                                                                       " +
                  "        <th  style='border: 1px solid #ddd;'  style='width: 120px;'>Guest</th>                                                                                                                  " +
                  "        <th  style='border: 1px solid #ddd;' >Category</th>                                                                                                                  " +
                  "        <th  style='border: 1px solid #ddd;' >Accommodation </th>                                                                                                                       " +
                  "        <th  style='border: 1px solid #ddd;' >Board</th>                                                                                                                          " +
                  "       <th  style='border: 1px solid #ddd;' >Nights</th>                                                                                                                          " +
                  "       <th  style='border: 1px solid #ddd;' >CheckIn</th>                                                                                                                        " +
                  "       <th  style='border: 1px solid #ddd;' >CheckOut</th>                                                                                                                      " +
                  "       <th  style='border: 1px solid #ddd;' >Rate</th>                                                                                                                          " +
                  "                                                                                                                                                " +
                  "        </tr>                                                                                                                                   ";
                foreach (var item in Hotel.RHRoom)
                {
                    table +=
                  "        <tr>                                                                                                           " +
                        "        <th  style='border: 1px solid #ddd;' >" + item.guset + " </th>                                                                                                                   " +
                        "        <th  style='border: 1px solid #ddd;' >" + item.name + " </th>                                                                                                                   " +
                        "        <th  style='border: 1px solid #ddd;' >" + item.type + " X " + item.count + "</th>                                                                                                                         " +
                        "        <th  style='border: 1px solid #ddd;' >  " + item.board + "      </th>                                                                                                                       " +
                        "        <th  style='border: 1px solid #ddd;' >" + (item.checkout.Value - item.checkin.Value).Days + "</th>                                                                                                                              " +
                        "        <th  style='border: 1px solid #ddd;' >" + item.checkin.Value.ToString("dd.MM.yyyy") + "</th>                                                                                                                     " +
                        "        <th  style='border: 1px solid #ddd;' >" + item.checkout.Value.ToString("dd.MM.yyyy") + "</th>                                                                                                                     " +
                        "        <th  style='border: 1px solid #ddd; width='70'' >  " + item.price + "  .Euro    </th>                                                                                                                       " +
                        "        </tr>                                                                                                                                   " +
                        "        ";

                }

                table +=
                "           </tbody></table>                 " +

                "<table  style=\"width: 600px;border: 1px solid #ddd;text-align:right\"  class=\"table table-responsive table-striped table-bordered\"> <tr> <th> </th> <th style='text-align:left;border: 1px solid #ddd; width:70px' class=\"Blueheader text-header\"> "
            + Hotel.Total + ".Euro" +
            "</th> </tr> </table> "
            ;

            }
            return table;
        }

        private static string CreateOthers(Reservations res)
        {
            // 
            string table = "";
            if (res.Tour.Count > 0)
            {
                table = "<p>Tours and Trips </p>" +
                 " <table  style=\"width: 600px;border: 1px solid #ddd;text-align:left\"  class=\"table table-responsive table-striped table-bordered\">";
                table += " <tr class=\"bg-info text-header \">                                                                                   " +
                       "        <th  style='border: 1px solid #ddd;'  style='width: 120px;'>Description</th>                            " +
                       "        <th  style='border: 1px solid #ddd;' >Date</th>                                       " +
                       "        <th  style='border: 1px solid #ddd;' >Flight Details</th>                             " +
                       "        <th  style='border: 1px solid #ddd;' >Pax</th>                                        " +
                       "        <th  style='border: 1px solid #ddd;'  width='70'>Rate</th>                                       " +
                       "    </tr>                                                   ";

                foreach (var item in res.Tour)
                {

                    table +=
                   "    <tr>                                                    " +
                   "        <th  style='border: 1px solid #ddd;' >" + item.name + "</th>                             " +
                   "        <th  style='border: 1px solid #ddd;' >" + item.date.ToString("dd.MM.yyyy") + " </th>                            " +
                   "        <th  style='border: 1px solid #ddd;' >" + item.note + "</th>                                           " +
                   "        <th  style='border: 1px solid #ddd;' >" + item.Pax + "</th>                                           " +
                   "        <th  style='border: 1px solid #ddd;' >" + item.Total + ".Euro </th>                                           " +
                   "    </tr>                                                   ";
                    table += "</table>";
                }
            }
            if (res.Transfers.Count > 0)
            {
                foreach (var item in res.Transfers)
                {
                    table += "<p>Transfers </p>" +
                " <table  style=\"width: 600px;border: 1px solid #ddd;text-align:left\"  class=\"table table-responsive table-striped table-bordered\">";
                    table += " <tr class=\"bg-info text-header \">                                                                                   " +
                          "        <th  style='border: 1px solid #ddd;'  style='width: 120px;'>Description</th>                            " +
                          "        <th  style='border: 1px solid #ddd;' >Date</th>                                       " +
                          "        <th  style='border: 1px solid #ddd;' >Flight Details</th>                             " +
                          "        <th  style='border: 1px solid #ddd;' >Pax</th>                                        " +
                          "        <th  style='border: 1px solid #ddd;'  width='70'>Rate</th>                                       " +
                          "    </tr>                                                   ";
                    table +=
                   "    <tr >                                                    " +
                   "        <th  style='border: 1px solid #ddd;' >" + item.city + "</th>                             " +
                   "        <th  style='border: 1px solid #ddd;' >" + item.date.ToString("dd.MM.yyyy") + " </th>                            " +
                   "        <th  style='border: 1px solid #ddd;' >" + item.note + "</th>                                           " +
                   "        <th  style='border: 1px solid #ddd;' >" + item.pax + "</th>                                           " +
                   "        <th  style='border: 1px solid #ddd;' >" + item.Total + " .Euro </th>                                           " +
                   "    </tr>                                                   ";
                    table += "</table>";
                }
            }
            if (res.ExtraService.Count > 0)
            {
                table += "<p>Packages And extra services </p>" +
               " <table  style=\"width: 600px;border: 1px solid #ddd;text-align:left\"  class=\"table table-responsive table-striped table-bordered\"> ";
                table += " <tr class=\"bg-info text-header \">                                                                                   " +
                      "        <th  style='border: 1px solid #ddd;'  style='width: 120px;'>Description</th>                            " +
                      "        <th  style='border: 1px solid #ddd;' >Date</th>                                       " +
                      "        <th  style='border: 1px solid #ddd;' >Flight Details</th>                             " +
                      "        <th  style='border: 1px solid #ddd;' >Pax</th>                                        " +
                      "        <th  style='border: 1px solid #ddd;'  width='70'>Rate</th>                                       " +
                      "    </tr>                                                   ";
            }
            foreach (var item in res.ExtraService)
            {

                table +=
               "    <tr>                                                    " +
               "        <th  style='border: 1px solid #ddd;' >" + item.name + "</th>                             " +
               "        <th  style='border: 1px solid #ddd;' >" + item.date.ToString("dd.MM.yyyy") + " </th>                            " +
               "        <th  style='border: 1px solid #ddd;' >" + item.note + "</th>                                           " +
               "        <th  style='border: 1px solid #ddd;' >" + item.Pax + "</th>                                           " +
               "        <th  style='border: 1px solid #ddd;' >" + item.Total + ".Euro</th>                                           " +

               "    </tr>                                                   ";

            }
            table += "</table>";
            //  table +=
            //         " <table  style=\"width: 600px;border: 1px solid #ddd;text-align:left\"  class=\"table table-responsive table-striped table-bordered\"> " +
            //  "       <tr>                                                                                         " +
            // "            <th  class=\"bg-success\" ></th>                                                " +
            // "             <th colspan='4' class=\"bg-success\"> Total: " + (res.Tour.Sum(d => d.Total) + res.Transfers.Sum(d => d.Total) + res.ExtraService.Sum(d => d.Total)) + "</th>   " +
            // "       </tr>                                                                                                                                         ";
            //table += " </table>";
            return table;

        }

        private static string CreateHotels2(ICollection<Rhotel> Hotels)
        {
            string table = "" +
            "<table style=\"font-size: 11px;-webkit-box-sizing: border-box;-moz-box-sizing: border-box;box-sizing: border-box;border-spacing: 0;border-collapse: collapse!important;background-color: transparent;width: 100%;max-width: 100%;margin-bottom: 20px;padding: 10px 15px;border-bottom: 1px solid transparent;border-top-left-radius: 3px;border-top-right-radius: 3px;border-color: #bce8f1;\" > " +
            " <tr style=\"-webkit-box-sizing: border-box;-moz-box-sizing: border-box;box-sizing: border-box;page-break-inside: avoid;color: white;background-color: #57a2ce !important;\">                                                                                   " +
            "     <th style=\"    height: 20px;-webkit-box-sizing: border-box;-moz-box-sizing: border-box;box-sizing: border-box;padding: 0;text-align: left;\" >Hotel Name</th>                                                                                     " +
            "     <th  style=\"-webkit-box-sizing: border-box;-moz-box-sizing: border-box;box-sizing: border-box;padding: 0;text-align: left;\">Check IN</th>                                                                                       " +
            "     <th  style=\"-webkit-box-sizing: border-box;-moz-box-sizing: border-box;box-sizing: border-box;padding: 0;text-align: left;\">Check Out</th>                                                                                      " +
            "     <th  style=\"-webkit-box-sizing: border-box;-moz-box-sizing: border-box;box-sizing: border-box;padding: 0;text-align: left;\">Board</th>                                                                                          " +
            "     <th  style=\"-webkit-box-sizing: border-box;-moz-box-sizing: border-box;box-sizing: border-box;padding: 0;text-align: left;\">Room</th>                                                                                           " +
            "     <th  style=\"-webkit-box-sizing: border-box;-moz-box-sizing: border-box;box-sizing: border-box;padding: 0;text-align: left;\" width='70'>Rate</th>                                                                                " +
            " </tr>                                                                                                       ";

            foreach (var item in Hotels)
            {
                table +=

                "<tr style=\"-webkit-box-sizing: border-box;-moz-box-sizing: border-box;box-sizing: border-box;page-break-inside: avoid; text-align: left;\">   <td style=\"-webkit-box-sizing: border-box;-moz-box-sizing: border-box;box-sizing: border-box;padding: 0;background-color: #fff!important;\">" + item.name + " </td> ";


                bool a = true;
                foreach (var room in item.RHRoom)
                {
                    if (a)
                    {
                        table +=
                              "         <td style=\"-webkit-box-sizing: border-box;-moz-box-sizing: border-box;box-sizing: border-box;padding: 0;background-color: #fff!important;\">" + room.checkin.Value.ToString("dd.MM.yyyy") + "</td>                       " +
                "         <td style=\"-webkit-box-sizing: border-box;-moz-box-sizing: border-box;box-sizing: border-box;padding: 0;background-color: #fff!important;\">" + room.checkout.Value.ToString("dd.MM.yyyy") + "</td>                       " +
                        "<td style=\"-webkit-box-sizing: border-box;-moz-box-sizing: border-box;box-sizing: border-box;padding: 0;background-color: #fff!important;\">" + room.board + " </td>                   " +
                                 " <td style=\"-webkit-box-sizing: border-box;-moz-box-sizing: border-box;box-sizing: border-box;padding: 0;background-color: #fff!important;\">" + room.name + " x " + room.count + " <br/> " + room.type + " </td>                         " +
                                 " <td style=\"-webkit-box-sizing: border-box;-moz-box-sizing: border-box;box-sizing: border-box;padding: 0;background-color: #fff!important;\">" + shared.DoFormat(room.price * room.count * (room.checkout.Value - room.checkin.Value).Days) + " </td>  </tr>             ";
                        a = false;
                    }
                    else
                    {
                        table += "<tr style='-webkit-box-sizing: border-box;-moz-box-sizing: border-box;box-sizing: border-box;page-break-inside: avoid;'> <td style=\"-webkit-box-sizing: border-box;-moz-box-sizing: border-box;box-sizing: border-box;padding: 0;background-color: #fff!important;\">  </td> " +
                            "         <td style=\"-webkit-box-sizing: border-box;-moz-box-sizing: border-box;box-sizing: border-box;padding: 0;background-color: #fff!important;\">" + room.checkin.Value.ToString("dd.MM.yyyy") + "</td>                       " +
                            "         <td style=\"-webkit-box-sizing: border-box;-moz-box-sizing: border-box;box-sizing: border-box;padding: 0;background-color: #fff!important;\">" + room.checkout.Value.ToString("dd.MM.yyyy") + "</td>                       " +
                            "         <td style=\"-webkit-box-sizing: border-box;-moz-box-sizing: border-box;box-sizing: border-box;padding: 0;background-color: #fff!important;\">" + room.board + " </td>                   " +
                                " <td style=\"-webkit-box-sizing: border-box;-moz-box-sizing: border-box;box-sizing: border-box;padding: 0;background-color: #fff!important;\"> " + room.name + "x " + room.count + " <br/> " + room.type + "</td>                         " +
                                " <td style=\"-webkit-box-sizing: border-box;-moz-box-sizing: border-box;box-sizing: border-box;padding: 0;background-color: #fff!important;\">" + shared.DoFormat(room.price * room.count * (room.checkout.Value - room.checkin.Value).Days) + " </td>  </tr>             ";
                    }
                }
            }
            table += " </table>";

            return table;
        }

        private static string CreateOthers2(Reservations res)
        {
            // 
            string table = "" +
                   " <table style=\"font-size: 11px;-webkit-box-sizing: border-box;-moz-box-sizing: border-box;box-sizing: border-box;border-spacing: 0;border-collapse: collapse!important;background-color: transparent;width: 100%;max-width: 100%;margin-bottom: 20px;padding: 10px 15px;border-bottom: 1px solid transparent;border-top-left-radius: 3px;border-top-right-radius: 3px;border-color: #bce8f1;\">" +
                   " <tr>  </tr>";

            table += " <tr style=\"-webkit-box-sizing: border-box;-moz-box-sizing: border-box;box-sizing: border-box;page-break-inside: avoid;color: white;background-color: #57a2ce !important;\" class=\"bg-success-1\">                                                                                   " +
                   "        <th style=\"    height: 20px;-webkit-box-sizing: border-box;-moz-box-sizing: border-box;box-sizing: border-box;padding: 0;text-align: left;\">Land Servicse</th>                            " +
                   "        <th style=\"-webkit-box-sizing: border-box;-moz-box-sizing: border-box;box-sizing: border-box;padding: 0;text-align: left;\">Description</th>                            " +
                   "        <th style=\"-webkit-box-sizing: border-box;-moz-box-sizing: border-box;box-sizing: border-box;padding: 0;text-align: left;\">Date</th>                                       " +
                   "        <th style=\"-webkit-box-sizing: border-box;-moz-box-sizing: border-box;box-sizing: border-box;padding: 0;text-align: left;\">Flight Details</th>                             " +
                   "        <th style=\"-webkit-box-sizing: border-box;-moz-box-sizing: border-box;box-sizing: border-box;padding: 0;text-align: left;\">Pax</th>                                        " +
                   "        <th style=\"-webkit-box-sizing: border-box;-moz-box-sizing: border-box;box-sizing: border-box;padding: 0;text-align: left;\" width='70'>Rate</th>                                       " +
                   "    </tr>                                                   ";

            foreach (var item in res.Tour)
            {

                table +=
               "    <tr style=\"-webkit-box-sizing: border-box;-moz-box-sizing: border-box;box-sizing: border-box;page-break-inside: avoid;text-align: left;\">                                                    " +
               "        <td style=\"-webkit-box-sizing: border-box;-moz-box-sizing: border-box;box-sizing: border-box;padding: 0;background-color: #fff!important;\">Tour </td>                             " +
               "        <td style=\"-webkit-box-sizing: border-box;-moz-box-sizing: border-box;box-sizing: border-box;padding: 0;background-color: #fff!important;\">" + item.name + "</td>                             " +
               "        <td style=\"-webkit-box-sizing: border-box;-moz-box-sizing: border-box;box-sizing: border-box;padding: 0;background-color: #fff!important;\">" + item.date.ToString("dd.MM.yyyy") + " </td>                            " +
               "        <td style=\"-webkit-box-sizing: border-box;-moz-box-sizing: border-box;box-sizing: border-box;padding: 0;background-color: #fff!important;\">" + item.city + "</td>                                           " +
               "        <td style=\"-webkit-box-sizing: border-box;-moz-box-sizing: border-box;box-sizing: border-box;padding: 0;background-color: #fff!important;\">" + item.Pax + "</td>                                           " +
               "        <td style=\"-webkit-box-sizing: border-box;-moz-box-sizing: border-box;box-sizing: border-box;padding: 0;background-color: #fff!important;\">" + shared.DoFormat(item.Total * item.Pax) + " </td>                                           " +
               "    </tr>                                                   ";
            }
            foreach (var item in res.Transfers)
            {

                table +=
               "    <tr style=\"-webkit-box-sizing: border-box;-moz-box-sizing: border-box;box-sizing: border-box;page-break-inside: avoid; text-align: left;\" >                                                    " +
               "        <td style=\"-webkit-box-sizing: border-box;-moz-box-sizing: border-box;box-sizing: border-box;padding: 0;background-color: #fff!important;\">Transfer </td>                             " +
               "        <td style=\"-webkit-box-sizing: border-box;-moz-box-sizing: border-box;box-sizing: border-box;padding: 0;background-color: #fff!important;\">" + item.city + ": " + item.pickup + " / " + item.dropoff + "</td>                             " +
               "        <td style=\"-webkit-box-sizing: border-box;-moz-box-sizing: border-box;box-sizing: border-box;padding: 0;background-color: #fff!important;\">" + item.date.ToString("dd.MM.yyyy") + " </td>                            " +
               "        <td style=\"-webkit-box-sizing: border-box;-moz-box-sizing: border-box;box-sizing: border-box;padding: 0;background-color: #fff!important;\">" + item.flightCode + "</td>                                           " +
               "        <td style=\"-webkit-box-sizing: border-box;-moz-box-sizing: border-box;box-sizing: border-box;padding: 0;background-color: #fff!important;\">" + item.pax + "</td>                                           " +
               "        <td style=\"-webkit-box-sizing: border-box;-moz-box-sizing: border-box;box-sizing: border-box;padding: 0;background-color: #fff!important;\">" + shared.DoFormat(item.Total) + " </td>                                           " +
               "    </tr>                                                   ";
            }
            foreach (var item in res.ExtraService)
            {

                table +=
               "    <tr style=\"-webkit-box-sizing: border-box;-moz-box-sizing: border-box;box-sizing: border-box;page-break-inside: avoid; text-align: left;\">                                                    " +
               "        <td style=\"-webkit-box-sizing: border-box;-moz-box-sizing: border-box;box-sizing: border-box;padding: 0;background-color: #fff!important;\">Package / Extra </td>                             " +
               "        <td style=\"-webkit-box-sizing: border-box;-moz-box-sizing: border-box;box-sizing: border-box;padding: 0;background-color: #fff!important;\">" + item.name + "</td>                             " +
               "        <td style=\"-webkit-box-sizing: border-box;-moz-box-sizing: border-box;box-sizing: border-box;padding: 0;background-color: #fff!important;\">" + item.date.ToString("dd.MM.yyyy") + " </td>                            " +
               "        <td style=\"-webkit-box-sizing: border-box;-moz-box-sizing: border-box;box-sizing: border-box;padding: 0;background-color: #fff!important;\">" + item.customer + "</td>                                           " +
               "        <td style=\"-webkit-box-sizing: border-box;-moz-box-sizing: border-box;box-sizing: border-box;padding: 0;background-color: #fff!important;\">" + item.Pax + "</td>                                           " +
               "        <td style=\"-webkit-box-sizing: border-box;-moz-box-sizing: border-box;box-sizing: border-box;padding: 0;background-color: #fff!important;\">" + shared.DoFormat(item.Total * item.Pax.Value) + " </td>                                           " +
               "    </tr>                                                   ";
            }
            table += " </table>";
            return table;

        }

        private static string BuildMailConfirmation(Reservations res)
        {
            string body = "";

            if (res.Rhotel.Count > 0)
            {
                body += WriteHotelsTables(res);
            }
            if (res.Tour.Count > 0)
            {
                body += WriteToursTables(res);
            }
            if (res.Transfers.Count > 0)
            {
                body += WriteTransferTables(res);
            }
            if (res.ExtraService.Count > 0)
            {
                body += WritePackageTables(res);
            }

            body += WriteTotalTable(res);

            body += "<br/><p>You may check your reservation details on web browser from ";//"<div style=\"width:600px\">";

            body += "<a href='" + HttpContext.Current.Request.Url.Host + "/out/invoice/" + res.ID + "?eref=" + res.Agency.ID + "'>here</a></p>";


            return body;

        }

        private static string WriteHotelsTables(Reservations res)
        {
            string table = "";
            foreach (var item in res.Rhotel)
            {
                table +=
                "<table width=\"700\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" style=\"border: 0px #777 solid; font-family:'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; font-size:13px\">" +
                 "<tr>" +
                   " <th style=\"border:1px #e5e5e5 solid;border-right:0;padding:5px\" width=\"200\">Hotel Reservations</th>" +
                    "<th style=\"border:1px #e5e5e5 solid;border-left:0;padding:5px\" width=\"500\">  " + item.name + "</th>" +
                "</tr>" +
                  " <tr><td colspan=\"2\" >" +
                    "<table width=\"700\" cellspacing=\"0\" cellpadding=\"2\" border=\"0\" style=\"border:0px #777 solid; font-family:'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; font-size:11px\">";

                bool header = true;

                foreach (var room in item.RHRoom)
                {

                    if (header)
                    {
                        table +=
                            " <tr>" +
                            "<th style=\"border:1px #e5e5e5 solid;\" width=\"200\">Guests</th>" +
                            "<th style=\"border:1px #e5e5e5 solid;\"  width=\"200\">Room Details</th>" +
                            "<th style=\"border:1px #e5e5e5 solid;\"  width=\"150\" align=\"center\">Dates</th>" +
                            "<th style=\"border:1px #e5e5e5 solid;\"  width=\"75\" align=\"center\">Rate</th>" +
                            "<th style=\"border:1px #e5e5e5 solid;\"  width=\"75\" align=\"center\">Total</th>" +
                        "</tr>";
                        header = false;
                    }

                    table +=

                      "<tr>" +
                        "    <td style=\"border:1px #e5e5e5 solid;\"  align=\"center\">" + room.guset + "</td>" +
                          "  <td style=\"border:1px #e5e5e5 solid;\"  align=\"center\">" + room.name + "X " + room.count + " - " + room.board + "<br/>" + room.type + "	</td>" +
                          "  <td style=\"border:1px #e5e5e5 solid;\"  align=\"center\">" + room.checkin.Value.ToString("dd.MM.yyyy") + " - " + room.checkout.Value.ToString("dd.MM.yyyy") + "</td>" +
                          "  <td style=\"border:1px #e5e5e5 solid;\"  align=\"center\">" + shared.DoFormat(room.price) + "</td>" +
                          "  <td style=\"border:1px #e5e5e5 solid;\"  align=\"center\">" + shared.DoFormat(room.price * room.count * (room.checkout.Value - room.checkin.Value).Days) + "</td>" +
                       " </tr>";

                }

                table += "" +
                "<tr>" +
                            "<td colspan=\"4\" align=\"center\"></td>" +
                            "<td style=\"border:1px #e5e5e5 solid;\" align=\"center\">" + shared.DoFormat(item.Total) + "</td>" +
                " </tr></table></td></tr></table><br/>";
            }

            return table;
        }

        private static string WritePackageTables(Reservations res)
        {
            bool header = true;
            var table = "";
            foreach (var item in res.ExtraService)
            {

                if (header)
                {
                    table +=
                      "<table width=\"700\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" style=\"border: 0px #777 solid; font-family:'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; font-size:13px\">" +
                       "<tr>" +
                         " <th style=\"border:1px #e5e5e5 solid;border-right:0;padding:5px\" width=\"200\">Package-Extra Reservations</th>" +
                          "<th style=\"border:1px #e5e5e5 solid;border-left:0;padding:5px\" width=\"500\">  </th>" +
                      "</tr>" +
                        " <tr><td colspan=\"2\" >" +
                          "<table width=\"700\" cellspacing=\"0\" cellpadding=\"2\" border=\"0\" style=\"border:0px #777 solid; font-family:'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; font-size:11px\">";

                    table +=
                            " <tr>" +
                            "<th style=\"border:1px #e5e5e5 solid;\" width=\"200\">Guests</th>" +
                            "<th style=\"border:1px #e5e5e5 solid;\"  width=\"200\"> Details</th>" +
                            "<th style=\"border:1px #e5e5e5 solid;\"  width=\"150\" align=\"center\">Dates</th>" +
                            "<th style=\"border:1px #e5e5e5 solid;\"  width=\"75\" align=\"center\">Rate</th>" +
                            "<th style=\"border:1px #e5e5e5 solid;\"  width=\"75\" align=\"center\">Total</th>" +
                        "</tr>";
                }

                table +=
                      "<tr>" +
                        "    <td style=\"border:1px #e5e5e5 solid;\"  align=\"center\">" + item.customer + "</td>" +
                          "  <td style=\"border:1px #e5e5e5 solid;\"  align=\"center\"> " + item.name + "	</td>" +
                          "  <td style=\"border:1px #e5e5e5 solid;\"  align=\"center\">" + item.date.ToString("dd.MM.yyyy") + "" + item.dateout.ToString("dd.MM.yyyy") + "</td>" +
                          "  <td style=\"border:1px #e5e5e5 solid;\"  align=\"center\">" + shared.DoFormat(item.Total) + "</td>" +
                          "  <td style=\"border:1px #e5e5e5 solid;\"  align=\"center\">" + shared.DoFormat(item.Total * item.Pax.Value) + "</td>" +
                       " </tr>";
           

            table += "" +
          " </table></td></tr></table><br/>";
            }
            return table;

        }

        private static string WriteTransferTables(Reservations res)
        {
            bool header = true;
            var table = "";
            foreach (var item in res.Transfers)
            {

                if (header)
                {
                    table +=
                      "<table width=\"700\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" style=\"border: 0px #777 solid; font-family:'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; font-size:13px\">" +
                       "<tr>" +
                         " <th style=\"border:1px #e5e5e5 solid;border-right:0;padding:5px\" width=\"200\">Transfer Reservations </th>" +
                          "<th style=\"border:1px #e5e5e5 solid;border-left:0;padding:5px\" width=\"500\">  </th>" +
                      "</tr>" +
                        " <tr><td colspan=\"2\" >" +
                          "<table width=\"700\" cellspacing=\"0\" cellpadding=\"2\" border=\"0\" style=\"border:0px #777 solid; font-family:'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; font-size:11px\">";

                    table +=
                            " <tr>" +
                            "<th style=\"border:1px #e5e5e5 solid;\" width=\"200\">Guests</th>" +
                            "<th style=\"border:1px #e5e5e5 solid;\"  width=\"200\"> Details</th>" +
                            "<th style=\"border:1px #e5e5e5 solid;\"  width=\"150\" align=\"center\">Dates</th>" +
                            "<th style=\"border:1px #e5e5e5 solid;\"  width=\"75\" align=\"center\">Rate</th>" +
                            "<th style=\"border:1px #e5e5e5 solid;\"  width=\"75\" align=\"center\">Total</th>" +
                        "</tr>";
                    header = false;
                }
                string flight = !string.IsNullOrWhiteSpace(item.flightCode) ? "Flight: " + item.flightCode + " " : "";
                string time = (!string.IsNullOrWhiteSpace(item.flightTime) ? " / " + item.flightTime + " " : "");
                string dropoff = (!string.IsNullOrWhiteSpace(item.dropoff) ? "<br/> Dropoff: " + item.dropoff + " " : "");
                table +=
                      "<tr>" +
                        "    <td style=\"border:1px #e5e5e5 solid;\"  align=\"center\">" + item.customer + "</td>" +
                          "  <td style=\"border:1px #e5e5e5 solid;\"  align=\"center\"> Pickup: " + item.city + " / " + item.pickup + dropoff + " <br/> " + flight + time + "Pax: " + item.pax + "	</td>" +
                          "  <td style=\"border:1px #e5e5e5 solid;\"  align=\"center\">" + item.date.ToString("dd.MM.yyyy") + "</td>" +
                          "  <td style=\"border:1px #e5e5e5 solid;\"  align=\"center\">" + shared.DoFormat(item.Total) + "</td>" +
                          "  <td style=\"border:1px #e5e5e5 solid;\"  align=\"center\">" + shared.DoFormat(item.Total) + "</td>" +
                       " </tr>";
            }

            table += "" +
          "<tr>" +
                      "<td colspan=\"4\" align=\"center\"></td>" +
                      "<td style=\"border:1px #e5e5e5 solid;\" align=\"center\">" + shared.DoFormat(res.Transfers.Sum(d => d.Total)) + "</td>" +
          " </tr></table></td></tr></table><br/>";

            return table;
        }

        private static string WriteToursTables(Reservations res)
        {
            bool header = true;
            var table = "";
            foreach (var item in res.Tour)
            {

                if (header)
                {
                    table +=
                      "<table width=\"700\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" style=\"border: 0px #777 solid; font-family:'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; font-size:13px\">" +
                       "<tr>" +
                         " <th style=\"border:1px #e5e5e5 solid;border-right:0;padding:5px\" width=\"200\">Tour Reservations </th>" +
                          "<th style=\"border:1px #e5e5e5 solid;border-left:0;padding:5px\" width=\"500\">  " + item.name + "</th>" +
                      "</tr>" +
                        " <tr><td colspan=\"2\" >" +
                          "<table width=\"700\" cellspacing=\"0\" cellpadding=\"2\" border=\"0\" style=\"border:0px #777 solid; font-family:'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; font-size:11px\">";

                    table +=
                            " <tr>" +
                            "<th style=\"border:1px #e5e5e5 solid;\" width=\"200\">Guests</th>" +
                            "<th style=\"border:1px #e5e5e5 solid;\"  width=\"200\"> Details</th>" +
                            "<th style=\"border:1px #e5e5e5 solid;\"  width=\"150\" align=\"center\">Dates</th>" +
                            "<th style=\"border:1px #e5e5e5 solid;\"  width=\"75\" align=\"center\">Rate</th>" +
                            "<th style=\"border:1px #e5e5e5 solid;\"  width=\"75\" align=\"center\">Total</th>" +
                        "</tr>";
                   
                }

                string time = (string.IsNullOrWhiteSpace(item.pickuptime) ? " / " + item.pickuptime + " " : "");
                string dropoff = (string.IsNullOrWhiteSpace(item.dropoff) ? "<br/> Dropoff: " + item.dropoff + " " : "");
                table +=
                      "<tr>" +
                        "    <td style=\"border:1px #e5e5e5 solid;\"  align=\"center\">" + item.customer + "</td>" +
                          "  <td style=\"border:1px #e5e5e5 solid;\"  align=\"center\"> Pickup: " + item.city + " / " + item.pickup + time + dropoff + " <br/> Pax: " + item.Pax + "	</td>" +
                          "  <td style=\"border:1px #e5e5e5 solid;\"  align=\"center\">" + item.date.ToString("dd.MM.yyyy") + "</td>" +
                          "  <td style=\"border:1px #e5e5e5 solid;\"  align=\"center\">" + shared.DoFormat(item.Total) + "</td>" +
                          "  <td style=\"border:1px #e5e5e5 solid;\"  align=\"center\">" + shared.DoFormat(item.Total * item.Pax) + "</td>" +
                       " </tr>";

            

            table += "" +
          " </table></td></tr></table><br/>";
            }
            return table;

        }

        private static string WriteTotalTable(Reservations res)
        {

            return
            "   <table width=\"700\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" style=\"border:0px #e5e5e5  solid;margin-top:15px;font-family:'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; font-size:13px\" \"=\"\">   " +
            "   <tbody><tr>                                                                                                                                                                                                      " +
            "       <th width=\"390\"></th>                                                                                                                                                                                      " +
            "       <th align=\"center\" style=\"border:1px #e5e5e5  solid;\" width=\"150\">Total</th>                                                                                                                           " +
            "       <th align=\"center\" style=\"border:1px #e5e5e5  solid;\" width=\"150\">" + shared.DoFormat(res.price) + " </th>                                                                                                                        " +
            "   </tr>                                                                                                                                                                                                            " +
            "</tbody></table>                                                                                                                                                                                                    ";
        }
    }
}