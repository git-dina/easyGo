using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyGo.Classes.ApiClasses
{
    public class CashTransfer
    {
        #region Attributes
        public long CashTransId { get; set; }
        public string TransType { get; set; }
        public Nullable<int> PosId { get; set; }
        public Nullable<long> UserId { get; set; }
        public Nullable<long> AgentId { get; set; }
        public Nullable<long> InvId { get; set; }
        public string TransNum { get; set; }
        public decimal Cash { get; set; }
        public string Notes { get; set; }
        public Nullable<int> PosIdCreator { get; set; }
        public Nullable<byte> IsConfirm { get; set; }
        public Nullable<int> CashTransIdSource { get; set; }
        public string Side { get; set; }
        public string DocNum { get; set; }
        public Nullable<int> BankId { get; set; }
        public string ProcessType { get; set; }
        public Nullable<int> CardId { get; set; }
        public Nullable<int> ShippingCompanyId { get; set; }
        public Nullable<decimal> CommissionValue { get; set; }
        public Nullable<decimal> CommissionRatio { get; set; }
        public int IsCommissionPaid { get; set; }
        public Nullable<decimal> Paid { get; set; }
        public Nullable<decimal> Deserved { get; set; }
        public string Purpose { get; set; }
        public bool IsInvPurpose { get; set; }
        public string OtherSide { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<long> UpdateUserId { get; set; }
        public Nullable<long> CreateUserId { get; set; }

        //extra
        public string BankName { get; set; }
        public string AgentName { get; set; }
        public string PosName { get; set; }
        public string CreateUserName { get; set; }
        public string UpdateUserName { get; set; }
        public string CreateUserLName { get; set; }
        public string CardName { get; set; }
        public string InvNumber { get; set; }
        #endregion

        #region Methods
        public async Task<List<PayedInvclass>> PayedBycashlist(List<CashTransfer> cashList)
        {
            List<PayedInvclass> payedlist = new List<PayedInvclass>();
            try
            {
                List<Card> cards = new List<Card>();

                //fill cards
                if (FillCombo.cardsList is null)
                { await FillCombo.RefreshCards(); }
                cards = FillCombo.cardsList.ToList();


                cashList = cashList.Where(C => (C.ProcessType == "card" || C.ProcessType == "cash")).ToList();
                int i = 0;
                payedlist = cashList.GroupBy(x => x.CardId).Select(x => new PayedInvclass
                {
                    ProcessType = x.FirstOrDefault().ProcessType,

                    Cash = x.Sum(c => c.Cash),
                    CardId = x.FirstOrDefault().CardId,
                    CardName = x.FirstOrDefault().ProcessType == "card" ? cards.Where(c => c.CardId == x.FirstOrDefault().CardId).FirstOrDefault().Name : "cash",
                    Sequenc = x.FirstOrDefault().ProcessType == "cash" ? 0 : ++i,
                    CommissionRatio = x.FirstOrDefault().CommissionRatio,
                    CommissionValue = x.FirstOrDefault().CommissionValue,
                }).OrderBy(c => c.CardId).ToList();
                return payedlist;
            }
            catch
            {
                return payedlist;
            }
        }
        #endregion
    }

    public class PayedInvclass
    {
        public string ProcessType { get; set; }
        public decimal Cash { get; set; }
        public string CardName { get; set; }
        public int Sequenc { get; set; }
        public Nullable<int> CardId { get; set; }
        public Nullable<decimal> CommissionValue { get; set; }
        public Nullable<decimal> CommissionRatio { get; set; }
        public string DocNum { get; set; }
    }
}
