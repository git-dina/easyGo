using EasyGo.ApiClasses;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EasyGo.Classes.ApiClasses
{
    public class PurchaseInvoice
    {
        #region Attributes
        public long InvoiceId { get; set; }
        public string InvNumber { get; set; }
        public string InvType { get; set; }
        public Nullable<long> SupplierId { get; set; }
        public string DiscountType { get; set; } = "rate";
        public decimal DiscountValue { get; set; }
        public decimal DiscountPercentage { get; set; }
        public Nullable<decimal> Total { get; set; }
        public Nullable<decimal> TotalNet { get; set; }
        public Nullable<decimal> Paid { get; set; }
        public Nullable<decimal> Deserved { get; set; }
        public Nullable<System.DateTime> DeservedDate { get; set; }
        public Nullable<int> BranchCreatorId { get; set; }
        public Nullable<int> BranchId { get; set; }
        public decimal Tax { get; set; } = 0;
        public string TaxType { get; set; } = "rate";
        public decimal TaxPercentage { get; set; } 
        public Nullable<System.DateTime> InvDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<long> CreateUserId { get; set; }
        public Nullable<long> UpdateUserId { get; set; }
        public Nullable<long> InvoiceMainId { get; set; }
        public string Notes { get; set; }
        public string VendorInvNum { get; set; }
        public Nullable<System.DateTime> VendorInvDate { get; set; }
        public Nullable<int> PosId { get; set; }
        public Nullable<byte> IsApproved { get; set; }
        public decimal ManualDiscountValue { get; set; }
        public string ManualDiscountType { get; set; }
        public bool IsActive { get; set; } = true;
        public decimal ShippingCost { get; set; }
        public Nullable<System.TimeSpan> InvTime { get; set; }
        //extra

        public string MainInvNumber { get; set; }
        public string SupplierName { get; set; }
        public string SupplierCode { get; set; }
        public string SupplierCompany { get; set; }
        public string SupplierMobile { get; set; }
        public string BranchCreatorName { get; set; }
        public string BranchName { get; set; }
        public string UserName { get; set; }
        public string UserLastName { get; set; }
        public List<PurInvoiceItem> InvoiceItems { get; set; }
        public List<PayedInvclass> cachTrans { get; set; }
       public List<CashTransfer> ListPayments { get; set; }
        public int Count { get; set; }
        #endregion
        #region Methods

        public CashTransfer posCashTransfer(PurchaseInvoice invoice, string invType)
        {
            #region pos Cash transfer
            CashTransfer posCash = new CashTransfer();
            posCash.PosId = MainWindow.posLogin.PosId;
            posCash.AgentId = invoice.SupplierId;
            posCash.CreateUserId = invoice.CreateUserId;
            posCash.ProcessType = "inv";
            posCash.Cash = (decimal)invoice.TotalNet;
            posCash.Side = "v"; // vendor

            #endregion
            switch (invType)
            {

                case "pi"://purchase invoice
                    posCash.TransType = "d";
                    posCash.TransNum = "dv";
                 
                    break;

                case "pb"://purchase bounce invoice
                    posCash.TransType = "p";
                    posCash.TransNum = "pv";
                    break;
            }

            return posCash;
        }

        public async Task<InvoiceResult> savePurchaseInvoice(PurchaseInvoice item,  Notification amountNot, 
                                                           CashTransfer PosCashTransfer, List<CashTransfer> listPayments, int posId)
        {

            InvoiceResult invoiceResult = new InvoiceResult();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "PurchaseInvoice/savePurchaseInvoice";

            var myContent = JsonConvert.SerializeObject(item);
            parameters.Add("itemObject", myContent);

            myContent = JsonConvert.SerializeObject(amountNot);
            parameters.Add("amountNot", myContent);


            myContent = JsonConvert.SerializeObject(PosCashTransfer);
            parameters.Add("PosCashTransfer", myContent);

            myContent = JsonConvert.SerializeObject(listPayments);
            parameters.Add("listPayments", myContent);

            parameters.Add("posId", posId.ToString());

            IEnumerable<Claim> claims = await APIResult.getList(method, parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    invoiceResult = JsonConvert.DeserializeObject<InvoiceResult>(c.Value);
                }
            }
            return invoiceResult;
        }

        public async Task<InvoiceResult> savePurchaseDraft(PurchaseInvoice item, int posId)
        {
            InvoiceResult invoiceResult = new InvoiceResult();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "PurchaseInvoice/savePurchaseDraft";

            var myContent = JsonConvert.SerializeObject(item);
            parameters.Add("itemObject", myContent);


            parameters.Add("posId", posId.ToString());

            IEnumerable<Claim> claims = await APIResult.getList(method, parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    invoiceResult = JsonConvert.DeserializeObject<InvoiceResult>(c.Value);
                }
            }
            return invoiceResult;
        }
        public async Task<InvoiceResult> savePurchaseBounce(PurchaseInvoice item, List<CashTransfer> listPayments,
                           CashTransfer posCashTransfer, Notification notification, int posId, int branchId)
        {
            InvoiceResult invoiceResult = new InvoiceResult();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "PurchaseInvoice/savePurchaseBounce";

            var myContent = JsonConvert.SerializeObject(item);
            parameters.Add("itemObject", myContent);

            myContent = JsonConvert.SerializeObject(listPayments);
            parameters.Add("listPayments", myContent);

            myContent = JsonConvert.SerializeObject(posCashTransfer);
            parameters.Add("posCashTransfer", myContent);

            myContent = JsonConvert.SerializeObject(notification);
            parameters.Add("notification", myContent);

            parameters.Add("posId", posId.ToString());
            parameters.Add("branchId", branchId.ToString());
            IEnumerable<Claim> claims = await APIResult.getList(method, parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    invoiceResult = JsonConvert.DeserializeObject<InvoiceResult>(c.Value);
                }
            }
            return invoiceResult;
        }

        public async Task<List<PurchaseInvoice>> GetInvoicesByCreator(string invoiceType, long createUserId, int duration)
        {
            List<PurchaseInvoice> items = new List<PurchaseInvoice>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("invoiceType", invoiceType);
            parameters.Add("createUserId", createUserId.ToString());
            parameters.Add("duration", duration.ToString());

            IEnumerable<Claim> claims = await APIResult.getList("PurchaseInvoice/GetInvoicesByCreator", parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    items.Add(JsonConvert.DeserializeObject<PurchaseInvoice>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return items;
        }
        #endregion
    }

    public class PurInvoiceItem : INotifyPropertyChanged
    {
        public int InvItemId { get; set; }
        public Nullable<long> InvoiceId { get; set; }

        //public int Quantity { get; set; }
        private int _Quantity;
        public int Quantity
        {
            get => _Quantity;
            set
            {
                if (_Quantity == value) return;

                _Quantity = value;
                OnPropertyChanged();
                Total = Quantity * Price;
            }
        }

        public string Notes { get; set; }
        //public decimal Price { get; set; }
        private decimal _Price;
        public decimal Price
        {
            get => _Price;
            set
            {
                if (_Price == value) return;

                _Price = value;
                OnPropertyChanged();
                Total = Quantity * Price;
            }
        }
        //public decimal Total { get; set; }
        private decimal _Total;
        public decimal Total
        {
            get => _Total;
            set
            {
                if (_Total == value) return;

                _Total = value;
                OnPropertyChanged();
            }
        }
        public Nullable<long> ItemUnitId { get; set; }
        public long ItemId { get; set; }
        public string UnitName { get; set; }
        public bool IsActive { get; set; } = true;
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<long> CreateUserId { get; set; }
        public Nullable<long> UpdateUserId { get; set; }

        //extra
        public ItemUnit ItemUnit { get; set; }
        public int Index { get; set; }
        public string ItemName { get; set; }
        public List<Item> PackageItems { get; set; }
        public string ItemType { get; set; }
        public string Barcode { get; set; }
        public int ID { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(
            [CallerMemberName]  string propertyName = null) 
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}
