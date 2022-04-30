using Line.Data;


namespace Line.Services
{
    /// <summary>
    /// Work context
    /// </summary>
    public interface IWorkContext
    {



        /// <summary>
        /// Gets or sets the current customer
        /// </summary>
        Members CurrentVisitor { get; set; }
        /// <summary>
        /// <summary>
        /// Get or set current user working language
        /// </summary>
        Language WorkingLanguage { get; set; }
        /// <summary>
        /// Get or set current user working currency
        /// </summary>
        // Currency WorkingCurrency { get; set; }
        /// <summary>
        /// <summary>
        /// Get or set value indicating whether we're in admin area
        /// </summary>
        bool IsAdmin { get; }
        bool IsMemeber { get; }

        bool IsReservation { get; }
        bool IsOperation { get; }
        bool IsAccounts { get; }
        bool IsManagment { get; }


    }
}
