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
    public class Branch
    {
        #region Attributes
        public int BranchId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string Notes { get; set; }
        public Nullable<int> ParentId { get; set; }
        public string Type { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<long> CreateUserId { get; set; }
        public Nullable<long> UpdateUserId { get; set; }

        #endregion

        #region Methods
        public async Task<List<Branch>> Get()
        {
            List<Branch> categories = new List<Branch>();

            IEnumerable<Claim> claims = await APIResult.getList("Branch/Get");

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    categories.Add(JsonConvert.DeserializeObject<Branch>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return categories;
        }
        #endregion
    }
}
