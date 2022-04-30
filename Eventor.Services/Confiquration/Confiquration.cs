using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Line.Data;
using Line.Models;
using Line.Data.Utility;
using System.Data.Entity;
using PagedList;
using PagedList.Mvc;
using System.Web.Mvc;
using System.Globalization;

namespace Line.Services
{
    public class ConfiqurationService : IConfiqurationService
    {
        private readonly DBEntities _Repository;
        private readonly IWorkContext _workcontext;
        private readonly int PageSize = 10;

        public ConfiqurationService( IWorkContext workcontext)
        {
            _workcontext = workcontext;
            _Repository = new DBEntities();
        }
        
        public List<SelectListItem> GetUsedCountries(string IsoCode)
        {
            Dictionary<string, string> countryNames = new Dictionary<string, string>();
            var countries = _Repository.Agency.Select(d => d.Country).Distinct();

            foreach (CultureInfo ci in CultureInfo.GetCultures(CultureTypes.SpecificCultures))
            {
                RegionInfo ri = new RegionInfo(ci.Name);
                if (!countryNames.ContainsKey(ri.TwoLetterISORegionName)) countryNames.Add(ri.TwoLetterISORegionName.ToUpper(), ri.EnglishName);
            }
            countryNames.Add("PALESTINE", "PS");
            countryNames.Add("IVORY COAST", "IC");
            countryNames.Add("KYRGY", "KG");
            countryNames.Add("MOLDOVIA", "MD");
            countryNames.Add("Korea (South)", "KR");
            countryNames.Add("CZECH REPUBLIC", "CZ");

            var countryDropdown = new List<SelectListItem>();

            foreach (var item in countries)
            {
                if (countryNames.ContainsKey(item))
                {
                    countryDropdown.Add(new SelectListItem { Value = item, Text = countryNames[item] });
                }
                else
                {
                    if (string.IsNullOrEmpty(item))
                    {
                        continue;
                    }
                    countryDropdown.Add(new SelectListItem { Value = item, Text = item });
                }
            }

            var selected = countryDropdown.FirstOrDefault(d =>  d.Value == IsoCode);
            if (selected != null)
            {
 selected.Selected = true;
            }
           

            return countryDropdown;
        }
        public SelectList GetCountries(string IsoCode)
        {
            Dictionary<string, string> countryNames = new Dictionary<string, string>();
            foreach (CultureInfo ci in CultureInfo.GetCultures(CultureTypes.SpecificCultures))
            {
                RegionInfo ri = new RegionInfo(ci.Name);
                if (!countryNames.ContainsKey(ri.TwoLetterISORegionName)) countryNames.Add(ri.TwoLetterISORegionName.ToUpper(), ri.EnglishName);
            }

            countryNames.Add("PALESTINE", "PS");
            countryNames.Add("IVORY COAST", "IC");
            countryNames.Add("KYRGY", "KG");
            countryNames.Add("MOLDOVIA", "MD");
            countryNames.Add("Korea (South)", "KR");
            countryNames.Add("CZECH REPUBLIC", "CZ");

            SelectList countryDropdown = new SelectList(countryNames.OrderBy(o => o.Value), "Key", "Value", IsoCode);
            

            return countryDropdown;
        }
        public bool DeleteAgency(int id)
        {
            var item = _Repository.Agency.Find(id);
            if (item != null)
            {
                var dbitem = _Repository.Agency.Remove(item);
                return _Repository.EntityDeleted(dbitem);
            }
            return false;
        }

        public bool DeleteHotel(int id)
        {
            var item = _Repository.DataHotels.Find(id);
            if (item != null)
            {
                var dbitem = _Repository.DataHotels.Remove(item);
                return _Repository.EntityDeleted(dbitem);
            }
            return false;
        }

        public bool DeleteMarketer(int id)
        {
            var item = _Repository.Marketman.Find(id);
            if (item != null)
            {
                var dbitem = _Repository.Marketman.Remove(item);
                return _Repository.EntityDeleted(dbitem);
            }
            return false;
        }

        public bool DeleteOfflineRoom(int id)
        {
            var item = _Repository.OfflineRooms.Find(id);
            if (item != null)
            {
                var dbitem = _Repository.OfflineRooms.Remove(item);
                return _Repository.EntityDeleted(dbitem);
            }
            return false;
        }

        public bool DeleteOnlineRoom(int id)
        {
            var item = _Repository.OnlineRooms.Find(id);
            if (item != null)
            {
                var dbitem = _Repository.OnlineRooms.Remove(item);
                return _Repository.EntityDeleted(dbitem);
            }
            return false;
        }

        public bool DeletePackage(int id)
        {
            var item = _Repository.DataPackages.Find(id);
            if (item != null)
            {
                var dbitem = _Repository.DataPackages.Remove(item);
                return _Repository.EntityDeleted(dbitem);
            }
            return false;
        }

        public bool DeleteSupplier(int id)
        {
            var item = _Repository.Suppliers.Find(id);
            if (item != null)
            {
                var dbitem = _Repository.Suppliers.Remove(item);
                return _Repository.EntityDeleted(dbitem);
            }
            return false;
        }

        public bool DeleteTour(int id)
        {
            var item = _Repository.DataTours.Find(id);
            if (item != null)
            {
                var dbitem = _Repository.DataTours.Remove(item);
                return _Repository.EntityDeleted(dbitem);
            }
            return false;
        }

        public Agency GetAgencyById(int id)
        {
            return _Repository.Agency.Find(id);
        }

        public DataHotel GetHotelById(int id)
        {
            return _Repository.DataHotels.Find(id);
        }

        public Marketman GetMarketerById(int id)
        {
            return _Repository.Marketman.Find(id);
        }

        public OfflineRooms GetOfflineRoomById(int id)
        {
            return _Repository.OfflineRooms.Find(id);
        }

        public OnlineRooms GetOnlineRoomById(int id)
        {
            return _Repository.OnlineRooms.Find(id);
        }

        public DataPackage GetPackageById(int id)
        {
            return _Repository.DataPackages.Find(id);
        }

        public Supplier GetSupplierById(int id)
        {
            return _Repository.Suppliers.Find(id);
        }

        public DataTour GetTourById(int id)
        {
            return _Repository.DataTours.Find(id);
        }

        public Agency InsertAgency(Agency value)
        {
            var dbitem = _Repository.Agency.Add(value);
            if (_Repository.EntityAdded(dbitem))
            {
                return dbitem;
            }
            return null;
        }

        public DataHotel InsertHotel(DataHotel value)
        {
            var dbitem = _Repository.DataHotels.Add(value);
            if (_Repository.EntityAdded(dbitem))
            {
                return dbitem;
            }
            return null;
        }

        public Marketman InsertMarketer(Marketman value)
        {
            var dbitem = _Repository.Marketman.Add(value);
            if (_Repository.EntityAdded(dbitem))
            {
                return dbitem;
            }
            return null;
        }

        public OfflineRooms InsertOfflineRoom(OfflineRooms value)
        {
            var dbitem = _Repository.OfflineRooms.Add(value);
            if (_Repository.EntityAdded(dbitem))
            {
                return dbitem;
            }
            return null;
        }

        public OnlineRooms InsertOnlineRoom(OnlineRooms value)
        {
            var dbitem = _Repository.OnlineRooms.Add(value);
            if (_Repository.EntityAdded(dbitem))
            {
                return dbitem;
            }
            return null;
        }

        public DataPackage InsertPackage(DataPackage value)
        {
            var dbitem = _Repository.DataPackages.Add(value);
            if (_Repository.EntityAdded(dbitem))
            {
                return dbitem;
            }
            return null;
        }

        public Supplier InsertSupplier(Supplier value)
        {
            var dbitem = _Repository.Suppliers.Add(value);
            if (_Repository.EntityAdded(dbitem))
            {
                return dbitem;
            }
            return null;
        }

        public DataTour InsertTour(DataTour value)
        {
            var dbitem = _Repository.DataTours.Add(value);
            if (_Repository.EntityAdded(dbitem))
            {
                return dbitem;
            }
            return null;
        }

        public List<Agency> SearchAgencies(int ID)
        {
            return _Repository.Agency.AsNoTracking().ToList();
        }

        public List<Agency> Agencies()
        {
            return _Repository.Agency.AsNoTracking().ToList();
        }
      

        public List<DataHotel> SearchHotels(int ID)
        {
            return _Repository.DataHotels.AsNoTracking().ToList();
        }

        public List<Marketman> SearchMarketer(int ID)
        {
            return _Repository.Marketman.AsNoTracking().ToList();
        }

        public List<OfflineRooms> SearchOfflineRooms(int ID)
        {
            return _Repository.OfflineRooms.AsNoTracking().ToList();
        }

        public List<OnlineRooms> SearchOnlineRooms(int ID)
        {
            return _Repository.OnlineRooms.AsNoTracking().ToList();
        }

        public List<DataPackage> SearchPackages(int ID)
        {
            return _Repository.DataPackages.AsNoTracking().ToList();
        }

        public List<Supplier> SearchSupplier(int ID)
        {
            return _Repository.Suppliers.AsNoTracking().ToList();
        }

        public List<DataTour> SearchTours(int ID)
        {
            return _Repository.DataTours.AsNoTracking().ToList();
        }

        public Agency UpdateAgency(Agency value)
        {
            var dbItem = _Repository.Agency.Find(value.ID);
            if (dbItem != null)
            {
                _Repository.Entry(dbItem).CurrentValues.SetValues(value);
                if (_Repository.EntityEdited(dbItem))
                {
                    return dbItem;
                }
            }
            return null;
        }

        public DataHotel UpdateHotel(DataHotel value)
        {
            var dbItem = _Repository.DataHotels.Find(value.Id);
            if (dbItem != null)
            {
                _Repository.Entry(dbItem).CurrentValues.SetValues(value);
                if (_Repository.EntityEdited(dbItem))
                {
                    return dbItem;
                }
            }
            return null;
        }

        public Marketman UpdateMarketer(Marketman value)
        {
            var dbItem = _Repository.Marketman.Find(value.Id);
            if (dbItem != null)
            {
                _Repository.Entry(dbItem).CurrentValues.SetValues(value);
                if (_Repository.EntityEdited(dbItem))
                {
                    return dbItem;
                }
            }
            return null;
        }

        public OfflineRooms UpdateOfflineRoom(OfflineRooms value)
        {
            var dbItem = _Repository.OfflineRooms.Find(value.Id);
            if (dbItem != null)
            {
                _Repository.Entry(dbItem).CurrentValues.SetValues(value);
                if (_Repository.EntityEdited(dbItem))
                {
                    return dbItem;
                }
            }
            return null;
        }

        public OnlineRooms UpdateOnlineRoom(OnlineRooms value)
        {
            var dbItem = _Repository.OnlineRooms.Find(value.Id);
            if (dbItem != null)
            {
                _Repository.Entry(dbItem).CurrentValues.SetValues(value);
                if (_Repository.EntityEdited(dbItem))
                {
                    return dbItem;
                }
            }
            return null;
        }

        public DataPackage UpdatePackage(DataPackage value)
        {
            var dbItem = _Repository.DataPackages.Find(value.Id);
            if (dbItem != null)
            {
                _Repository.Entry(dbItem).CurrentValues.SetValues(value);
                if (_Repository.EntityEdited(dbItem))
                {
                    return dbItem;
                }
            }
            return null;
        }

        public Supplier UpdateSupplier(Supplier value)
        {
            var dbItem = _Repository.Suppliers.Find(value.Id);
            if (dbItem != null)
            {
                _Repository.Entry(dbItem).CurrentValues.SetValues(value);
                if (_Repository.EntityEdited(dbItem))
                {
                    return dbItem;
                }
            }
            return null;
        }

        public DataTour UpdateTour(DataTour value)
        {
            var dbItem = _Repository.DataTours.Find(value.Id);
            if (dbItem != null)
            {
                _Repository.Entry(dbItem).CurrentValues.SetValues(value);
                if (_Repository.EntityEdited(dbItem))
                {
                    return dbItem;
                }
            }
            return null;
        }

        public Settings GetSettings()
        {
            var setting =  _Repository.Settings.FirstOrDefault();
            if (setting == null)
            {
                var model = new Settings
                {
                    AccountingName = "AccountingName",
                    AccountingTitle = "AccountingTitle",
                    BankAccountNo = "BankAccountNo",
                    BackIBAN = "BankIBAN",
                    BankAccount = "BankAccount",
                    BankAddress = "BankAddress",
                    BankName = "BankName",
                    BankSwift = "BankSwift",
                    EmailDefaultUser = "EmailDefaultUser",
                    EmailDefaultPass = "EmailDefaultPass",
                    EmailPort = "25",
                    EmailSMTP = "smtp.gmail.com",
                    EmailUseCredentials = true,
                    EmailUseSSL = true,
                };
                _Repository.Settings.Add(model);
                if (_Repository.EntityAdded(model))
                {
                    return model;
                }
            }
            return setting;
        }

        public Settings SetSettings(Settings model)
        {
            var dbItem = _Repository.Settings.Find(model.Id);
            if (dbItem != null)
            {
                _Repository.Entry(dbItem).CurrentValues.SetValues(model);
                if (_Repository.EntityEdited(dbItem))
                {
                    return dbItem;
                }
            }
            return null;
        }

        public List<Agency> SearchAgencies(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return _Repository.Agency.AsNoTracking().ToList();
            }
            return _Repository.Agency.Where(d => d.AgencyName.ToLower().Contains(name.ToLower())).ToList();
        }

        public List<DataHotel> SearchHotels(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return _Repository.DataHotels.AsNoTracking().ToList();
            }
            return _Repository.DataHotels.Where(d => d.name.ToLower().Contains(name.ToLower())).ToList();
        }
    }
}
