using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Line.Data;
using Line.Models;
using Line.Data.Utility;
using System.Data.Entity;
namespace Line.Services
{
    public class ReportsService : IReportsService
    {
        private readonly DBEntities _Repository;
        private readonly IWorkContext _workcontext;
        private readonly IReservationService _Reservations;

        public ReportsService(IWorkContext workcontext, IReservationService Reservations)
        {
            _workcontext = workcontext;
            _Repository = new DBEntities();
            _Reservations = Reservations;
        }

        public int Getlastyear()
        {
            return _Repository.Reports.Count() > 0 ? _Repository.Reports.Max(d => d.Year) : 2007;
        }

        public void DeleteReportBase(int id)
        {
            throw new NotImplementedException();
        }

        public void DeleteUnpaidFile(int id)
        {
            throw new NotImplementedException();
        }

        public List<ShortReport> GetAgencyReport(int year)
        {
            if (year == DateTime.Now.Year) // read from database
            {
                var Searchmodel = new Models.ReservationSearchModel { CheckoutDatefrom = new DateTime(DateTime.Now.Year, 1, 1), CheckoutDateto = new DateTime(DateTime.Now.Year + 1, 1, 1) };
                var CurrentRes = _Reservations.SearchReservations(Searchmodel, true);
                return CurrentRes.GroupBy(d => d.ref_agency)
                .Select(x => new ShortReport
                {
                    Name = x.FirstOrDefault().Agency.AgencyName,
                    Count = x.Count(),
                    Profit = x.Sum(d => d.price) - x.Sum(d => d.cost.Value),
                    Sales = x.Sum(c => c.price)
                }).OrderByDescending(v => v.Sales).Take(15).ToList();

            }

            return _Repository.Reports.Where(d => d.Year == year)
                .GroupBy(d => d.ref_agency)
                .Select(x => new ShortReport
                {
                    Name = x.FirstOrDefault().Agency.AgencyName,
                    Count = x.FirstOrDefault().Agency.Reservations.Count(z => z.checkindate.Year == year),
                    Profit = x.Sum(c => c.Total) - x.Sum(c => c.Costs),
                    Sales = x.Sum(c => c.Total)
                }).OrderByDescending(v => v.Sales).Take(15).ToList();
        }

        public List<ShortReport> GetAgencyReport(int year,IEnumerable<Reservations> Items)
        {
                return Items.Where(d=> d.balanceDate.Year == year).GroupBy(d => d.ref_agency)
                .Select(x => new ShortReport
                {
                    Name = x.FirstOrDefault().Agency.AgencyName,
                    Count = x.Count(),
                    Profit = x.Sum(d => d.price) - x.Sum(d => d.cost.Value),
                    Sales = x.Sum(c => c.price)
                }).OrderByDescending(v => v.Sales).Take(15).ToList();
        }

        public List<ShortReport> GetAgencyReportByCountry(int Startyear, string name = "")
        {
            var ReturnList = new List<ShortReport>();
            for (int i = Startyear; i <= DateTime.Now.Year; i++)
            {
                var Searchmodel = new Models.ReservationSearchModel { CheckoutDatefrom = new DateTime(i, 1, 1), CheckoutDateto = new DateTime(i + 1, 1, 1), Country = name };
                var CurrentRes = _Reservations.SearchReservations(Searchmodel, true);
                ReturnList.AddRange(CurrentRes.GroupBy(d => d.ref_agency)
                .Select(x => new ShortReport
                {
                    Year = i,
                    AgencyId = x.Key.Value,
                    Name = x.FirstOrDefault().Agency.AgencyName,
                    Count = x.Count(),
                    Profit = x.Sum(d => d.price) - x.Sum(d => d.cost.Value),
                    Sales = x.Sum(d => d.price)
                }).ToList());
            }

            return ReturnList.OrderByDescending(d => d.Sales).ToList();

        }

        public List<MonthSales> GetAgencyReportById(int Startyear, int Id)
        {
            var ReturnList = new List<MonthSales>();
            for (int i = Startyear; i <= DateTime.Now.Year; i++)
            {
                //currenct  Year
                var total = new Models.ReservationSearchModel { CheckoutDatefrom = new DateTime(i, 1, 1), CheckoutDateto = new DateTime(i + 1, 1, 1), AgencyId = new List<int> { Id } };
                var CurrentRes = _Reservations.SearchReservations(total, true);
                var CurrentYear = new MonthSales
                {
                    Year = i,
                    Jan = CurrentRes.Where(d => d.balanceDate >= new DateTime(i, 1, 1) && d.balanceDate < new DateTime(i, 2, 1)).Sum(x => x.price),
                    Feb = CurrentRes.Where(d => d.balanceDate >= new DateTime(i, 2, 1) && d.balanceDate < new DateTime(i, 3, 1)).Sum(x => x.price),
                    Mar = CurrentRes.Where(d => d.balanceDate >= new DateTime(i, 3, 1) && d.balanceDate < new DateTime(i, 4, 1)).Sum(x => x.price),
                    Apr = CurrentRes.Where(d => d.balanceDate >= new DateTime(i, 4, 1) && d.balanceDate < new DateTime(i, 5, 1)).Sum(x => x.price),
                    May = CurrentRes.Where(d => d.balanceDate >= new DateTime(i, 5, 1) && d.balanceDate < new DateTime(i, 6, 1)).Sum(x => x.price),
                    Jun = CurrentRes.Where(d => d.balanceDate >= new DateTime(i, 6, 1) && d.balanceDate < new DateTime(i, 7, 1)).Sum(x => x.price),
                    Jul = CurrentRes.Where(d => d.balanceDate >= new DateTime(i, 7, 1) && d.balanceDate < new DateTime(i, 8, 1)).Sum(x => x.price),
                    Aug = CurrentRes.Where(d => d.balanceDate >= new DateTime(i, 8, 1) && d.balanceDate < new DateTime(i, 9, 1)).Sum(x => x.price),
                    Sep = CurrentRes.Where(d => d.balanceDate >= new DateTime(i, 9, 1) && d.balanceDate < new DateTime(i, 10, 1)).Sum(x => x.price),
                    Oct = CurrentRes.Where(d => d.balanceDate >= new DateTime(i, 10, 1) && d.balanceDate < new DateTime(i, 11, 1)).Sum(x => x.price),
                    Nov = CurrentRes.Where(d => d.balanceDate >= new DateTime(i, 11, 1) && d.balanceDate < new DateTime(i, 12, 1)).Sum(x => x.price),
                    Dec = CurrentRes.Where(d => d.balanceDate >= new DateTime(i, 12, 1) && d.balanceDate < new DateTime(i + 1, 1, 1)).Sum(x => x.price),
                    Total = CurrentRes.Sum(x => x.price),
                };

                ReturnList.Insert(0, CurrentYear);
            }
            return ReturnList.OrderByDescending(d => d.Year).ToList();
        }

        public List<ShortReport> GetAgencyReportByMemberId(int Startyear, int Id)
        {
            var ReturnList = new List<ShortReport>();
            for (int i = Startyear; i <= DateTime.Now.Year; i++)
            {
                var Searchmodel = new Models.ReservationSearchModel
                {
                    CheckoutDatefrom = new DateTime(Startyear, 1, 1),
                    CheckoutDateto = new DateTime(DateTime.Now.Year + 1, 1, 1),
                    CreatedBy = new List<int> { Id }
                };
                var CurrentRes = _Reservations.SearchReservations(Searchmodel, true);
                ReturnList.AddRange(CurrentRes.GroupBy(d => d.ref_agency)
                .Select(x => new ShortReport
                {
                    Year = i,
                    Name = x.FirstOrDefault().Agency.AgencyName,
                    Count = x.Count(),
                    Profit = x.Sum(d => d.price) - x.Sum(d => d.cost.Value),
                    Sales = x.Sum(d => d.price)
                }).ToList());
            }

            return ReturnList.OrderByDescending(d => d.Sales).ToList();
        }

        public List<ShortReport> GetCountriesReport(int year)
        {
            if (year == DateTime.Now.Year)
            {
                var Searchmodel = new Models.ReservationSearchModel { CheckoutDatefrom = new DateTime(DateTime.Now.Year, 1, 1), CheckoutDateto = new DateTime(DateTime.Now.Year + 1, 1, 1) };
                var CurrentRes = _Reservations.SearchReservations(Searchmodel, true);
                return CurrentRes.GroupBy(d => d.Agency.Country)
                .Select(x => new ShortReport
                {
                    Year = year,
                    Name = x.FirstOrDefault().Agency.Country,
                    Count = x.Count(),
                    Profit = x.Sum(d => d.price) - x.Sum(d => d.cost.Value),
                    Sales = x.Sum(c => c.price)
                }).OrderByDescending(v => v.Sales).Take(15).ToList();
            }
            var Sales = _Repository.Reports.Where(d => d.Year == year)
                 .Select(x => new ShortReport
                 {
                     Name = x.Country,
                     Count = x.Agency.Reservations.Count(z => z.checkindate.Year == year),
                     Profit = x.Total - x.Costs,
                     Sales = x.Total
                 }).OrderBy(d => d.Sales).ToList();

            return Sales.GroupBy(d => d.Name)
               .Select(x => new ShortReport
               {
                   Year = year,
                   Name = x.Key,
                   Count = x.Sum(d => d.Count),
                   Profit = x.Sum(d => d.Profit),
                   Sales = x.Sum(d => d.Sales)
               })
               .OrderByDescending(d => d.Sales).Take(15).ToList();
        }

        public List<ShortReport> GetCountriesReport(IEnumerable<Reservations> Items)
        {

            return Items.GroupBy(d => new { d.Agency.Country , d.balanceDate.Year})
            .Select(x => new ShortReport
            {
                Year = x.Key.Year,
                Name = x.Key.Country,
                Count = x.Count(),
                Sales = x.Sum(c => c.price)
            }).ToList();

            //var Result = new List<ShortReport>();

            //var x = Items.Select(d => d.Agency.Country).Distinct();
            //foreach (var item in x)
            //{
            //    var country = Items.Where(d => d.balanceDate.Year == year && d.Agency.Country == item);
                
            //    var z =  new ShortReport
            //    {
            //        Year = year,
            //        Name = item,
            //        Count = country.Count(),
            //        Profit = country.Sum(d => d.price - d.cost.Value),
            //        Sales = country.Sum(c => c.price)
            //    };
            //    Result.Add(z);
            //}
            //return Result.OrderByDescending(v => v.Sales).Take(15).ToList();
       
        }

        public List<ShortReport> GetHotelsReport(int year)
        {
            var sales = _Repository.RHRoom.Where(d => (d.Rhotel.Reservations.status == ReservationStatus.Confirmed.ToString() || d.Rhotel.Reservations.status == ReservationStatus.ReConfirmed.ToString()) && d.checkin.Value.Year == year);
            var RoomsUpdate = new List<ShortReport>();
            if (sales.Count() == 0)
            {
                return RoomsUpdate;
            }
            var test = sales.GroupBy(d => d.Rhotel.name).Select(x => new ShortReport
            {
                Year = year,
                Name = x.Key,
                Count = x.Sum(m => System.Data.Entity.SqlServer.SqlFunctions.DateDiff("day", m.checkin.Value, m.checkout.Value).Value),
            }).OrderByDescending(x => x.Count);

            return test.ToList();
            
        }
        public List<ShortReport> GetHotelsReport(int year, IEnumerable<Reservations> Items)
        {
            var result = new List<ShortReport>();
            var HotelRooms = new List<RHRoom>();

            //Items.AsQueryable().Include(d=> d.Rhotel.)
            //foreach (var item in Items)
            //{
            //    foreach (var hotel in  item.Rhotel)
            //    {
            //        foreach (var room in hotel.RHRoom)
            //        {
            //            int count = room.checkout.HasValue && room.checkin.HasValue ? (int)(room.checkout.Value - room.checkin.Value).Days : 0;
            //            result.Add(new ShortReport { Name = hotel.name, Count = count });
            //        }
            //    }
            //}
            //var gp = result.GroupBy(d => d.Name).Select(x => new ShortReport
            //{
            //    Year = year,
            //    Name = x.Key,
            //    Count = x.Sum(m => m.Count),
            //}).OrderByDescending(x => x.Count).Take(15);


            var sales = _Repository.RHRoom.Where(d => (d.Rhotel.Reservations.status == ReservationStatus.Confirmed.ToString() || d.Rhotel.Reservations.status == ReservationStatus.ReConfirmed.ToString()) && d.checkin.Value.Year == year);
            var RoomsUpdate = new List<ShortReport>();
            if (sales.Count() == 0)
            {
                return RoomsUpdate;
            }

            var test = sales.GroupBy(d=> d.Rhotel.name).Select(x => new ShortReport
            {
                Year = year,
                Name = x.Key,
                Count = x.Sum(m => System.Data.Entity.SqlServer.SqlFunctions.DateDiff("day", m.checkin.Value, m.checkout.Value).Value),
            }).OrderByDescending(x => x.Count).Take(15);

            return test.ToList();

            //foreach (var item in sales)
            //{
            //    int count = item.checkout.HasValue && item.checkin.HasValue ? (int)(item.checkout.Value - item.checkin.Value).Days : 0;
            //    RoomsUpdate.Add(new ShortReport { Name = item.Rhotel.name, Count = count });
            //}
            //var gp = RoomsUpdate.GroupBy(d => d.Name).Select(x => new ShortReport
            //{
            //    Year = year,
            //    Name = x.Key,
            //    Count = x.Sum(m =>  m.Count),
            //}).OrderByDescending(x => x.Count).Take(15);


        }
        public List<ShortReport> GetHotelReportById(int year, int Id)
        {
            var sales = _Repository.RHRoom.Where(d => (d.Rhotel.Reservations.status == ReservationStatus.Confirmed.ToString() || d.Rhotel.Reservations.status == ReservationStatus.ReConfirmed.ToString()) && d.checkin.Value.Year == year && d.HID == Id);
            var RoomsUpdate = new List<ShortReport>();
            if (sales.Count() == 0)
            {
                return RoomsUpdate;
            }
            foreach (var item in sales)
            {
                int count = item.checkout.HasValue && item.checkin.HasValue ? (int)(item.checkout.Value - item.checkin.Value).Days : 0;
                RoomsUpdate.Add(new ShortReport { Name = item.Rhotel.name, Count = count });
            }
            var gp = RoomsUpdate.GroupBy(d => d.Name).Select(x => new ShortReport
            {
                Year = year,
                Name = x.Key,
                Count = x.Sum(m => m.Count),
            }).OrderByDescending(x => x.Count);

            return gp.ToList();
        }

        public Report GetReportBaseById(int id)
        {
            throw new NotImplementedException();
        }

        public List<ShortReport> GetReports(ReportType Type, ReportCategory Category)
        {
            return null;
        }

        public List<MonthSales> GetSales(int startYear)
        {
            //var Sales = _Repository.Reports.Where(x => x.Year >= startYear).GroupBy(d => d.Year).Select(d => new MonthSales
            //{
            //    Year = d.Key,
            //    Jan = d.Sum(x => x.Jan),
            //    Feb = d.Sum(x => x.Feb),
            //    Mar = d.Sum(x => x.Mar),
            //    Apr = d.Sum(x => x.Apr),
            //    May = d.Sum(x => x.May),
            //    Jun = d.Sum(x => x.Jun),
            //    Jul = d.Sum(x => x.Jul),
            //    Aug = d.Sum(x => x.Aug),
            //    Sep = d.Sum(x => x.Sep),
            //    Oct = d.Sum(x => x.Oct),
            //    Nov = d.Sum(x => x.Nov),
            //    Dec = d.Sum(x => x.Dec),
            //    Total = d.Sum(x => x.Total),
            //});
            var Returnlist = new List<MonthSales>();// Sales.OrderByDescending(d => d.Year).ToList();

            //currenct  Year
            var total = new Models.ReservationSearchModel { CheckoutDatefrom = new DateTime(startYear, 1, 1), CheckoutDateto = new DateTime(DateTime.Now.Year + 1, 1, 1) };
            var CurrentRes = _Reservations.SearchReservations(total, true);
            for (int i = startYear; i <= DateTime.Now.Year; i++)
            {
                var CurrentYear = new MonthSales
                {
                    Year = i,
                    Jan = CurrentRes.Where(d => d.balanceDate >= new DateTime(i, 1, 1) && d.balanceDate <  new DateTime(i, 2, 1)).Sum(x => x.price),
                    Feb = CurrentRes.Where(d => d.balanceDate >= new DateTime(i, 2, 1) && d.balanceDate <  new DateTime(i, 3, 1)).Sum(x => x.price),
                    Mar = CurrentRes.Where(d => d.balanceDate >= new DateTime(i, 3, 1) && d.balanceDate <  new DateTime(i, 4, 1)).Sum(x => x.price),
                    Apr = CurrentRes.Where(d => d.balanceDate >= new DateTime(i, 4, 1) && d.balanceDate <  new DateTime(i, 5, 1)).Sum(x => x.price),
                    May = CurrentRes.Where(d => d.balanceDate >= new DateTime(i, 5, 1) && d.balanceDate <  new DateTime(i, 6, 1)).Sum(x => x.price),
                    Jun = CurrentRes.Where(d => d.balanceDate >= new DateTime(i, 6, 1) && d.balanceDate <  new DateTime(i, 7, 1)).Sum(x => x.price),
                    Jul = CurrentRes.Where(d => d.balanceDate >= new DateTime(i, 7, 1) && d.balanceDate <  new DateTime(i, 8, 1)).Sum(x => x.price),
                    Aug = CurrentRes.Where(d => d.balanceDate >= new DateTime(i, 8, 1) && d.balanceDate <  new DateTime(i, 9, 1)).Sum(x => x.price),
                    Sep = CurrentRes.Where(d => d.balanceDate >= new DateTime(i, 9, 1) && d.balanceDate <  new DateTime(i, 10, 1)).Sum(x => x.price),
                    Oct = CurrentRes.Where(d => d.balanceDate >= new DateTime(i, 10, 1) && d.balanceDate < new DateTime(i, 11, 1)).Sum(x => x.price),
                    Nov = CurrentRes.Where(d => d.balanceDate >= new DateTime(i, 11, 1) && d.balanceDate < new DateTime(i, 12, 1)).Sum(x => x.price),
                    Dec = CurrentRes.Where(d => d.balanceDate >= new DateTime(i, 12, 1) && d.balanceDate < new DateTime(i + 1, 1, 1)).Sum(x => x.price),
                    Total = CurrentRes.Where(d => d.balanceDate.Year == i).Sum(x => x.price),
                };
                Returnlist.Add(CurrentYear);
            }
            return Returnlist;
        }

        public List<MonthSales> GetSalesByRegistrations(IEnumerable<Reservations> Items)
        {
            var ReturnList = new List<MonthSales>();
            //currenct  Year
            foreach (var item in Items.Select(d=> d.balanceDate.Year).Distinct())
            {
                var CurrentYear = new MonthSales
                {
                    Year = item,
                    Jan = Items.Where(d => d.balanceDate >= new DateTime(item, 1, 1) && d.balanceDate <  new DateTime(item, 2, 1)).Sum(x => x.price),
                    Feb = Items.Where(d => d.balanceDate >= new DateTime(item, 2, 1) && d.balanceDate <  new DateTime(item, 3, 1)).Sum(x => x.price),
                    Mar = Items.Where(d => d.balanceDate >= new DateTime(item, 3, 1) && d.balanceDate <  new DateTime(item, 4, 1)).Sum(x => x.price),
                    Apr = Items.Where(d => d.balanceDate >= new DateTime(item, 4, 1) && d.balanceDate <  new DateTime(item, 5, 1)).Sum(x => x.price),
                    May = Items.Where(d => d.balanceDate >= new DateTime(item, 5, 1) && d.balanceDate <  new DateTime(item, 6, 1)).Sum(x => x.price),
                    Jun = Items.Where(d => d.balanceDate >= new DateTime(item, 6, 1) && d.balanceDate <  new DateTime(item, 7, 1)).Sum(x => x.price),
                    Jul = Items.Where(d => d.balanceDate >= new DateTime(item, 7, 1) && d.balanceDate <  new DateTime(item, 8, 1)).Sum(x => x.price),
                    Aug = Items.Where(d => d.balanceDate >= new DateTime(item, 8, 1) && d.balanceDate <  new DateTime(item, 9, 1)).Sum(x => x.price),
                    Sep = Items.Where(d => d.balanceDate >= new DateTime(item, 9, 1) && d.balanceDate <  new DateTime(item, 10, 1)).Sum(x => x.price),
                    Oct = Items.Where(d => d.balanceDate >= new DateTime(item, 10, 1) && d.balanceDate < new DateTime(item, 11, 1)).Sum(x => x.price),
                    Nov = Items.Where(d => d.balanceDate >= new DateTime(item, 11, 1) && d.balanceDate < new DateTime(item, 12, 1)).Sum(x => x.price),
                    Dec = Items.Where(d => d.balanceDate >= new DateTime(item, 12, 1) && d.balanceDate < new DateTime(item + 1, 1, 1)).Sum(x => x.price),
                    Total = Items.Where(d=> d.balanceDate.Year == item).Sum(x => x.price),
                };
                ReturnList.Add(CurrentYear);
            }
            return ReturnList;
        }


        public List<MonthSales> GetSalesByStaff(IEnumerable<Reservations> Items)
        {
            var ReturnList = new List<MonthSales>();
            //currenct  Year
            var Users = _Repository.Members.Where(d => d.active);
            foreach (var item in Items.Select(d => d.date.Value.Year).Distinct())
            {
                foreach (var user in Users)
                {
                    var CurrentYear = new MonthSales
                    {
                        Year = item,
                        name = user.FirstName + user.LastName,
                        Jan = Items.Where(d => d.ref_member == user.Id && d.date >= new DateTime(item, 1, 1)  && d.date < new DateTime(item, 2, 1)).Sum(x => x.price),
                        Feb = Items.Where(d => d.ref_member == user.Id && d.date >= new DateTime(item, 2, 1)  && d.date < new DateTime(item, 3, 1)).Sum(x => x.price),
                        Mar = Items.Where(d => d.ref_member == user.Id && d.date >= new DateTime(item, 3, 1)  && d.date < new DateTime(item, 4, 1)).Sum(x => x.price),
                        Apr = Items.Where(d => d.ref_member == user.Id && d.date >= new DateTime(item, 4, 1)  && d.date < new DateTime(item, 5, 1)).Sum(x => x.price),
                        May = Items.Where(d => d.ref_member == user.Id && d.date >= new DateTime(item, 5, 1)  && d.date < new DateTime(item, 6, 1)).Sum(x => x.price),
                        Jun = Items.Where(d => d.ref_member == user.Id && d.date >= new DateTime(item, 6, 1)  && d.date < new DateTime(item, 7, 1)).Sum(x => x.price),
                        Jul = Items.Where(d => d.ref_member == user.Id && d.date >= new DateTime(item, 7, 1)  && d.date < new DateTime(item, 8, 1)).Sum(x => x.price),
                        Aug = Items.Where(d => d.ref_member == user.Id && d.date >= new DateTime(item, 8, 1)  && d.date < new DateTime(item, 9, 1)).Sum(x => x.price),
                        Sep = Items.Where(d => d.ref_member == user.Id && d.date >= new DateTime(item, 9, 1)  && d.date < new DateTime(item, 10, 1)).Sum(x => x.price),
                        Oct = Items.Where(d => d.ref_member == user.Id && d.date >= new DateTime(item, 10, 1) && d.date < new DateTime(item, 11, 1)).Sum(x => x.price),
                        Nov = Items.Where(d => d.ref_member == user.Id && d.date >= new DateTime(item, 11, 1) && d.date < new DateTime(item, 12, 1)).Sum(x => x.price),
                        Dec = Items.Where(d => d.ref_member == user.Id && d.date >= new DateTime(item, 12, 1) && d.date < new DateTime(item + 1, 1, 1)).Sum(x => x.price),
                        Total = Items.Where(d => d.ref_member == user.Id && d.date.Value.Year == item).Sum(x => x.price),
                    };
                    ReturnList.Add(CurrentYear);
                }
            }
            return ReturnList;
        }

        public List<MonthSales> GetProfits(int startYear)
        {
            //var Sales = _Repository.Reports.Where(x => x.Year >= startYear).GroupBy(d => d.Year).Select(d => new MonthSales
            //{
            //    Year = d.Key,
            //    Jan = d.Sum(x => x.Jan - x.JanCost),
            //    Feb = d.Sum(x => x.Feb - x.FebCost),
            //    Mar = d.Sum(x => x.Mar - x.MarCost),
            //    Apr = d.Sum(x => x.Apr - x.AprCost),
            //    May = d.Sum(x => x.May - x.MayCost),
            //    Jun = d.Sum(x => x.Jun - x.JunCost),
            //    Jul = d.Sum(x => x.Jul - x.JulCost),
            //    Aug = d.Sum(x => x.Aug - x.AugCost),
            //    Sep = d.Sum(x => x.Sep - x.SepCost),
            //    Oct = d.Sum(x => x.Oct - x.OctCost),
            //    Nov = d.Sum(x => x.Nov - x.NovCost),
            //    Dec = d.Sum(x => x.Dec - x.DecCost),
            //    Total = d.Sum(x => x.Total - x.Total),
            //});
            var Returnlist = new List<MonthSales>();
            for (int i = startYear; i <= DateTime.Now.Year; i++)
            {
                var total = new Models.ReservationSearchModel { CheckoutDatefrom = new DateTime(i, 1, 1), CheckoutDateto = new DateTime(i + 1, 1, 1) };
                var CurrentRes = _Reservations.SearchReservations(total, true);
                var CurrentYear = new MonthSales
                {
                    Year = i,
                    Jan = CurrentRes.Where(d => d.balanceDate >= new DateTime(i, 1, 1) && d.balanceDate <  new DateTime(i, 2, 1)).Sum(x => (x.price - x.cost.Value)),
                    Feb = CurrentRes.Where(d => d.balanceDate >= new DateTime(i, 2, 1) && d.balanceDate <  new DateTime(i, 3, 1)).Sum(x => x.price - x.cost.Value),
                    Mar = CurrentRes.Where(d => d.balanceDate >= new DateTime(i, 3, 1) && d.balanceDate <  new DateTime(i, 4, 1)).Sum(x => x.price - x.cost.Value),
                    Apr = CurrentRes.Where(d => d.balanceDate >= new DateTime(i, 4, 1) && d.balanceDate <  new DateTime(i, 5, 1)).Sum(x => x.price - x.cost.Value),
                    May = CurrentRes.Where(d => d.balanceDate >= new DateTime(i, 5, 1) && d.balanceDate <  new DateTime(i, 6, 1)).Sum(x => x.price - x.cost.Value),
                    Jun = CurrentRes.Where(d => d.balanceDate >= new DateTime(i, 6, 1) && d.balanceDate <  new DateTime(i, 7, 1)).Sum(x => x.price - x.cost.Value),
                    Jul = CurrentRes.Where(d => d.balanceDate >= new DateTime(i, 7, 1) && d.balanceDate <  new DateTime(i, 8, 1)).Sum(x => x.price - x.cost.Value),
                    Aug = CurrentRes.Where(d => d.balanceDate >= new DateTime(i, 8, 1) && d.balanceDate <  new DateTime(i, 9, 1)).Sum(x => x.price - x.cost.Value),
                    Sep = CurrentRes.Where(d => d.balanceDate >= new DateTime(i, 9, 1) && d.balanceDate <  new DateTime(i, 10, 1)).Sum(x => x.price - x.cost.Value),
                    Oct = CurrentRes.Where(d => d.balanceDate >= new DateTime(i, 10, 1) && d.balanceDate < new DateTime(i, 11, 1)).Sum(x => x.price - x.cost.Value),
                    Nov = CurrentRes.Where(d => d.balanceDate >= new DateTime(i, 11, 1) && d.balanceDate < new DateTime(i, 12, 1)).Sum(x => x.price - x.cost.Value),
                    Dec = CurrentRes.Where(d => d.balanceDate >= new DateTime(i, 12, 1) && d.balanceDate < new DateTime(i + 1, 1, 1)).Sum(x => x.price - x.cost.Value),
                    Total = CurrentRes.Sum(x => x.price - x.cost.Value),
                };
                Returnlist.Add(CurrentYear);
            }
            //currenct  Year
            
            return Returnlist;
        }
        public Unpaid GetUnpaidFileById(int id)
        {
            throw new NotImplementedException();
        }

        public List<YearSales> GetYearsDashboard()
        {
            //try to return last 5 Years
            var Results = new List<YearSales>();
            var Year = DateTime.Now.Year;
            string g = ReservationStatus.ReConfirmed.ToString();
            string y = ReservationStatus.Confirmed.ToString();
            for (int i = Year - 4; i <= Year; i++)
            {
                var SalesReport = _Repository.Reservations.Where(d => (d.status == g || d.status == y) && d.balanceDate.Year == i);
                var year = new YearSales
                {
                    Year = i,
                    Sales = SalesReport.Sum(x => x.price),
                    Profit = SalesReport.Sum(x => x.price - x.cost.Value)
                };
                Results.Add(year);
            }
            
            return Results.OrderBy(d => d.Year).ToList();
        }




        public Report InsertReportBase(Report value)
        {
            var dbitem = _Repository.Reports.Add(value);
            if (_Repository.EntityAdded(dbitem))
            {
                return dbitem;
            }
            return null;
        }

        public Unpaid InsertUnpaidFile(Unpaid value)
        {
            throw new NotImplementedException();
        }

        public List<Report> SearchReportBases(int FileId)
        {
            throw new NotImplementedException();
        }

        public List<Unpaid> SearchUnpaidFiles(int FileId)
        {
            throw new NotImplementedException();
        }

        public Report UpdateReportBase(Report value)
        {
            throw new NotImplementedException();
        }

        public void UpdateReportTable(int year)
        {
            DateTime firstDay = new DateTime(year, 1, 1);
            DateTime lastDay = new DateTime(year + 1, 1, 1);
            ReservationSearchModel Model = new ReservationSearchModel();
            Model.CheckoutDatefrom = firstDay;
            Model.CheckoutDateto = lastDay;
            var resvs = _Reservations.SearchReservations(Model, true);

            var Grouped = resvs.GroupBy(d => d.ref_agency).Select(x => new Report
            {
                ref_agency = x.Key.Value,
                Total = x.Sum(z => z.price),
                Costs = x.Sum(z => z.cost.Value),
                Year = year,
            });

            foreach (var item in Grouped.ToList())
            {
                var cresv = resvs.Where(d => d.ref_agency == item.ref_agency);

                item.Country = _Repository.Agency.Find(item.ref_agency).Country;

                item.Jan = cresv.Where(d => d.balanceDate.Month == 1).Sum(d => d.price);
                item.JanCost = cresv.Where(d => d.balanceDate.Month == 1).Sum(d => d.cost.Value);

                item.Feb = cresv.Where(d => d.balanceDate.Month == 2).Sum(d => d.price);
                item.FebCost = cresv.Where(d => d.balanceDate.Month == 2).Sum(d => d.cost.Value);

                item.Mar = cresv.Where(d => d.balanceDate.Month == 3).Sum(d => d.price);
                item.MarCost = cresv.Where(d => d.balanceDate.Month == 3).Sum(d => d.cost.Value);

                item.Apr = cresv.Where(d => d.balanceDate.Month == 4).Sum(d => d.price);
                item.AprCost = cresv.Where(d => d.balanceDate.Month == 4).Sum(d => d.cost.Value);

                item.May = cresv.Where(d => d.balanceDate.Month == 5).Sum(d => d.price);
                item.MayCost = cresv.Where(d => d.balanceDate.Month == 5).Sum(d => d.cost.Value);

                item.Jun = cresv.Where(d => d.balanceDate.Month == 6).Sum(d => d.price);
                item.JunCost = cresv.Where(d => d.balanceDate.Month == 6).Sum(d => d.cost.Value);

                item.Jul = cresv.Where(d => d.balanceDate.Month == 7).Sum(d => d.price);
                item.JulCost = cresv.Where(d => d.balanceDate.Month == 7).Sum(d => d.cost.Value);

                item.Aug = cresv.Where(d => d.balanceDate.Month == 8).Sum(d => d.price);
                item.AugCost = cresv.Where(d => d.balanceDate.Month == 8).Sum(d => d.cost.Value);

                item.Sep = cresv.Where(d => d.balanceDate.Month == 9).Sum(d => d.price);
                item.SepCost = cresv.Where(d => d.balanceDate.Month == 9).Sum(d => d.cost.Value);

                item.Oct = cresv.Where(d => d.balanceDate.Month == 10).Sum(d => d.price);
                item.OctCost = cresv.Where(d => d.balanceDate.Month == 10).Sum(d => d.cost.Value);

                item.Nov = cresv.Where(d => d.balanceDate.Month == 11).Sum(d => d.price);
                item.NovCost = cresv.Where(d => d.balanceDate.Month == 11).Sum(d => d.cost.Value);

                item.Dec = cresv.Where(d => d.balanceDate.Month == 12).Sum(d => d.price);
                item.DecCost = cresv.Where(d => d.balanceDate.Month == 12).Sum(d => d.cost.Value);

                InsertReportBase(item);

            }


        }

        public Unpaid UpdateUnpaidFile(Unpaid value)
        {
            throw new NotImplementedException();
        }

        public FileCounters GetSalesFilesCounters(int year)
        {
            return new FileCounters
            {
                Confirmed = _Repository.Reservations.Count(d => (d.status == ReservationStatus.Confirmed.ToString() || d.status == ReservationStatus.ReConfirmed.ToString()) && d.checkindate.Year == year),
                Canceled = _Repository.Reservations.Count(d => d.status == ReservationStatus.Canceled.ToString() && d.checkindate.Year == year),
                Requests = _Repository.Reservations.Count(d => d.status == ReservationStatus.Request.ToString() && d.checkindate.Year == year),
            };
        }

        public FileCounters GetCountryFilesCounters(int year, string country)
        {
            return new FileCounters
            {
                Confirmed = _Repository.Reservations.Count(d => d.Agency.Country.ToLower() == country.ToLower() && (d.status == ReservationStatus.Confirmed.ToString() || d.status == ReservationStatus.ReConfirmed.ToString()) && d.checkindate.Year == year),
                Canceled = _Repository.Reservations.Count(d => d.Agency.Country.ToLower() == country.ToLower() && d.status == ReservationStatus.Canceled.ToString() && d.checkindate.Year == year),
                Requests = _Repository.Reservations.Count(d => d.Agency.Country.ToLower() == country.ToLower() && d.status == ReservationStatus.Request.ToString() && d.checkindate.Year == year),
            };
        }

        public FileCounters GetAgencyFilesCounters(int year, int AgencyId)
        {
            return new FileCounters
            {
                Confirmed = _Repository.Reservations.Count(d => d.ref_agency.Value == AgencyId && (d.status == ReservationStatus.Confirmed.ToString() || d.status == ReservationStatus.ReConfirmed.ToString()) && d.checkindate.Year == year),
                Canceled = _Repository.Reservations.Count(d => d.ref_agency.Value == AgencyId && d.status == ReservationStatus.Canceled.ToString() && d.checkindate.Year == year),
                Requests = _Repository.Reservations.Count(d => d.ref_agency.Value == AgencyId && d.status == ReservationStatus.Request.ToString() && d.checkindate.Year == year),
            };
        }

        public FileCounters GetStaffFilesCounters(int year, int MemberId)
        {
            return new FileCounters
            {
                Confirmed = _Repository.Reservations.Count(d => d.ref_member == MemberId && (d.status == ReservationStatus.Confirmed.ToString() || d.status == ReservationStatus.ReConfirmed.ToString()) && d.checkindate.Year == year),
                Canceled = _Repository.Reservations.Count(d => d.ref_member == MemberId && d.status == ReservationStatus.Canceled.ToString() && d.checkindate.Year == year),
                Requests = _Repository.Reservations.Count(d => d.ref_member == MemberId && d.status == ReservationStatus.Request.ToString() && d.checkindate.Year == year),
            };
        }

        public IEnumerable<AccountingReport> GetDebtReport(string country = "")
        {
            var searchmodel = new ReservationSearchModel { OnlyUnpaidFiles = true };
            if (!string.IsNullOrEmpty(country))
            {
                searchmodel.Country = country;
            }
            var Reservations = _Reservations.SearchReservations(searchmodel, true);

            //var AgenctIds = Reservations.Select(d => d.ref_agency.Value).Distinct();

            var Model = Reservations.GroupBy(d => d.ref_agency)
                .Select(d => new AccountingReport { AgencyId = d.Key.Value,
                Balance = Reservations.Where(x => x.balanceDate <= DateTime.Now && x.ref_agency == d.Key).Sum(m => m.balance),
                LastBalance = Reservations.Where(x => x.balanceDate.Year < DateTime.Now.Year && x.ref_agency == d.Key).Sum(m => m.balance),
                CurrentBalance = Reservations.Where(x=> x.balanceDate.Year == DateTime.Now.Year && x.ref_agency == d.Key).Sum(m=> m.balance),
                AgencyName = d.FirstOrDefault().Agency.AgencyName,
                Country = d.FirstOrDefault().Agency.Country,
                LateBalance30 = Reservations.Where(x => x.balanceDate.AddDays(0) <= DateTime.Now && x.balanceDate.AddDays(30) >= DateTime.Now  && x.ref_agency == d.Key).Sum(m => m.balance),
                LateBalance60 = Reservations.Where(x => x.balanceDate.AddDays(30) <= DateTime.Now && x.balanceDate.AddDays(60) >= DateTime.Now && x.ref_agency == d.Key).Sum(m => m.balance),
                LateBalance90 = Reservations.Where(x => x.balanceDate.AddDays(60) <= DateTime.Now && x.balanceDate.AddDays(90) >= DateTime.Now && x.ref_agency == d.Key).Sum(m => m.balance),
                LateBalance120 = Reservations.Where(x => x.balanceDate.AddDays(90) <= DateTime.Now  && x.ref_agency == d.Key).Sum(m => m.balance),
                Future = Reservations.Where(x=> x.balanceDate >= DateTime.Now && x.ref_agency == d.Key).Sum(m=> m.balance),
                });

            Model = Model.Where(d => d.Balance > 1);

            return Model.OrderBy(d=> d.AgencyName);
        }
    }
}
