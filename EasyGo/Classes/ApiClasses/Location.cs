using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyGo.Classes.ApiClasses
{
    public class Location
    {
        #region Attributes
        public int LocationId { get; set; }
        public string x { get; set; }
        public string y { get; set; }
        public string z { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<long> CreateUserId { get; set; }
        public Nullable<long> UpdateUserId { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<int> SectionId { get; set; }
        public string Notes { get; set; }
        public Nullable<int> BranchId { get; set; }
        public Nullable<bool> IsFreeZone { get; set; }

        //extra
        public string SectionName { get; set; }
        public string LocationName { get; set; }
        #endregion
    }
}
