using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyGo.Classes.ApiClasses
{
    public class SalesInvoice
    {
        #region Attributes
        public long InvoiceId { get; set; }
        public string InvNumber { get; set; }
        public string InvType { get; set; }
        public Nullable<long> CustomerId { get; set; }
        public string DiscountType { get; set; }
        public Nullable<decimal> DiscountValue { get; set; }
        public Nullable<decimal> DiscountPercentage { get; set; }
        public Nullable<decimal> Total { get; set; }
        public Nullable<decimal> TotalNet { get; set; }
        public Nullable<decimal> Paid { get; set; }
        public Nullable<decimal> Deserved { get; set; }
        public Nullable<System.DateTime> DeservedDate { get; set; }
        public Nullable<int> BranchCreatorId { get; set; }
        public Nullable<int> BranchId { get; set; }
        public Nullable<decimal> Tax { get; set; }
        public string TaxType { get; set; }
        public decimal TaxPercentage { get; set; }
        public Nullable<System.DateTime> InvDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<long> CreateUserId { get; set; }
        public Nullable<long> UpdateUserId { get; set; }
        public Nullable<long> InvoiceMainId { get; set; }
        public string Notes { get; set; }
        public Nullable<int> PosId { get; set; }
        public Nullable<byte> IsApproved { get; set; }
        public bool IsActive { get; set; }
        public decimal ShippingCost { get; set; }
        public decimal RealShippingCost { get; set; }
        public Nullable<decimal> Remain { get; set; }
        public Nullable<int> ShippingCompanyId { get; set; }
        public Nullable<long> ShipUserId { get; set; }
        public bool IsPrePaid { get; set; }
        public bool IsShipPaid { get; set; }
        public bool IsFreeShip { get; set; }

        //extra
        public string MainInvNumber { get; set; }
        public string BranchCreatorName { get; set; }
        public string BranchName { get; set; }
        public int ItemsCount { get; set; }
        public int Count { get; set; }

        public bool HasNextInvoice { get; set; }
        public bool HasPrevInvoice { get; set; }
        public List<SalesInvoiceItem> InvoiceItems { get; set; }
        public List<CashTransfer> ListPayments { get; set; }
        #endregion

        #region Methods
        #endregion
    }

    public class SalesInvoiceItem
    {
        public int ItemTransId { get; set; }
        public int Quantity { get; set; }
        public Nullable<long> InvoiceId { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<long> CreateUserId { get; set; }
        public Nullable<long> UpdateUserId { get; set; }
        public string Notes { get; set; }
        public decimal Price { get; set; }
        public Nullable<long> ItemUnitId { get; set; }
        public Nullable<int> OfferId { get; set; }
        public decimal Profit { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal ItemTax { get; set; }
        public decimal ItemUnitPrice { get; set; }
        public decimal OfferValue { get; set; }
        public string OfferType { get; set; }
        public string OfferName { get; set; }

        //extra
        public ItemUnit ItemUnit { get; set; }
        public Nullable<long> ItemId { get; set; }
        public string ItemName { get; set; }
        public string UnitName { get; set; }
        public string Barcode { get; set; }
        public string ItemType { get; set; }
        public Nullable<long> UnitId { get; set; }
        public List<Item> PackageItems { get; set; }
    }
}
