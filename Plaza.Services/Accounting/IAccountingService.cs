using Line.Data;
using Line.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Line.Services
{
    public partial interface IAccountingService
    {

        List<Reservations> SearchReservationsByAgencyId(int Id);
        List<Reservations> SearchFutureReservationsByAgencyId(int Id);
        List<Reservations> SearchOldReservationsByAgencyId(int Id);
        List<Payments> SearchPaymentsByAgencyId(int Id);

        List<Reservations> SearchReservationsByCustomerId(int Id);
        List<Payments> SearchPaymentsByCustomerId(int Id);

        List<Reservations> SearchReservationsBySupplierId(int Id);
        List<Payments> SearchPaymentsBySupplierId(int Id);


        List<Payments> SearchPayment(GenerecSearchModel model);
        Payments GetPaymentById(int id);
        Payments InsertPayment(Payments value);
        Payments UpdatePayment(Payments value);
        void DeletePayment(int id);

        List<AdvancePayments> SearchAdvancePayments(GenerecSearchModel model);
        AdvancePayments GetAdvancePaymentById(int id);
        AdvancePayments InsertAdvancePayment(AdvancePayments value);
        AdvancePayments UpdateAdvancePayment(AdvancePayments value);
        void DeleteAdvancePayment(int id);

        List<SupplierPayment> SearchSupplierPayments(int FileId);
        SupplierPayment GetSupplierPaymentById(int id);
        SupplierPayment InsertSupplierPayment(SupplierPayment value);
        SupplierPayment UpdateSupplierPayment(SupplierPayment value);
        void DeleteSupplierPayment(int id);


    }
}
