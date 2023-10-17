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
        #endregion
    }
}
