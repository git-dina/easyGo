using EasyGo.ApiClasses;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EasyGo.Classes.ApiClasses
{
    public class Card
    {
        #region Attributes
        public int CardId { get; set; }
        public string Name { get; set; }
        public string Notes { get; set; }
        public string Image { get; set; }
        public Nullable<bool> HasProcessNum { get; set; }
        public Nullable<decimal> CommissionValue { get; set; }
        public Nullable<decimal> CommissionRatio { get; set; }
        public decimal Balance { get; set; }
        public bool BalanceType { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<long> CreateUserId { get; set; }
        public Nullable<long> UpdateUserId { get; set; }
        #endregion

        #region Methods
        public async Task<List<Card>> Get()
        {
            List<Card> categories = new List<Card>();

            IEnumerable<Claim> claims = await APIResult.getList("Card/Get");

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    categories.Add(JsonConvert.DeserializeObject<Card>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return categories;
        }


        public async Task<byte[]> downloadImage(string imageName)
        {
            byte[] byteImg = null;
            if (imageName != "")
            {
                byteImg = await APIResult.getImage("Categories/GetImage", imageName);

                string dir = Directory.GetCurrentDirectory();
                string tmpPath = Path.Combine(dir, Global.TMPFolder);
                if (!Directory.Exists(tmpPath))
                    Directory.CreateDirectory(tmpPath);
                tmpPath = Path.Combine(tmpPath, imageName);
                if (System.IO.File.Exists(tmpPath))
                {
                    System.IO.File.Delete(tmpPath);
                }
                if (byteImg != null)
                {
                    using (FileStream fs = new FileStream(tmpPath, FileMode.Create, FileAccess.ReadWrite))
                    {
                        fs.Write(byteImg, 0, byteImg.Length);
                    }
                }

            }

            return byteImg;

        }

        internal Task<string> Save(Card card)
        {
            throw new NotImplementedException();
        }

        internal Task<string> uploadImage(string imgFileName, string v, long cardId)
        {
            throw new NotImplementedException();
        }

        internal Task<string> Delete(int cardId, long userId)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
