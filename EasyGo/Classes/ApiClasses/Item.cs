using EasyGo.ApiClasses;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
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
        public Nullable<bool> IsActive { get; set; } = true;
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

        public async Task<string> Save(Item item)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "Item/Save";

            var myContent = JsonConvert.SerializeObject(item);
            parameters.Add("itemObject", myContent);
            return await APIResult.post(method, parameters);
        }

        public async Task<string> UploadImage(string imagePath, string imageName, long itemId)
        {
            if (imagePath != "")
            {
                MultipartFormDataContent form = new MultipartFormDataContent();
                // get file extension
                var ext = imagePath.Substring(imagePath.LastIndexOf('.'));
                var extension = ext.ToLower();
                string fileName = imageName + extension;
                try
                {
                    // configure trmporery path
                    string dir = Directory.GetCurrentDirectory();
                    string tmpPath = Path.Combine(dir, Global.TMPItemFolder);
                    string[] files = System.IO.Directory.GetFiles(tmpPath, imageName + ".*");
                    foreach (string f in files)
                    {
                        System.IO.File.Delete(f);
                    }

                    tmpPath = Path.Combine(tmpPath, imageName + extension);

                    if (imagePath != tmpPath) // edit mode
                    {
                        // resize image
                        ImageProcess imageP = new ImageProcess(150, imagePath);
                        imageP.ScaleImage(tmpPath);

                        // read image file
                        var stream = new FileStream(tmpPath, FileMode.Open, FileAccess.Read);

                        // create http client request
                        using (var client = new HttpClient())
                        {
                            client.BaseAddress = new Uri(Global.APIUri);
                            client.Timeout = System.TimeSpan.FromSeconds(3600);
                            string boundary = string.Format("----WebKitFormBoundary{0}", DateTime.Now.Ticks.ToString("x"));
                            HttpContent content = new StreamContent(stream);
                            content.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
                            content.Headers.Add("client", "true");

                            content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                            {
                                Name = imageName,
                                FileName = fileName
                            };
                            form.Add(content, "fileToUpload");

                            var response = await client.PostAsync(@"Item/PostItemImage", form);

                        }
                        stream.Dispose();
                    }
                    // save image name in DB
                    await UpdateImage(itemId,fileName);
                    return fileName;
                }
                catch
                { return ""; }
            }
            return "";
        }

        public async Task<string> UpdateImage(long itemId, string fileName)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();

            parameters.Add("itemId", itemId.ToString());
            parameters.Add("image", fileName);

            string method = "Item/UpdateImage";
            return await APIResult.post(method, parameters);
        }

        public async Task<string> Delete(long itemId, long userId)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("itemId", itemId.ToString());
            parameters.Add("userId", userId.ToString());

            string method = "Item/Delete";
            return await APIResult.post(method, parameters);
        }
        #endregion
    }
}
