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
    public class ReservationService : IReservationService
    {
        private readonly DBEntities _Repository;
        private readonly IWorkContext _workcontext;

        public ReservationService(IWorkContext workcontext)
        {
            _workcontext = workcontext;
            _Repository = new DBEntities();
        }

        public List<Reservations> GetRecentFiles(int id, int v)
        {
           return _Repository.Reservations.Where(d => d.ref_member == id).OrderByDescending(d => d.ID).Take(v).ToList();
        }

        public void GetReservationDate(Reservations r, out DateTime icheckout,out DateTime icheckin)
        {
            var checkin = DateTime.Today.AddYears(5).Date;
            var checkout = DateTime.Today.AddYears(-5).Date;

            foreach (var hotel in r.Rhotel)
            {
                if (hotel.RHRoom.Count ==  0)
                {
                    continue;
                }
                var hdate = hotel.RHRoom.Min(d => d.checkin.Value);
                var odate = hotel.RHRoom.Max(d => d.checkout).Value;

                checkout = odate > checkout ? odate : checkout;
                checkin  = hdate <= checkin ? hdate : checkin;
            }

            if (r.Transfers.Count > 0)
            {
                var transfdate = r.Transfers.Min(d => d.date);
                checkin = transfdate <= checkin ? transfdate : checkin;

                var odate = r.Transfers.Max(d => d.date);
                checkout = odate > checkout ? odate : checkout;
            }


            if (r.Tour.Count > 0)
            {
                var tourdate = r.Tour.Min(d => d.date);
                checkin = tourdate <= checkin ? tourdate : checkin;

                var odate = r.Tour.Max(d => d.date);
                checkout = odate > checkout ? odate : checkout;
            }

            if (r.ExtraService.Count > 0)
            {
                var exdate = r.ExtraService.Min(d => d.date);
                checkin = exdate <= checkin ? exdate : checkin;
                var odate = r.ExtraService.Max(d => d.date);
                checkout = odate > checkout ? odate : checkout;
            }
            if (r.Flight.Count > 0)
            {
                var exdate = r.Flight.Min(d => d.date);
                checkin = exdate <= checkin ? exdate : checkin;
                var odate = r.Flight.Max(d => d.date);
                checkout = odate > checkout ? odate : checkout;
            }

            icheckout = checkout;
            icheckin = checkin;
        }

        public void CalculateReservation(int Id,bool ChangeStats = true)
        {
            var res = GetReservationById(Id);
            res.price = res.Rhotel.Sum(d => d.Total) + res.Transfers.Sum(d => d.Total) + res.Tour.Sum(d => d.Total * d.Pax) + res.ExtraService.Sum(d => d.Total * d.Pax).Value + res.Flight.Sum(d => d.Total * d.Pax).Value;
            res.cost = res.Rhotel.Sum(d => d.Cost) + res.Transfers.Sum(d => d.Cost) + res.Tour.Sum(d => d.Cost * d.Pax) + res.ExtraService.Sum(d => d.Cost * d.Pax).Value + res.Flight.Sum(d => d.Cost * d.Pax).Value;
            DateTime checkin,checkout;
            res.balance = res.price - res.Payments.Sum(d => d.Payment);
            GetReservationDate(res, out checkout, out checkin);
            res.balanceDate = checkout;
            res.checkindate = checkin;
            res.profit = res.price - res.cost;
            res.updatedate = DateTime.Now;
            res.updateuser = _workcontext.CurrentVisitor.GetFullName();
            if (ChangeStats)
            {
                res.status = ReservationStatus.Request.ToString();
            }
            UpdateReservation(res);
        }

        public void ClearHotelchangedRecoreds(int id)
        {
            var hotel = _Repository.hotelchange.FirstOrDefault(d => d.hid == id);
            if (hotel != null)
            {
                foreach (var room in hotel.ChangeRoom.ToList())
                {
                    DeleteRoomChange(room.ID);
                }
                DeleteHotelChange(hotel.id);
            }
        }

        public bool DeleteDeadline(int id)
        {
            var item = _Repository.DeadLine.Find(id);
            if (item != null)
            {
                var dbitem = _Repository.DeadLine.Remove(item);
                return _Repository.EntityDeleted(dbitem);
            }
            return false;
        }

        public bool DeleteFileNote(int id)
        {
            var item = _Repository.Filelogs.Find(id);
            if (item != null)
            {
                var dbitem = _Repository.Filelogs.Remove(item);
                return _Repository.EntityDeleted(dbitem);
            }
            return false;
        }

        public bool DeleteFlight(int id)
        {
            var item = _Repository.Flights.Find(id);
            if (item != null)
            {
                var dbitem = _Repository.Flights.Remove(item);
                return _Repository.EntityDeleted(dbitem);
            }
            return false;
        }

        public bool DeleteHotel(int id)
        {
            var item = _Repository.Rhotel.Find(id);
            if (item != null)
            {
                var dbitem = _Repository.Rhotel.Remove(item);
                return _Repository.EntityDeleted(dbitem);
            }
            return false;
        }

        public bool DeleteHotelChange(int id)
        {
            var item = _Repository.hotelchange.Find(id);
            if (item != null)
            {
                var dbitem = _Repository.hotelchange.Remove(item);
                return _Repository.EntityDeleted(dbitem);
            }
            return false;
        }

        public bool DeleteHotelRoom(int id)
        {
            var item = _Repository.RHRoom.Find(id);
            if (item != null)
            {
                var dbitem = _Repository.RHRoom.Remove(item);
                return _Repository.EntityDeleted(dbitem);
            }
            return false;
        }

        public bool DeleteInvoice(int id)
        {
            var item = _Repository.ininvoice.Find(id);
            if (item != null)
            {
                var dbitem = _Repository.ininvoice.Remove(item);
                return _Repository.EntityDeleted(dbitem);
            }
            return false;
        }

        public bool DeletePackage(int id)
        {
            var item = _Repository.ExtraService.Find(id);
            if (item != null)
            {
                var dbitem = _Repository.ExtraService.Remove(item);
                return _Repository.EntityDeleted(dbitem);
            }
            return false;
        }

        public bool DeleteReservation(int id)
        {
            var item = _Repository.Reservations.Find(id);
            if (item != null)
            {
                var dbitem = _Repository.Reservations.Remove(item);
                return _Repository.EntityDeleted(dbitem);
            }
            return false;
        }

        public bool DeleteRoomChange(int id)
        {
            var item = _Repository.ChangeRoom.Find(id);
            if (item != null)
            {
                var dbitem = _Repository.ChangeRoom.Remove(item);
                return _Repository.EntityDeleted(dbitem);
            }
            return false;
        }

        public bool DeleteTour(int id)
        {
            var item = _Repository.Tour.Find(id);
            if (item != null)
            {
                var dbitem = _Repository.Tour.Remove(item);
                return _Repository.EntityDeleted(dbitem);
            }
            return false;
        }

        public bool DeleteTransfer(int id)
        {
            var item = _Repository.Transfers.Find(id);
            if (item != null)
            {
                var dbitem = _Repository.Transfers.Remove(item);
                return _Repository.EntityDeleted(dbitem);
            }
            return false;
        }

        public DeadLine GetDeadlineById(int id)
        {
            return _Repository.DeadLine.Find(id);
        }

        public Filelog GetFileNoteById(int id)
        {
            return _Repository.Filelogs.Find(id);
        }

        public Flight GetFlightById(int id)
        {
            return _Repository.Flights.Find(id);
        }

        public Rhotel GetHotelById(int id)
        {
            return _Repository.Rhotel.Find(id);
        }

        public hotelchange GetHotelChangeById(int id)
        {
            return _Repository.hotelchange.Find(id);
        }

        public RHRoom GetHotelRoomById(int id)
        {
            return _Repository.RHRoom.Find(id);
        }

        public ininvoice GetInvoiceById(int id)
        {
            return _Repository.ininvoice.Find(id);
        }

        public ExtraService GetPackageById(int id)
        {
            return _Repository.ExtraService.Find(id);
        }


        public Reservations GetReservationById(int id)
        {
            return _Repository.Reservations.Find(id);
        }

        public ChangeRoom GetRoomChangeById(int id)
        {
            return _Repository.ChangeRoom.Find(id);
        }

        public Tour GetTourById(int id)
        {
            return _Repository.Tour.Find(id);
        }

        public Transfers GetTransferById(int id)
        {
            return _Repository.Transfers.Find(id);
        }

        public DeadLine InsertDeadline(DeadLine value)
        {
            var dbitem = _Repository.DeadLine.Add(value);
            if (_Repository.EntityAdded(dbitem))
            {
                return dbitem;
            }
            return null;
        }

        public Filelog InsertFileNote(Filelog value)
        {
            var dbitem = _Repository.Filelogs.Add(value);
            if (_Repository.EntityAdded(dbitem))
            {
                return dbitem;
            }
            return null;
        }

        public Flight InsertFlight(Flight value)
        {
            var dbitem = _Repository.Flights.Add(value);
            if (_Repository.EntityAdded(dbitem))
            {
                return dbitem;
            }
            return null;
        }

        public Rhotel InsertHotel(Rhotel value)
        {
            var dbitem = _Repository.Rhotel.Add(value);
            if (_Repository.EntityAdded(dbitem))
            {
                return dbitem;
            }
            return null;
        }

        public hotelchange InsertHotelChange(hotelchange value)
        {
            var dbitem = _Repository.hotelchange.Add(value);
            if (_Repository.EntityAdded(dbitem))
            {
                return dbitem;
            }
            return null;
        }

        public RHRoom InsertHotelRoom(RHRoom value)
        {
            var dbitem = _Repository.RHRoom.Add(value);
            if (_Repository.EntityAdded(dbitem))
            {
                return dbitem;
            }
            return null;
        }

        public ininvoice InsertInvoice(ininvoice value)
        {
            var dbitem = _Repository.ininvoice.Add(value);
            if (_Repository.EntityAdded(dbitem))
            {
                return dbitem;
            }
            return null;
        }

        public ExtraService InsertPackage(ExtraService value)
        {
            var dbitem = _Repository.ExtraService.Add(value);
            if (_Repository.EntityAdded(dbitem))
            {
                return dbitem;
            }
            return null;
        }

        public Payments InsertPayment(Payments value)
        {
            var dbitem = _Repository.Payments.Add(value);
            if (_Repository.EntityAdded(dbitem))
            {
                return dbitem;
            }
            return null;
        }

        public Reservations InsertReservation(Reservations value)
        {
            var dbitem = _Repository.Reservations.Add(value);
            if (_Repository.EntityAdded(dbitem))
            {
                return dbitem;
            }
            return null;
        }

        public ChangeRoom InsertRoomChange(ChangeRoom value)
        {
            var dbitem = _Repository.ChangeRoom.Add(value);
            if (_Repository.EntityAdded(dbitem))
            {
                return dbitem;
            }
            return null;
        }

        public Tour InsertTour(Tour value)
        {
            var dbitem = _Repository.Tour.Add(value);
            if (_Repository.EntityAdded(dbitem))
            {
                return dbitem;
            }
            return null;
        }

        public Transfers InsertTransfer(Transfers value)
        {
            var dbitem = _Repository.Transfers.Add(value);
            if (_Repository.EntityAdded(dbitem))
            {
                return dbitem;
            }
            return null;
        }

        public List<DeadLine> SearchDeadlines(int FileId)
        {
            return _Repository.DeadLine.AsNoTracking().Where(d => d.Ref_file == FileId).ToList();
        }

        public List<Filelog> SearchFileNote(int FileId)
        {
            return _Repository.Filelogs.AsNoTracking().Where(d => d.ref_file == FileId).ToList();

        }

        public List<Flight> SearchFlights(GenerecSearchModel model)
        {
            var reservations = _Repository.Flights.AsNoTracking().AsQueryable();

            reservations = reservations.Where(d => d.Reservations.status == ReservationStatus.Confirmed.ToString() || d.Reservations.status == ReservationStatus.ReConfirmed.ToString());

            // filter Country
            if (model.Country != null)
            {
                reservations = reservations.Where(d => d.Reservations.Agency.Country == model.Country);
            }

            //filter Users
            if (model.CreatedBy.Count > 0)
            {
                reservations = reservations.Where(d => model.CreatedBy.Contains(d.Reservations.ref_member));
            }

            // filter agency
            if (model.AgencyId.Count > 0)
            {
                reservations = reservations.Where(d => model.AgencyId.Contains(d.Reservations.ref_agency.Value));
            }
            // filter checkin
            if (model.Datefrom != new DateTime() && model.Dateto != new DateTime())
            {
                reservations = reservations.Where(d => d.date >= model.Datefrom && d.date <= model.Dateto);
            }
            // filter checkout
            if (model.CheckOutFrom != new DateTime() && model.CheckOutTo != new DateTime())
            {
                reservations = reservations.Where(d => d.dateout >= model.Datefrom && d.dateout <= model.Dateto);
            }

            return reservations.ToList();

        }

        public List<hotelchange> SearchHotelChange(int FileId)
        {
            return _Repository.hotelchange.AsNoTracking().Where(d => d.RID == FileId).ToList();
        }

        public List<RHRoom> SearchHotelRoom(int HotelId)
        {
            return _Repository.RHRoom.AsNoTracking().Where(d => d.HID == HotelId).ToList();
        }

        public List<Rhotel> SearchHotels(HotelSearchModel model)
        {
            var reservations = _Repository.Rhotel.AsNoTracking().AsQueryable();

            reservations = reservations.Where(d => d.Reservations.status == ReservationStatus.Confirmed.ToString() || d.Reservations.status == ReservationStatus.ReConfirmed.ToString());

            if (model.UnConfirmed)
            {
                reservations = reservations.Where(d => !d.Confirmed);
            }

            if (model.IsCash)
            {
                reservations = reservations.Where(d => d.IsCash);

                if (model.Cashfrom != new DateTime() && model.Cashto != new DateTime())
                {
                    reservations = reservations.Where(d => d.CashDate.HasValue && d.CashDate.Value >= model.Cashfrom && d.CashDate.Value <= model.Cashto);
                }
            }
            //filter hotel
            if (model.hotel > 0)
            {
                reservations = reservations.Where(d => d.hid == model.hotel);
            }
            // filter Country
            if (model.Country != null)
            {
                reservations = reservations.Where(d => d.Reservations.Agency.Country == model.Country);
            }

            //filter Users
            if (model.CreatedBy.Count > 0)
            {
                reservations = reservations.Where(d => model.CreatedBy.Contains(d.Reservations.ref_member));
            }

            // filter agency
            if (model.AgencyId.Count > 0)
            {
                reservations = reservations.Where(d => model.AgencyId.Contains(d.Reservations.ref_agency.Value));
            }
            // filter checkin
            if (model.CheckinDatefrom != new DateTime() && model.CheckinDateto != new DateTime())
            {
                reservations = reservations.Where(d => d.checkin >= model.CheckinDatefrom && d.checkin <= model.CheckinDateto);
            }
            // filter checkout
            if (model.CheckoutDatefrom != new DateTime() && model.CheckoutDateto != new DateTime())
            {
                reservations = reservations.Where(d => d.checkout >= model.CheckoutDatefrom && d.checkout <= model.CheckoutDateto);
            }
            

            return reservations.OrderBy(d=> d.checkin).ToList();
        }

        public List<ininvoice> SearchInvoice(int FileId)
        {
            return _Repository.ininvoice.AsNoTracking().Where(d => d.RID == FileId).ToList();
        }

        public List<ExtraService> SearchPackages(GenerecSearchModel model)
        {
            var reservations = _Repository.ExtraService.AsNoTracking().AsQueryable();

            reservations = reservations.Where(d => d.Reservations.status == ReservationStatus.Confirmed.ToString() || d.Reservations.status == ReservationStatus.ReConfirmed.ToString());

            // filter Country
            if (model.Country != null)
            {
                reservations = reservations.Where(d => d.Reservations.Agency.Country == model.Country);
            }

            //filter Users
            if (model.CreatedBy.Count > 0)
            {
                reservations = reservations.Where(d => model.CreatedBy.Contains(d.Reservations.ref_member));
            }

            // filter agency
            if (model.AgencyId.Count > 0)
            {
                reservations = reservations.Where(d => model.AgencyId.Contains(d.Reservations.ref_agency.Value));
            }
            // filter checkin
            if (model.Datefrom != new DateTime() && model.Dateto != new DateTime())
            {
                reservations = reservations.Where(d => d.date >= model.Datefrom && d.date <= model.Dateto);
            }
            // filter checkout
            if (model.CheckOutFrom != new DateTime() && model.CheckOutTo != new DateTime())
            {
                reservations = reservations.Where(d => d.dateout >= model.Datefrom && d.dateout <= model.Dateto);
            }

            return reservations.ToList();
        }

        public List<Reservations> SearchReservations(int FileId)
        {
            return _Repository.Reservations.AsNoTracking().ToList();
        }
        public List<Reservations> SearchReservations(ReservationSearchModel model, bool onlyConfirmed = false)
        {
            var reservations = _Repository.Reservations.AsNoTracking().AsQueryable();

            if (onlyConfirmed)
            {
                reservations = reservations.Where(d => d.status == ReservationStatus.Confirmed.ToString() || d.status == ReservationStatus.ReConfirmed.ToString());
            }

            if (model.Status.Count > 0)
            {
                reservations = reservations.Where(d => model.Status.Contains(d.status));
            }

            if (model.OnlyUnpaidFiles) // for file not yet closed
            {
                reservations = reservations.Where(d => d.balance  != 0);
            }
            else if (model.OnlyPaidFiles) // for file not yet closed
            {
                reservations = reservations.Where(d => d.balance == 0);
            }

            if (model.CreatedBy.Count > 0)
            {
                reservations = reservations.Where(d => model.CreatedBy.Contains(d.ref_member));
            }

            //filter date
            if (model.CreateDate != new DateTime() && model.CreateDateTo != new DateTime()) { 
                reservations = reservations.Where(d=> d.date >= model.CreateDate && d.date <= model.CreateDateTo);
            }
          
            // filter agency
            if (model.AgencyId.Count > 0)
            {
                reservations = reservations.Where(d => model.AgencyId.Contains(d.ref_agency.Value));
            }
            // filter checkin
            if (model.CheckinDatefrom != new DateTime() && model.CheckinDateto != new DateTime())
            {
                reservations = reservations.Where(d => d.checkindate >= model.CheckinDatefrom && d.checkindate <= model.CheckinDateto);
            }
            // filter checkout
            if (model.CheckoutDatefrom != new DateTime() && model.CheckoutDateto != new DateTime())
            {
                reservations = reservations.Where(d => d.balanceDate >= model.CheckoutDatefrom && d.balanceDate <= model.CheckoutDateto);
            }
            // filter Country
            if (model.Country != null )
            {
                reservations = reservations.Where(d => d.Agency.Country == model.Country);
            }

            // filter Customers
            if (model.Customer != null)
            {
                reservations = reservations.Where(d => d.Rhotel.Any(x=> x.customer.ToLower().Contains(model.Customer)));
            }

            return reservations.OrderBy(d => d.checkindate).ToList();

        }

        public List<Reservations> SearchRooms(ReservationSearchModel model, bool onlyConfirmed = false)
        {

            var reservations = new List<Reservations>();

            var rooms = _Repository.RHRoom.AsNoTracking().AsQueryable();

            // filter checkin
            if (model.CheckinDatefrom != new DateTime() && model.CheckinDateto != new DateTime())
            {
                rooms = rooms.Where(d => d.checkin >= model.CheckinDatefrom && d.checkout <= model.CheckinDateto);
            }
            // filter checkout
            if (model.CheckoutDatefrom != new DateTime() && model.CheckoutDateto != new DateTime())
            {
                rooms = rooms.Where(d => d.checkout >= model.CheckoutDatefrom && d.checkout <= model.CheckoutDateto);
            }

            var OrderedReservations = rooms.OrderBy(d => d.checkin);

            reservations.AddRange(OrderedReservations.Select(d => d.Rhotel.Reservations));

            //tours
            var tours = _Repository.Tour.AsNoTracking().AsQueryable();

            // filter checkin
            if (model.CheckinDatefrom != new DateTime() && model.CheckinDateto != new DateTime())
            {
                tours = tours.Where(d => d.date >= model.CheckinDatefrom && d.date <= model.CheckinDateto);
            }
            // filter checkout
            if (model.CheckoutDatefrom != new DateTime() && model.CheckoutDateto != new DateTime())
            {
                tours = tours.Where(d => d.date >= model.CheckoutDatefrom && d.date <= model.CheckoutDateto);
            }

            reservations.AddRange(tours.Select(d => d.Reservations));

            //transfer
            var transfers = _Repository.Transfers.AsNoTracking().AsQueryable();

            // filter checkin
            if (model.CheckinDatefrom != new DateTime() && model.CheckinDateto != new DateTime())
            {
                transfers = transfers.Where(d => d.date >= model.CheckinDatefrom && d.date <= model.CheckinDateto);
            }
            // filter checkout
            if (model.CheckoutDatefrom != new DateTime() && model.CheckoutDateto != new DateTime())
            {
                transfers = transfers.Where(d => d.date >= model.CheckoutDatefrom && d.date <= model.CheckoutDateto);
            }
            reservations.AddRange(transfers.Select(d => d.Reservations));

            //extra
            var extras = _Repository.ExtraService.AsNoTracking().AsQueryable();

            // filter checkin
            if (model.CheckinDatefrom != new DateTime() && model.CheckinDateto != new DateTime())
            {
                extras = extras.Where(d => d.date >= model.CheckinDatefrom && d.date <= model.CheckinDateto);
            }
            // filter checkout
            if (model.CheckoutDatefrom != new DateTime() && model.CheckoutDateto != new DateTime())
            {
                extras = extras.Where(d => d.dateout >= model.CheckoutDatefrom && d.dateout <= model.CheckoutDateto);
            }
            reservations.AddRange(extras.Select(d => d.Reservations));

            var query = reservations.Distinct(new ItemEqualityComparer()).AsQueryable();

            if (onlyConfirmed)
            {
                query = query.Where(d => d.status == ReservationStatus.Confirmed.ToString() || d.status == ReservationStatus.ReConfirmed.ToString());
            }

            if (model.Status.Count > 0)
            {
                query = query.Where(d => model.Status.Contains(d.status));
            }

            if (model.OnlyUnpaidFiles) // for file not yet closed
            {
                query = query.Where(d => d.balance != 0);
            }
            else
            if (model.OnlyPaidFiles) // for file not yet closed
            {
                query = query.Where(d => d.balance == 0);
            }

            if (model.CreatedBy.Count > 0)
            {
                query = query.Where(d => model.CreatedBy.Contains(d.ref_member));
            }

            //filter date
            if (model.CreateDate != new DateTime() && model.CreateDateTo != new DateTime())
            {
                query = query.Where(d => d.date >= model.CreateDate && d.date <= model.CreateDateTo);
            }

            // filter agency
            if (model.AgencyId.Count > 0)
            {
                query = query.Where(d => model.AgencyId.Contains(d.ref_agency.Value));
            }
            
            // filter Country
            if (model.Country != null)
            {
                query = query.Where(d => d.Agency.Country == model.Country);
            }

            // filter Customers
            //if (model.Customer != null)
            //{
            //    query = query.Where(x => x.Rhotel.Any(.ToLower().Contains(model.Customer));
            //}

            return query.OrderBy(d => d.checkindate).ToList();

        }

        class ItemEqualityComparer : IEqualityComparer<Reservations>
        {
            public bool Equals(Reservations x, Reservations y)
            {
                // Two items are equal if their keys are equal.
                return x.ID == y.ID;
            }

            public int GetHashCode(Reservations obj)
            {
                return obj.ID.GetHashCode();
            }
        }

        public List<ChangeRoom> SearchRoomChanges(int HotelId)
        {
            return _Repository.ChangeRoom.AsNoTracking().Where(d => d.HID == HotelId).ToList();
        }

        public List<Tour> SearchTours(GenerecSearchModel model)
        {
            var reservations = _Repository.Tour.AsNoTracking().AsQueryable();

            reservations = reservations.Where(d => d.Reservations.status == ReservationStatus.Confirmed.ToString() || d.Reservations.status == ReservationStatus.ReConfirmed.ToString());

            // filter Country
            if (model.Country != null)
            {
                reservations = reservations.Where(d => d.Reservations.Agency.Country == model.Country);
            }

            //filter Users
            if (model.CreatedBy.Count > 0)
            {
                reservations = reservations.Where(d => model.CreatedBy.Contains(d.Reservations.ref_member));
            }

            // filter agency
            if (model.AgencyId.Count > 0)
            {
                reservations = reservations.Where(d => model.AgencyId.Contains(d.Reservations.ref_agency.Value));
            }
            // filter checkin
            if (model.Datefrom != new DateTime() && model.Dateto != new DateTime())
            {
                reservations = reservations.Where(d => d.date >= model.Datefrom && d.date <= model.Dateto);
            }

            return reservations.OrderBy(d=> d.date).ToList();
        }

        public List<Transfers> SearchTransfer(GenerecSearchModel model)
        {
            var reservations = _Repository.Transfers.AsNoTracking().AsQueryable();

            reservations = reservations.Where(d => d.Reservations.status == ReservationStatus.Confirmed.ToString() || d.Reservations.status == ReservationStatus.ReConfirmed.ToString());

            // filter Country
            if (model.Country != null)
            {
                reservations = reservations.Where(d => d.Reservations.Agency.Country == model.Country);
            }

            //filter Users
            if (model.CreatedBy.Count > 0)
            {
                reservations = reservations.Where(d => model.CreatedBy.Contains(d.Reservations.ref_member));
            }

            // filter agency
            if (model.AgencyId.Count > 0)
            {
                reservations = reservations.Where(d => model.AgencyId.Contains(d.Reservations.ref_agency.Value));
            }
            // filter checkin
            if (model.Datefrom != new DateTime() && model.Dateto != new DateTime())
            {
                reservations = reservations.Where(d => d.date >= model.Datefrom && d.date <= model.Dateto);
            }

            return reservations.OrderByDescending(d => d.date).ToList();
        }

        public DeadLine UpdateDeadline(DeadLine value)
        {
            var dbItem = _Repository.DeadLine.Find(value.id);
            if (value != null)
            {
                _Repository.Entry(dbItem).CurrentValues.SetValues(value);
                if (_Repository.EntityEdited(dbItem))
                {
                    return dbItem;
                }
            }
            return null;
        }

        public Filelog UpdateFileNote(Filelog value)
        {
            var dbItem = _Repository.Filelogs.Find(value.Id);
            if (value != null)
            {
                _Repository.Entry(dbItem).CurrentValues.SetValues(value);
                if (_Repository.EntityEdited(dbItem))
                {
                    return dbItem;
                }
            }
            return null;
        }

        public Flight UpdateFlight(Flight value)
        {
            var dbItem = _Repository.Flights.Find(value.Id);
            if (value != null)
            {
                _Repository.Entry(dbItem).CurrentValues.SetValues(value);
                if (_Repository.EntityEdited(dbItem))
                {
                    return dbItem;
                }
            }
            return null;
        }

        public Rhotel UpdateHotel(Rhotel value)
        {
            var dbItem = _Repository.Rhotel.Find(value.ID);
            if (value != null)
            {
                _Repository.Entry(dbItem).CurrentValues.SetValues(value);
                if (_Repository.EntityEdited(dbItem))
                {
                    return dbItem;
                }
            }
            return null;
        }

        public hotelchange UpdateHotelChange(hotelchange value)
        {
            var dbItem = _Repository.hotelchange.Find(value.id);
            if (value != null)
            {
                _Repository.Entry(dbItem).CurrentValues.SetValues(value);
                if (_Repository.EntityEdited(dbItem))
                {
                    return dbItem;
                }
            }
            return null;
        }

        public RHRoom UpdateHotelRoom(RHRoom value)
        {
            var dbItem = _Repository.RHRoom.Find(value.ID);
            if (value != null)
            {
                _Repository.Entry(dbItem).CurrentValues.SetValues(value);
                if (_Repository.EntityEdited(dbItem))
                {
                    return dbItem;
                }
            }
            return null;
        }

        public ininvoice UpdateInvoice(ininvoice value)
        {
            var dbItem = _Repository.ininvoice.Find(value.id);
            if (value != null)
            {
                _Repository.Entry(dbItem).CurrentValues.SetValues(value);
                if (_Repository.EntityEdited(dbItem))
                {
                    return dbItem;
                }
            }
            return null;
        }

        public ExtraService UpdatePackage(ExtraService value)
        {
            var dbItem = _Repository.ExtraService.Find(value.ID);
            if (value != null)
            {
                _Repository.Entry(dbItem).CurrentValues.SetValues(value);
                if (_Repository.EntityEdited(dbItem))
                {
                    return dbItem;
                }
            }
            return null;
        }


        public Reservations UpdateReservation(Reservations value)
        {
            var dbItem = _Repository.Reservations.Find(value.ID);
            if (value != null)
            {
                _Repository.Entry(dbItem).CurrentValues.SetValues(value);
                if (_Repository.EntityEdited(dbItem))
                {
                    return dbItem;
                }
            }
            return null;
        }

        public ChangeRoom UpdateRoomChange(ChangeRoom value)
        {
            var dbItem = _Repository.ChangeRoom.Find(value.ID);
            if (value != null)
            {
                _Repository.Entry(dbItem).CurrentValues.SetValues(value);
                if (_Repository.EntityEdited(dbItem))
                {
                    return dbItem;
                }
            }
            return null;
        }

        public Tour UpdateTour(Tour value)
        {
            var dbItem = _Repository.Tour.Find(value.ID);
            if (value != null)
            {
                _Repository.Entry(dbItem).CurrentValues.SetValues(value);
                if (_Repository.EntityEdited(dbItem))
                {
                    return dbItem;
                }
            }
            return null;
        }

        public Transfers UpdateTransfer(Transfers value)
        {
            var dbItem = _Repository.Transfers.Find(value.ID);
            if (value != null)
            {
                _Repository.Entry(dbItem).CurrentValues.SetValues(value);
                if (_Repository.EntityEdited(dbItem))
                {
                    return dbItem;
                }
            }
            return null;
        }

        public hotelchange GetHotelChangeByHotelId(int id)
        {
            return _Repository.hotelchange.FirstOrDefault(d => d.hid.Value == id);
        }

        
    }
}
