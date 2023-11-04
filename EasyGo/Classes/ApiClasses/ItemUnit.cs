using EasyGo.ApiClasses;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
        public Nullable<bool> IsDefaultSale { get; set; } = false;
        public Nullable<bool> IsDefaultPurchase { get; set; } = false;
        public Nullable<decimal> Price { get; set; }
        public Nullable<decimal> Cost { get; set; }
        public string Barcode { get; set; }
        public string Notes { get; set; }
        public Nullable<bool> IsActive { get; set; } = true;
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<long> CreateUserId { get; set; }
        public Nullable<long> UpdateUserId { get; set; }
        public Nullable<decimal> PurchasePrice { get; set; }
        public Nullable<decimal> PackCost { get; set; }
        public int UnitCount { get; set; }
        public Nullable<int> SmallestUnitId { get; set; }

        //extra
        public string ItemName { get; set; }
        public string ItemCode { get; set; }
        public string UnitName { get; set; }
        public string SmallUnit { get; set; }
        public string SmallestUnitName { get; set; }
        public string ItemType { get; set; }
        public Nullable<long> Quantity { get; set; }
        #endregion

        #region Methods

        public async Task<List<ItemUnit>> Get()
        {
            List<ItemUnit> itemUnits = new List<ItemUnit>();

            IEnumerable<Claim> claims = await APIResult.getList("ItemUnit/Get");

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    itemUnits.Add(JsonConvert.DeserializeObject<ItemUnit>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return itemUnits;
        }
         public async Task<List<ItemUnit>> GetItemUnit(long itemId)
        {
            List<ItemUnit> itemUnits = new List<ItemUnit>();

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("itemId", itemId.ToString());
            IEnumerable<Claim> claims = await APIResult.getList("ItemUnit/GetItemUnit",parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    itemUnits.Add(JsonConvert.DeserializeObject<ItemUnit>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return itemUnits;
        }

        public async Task<string> Save(ItemUnit itemUnit)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "ItemUnit/Save";

            var myContent = JsonConvert.SerializeObject(itemUnit);
            parameters.Add("Object", myContent);
            return await APIResult.post(method, parameters);
        }

        public async Task<string> Delete(long itemUnitId, long userId)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("itemId", itemUnitId.ToString());
            parameters.Add("userId", userId.ToString());

            string method = "ItemUnit/Delete";
            return await APIResult.post(method, parameters);
        }
        #endregion
    }
}
