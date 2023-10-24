﻿using EasyGo.ApiClasses;
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
    public class Item
    {
        #region Attributes
        public long ItemId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Details { get; set; }
        public string Type { get; set; }
        public string Image { get; set; }
        public string Notes { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<int> Min { get; set; }
        public Nullable<int> Max { get; set; }
        public Nullable<int> CategoryId { get; set; }
        public bool IsExpired { get; set; }
        public int AlertDays { get; set; }
        public bool IsTaxExempt { get; set; }
        public Nullable<decimal> Taxes { get; set; }
        public Nullable<int> MinUnitId { get; set; }
        public Nullable<int> MaxUnitId { get; set; }
        public Nullable<decimal> AvgPurchasePrice { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<long> CreateUserId { get; set; }
        public Nullable<long> UpdateUserId { get; set; }
        #endregion

        #region Methods

        public async Task<List<Item>> Get()
        {
            List<Item> items = new List<Item>();

            IEnumerable<Claim> claims = await APIResult.getList("Item/Get");

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    items.Add(JsonConvert.DeserializeObject<Item>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return items;
        }
        #endregion
    }
}
