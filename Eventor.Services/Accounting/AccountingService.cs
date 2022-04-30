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
    public class AccountingService : IAccountingService
    {
        private readonly DBEntities _Repository;
        private readonly IWorkContext _workcontext;

        public AccountingService(IWorkContext workcontext)
        {
            _workcontext = workcontext;
            _Repository = new DBEntities();
        }

        public void DeleteAdvancePayment(int id)
        {
            var dbitem = _Repository.AdvancePayments.Find(id);
            _Repository.EntityDeleted(dbitem);
        }

        public void DeletePayment(int id)
        {
            var dbitem = _Repository.Payments.Find(id);
            _Repository.EntityDeleted(dbitem);
        }

        public void DeleteSupplierPayment(int id)
        {
            throw new NotImplementedException();
        }

        public AdvancePayments GetAdvancePaymentById(int id)
        {
            return _Repository.AdvancePayments.Find(id);
        }

        public Payments GetPaymentById(int id)
        {
            return _Repository.Payments.Find(id);
        }

        public SupplierPayment GetSupplierPaymentById(int id)
        {
            throw new NotImplementedException();
        }

        public AdvancePayments InsertAdvancePayment(AdvancePayments value)
        {
            var dbitem = _Repository.AdvancePayments.Add(value);
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
       

        public SupplierPayment InsertSupplierPayment(SupplierPayment value)
        {
            throw new NotImplementedException();
        }

        public List<AdvancePayments> SearchAdvancePayments(GenerecSearchModel model)
        {
            var reservations = _Repository.AdvancePayments.AsNoTracking().AsQueryable();

            // filter agency
            if (model.AgencyId.Count > 0)
            {
                reservations = reservations.Where(d => model.AgencyId.Contains(d.ref_Agency));
            }
            // filter checkin
            if (model.Datefrom != new DateTime() && model.Dateto != new DateTime())
            {
                reservations = reservations.Where(d => d.date >= model.Datefrom && d.date < model.Dateto);
            }

            // filter Type
            if (!string.IsNullOrEmpty(model.Type))
            {
                reservations = reservations.Where(d => d.type == model.Type);
            }

            return reservations.OrderByDescending(d=> d.date).ToList();
        }

        public List<Reservations> SearchFutureReservationsByAgencyId(int Id)
        {
            throw new NotImplementedException();
        }

        public List<Reservations> SearchOldReservationsByAgencyId(int Id)
        {
            throw new NotImplementedException();
        }

        public List<Payments> SearchPayment(GenerecSearchModel model)
        {
            var reservations = _Repository.Payments.AsNoTracking().AsQueryable();

            // filter agency
            if (model.AgencyId.Count > 0)
            {
                reservations = reservations.Where(d => model.AgencyId.Contains(d.Reservations.ref_agency.Value));
            }
            // filter checkin
            if (model.Datefrom != new DateTime() && model.Dateto != new DateTime())
            {
                reservations = reservations.Where(d => d.date >= model.Datefrom && d.date < model.Dateto);
            }

            // filter Type
            if (!string.IsNullOrEmpty(model.Type))
            {
                reservations = reservations.Where(d => d.type == model.Type);
            }

            return reservations.ToList();
        }

        public List<Payments> SearchPaymentsByAgencyId(int Id)
        {
            throw new NotImplementedException();
        }

        public List<Payments> SearchPaymentsByCustomerId(int Id)
        {
            throw new NotImplementedException();
        }

        public List<Payments> SearchPaymentsBySupplierId(int Id)
        {
            throw new NotImplementedException();
        }

        public List<Reservations> SearchReservationsByAgencyId(int Id)
        {
            throw new NotImplementedException();
        }

        public List<Reservations> SearchReservationsByCustomerId(int Id)
        {
            throw new NotImplementedException();
        }

        public List<Reservations> SearchReservationsBySupplierId(int Id)
        {
            throw new NotImplementedException();
        }

        public List<SupplierPayment> SearchSupplierPayments(int FileId)
        {
            throw new NotImplementedException();
        }

        public AdvancePayments UpdateAdvancePayment(AdvancePayments value)
        {
            throw new NotImplementedException();
        }

        public Payments UpdatePayment(Payments value)
        {
            throw new NotImplementedException();
        }

        public SupplierPayment UpdateSupplierPayment(SupplierPayment value)
        {
            throw new NotImplementedException();
        }
    }
}
