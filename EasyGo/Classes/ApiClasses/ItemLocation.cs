using EasyGo.ApiClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyGo.Classes.ApiClasses
{
    public class ItemLocation
    {
        #region Attributes
        public long ItemLocId { get; set; }
        public Nullable<int> LocationId { get; set; }
        public Nullable<long> Quantity { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<long> CreateUserId { get; set; }
        public Nullable<long> UpdateUserId { get; set; }
        public bool IsExpired { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public Nullable<long> ItemUnitId { get; set; }
        public string Notes { get; set; }

        //extra
        public string ItemName { get; set; }
        public string LocationName { get; set; }
        public string SectionName { get; set; }
        public string UnitName { get; set; }
        public string ItemType { get; set; }
        public bool IsFreeZone { get; set; }

        internal Task<int> changeUnitExpireDate(object itemsLocId, DateTime startDate, DateTime endDate, long userId)
        {
            throw new NotImplementedException();
        }

        internal Task<List<ItemLocation>> GetFreeZoneItems(object branchId)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Methods
        public async Task<int> getAmountInBranch(long itemUnitId, int branchId)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "ItemLocation/getAmountInBranch";

            parameters.Add("branchId", branchId.ToString());
            parameters.Add("itemUnitId", itemUnitId.ToString());

            return (int)await APIResult.PostReturnDecimal(method, parameters);
        }
        #endregion
    }
}
