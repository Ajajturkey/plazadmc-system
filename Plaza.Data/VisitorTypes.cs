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
    
    public partial class VisitorTypes
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int VisitorId { get; set; }
    
        public virtual Members Visitor { get; set; }
    }
}
