//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AnabiaOnline.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class InvoicePaymentReceipt
    {
        public int IPRID { get; set; }
        public Nullable<int> InvoiceID { get; set; }
        public string Receipt { get; set; }
        public string ReceiptDate { get; set; }
    
        public virtual Store Store { get; set; }
    }
}
