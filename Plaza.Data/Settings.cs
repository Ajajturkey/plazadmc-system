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
    
    public partial class Settings
    {
        public int Id { get; set; }
        public int Logo { get; set; }
        public int InvoiceHeader { get; set; }
        public int InvoiceFooter { get; set; }
        public int VoucherHeader { get; set; }
        public int VoucherFooter { get; set; }
        public int HotelRequest { get; set; }
        public int HotelChange { get; set; }
        public int HotelCancel { get; set; }
        public int HotelFooter { get; set; }
        public string EmailSMTP { get; set; }
        public string EmailPort { get; set; }
        public bool EmailUseSSL { get; set; }
        public bool EmailUseCredentials { get; set; }
        public string EmailDefaultUser { get; set; }
        public string EmailDefaultPass { get; set; }
        public string AccountingName { get; set; }
        public string AccountingTitle { get; set; }
        public string BankName { get; set; }
        public string BankAccount { get; set; }
        public string BankSwift { get; set; }
        public string BankAccountNo { get; set; }
        public string BankAddress { get; set; }
        public int DefaultCurrency { get; set; }
        public string OfficeAddress { get; set; }
        public string BankIBAN { get; set; }
        public string BankBranch { get; set; }
    }
}
