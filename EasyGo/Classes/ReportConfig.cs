using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EasyGo.Classes.ApiClasses;
using Newtonsoft.Json;
using System.Windows.Threading;
using System.Collections.Specialized;
using Microsoft.Win32;
using System.Xml;
using System.Windows.Controls;
using System.Resources;
using System.Reflection;

namespace EasyGo.Classes
{
    class ReportConfig
    {
        
        public static void setReportLanguage(List<ReportParameter> paramarr)
        {

            paramarr.Add(new ReportParameter("lang", AppSettings.Reportlang));

        }
        public static void setInvoiceLanguage(List<ReportParameter> paramarr)
        {

            paramarr.Add(new ReportParameter("lang", AppSettings.invoice_lang));

        }

        public static void Header(List<ReportParameter> paramarr)
        {

            ReportCls rep = new ReportCls();



            if (AppSettings.Reportlang == "ar")
            {

                paramarr.Add(new ReportParameter("companyName", AppSettings.com_name_ar));
                paramarr.Add(new ReportParameter("Address", AppSettings.com_address_ar));
            }
            else
            {
                paramarr.Add(new ReportParameter("companyName", AppSettings.companyName));
                paramarr.Add(new ReportParameter("Address", AppSettings.Address));
            }
            paramarr.Add(new ReportParameter("Fax", AppSettings.Fax));
            paramarr.Add(new ReportParameter("Tel", AppSettings.Mobile));

            paramarr.Add(new ReportParameter("Email", AppSettings.Email));
            paramarr.Add(new ReportParameter("logoImage", "file:\\" + rep.GetLogoImagePath()));
            paramarr.Add(new ReportParameter("show_header", AppSettings.show_header));
            //trans
            paramarr.Add(new ReportParameter("trcomAddress", AppSettings.resourcemanagerreport.GetString("trAddress")));
            paramarr.Add(new ReportParameter("trcomTel", AppSettings.resourcemanagerreport.GetString("tel")));
            paramarr.Add(new ReportParameter("trcomFax", AppSettings.resourcemanagerreport.GetString("fax")));
            paramarr.Add(new ReportParameter("trcomEmail", AppSettings.resourcemanagerreport.GetString("email")));
            paramarr.Add(new ReportParameter("trNoData", AppSettings.resourcemanagerreport.GetString("thereArenodata")));

        }
        public static void InvoiceHeader(List<ReportParameter> paramarr)
        {

            ReportCls rep = new ReportCls();
            // AppSettings.invoice_lang;
            if (AppSettings.invoice_lang == "en")
            {
                paramarr.Add(new ReportParameter("companyName", AppSettings.companyName));
                paramarr.Add(new ReportParameter("Address", AppSettings.Address));
            }
            else if (AppSettings.invoice_lang == "ar")
            {
                paramarr.Add(new ReportParameter("companyName", AppSettings.com_name_ar));
                paramarr.Add(new ReportParameter("Address", AppSettings.com_address_ar));
            }
            else
            {//both
                paramarr.Add(new ReportParameter("companyName", AppSettings.companyName));
                paramarr.Add(new ReportParameter("Address", AppSettings.Address));
                paramarr.Add(new ReportParameter("companyNameAr", AppSettings.com_name_ar));
                paramarr.Add(new ReportParameter("AddressAr", AppSettings.com_address_ar));
                //
                paramarr.Add(new ReportParameter("trcomAddressAr", AppSettings.resourcemanagerAr.GetString("trAddress")));
                paramarr.Add(new ReportParameter("trcomTelAr", AppSettings.resourcemanagerAr.GetString("tel")));
                paramarr.Add(new ReportParameter("trcomFaxAr", AppSettings.resourcemanagerAr.GetString("fax")));
                paramarr.Add(new ReportParameter("trcomEmailAr", AppSettings.resourcemanagerAr.GetString("email")));
            }
            paramarr.Add(new ReportParameter("trcomAddress", AppSettings.resourcemanagerreport.GetString("trAddress")));
            paramarr.Add(new ReportParameter("trcomTel", AppSettings.resourcemanagerreport.GetString("tel")));
            paramarr.Add(new ReportParameter("trcomFax", AppSettings.resourcemanagerreport.GetString("fax")));
            paramarr.Add(new ReportParameter("trcomEmail", AppSettings.resourcemanagerreport.GetString("email")));
            //
            paramarr.Add(new ReportParameter("Fax", AppSettings.Fax.Replace("--", "")));
            paramarr.Add(new ReportParameter("Tel", AppSettings.Phone.Replace("--", "")));

            paramarr.Add(new ReportParameter("Email", AppSettings.Email));
            paramarr.Add(new ReportParameter("logoImage", "file:\\" + rep.GetLogoImagePath()));
            paramarr.Add(new ReportParameter("show_header", AppSettings.show_header));

            //social
            string iconname = AppSettings.logoImage;//temp value
            paramarr.Add(new ReportParameter("com_tel_icon", "file:\\" + rep.GetIconImagePath("phone")));
            paramarr.Add(new ReportParameter("com_fax_icon", "file:\\" + rep.GetIconImagePath("fax")));
            paramarr.Add(new ReportParameter("com_social_icon", "file:\\" + rep.GetIconImagePath(AppSettings.com_social_icon)));
            paramarr.Add(new ReportParameter("com_social", AppSettings.com_social));
            paramarr.Add(new ReportParameter("com_website_icon", "file:\\" + rep.GetIconImagePath("website")));
            paramarr.Add(new ReportParameter("com_website", AppSettings.com_website));
            paramarr.Add(new ReportParameter("com_email_icon", "file:\\" + rep.GetIconImagePath("email")));

            paramarr.Add(new ReportParameter("com_mobile", AppSettings.Mobile.Replace("--", "")));
            paramarr.Add(new ReportParameter("com_mobile_icon", "file:\\" + rep.GetIconImagePath("mobile")));
        }
        public static void HeaderNoLogo(List<ReportParameter> paramarr)
        {

            ReportCls rep = new ReportCls();
            if (AppSettings.Reportlang == "ar")
            {

                paramarr.Add(new ReportParameter("companyName", AppSettings.com_name_ar));
                paramarr.Add(new ReportParameter("Address", AppSettings.com_address_ar));
            }
            else
            {
                paramarr.Add(new ReportParameter("companyName", AppSettings.companyName));
                paramarr.Add(new ReportParameter("Address", AppSettings.Address));
            }

            paramarr.Add(new ReportParameter("Fax", AppSettings.Fax));
            paramarr.Add(new ReportParameter("Tel", AppSettings.Mobile));

            paramarr.Add(new ReportParameter("Email", AppSettings.Email));
            paramarr.Add(new ReportParameter("show_header", AppSettings.show_header));

            paramarr.Add(new ReportParameter("trcomAddress", AppSettings.resourcemanagerreport.GetString("trAddress")));
            paramarr.Add(new ReportParameter("trcomTel", AppSettings.resourcemanagerreport.GetString("tel")));
            paramarr.Add(new ReportParameter("trcomFax", AppSettings.resourcemanagerreport.GetString("fax")));
            paramarr.Add(new ReportParameter("trcomEmail", AppSettings.resourcemanagerreport.GetString("email")));

        }
        //

        public static void bankdg(List<ReportParameter> paramarr)
        {


            paramarr.Add(new ReportParameter("trTransferNumber", AppSettings.resourcemanagerreport.GetString("trTransferNumberTooltip")));


        }
        public static void bondsDocReport(LocalReport rep, string reppath, List<ReportParameter> paramarr)
        {
            rep.ReportPath = reppath;
            rep.EnableExternalImages = true;
            rep.DataSources.Clear();

            DateFormConv(paramarr);

        }
        //public static void bondsReport(IEnumerable<Bonds> bondsQuery, LocalReport rep, string reppath, List<ReportParameter> paramarr)
        //{
        //    rep.ReportPath = reppath;
        //    rep.EnableExternalImages = true;
        //    rep.DataSources.Clear();

        //    paramarr.Add(new ReportParameter("trDocNumTooltip", AppSettings.resourcemanagerreport.GetString("trDocNumTooltip")));
        //    paramarr.Add(new ReportParameter("trRecipientTooltip", AppSettings.resourcemanagerreport.GetString("trRecipientTooltip")));

        //    paramarr.Add(new ReportParameter("trPaymentTypeTooltip", AppSettings.resourcemanagerreport.GetString("trPaymentTypeTooltip")));

        //    paramarr.Add(new ReportParameter("trDocDateTooltip", AppSettings.resourcemanagerreport.GetString("trDocDateTooltip")));

        //    paramarr.Add(new ReportParameter("trPayDate", AppSettings.resourcemanagerreport.GetString("trPayDate")));
        //    paramarr.Add(new ReportParameter("trCashTooltip", AppSettings.resourcemanagerreport.GetString("trCashTooltip")));

        //    foreach (var c in bondsQuery)
        //    {

        //        c.amount = decimal.Parse(HelpClass.DecTostring(c.amount));
        //    }
        //    rep.DataSources.Add(new ReportDataSource("DataSetBond", bondsQuery));

        //    DateFormConv(paramarr);
        //    AccountSideConv(paramarr);
        //    cashTransTypeConv(paramarr);

        //}


        //public static void orderReport(IEnumerable<Invoice> invoiceQuery, LocalReport rep, string reppath)
        //{
        //    rep.ReportPath = reppath;
        //    rep.EnableExternalImages = true;
        //    rep.DataSources.Clear();
        //    foreach(var o in invoiceQuery)
        //    {
        //        string status = "";
        //        switch (o.status)
        //        {
        //            case "tr":
        //                status = AppSettings.resourcemanager.GetString("trDelivered");
        //                break;
        //            case "rc":
        //                status = AppSettings.resourcemanager.GetString("trInDelivery");
        //                break;
        //            default:
        //                status = "";
        //                break;
        //        }
        //        o.status = status;
        //        o.deserved = decimal.Parse(HelpClass.DecTostring(o.deserved));
        //    }
        //    rep.DataSources.Add(new ReportDataSource("DataSetInvoice", invoiceQuery));
        //}
       

        public static string invoicePayStatusConvert(string payStatus)
        {

            switch (payStatus)
            {
                case "payed": return AppSettings.resourcemanagerreport.GetString("trPaid_");

                case "unpayed": return AppSettings.resourcemanagerreport.GetString("trCredit");

                case "partpayed": return AppSettings.resourcemanagerreport.GetString("trPartialPay");

                default: return "";

            }
        }
        public static void DeliverStateConv(List<ReportParameter> paramarr)
        {
            paramarr.Add(new ReportParameter("trDelivered", AppSettings.resourcemanagerreport.GetString("trDelivered")));
            paramarr.Add(new ReportParameter("trInDelivery", AppSettings.resourcemanagerreport.GetString("trInDelivery")));

        }
   
        public static string ConvertdateFormat(DateTime? value)
        {
            try
            {
                DateTime date;
                if (value is DateTime)
                    date = (DateTime)value;
                else return value.ToString();

                switch (AppSettings.dateFormat)
                {
                    case "ShortDatePattern":
                        return date.ToString(@"dd/MM/yyyy");
                    case "LongDatePattern":
                        return date.ToString(@"dddd, MMMM d, yyyy");
                    case "MonthDayPattern":
                        return date.ToString(@"MMMM dd");
                    case "YearMonthPattern":
                        return date.ToString(@"MMMM yyyy");
                    default:
                        return date.ToString(@"dd/MM/yyyy");
                }
            }
            catch
            {
                return "";
            }
        }
       

        public static string processTypeConvswitch(string processType, string cardName)
        {

            switch (processType)
            {
                case "cash": return AppSettings.resourcemanagerreport.GetString("trCash");
                //break;
                case "doc": return AppSettings.resourcemanagerreport.GetString("trDocument");
                //break;
                case "cheque": return AppSettings.resourcemanagerreport.GetString("trCheque");
                //break;
                case "balance": return AppSettings.resourcemanagerreport.GetString("trCredit");
                //break;
                case "card": return cardName;
                //break;
                case "inv": return AppSettings.resourcemanagerreport.GetString("trInv");
                case "admin": return AppSettings.resourcemanagerreport.GetString("trAdministrative");
                //break;
                default: return processType;
                    //break;
            }
        }
     
        public static string AgentCompanyUnKnownConvert(long? agentId, string side, string agentCompany)
        {
            if (agentId == null || agentId == 0)
            {
                agentCompany = AppSettings.resourcemanagerreport.GetString("trUnKnown");
                agentCompany = "";
            }
            return agentCompany;
        }


       
      
        public static string subscriptionTypeConverter(string subscriptionType)
        {

            switch (subscriptionType)
            {
                case "f": return AppSettings.resourcemanagerreport.GetString("trFree");

                case "m": return AppSettings.resourcemanagerreport.GetString("trMonthly");

                case "y": return AppSettings.resourcemanagerreport.GetString("trYearly");

                case "o": return AppSettings.resourcemanagerreport.GetString("trOnce");

                default: return AppSettings.resourcemanagerreport.GetString("");

            }
        }

        public static string unlimitedEndDateConverter(string subscriptionType, DateTime? EndDate)
        {
            if (subscriptionType != null && EndDate != null)
            {
                string sType = subscriptionType;
                DateTime sDate = (DateTime)EndDate;

                if (sType == "o" || sType == "f")
                    return AppSettings.resourcemanager.GetString("trUnlimited");
                else
                {


                    switch (AppSettings.dateFormat)
                    {
                        case "ShortDatePattern":
                            return sDate.ToString(@"dd/MM/yyyy");
                        case "LongDatePattern":
                            return sDate.ToString(@"dddd, MMMM d, yyyy");
                        case "MonthDayPattern":
                            return sDate.ToString(@"MMMM dd");
                        case "YearMonthPattern":
                            return sDate.ToString(@"MMMM yyyy");
                        default:
                            return sDate.ToString(@"dd/MM/yyyy");
                    }

                }
            }
            else if (subscriptionType == "o" || subscriptionType == "f")
            {
                return AppSettings.resourcemanager.GetString("trUnlimited");
            }
            else return "";
        }
       
        public string posTransfersStatusConverter(byte isConfirm1, byte isConfirm2)
        {

            if ((isConfirm1 == 1) && (isConfirm2 == 1))
                return AppSettings.resourcemanager.GetString("trConfirmed");
            else if ((isConfirm1 == 2) || (isConfirm2 == 2))
                return AppSettings.resourcemanager.GetString("trCanceled");
            else
                return AppSettings.resourcemanager.GetString("trWaiting");
        }



       
        
       
        public static string ExceedConv(string isExceed)
        {
            switch (isExceed)
            {
                // used in reservation update to know if reservation exceed warning Time For Late
                case "exceed":
                    isExceed = AppSettings.resourcemanagerreport.GetString("trExceed");
                    break;

                case "":
                    isExceed = "-";
                    break;
            }
            return isExceed;
        }
        // timeFrameConverter
        public static string timeFrameConv(DateTime date)
        {


            //    DateTimeFormatInfo dtfi = DateTimeFormatInfo.CurrentInfo;

            if (!(date is DateTime))
                return date.ToString();


            switch (AppSettings.timeFormat)
            {
                case "ShortTimePattern":
                    return date.ToShortTimeString();
                case "LongTimePattern":
                    return date.ToLongTimeString();
                default:
                    return date.ToShortTimeString();
            }

        }
       

       
        public static string dateTimeToTimeConvert(DateTime? orderTime)
        {
            if (orderTime != null)
            {
                DateTime dt = (DateTime)orderTime;
                return dt.ToShortTimeString();
            }
            else
                return "-";
        }
        public static string driverOrShipcompanyConvert(int isDriver, string shipUserName, string shipUserLastName, string shippingCompanyName)
        {
            string name = "";
            if (isDriver == 1)
            {
                name = shipUserName + " " + shipUserLastName;
            }
            else
            {
                name = shippingCompanyName;
            }

            return name;
        }

        
        public static string agentResSectorsAddressConv(string agentResSectorsName, string agentAddress)
        {
            if (agentResSectorsName == "" && agentAddress == "")
            {
                agentAddress = "-";

            }
            else if ((agentResSectorsName == "" || agentAddress == ""))
            {
                agentAddress = agentResSectorsName + agentAddress;
            }
            else
            {
                agentAddress = agentResSectorsName + "-" + agentAddress;
            }
            return agentAddress;
        }
       
        public static string DeliveryTypeConvert(string deliveryType)
        {

            switch (deliveryType)
            {
                case "local": return AppSettings.resourcemanagerreport.GetString("trLocaly");
                //break;
                case "com": return AppSettings.resourcemanagerreport.GetString("trShippingCompany");
                //break;
                default: return AppSettings.resourcemanagerreport.GetString("");
                    //break;
            }
        }
        public static string deliveryConverter(string shippingCompanyName, string shipUserName, string shipUserLastName)
        {

            try
            {
                if (shippingCompanyName == null || shippingCompanyName == "")
                    return "-";
                else
                {
                    if (shipUserName != null && shipUserName != "")
                        return shipUserName + " " + shipUserLastName;
                    else
                        return shippingCompanyName;
                }
            }
            catch
            {
                return "";
            }
        }
        public static string preparingOrderStatusConvert(string status)
        {
            switch (status)
            {
                case "Listed": return AppSettings.resourcemanagerreport.GetString("trListed");
                case "Preparing": return AppSettings.resourcemanagerreport.GetString("trPreparing");
                case "Ready": return AppSettings.resourcemanagerreport.GetString("trReady");
                case "Collected": return AppSettings.resourcemanagerreport.GetString("withDeliveryMan");
                case "InTheWay": return AppSettings.resourcemanagerreport.GetString("onTheWay");
                case "Done": return AppSettings.resourcemanagerreport.GetString("trDone");// gived to customer
                default: return "";
            }
        }
     
        public static void PosReport(IEnumerable<Pos> Query, LocalReport rep, string reppath, List<ReportParameter> paramarr)
        {
            rep.ReportPath = reppath;
            rep.EnableExternalImages = true;
            rep.DataSources.Clear();
            rep.DataSources.Add(new ReportDataSource("DataSetPos", Query));
            //title
            paramarr.Add(new ReportParameter("trTitle", AppSettings.resourcemanagerreport.GetString("trPOSs")));
            //table columns
            paramarr.Add(new ReportParameter("trPosCode", AppSettings.resourcemanagerreport.GetString("trPosCode")));
            paramarr.Add(new ReportParameter("trPosName", AppSettings.resourcemanagerreport.GetString("trPosName")));
            paramarr.Add(new ReportParameter("trBranchName", AppSettings.resourcemanagerreport.GetString("trBranchName")));
            paramarr.Add(new ReportParameter("trNote", AppSettings.resourcemanagerreport.GetString("trNote")));
            paramarr.Add(new ReportParameter("trNo", AppSettings.resourcemanagerreport.GetString("trNo.")));

        }
      
       
        public static string CategoryConv(string categoryName)
        {
            if (categoryName == "appetizers")
            {
                categoryName = AppSettings.resourcemanagerreport.GetString("trAppetizers");
            }
            else if (categoryName == "beverages")
            {
                categoryName = AppSettings.resourcemanagerreport.GetString("trBeverages");
            }
            else if (categoryName == "fastFood")
            {
                categoryName = AppSettings.resourcemanagerreport.GetString("trFastFood");
            }
            else if (categoryName == "mainCourses")
            {
                categoryName = AppSettings.resourcemanagerreport.GetString("trMainCourses"); ;
            }
            else if (categoryName == "desserts")
            {
                categoryName = AppSettings.resourcemanagerreport.GetString("trDesserts");
            }
            else if (categoryName == "package")
            {
                categoryName = AppSettings.resourcemanagerreport.GetString("trPackages");
            }
            else if (categoryName == "RawMaterials")
            {
                categoryName = AppSettings.resourcemanagerreport.GetString("trRawMaterials");
            }
            else if (categoryName == "Vegetables")
            {
                categoryName = AppSettings.resourcemanagerreport.GetString("trVegetables");
            }
            else if (categoryName == "Meat")
            {
                categoryName = AppSettings.resourcemanagerreport.GetString("trMeat");
            }
            else if (categoryName == "Drinks")
            {
                categoryName = AppSettings.resourcemanagerreport.GetString("trDrinks");
            }
            else if (categoryName == "extraOrders")
            {
                categoryName = AppSettings.resourcemanagerreport.GetString("extraOrders");
            }

            return categoryName;
        }
      
       
        public static string forAgentsConverters(string forAgents)
        {
            try
            {
                if (forAgents != null)
                {

                    switch (forAgents)
                    {
                        case "pb": return AppSettings.resourcemanagerreport.GetString("public");

                        case "pr": return AppSettings.resourcemanagerreport.GetString("private");

                        default: return "";
                    }
                }
                else
                    return "";
            }
            catch
            {
                return "";
            }
        }
      
        public string unlimitedCouponConv(decimal quantity)
        {

            if (quantity == 0)
                return AppSettings.resourcemanagerreport.GetString("trUnlimited");
            else
                return quantity.ToString();
        }
        public static void couponExportReport(LocalReport rep, string reppath, List<ReportParameter> paramarr, string barcode)
        {

            rep.ReportPath = reppath;
            rep.EnableExternalImages = true;
            rep.DataSources.Clear();

            ReportCls repcls = new ReportCls();


            paramarr.Add(new ReportParameter("invNumber", barcode));
            paramarr.Add(new ReportParameter("barcodeimage", "file:\\" + repcls.BarcodeToImage(barcode, "barcode")));

        }

     
        public static string BranchStoreConverter(string type)
        {
            string s = "";
            switch (type)
            {
                case "b": s = AppSettings.resourcemanagerreport.GetString("tr_Branch"); break;
                case "s": s = AppSettings.resourcemanagerreport.GetString("tr_Store"); break;

            }

            return s;
        }
       
      
        public static void itemTransferDiscountTypeConv(List<ReportParameter> paramarr)
        {

            paramarr.Add(new ReportParameter("trValueDiscount", AppSettings.resourcemanagerreport.GetString("trValueDiscount")));
            paramarr.Add(new ReportParameter("trPercentageDiscount", AppSettings.resourcemanagerreport.GetString("trPercentageDiscount")));




        }

        public static void itemTypeConv(List<ReportParameter> paramarr)
        {
            paramarr.Add(new ReportParameter("trNormal", AppSettings.resourcemanagerreport.GetString("trNormal")));
            paramarr.Add(new ReportParameter("trHaveExpirationDate", AppSettings.resourcemanagerreport.GetString("trHaveExpirationDate")));
            paramarr.Add(new ReportParameter("trHaveSerialNumber", AppSettings.resourcemanagerreport.GetString("trHaveSerialNumber")));
            paramarr.Add(new ReportParameter("trService", AppSettings.resourcemanagerreport.GetString("trService")));
            paramarr.Add(new ReportParameter("trPackage", AppSettings.resourcemanagerreport.GetString("trPackage")));
        }
       
     
        public static string InvoiceTypeConv(string invType)
        {
            switch (invType)
            {
                //مبيعات
                case "s":
                    invType = AppSettings.resourcemanagerreport.GetString("trDiningHallType");
                    break;
                // طلب خارجي
                case "ts":
                    invType = AppSettings.resourcemanagerreport.GetString("trTakeAway");
                    break;
                // خدمة ذاتية
                case "ss":
                    invType = AppSettings.resourcemanagerreport.GetString("trSelfService");
                    break;

                //مشتريات 
                case "p":
                    invType = AppSettings.resourcemanagerreport.GetString("trPurchaseInvoice");
                    break;
                //فاتورة مشتريات بانتظار الادخال
                case "pw":
                    invType = AppSettings.resourcemanagerreport.GetString("trPurchaseInvoiceWaiting");
                    break;

                //مرتجع مبيعات
                case "sb":
                    invType = AppSettings.resourcemanagerreport.GetString("trSalesReturnInvoice");
                    break;
                //مرتجع مشتريات
                case "pb":
                    invType = AppSettings.resourcemanagerreport.GetString("trPurchaseReturnInvoice");
                    break;
                //فاتورة مرتجع مشتريات بانتظار الاخراج
                case "pbw":
                    invType = AppSettings.resourcemanagerreport.GetString("trPurchaseReturnInvoiceWaiting");
                    break;
                //مسودة مشتريات 
                case "pd":
                    invType = AppSettings.resourcemanagerreport.GetString("trDraftPurchaseBill");
                    break;
                //مسودة مبيعات
                case "sd":
                    invType = AppSettings.resourcemanagerreport.GetString("DiningHallInvoiceDraft");
                    // invType = AppSettings.resourcemanagerreport.GetString("trSalesDraft");

                    break;
                //مسودة مرتجع مبيعات
                case "sbd":
                    invType = AppSettings.resourcemanagerreport.GetString("trSalesReturnDraft");
                    break;
                //مسودة مرتجع مشتريات
                case "pbd":
                    invType = AppSettings.resourcemanagerreport.GetString("trPurchaseReturnDraft");
                    break;
                // مسودة طلبية مبيعا 
                case "ord":
                    invType = AppSettings.resourcemanagerreport.GetString("trDraft");
                    break;
                //طلبية مبيعات 
                case "or":
                    invType = AppSettings.resourcemanagerreport.GetString("trSaleOrder");
                    break;
                //مسودة طلبية شراء 
                case "pod":
                    invType = AppSettings.resourcemanagerreport.GetString("trDraft");
                    break;
                //طلبية شراء 
                case "po":
                    invType = AppSettings.resourcemanagerreport.GetString("trPurchaceOrder");
                    break;
                // طلبية شراء أو بيع محفوظة
                case "pos":
                case "ors":
                    invType = AppSettings.resourcemanagerreport.GetString("trSaved");
                    break;
                //مسودة عرض 
                case "qd":
                    invType = AppSettings.resourcemanagerreport.GetString("trQuotationsDraft");
                    break;
                //عرض سعر محفوظ
                case "qs":
                    invType = AppSettings.resourcemanagerreport.GetString("trSaved");
                    break;
                //فاتورة عرض اسعار
                case "q":
                    invType = AppSettings.resourcemanagerreport.GetString("trQuotations");
                    break;
                //الإتلاف
                case "d":
                    invType = AppSettings.resourcemanagerreport.GetString("trDestructive");
                    break;
                //النواقص
                case "sh":
                    invType = AppSettings.resourcemanagerreport.GetString("trShortage");
                    break;
                //مسودة  استراد
                case "imd":
                    invType = AppSettings.resourcemanagerreport.GetString("trImportDraft");
                    break;
                // استراد
                case "im":
                    invType = AppSettings.resourcemanagerreport.GetString("trImport");
                    break;
                // طلب استيراد
                case "imw":
                    invType = AppSettings.resourcemanagerreport.GetString("trImportOrder");
                    break;
                //مسودة تصدير
                case "exd":
                    invType = AppSettings.resourcemanagerreport.GetString("trExportDraft");
                    break;
                // تصدير
                case "ex":
                    invType = AppSettings.resourcemanagerreport.GetString("trExport");
                    break;
                // طلب تصدير
                case "exw":
                    invType = AppSettings.resourcemanagerreport.GetString("trExportOrder");
                    break;
                // إدخال مباشر
                case "is":
                    invType = AppSettings.resourcemanagerreport.GetString("trDirectEntry");
                    break;
                // مسودة إدخال مباشر
                case "isd":
                    invType = AppSettings.resourcemanagerreport.GetString("trDirectEntryDraft");
                    break;
                // مسودة طلب خارجي
                case "tsd":
                    invType = AppSettings.resourcemanagerreport.GetString("trTakeAwayDraft");
                    break;


                // خدمة ذاتية مسودة
                case "ssd":
                    invType = AppSettings.resourcemanagerreport.GetString("trSelfServiceDraft");
                    break;
                // فاتورة استهلاك
                case "fbc":
                    invType = AppSettings.resourcemanagerreport.GetString("consumptionInvoice");
                    break;
                // مسودة طلب صرف
                case "srd":
                    invType = AppSettings.resourcemanagerreport.GetString("trSpendingRequestDraft");
                    break;
                //  طلب صرف
                case "sr":
                    invType = AppSettings.resourcemanagerreport.GetString("trSpendingRequest");
                    break;
                // مرتجع طلب صرف 
                case "srb":
                    invType = AppSettings.resourcemanagerreport.GetString("trSpendingRequestReturn");
                    break;
                //  طلب صرف في الانتظار 
                case "srw":
                    invType = AppSettings.resourcemanagerreport.GetString("trSpendingOrderWait");
                    break;
                default: break;
            }
            return invType;
        }
       
        public static string ProfitDescriptionConvert(string invNumber, string side)
        {
            string description = "";
            if (!string.IsNullOrEmpty(invNumber))
                description = AppSettings.resourcemanagerreport.GetString("trProfitInvoice");
            else
            {
                switch (side.ToString())
                {
                    case "bnd": break;
                    case "v": description = AppSettings.resourcemanagerreport.GetString("trVendor"); break;
                    case "c": description = AppSettings.resourcemanagerreport.GetString("trCustomer"); break;
                    case "u": description = AppSettings.resourcemanagerreport.GetString("trUser"); break;
                    case "s": description = AppSettings.resourcemanagerreport.GetString("trSalary"); break;
                    case "e": description = AppSettings.resourcemanagerreport.GetString("trGeneralExpenses"); break;
                    case "m": description = AppSettings.resourcemanagerreport.GetString("trAdministrativePull"); break;
                    case "sh": description = AppSettings.resourcemanagerreport.GetString("trShippingCompany"); break;
                    case "tax": description = AppSettings.resourcemanagerreport.GetString("trTaxCollection"); break;
                    default: break;
                }
                description = AppSettings.resourcemanagerreport.GetString("trPayment") + "-" + description;
            }
            return description;
        }
        //public static void ProfitReport(IEnumerable<ItemUnitInvoiceProfit> tempquery, LocalReport rep, string reppath, List<ReportParameter> paramarr)
        //{
        //    rep.ReportPath = reppath;
        //    rep.EnableExternalImages = true;
        //    rep.DataSources.Clear();
        //    foreach (var r in tempquery)
        //    {

        //        r.totalNet = decimal.Parse(HelpClass.DecTostring(r.totalNet));
        //        r.invoiceProfit = decimal.Parse(HelpClass.DecTostring(r.invoiceProfit));
        //        r.itemProfit = decimal.Parse(HelpClass.DecTostring(r.itemProfit));
        //        r.itemunitProfit = decimal.Parse(HelpClass.DecTostring(r.itemunitProfit));
        //    }
        //    rep.DataSources.Add(new ReportDataSource("DataSetProfit", tempquery));
        //    paramarr.Add(new ReportParameter("title", AppSettings.resourcemanagerreport.GetString("trProfits")));
        //    paramarr.Add(new ReportParameter("Currency", AppSettings.Currency));
        //    itemTransferInvTypeConv(paramarr);
        //    paramarr.Add(new ReportParameter("trNo", AppSettings.resourcemanagerreport.GetString("trNo.")));
        //    paramarr.Add(new ReportParameter("trType", AppSettings.resourcemanagerreport.GetString("trType")));
        //    paramarr.Add(new ReportParameter("trDate", AppSettings.resourcemanagerreport.GetString("trDate")));
        //    paramarr.Add(new ReportParameter("trTotal", AppSettings.resourcemanagerreport.GetString("trTotal")));
        //    paramarr.Add(new ReportParameter("trItem", AppSettings.resourcemanagerreport.GetString("trItem")));
        //    paramarr.Add(new ReportParameter("trUnit", AppSettings.resourcemanagerreport.GetString("trUnit")));
        //    paramarr.Add(new ReportParameter("trQTR", AppSettings.resourcemanagerreport.GetString("trQTR")));
        //    paramarr.Add(new ReportParameter("trBranch", AppSettings.resourcemanagerreport.GetString("trBranch")));
        //    paramarr.Add(new ReportParameter("trPOS", AppSettings.resourcemanagerreport.GetString("trPOS")));
        //    paramarr.Add(new ReportParameter("trProfits", AppSettings.resourcemanagerreport.GetString("trProfits")));

        //}
        
        public static string ReportTabTitle(string firstTitle, string secondTitle)
        {
            string trtext = "";
            //////////////////////////////////////////////////////////////////////////////
            if (firstTitle == "invoice")
                firstTitle = AppSettings.resourcemanagerreport.GetString("trInvoices");
            else if (firstTitle == "quotation")
                firstTitle = AppSettings.resourcemanagerreport.GetString("trQuotations");
            else if (firstTitle == "promotion")
                firstTitle = AppSettings.resourcemanagerreport.GetString("trThePromotion");
            else if (firstTitle == "internal")
                firstTitle = AppSettings.resourcemanagerreport.GetString("trInternal");
            else if (firstTitle == "external")
                firstTitle = AppSettings.resourcemanagerreport.GetString("trExternal");
            else if (firstTitle == "banksReport")
                firstTitle = AppSettings.resourcemanagerreport.GetString("trBanks");
            else if (firstTitle == "destroied")
                firstTitle = AppSettings.resourcemanagerreport.GetString("trDestructives");
            else if (firstTitle == "usersReport")
                firstTitle = AppSettings.resourcemanagerreport.GetString("trUsers");
            else if (firstTitle == "storageReports")
                firstTitle = AppSettings.resourcemanagerreport.GetString("trStorage");
            else if (firstTitle == "stocktaking")
                firstTitle = AppSettings.resourcemanagerreport.GetString("trStocktaking");
            else if (firstTitle == "stock")
                firstTitle = AppSettings.resourcemanagerreport.GetString("trStock");
            else if (firstTitle == "purchaseOrders")
                firstTitle = AppSettings.resourcemanagerreport.GetString("trPurchaseOrders");
            else if (firstTitle == "saleOrders")
                firstTitle = AppSettings.resourcemanagerreport.GetString("trSalesOrders");

            else if (firstTitle == "saleItems" || firstTitle == "purchaseItem")
                firstTitle = AppSettings.resourcemanagerreport.GetString("trItems");
            else if (firstTitle == "recipientReport")
                firstTitle = AppSettings.resourcemanagerreport.GetString("trReceived");
            else if (firstTitle == "accountStatement")
                firstTitle = AppSettings.resourcemanagerreport.GetString("trAccountStatement");
            else if (firstTitle == "paymentsReport")
                firstTitle = AppSettings.resourcemanagerreport.GetString("trPayments");
            else if (firstTitle == "posReports")
                firstTitle = AppSettings.resourcemanagerreport.GetString("trPOS");
            else if (firstTitle == "dailySalesStatistic")
                firstTitle = AppSettings.resourcemanagerreport.GetString("trDailySales");
            else if (firstTitle == "accountProfits")
                firstTitle = AppSettings.resourcemanagerreport.GetString("trProfits");
            else if (firstTitle == "accountFund")
                firstTitle = AppSettings.resourcemanagerreport.GetString("trCashBalance");
            else if (firstTitle == "quotations")
                firstTitle = AppSettings.resourcemanagerreport.GetString("trQTReport");
            else if (firstTitle == "transfers")
                firstTitle = AppSettings.resourcemanagerreport.GetString("trTransfers");
            else if (firstTitle == "fund")
                firstTitle = AppSettings.resourcemanagerreport.GetString("trCashBalance");
            else if (firstTitle == "DirectEntry")
                firstTitle = AppSettings.resourcemanagerreport.GetString("trDirectEntry");
            else if (firstTitle == "tax")
                firstTitle = AppSettings.resourcemanagerreport.GetString("trTax");
            else if (firstTitle == "closing")
                firstTitle = AppSettings.resourcemanagerreport.GetString("trDailyClosing");
            else if (firstTitle == "orders")
                firstTitle = AppSettings.resourcemanagerreport.GetString("orderReport");
            else if (firstTitle == "PreparingOrders")
                firstTitle = AppSettings.resourcemanagerreport.GetString("trPreparingOrders");
            else if (firstTitle == "SpendingRequests")
                firstTitle = AppSettings.resourcemanagerreport.GetString("trSpendingRequests");
            else if (firstTitle == "Consumption")
                firstTitle = AppSettings.resourcemanagerreport.GetString("trConsumption");
            else if (firstTitle == "membership")
                firstTitle = AppSettings.resourcemanagerreport.GetString("membership");
            //trCashBalance trDirectEntry
            //membership 
            //////////////////////////////////////////////////////////////////////////////

            if (secondTitle == "branch")
                secondTitle = AppSettings.resourcemanagerreport.GetString("trBranches");
            else if (secondTitle == "pos")
                secondTitle = AppSettings.resourcemanagerreport.GetString("trPOS");
            else if (secondTitle == "vendors" || secondTitle == "vendor")
                secondTitle = AppSettings.resourcemanagerreport.GetString("trVendors");
            else if (secondTitle == "customers" || secondTitle == "customer")
                secondTitle = AppSettings.resourcemanagerreport.GetString("trCustomers");
            else if (secondTitle == "users" || secondTitle == "user")
                secondTitle = AppSettings.resourcemanagerreport.GetString("trUsers");
            else if (secondTitle == "items" || secondTitle == "item")
                secondTitle = AppSettings.resourcemanagerreport.GetString("trItems");
            else if (secondTitle == "coupon")
                secondTitle = AppSettings.resourcemanagerreport.GetString("trCoupons");
            else if (secondTitle == "offers")
                secondTitle = AppSettings.resourcemanagerreport.GetString("trOffers");
            else if (secondTitle == "invoice")
                secondTitle = AppSettings.resourcemanagerreport.GetString("trInvoices");
            else if (secondTitle == "order")
                secondTitle = AppSettings.resourcemanagerreport.GetString("trOrders");
            else if (secondTitle == "quotation")
                secondTitle = AppSettings.resourcemanagerreport.GetString("trQTReport");
            else if (secondTitle == "operator")
                secondTitle = AppSettings.resourcemanagerreport.GetString("trOperator");
            else if (secondTitle == "operations")
                secondTitle = AppSettings.resourcemanagerreport.GetString("trOperations");//trOperations
            else if (secondTitle == "payments")
                secondTitle = AppSettings.resourcemanagerreport.GetString("trPayments");
            else if (secondTitle == "recipient")
                secondTitle = AppSettings.resourcemanagerreport.GetString("trReceived");
            else if (secondTitle == "destroied")
                secondTitle = AppSettings.resourcemanagerreport.GetString("trDestructives");
            else if (secondTitle == "agent")
                secondTitle = AppSettings.resourcemanagerreport.GetString("trCustomers");
            else if (secondTitle == "stock")
                secondTitle = AppSettings.resourcemanagerreport.GetString("trStock");
            else if (secondTitle == "external")
                secondTitle = AppSettings.resourcemanagerreport.GetString("trExternal");
            else if (secondTitle == "internal")
                secondTitle = AppSettings.resourcemanagerreport.GetString("trInternal");
            else if (secondTitle == "stocktaking")
                secondTitle = AppSettings.resourcemanagerreport.GetString("trStocktaking");
            else if (secondTitle == "archives")
                secondTitle = AppSettings.resourcemanagerreport.GetString("trArchive");
            else if (secondTitle == "shortfalls")
                secondTitle = AppSettings.resourcemanagerreport.GetString("trShortages");
            else if (secondTitle == "location")
                secondTitle = AppSettings.resourcemanagerreport.GetString("trLocation");
            else if (secondTitle == "locations")
                secondTitle = AppSettings.resourcemanagerreport.GetString("trLocations");
            else if (secondTitle == "collect")
                secondTitle = AppSettings.resourcemanagerreport.GetString("trCollect");
            else if (secondTitle == "shipping")
                secondTitle = AppSettings.resourcemanagerreport.GetString("trShipping");
            else if (secondTitle == "salary")
                secondTitle = AppSettings.resourcemanagerreport.GetString("trSalary");
            else if (secondTitle == "generalExpenses")
                secondTitle = AppSettings.resourcemanagerreport.GetString("trGeneralExpenses");
            else if (secondTitle == "administrativePull")
                secondTitle = AppSettings.resourcemanagerreport.GetString("trAdministrativePull");
            else if (secondTitle == "AdministrativeDeposit")
                secondTitle = AppSettings.resourcemanagerreport.GetString("trAdministrativeDeposit");
            else if (secondTitle == "BestSeller")
                secondTitle = AppSettings.resourcemanagerreport.GetString("trBestSeller");
            else if (secondTitle == "MostPurchased")
                secondTitle = AppSettings.resourcemanagerreport.GetString("trMostPurchased");
            else if (secondTitle == "pull")
                secondTitle = AppSettings.resourcemanagerreport.GetString("trPull");
            else if (secondTitle == "deposit")
                secondTitle = AppSettings.resourcemanagerreport.GetString("trDeposit");
            else if (secondTitle == "discounts")
                secondTitle = AppSettings.resourcemanagerreport.GetString("discounts");
            else if (secondTitle == "invClasses")
                secondTitle = AppSettings.resourcemanagerreport.GetString("trInvoicesClasses");
            else if (secondTitle == "memberships")
                secondTitle = AppSettings.resourcemanagerreport.GetString("trMemberships");
            //
            else if (secondTitle == "delivered")
                secondTitle = AppSettings.resourcemanagerreport.GetString("trDelivered");
            else if (secondTitle == "indelivery")
                secondTitle = AppSettings.resourcemanagerreport.GetString("trInDelivery");
            else if (secondTitle == "taxCollection")
                secondTitle = AppSettings.resourcemanagerreport.GetString("trTaxCollection");
            else if (secondTitle == "netProfit")
                secondTitle = AppSettings.resourcemanagerreport.GetString("trNetProfit");
            else if (secondTitle == "tragent")
                secondTitle = AppSettings.resourcemanagerreport.GetString("trAgents");
            else if (secondTitle == "trExpired")
                secondTitle = AppSettings.resourcemanagerreport.GetString("trExpired");
            else if (secondTitle == "done")
                secondTitle = AppSettings.resourcemanagerreport.GetString("trDone");
            //memberships
            //////////////////////////////////////////////////////////////////////////////
            if (firstTitle == "" && secondTitle != "")
            {
                trtext = secondTitle;
            }
            else if (secondTitle == "" && firstTitle != "")
            {
                trtext = firstTitle;
            }
            else
            {
                trtext = firstTitle + " / " + secondTitle;
            }

            return trtext;
        }
       
        public static string PaymentConvert(string processType, string cardName)
        {
            string s = "";
            switch (processType)
            {
                case "cash":
                    s = AppSettings.resourcemanagerreport.GetString("trCash");
                    break;
                case "doc":
                    s = AppSettings.resourcemanagerreport.GetString("trDocument");
                    break;
                case "cheque":
                    s = AppSettings.resourcemanagerreport.GetString("trCheque");
                    break;
                case "balance":
                    s = AppSettings.resourcemanagerreport.GetString("trCredit");
                    break;
                case "card":
                    s = cardName;
                    break;
                case "inv":
                    s = AppSettings.resourcemanagerreport.GetString("trInv");
                    break;
                default:
                    s = processType;
                    break;


            }
            return s;
        }
        public static void posReport(IEnumerable<Pos> possQuery, LocalReport rep, string reppath)
        {
            rep.ReportPath = reppath;
            rep.EnableExternalImages = true;
            rep.DataSources.Clear();
            rep.DataSources.Add(new ReportDataSource("DataSetPos", possQuery));
        }

        
        public static void userReport(IEnumerable<User> usersQuery, LocalReport rep, string reppath)
        {
            rep.ReportPath = reppath;
            rep.EnableExternalImages = true;
            rep.DataSources.Clear();
            rep.DataSources.Add(new ReportDataSource("DataSetUser", usersQuery));
        }

        public static void UserReport(IEnumerable<User> Query1, LocalReport rep, string reppath, List<ReportParameter> paramarr)
        {
            List<User> Query = JsonConvert.DeserializeObject<List<User>>(JsonConvert.SerializeObject(Query1));

            rep.ReportPath = reppath;
            rep.EnableExternalImages = true;
            rep.DataSources.Clear();

            //title
            paramarr.Add(new ReportParameter("trTitle", AppSettings.resourcemanagerreport.GetString("trUsers")));
            //table columns
            paramarr.Add(new ReportParameter("trName", AppSettings.resourcemanagerreport.GetString("trName")));
            paramarr.Add(new ReportParameter("trMobile", AppSettings.resourcemanagerreport.GetString("trMobile")));
            paramarr.Add(new ReportParameter("trEmail", AppSettings.resourcemanagerreport.GetString("trEmail")));
            paramarr.Add(new ReportParameter("trNote", AppSettings.resourcemanagerreport.GetString("trNote")));
            paramarr.Add(new ReportParameter("trNo", AppSettings.resourcemanagerreport.GetString("trNo.")));


            rep.DataSources.Add(new ReportDataSource("DataSetUser", Query));
        }

        public static string ConvertInvType(string invType)
        {
            string value = "";
            value = invType;

            try
            {

                switch (value)
                {
                    //مشتريات 
                    case "p":
                        value = AppSettings.resourcemanagerreport.GetString("trPurchaseInvoice");
                        break;
                    //فاتورة مشتريات بانتظار الادخال
                    case "pw":
                        value = AppSettings.resourcemanagerreport.GetString("trPurchaseInvoiceWaiting");
                        break;
                    //مبيعات
                    case "s":
                        value = AppSettings.resourcemanagerreport.GetString("DiningHallInvoice");
                        //   value = AppSettings.resourcemanagerreport.GetString("trSalesInvoice");
                        break;
                    //مرتجع مبيعات
                    case "sb":
                        value = AppSettings.resourcemanagerreport.GetString("trSalesReturnInvoice");
                        break;
                    //مرتجع مشتريات
                    case "pb":
                        value = AppSettings.resourcemanagerreport.GetString("trPurchaseReturnInvoice");
                        break;
                    //فاتورة مرتجع مشتريات بانتظار الاخراج
                    case "pbw":
                        value = AppSettings.resourcemanagerreport.GetString("trPurchaseReturnInvoiceWaiting");
                        break;
                    //مسودة مشتريات 
                    case "pd":
                        value = AppSettings.resourcemanagerreport.GetString("trDraftPurchaseBill");
                        break;
                    //مسودة مبيعات
                    case "sd":
                        value = AppSettings.resourcemanagerreport.GetString("DiningHallInvoiceDraft");
                        // value = AppSettings.resourcemanagerreport.GetString("trSalesDraft");

                        break;
                    //مسودة مرتجع مبيعات
                    case "sbd":
                        value = AppSettings.resourcemanagerreport.GetString("trSalesReturnDraft");
                        break;
                    //مسودة مرتجع مشتريات
                    case "pbd":
                        value = AppSettings.resourcemanagerreport.GetString("trPurchaseReturnDraft");
                        break;
                    // مسودة طلبية مبيعا 
                    case "ord":
                        value = AppSettings.resourcemanagerreport.GetString("trDraft");
                        break;
                    //طلبية مبيعات 
                    case "or":
                        value = AppSettings.resourcemanagerreport.GetString("trSaleOrder");
                        break;
                    //مسودة طلبية شراء 
                    case "pod":
                        value = AppSettings.resourcemanagerreport.GetString("trDraft");
                        break;
                    //طلبية شراء 
                    case "po":
                        value = AppSettings.resourcemanagerreport.GetString("trPurchaceOrder");
                        break;
                    // طلبية شراء أو بيع محفوظة
                    case "pos":
                    case "ors":
                        value = AppSettings.resourcemanagerreport.GetString("trSaved");
                        break;
                    //مسودة عرض 
                    case "qd":
                        value = AppSettings.resourcemanagerreport.GetString("trQuotationsDraft");
                        break;
                    //عرض سعر محفوظ
                    case "qs":
                        value = AppSettings.resourcemanagerreport.GetString("trSaved");
                        break;
                    //فاتورة عرض اسعار
                    case "q":
                        value = AppSettings.resourcemanagerreport.GetString("trQuotations");
                        break;
                    //الإتلاف
                    case "d":
                        value = AppSettings.resourcemanagerreport.GetString("trDestructive");
                        break;
                    //النواقص
                    case "sh":
                        value = AppSettings.resourcemanagerreport.GetString("trShortage");
                        break;
                    //مسودة  استراد
                    case "imd":
                        value = AppSettings.resourcemanagerreport.GetString("trImportDraft");
                        break;
                    // استراد
                    case "im":
                        value = AppSettings.resourcemanagerreport.GetString("trImport");
                        break;
                    // طلب استيراد
                    case "imw":
                        value = AppSettings.resourcemanagerreport.GetString("trImportOrder");
                        break;
                    //مسودة تصدير
                    case "exd":
                        value = AppSettings.resourcemanagerreport.GetString("trExportDraft");
                        break;
                    // تصدير
                    case "ex":
                        value = AppSettings.resourcemanagerreport.GetString("trExport");
                        break;
                    // طلب تصدير
                    case "exw":
                        value = AppSettings.resourcemanagerreport.GetString("trExportOrder");
                        break;
                    // إدخال مباشر
                    case "is":
                        value = AppSettings.resourcemanagerreport.GetString("trDirectEntry");
                        break;
                    // مسودة إدخال مباشر
                    case "isd":
                        value = AppSettings.resourcemanagerreport.GetString("trDirectEntryDraft");
                        break;
                    // مسودة طلب خارجي
                    case "tsd":
                        value = AppSettings.resourcemanagerreport.GetString("trTakeAwayDraft");
                        break;
                    // طلب خارجي
                    case "ts":
                        value = AppSettings.resourcemanagerreport.GetString("trTakeAway");
                        break;
                    // خدمة ذاتية
                    case "ss":
                        value = AppSettings.resourcemanagerreport.GetString("trSelfService");
                        break;
                    // خدمة ذاتية مسودة
                    case "ssd":
                        value = AppSettings.resourcemanagerreport.GetString("trSelfServiceDraft");
                        break;
                    // فاتورة استهلاك
                    case "fbc":
                        value = AppSettings.resourcemanagerreport.GetString("consumptionInvoice");
                        break;
                    // مسودة طلب صرف
                    case "srd":
                        value = AppSettings.resourcemanagerreport.GetString("trSpendingRequestDraft");
                        break;
                    //  طلب صرف
                    case "sr":
                        value = AppSettings.resourcemanagerreport.GetString("trSpendingRequest");
                        break;
                    // مرتجع طلب صرف 
                    case "srb":
                        value = AppSettings.resourcemanagerreport.GetString("trSpendingRequestReturn");
                        break;
                    //  طلب صرف في الانتظار 
                    case "srw":
                        value = AppSettings.resourcemanagerreport.GetString("trSpendingOrderWait");
                        break;
                    default: break;
                }
                return value;
            }
            catch
            {
                return "";
            }
        }

      

        public static void Stocktakingparam(List<ReportParameter> paramarr)
        {
            paramarr.Add(new ReportParameter("trBranch", AppSettings.resourcemanagerreport.GetString("trBranch")));
            paramarr.Add(new ReportParameter("trItemUnit", AppSettings.resourcemanagerreport.GetString("trItemUnit")));
            paramarr.Add(new ReportParameter("trNo", AppSettings.resourcemanagerreport.GetString("trNo.")));
            paramarr.Add(new ReportParameter("trType", AppSettings.resourcemanagerreport.GetString("trType")));
            paramarr.Add(new ReportParameter("trDate", AppSettings.resourcemanagerreport.GetString("trDate")));
            paramarr.Add(new ReportParameter("trDiffrencePercentage", AppSettings.resourcemanagerreport.GetString("trDiffrencePercentage")));
            paramarr.Add(new ReportParameter("trItemsCount", AppSettings.resourcemanagerreport.GetString("trItemsCount")));
            paramarr.Add(new ReportParameter("trDestroyedCount", AppSettings.resourcemanagerreport.GetString("trDestroyedCount")));
            paramarr.Add(new ReportParameter("trReason", AppSettings.resourcemanagerreport.GetString("trReason")));
        }

       

        public static string archiveTypeConverter(string type)
        {
            string res = "";
            switch (type)
            {
                case "a": res = AppSettings.resourcemanagerreport.GetString("trArchived"); break;
                case "d": res = AppSettings.resourcemanagerreport.GetString("trDraft"); break;
                case "n": res = AppSettings.resourcemanagerreport.GetString("trSaved"); break;
                default: res = ""; break;
            };
            return res;
        }

        public static string StsStatementPaymentConvert(string value)
        {
            string s = "";
            switch (value)
            {
                case "cash":
                    s = AppSettings.resourcemanagerreport.GetString("trCash");
                    break;
                case "doc":
                    s = AppSettings.resourcemanagerreport.GetString("trDocument");
                    break;
                case "cheque":
                    s = AppSettings.resourcemanagerreport.GetString("trCheque");
                    break;
                case "balance":
                    s = AppSettings.resourcemanagerreport.GetString("trCredit");
                    break;
                case "card":
                    s = AppSettings.resourcemanagerreport.GetString("trAnotherPaymentMethods");
                    break;
                case "inv":
                    s = AppSettings.resourcemanagerreport.GetString("trInv");
                    break;
                default:
                    s = value;
                    break;


            }
            return s;
        }
       public static string processTypeAndCardConverter(string processType, string cardName, string transType)
        {
            string pType = processType;
            string cName = cardName;

            switch (pType)
            {
                case "statement":
                    if (transType == "p")
                    { return AppSettings.resourcemanagerreport.GetString("trRequired"); }
                    else { return AppSettings.resourcemanagerreport.GetString("trWorthy"); }
                case "cash": return AppSettings.resourcemanagerreport.GetString("trCash");
                //break;
                case "doc": return AppSettings.resourcemanagerreport.GetString("trDocument");
                //break;
                case "cheque": return AppSettings.resourcemanagerreport.GetString("trCheque");
                //break;
                case "balance": return AppSettings.resourcemanagerreport.GetString("trCredit");
                //break;
                case "card": return cName;
                //break;
                case "inv": return "-";
                case "multiple": return AppSettings.resourcemanagerreport.GetString("trMultiplePayment");
                //break;
                case "commissionAgent":
                case "destroy":
                case "shortage":
                case "deliver": return "-";
                //break;
                default: return pType;
                    //break;
            }
        }
      
     
        public static void DateFormConv(List<ReportParameter> paramarr)
        {

            paramarr.Add(new ReportParameter("dateForm", AppSettings.dateFormat));
        }

        public static void InventoryTypeConv(List<ReportParameter> paramarr)
        {

            paramarr.Add(new ReportParameter("trArchived", AppSettings.resourcemanagerreport.GetString("trArchived")));
            paramarr.Add(new ReportParameter("trSaved", AppSettings.resourcemanagerreport.GetString("trSaved")));
            paramarr.Add(new ReportParameter("trDraft", AppSettings.resourcemanagerreport.GetString("trDraft")));
        }
        public static void cashTransTypeConv(List<ReportParameter> paramarr)
        {

            paramarr.Add(new ReportParameter("trPull", AppSettings.resourcemanagerreport.GetString("trPull")));
            paramarr.Add(new ReportParameter("trDeposit", AppSettings.resourcemanagerreport.GetString("trDeposit")));

        }

        public static void cashTransferProcessTypeConv(List<ReportParameter> paramarr)
        {
            paramarr.Add(new ReportParameter("trCash", AppSettings.resourcemanagerreport.GetString("trCash")));
            paramarr.Add(new ReportParameter("trDocument", AppSettings.resourcemanagerreport.GetString("trDocument")));
            paramarr.Add(new ReportParameter("trCheque", AppSettings.resourcemanagerreport.GetString("trCheque")));
            paramarr.Add(new ReportParameter("trCredit", AppSettings.resourcemanagerreport.GetString("trCredit")));
            paramarr.Add(new ReportParameter("trInv", AppSettings.resourcemanagerreport.GetString("trInv")));
            paramarr.Add(new ReportParameter("trCard", AppSettings.resourcemanagerreport.GetString("trCreditCard")));
        }
        public static void itemTransferInvTypeConv(List<ReportParameter> paramarr)
        {
            paramarr.Add(new ReportParameter("dateForm", AppSettings.dateFormat));
            paramarr.Add(new ReportParameter("trPurchaseInvoice", AppSettings.resourcemanagerreport.GetString("trPurchaseInvoice")));
            paramarr.Add(new ReportParameter("trPurchaseInvoiceWaiting", AppSettings.resourcemanagerreport.GetString("trPurchaseInvoiceWaiting")));
            paramarr.Add(new ReportParameter("trSalesInvoice", AppSettings.resourcemanagerreport.GetString("trSalesInvoice")));
            paramarr.Add(new ReportParameter("trSalesReturnInvoice", AppSettings.resourcemanagerreport.GetString("trSalesReturnInvoice")));
            paramarr.Add(new ReportParameter("trPurchaseReturnInvoice", AppSettings.resourcemanagerreport.GetString("trPurchaseReturnInvoice")));
            paramarr.Add(new ReportParameter("trPurchaseReturnInvoiceWaiting", AppSettings.resourcemanagerreport.GetString("trPurchaseReturnInvoiceWaiting")));
            paramarr.Add(new ReportParameter("trDraftPurchaseBill", AppSettings.resourcemanagerreport.GetString("trDraftPurchaseBill")));
            paramarr.Add(new ReportParameter("trSalesDraft", AppSettings.resourcemanagerreport.GetString("trSalesDraft")));
            paramarr.Add(new ReportParameter("trSalesReturnDraft", AppSettings.resourcemanagerreport.GetString("trSalesReturnDraft")));

            paramarr.Add(new ReportParameter("trSaleOrderDraft", AppSettings.resourcemanagerreport.GetString("trSaleOrderDraft")));
            paramarr.Add(new ReportParameter("trSaleOrder", AppSettings.resourcemanagerreport.GetString("trSaleOrder")));
            paramarr.Add(new ReportParameter("trPurchaceOrderDraft", AppSettings.resourcemanagerreport.GetString("trPurchaceOrderDraft")));
            paramarr.Add(new ReportParameter("trPurchaceOrder", AppSettings.resourcemanagerreport.GetString("trPurchaceOrder")));
            paramarr.Add(new ReportParameter("trQuotationsDraft", AppSettings.resourcemanagerreport.GetString("trQuotationsDraft")));
            paramarr.Add(new ReportParameter("trQuotations", AppSettings.resourcemanagerreport.GetString("trQuotations")));
            paramarr.Add(new ReportParameter("trDestructive", AppSettings.resourcemanagerreport.GetString("trDestructive")));
            paramarr.Add(new ReportParameter("trShortage", AppSettings.resourcemanagerreport.GetString("trShortage")));
            paramarr.Add(new ReportParameter("trImportDraft", AppSettings.resourcemanagerreport.GetString("trImportDraft")));
            paramarr.Add(new ReportParameter("trImport", AppSettings.resourcemanagerreport.GetString("trImport")));
            paramarr.Add(new ReportParameter("trImportOrder", AppSettings.resourcemanagerreport.GetString("trImportOrder")));
            paramarr.Add(new ReportParameter("trExportDraft", AppSettings.resourcemanagerreport.GetString("trExportDraft")));

            paramarr.Add(new ReportParameter("trExport", AppSettings.resourcemanagerreport.GetString("trExport")));

            paramarr.Add(new ReportParameter("trExportOrder", AppSettings.resourcemanagerreport.GetString("trExportOrder")));

        }
        public static void invoiceSideConv(List<ReportParameter> paramarr)
        {


            paramarr.Add(new ReportParameter("trVendor", AppSettings.resourcemanagerreport.GetString("trVendor")));
            paramarr.Add(new ReportParameter("trCustomer", AppSettings.resourcemanagerreport.GetString("trCustomer")));


        }
        public static void AccountSideConv(List<ReportParameter> paramarr)
        {

            paramarr.Add(new ReportParameter("dateForm", AppSettings.dateFormat));

            paramarr.Add(new ReportParameter("trVendor", AppSettings.resourcemanagerreport.GetString("trVendor")));
            paramarr.Add(new ReportParameter("trCustomer", AppSettings.resourcemanagerreport.GetString("trCustomer")));
            paramarr.Add(new ReportParameter("trUser", AppSettings.resourcemanagerreport.GetString("trUser")));
            paramarr.Add(new ReportParameter("trSalary", AppSettings.resourcemanagerreport.GetString("trSalary")));
            paramarr.Add(new ReportParameter("trGeneralExpenses", AppSettings.resourcemanagerreport.GetString("trGeneralExpenses")));

            paramarr.Add(new ReportParameter("trAdministrativeDeposit", AppSettings.resourcemanagerreport.GetString("trAdministrativeDeposit")));

            paramarr.Add(new ReportParameter("trAdministrativePull", AppSettings.resourcemanagerreport.GetString("trAdministrativePull")));
            paramarr.Add(new ReportParameter("trShippingCompany", AppSettings.resourcemanagerreport.GetString("trShippingCompany")));


        }
         public static string destructiveConverter(string userdestroy)
        {
            try
            {
                if (userdestroy != null && userdestroy != "")
                {
                    return AppSettings.resourcemanagerreport.GetString("onUser") + " : " + userdestroy;
                }
                //else if (values[1] != null)
                //    return MainWindow.resourcemanager.GetString("onCompany") + " : " + values[1];
                else
                    return AppSettings.resourcemanagerreport.GetString("onCompany");
            }
            catch
            {
                return "";
            }
        }
       public static string isFreeShipConverter(int isFreeShip)
        {
            try
            {
                int value = isFreeShip;
                string result = "";
                if (value == 0)
                    result = AppSettings.resourcemanagerreport.GetString("trPaid");
                else if (value == 1)
                    result = AppSettings.resourcemanagerreport.GetString("trFree");
                else
                    result = "";
                return result;
            }
            catch
            {
                return "";
            }
        }
        public static string driverConvert(string shipUserName, long? shipUserId)
        {
            if (shipUserId != null)
            {
                return shipUserName;
            }
            else return "-";
        }

        public static string shippingCompanyNameConvert(string shippingCompanyName)
        {
            if (shippingCompanyName != null)
            {
                string s = shippingCompanyName;
                if (s.Equals("local ship"))
                    return "-";
                else
                    return s;
            }
            else return "-";
        }
     
        

        public static string accuracyDiscountConvert(decimal? discountValue, byte? discountType)
        {
            if (discountValue != null && discountType != null)
            {
                byte type = (byte)discountType;
                decimal value = (decimal)discountValue;

                decimal num = decimal.Parse(value.ToString());
                string s = num.ToString();

                switch (AppSettings.accuracy)
                {
                    case "0":
                        s = string.Format("{0:F0}", num);
                        break;
                    case "1":
                        s = string.Format("{0:F1}", num);
                        break;
                    case "2":
                        s = string.Format("{0:F2}", num);
                        break;
                    case "3":
                        s = string.Format("{0:F3}", num);
                        break;
                    default:
                        s = string.Format("{0:F1}", num);
                        break;
                }

                if (type == 2)
                {
                    string sdc = string.Format("{0:G29}", decimal.Parse(s));
                    //return sdc + "%";
                    return sdc;
                }
                else
                    return s;

            }
            else return "";
        }
        public static string accuracyDiscountConvert(decimal? discountValue, string discountType)
        {
            if (discountValue != null && discountType != null)
            {
                string type = discountType;
                decimal value = (decimal)discountValue;

                decimal num = decimal.Parse(value.ToString());
                string s = num.ToString();

                switch (AppSettings.accuracy)
                {
                    case "0":
                        s = string.Format("{0:F0}", num);
                        break;
                    case "1":
                        s = string.Format("{0:F1}", num);
                        break;
                    case "2":
                        s = string.Format("{0:F2}", num);
                        break;
                    case "3":
                        s = string.Format("{0:F3}", num);
                        break;
                    default:
                        s = string.Format("{0:F1}", num);
                        break;
                }

                if (type == "2")
                {
                    string sdc = string.Format("{0:G29}", decimal.Parse(s));
                    //return sdc + "%";
                    return sdc;
                }
                else
                    return s;

            }
            else return "";
        }
      
     
        public string GetparameterByname(List<ReportParameter> paramarr, string name)
        {
            string value = "";
            value = paramarr.Where(x => x.Name == name).FirstOrDefault().Values[0].ToString();
            return value;
        }

        public static string processTypeAndBankCardConverter(string processType, string cardName, string side)
        {

            string pType = processType;
            string cName = cardName;
            if (side == "bn" || side == "p")
            {
                return AppSettings.resourcemanagerreport.GetString("trCash");


            }
            else
            {

                switch (pType)
                {
                    case "cash": return AppSettings.resourcemanagerreport.GetString("trCash");
                    //break;
                    case "doc": return AppSettings.resourcemanagerreport.GetString("trDocument");
                    //break;
                    case "cheque": return AppSettings.resourcemanagerreport.GetString("trCheque");
                    //break;
                    case "balance": return AppSettings.resourcemanagerreport.GetString("trCredit");
                    //break;
                    case "card": return cName;
                    //break;
                    case "inv": return "-";
                    case "multiple": return AppSettings.resourcemanagerreport.GetString("trMultiplePayment");
                    case "box": return AppSettings.resourcemanagerreport.GetString("trCash");//open box
                    //break;
                    default: return pType;
                        //break;
                }
            }

        }
      public static string BranchStoreConverter(string type, string lang)
        {
            string s = "";
            if (lang == "both")
            {
                switch (type)
                {
                    case "b": s = AppSettings.resourcemanagerAr.GetString("tr_Branch"); break;
                    case "s": s = AppSettings.resourcemanagerAr.GetString("tr_Store"); break;

                }
            }
            else
            {
                switch (type)
                {
                    case "b": s = AppSettings.resourcemanagerreport.GetString("tr_Branch"); break;
                    case "s": s = AppSettings.resourcemanagerreport.GetString("tr_Store"); break;

                }
            }


            return s;
        }

        public static string ConvertBoxStat(string boxState)
        {
            try
            {
                if (boxState != null)
                {
                    string s = boxState.ToString();
                    switch (s)
                    {
                        case "c":
                            return AppSettings.resourcemanagerreport.GetString("trClosed");
                        case "o":
                            return AppSettings.resourcemanagerreport.GetString("trOpen");
                        default:
                            return "";
                    }
                }
                else return "";
            }
            catch
            {
                return "";
            }
        }

        //public static string stackToString(StackPanel stackP)
        //{
        //    string selecteditems = "";
        //    if (stackP.Children.Count > 0)
        //    {
        //        foreach (MaterialDesignThemes.Wpf.Chip c in stackP.Children)
        //        {
        //            selecteditems += c.Content.ToString() + " , ";
        //        }
        //        selecteditems = selecteditems.Remove(selecteditems.Length - 2);

        //        return selecteditems;
        //    }
        //    else  
        //    {
        //        return AppSettings.resourcemanagerreport.GetString("trAll");
        //    }

        //}
        public static string stackToString(StackPanel stackP, bool? chk_all)
        {
            string selecteditems = "";
            if (stackP.Children.Count > 0)
            {
                foreach (MaterialDesignThemes.Wpf.Chip c in stackP.Children)
                {
                    selecteditems += c.Content.ToString() + " , ";
                }
                selecteditems = selecteditems.Remove(selecteditems.Length - 2);

                return selecteditems;
            }
            else if (chk_all == true)
            {
                return AppSettings.resourcemanagerreport.GetString("trAll");
            }
            else
            {
                return "";
            }
        }
        public static string PaymentComboConvert(string Value)
        {
            switch (Value)
            {
                case "cash": return AppSettings.resourcemanagerreport.GetString("trCash");
                case "card": return AppSettings.resourcemanagerreport.GetString("trAnotherPaymentMethods");
                case "balance": return AppSettings.resourcemanagerreport.GetString("trCredit");
                case "multiple": return AppSettings.resourcemanagerreport.GetString("trMultiplePayment");
                default: return "";
            }
        }
        public static string serviceComboConvert(string Value)
        {
            switch (Value)
            {
                case "s": return AppSettings.resourcemanagerreport.GetString("trDiningHallType");
                case "ts": return AppSettings.resourcemanagerreport.GetString("trTakeAway");
                case "ss": return AppSettings.resourcemanagerreport.GetString("trSelfService");
                default: return "";
            }

        }
        public static string DiscountConvert(string type, decimal? value)
        {
            if (value != null)
            {
                decimal num = (decimal)value;
                string s = num.ToString();

                switch (AppSettings.accuracy)
                {
                    case "0":
                        s = string.Format("{0:F0}", num);
                        break;
                    case "1":
                        s = string.Format("{0:F1}", num);
                        break;
                    case "2":
                        s = string.Format("{0:F2}", num);
                        break;
                    case "3":
                        s = string.Format("{0:F3}", num);
                        break;
                    default:
                        s = string.Format("{0:F1}", num);
                        break;
                }
                if (num == 0)
                    s = string.Format("{0:G29}", num);

                if (type == "2")
                {
                    string sdc = string.Format("{0:G29}", decimal.Parse(s));
                    return sdc;
                }
                else
                {

                    return s;
                }
            }
            else
            {
                return "0";
            }

        }
        public static string dateTimeToTimeConverter(DateTime? value)
        {
            try
            {
                if (value != null)
                {
                    DateTime dt = (DateTime)value;
                    return dt.ToShortTimeString();
                }
                else
                    return "-";
            }
            catch
            {
                return "";
            }
        }

    }
}
