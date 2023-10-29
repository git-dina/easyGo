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
    public class Unit
    {
        #region Attributes
        public int UnitId { get; set; }
        public string Name { get; set; }
        public string Notes { get; set; }
        public Nullable<bool> IsActive { get; set; } = true;
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<long> CreateUserId { get; set; }
        public Nullable<long> UpdateUserId { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        #endregion

        #region Methods
        public async Task<string> Save(Unit unit)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "Unit/Save";

            var myContent = JsonConvert.SerializeObject(unit);
            parameters.Add("itemObject", myContent);
            return await APIResult.post(method, parameters);
        }

        public async Task<List<Unit>> Get()
        {
            List<Unit> units = new List<Unit>();

            IEnumerable<Claim> claims = await APIResult.getList("Unit/Get");

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    units.Add(JsonConvert.DeserializeObject<Unit>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return units;
        }
        public async Task<string> Delete(int unitId,long userId)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("itemId", unitId.ToString());
            parameters.Add("userId", userId.ToString());

            string method = "Unit/Delete";
            return await APIResult.post(method, parameters);
        }

        private List<int>  unitsIds = new List<int>();
        public async Task<List<Unit>> getSmallUnits(long itemId, int unitId)
        {
            if (FillCombo.itemUnitList is null)
                await FillCombo.RefreshItemUnit();
            if (FillCombo.unitsList is null)
                await FillCombo.RefreshUnits();

            var unitsList = FillCombo.itemUnitList
                   .Where(x => x.ItemId == itemId)
                    .Select(p => new ItemUnit
                    {
                        UnitId = p.UnitId,
                          SubUnitId = p.SubUnitId,
                    })
                   .ToList();

            unitsIds = new List<int>();

            var result = Recursive(unitsList, unitId);

         
            var units = (from u in FillCombo.unitsList
                         select new Unit()
                         {
                             UnitId = u.UnitId,
                             Name = u.Name,  
                             Notes = u.Notes,
                             CreateDate = u.CreateDate,
                             CreateUserId = u.CreateUserId,
                             UpdateDate = u.UpdateDate,
                             UpdateUserId = u.UpdateUserId,                           

                         }).Where(p => !unitsIds.Contains((int)p.UnitId)).ToList();

            return units;
        }

        public IEnumerable<ItemUnit> Recursive(List<ItemUnit> unitsList, int smallLevelid)
        {
            List<ItemUnit> inner = new List<ItemUnit>();

            foreach (var t in unitsList.Where(item => item.SubUnitId == smallLevelid))
            {
                if (t.UnitId.Value != smallLevelid)
                {
                    unitsIds.Add(t.UnitId.Value);
                    inner.Add(t);
                }
                if (t.UnitId.Value == smallLevelid)
                    return inner;
                inner = inner.Union(Recursive(unitsList, t.UnitId.Value)).ToList();
            }

            return inner;
        }
        #endregion
    }
}
