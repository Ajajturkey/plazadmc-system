using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Line.Data;
using Line.Models;
using System.Web.Mvc;

namespace Line.Services
{
    public class VisitorService : IVisitorService
    {
        private readonly DBEntities _Repository;

        public VisitorService()
        {
            _Repository = new DBEntities();
        }

        public int DeleteGuestVisitors(DateTime? createdFromUtc, DateTime? createdToUtc)
        {
            var Guestes = _Repository.Members.Where(d => d.Created_utc <= createdFromUtc.Value && d.Created_utc >= createdToUtc.Value).AsEnumerable();

            foreach (var item in Guestes)
            {
                var dbitem = _Repository.Members.Remove(item);
                _Repository.EntityDeleted(dbitem);
            }
            return Guestes.Count();
        }

        

       
        public void DeleteVisitor(Members Visitor)
        {
            var visitor = _Repository.Members.FirstOrDefault(d => d.Id == Visitor.Id);
            if (visitor != null)
            {
                var dbitem = _Repository.Members.Remove(visitor);
                _Repository.EntityDeleted(dbitem);

            }

        }

        public void DeleteVisitorTypes(VisitorTypes VisitorTypes)
        {
            var type = _Repository.VisitorTypes.FirstOrDefault(d => d.Id == VisitorTypes.Id );
            if (type != null)
            {

                var dbitem = _Repository.VisitorTypes.Remove(type);
                _Repository.EntityDeleted(dbitem);

            }
        }

        public List<SelectListItem> GetMembers()
        {
            var visitors =  _Repository.Members.AsNoTracking().Include(z => z.VisitorTypes).Where(x => !x.Deleted && x.active && x.VisitorTypes.Any(d => d.Name == SystemVisitorTypeNames.Reservations)).ToList();

            var members = new List<SelectListItem>();

            _Repository.Members.ToList().ForEach(d => members.Add(new SelectListItem { Text = d.GetFullName(), Value = d.Id.ToString() }));
;
            return members;
        }

        public IList<Members> GetAllVisitors(bool IsMembers)
        {
            if (IsMembers)
            {
                return _Repository.Members.AsNoTracking().Include(z => z.VisitorTypes).Where(x => !x.Deleted && x.active && x.VisitorTypes.Any(d => d.Name == SystemVisitorTypeNames.Reservations)).ToList();

            }
            return _Repository.Members.AsNoTracking().Include(z => z.VisitorTypes).Where(x => !x.Deleted /*&& x.VisitorTypes.Any(d => d.Name == SystemVisitorTypeNames.Members)*/).ToList();
        }

        public IList<VisitorTypes> GetAllVisitorTypess(bool showHidden = false)
        {
            if (showHidden)
            {
                return _Repository.VisitorTypes.ToList();
            }
            else
            {
                return _Repository.VisitorTypes.ToList();
                // return _Repository.VisitorTypes.Where(d=>d.Active).ToList();
            }
        }

       

        public IList<Members> GetOnlineVisitors(DateTime lastActivityFromUtc, int[] VisitorTypesIds)
        {

            var query = _Repository.Members.AsQueryable();
            query = query.Where(c => lastActivityFromUtc <= c.lastActivityFromUtc);
            query = query.Where(c => !c.Deleted);
            if (VisitorTypesIds != null && VisitorTypesIds.Length > 0)
                query = query.Where(c => c.VisitorTypes.Select(cr => cr.Id).Intersect(VisitorTypesIds).Any());

            query = query.OrderByDescending(c => c.lastActivityFromUtc);

            return query.ToList();
        }

        public Members GetVisitorByEmail(string email)
        {
            return _Repository.Members.FirstOrDefault(d => d.email == email);

        }

        public Members GetVisitorByGuid(Guid VisitorGuid)
        {
            return _Repository.Members.FirstOrDefault(d => d.VisitorGuid == VisitorGuid);

        }

        public Members GetVisitorById(int VisitorId)
        {
            return _Repository.Members.Find(VisitorId);

        }

        public Members GetVisitorBySystemName(string systemName)
        {
            throw new NotImplementedException();
        }

        public Members GetVisitorByUsername(string username)
        {
            return _Repository.Members.FirstOrDefault(d => d.username
            == username);

        }

        public IList<Members> GetVisitorsByIds(int[] VisitorIds)
        {
            if (VisitorIds == null || VisitorIds.Length == 0)
                return new List<Members>();

            var query = from c in _Repository.Members.AsQueryable()
                        where VisitorIds.Contains(c.Id)
                        select c;
            var customers = query.ToList();
            //sort by passed identifiers
            var sortedCustomers = new List<Members>();
            foreach (int id in VisitorIds)
            {
                var customer = customers.Find(x => x.Id == id);
                if (customer != null)
                    sortedCustomers.Add(customer);
            }
            return sortedCustomers;
        }

        public VisitorTypes GetVisitorTypesById(int VisitorTypesId)
        {
            return _Repository.VisitorTypes.Find(VisitorTypesId);
        }

        public VisitorTypes GetVisitorTypesBySystemName(string systemName)
        {
            var type = _Repository.VisitorTypes.FirstOrDefault(d => d.Name == systemName);
            if (type == null)
            {

            }

            // create system type 

            return type;
        }

        public Members InsertGuestVisitor()
        {
            var visitor = new Members
            {
                VisitorGuid = Guid.NewGuid(),
                active = true,
                Created_utc = DateTime.UtcNow,
                lastActivityFromUtc = DateTime.UtcNow,
                username = "",
                password = "",
                email = ""
            };

           
            //add to 'Guests' role
            //var guestRole = GetVisitorTypesBySystemName(SystemVisitorTypeNames.Guests);
            //if (guestRole == null)
            //     throw new Exception("'Guests' role could not be loaded");


            //var dbitem = _Visitors.Add(visitor);

            
            //visitor.VisitorTypes.Add(new VisitorTypes { Name = SystemVisitorTypeNames.Guests });
            //_Repository.EntityAdded(dbitem);

            return visitor;
        }

      

        public Members InsertVisitor(Members Visitor)
        {
            if (Visitor != null)
            {
                Members dbitem = _Repository.Members.Add(Visitor);
                _Repository.EntityAdded(dbitem);
                return dbitem;
            }
            return null;
        }

        public void InsertVisitorTypes(VisitorTypes VisitorTypes)
        {
            if (VisitorTypes != null)
            {
                var dbitem = _Repository.VisitorTypes.Add(VisitorTypes);
                _Repository.EntityAdded(dbitem);
            }
        }


        public Members UpdateVisitor(Members Visitor)
        {
            if (Visitor == null)
                throw new ArgumentNullException("localeStringResource");

            var dbitem = _Repository.Members.Find(Visitor.Id);
            _Repository.Entry(dbitem).CurrentValues.SetValues(Visitor);
            //update language
            _Repository.EntityEdited(dbitem);

            return dbitem;
        }

        public void UpdateVisitorTypes(VisitorTypes VisitorTypes)
        {
            if (VisitorTypes == null)
                throw new ArgumentNullException("localeStringResource");

            var dbitem = _Repository.VisitorTypes.Find(VisitorTypes.Id);
            dbitem = VisitorTypes;
            //update language
            _Repository.EntityEdited(dbitem);
        }

        public List<SystemLog> GetSystemLogs()
        {
            return _Repository.SystemLog.AsNoTracking().ToList();
        }

        public SystemLog Log(SystemLog log)
        {
            return _Repository.SystemLog.Add(log);
        }

        public SystemLog Log(string location, string error)
        {
            return _Repository.SystemLog.Add(new SystemLog(){EntityName = location, Error = error});
        }

        public void ClearLog()
        {
            _Repository.SystemLog.ToList().ForEach(d => _Repository.SystemLog.Remove(d));
            _Repository.SaveChanges();
        }

        public void loginLog(LoginLog log)
        {
            var o = _Repository.LoginLog.Add(log);
            _Repository.EntityAdded(o);
        }
    }
}
