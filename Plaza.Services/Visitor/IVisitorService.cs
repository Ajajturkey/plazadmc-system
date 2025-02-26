using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Line.Data;

namespace Line.Services
{
    public interface IVisitorService
    {
        #region visitors

        /// <summary>
        /// Gets all Visitors
        /// </summary>
        /// <param name="createdFromUtc">Created date from (UTC); null to load all records</param>
        /// <param name="createdToUtc">Created date to (UTC); null to load all records</param>
        /// <param name="affiliateId">Affiliate identifier</param>
        /// <param name="vendorId">Vendor identifier</param>
        /// <param name="VisitorTypesIds">A list of Visitor role identifiers to filter by (at least one match); pass null or empty list in order to load all Visitors; </param>
        /// <param name="email">Email; null to load all Visitors</param>
        /// <param name="username">Username; null to load all Visitors</param>
        /// <param name="firstName">First name; null to load all Visitors</param>
        /// <param name="lastName">Last name; null to load all Visitors</param>
        /// <param name="dayOfBirth">Day of birth; 0 to load all Visitors</param>
        /// <param name="monthOfBirth">Month of birth; 0 to load all Visitors</param>
        /// <param name="company">Company; null to load all Visitors</param>
        /// <param name="phone">Phone; null to load all Visitors</param>
        /// <param name="zipPostalCode">Phone; null to load all Visitors</param>
        /// <param name="ipAddress">IP address; null to load all Visitors</param>
        /// <param name="loadOnlyWithShoppingCart">Value indicating whether to load Visitors only with shopping cart</param>
        /// <param name="sct">Value indicating what shopping cart type to filter; userd when 'loadOnlyWithShoppingCart' param is 'true'</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>Visitors</returns>
        IList<Members> GetAllVisitors(bool IsMembers);

        List<SelectListItem> GetMembers();


        /// <summary>
        /// Gets online Visitors
        /// </summary>
        /// <param name="lastActivityFromUtc">Visitor last activity date (from)</param>
        /// <param name="VisitorTypesIds">A list of Visitor role identifiers to filter by (at least one match); pass null or empty list in order to load all Visitors; </param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>Visitors</returns>
        IList<Members> GetOnlineVisitors(DateTime lastActivityFromUtc,
            int[] VisitorTypesIds);

        /// <summary>
        /// Delete a Visitor
        /// </summary>
        /// <param name="Visitor">Visitor</param>
        void DeleteVisitor(Members Visitor);

        /// <summary>
        /// Gets a Visitor
        /// </summary>
        /// <param name="VisitorId">Visitor identifier</param>
        /// <returns>A Visitor</returns>
        Members GetVisitorById(int VisitorId);

        /// <summary>
        /// Get Visitors by identifiers
        /// </summary>
        /// <param name="VisitorIds">Visitor identifiers</param>
        /// <returns>Visitors</returns>
        IList<Members> GetVisitorsByIds(int[] VisitorIds);

        /// <summary>
        /// Gets a Visitor by GUID
        /// </summary>
        /// <param name="VisitorGuid">Visitor GUID</param>
        /// <returns>A Visitor</returns>
        Members GetVisitorByGuid(Guid VisitorGuid);

        /// <summary>
        /// Get Visitor by email
        /// </summary>
        /// <param name="email">Email</param>
        /// <returns>Visitor</returns>
        Members GetVisitorByEmail(string email);

        /// <summary>
        /// Get Visitor by system role
        /// </summary>
        /// <param name="systemName">System name</param>
        /// <returns>Visitor</returns>
        Members GetVisitorBySystemName(string systemName);

        /// <summary>
        /// Get Visitor by username
        /// </summary>
        /// <param name="username">Username</param>
        /// <returns>Visitor</returns>
        Members GetVisitorByUsername(string username);

        /// <summary>
        /// Insert a guest Visitor
        /// </summary>
        /// <returns>Visitor</returns>
        Members InsertGuestVisitor();

        /// <summary>
        /// Insert a Visitor
        /// </summary>
        /// <param name="Visitor">Visitor</param>
        Members InsertVisitor(Members Visitor);

        /// <summary>
        /// Updates the Visitor
        /// </summary>
        /// <param name="Visitor">Visitor</param>
        Members UpdateVisitor(Members Visitor);



        /// <summary>
        /// Delete guest Visitor records
        /// </summary>
        /// <param name="createdFromUtc">Created date from (UTC); null to load all records</param>
        /// <param name="createdToUtc">Created date to (UTC); null to load all records</param>
        /// <param name="onlyWithoutShoppingCart">A value indicating whether to delete Visitors only without shopping cart</param>
        /// <returns>Number of deleted Visitors</returns>
        int DeleteGuestVisitors(DateTime? createdFromUtc, DateTime? createdToUtc);

        #endregion

        #region Visitor roles

        /// <summary>
        /// Delete a Visitor role
        /// </summary>
        /// <param name="VisitorTypes">Visitor role</param>
        void DeleteVisitorTypes(VisitorTypes VisitorTypes);

        /// <summary>
        /// Gets a Visitor role
        /// </summary>
        /// <param name="VisitorTypesId">Visitor role identifier</param>
        /// <returns>Visitor role</returns>
        VisitorTypes GetVisitorTypesById(int VisitorTypesId);

        /// <summary>
        /// Gets a Visitor role
        /// </summary>
        /// <param name="systemName">Visitor role system name</param>
        /// <returns>Visitor role</returns>
        VisitorTypes GetVisitorTypesBySystemName(string systemName);

        /// <summary>
        /// Gets all Visitor roles
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Visitor roles</returns>
        IList<VisitorTypes> GetAllVisitorTypess(bool showHidden = false);

        /// <summary>
        /// Inserts a Visitor role
        /// </summary>
        /// <param name="VisitorTypes">Visitor role</param>
        void InsertVisitorTypes(VisitorTypes VisitorTypes);

        /// <summary>
        /// Updates the Visitor role
        /// </summary>
        /// <param name="VisitorTypes">Visitor role</param>
        void UpdateVisitorTypes(VisitorTypes VisitorTypes);

        #endregion


        List<SystemLog> GetSystemLogs();
        SystemLog Log(SystemLog log);
        void loginLog(LoginLog log);
        SystemLog Log(string location,string error);
        void ClearLog();






    }
}
