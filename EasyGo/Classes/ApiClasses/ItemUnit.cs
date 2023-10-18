using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyGo.Classes.ApiClasses
{
    public class ItemUnit
    {
        #region Attributes
        public long ItemUnitId { get; set; }
        public Nullable<long> ItemId { get; set; }
        public Nullable<int> UnitId { get; set; }
        public Nullable<int> SubUnitId { get; set; }
        public Nullable<int> UnitValue { get; set; }
        public Nullable<bool> IsDefaultSale { get; set; }
        public Nullable<bool> IsDefaultPurchase { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<decimal> Cost { get; set; }
        public string Barcode { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<int> CreateUserId { get; set; }
        public Nullable<int> UpdateUserId { get; set; }
        public Nullable<decimal> purchasePrice { get; set; }
        #endregion
    }
}
