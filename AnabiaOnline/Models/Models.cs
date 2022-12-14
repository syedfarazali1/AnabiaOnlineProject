//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

public partial class City
{
    public int CityId { get; set; }
    public string Cit_Name { get; set; }
    public Nullable<int> countryid { get; set; }
}

public partial class Country
{
    public int CountryId { get; set; }
    public string Con_Name { get; set; }
}

public partial class Invoice
{
    public int InvoiceID { get; set; }
    public Nullable<int> StoreID { get; set; }
    public string InvoiceNumber { get; set; }
    public string InvoiceMonth { get; set; }
    public string InvoiceDate { get; set; }
    public string DueDate { get; set; }
    public Nullable<decimal> InvoiceAmount { get; set; }
    public Nullable<int> InvoiceType { get; set; }
    public Nullable<int> InvoiceGenerateBy { get; set; }
    public Nullable<int> PaymentMethod { get; set; }
    public Nullable<int> PaymentStatus { get; set; }

    public virtual InvoiceType InvoiceType1 { get; set; }
    public virtual PaymentMethod PaymentMethod1 { get; set; }
    public virtual Store Store { get; set; }
}

public partial class InvoicePaymentReceipt
{
    public int IPRID { get; set; }
    public Nullable<int> InvoiceID { get; set; }
    public string Receipt { get; set; }
    public string ReceiptDate { get; set; }

    public virtual Store Store { get; set; }
}

public partial class InvoiceType
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public InvoiceType()
    {
        this.Invoices = new HashSet<Invoice>();
    }

    public int InvoiceTypeID { get; set; }
    public string InvoiceType1 { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
    public virtual ICollection<Invoice> Invoices { get; set; }
}

public partial class logo
{
    public int LogoId { get; set; }
    public string Logo1 { get; set; }
    public Nullable<int> StoreID { get; set; }
}

public partial class Order
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public Order()
    {
        this.OrderDetails = new HashSet<OrderDetail>();
    }

    public int OrderID { get; set; }
    public Nullable<int> StoreID { get; set; }
    public string CustomerName { get; set; }
    public string ContactNumber { get; set; }
    public string Address { get; set; }
    public Nullable<int> Country { get; set; }
    public Nullable<int> City { get; set; }
    public string OrderDate { get; set; }
    public string DispatchDate { get; set; }
    public string OrderDispatchedBy { get; set; }
    public string DeliveryDate { get; set; }
    public string OrderDeliverBy { get; set; }
    public Nullable<int> OrderStatus { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
    public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    public virtual Store Store { get; set; }
}

public partial class OrderDetail
{
    public int OrderDetailID { get; set; }
    public Nullable<int> StoreID { get; set; }
    public Nullable<int> OrderID { get; set; }
    public Nullable<int> CategoryID { get; set; }
    public Nullable<int> ProductID { get; set; }
    public Nullable<int> Quantity { get; set; }
    public Nullable<decimal> Price { get; set; }
    public Nullable<decimal> DeliveryCharges { get; set; }
    public Nullable<decimal> TotalAmount { get; set; }

    public virtual ProductCategory ProductCategory { get; set; }
    public virtual Order Order { get; set; }
    public virtual Product Product { get; set; }
    public virtual Store Store { get; set; }
}

public partial class Package
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public Package()
    {
        this.Stores = new HashSet<Store>();
    }

    public int PackageID { get; set; }
    public string PackageName { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
    public virtual ICollection<Store> Stores { get; set; }
}

public partial class PaymentMethod
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public PaymentMethod()
    {
        this.Invoices = new HashSet<Invoice>();
    }

    public int PaymentMethodID { get; set; }
    public string PaymentMethod1 { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
    public virtual ICollection<Invoice> Invoices { get; set; }
}

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

public partial class ProductCategory
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public ProductCategory()
    {
        this.OrderDetails = new HashSet<OrderDetail>();
        this.Products = new HashSet<Product>();
    }

    public int CategoryID { get; set; }
    public Nullable<int> StoreID { get; set; }
    public string CategoryName { get; set; }
    public string CreationDate { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
    public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    public virtual Store Store { get; set; }
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
    public virtual ICollection<Product> Products { get; set; }
}

public partial class ProductImage
{
    public int ProductImagesID { get; set; }
    public Nullable<int> StoreID { get; set; }
    public Nullable<int> ProductID { get; set; }
    public string ProductImage1 { get; set; }

    public virtual Product Product { get; set; }
    public virtual Store Store { get; set; }
}

public partial class Role
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public Role()
    {
        this.Users = new HashSet<User>();
    }

    public int RoleID { get; set; }
    public string RoleName { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
    public virtual ICollection<User> Users { get; set; }
}

public partial class Store
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public Store()
    {
        this.Invoices = new HashSet<Invoice>();
        this.InvoicePaymentReceipts = new HashSet<InvoicePaymentReceipt>();
        this.OrderDetails = new HashSet<OrderDetail>();
        this.Orders = new HashSet<Order>();
        this.ProductCategories = new HashSet<ProductCategory>();
        this.ProductImages = new HashSet<ProductImage>();
        this.Products = new HashSet<Product>();
        this.StoreWebs = new HashSet<StoreWeb>();
    }

    public int StoreID { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string ContactNumber { get; set; }
    public string BusinessName { get; set; }
    public string BusinessAddress { get; set; }
    public Nullable<int> Country { get; set; }
    public Nullable<int> City { get; set; }
    public Nullable<int> State { get; set; }
    public string EmailID { get; set; }
    public string CnicHolderName { get; set; }
    public string CnicNumber { get; set; }
    public string CnicExpiryDate { get; set; }
    public string FrontCopyOfCnic { get; set; }
    public string BackCopyOfCnic { get; set; }
    public string DomainName { get; set; }
    public Nullable<int> Package { get; set; }
    public string LoginID { get; set; }
    public string Password { get; set; }
    public Nullable<int> Status { get; set; }
    public string RegistrationDate { get; set; }
    public Nullable<int> ApprovedBy { get; set; }
    public string Store_Logo { get; set; }
    public Nullable<int> CityId { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
    public virtual ICollection<Invoice> Invoices { get; set; }
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
    public virtual ICollection<InvoicePaymentReceipt> InvoicePaymentReceipts { get; set; }
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
    public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
    public virtual ICollection<Order> Orders { get; set; }
    public virtual Package Package1 { get; set; }
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
    public virtual ICollection<ProductCategory> ProductCategories { get; set; }
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
    public virtual ICollection<ProductImage> ProductImages { get; set; }
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
    public virtual ICollection<Product> Products { get; set; }
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
    public virtual ICollection<StoreWeb> StoreWebs { get; set; }
}

public partial class StoreWeb
{
    public int StoreWebID { get; set; }
    public Nullable<int> StoreID { get; set; }
    public string StoreBanner1 { get; set; }
    public string StoreBanner2 { get; set; }
    public string StoreBanner3 { get; set; }
    public string CreationDate { get; set; }

    public virtual Store Store { get; set; }
}

public partial class sysdiagram
{
    public string name { get; set; }
    public int principal_id { get; set; }
    public int diagram_id { get; set; }
    public Nullable<int> version { get; set; }
    public byte[] definition { get; set; }
}

public partial class User
{
    public int UserID { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public Nullable<int> Department { get; set; }
    public Nullable<int> Designation { get; set; }
    public string CnicNumber { get; set; }
    public string LoginID { get; set; }
    public string Password { get; set; }
    public string CreationDate { get; set; }
    public Nullable<int> RoleID { get; set; }

    public virtual Role Role { get; set; }
}
