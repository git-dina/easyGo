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
        List<long> itemUnitsIds = new List<long>();
        public List<ItemUnit> getSmallItemUnits(long itemId,long itemUnitId)
        {
            var item = FillCombo.itemsHasUnitsList.Where(x => x.ItemId == itemId).FirstOrDefault();
            List<ItemUnit> unitsList = item.ItemUnits
                       .Where(x => x.ItemId == itemId)
                        .Select(p => new ItemUnit
                        {
                            ItemUnitId = p.ItemUnitId,
                            UnitId = p.UnitId,
                            SubUnitId = p.SubUnitId,
                        })
                       .ToList();

            var unitId = unitsList.Where(x => x.ItemUnitId == itemUnitId).Select(x => x.UnitId).Single();
            itemUnitsIds = new List<long>();
            itemUnitsIds.Add(itemUnitId);

            var result = Recursive(unitsList, (long)unitId);

            var units = unitsList.Where(p => !itemUnitsIds.Contains((long)p.ItemUnitId)).ToList();

           return units;
        }

        public IEnumerable<ItemUnit> Recursive(List<ItemUnit> unitsList, long smallLevelid)
        {
            List<ItemUnit> inner = new List<ItemUnit>();

            foreach (var t in unitsList.Where(item => item.SubUnitId == smallLevelid && item.UnitId != smallLevelid))
            {

                itemUnitsIds.Add(t.ItemUnitId);
                inner.Add(t);

                if (t.UnitId.Value == smallLevelid)
                    return inner;
                inner = inner.Union(Recursive(unitsList, t.UnitId.Value)).ToList();
            }

            return inner;
        }

        public int fromUnitToUnitQuantity(int quantity, long itemId, long fromItemUnitId, long toItemUnitId)
        {
            int remain = 0;
            int _ConversionQuantity;
            int _ToQuantity = 0;

            if (quantity != 0)
            {
                List<ItemUnit> smallUnits =  getSmallItemUnits(itemId, fromItemUnitId);

                var isSmall = smallUnits.Find(x => x.ItemUnitId == toItemUnitId);
                if (isSmall != null) // from-unit is bigger than to-unit
                {
                    _ConversionQuantity =  largeToSmallUnitQuan(fromItemUnitId, toItemUnitId, itemId);
                    _ToQuantity = quantity * _ConversionQuantity;

                }
                else
                {
                    _ConversionQuantity =  smallToLargeUnit(fromItemUnitId, toItemUnitId, itemId);

                    if (_ConversionQuantity != 0)
                    {
                        _ToQuantity = quantity / _ConversionQuantity;
                        remain = quantity - (_ToQuantity * _ConversionQuantity); // get remain quantity which cannot be changeed
                    }
                }
            }

            return _ToQuantity;
        }

        public int largeToSmallUnitQuan(long fromItemUnit, long toItemUnit,long itemId)
        {
            int amount = 0;
            amount += getUnitConversionQuan(fromItemUnit, toItemUnit, itemId);

            return amount;
        }
        public int smallToLargeUnit(long fromItemUnit, long toItemUnit,long itemId)
        {
            int amount = 0;
            amount = getLargeUnitConversionQuan(fromItemUnit, toItemUnit,  itemId);

            return amount;
        }

        private int getLargeUnitConversionQuan(long fromItemUnit, long toItemUnit, long itemId)
        {
            int amount = 0;

            var item = FillCombo.itemsHasUnitsList.Where(x => x.ItemId == itemId).FirstOrDefault();
            List<ItemUnit> unitsList = item.ItemUnits
                       .Where(x => x.ItemId == itemId)
                        .Select(p => new ItemUnit
                        {
                            ItemUnitId = p.ItemUnitId,
                            UnitId = p.UnitId,
                            SubUnitId = p.SubUnitId,
                            UnitValue = p.UnitValue,
                            ItemId = p.ItemId,
                        })
                       .ToList();

            var unit = unitsList.Where(x => x.ItemUnitId == toItemUnit).Select(x => new { x.UnitId, x.ItemId, x.SubUnitId, x.UnitValue }).FirstOrDefault();
            var smallUnit = unitsList.Where(x => x.UnitId == unit.SubUnitId && x.ItemId == unit.ItemId).Select(x => new { x.UnitValue, x.ItemUnitId }).FirstOrDefault();

            if (toItemUnit == smallUnit.ItemUnitId)
            {
                amount = 1;
                return amount;
            }
            if (smallUnit != null)
                amount += (int)unit.UnitValue * getLargeUnitConversionQuan(fromItemUnit, smallUnit.ItemUnitId, itemId);

            return amount;
        }
        private int getUnitConversionQuan(long fromItemUnit, long toItemUnit,long itemId)
        {
            int amount = 0;

            var item = FillCombo.itemsHasUnitsList.Where(x => x.ItemId == itemId).FirstOrDefault();
            List<ItemUnit> unitsList = item.ItemUnits
                       .Where(x => x.ItemId == itemId)
                        .Select(p => new ItemUnit
                        {
                            ItemUnitId = p.ItemUnitId,
                            UnitId = p.UnitId,
                            SubUnitId = p.SubUnitId,
                        })
                       .ToList();

            var unit = unitsList.Where(x => x.ItemUnitId == toItemUnit).Select(x => new { x.UnitId, x.ItemId }).FirstOrDefault();
            var upperUnit = unitsList.Where(x => x.SubUnitId == unit.UnitId && x.ItemId == unit.ItemId && x.SubUnitId != x.UnitId ).Select(x => new { x.UnitValue, x.ItemUnitId }).FirstOrDefault();
            if (upperUnit != null)
                amount = (int)upperUnit.UnitValue;
            if (fromItemUnit == upperUnit.ItemUnitId)
                return amount;
            if (upperUnit != null)
                amount += (int)upperUnit.UnitValue * getUnitConversionQuan(fromItemUnit, upperUnit.ItemUnitId,itemId);

            return amount;
            
        }
        #endregion
    }
}
