﻿using EasyGo.ApiClasses;
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
    public class Category
    {
        #region Attributes
        public int CategoryId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Details { get; set; }
        public string Image { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<int> ParentId { get; set; }
        public string Notes { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<long> CreateUserId { get; set; }
        public Nullable<long> UpdateUserId { get; set; }

        #endregion

        #region Methods
        public async Task<string> Save(Category category)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "Category/Save";

            var myContent = JsonConvert.SerializeObject(category);
            parameters.Add("itemObject", myContent);
            return await APIResult.post(method, parameters);
        }

        public async Task<List<Category>> Get()
        {
            List<Category> units = new List<Category>();

            IEnumerable<Claim> claims = await APIResult.getList("Category/Get");

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    units.Add(JsonConvert.DeserializeObject<Category>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return units;
        }
        public async Task<string> Delete(long delUserId, long categoryId)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("delUserId", delUserId.ToString());
            parameters.Add("unitId", categoryId.ToString());

            string method = "Category/Delete";
            return await APIResult.post(method, parameters);
        }

        public async Task<byte[]> DownloadImage(string imageName)
        {
            byte[] byteImg = null;
            if (imageName != "")
            {
                byteImg = await APIResult.getImage("User/GetImage", imageName);

                string dir = Directory.GetCurrentDirectory();
                string tmpPath = Path.Combine(dir, Global.TMPCatFolder);
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
        public async Task<string> uploadImage(string imagePath, string imageName, int categoryId)
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
                    string tmpPath = Path.Combine(dir, Global.TMPCatFolder);
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

                            var response = await client.PostAsync(@"Category/PostUserImage", form);

                        }
                        stream.Dispose();
                    }
                    // save image name in DB
                    Category cat = new Category();
                    cat.CategoryId = categoryId;
                    cat.Image = fileName;
                    await updateImage(cat);
                    return fileName;
                }
                catch
                { return ""; }
            }
            return "";
        }

        public async Task<string> updateImage(Category cat)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            var myContent = JsonConvert.SerializeObject(cat);
            parameters.Add("itemObject", myContent);

            string method = "Category/UpdateImage";
            return await APIResult.post(method, parameters);
        }
        #endregion
    }
}