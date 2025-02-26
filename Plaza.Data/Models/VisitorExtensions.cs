using Line.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml;

namespace Line.Models
{
    public static class CustomerExtensions
    {
        /// <summary>
        /// Get full name
        /// </summary>
        /// <param name="customer">Customer</param>
        /// <returns>Customer full name</returns>
        public static string GetFullName(this Members customer)
        {
            if (customer == null)
                throw new ArgumentNullException("customer");
            var firstName = customer.FirstName;
            var lastName = customer.LastName;

            string fullName = "";
            if (!String.IsNullOrWhiteSpace(firstName) && !String.IsNullOrWhiteSpace(lastName))
                fullName = string.Format("{0} {1}", firstName, lastName);
            else
            {
                if (!String.IsNullOrWhiteSpace(firstName))
                    fullName = firstName;

                if (!String.IsNullOrWhiteSpace(lastName))
                    fullName = lastName;
            }
            return fullName;
        }

        public static string GetAddress(this Members customer)
        {
            if (customer == null)
                throw new ArgumentNullException("customer");
            var country = customer.Country;
            var city = customer.City;

            string adress = "";
            if (!String.IsNullOrWhiteSpace(country) && !String.IsNullOrWhiteSpace(city))
                adress = string.Format("{0} {1}", city, country);
            else
            {
                if (!String.IsNullOrWhiteSpace(city))
                    adress = city;

                if (!String.IsNullOrWhiteSpace(country))
                    adress = country;
            }
            return adress;
        }

        public static bool IsPasswordRecoveryTokenValid(this Members customer, string token)
        {
            if (customer == null)
                throw new ArgumentNullException("customer");

            //var cPrt = customer.PasswordRecoveryToken);
            //if (String.IsNullOrEmpty(cPrt))
            //    return false;

            //if (!cPrt.Equals(token, StringComparison.InvariantCultureIgnoreCase))
            //    return false;

            return true;
        }


        public static bool IsInCustomerRole(this Members customer,
       string customerRoleSystemName, bool onlyActiveCustomerRoles = true)
        {
            if (customer == null)
                throw new ArgumentNullException("customer");

            if (String.IsNullOrEmpty(customerRoleSystemName))
                throw new ArgumentNullException("customerRoleSystemName");

            var result = customer.VisitorTypes
                .FirstOrDefault(cr => /*(!onlyActiveCustomerRoles || cr.Active) &&*/ (cr.Name == customerRoleSystemName)) != null;
            return result;
        }
        /// <summary>
        /// Gets a value indicating whether customer is administrator
        /// </summary>
        /// <param name="customer">Customer</param>
        /// <param name="onlyActiveCustomerRoles">A value indicating whether we should look only in active customer roles</param>
        /// <returns>Result</returns>
        public static bool IsAdmin(this Members customer, bool onlyActiveCustomerRoles = true)
        {
            return IsInCustomerRole(customer, SystemVisitorTypeNames.Administrators, onlyActiveCustomerRoles);
        }

       
        /// <summary>
        /// Gets a value indicating whether customer is registered
        /// </summary>
        /// <param name="customer">Customer</param>
        /// <param name="onlyActiveCustomerRoles">A value indicating whether we should look only in active customer roles</param>
        /// <returns>Result</returns>
        public static bool IsRegistered(this Members customer, bool onlyActiveCustomerRoles = true)
        {
            return IsInCustomerRole(customer, SystemVisitorTypeNames.Members, onlyActiveCustomerRoles);
        }

        /// <summary>
        /// Gets a value indicating whether customer is guest
        /// </summary>
        /// <param name="customer">Customer</param>
        /// <param name="onlyActiveCustomerRoles">A value indicating whether we should look only in active customer roles</param>
        /// <returns>Result</returns>
        public static bool IsGuest(this Members customer, bool onlyActiveCustomerRoles = true)
        {
            return IsInCustomerRole(customer, SystemVisitorTypeNames.Guests, onlyActiveCustomerRoles);
        }

        /// <summary>
        /// Check whether password recovery link is expired
        /// </summary>
        /// <param name="customer">Customer</param>
        /// <param name="customerSettings">Customer settings</param>
        /// <returns>Result</returns>
        //public static bool IsPasswordRecoveryLinkExpired(thisMemberscustomer, CustomerSettings customerSettings)
        //{
        //    if (customer == null)
        //        throw new ArgumentNullException("customer");

        //    if (customerSettings == null)
        //        throw new ArgumentNullException("customerSettings");

        //    if (customerSettings.PasswordRecoveryLinkDaysValid == 0)
        //        return false;

        //    var geneatedDate = customer.PasswordRecoveryTokenDateGenerated);
        //    if (!geneatedDate.HasValue)
        //        return false;

        //    var daysPassed = (DateTime.UtcNow - geneatedDate.Value).TotalDays;
        //    if (daysPassed > customerSettings.PasswordRecoveryLinkDaysValid)
        //        return true;

        //    return false;
        //}

        /// <summary>
        /// Get customer role identifiers
        /// </summary>
        /// <param name="customer">Customer</param>
        /// <param name="showHidden">A value indicating whether to load hidden records</param>
        /// <returns>Customer role identifiers</returns>
        public static int[] GetCustomerRoleIds(this Members customer, bool showHidden = false)
        {
            if (customer == null)
                throw new ArgumentNullException("customer");

            var customerRolesIds = customer.VisitorTypes
               .Where(cr => showHidden)
               .Select(cr => cr.Id)
               .ToArray();

            return customerRolesIds;
        }
    }
}
