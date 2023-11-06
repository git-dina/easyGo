using EasyGo.ApiClasses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyGo.Classes.ApiClasses
{
    public class Notification
    {
        #region Attributs
        public long NotId { get; set; }
        public string Title { get; set; }
        public string Ncontent { get; set; }
        public string MsgType { get; set; }
        public bool IsActive { get; set; } = true;
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<long> CreateUserId { get; set; }
        public Nullable<long> UpdateUserId { get; set; }

        //extra
        public string ObjectName { get; set; }
        public string Prefix { get; set; }

        public Nullable<int> RecieveId { get; set; }
        public Nullable<int> BranchId { get; set; }

        #endregion
        //***********************************************
        public async Task<string> save(Notification obj, int branchId, string objectName, string prefix,
            int userId = 0, int posId = 0)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "Notification/Save";
            var myContent = JsonConvert.SerializeObject(obj);
            parameters.Add("itemObject", myContent);
            parameters.Add("branchId", branchId.ToString());
            parameters.Add("objectName", objectName);
            parameters.Add("prefix", prefix);
            parameters.Add("userId", userId.ToString());
            parameters.Add("posId", posId.ToString());


            return await APIResult.post(method, parameters);
        }

    }
}
