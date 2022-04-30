using Line.Data;
using Line.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;
using PagedList.Mvc;
using System.Web.Mvc;

namespace Line.Services
{
    public partial interface IConfiqurationService
    {

        Settings GetSettings();
        Settings SetSettings(Settings model);

        SelectList GetCountries(string IsoCode);
        List<Agency> SearchAgencies(int ID);
        List<Agency> SearchAgencies(string name);
        List<Agency> Agencies();
        Agency GetAgencyById(int id);
        Agency InsertAgency(Agency value);
        Agency UpdateAgency(Agency value);
        bool DeleteAgency(int id);

        List<DataHotel> SearchHotels(int ID);
        List<DataHotel> SearchHotels(string name);
        DataHotel GetHotelById(int id);
        DataHotel InsertHotel(DataHotel value);
        DataHotel UpdateHotel(DataHotel value);
        bool DeleteHotel(int id);


        List<OfflineRooms> SearchOfflineRooms(int ID);
        OfflineRooms GetOfflineRoomById(int id);
        OfflineRooms InsertOfflineRoom(OfflineRooms value);
        OfflineRooms UpdateOfflineRoom(OfflineRooms value);
        bool DeleteOfflineRoom(int id);

        List<OnlineRooms> SearchOnlineRooms(int ID);
        OnlineRooms GetOnlineRoomById(int id);
        OnlineRooms InsertOnlineRoom(OnlineRooms value);
        OnlineRooms UpdateOnlineRoom(OnlineRooms value);
        bool DeleteOnlineRoom(int id);

        List<DataTour> SearchTours(int ID);
        DataTour GetTourById(int id);
        DataTour InsertTour(DataTour value);
        DataTour UpdateTour(DataTour value);
        bool DeleteTour(int id);

        List<DataPackage> SearchPackages(int ID);
        DataPackage GetPackageById(int id);
        DataPackage InsertPackage(DataPackage value);
        DataPackage UpdatePackage(DataPackage value);
        bool DeletePackage(int id);


        List<Supplier> SearchSupplier(int ID);
        Supplier GetSupplierById(int id);
        Supplier InsertSupplier(Supplier value);
        Supplier UpdateSupplier(Supplier value);
        bool DeleteSupplier(int id);


        List<Marketman> SearchMarketer(int ID);
        Marketman GetMarketerById(int id);
        Marketman InsertMarketer(Marketman value);
        Marketman UpdateMarketer(Marketman value);
        bool DeleteMarketer(int id);
        List<SelectListItem> GetUsedCountries(string name);
    }
}
