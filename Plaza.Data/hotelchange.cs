//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Line.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class hotelchange
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public hotelchange()
        {
            this.ChangeRoom = new HashSet<ChangeRoom>();
        }
    
        public int id { get; set; }
        public int RID { get; set; }
        public string name { get; set; }
        public Nullable<int> hid { get; set; }
        public System.DateTime checkout { get; set; }
        public System.DateTime checkin { get; set; }
        public decimal Cost { get; set; }
        public decimal Total { get; set; }
        public string customer { get; set; }
        public string note { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChangeRoom> ChangeRoom { get; set; }
        public virtual Reservations Reservations { get; set; }
    }
}
