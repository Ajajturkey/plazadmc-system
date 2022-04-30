using Line.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Line.Services
{
    static class DBEvents
    {

        public static bool EntityAdded(this DBEntities _Repository, object o)
        {

            _Repository.Entry(o).State = EntityState.Added;
            try
            {
                _Repository.SaveChanges();
                return true;
            }
            catch (DbEntityValidationException ex)
            {
                foreach (DbEntityValidationResult item in ex.EntityValidationErrors)
                {
                    // Get entry

                    DbEntityEntry entry = item.Entry;
                    string entityTypeName = entry.Entity.GetType().Name;

                    // Display or log error messages

                    foreach (DbValidationError subItem in item.ValidationErrors)
                    {
                        string message = string.Format("Error '{0}' occurred in {1} at {2}",
                                 subItem.ErrorMessage, entityTypeName, subItem.PropertyName);
                        _Repository.SystemLog.Add(new SystemLog { EntityID = 0, EntityName = subItem.PropertyName, Error = subItem.PropertyName });

                    }
                }
                _Repository.Entry(o).State = EntityState.Detached;
                _Repository = new DBEntities();
                return false;

            }
            catch (Exception ex)
            {
                _Repository.Entry(o).State = EntityState.Detached;
                _Repository = new DBEntities();
                var subItem = _Repository.Entry(o);
                string message = string.Format("Error '{0}' occurred in {1}",
                 ex.Message, ex.StackTrace);
                _Repository.SystemLog.Add(new SystemLog
                {
                    EntityID = 0,
                    EntityName = subItem.GetType().Name,
                    Error = message
                });
                return false;

            }



        }

        public static bool EntityEdited(this DBEntities _Repository, object o)
        {

            _Repository.Entry(o).State = EntityState.Modified;
            try
            {
                _Repository.SaveChanges();
                return true;
            }
            catch (DbEntityValidationException ex)
            {
                foreach (DbEntityValidationResult item in ex.EntityValidationErrors)
                {
                    // Get entry

                    DbEntityEntry entry = item.Entry;
                    string entityTypeName = entry.Entity.GetType().Name;

                    // Display or log error messages

                    foreach (DbValidationError subItem in item.ValidationErrors)
                    {
                        string message = string.Format("Error '{0}' occurred in {1} at {2}",
                                 subItem.ErrorMessage, entityTypeName, subItem.PropertyName);
                        Console.WriteLine(message);
                    }
                }
                _Repository.Entry(o).CurrentValues.SetValues(_Repository.Entry(o).OriginalValues);
                _Repository.Entry(o).State = EntityState.Unchanged;
                _Repository = new DBEntities();
                return false;

            }
            catch (Exception ex)
            {
                _Repository.Entry(o).CurrentValues.SetValues(_Repository.Entry(o).OriginalValues);
                _Repository.Entry(o).State = EntityState.Unchanged;
                _Repository = new DBEntities();

                var subItem = _Repository.Entry(o);
                string message = string.Format("Error '{0}' occurred in {1}",
                 ex.Message, ex.StackTrace);
                _Repository.SystemLog.Add(new SystemLog
                {
                    EntityID = 0,
                    EntityName = subItem.GetType().Name,
                    Error = message
                });

                return false;
            }


        }

        public static bool EntityDeleted(this DBEntities _Repository, object o)
        {
            _Repository.Entry(o).State = EntityState.Deleted;
            try
            {
                _Repository.SaveChanges();
                return true;
            }
            catch (DbEntityValidationException ex)
            {
                foreach (DbEntityValidationResult item in ex.EntityValidationErrors)
                {
                    // Get entry

                    DbEntityEntry entry = item.Entry;
                    string entityTypeName = entry.Entity.GetType().Name;

                    // Display or log error messages

                    foreach (DbValidationError subItem in item.ValidationErrors)
                    {
                        string message = string.Format("Error '{0}' occurred in {1} at {2}",
                                 subItem.ErrorMessage, entityTypeName, subItem.PropertyName);
                        Console.WriteLine(message);
                    }
                }
                _Repository.Entry(o).State = EntityState.Unchanged;
                _Repository = new DBEntities();
                return false;
            }
            catch (Exception ex)
            {
                _Repository.Entry(o).State = EntityState.Unchanged;
                _Repository = new DBEntities();
                var subItem = _Repository.Entry(o);
                string message = string.Format("Error '{0}' occurred in {1}",
                               ex.Message, ex.StackTrace);
                _Repository.SystemLog.Add(new SystemLog
                {
                    EntityID = 0
                   ,
                    EntityName = subItem.GetType().Name,
                    Error = message
                });
                return false;
            }
        }
    }
}
