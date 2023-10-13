using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyGo.Classes.ApiClasses
{
    public class User
    {
        public long UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Mobile { get; set; }
        public string Notes { get; set; }
        public string Image { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public Nullable<bool> IsOnline { get; set; }
        public Nullable<short> IsActive { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<int> CreateUserId { get; set; }
        public Nullable<int> UpdateUserId { get; set; }
        public Nullable<long> RoleId { get; set; }
        public Nullable<decimal> Balance { get; set; }
        public Nullable<byte> BalanceType { get; set; }
        public bool HasCommission { get; set; }
        public Nullable<decimal> CommissionValue { get; set; }
        public Nullable<decimal> CommissionRatio { get; set; }
    }
}
