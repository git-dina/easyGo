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

        internal async Task<List<ItemLocation>> GetFreeZoneItems(int branchId)
        {
            List<ItemLocation> list = new List<ItemLocation>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("branchId", branchId.ToString());

            //#################
            IEnumerable<Claim> claims = await APIResult.getList("ItemLocation/GetFreeZoneItems", parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    list.Add(JsonConvert.DeserializeObject<ItemLocation>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return list;

        }

        public async Task<string> changeUnitExpireDate(long itemLocId, DateTime startDate, DateTime endDate, long userId)
        {

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "ItemLocation/ChangeUnitExpireDate";


            parameters.Add("itemLocId", itemLocId.ToString());
            parameters.Add("userId", userId.ToString());
            parameters.Add("startDate", startDate.ToString());
            parameters.Add("endDate", endDate.ToString());

            return await APIResult.post(method, parameters);
        }
        public async Task<string> SaveItemNotes(long itemLocId,string notes,  long userId)
        {

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "ItemLocation/SaveItemNotes";


            parameters.Add("itemLocId", itemLocId.ToString());
            parameters.Add("userId", userId.ToString());
            parameters.Add("notes", notes.ToString());

            return await APIResult.post(method, parameters);
        }
        #endregion
    }
}
