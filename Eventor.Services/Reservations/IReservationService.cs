using Line.Data;
using Line.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Line.Services
{
    public partial interface IReservationService
    {

        List<Reservations> SearchReservations(int FileId);
        List<Reservations> SearchReservations(ReservationSearchModel model, bool onlyConfirmed);
        void CalculateReservation(int Id, bool ChangeStats = true);
        void GetReservationDate(Reservations r, out DateTime icheckout, out DateTime icheckin);
        Reservations GetReservationById(int id);
        Reservations InsertReservation(Reservations value);
        Reservations UpdateReservation(Reservations value);
        List<Reservations> GetRecentFiles(int id, int v);
        bool DeleteReservation(int id);

        List<Rhotel> SearchHotels(HotelSearchModel Model);
        Rhotel GetHotelById(int id);
        Rhotel InsertHotel(Rhotel value);
        Rhotel UpdateHotel(Rhotel value);
        bool DeleteHotel(int id);

        List<Tour> SearchTours(GenerecSearchModel Model);
        Tour GetTourById(int id);
        Tour InsertTour(Tour value);
        Tour UpdateTour(Tour value);
        bool DeleteTour(int id);

        List<Transfers> SearchTransfer(GenerecSearchModel Model);
        Transfers GetTransferById(int id);
        Transfers InsertTransfer(Transfers value);
        Transfers UpdateTransfer(Transfers value);
        bool DeleteTransfer(int id);

        List<ExtraService> SearchPackages(GenerecSearchModel Model);
        ExtraService GetPackageById(int id);
        ExtraService InsertPackage(ExtraService value);
        ExtraService UpdatePackage(ExtraService value);
        bool DeletePackage(int id);

        List<Flight> SearchFlights(GenerecSearchModel Model);
        Flight GetFlightById(int id);
        Flight InsertFlight(Flight value);
        Flight UpdateFlight(Flight value);
        bool DeleteFlight(int id);


        List<Filelog> SearchFileNote(int FileId);
        Filelog GetFileNoteById(int id);
        Filelog InsertFileNote(Filelog value);
        Filelog UpdateFileNote(Filelog value);
        bool DeleteFileNote(int id);

        List<ininvoice> SearchInvoice(int FileId);
        ininvoice GetInvoiceById(int id);
        ininvoice InsertInvoice(ininvoice value);
        ininvoice UpdateInvoice(ininvoice value);
        bool DeleteInvoice(int id);

        List<hotelchange> SearchHotelChange(int FileId);
        hotelchange GetHotelChangeById(int id);
        hotelchange InsertHotelChange(hotelchange value);
        hotelchange UpdateHotelChange(hotelchange value);
        bool DeleteHotelChange(int id);

        List<ChangeRoom> SearchRoomChanges(int FileId);
        ChangeRoom GetRoomChangeById(int id);
        ChangeRoom InsertRoomChange(ChangeRoom value);
        ChangeRoom UpdateRoomChange(ChangeRoom value);
        bool DeleteRoomChange(int id);
        void ClearHotelchangedRecoreds(int id);
        hotelchange GetHotelChangeByHotelId(int id);

        List<RHRoom> SearchHotelRoom(int FileId);
        RHRoom GetHotelRoomById(int id);
        RHRoom InsertHotelRoom(RHRoom value);
        RHRoom UpdateHotelRoom(RHRoom value);
        bool DeleteHotelRoom(int id);

        List<DeadLine> SearchDeadlines(int FileId);
        DeadLine GetDeadlineById(int id);
        DeadLine InsertDeadline(DeadLine value);
        DeadLine UpdateDeadline(DeadLine value);
        bool DeleteDeadline(int id);
    }
}
