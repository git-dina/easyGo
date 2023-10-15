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
    public class User
    {
        #region Attributes
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
        public bool IsActive { get; set; } = true;
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<long> CreateUserId { get; set; }
        public Nullable<long> UpdateUserId { get; set; }
        public Nullable<long> RoleId { get; set; }
        public Nullable<decimal> Balance { get; set; } = 0;
        public Nullable<byte> BalanceType { get; set; } = 0;
        public bool HasCommission { get; set; }
        public Nullable<decimal> CommissionValue { get; set; }
        public Nullable<decimal> CommissionRatio { get; set; }
        public Nullable<bool> IsAdmin { get; set; }
        #endregion

        #region Methods
        public async Task<string> Save(User user)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "User/Save";

            var myContent = JsonConvert.SerializeObject(user);
            parameters.Add("itemObject", myContent);
            return await APIResult.post(method, parameters);
        }

        public async Task<List<User>> Get()
        {
            List<User> users = new List<User>();

            IEnumerable<Claim> claims = await APIResult.getList("User/Get");

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    users.Add(JsonConvert.DeserializeObject<User>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return users;
        }
        public async Task<string> Delete(long delUserId, long userId)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("delUserId", delUserId.ToString());
            parameters.Add("userId", userId.ToString());

            string method = "User/Delete";
            return await APIResult.post(method, parameters);
        }
        public async Task<byte[]> DownloadImage(string imageName)
        {
            byte[] byteImg = null;
            if (imageName != "")
            {
                byteImg = await APIResult.getImage("User/GetImage", imageName);

                string dir = Directory.GetCurrentDirectory();
                string tmpPath = Path.Combine(dir, Global.TMPUsersFolder);
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
        public async Task<string> uploadImage(string imagePath, string imageName, long userId)
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
                    string tmpPath = Path.Combine(dir, Global.TMPUsersFolder);
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

                            var response = await client.PostAsync(@"user/PostUserImage", form);

                        }
                        stream.Dispose();
                    }
                    // save image name in DB
                    User user = new User();
                    user.UserId = userId;
                    user.Image = fileName;
                    await updateImage(user);
                    return fileName;
                }
                catch
                { return ""; }
            }
            return "";
        }

        public async Task<string> updateImage(User user)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            var myContent = JsonConvert.SerializeObject(user);
            parameters.Add("itemObject", myContent);

            string method = "User/UpdateImage";
            return await APIResult.post(method, parameters);
        }
        #endregion
    }

    public class UserLog
    {
        public long LogId { get; set; }
        public Nullable<System.DateTime> SInDate { get; set; }
        public Nullable<System.DateTime> SOutDate { get; set; }
        public Nullable<long> PosId { get; set; }
        public Nullable<long> UserId { get; set; }
    }
}
