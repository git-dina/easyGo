using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyGo.Classes.ApiClasses
{
    public class PurchaseInvoice
    {
        #region Attributes
        public long InvoiceId { get; set; }
        public string InvNumber { get; set; }
        public string InvType { get; set; }
        public Nullable<long> SupplierId { get; set; }
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
        public Nullable<int> InvoiceMainId { get; set; }
        public string Notes { get; set; }
        public string VendorInvNum { get; set; }
        public Nullable<System.DateTime> VendorInvDate { get; set; }
        public Nullable<int> PosId { get; set; }
        public Nullable<byte> IsApproved { get; set; }
        public decimal ManualDiscountValue { get; set; }
        public string ManualDiscountType { get; set; }
        public bool IsActive { get; set; } = true;
        public decimal ShippingCost { get; set; }

        //extra
        public string SupplierName { get; set; }
        public List<PurInvoiceItem> InvoiceItems { get; set; }
        #endregion
    }

    public class PurInvoiceItem
    {
        public int InvItemId { get; set; }
        public Nullable<long> InvoiceId { get; set; }
        public Nullable<int> Quantity { get; set; }
        public string Notes { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<long> ItemUnitId { get; set; }
        public string UnitName { get; set; }
        public bool IsActive { get; set; } = true;
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<long> CreateUserId { get; set; }
        public Nullable<long> UpdateUserId { get; set; }

        //extra
        public ItemUnit ItemUnit { get; set; }
    }
}
