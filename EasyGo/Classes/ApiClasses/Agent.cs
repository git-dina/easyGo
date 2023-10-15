using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyGo.Classes.ApiClasses
{
    public class Agent
    {
        #region Attributes
        public long AgentId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Company { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Image { get; set; }
        public string Type { get; set; }
        public Nullable<decimal> Balance { get; set; }
        public Nullable<byte> BalanceType { get; set; }
        public string Fax { get; set; }
        public bool IsLimited { get; set; }
        public Nullable<decimal> MaxDeserve { get; set; }
        public string PayType { get; set; }
        public string Notes { get; set; }
        public bool IsActive { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<long> CreateUserId { get; set; }
        public Nullable<long> UpdateUserId { get; set; }
        #endregion
    }
}
