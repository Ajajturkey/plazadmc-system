using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Line.Models
{
    public static partial class SystemVisitorTypeNames
    {
        public static string Administrators { get { return "Administrators"; } }
        public static string Members { get { return "Members"; } }
        public static string Guests { get { return "Guests"; } }
        public static string Reservations { get { return "Reservations"; } }
        public static string Operations { get { return "Operations"; } }
        public static string Accounting { get { return "Accounting"; } }
        public static string Managing { get { return "Managing"; } }
        public static string Reporting { get { return "Reporting"; } }
    }

    public enum UploadLocation
    {
         MainSlider, 
         CustomerReview  
    }

    public static partial class FileStates
    {
        public static string Created { get { return "Created"; } }
        public static string Policy { get { return "Policy"; } }
        public static string Packeged { get { return "Packeged"; } }
        public static string deliveredToCarrier { get { return "DeliveredToCarrier"; } }
        public static string delivered { get { return "Delivered"; } }

    }
}
