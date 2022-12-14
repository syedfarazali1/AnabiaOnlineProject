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
    
    public partial class Product
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Product()
        {
            this.OrderDetails = new HashSet<OrderDetail>();
            this.ProductImages = new HashSet<ProductImage>();
        }
    
        public int ProductID { get; set; }
        public Nullable<int> StoreID { get; set; }
        public Nullable<int> CategoryID { get; set; }
        public string ProductName { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<decimal> DeliveryCharges { get; set; }
        public string ProductDescription { get; set; }
        public string CreationDate { get; set; }
        public Nullable<int> ProductStatus { get; set; }
        public string ProductImage { get; set; }
        public Nullable<int> ProductStockStatus { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ProductCategory ProductCategory { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductImage> ProductImages { get; set; }
        public virtual Store Store { get; set; }
    }
}
