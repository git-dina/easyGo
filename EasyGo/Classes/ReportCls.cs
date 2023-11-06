using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using Microsoft.Reporting.WinForms;
using System.Resources;
using System.Reflection;
using System.Globalization;
using System.Collections;

using EasyGo.Classes.ApiClasses;
using System.Windows.Threading;
using netoaster;
using System.Xml;

namespace EasyGo.Classes
{
    public class ReportSize
    {

        public int width { get; set; }
        public int height { get; set; }
        // public string path { get; set; }
        public LocalReport rep { get; set; }
        public string printerName { get; set; }
        public string paperSize { get; set; }
        public string reppath { get; set; }
    }
    //public class resultmessage
    //{

    //    public string result { get; set; }
    //    public string pdfpath { get; set; }
    //    public int width { get; set; }
    //    public int height { get; set; }

    //    public LocalReport rep { get; set; }
    //    public int count { get; set; }
    //    public reportsize rs { get; set; }
    //    public Invoice prInvoice { get; set; }
    //    public List<ReportParameter> paramarr { get; set; }
    //    public string paperSize { get; set; }

    //}
    class ReportCls
    {

        List<CurrencyInfo> currencies = new List<CurrencyInfo>();
        public static void clearFolder(string FolderName)
        {
            string filename = "";
            DirectoryInfo dir = new DirectoryInfo(FolderName);

            foreach (FileInfo fi in dir.GetFiles())
            {
                filename = fi.FullName;

                if (!FileIsLocked(filename) && (fi.Extension == "PDF" || fi.Extension == "pdf"))
                {
                    fi.Delete();
                }

            }


        }

        public static bool FileIsLocked(string strFullFileName)
        {
            bool blnReturn = false;
            FileStream fs = null;

            try
            {
                fs = File.Open(strFullFileName, FileMode.OpenOrCreate, FileAccess.Read, FileShare.None);
                fs.Close();
            }
            catch (IOException ex)
            {
                blnReturn = true;
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
            return blnReturn;

        }
        public void Fillcurrency()
        {

            currencies.Add(new CurrencyInfo(CurrencyInfo.Currencies.Kuwait));
            currencies.Add(new CurrencyInfo(CurrencyInfo.Currencies.Saudi_Arabia));
            currencies.Add(new CurrencyInfo(CurrencyInfo.Currencies.Oman));
            currencies.Add(new CurrencyInfo(CurrencyInfo.Currencies.United_Arab_Emirates));
            currencies.Add(new CurrencyInfo(CurrencyInfo.Currencies.Qatar));
            currencies.Add(new CurrencyInfo(CurrencyInfo.Currencies.Bahrain));
            currencies.Add(new CurrencyInfo(CurrencyInfo.Currencies.Iraq));
            currencies.Add(new CurrencyInfo(CurrencyInfo.Currencies.Lebanon));
            currencies.Add(new CurrencyInfo(CurrencyInfo.Currencies.Syria));
            currencies.Add(new CurrencyInfo(CurrencyInfo.Currencies.Yemen));
            currencies.Add(new CurrencyInfo(CurrencyInfo.Currencies.Jordan));
            currencies.Add(new CurrencyInfo(CurrencyInfo.Currencies.Algeria));
            currencies.Add(new CurrencyInfo(CurrencyInfo.Currencies.Egypt));
            currencies.Add(new CurrencyInfo(CurrencyInfo.Currencies.Tunisia));
            currencies.Add(new CurrencyInfo(CurrencyInfo.Currencies.Sudan));
            currencies.Add(new CurrencyInfo(CurrencyInfo.Currencies.Morocco));
            currencies.Add(new CurrencyInfo(CurrencyInfo.Currencies.Libya));
            currencies.Add(new CurrencyInfo(CurrencyInfo.Currencies.Somalia));
            currencies.Add(new CurrencyInfo(CurrencyInfo.Currencies.Turkey));


        }

        public string PathUp(string path, int levelnum, string addtopath)
        {
            levelnum = 0;

            string newPath = path + addtopath;
            try
            {
                FileAttributes attr = File.GetAttributes(newPath);
                if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                { }
                else
                {
                    string finalDir = Path.GetDirectoryName(newPath);
                    if (!Directory.Exists(finalDir))
                        Directory.CreateDirectory(finalDir);
                    if (!File.Exists(newPath))
                        File.Create(newPath);
                }
            }
            catch { }
            return newPath;
        }

        public string TimeToString(TimeSpan? time)
        {
            if (time != null)
            {


                TimeSpan ts = TimeSpan.Parse(time.ToString());
                // @"hh\:mm\:ss"
                string stime = ts.ToString(@"hh\:mm");
                return stime;
            }
            else
            {
                return "-";
            }
        }

        public string DateToString(DateTime? date)
        {
            string sdate = "";
            if (date != null)
            {
                //DateTime ts = DateTime.Parse(date.ToString());
                // @"hh\:mm\:ss"
                //sdate = ts.ToString(@"d/M/yyyy");
                DateTimeFormatInfo dtfi = DateTimeFormatInfo.CurrentInfo;

                switch (AppSettings.dateFormat)
                {
                    case "ShortDatePattern":
                        sdate = date.Value.ToString(dtfi.ShortDatePattern);
                        break;
                    case "LongDatePattern":
                        sdate = date.Value.ToString(dtfi.LongDatePattern);
                        break;
                    case "MonthDayPattern":
                        sdate = date.Value.ToString(dtfi.MonthDayPattern);
                        break;
                    case "YearMonthPattern":
                        sdate = date.Value.ToString(dtfi.YearMonthPattern);
                        break;
                    default:
                        sdate = date.Value.ToString(dtfi.ShortDatePattern);
                        break;
                }
            }

            return sdate;
        }
        public static string DateToStringPatern(DateTime? date)
        {
            string sdate = "";
            if (date != null)
            {
                //DateTime ts = DateTime.Parse(date.ToString());
                // @"hh\:mm\:ss"
                //sdate = ts.ToString(@"d/M/yyyy");
                DateTimeFormatInfo dtfi = DateTimeFormatInfo.CurrentInfo;

                switch (AppSettings.dateFormat)
                {
                    case "ShortDatePattern":
                        sdate = date.Value.ToString(@"dd/MM/yyyy");
                        break;
                    case "LongDatePattern":
                        sdate = date.Value.ToString(@"dddd, MMMM d, yyyy");
                        break;
                    case "MonthDayPattern":
                        sdate = date.Value.ToString(@"MMMM dd");
                        break;
                    case "YearMonthPattern":
                        sdate = date.Value.ToString(@"MMMM yyyy");
                        break;
                    default:
                        sdate = date.Value.ToString(@"dd/MM/yyyy");
                        break;
                }
            }

            return sdate;
        }


        public ReportSize GetpayInvoiceRdlcpath(PurchaseInvoice invoice, int itemscount, string PaperSize)
        {
            string addpath="";
            string isArabic = ReportCls.checkInvLang();
            ReportSize rs = new ReportSize();
            if (isArabic == "ar")
            {
                if (invoice.InvType == "or" || invoice.InvType == "po" || invoice.InvType == "pos" || invoice.InvType == "pod" || invoice.InvType == "ors")
                {
                    //order Ar
                    if (PaperSize == "5.7cm")
                    {
                        addpath = @"\Reports\Purchase\Ar\SmallPurOrder.rdlc";
                        rs.width = 224;//224 =5.7cm
                        rs.height = GetpageHeight(itemscount , 500);

                    }
                    else //MainWindow.salePaperSize == "A4"
                    {

                        addpath = @"\Reports\Purchase\Ar\ArInvPurOrderReport.rdlc";
                    }
                }
                else
                {
                    if (PaperSize == "5.7cm")
                    {
                        addpath = @"\Reports\Purchase\Ar\SmallPur.rdlc";
                        rs.width = 224;//224 =5.7cm
                        rs.height = GetpageHeight(itemscount , 500);
                    }
                    else //PaperSize == "A4"
                    {

                        addpath = @"\Reports\Purchase\Ar\ArInvPurReport.rdlc";
                    }
                }
            }
            else if (isArabic == "en")
            {
                if (invoice.InvType == "or" || invoice.InvType == "po" || invoice.InvType == "pos" || invoice.InvType == "pod" || invoice.InvType == "ors")
                {
                    //order Ar
                    if (PaperSize == "5.7cm")
                    {
                        addpath = @"\Reports\Purchase\En\SmallPurOrder.rdlc";
                        rs.width = 224;//224 =5.7cm
                        rs.height = GetpageHeight(itemscount , 500);
                    }
                    else //PaperSize == "A4"
                    {

                        addpath = @"\Reports\Purchase\En\InvPurOrderReport.rdlc";
                    }
                }
                else
                {
                    if (PaperSize == "5.7cm")
                    {
                        addpath = @"\Reports\Purchase\En\SmallPur.rdlc";
                        rs.width = 224;//224 =5.7cm
                        rs.height = GetpageHeight(itemscount , 500);
                    }
                    else //PaperSize == "A4"
                    {

                        addpath = @"\Reports\Purchase\En\InvPurReport.rdlc";
                    }
                }
            }
           
            //
            string reppath = PathUp(Directory.GetCurrentDirectory(), 2, addpath);
            rs.reppath = reppath;
            rs.paperSize = PaperSize;
            return rs;
        }

        public List<ReportParameter> fillPurInvReport(PurchaseInvoice invoice, List<ReportParameter> paramarr)
        {
            string lang = checkInvLang();


            invoice.DiscountType = "1";


            string userName = invoice.UserName + " " + invoice.UserLastName;
           
            string agentName = ((invoice.SupplierName != null && invoice.SupplierName != "") ? invoice.SupplierName.Trim()
                : (invoice.SupplierCompany != null && invoice.SupplierCompany != "") ? invoice.SupplierCompany.Trim() : "-");

            //discountType
            paramarr.Add(new ReportParameter("invNumber", invoice.InvNumber == null ? "-" : invoice.InvNumber.ToString()));//paramarr[6]
            paramarr.Add(new ReportParameter("invoiceId", invoice.InvoiceId.ToString()));
            paramarr.Add(new ReportParameter("invDate", DateToString(invoice.UpdateDate) == null ? "-" : DateToString(invoice.UpdateDate)));
            paramarr.Add(new ReportParameter("invTime", invoice.UpdateDate == null ? "-" : TimeToString(((DateTime)(invoice.UpdateDate)).TimeOfDay)));
            paramarr.Add(new ReportParameter("vendorInvNum", invoice.SupplierCode == "-" ? "-" : invoice.SupplierCode.ToString()));
            paramarr.Add(new ReportParameter("agentName", agentName));
            paramarr.Add(new ReportParameter("total", HelpClass.DecTostring(invoice.Total) == null ? "-" : HelpClass.DecTostring(invoice.Total)));

            //  paramarr.Add(new ReportParameter("discountValue", DecTostring(disval) == null ? "-" : DecTostring(disval)));
            paramarr.Add(new ReportParameter("discountValue",  HelpClass.DecTostring(invoice.DiscountValue)));
            paramarr.Add(new ReportParameter("discountType", invoice.DiscountType == null ? "1" : invoice.DiscountType.ToString()));

            paramarr.Add(new ReportParameter("totalNet", HelpClass.DecTostring(invoice.TotalNet) == null ? "-" : HelpClass.DecTostring(invoice.TotalNet)));
            paramarr.Add(new ReportParameter("paid", HelpClass.DecTostring(invoice.Paid) == null ? "-" : HelpClass.DecTostring(invoice.Paid)));
            paramarr.Add(new ReportParameter("deserved", HelpClass.DecTostring(invoice.Deserved) == null ? "-" : HelpClass.DecTostring(invoice.Deserved)));
            paramarr.Add(new ReportParameter("remain",  "0" ));
            paramarr.Add(new ReportParameter("deservedDate", invoice.DeservedDate.ToString() == null ? "-" : DateToString(invoice.DeservedDate)));
            paramarr.Add(new ReportParameter("tax", invoice.Tax == null ? "0" : HelpClass.PercentageDecTostring(invoice.Tax)));
            string invNum = invoice.InvNumber == null ? "-" : invoice.InvNumber.ToString();
            paramarr.Add(new ReportParameter("barcodeimage", "file:\\" + BarcodeToImage(invNum, "invnum")));
            paramarr.Add(new ReportParameter("Currency", AppSettings.Currency));
            paramarr.Add(new ReportParameter("logoImage", "file:\\" + GetLogoImagePath()));
            paramarr.Add(new ReportParameter("branchName", invoice.BranchCreatorName == null ? "-" : invoice.BranchCreatorName));
            paramarr.Add(new ReportParameter("branchReciever", invoice.BranchName == null ? "-" : invoice.BranchName));
            paramarr.Add(new ReportParameter("deserveDate", invoice.DeservedDate == null ? "-" : DateToString(invoice.DeservedDate)));
            paramarr.Add(new ReportParameter("venInvoiceNumber", (invoice.VendorInvNum == null || invoice.VendorInvNum == "") ? "-" : invoice.VendorInvNum.ToString()));//paramarr[6]
            paramarr.Add(new ReportParameter("trTheRemine", AppSettings.resourcemanagerreport.GetString("trTheRemine")));
            paramarr.Add(new ReportParameter("trReceiverName", AppSettings.resourcemanagerreport.GetString("receiverName")));
            paramarr.Add(new ReportParameter("trDepartment", AppSettings.resourcemanagerreport.GetString("Purchases Department")));



            paramarr.Add(new ReportParameter("userName", userName.Trim()));
            if (invoice.InvType == "pd" ||invoice.InvType == "pbd" || invoice.InvType == "pod"
                    || invoice.InvType == "imd" || invoice.InvType == "exd" || invoice.InvType == "isd")
            {

                paramarr.Add(new ReportParameter("watermark", "1"));
                paramarr.Add(new ReportParameter("isSaved", "n"));
            }
            else
            {
                paramarr.Add(new ReportParameter("watermark", "0"));
                paramarr.Add(new ReportParameter("isSaved", "y"));
            }
            if (invoice.InvType == "pbd" || invoice.InvType == "pb" || invoice.InvType == "pbw")
            {
                paramarr.Add(new ReportParameter("Title", AppSettings.resourcemanagerreport.GetString("trPurchaseReturnInvTitle")));
                paramarr.Add(new ReportParameter("TitleAr", AppSettings.resourcemanagerAr.GetString("trPurchaseReturnInvTitle")));
            }
            else if (invoice.InvType == "p" || invoice.InvType == "pd" || invoice.InvType == "pw" || invoice.InvType == "pwd")
            {
                paramarr.Add(new ReportParameter("Title", AppSettings.resourcemanagerreport.GetString("trPurchasesInvoice")));
                paramarr.Add(new ReportParameter("TitleAr", AppSettings.resourcemanagerAr.GetString("trPurchasesInvoice")));
            }
            else if (invoice.InvType == "is" || invoice.InvType == "isd")
            {
                paramarr.Add(new ReportParameter("Title", AppSettings.resourcemanagerreport.GetString("trDirectEntry")));
                paramarr.Add(new ReportParameter("TitleAr", AppSettings.resourcemanagerAr.GetString("trDirectEntry")));
            }
            else if (invoice.InvType == "pod" || invoice.InvType == "po" || invoice.InvType == "ors" || invoice.InvType == "pos" )
            {
                paramarr.Add(new ReportParameter("Title", AppSettings.resourcemanagerreport.GetString("trPurchaceOrder")));
                paramarr.Add(new ReportParameter("TitleAr", AppSettings.resourcemanagerAr.GetString("trPurchaceOrder")));
            }
            //trPurchaceOrder

            paramarr.Add(new ReportParameter("trDraftInv", AppSettings.resourcemanagerreport.GetString("trDraft")));

            paramarr.Add(new ReportParameter("trNo", AppSettings.resourcemanagerreport.GetString("trNo.")));
            paramarr.Add(new ReportParameter("trItem", AppSettings.resourcemanagerreport.GetString("trDescription")));
            paramarr.Add(new ReportParameter("trUnit", AppSettings.resourcemanagerreport.GetString("trUnit")));
            paramarr.Add(new ReportParameter("trQTR", AppSettings.resourcemanagerreport.GetString("trQTR")));
            paramarr.Add(new ReportParameter("trPrice", AppSettings.resourcemanagerreport.GetString("trPrice")));
            paramarr.Add(new ReportParameter("trTotal", AppSettings.resourcemanagerreport.GetString("trTotal")));
            paramarr.Add(new ReportParameter("trSerials", AppSettings.resourcemanagerreport.GetString("trSerials")));
            paramarr.Add(new ReportParameter("By", AppSettings.resourcemanagerreport.GetString("By")));
           // paramarr.Add(new ReportParameter("isArchived", invoice.isArchived.ToString()));
            paramarr.Add(new ReportParameter("trArchived", AppSettings.resourcemanagerreport.GetString("trArchived")));
            paramarr.Add(new ReportParameter("mainInvNumber", invoice.MainInvNumber));
            paramarr.Add(new ReportParameter("trRefNo", AppSettings.resourcemanagerreport.GetString("trRefNo.")));
            paramarr.Add(new ReportParameter("invType", invoice.InvType));
            paramarr.Add(new ReportParameter("trUpdatedInvoice", AppSettings.resourcemanagerreport.GetString("UpdatedInvoice")));
            //
            paramarr.Add(new ReportParameter("trInvoiceCharp", AppSettings.resourcemanagerreport.GetString("trInvoiceCharp")));
            paramarr.Add(new ReportParameter("trDate", AppSettings.resourcemanagerreport.GetString("trDate")));
            paramarr.Add(new ReportParameter("trBranchStore", AppSettings.resourcemanagerreport.GetString("trBranch/Store")));
            paramarr.Add(new ReportParameter("trCreator", AppSettings.resourcemanagerreport.GetString("trCreator")));
            paramarr.Add(new ReportParameter("Receiver", AppSettings.resourcemanagerreport.GetString("Receiver")));
            paramarr.Add(new ReportParameter("trCompany", AppSettings.resourcemanagerreport.GetString("trCompany")));
            paramarr.Add(new ReportParameter("trVendor", AppSettings.resourcemanagerreport.GetString("trVendor")));
            paramarr.Add(new ReportParameter("trPayments", AppSettings.resourcemanagerreport.GetString("trPayments")));
            paramarr.Add(new ReportParameter("trCashPaid", AppSettings.resourcemanagerreport.GetString("trCashPaid")));
            paramarr.Add(new ReportParameter("trDeservedDate", AppSettings.resourcemanagerreport.GetString("trDeservedDate")));
            paramarr.Add(new ReportParameter("trSum", AppSettings.resourcemanagerreport.GetString("trSum")));
            paramarr.Add(new ReportParameter("trDiscount", AppSettings.resourcemanagerreport.GetString("trDiscount")));
            paramarr.Add(new ReportParameter("trTax", AppSettings.resourcemanagerreport.GetString("trTax")));
            paramarr.Add(new ReportParameter("trShippingAmount", AppSettings.resourcemanagerreport.GetString("trShippingAmount")));
            paramarr.Add(new ReportParameter("trBranch", AppSettings.resourcemanagerreport.GetString("trBranch")));
            paramarr.Add(new ReportParameter("trTo", AppSettings.resourcemanagerreport.GetString("trTo")));
            paramarr.Add(new ReportParameter("trDeserved", AppSettings.resourcemanagerreport.GetString("deserved")));

            string agentMobile = (invoice.SupplierMobile == null || invoice.SupplierMobile == "") ? "" : invoice.SupplierMobile;
            agentMobile = agentMobile.Length <= 7 ? "" : agentMobile;
            paramarr.Add(new ReportParameter("agentMobile", agentMobile));
            paramarr.Add(new ReportParameter("trAgentMobile", AppSettings.resourcemanagerreport.GetString("trMobile")));
            paramarr.Add(new ReportParameter("trReceiverName", AppSettings.resourcemanagerreport.GetString("receiverName")));
            paramarr.Add(new ReportParameter("trDepartment", AppSettings.resourcemanagerreport.GetString("purchasesDepartment")));
            paramarr.Add(new ReportParameter("taxValue", invoice.Tax == null ? "0" : HelpClass.DecTostring(invoice.Tax)));


           
            paramarr.Add(new ReportParameter("Notes", (invoice.Notes == null || invoice.Notes == "") ? "" : invoice.Notes.Trim()));
            paramarr.Add(new ReportParameter("invoiceMainId", invoice.InvoiceMainId == null ? "0" : invoice.InvoiceMainId.ToString()));
            //paramarr.Add(new ReportParameter("isUpdated", invoice.ChildInvoice == null ? (0).ToString() : (1).ToString()));
            paramarr.Add(new ReportParameter("shippingCost", HelpClass.DecTostring(invoice.ShippingCost)));
            paramarr.Add(new ReportParameter("trCode", AppSettings.resourcemanagerreport.GetString("trCode")));
            return paramarr;
        }
        public string BarcodeToImage(string barcodeStr, string imagename)
        {
            // create encoding object
            Zen.Barcode.Code128BarcodeDraw barcode = Zen.Barcode.BarcodeDrawFactory.Code128WithChecksum;
            string addpath = @"\Thumb\" + imagename + ".png";
            string imgpath = this.PathUp(Directory.GetCurrentDirectory(), 2, addpath);
            if (File.Exists(imgpath))
            {
                File.Delete(imgpath);
            }
            if (barcodeStr != "")
            {
                System.Drawing.Bitmap serial_bitmap = (System.Drawing.Bitmap)barcode.Draw(barcodeStr, 60);
                // System.Drawing.ImageConverter ic = new System.Drawing.ImageConverter();

                serial_bitmap.Save(imgpath);

                //  generate bitmap
                //  img_barcode.Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(serial_bitmap.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            }
            else
            {

                imgpath = "";
            }
            if (File.Exists(imgpath))
            {
                return imgpath;
            }
            else
            {
                return "";
            }


        }
        public decimal percentValue(decimal? value, decimal? percent)
        {
            decimal? perval = (value * percent / 100);
            return (decimal)perval;
        }

        public string BarcodeToImage28(string barcodeStr, string imagename)
        {
            // create encoding object
            Zen.Barcode.Code128BarcodeDraw barcode = Zen.Barcode.BarcodeDrawFactory.Code128WithChecksum;
            string addpath = @"\Thumb\" + imagename + ".png";
            string imgpath = this.PathUp(Directory.GetCurrentDirectory(), 2, addpath);
            if (File.Exists(imgpath))
            {
                File.Delete(imgpath);
            }
            if (barcodeStr != "")
            {
                System.Drawing.Bitmap serial_bitmap = (System.Drawing.Bitmap)barcode.Draw(barcodeStr, 60);
                // System.Drawing.ImageConverter ic = new System.Drawing.ImageConverter();

                serial_bitmap.Save(imgpath);

                //  generate bitmap
                //  img_barcode.Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(serial_bitmap.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            }
            else
            {

                imgpath = "";
            }
            if (File.Exists(imgpath))
            {
                return imgpath;
            }
            else
            {
                return "";
            }


        }
        public static bool checkLang()
        {
            bool isArabic;
            if (AppSettings.Reportlang.Equals("en"))
            {

                AppSettings.resourcemanagerreport = new ResourceManager("EasyGo.en_file", Assembly.GetExecutingAssembly());
                isArabic = false;
            }
            else
            {
                AppSettings.resourcemanagerreport = new ResourceManager("EasyGo.ar_file", Assembly.GetExecutingAssembly());
                isArabic = true;
            }
            return isArabic;
        }
        public static string checkInvLang()
        {
            string invlang = "en";
            AppSettings.resourcemanagerAr = new ResourceManager("EasyGo.ar_file", Assembly.GetExecutingAssembly());
            AppSettings.resourcemanagerEn = new ResourceManager("EasyGo.en_file", Assembly.GetExecutingAssembly());


            if (AppSettings.invoice_lang.Equals("en"))
            {
                AppSettings.resourcemanagerreport = new ResourceManager("EasyGo.en_file", Assembly.GetExecutingAssembly());
                invlang = "en";
            }
            else if (AppSettings.invoice_lang.Equals("ar"))
            {
                AppSettings.resourcemanagerreport = new ResourceManager("EasyGo.ar_file", Assembly.GetExecutingAssembly());
                invlang = "ar";
            }
            else
            {
                AppSettings.resourcemanagerreport = new ResourceManager("EasyGo.en_file", Assembly.GetExecutingAssembly());
                invlang = "both";
            }
            return invlang;
        }

        //public List<ReportParameter> fillPayReport(CashTransfer cashtrans)
        //{
        //    bool isArabic = checkLang();

        //    Fillcurrency();
        //    string title;
        //    string purposeval = "";
        //    if (cashtrans.transType == "p")
        //    {
        //        title = AppSettings.resourcemanagerreport.GetString("trPayVocher");
        //        if (cashtrans.isInvPurpose)
        //        {
        //            ReportConfig.ConvertInvType(cashtrans.invType);
        //            purposeval = AppSettings.resourcemanagerreport.GetString("Paymentfor") + " " + ReportConfig.ConvertInvType(cashtrans.invType) + " " + AppSettings.resourcemanagerreport.GetString("invoicenumber") + " : " + cashtrans.invNumber;
        //        }
        //        else
        //        {
        //            purposeval = cashtrans.purpose;
        //        }
        //    }
        //    else
        //    {
        //        title = AppSettings.resourcemanagerreport.GetString("trReceiptVoucher");
        //        if (cashtrans.isInvPurpose)
        //        {

        //            purposeval = AppSettings.resourcemanagerreport.GetString("Depositfor") + " " + ReportConfig.ConvertInvType(cashtrans.invType) + " " + AppSettings.resourcemanagerreport.GetString("invoicenumber") + " : " + cashtrans.invNumber;
        //        }
        //        else
        //        {
        //            purposeval = cashtrans.purpose;
        //        }
        //    }
        //    if (cashtrans.side == "mb")
        //    {
        //        purposeval = AppSettings.resourcemanagerreport.GetString("membershipSubscriptions");

        //    }
        //    string company_name = AppSettings.companyName;
        //    string comapny_address = AppSettings.Address;
        //    string company_phone = AppSettings.Phone;
        //    string company_fax = AppSettings.Fax;
        //    string company_email = AppSettings.Email;
        //    string amount = DecTostring(cashtrans.cash);
        //    string voucher_num = cashtrans.transNum.ToString();
        //    string type = "";
        //    string isCash = "0";
        //    string trans_num_txt = "";
        //    string check_num = cashtrans.docNum;
        //    //string date = cashtrans.createDate.ToString();
        //    string date = DateToString(cashtrans.createDate);
        //    string from = "";
        //    string amount_in_words = "";

        //    string recived_by = "";
        //    string user_name = cashtrans.createUserName + " " + cashtrans.createUserLName;
        //    string job = AppSettings.resourcemanagerreport.GetString("trAccoutant");
        //    string pay_to;
           
        //    if (cashtrans.side == "u" || cashtrans.side == "s")
        //    {
        //        pay_to = cashtrans.usersName + " " + cashtrans.usersLName;
        //    }
        //    else if (cashtrans.side == "v" || cashtrans.side == "c" || cashtrans.side == "mb")
        //    {
        //        pay_to = (cashtrans.agentId == null || cashtrans.agentId == 0) ? AppSettings.resourcemanagerreport.GetString("trUnKnown") : cashtrans.agentName;

        //    }
        //    else if (cashtrans.side == "sh" || cashtrans.side == "shd")
        //    {
        //        pay_to = cashtrans.shippingCompanyName;
        //    }
        //    else if (cashtrans.side == "e" || cashtrans.side == "m" || cashtrans.side == "tax")
        //    {
        //        pay_to = cashtrans.otherSide;
        //    }
        //    else
        //    {
        //        pay_to = "";
        //    }
        //    if (cashtrans.processType == "cheque")
        //    {

        //        type = AppSettings.resourcemanagerreport.GetString("trCheque");
        //        trans_num_txt = AppSettings.resourcemanagerreport.GetString("ChequeNum");
        //    }
        //    else if (cashtrans.processType == "card")
        //    {
        //        type = cashtrans.cardName;
        //        trans_num_txt = AppSettings.resourcemanagerreport.GetString("TransferNum");

        //        // card name and number
        //    }
        //    else if (cashtrans.processType == "cash")
        //    {
        //        type = AppSettings.resourcemanagerreport.GetString("trCash");
        //        isCash = "1";

        //    }
        //    else if (cashtrans.processType == "doc")
        //    {
        //        type = AppSettings.resourcemanagerreport.GetString("trDocument");
        //        trans_num_txt = AppSettings.resourcemanagerreport.GetString("DocumentNum");
        //    }
        //    /////
        //    try
        //    {
        //        long id = AppSettings.CurrencyId;
        //        ToWord toWord = new ToWord(Convert.ToDecimal(amount), currencies[int.Parse(id.ToString())]);
        //        if (isArabic)
        //        {
        //            amount_in_words = toWord.ConvertToArabic();
        //            // cashtrans.cash
        //        }
        //        else
        //        {
        //            amount_in_words = toWord.ConvertToEnglish(); ;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        amount_in_words = String.Empty;
        //    }
        //    //  rep.DataSources.Add(new ReportDataSource("DataSetBank", banksQuery));
        //    List<ReportParameter> paramarr = new List<ReportParameter>();
        //    ReportConfig.Header(paramarr);
        //    paramarr.Add(new ReportParameter("lang", AppSettings.Reportlang));
        //    paramarr.Add(new ReportParameter("title", title));
        //    paramarr.Add(new ReportParameter("company_name", company_name));
        //    paramarr.Add(new ReportParameter("comapny_address", comapny_address));
        //    paramarr.Add(new ReportParameter("company_phone", company_phone));
        //    paramarr.Add(new ReportParameter("company_fax", company_fax));
        //    paramarr.Add(new ReportParameter("company_email", company_email));
        //    paramarr.Add(new ReportParameter("company_logo_img", "file:\\" + GetLogoImagePath()));
        //    paramarr.Add(new ReportParameter("amount", amount));
        //    paramarr.Add(new ReportParameter("voucher_num", voucher_num));
        //    paramarr.Add(new ReportParameter("type", type));
        //    paramarr.Add(new ReportParameter("check_num", check_num));
        //    paramarr.Add(new ReportParameter("date", date));
        //    paramarr.Add(new ReportParameter("from", from));
        //    paramarr.Add(new ReportParameter("amount_in_words", amount_in_words));
        //    paramarr.Add(new ReportParameter("purpose", purposeval));
        //    paramarr.Add(new ReportParameter("recived_by", recived_by));
        //    paramarr.Add(new ReportParameter("user_name", user_name));
        //    paramarr.Add(new ReportParameter("pay_to", pay_to));
        //    paramarr.Add(new ReportParameter("job", job));
        //    paramarr.Add(new ReportParameter("isCash", isCash));
        //    paramarr.Add(new ReportParameter("trans_num_txt", trans_num_txt));
        //    paramarr.Add(new ReportParameter("show_header", AppSettings.show_header));
        //    //
        //    paramarr.Add(new ReportParameter("trcashAmount", AppSettings.resourcemanagerreport.GetString("cashAmount")));
        //    paramarr.Add(new ReportParameter("trVoucherno", AppSettings.resourcemanagerreport.GetString("Voucherno")));
        //    paramarr.Add(new ReportParameter("trDate", AppSettings.resourcemanagerreport.GetString("trDate")));
        //    paramarr.Add(new ReportParameter("trRecivedFromMr", AppSettings.resourcemanagerreport.GetString("RecivedFromMr")));
        //    paramarr.Add(new ReportParameter("trPaytoMr", AppSettings.resourcemanagerreport.GetString("PaytoMr")));
        //    paramarr.Add(new ReportParameter("trAmountInWords", AppSettings.resourcemanagerreport.GetString("AmountInWords")));
        //    paramarr.Add(new ReportParameter("trRecivedPurpose", AppSettings.resourcemanagerreport.GetString("RecivedPurpose")));
        //    paramarr.Add(new ReportParameter("trPaymentPurpose", AppSettings.resourcemanagerreport.GetString("PaymentPurpose")));
        //    paramarr.Add(new ReportParameter("trReceiver", AppSettings.resourcemanagerreport.GetString("Receiver")));
        //    paramarr.Add(new ReportParameter("currency", AppSettings.Currency));
        //    return paramarr;
        //}
        public string ConvertAmountToWords(Nullable<decimal> amount)
        {
            Fillcurrency();
            string amount_in_words = "";
            try
            {

                bool isArabic;
                long id = AppSettings.CurrencyId;
                ToWord toWord = new ToWord(Convert.ToDecimal(amount), currencies[int.Parse(id.ToString())]);
                isArabic = checkLang();
                if (isArabic)
                {
                    amount_in_words = toWord.ConvertToArabic();
                    // cashtrans.cash
                }
                else
                {
                    amount_in_words = toWord.ConvertToEnglish(); ;
                }

            }
            catch (Exception ex)
            {
                amount_in_words = String.Empty;

            }
            return amount_in_words;

        }
        public static string NumberToWordsEN(int number)
        {
            if (number == 0)
                return "zero";

            if (number < 0)
                return "minus " + NumberToWordsEN(Math.Abs(number));

            string words = "";

            if ((number / 1000000) > 0)
            {
                words += NumberToWordsEN(number / 1000000) + " million ";
                number %= 1000000;
            }

            if ((number / 1000) > 0)
            {
                words += NumberToWordsEN(number / 1000) + " thousand ";
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                words += NumberToWordsEN(number / 100) + " hundred ";
                number %= 100;
            }

            if (number > 0)
            {
                if (words != "")
                    words += "and ";

                var unitsMap = new[] { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
                var tensMap = new[] { "zero", "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };

                if (number < 20)
                    words += unitsMap[number];
                else
                {
                    words += tensMap[number / 10];
                    if ((number % 10) > 0)
                        words += "-" + unitsMap[number % 10];
                }
            }

            return words;
        }
        public static string NumberToWordsAR(int number)
        {
            if (number == 0)
                return "صفر";

            if (number < 0)
                return "ناقص " + NumberToWordsAR(Math.Abs(number));

            string words = "";

            if ((number / 1000000) > 0)
            {
                words += NumberToWordsAR(number / 1000000) + " مليون ";
                number %= 1000000;
            }

            if ((number / 1000) > 0)
            {
                words += NumberToWordsAR(number / 1000) + " الف ";
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                words += NumberToWordsAR(number / 100) + " مئة ";
                number %= 100;
            }

            if (number > 0)
            {
                if (words != "")
                    words += "و ";

                var unitsMap = new[] { "صفر", "واحد", "اثنان", "ثلاثة", "اربعة", "خمسة", "ستة", "سبعة", "ثمانية", "تسعة", "عشرة", "احدى عشر", "اثنا عشر", "ثلاثة عشر", "اربعة عشر", "خمسة عشر", "ستة عشر", "سبعة عشر", "ثمانية عشر", "تسعة عشر" };
                var tensMap = new[] { "صفر", "عشرة", "عشرون", "ثلاثون", "اربعون", "خمسون", "ستون", "سبعون", "ثمانون", "تسعون" };

                if (number < 20)
                    words += unitsMap[number];
                else
                {
                    words += tensMap[number / 10];
                    if ((number % 10) > 0)
                        words += "-" + unitsMap[number % 10];
                }
            }

            return words;
        }

        public string GetLogoImagePath()
        {
            try
            {
                string imageName = AppSettings.logoImage;
                string dir = Directory.GetCurrentDirectory();
                string tmpPath = Path.Combine(dir, @"Thumb\setting");
                tmpPath = Path.Combine(tmpPath, imageName);
                if (File.Exists(tmpPath))
                {

                    return tmpPath;
                }
                else
                {
                    return Path.Combine(Directory.GetCurrentDirectory(), @"Thumb\setting\emptylogo.png");
                }



                //string addpath = @"\Thumb\setting\" ;

            }
            catch
            {
                return Path.Combine(Directory.GetCurrentDirectory(), @"Thumb\setting\emptylogo.png");
            }
        }
        public string GetIconImagePath(string iconName)
        {
            try
            {
                string imageName = iconName + ".png";
                string dir = Directory.GetCurrentDirectory();
                string tmpPath = Path.Combine(dir, @"pic\socialMedia");
                tmpPath = Path.Combine(tmpPath, imageName);
                if (File.Exists(tmpPath))
                {

                    return tmpPath;
                }
                else
                {
                    return Path.Combine(Directory.GetCurrentDirectory(), @"Thumb\setting\emptylogo.png");
                }



                //string addpath = @"\Thumb\setting\" ;

            }
            catch
            {
                return Path.Combine(Directory.GetCurrentDirectory(), @"Thumb\setting\emptylogo.png");
            }
        }
        //

        public string GetPath(string localpath)
        {
            //string imageName = AppSettings.logoImage;
            //string dir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            string dir = Directory.GetCurrentDirectory();
            string tmpPath = Path.Combine(dir, localpath);



            //string addpath = @"\Thumb\setting\" ;

            return tmpPath;
        }

        public string ReadFile(string localpath)
        {
            string path = GetPath(localpath);
            StreamReader str = new StreamReader(path);
            string content = str.ReadToEnd();
            str.Close();
            return content;
        }

        //public reportsize GetpayInvoiceRdlcpath(Invoice invoice, string PaperSize, int itemscount, LocalReport rep)
        //{
        //    string addpath = "";
        //    string isArabic = checkInvLang();
        //    reportsize rs = new reportsize();
        //    if (isArabic == "ar")
        //    {
        //        if (invoice.invType == "or" || invoice.invType == "po" || invoice.invType == "pos" || invoice.invType == "pod" || invoice.invType == "ors")
        //        {
        //            if (PaperSize == "5.7cm")
        //            {
        //                addpath = @"\Reports\Purchase\Ar\SmallPurOrder.rdlc";
        //                rs.width = 224;//224 =5.7cm
        //                               //   rs.height = GetpageHeight(itemscount  , 500);
        //                rs.height = GetpageHeight(itemscount, 480, 19);
        //            }
        //            else //MainWindow.salePaperSize == "A4"
        //            {
        //                addpath = @"\Reports\Purchase\Ar\ArPurOrderInv.rdlc";
        //            }


        //        }
        //        else if (invoice.invType == "is" || invoice.invType == "isd")
        //        {
        //            if (PaperSize == "5.7cm")
        //            {
        //                addpath = @"\Reports\Storage\Invoice\Ar\SmallDirectEntry.rdlc";
        //                rs.width = 224;//224 =5.7cm
        //                               //   rs.height = GetpageHeight(itemscount  , 500);
        //                rs.height = GetpageHeight(itemscount, 480, 19);
        //            }
        //            else //MainWindow.salePaperSize == "A4"
        //            {
        //                addpath = @"\Reports\Storage\Invoice\Ar\DirectEntry.rdlc";
        //            }


        //        }
        //        else
        //        {
        //            if (PaperSize == "5.7cm")
        //            {
        //                addpath = @"\Reports\Purchase\Ar\SmallPur.rdlc";
        //                rs.width = 224;//224 =5.7cm
        //                               //   rs.height = GetpageHeight(itemscount  , 500);
        //                rs.height = GetpageHeight(itemscount, 480, 19);
        //            }
        //            else //MainWindow.salePaperSize == "A4"
        //            {
        //                addpath = @"\Reports\Purchase\Ar\ArPurInv.rdlc";
        //            }

        //        }

        //    }
        //    else if (isArabic == "en")
        //    {
        //        if (invoice.invType == "or" || invoice.invType == "po" || invoice.invType == "pos" || invoice.invType == "pod" || invoice.invType == "ors")
        //        {
        //            if (PaperSize == "5.7cm")
        //            {
        //                addpath = @"\Reports\Purchase\En\SmallPurOrder.rdlc";
        //                rs.width = 224;//224 =5.7cm
        //                               //   rs.height = GetpageHeight(itemscount  , 500);
        //                rs.height = GetpageHeight(itemscount, 480, 19);
        //            }
        //            else //MainWindow.salePaperSize == "A4"
        //            {

        //                addpath = @"\Reports\Purchase\En\EnPurOrderInv.rdlc";
        //            }
        //        }
        //        else if (invoice.invType == "is" || invoice.invType == "isd")
        //        {
        //            if (PaperSize == "5.7cm")
        //            {
        //                addpath = @"\Reports\Storage\Invoice\En\SmallDirectEntry.rdlc";
        //                rs.width = 224;//224 =5.7cm
        //                               //   rs.height = GetpageHeight(itemscount  , 500);
        //                rs.height = GetpageHeight(itemscount, 480, 19);
        //            }
        //            else //MainWindow.salePaperSize == "A4"
        //            {
        //                addpath = @"\Reports\Storage\Invoice\En\DirectEntry.rdlc";
        //            }
        //        }
        //        else
        //        {
        //            if (PaperSize == "5.7cm")
        //            {
        //                addpath = @"\Reports\Purchase\En\SmallPur.rdlc";
        //                rs.width = 224;//224 =5.7cm
        //                               //   rs.height = GetpageHeight(itemscount  , 500);
        //                rs.height = GetpageHeight(itemscount, 480, 19);
        //            }
        //            else //MainWindow.salePaperSize == "A4"
        //            {
        //                addpath = @"\Reports\Purchase\En\EnPurInv.rdlc";
        //            }
        //        }
        //    }
        //    else
        //    {//Both
        //        if (invoice.invType == "or" || invoice.invType == "po" || invoice.invType == "pos" || invoice.invType == "pod" || invoice.invType == "ors")
        //        {
        //            if (PaperSize == "5.7cm")
        //            {
        //                addpath = @"\Reports\Purchase\Both\SmallPurOrder.rdlc";
        //                rs.width = 224;//224 =5.7cm
        //                               //   rs.height = GetpageHeight(itemscount  , 500);
        //                rs.height = GetpageHeight(itemscount, 580, 19);//380
        //            }
        //            else //MainWindow.salePaperSize == "A4"
        //            {

        //                addpath = @"\Reports\Purchase\Both\InvPurOrder.rdlc";
        //            }
        //        }
        //        else if (invoice.invType == "is" || invoice.invType == "isd")
        //        {
        //            if (PaperSize == "5.7cm")
        //            {
        //                addpath = @"\Reports\Storage\Invoice\Both\SmallDirectEntry.rdlc";
        //                rs.width = 224;//224 =5.7cm
        //                               //   rs.height = GetpageHeight(itemscount  , 500);
        //                rs.height = GetpageHeight(itemscount, 580, 19);
        //            }
        //            else //MainWindow.salePaperSize == "A4"
        //            {
        //                addpath = @"\Reports\Storage\Invoice\Both\DirectEntry.rdlc";
        //            }
        //        }
        //        else
        //        {
        //            if (PaperSize == "5.7cm")
        //            {
        //                addpath = @"\Reports\Purchase\Both\SmallPur.rdlc";
        //                rs.width = 224;//224 =5.7cm
        //                               //   rs.height = GetpageHeight(itemscount  , 500);
        //                rs.height = GetpageHeight(itemscount, 580, 19);
        //            }
        //            else //MainWindow.salePaperSize == "A4"
        //            {
        //                addpath = @"\Reports\Purchase\Both\InvPur.rdlc";
        //            }
        //        }
        //    }
        //    //
        //    string reppath = PathUp(Directory.GetCurrentDirectory(), 2, addpath);
        //    rep.ReportPath = reppath;
        //    rs.rep = rep;
        //    rs.paperSize = PaperSize;

        //    return rs;
        //}
        public int GetpageHeight(int itemcount, int repheight)
        {
            // int repheight = 457;
            int tableheight = 33 * itemcount;// 33 is cell height


            int totalheight = repheight + tableheight;
            return totalheight;

        }
        public int GetpageHeight(int itemcount, int repheight, int itemHeight)
        {
            // int repheight = 457;
            int tableheight = itemHeight * itemcount;// 33 is cell height


            int totalheight = repheight + tableheight;
            return totalheight;

        }
        //public reportsize GetDirectEntryRdlcpath(Invoice invoice, string PaperSize, int itemscount, LocalReport rep)
        //{
        //    string addpath = "";
        //    string isArabic = checkInvLang();
        //    reportsize rs = new reportsize();

        //    if (isArabic == "ar")
        //    {
        //        if (invoice.invType == "or" || invoice.invType == "po" || invoice.invType == "pos" || invoice.invType == "pod" || invoice.invType == "ors")
        //        {
        //            addpath = @"\Reports\Purchase\Ar\ArPurOrderInv.rdlc";
        //        }
        //        else if (invoice.invType == "is" || invoice.invType == "isd")
        //        {
        //            addpath = @"\Reports\Storage\storageOperations\Ar\ArDirectEntry.rdlc";
        //        }
        //        else
        //        {
        //            addpath = @"\Reports\Purchase\Ar\ArPurInv.rdlc";
        //        }
        //    }
        //    else if (isArabic == "en")
        //    {
        //        if (invoice.invType == "or" || invoice.invType == "po" || invoice.invType == "pos" || invoice.invType == "pod" || invoice.invType == "ors")
        //        {
        //            addpath = @"\Reports\Purchase\En\EnPurOrderInv.rdlc";
        //        }
        //        else if (invoice.invType == "is" || invoice.invType == "isd")
        //        {
        //            addpath = @"\Reports\Storage\storageOperations\En\EnDirectEntry.rdlc";
        //        }
        //        else
        //        {
        //            addpath = @"\Reports\Purchase\En\EnPurInv.rdlc";
        //        }
        //    }
        //    else
        //    {//both

        //    }
        //    string reppath = PathUp(Directory.GetCurrentDirectory(), 2, addpath);
        //    rep.ReportPath = reppath;
        //    rs.rep = rep;
        //    return rs;
        //}
        //public reportsize SpendingRequestRdlcpath(Invoice invoice, string PaperSize, int itemscount, LocalReport rep)
        //{
        //    string addpath = "";
        //    string isArabic = checkInvLang();
        //    reportsize rs = new reportsize();
        //    if (isArabic == "ar")
        //    {

        //        if (PaperSize == "5.7cm")
        //        {
        //            addpath = @"\Reports\Kitchen\Invoice\Ar\SmallSpendingRequest.rdlc";
        //            rs.width = 224;//224 =5.7cm                                        
        //            rs.height = GetpageHeight(itemscount, 380, 19);
        //        }
        //        else //MainWindow.salePaperSize == "A4"
        //        {
        //            addpath = @"\Reports\Kitchen\Invoice\Ar\SpendingRequest.rdlc";
        //        }

        //    }
        //    else if (isArabic == "en")
        //    {

        //        if (PaperSize == "5.7cm")
        //        {
        //            addpath = @"\Reports\Kitchen\Invoice\En\SmallSpendingRequest.rdlc";
        //            rs.width = 224;//224 =5.7cm                                   
        //            rs.height = GetpageHeight(itemscount, 380, 19);
        //        }
        //        else //MainWindow.salePaperSize == "A4"
        //        {

        //            addpath = @"\Reports\Kitchen\Invoice\En\SpendingRequest.rdlc";
        //        }


        //    }
        //    else
        //    {//both

        //        if (PaperSize == "5.7cm")
        //        {
        //            addpath = @"\Reports\Kitchen\Invoice\Both\SmallSpendingRequest.rdlc";
        //            rs.width = 224;//224 =5.7cm                                  
        //            rs.height = GetpageHeight(itemscount, 480, 19);//380
        //        }
        //        else //MainWindow.salePaperSize == "A4"
        //        {
        //            addpath = @"\Reports\Kitchen\Invoice\Both\SpendingRequest.rdlc";
        //        }

        //    }
        //    string reppath = PathUp(Directory.GetCurrentDirectory(), 2, addpath);
        //    rep.ReportPath = reppath;
        //    rs.rep = rep;
        //    rs.paperSize = PaperSize;
        //    return rs;
        //}
        public string SpendingRequestRdlcpath()
        {
            string addpath;
            bool isArabic = ReportCls.checkLang();
            if (isArabic)
            {

                addpath = @"\Reports\Kitchen\Ar\ArSpendingRequest.rdlc";


            }
            else
            {

                addpath = @"\Reports\Kitchen\En\EnSpendingRequest.rdlc";

            }


            string reppath = PathUp(Directory.GetCurrentDirectory(), 2, addpath);
            return reppath;
        }

        //public string GetreceiptInvoiceRdlcpath(Invoice invoice)
        //{
        //    string addpath;
        //    bool isArabic = checkLang();
        //    if (isArabic)
        //    {


        //        if (invoice.invType == "or" || invoice.invType == "ord" || invoice.invType == "ors")
        //        {
        //            addpath = @"\Reports\Sale\Invoice\Ar\ArInvPurOrderReport.rdlc";
        //        }
        //        else
        //        {

        //            if (AppSettings.salePaperSize == "10cm")
        //            {
        //                addpath = @"\Reports\Sale\Invoice\Ar\LargeSaleReport.rdlc";
        //                uc_diningHall.width = 400;//400 =10cm
        //                uc_diningHall.height = GetpageHeight(uc_diningHall.itemscount, 500);

        //            }
        //            else if (AppSettings.salePaperSize == "8cm")
        //            {
        //                addpath = @"\Reports\Sale\Invoice\Ar\MediumSaleReport.rdlc";
        //                uc_diningHall.width = 315;//315 =8cm
        //                uc_diningHall.height = GetpageHeight(uc_diningHall.itemscount, 500);
        //            }
        //            else if (AppSettings.salePaperSize == "5.7cm")
        //            {
        //                addpath = @"\Reports\Sale\Invoice\Ar\SmallSaleReport.rdlc";
        //                uc_diningHall.width = 224;//224 =5.7cm
        //                uc_diningHall.height = GetpageHeight(uc_diningHall.itemscount, 460);
        //            }
        //            else //MainWindow.salePaperSize == "A4"
        //            {
        //                addpath = @"\Reports\Sale\Invoice\Ar\ArInvPurReport.rdlc";
        //            }


        //        }

        //    }
        //    else
        //    {
        //        //if (invoice.invType == "q" || invoice.invType == "qd" || invoice.invType == "qs")
        //        //{
        //        //    addpath = @"\Reports\Sale\En\InvPurQtReport.rdlc";
        //        //}
        //        //else
        //        if (invoice.invType == "or" || invoice.invType == "ord" || invoice.invType == "ors")
        //        {
        //            addpath = @"\Reports\Sale\Invoice\En\InvPurOrderReport.rdlc";
        //        }
        //        else
        //        {
        //            if (AppSettings.salePaperSize == "10cm")
        //            {
        //                addpath = @"\Reports\Sale\Invoice\En\LargeSaleReport.rdlc";
        //                uc_diningHall.width = 400;//400 =10cm
        //                uc_diningHall.height = GetpageHeight(uc_diningHall.itemscount, 500);

        //            }
        //            else if (AppSettings.salePaperSize == "8cm")
        //            {
        //                addpath = @"\Reports\Sale\Invoice\En\MediumSaleReport.rdlc";
        //                uc_diningHall.width = 315;//315 =8cm
        //                uc_diningHall.height = GetpageHeight(uc_diningHall.itemscount, 500);

        //            }
        //            else if (AppSettings.salePaperSize == "5.7cm")
        //            {
        //                addpath = @"\Reports\Sale\Invoice\En\SmallSaleReport.rdlc";
        //                uc_diningHall.width = 224;//224 =5.7cm
        //                uc_diningHall.height = GetpageHeight(uc_diningHall.itemscount, 460);

        //            }
        //            else //MainWindow.salePaperSize == "A4"
        //            {

        //                addpath = @"\Reports\Sale\Invoice\En\InvPurReport.rdlc";
        //            }

        //        }

        //    }

        //    //

        //    string reppath = PathUp(Directory.GetCurrentDirectory(), 2, addpath);
        //    return reppath;
        //}

        //public string GetreceiptInvoiceRdlcpath(Invoice invoice, int isPreview)
        //{
        //    string addpath;
        //    bool isArabic = checkLang();
        //    if (isArabic)
        //    {

        //        //if ((invoice.invType == "q" || invoice.invType == "qd" || invoice.invType == "qs"))
        //        //{
        //        //    addpath = @"\Reports\Sale\Ar\ArInvPurQtReport.rdlc";
        //        //}
        //        //else 
        //        if (invoice.invType == "or" || invoice.invType == "ord" || invoice.invType == "ors")
        //        {
        //            addpath = @"\Reports\Sale\Invoice\Ar\ArInvPurOrderReport.rdlc";
        //        }
        //        else
        //        {

        //            if (AppSettings.salePaperSize == "10cm" && isPreview == 1)
        //            {
        //                addpath = @"\Reports\Sale\Invoice\Ar\LargeSaleReport.rdlc";
        //                uc_diningHall.width = 400;//400 =10cm
        //                uc_diningHall.height = GetpageHeight(uc_diningHall.itemscount, 500);

        //            }
        //            else if (AppSettings.salePaperSize == "8cm" && isPreview == 1)
        //            {
        //                addpath = @"\Reports\Sale\Invoice\Ar\MediumSaleReport.rdlc";
        //                uc_diningHall.width = 315;//315 =8cm
        //                uc_diningHall.height = GetpageHeight(uc_diningHall.itemscount, 500);


        //            }
        //            else if (AppSettings.salePaperSize == "5.7cm" && isPreview == 1)
        //            {
        //                addpath = @"\Reports\Sale\Invoice\Ar\SmallSaleReport.rdlc";
        //                uc_diningHall.width = 224;//224 =5.7cm
        //                uc_diningHall.height = GetpageHeight(uc_diningHall.itemscount, 460);

        //            }
        //            else //MainWindow.salePaperSize == "A4"
        //            {

        //                addpath = @"\Reports\Sale\Invoice\Ar\ArInvPurReport.rdlc";
        //            }

        //            //   addpath = @"\Reports\Sale\Ar\LargeSaleReport.rdlc";
        //            //   addpath = @"\Reports\Sale\Ar\MediumSaleReport.rdlc";
        //            //   addpath = @"\Reports\Sale\Ar\SmallSaleReport.rdlc";
        //        }

        //    }
        //    else
        //    {
        //        //if (invoice.invType == "q" || invoice.invType == "qd" || invoice.invType == "qs")
        //        //{
        //        //    addpath = @"\Reports\Sale\En\InvPurQtReport.rdlc";
        //        //}
        //        //else
        //        if (invoice.invType == "or" || invoice.invType == "ord" || invoice.invType == "ors")
        //        {
        //            addpath = @"\Reports\Sale\Invoice\En\InvPurOrderReport.rdlc";
        //        }
        //        else
        //        {
        //            if (AppSettings.salePaperSize == "10cm" && isPreview == 1)
        //            {
        //                addpath = @"\Reports\Sale\Invoice\En\LargeSaleReport.rdlc";
        //                uc_diningHall.width = 400;//400 =10cm
        //                uc_diningHall.height = GetpageHeight(uc_diningHall.itemscount, 500);

        //            }
        //            else if (AppSettings.salePaperSize == "8cm" && isPreview == 1)
        //            {
        //                addpath = @"\Reports\Sale\Invoice\En\MediumSaleReport.rdlc";
        //                uc_diningHall.width = 315;//315 =8cm
        //                uc_diningHall.height = GetpageHeight(uc_diningHall.itemscount, 500);

        //            }
        //            else if (AppSettings.salePaperSize == "5.7cm" && isPreview == 1)
        //            {
        //                addpath = @"\Reports\Sale\Invoice\En\SmallSaleReport.rdlc";
        //                uc_diningHall.width = 224;//224 =5.7cm
        //                uc_diningHall.height = GetpageHeight(uc_diningHall.itemscount, 460);

        //            }
        //            else //MainWindow.salePaperSize == "A4"
        //            {

        //                addpath = @"\Reports\Sale\Invoice\En\InvPurReport.rdlc";
        //            }
        //            //  addpath = @"\Reports\Sale\En\InvPurReport.rdlc";
        //            //    addpath = @"\Reports\Sale\En\LargeSaleReport.rdlc";
        //            //   addpath = @"\Reports\Sale\En\MediumSaleReport.rdlc";
        //            // addpath = @"\Reports\Sale\En\SmallSaleReport.rdlc";
        //        }

        //    }


        //    //

        //    string reppath = PathUp(Directory.GetCurrentDirectory(), 2, addpath);
        //    return reppath;
        //}
        //public reportsize GetreceiptInvoiceRdlcpath(Invoice invoice, int isPreview, string PaperSize, int itemscount, LocalReport rep)
        //{
        //    string addpath = "";
        //    string isArabic = checkInvLang();
        //    reportsize rs = new reportsize();
        //    if (isArabic == "ar")
        //    {


        //        if (invoice.invType == "or" || invoice.invType == "ord" || invoice.invType == "ors")
        //        {
        //            if (PaperSize == "5.7cm")
        //            {
        //                addpath = @"\Reports\Sale\Invoice\Ar\SmallSaleOrder.rdlc";
        //                rs.width = 224;//224 =5.7cm
        //                               //   rs.height = GetpageHeight(itemscount  , 500);
        //                rs.height = GetpageHeight(itemscount, 480, 19);
        //            }
        //            else //MainWindow.salePaperSize == "A4"
        //            {
        //                addpath = @"\Reports\Sale\Invoice\Ar\ArInvPurOrderReport.rdlc";
        //            }
        //        }
        //        else
        //        {
        //            //sal
        //            //if (PaperSize == "5.7cm")
        //            //{
        //            addpath = @"\Reports\Sale\Invoice\Ar\SmallSaleReport.rdlc";
        //            rs.width = 224;//224 =5.7cm
        //                           //  rs.height = GetpageHeight( itemscount, 460);
        //            rs.height = GetpageHeight(itemscount, 480, 19);

        //            //}
        //            //else //MainWindow.salePaperSize == "A4"
        //            //{

        //            //    addpath = @"\Reports\Sale\Invoice\Ar\ArInvPurReport.rdlc";
        //            //}

        //        }

        //    }
        //    else if (isArabic == "en")
        //    {// en

        //        if (invoice.invType == "or" || invoice.invType == "ord" || invoice.invType == "ors")
        //        {
        //            if (PaperSize == "5.7cm")
        //            {
        //                addpath = @"\Reports\Sale\Invoice\En\SmallSaleOrder.rdlc";
        //                rs.width = 224;//224 =5.7cm
        //                rs.height = GetpageHeight(itemscount, 480, 19);

        //            }
        //            else //MainWindow.salePaperSize == "A4"
        //            {

        //                addpath = @"\Reports\Sale\Invoice\En\InvPurOrderReport.rdlc";
        //            }
        //        }
        //        else
        //        {
        //            //if (PaperSize == "10cm" && isPreview == 1)
        //            //{
        //            //    addpath = @"\Reports\Sale\Invoice\En\LargeSaleReport.rdlc";
        //            //   rs.width = 400;//400 =10cm

        //            //    rs.height = GetpageHeight(itemscount, 440, 15);
        //            //}
        //            //else if (PaperSize == "8cm" && isPreview == 1)
        //            //{
        //            //    addpath = @"\Reports\Sale\Invoice\En\MediumSaleReport.rdlc";
        //            //    rs.width = 315;//315 =8cm
        //            // //   rs.height = GetpageHeight( itemscount, 500);
        //            //    rs.height = GetpageHeight(itemscount, 440, 19);
        //            //}
        //            // else
        //            //if (PaperSize == "5.7cm")
        //            //{
        //            addpath = @"\Reports\Sale\Invoice\En\SmallSaleReport.rdlc";
        //            //  string reppath5 = PathUp(Directory.GetCurrentDirectory(), 2, addpath);
        //            rs.width = 224;//224 =5.7cm
        //            rs.height = GetpageHeight(itemscount, 480, 19);

        //            //}
        //            //else //MainWindow.salePaperSize == "A4"
        //            //{

        //            //    addpath = @"\Reports\Sale\Invoice\En\InvPurReport.rdlc";
        //            //}
        //        }
        //    }
        //    else
        //    {//Both
        //        if (invoice.invType == "or" || invoice.invType == "ord" || invoice.invType == "ors")
        //        {
        //            //Order Both
        //            if (PaperSize == "5.7cm")
        //            {
        //                addpath = @"\Reports\Sale\Invoice\Both\SmallSaleOrder.rdlc";
        //                rs.width = 224;//224 =5.7cm
        //                rs.height = GetpageHeight(itemscount, 600, 19);

        //            }
        //            else //MainWindow.salePaperSize == "A4"
        //            {

        //                addpath = @"\Reports\Sale\Invoice\Both\InvPurOrderReport.rdlc";
        //            }
        //        }
        //        else
        //        {
        //            //sale
        //            //if (PaperSize == "5.7cm")
        //            //{
        //            addpath = @"\Reports\Sale\Invoice\Both\SmallSaleReport.rdlc";
        //            rs.width = 224;//224 =5.7cm
        //            rs.height = GetpageHeight(itemscount, 600, 19);

        //            //}
        //            //else //MainWindow.salePaperSize == "A4"
        //            //{

        //            //    addpath = @"\Reports\Sale\Invoice\Both\InvPurReportBoth.rdlc";
        //            //}
        //        }
        //    }
        //    string reppath = PathUp(Directory.GetCurrentDirectory(), 2, addpath);
        //    rep.ReportPath = reppath;
        //    rs.rep = rep;
        //    rs.paperSize = PaperSize;
        //    return rs;
        //}

        //public reportsize GetMovementRdlcpath(Invoice invoice, string PaperSize, int itemscount, LocalReport rep)
        //{
        //    string addpath;
        //    string isArabic = ReportCls.checkInvLang();
        //    reportsize rs = new reportsize();
        //    if (isArabic == "ar")
        //    {//ItemsExport
        //        if (PaperSize == "5.7cm")
        //        {
        //            addpath = @"\Reports\Storage\Invoice\Ar\SmallMovement.rdlc";
        //            rs.width = 224;//224 =5.7cm
        //                           //   rs.height = GetpageHeight(itemscount  , 500);
        //            rs.height = GetpageHeight(itemscount, 380, 19);
        //        }
        //        else //MainWindow.salePaperSize == "A4"
        //        {
        //            addpath = @"\Reports\Storage\Invoice\Ar\Movement.rdlc";
        //        }

        //    }
        //    else if (isArabic == "en")
        //    {
        //        if (PaperSize == "5.7cm")
        //        {
        //            addpath = @"\Reports\Storage\Invoice\En\SmallMovement.rdlc";
        //            rs.width = 224;//224 =5.7cm
        //                           //   rs.height = GetpageHeight(itemscount  , 500);
        //            rs.height = GetpageHeight(itemscount, 380, 19);
        //        }
        //        else //MainWindow.salePaperSize == "A4"
        //        {
        //            addpath = @"\Reports\Storage\Invoice\En\Movement.rdlc";
        //        }


        //    }
        //    else
        //    {//both
        //        if (PaperSize == "5.7cm")
        //        {
        //            addpath = @"\Reports\Storage\Invoice\Both\SmallMovement.rdlc";
        //            rs.width = 224;//224 =5.7cm
        //                           //   rs.height = GetpageHeight(itemscount  , 500);
        //            rs.height = GetpageHeight(itemscount, 480, 19);
        //        }
        //        else //MainWindow.salePaperSize == "A4"
        //        {
        //            addpath = @"\Reports\Storage\Invoice\Both\Movement.rdlc";
        //        }

        //    }

        //    //
        //    string reppath = PathUp(Directory.GetCurrentDirectory(), 2, addpath);
        //    rep.ReportPath = reppath;
        //    rs.rep = rep;
        //    rs.paperSize = PaperSize;
        //    return rs;
        //}
        public ReportSize GetKitchenRdlcpath(string PaperSize, int itemscount, LocalReport rep)
        {
            PaperSize = "5.7cm";
            // LocalReport rep = new LocalReport();
            string addpath;
            string isArabic = ReportCls.checkInvLang();
            ReportSize rs = new ReportSize();
            rs.rep = rep;
            if (itemscount > 3)
            {
                itemscount = itemscount - 3;
            }
            else
            {
                itemscount = 1;
            }
            if (isArabic == "ar")
            {
                if (PaperSize == "10cm")
                {
                    addpath = @"\Reports\Sale\Kitchen\Ar\LargeReport.rdlc";
                    rs.width = 400;//400 =10cm
                    rs.height = GetpageHeight(itemscount, 290, 25);

                }
                else if (PaperSize == "8cm")
                {
                    addpath = @"\Reports\Sale\Kitchen\Ar\MediumReport.rdlc";
                    rs.width = 315;//315 =8cm
                    rs.height = GetpageHeight(itemscount, 270, 25);


                }
                else if (PaperSize == "5.7cm")
                {
                    addpath = @"\Reports\Sale\Kitchen\Ar\SmallReport.rdlc";
                    rs.width = 224;//224 =5.7cm
                    rs.height = GetpageHeight(itemscount, 300, 25);
                }
                else //MainWindow.salePaperSize == "A4"
                {

                    addpath = @"\Reports\Sale\Kitchen\Ar\ArInvReport.rdlc";
                }
            }
            else
            {

                if (PaperSize == "10cm")
                {
                    addpath = @"\Reports\Sale\Kitchen\En\LargeReport.rdlc";
                    rs.width = 400;//400 =10cm
                    rs.height = GetpageHeight(itemscount, 290, 25);

                }
                else if (PaperSize == "8cm")
                {
                    addpath = @"\Reports\Sale\Kitchen\En\MediumReport.rdlc";
                    rs.width = 315;//315 =8cm
                    rs.height = GetpageHeight(itemscount, 270, 25);

                }
                else if (PaperSize == "5.7cm")
                {
                    addpath = @"\Reports\Sale\Kitchen\En\SmallReport.rdlc";
                    rs.width = 224;//224 =5.7cm
                    rs.height = GetpageHeight(itemscount, 300, 25);

                }
                else //MainWindow.salePaperSize == "A4"
                {

                    addpath = @"\Reports\Sale\Kitchen\En\InvReport.rdlc";
                }
            }
            //
            string reppath = PathUp(Directory.GetCurrentDirectory(), 2, addpath);
            // rs.path = reppath;
            rs.rep.ReportPath = reppath;
            rs.paperSize = PaperSize;
            //rs.rep = rep;
            return rs;
        }

        public decimal calcpercentval(string discountType, decimal? discountValue, decimal? total)
        {

            decimal disval;
            if (discountValue == null || discountValue == 0)
            {
                disval = 0;

            }
            else if (discountValue > 0)
            {

                if (discountType == null || discountType == "-1" || discountType == "0" || discountType == "1")
                {
                    disval = (decimal)discountValue;
                }
                else

                {//percent
                    if (total == null || total == 0)
                    {
                        disval = 0;
                    }
                    else
                    {
                        disval = percentValue(total, discountValue);
                    }
                }
            }
            else
            {
                disval = 0;
            }

            return disval;
        }
        //public List<ReportParameter> fillSaleInvReport(Invoice invoice, List<ReportParameter> paramarr)
        //{
        //    checkLang();

        //    string agentName = (invoice.agentCompany != null || invoice.agentCompany != "") ? invoice.agentCompany.Trim()
        //    : ((invoice.agentName != null || invoice.agentName != "") ? invoice.agentName.Trim() : "-");
        //    string userName = invoice.uuserName + " " + invoice.uuserLast;

        //    //  rep.DataSources.Add(new ReportDataSource("DataSetBank", banksQuery));

        //    decimal disval = calcpercentval(invoice.discountType, invoice.discountValue, invoice.total);
        //    decimal manualdisval = calcpercentval(invoice.manualDiscountType, invoice.manualDiscountValue, invoice.total);
        //    invoice.discountValue = disval + manualdisval;
        //    invoice.discountType = "1";

        //    //  decimal totalafterdis;
        //    //if (invoice.total != null)
        //    //{
        //    //    totalafterdis = (decimal)invoice.total - disval;
        //    //}
        //    //else
        //    //{
        //    //    totalafterdis = 0;
        //    //}

        //    // discountType
        //    //  decimal taxval = calcpercentval("2", invoice.tax, totalafterdis);

        //    // decimal totalnet = totalafterdis + taxval;
        //    //  percentValue(decimal ? value, decimal ? percent);
        //    paramarr.Add(new ReportParameter("sales_invoice_note", AppSettings.sales_invoice_note));
        //    paramarr.Add(new ReportParameter("Notes", (invoice.notes == null || invoice.notes == "") ? "-" : invoice.notes.Trim()));
        //    paramarr.Add(new ReportParameter("invNumber", (invoice.invNumber == null || invoice.invNumber == "") ? "-" : invoice.invNumber.ToString()));//paramarr[6]
        //    paramarr.Add(new ReportParameter("invoiceId", invoice.invoiceId.ToString()));

        //    paramarr.Add(new ReportParameter("invDate", DateToString(invoice.updateDate) == null ? "-" : DateToString(invoice.invDate)));
        //    paramarr.Add(new ReportParameter("invTime", TimeToString(invoice.invTime)));
        //    paramarr.Add(new ReportParameter("vendorInvNum", invoice.agentCode == "-" ? "-" : invoice.agentCode.ToString()));
        //    paramarr.Add(new ReportParameter("agentName", agentName.Trim()));
        //    paramarr.Add(new ReportParameter("total", DecTostring(invoice.total) == null ? "-" : DecTostring(invoice.total)));



        //    //  paramarr.Add(new ReportParameter("discountValue", DecTostring(disval) == null ? "-" : DecTostring(disval)));
        //    paramarr.Add(new ReportParameter("discountValue", invoice.discountValue == null ? "0" : DecTostring(invoice.discountValue)));
        //    paramarr.Add(new ReportParameter("discountType", invoice.discountType == null ? "1" : invoice.discountType.ToString()));

        //    paramarr.Add(new ReportParameter("totalNet", DecTostring(invoice.totalNet) == null ? "-" : DecTostring(invoice.totalNet)));
        //    paramarr.Add(new ReportParameter("paid", DecTostring(invoice.paid) == null ? "-" : DecTostring(invoice.paid)));
        //    paramarr.Add(new ReportParameter("deserved", DecTostring(invoice.deserved) == null ? "-" : DecTostring(invoice.deserved)));
        //    //paramarr.Add(new ReportParameter("deservedDate", invoice.deservedDate.ToString() == null ? "-" : invoice.deservedDate.ToString()));
        //    paramarr.Add(new ReportParameter("deservedDate", invoice.deservedDate.ToString() == null ? "-" : DateToString(invoice.deservedDate)));


        //    paramarr.Add(new ReportParameter("tax", DecTostring(invoice.tax) == null ? "0" : DecTostring(invoice.tax)));
        //    string invNum = invoice.invBarcode == null ? "-" : invoice.invBarcode.ToString();
        //    paramarr.Add(new ReportParameter("barcodeimage", "file:\\" + BarcodeToImage(invNum, "invnum")));
        //    paramarr.Add(new ReportParameter("Currency", AppSettings.Currency));
        //    paramarr.Add(new ReportParameter("branchName", invoice.branchName == null ? "-" : invoice.branchName));
        //    paramarr.Add(new ReportParameter("userName", userName.Trim()));
        //    paramarr.Add(new ReportParameter("logoImage", "file:\\" + GetLogoImagePath()));
        //    if (invoice.invType == "pd" || invoice.invType == "sd" || invoice.invType == "qd"
        //                || invoice.invType == "sbd" || invoice.invType == "pbd" || invoice.invType == "pod"
        //                || invoice.invType == "ord" || invoice.invType == "imd" || invoice.invType == "exd")
        //    {

        //        paramarr.Add(new ReportParameter("watermark", "1"));
        //    }
        //    else
        //    {
        //        paramarr.Add(new ReportParameter("watermark", "0"));
        //    }
        //    paramarr.Add(new ReportParameter("shippingCost", DecTostring(invoice.shippingCost)));

        //    if (invoice.invType == "sbd" || invoice.invType == "sb")
        //    {
        //        paramarr.Add(new ReportParameter("Title", AppSettings.resourcemanagerreport.GetString("trSalesReturnInvTitle")));
        //    }
        //    else if (invoice.invType == "s" || invoice.invType == "sd")
        //    {
        //        paramarr.Add(new ReportParameter("Title", AppSettings.resourcemanagerreport.GetString("trSalesInvoice")));

        //    }
        //    return paramarr;

        //}
        //public LocalReport AddDataset(LocalReport rep, List<InvoiceTaxes> invoiceTaxesList)
        //{
        //    List<InvoiceTaxes> emptyList = new List<InvoiceTaxes>();
        //    if (invoiceTaxesList != null)
        //    {
        //        foreach (InvoiceTaxes row in invoiceTaxesList)
        //        {
        //            row.rate = decimal.Parse(HelpClass.PercentageDecTostring(row.rate));
        //        }
        //        rep.DataSources.Add(new ReportDataSource("DataSetInvoiceTaxes", invoiceTaxesList));
        //    }
        //    else
        //    {
        //        rep.DataSources.Add(new ReportDataSource("DataSetInvoiceTaxes", emptyList));
        //    }
        //    return rep;
        //}

        //public List<ReportParameter> fillSaleInvReport(Invoice invoice, List<ReportParameter> paramarr, ShippingCompanies shippingcompany)
        //{
        //    string lang = checkInvLang();

        //    string agentName = (invoice.agentCompany != null && invoice.agentCompany != "") ? invoice.agentCompany.Trim()
        //    : ((invoice.agentName != null && invoice.agentName != "") ? invoice.agentName.Trim() : "-");
        //    string userName = invoice.uuserName + " " + invoice.uuserLast;

        //    //  rep.DataSources.Add(new ReportDataSource("DataSetBank", banksQuery));

        //    decimal disval = calcpercentval(invoice.discountType, invoice.discountValue, invoice.total);
        //    decimal manualdisval = calcpercentval(invoice.manualDiscountType, invoice.manualDiscountValue, invoice.total);
        //    //invoice.discountValue = disval + manualdisval;
        //    decimal totaldiscount = disval + manualdisval;
        //    invoice.discountType = "1";
        //    // code for tax
        //    #region multitax
        //    decimal totalnotax = (decimal)invoice.total - totaldiscount;
        //    decimal totaltaxvalue = 0;
        //    decimal totaltaxrate = 0;
        //    if (invoice.invoiceTaxes != null)
        //    {
        //        totaltaxvalue = invoice.invoiceTaxes.Sum(t => (decimal)t.taxValue);
        //        totaltaxrate = invoice.invoiceTaxes.Sum(t => (decimal)t.rate);// %
        //    }
        //    paramarr.Add(new ReportParameter("totalNotax", DecTostring(totalnotax)));
        //    paramarr.Add(new ReportParameter("totalTaxvalue", DecTostring(totaltaxvalue)));
        //    paramarr.Add(new ReportParameter("totalTaxrate", HelpClass.PercentageDecTostring(totaltaxrate)));
        //    paramarr.Add(new ReportParameter("trTotalNotax", AppSettings.resourcemanagerreport.GetString("TAXABLEAMT")));
        //    paramarr.Add(new ReportParameter("trTotalTax", AppSettings.resourcemanagerreport.GetString("TAXAMT")));
        //    paramarr.Add(new ReportParameter("trTotalTaxRate", AppSettings.resourcemanagerreport.GetString("RATE")));
        //    #endregion
        //    paramarr.Add(new ReportParameter("sales_invoice_note", (invoice.sales_invoice_note == null) ? "" : invoice.sales_invoice_note.Trim()));
        //    paramarr.Add(new ReportParameter("Notes", (invoice.notes == null || invoice.notes == "") ? "-" : invoice.notes.Trim()));
        //    paramarr.Add(new ReportParameter("invNumber", (invoice.invNumber == null || invoice.invNumber == "") ? "-" : invoice.invNumber.ToString()));//paramarr[6]
        //    paramarr.Add(new ReportParameter("invoiceId", invoice.invoiceId.ToString()));
        //    paramarr.Add(new ReportParameter("invDate", DateToString(invoice.invDate) == null ? "-" : DateToString(invoice.invDate)));
        //    paramarr.Add(new ReportParameter("invTime", invoice.invTime == null ? "-" : TimeToString(invoice.invTime)));
        //    paramarr.Add(new ReportParameter("vendorInvNum", invoice.agentCode == "-" ? "-" : invoice.agentCode.ToString()));
        //    paramarr.Add(new ReportParameter("agentName", agentName.Trim()));
        //    paramarr.Add(new ReportParameter("total", DecTostring(invoice.total) == null ? "-" : DecTostring(invoice.total)));

        //    //  paramarr.Add(new ReportParameter("discountValue", DecTostring(disval) == null ? "-" : DecTostring(disval)));
        //    paramarr.Add(new ReportParameter("discountValue", DecTostring(totaldiscount)));
        //    paramarr.Add(new ReportParameter("discountType", invoice.discountType == null ? "1" : invoice.discountType.ToString()));

        //    paramarr.Add(new ReportParameter("totalNet", DecTostring(invoice.totalNet) == null ? "-" : DecTostring(invoice.totalNet)));

        //    decimal finalshipCost = invoice.shippingCost - percentValue(invoice.shippingCost, invoice.shippingCostDiscount);
        //    if (invoice.isFreeShip == 1)
        //    {
        //        finalshipCost = 0;
        //    }
        //    paramarr.Add(new ReportParameter("totalNoShip", DecTostring(invoice.totalNet - finalshipCost)));

        //    paramarr.Add(new ReportParameter("paid", DecTostring(invoice.paid) == null ? "-" : DecTostring(invoice.paid)));
        //    //    paramarr.Add(new ReportParameter("deserved", DecTostring(invoice.deserved) == null ? "-" : DecTostring(invoice.deserved)));
        //    //paramarr.Add(new ReportParameter("deservedDate", invoice.deservedDate.ToString() == null ? "-" : invoice.deservedDate.ToString()));
        //    paramarr.Add(new ReportParameter("deservedDate", invoice.deservedDate.ToString() == null ? "-" : DateToString(invoice.deservedDate)));

        //    paramarr.Add(new ReportParameter("tax", invoice.tax == null ? "0" : HelpClass.PercentageDecTostring(invoice.tax)));
        //    string invNum = invoice.invBarcode == null ? "-" : invoice.invBarcode.ToString();
        //    paramarr.Add(new ReportParameter("barcodeimage", "file:\\" + BarcodeToImage(invNum, "invnum")));
        //    paramarr.Add(new ReportParameter("Currency", AppSettings.Currency));
        //    paramarr.Add(new ReportParameter("branchName", invoice.branchName == null ? "-" : invoice.branchName));
        //    paramarr.Add(new ReportParameter("userName", userName.Trim()));
        //    paramarr.Add(new ReportParameter("logoImage", "file:\\" + GetLogoImagePath()));
        //    if (invoice.invType == "pd" || invoice.invType == "sd" || invoice.invType == "qd"
        //                || invoice.invType == "sbd" || invoice.invType == "pbd" || invoice.invType == "pod"
        //                || invoice.invType == "ord" || invoice.invType == "imd" || invoice.invType == "exd"
        //                || invoice.invType == "tsd" || invoice.invType == "ssd")
        //    {

        //        paramarr.Add(new ReportParameter("watermark", "1"));
        //        paramarr.Add(new ReportParameter("isSaved", "n"));
        //    }
        //    else
        //    {
        //        paramarr.Add(new ReportParameter("watermark", "0"));
        //        paramarr.Add(new ReportParameter("isSaved", "y"));
        //    }
        //    paramarr.Add(new ReportParameter("shippingCost", DecTostring(finalshipCost)));

        //    if (invoice.invType == "sbd" || invoice.invType == "sb")
        //    {
        //        paramarr.Add(new ReportParameter("Title", AppSettings.resourcemanagerreport.GetString("trSalesReturnInvTitle")));
        //        paramarr.Add(new ReportParameter("TitleAr", AppSettings.resourcemanagerAr.GetString("trSalesReturnInvTitle")));
        //    }
        //    else if (invoice.invType == "s" || invoice.invType == "sd" || invoice.invType == "ss" || invoice.invType == "ts" || invoice.invType == "tsd" || invoice.invType == "ssd")
        //    {
        //        paramarr.Add(new ReportParameter("Title", AppSettings.resourcemanagerreport.GetString("restaurantInvoice")));
        //        paramarr.Add(new ReportParameter("TitleAr", AppSettings.resourcemanagerAr.GetString("restaurantInvoice")));

        //    }
        //    paramarr.Add(new ReportParameter("trDeliveryMan", AppSettings.resourcemanagerreport.GetString("trDriver")));
        //    paramarr.Add(new ReportParameter("trTheShippingCompany", AppSettings.resourcemanagerreport.GetString("theShippingCompany")));
        //    paramarr.Add(new ReportParameter("DeliveryMan", invoice.shipUserName));
        //    paramarr.Add(new ReportParameter("ShippingCompany", ReportConfig.shippingCompanyNameConvert(shippingcompany.name)));
        //    paramarr.Add(new ReportParameter("deliveryType", shippingcompany.deliveryType));
        //    paramarr.Add(new ReportParameter("shippingCompanyId", invoice.shippingCompanyId == null ? "0" : invoice.shippingCompanyId.ToString()));
        //    paramarr.Add(new ReportParameter("trFree", AppSettings.resourcemanagerreport.GetString("trFree")));
        //    paramarr.Add(new ReportParameter("trDraftInv", AppSettings.resourcemanagerreport.GetString("trDraft")));

        //    paramarr.Add(new ReportParameter("trNo", AppSettings.resourcemanagerreport.GetString("trNo.")));
        //    paramarr.Add(new ReportParameter("trItem", AppSettings.resourcemanagerreport.GetString("trDescription")));
        //    paramarr.Add(new ReportParameter("trUnit", AppSettings.resourcemanagerreport.GetString("trUnit")));
        //    paramarr.Add(new ReportParameter("trQTR", AppSettings.resourcemanagerreport.GetString("trQTR")));
        //    paramarr.Add(new ReportParameter("trPrice", AppSettings.resourcemanagerreport.GetString("trPrice")));
        //    paramarr.Add(new ReportParameter("trTotal", AppSettings.resourcemanagerreport.GetString("trTotal")));
        //    //
        //    paramarr.Add(new ReportParameter("By", AppSettings.resourcemanagerreport.GetString("By")));
        //    paramarr.Add(new ReportParameter("trWarrantyPeriod", AppSettings.resourcemanagerreport.GetString("warranty")));
        //    paramarr.Add(new ReportParameter("trOnDelivery", AppSettings.resourcemanagerreport.GetString("OnDelivery")));

        //    // paramarr.Add(new ReportParameter("isArchived", invoice.isArchived.ToString()));
        //    // paramarr.Add(new ReportParameter("mainInvNumber", invoice.mainInvNumber));
        //    paramarr.Add(new ReportParameter("trRefNo", AppSettings.resourcemanagerreport.GetString("trRefNo.")));
        //    paramarr.Add(new ReportParameter("invType", invoice.invType));
        //    paramarr.Add(new ReportParameter("trContents", AppSettings.resourcemanagerreport.GetString("contents")));
        //    //  paramarr.Add(new ReportParameter("trSerial", AppSettings.resourcemanagerreport.GetString("trSerial")));
        //    paramarr.Add(new ReportParameter("invoiceMainId", invoice.invoiceMainId == null ? "0" : invoice.invoiceMainId.ToString()));
        //    //   paramarr.Add(new ReportParameter("trinvoiceClass", AppSettings.resourcemanagerreport.GetString("invoiceClass")));

        //    paramarr.Add(new ReportParameter("remain", DecTostring(invoice.cashReturn) == null ? "0" : DecTostring(invoice.cashReturn)));

        //    //
        //    paramarr.Add(new ReportParameter("trInvoiceCharp", AppSettings.resourcemanagerreport.GetString("trInvoiceCharp")));
        //    paramarr.Add(new ReportParameter("trDate", AppSettings.resourcemanagerreport.GetString("trDate")));
        //    paramarr.Add(new ReportParameter("trBranch", AppSettings.resourcemanagerreport.GetString("trBranch")));
        //    paramarr.Add(new ReportParameter("trTo", AppSettings.resourcemanagerreport.GetString("trTo")));
        //    paramarr.Add(new ReportParameter("trPayments", AppSettings.resourcemanagerreport.GetString("trPayments")));
        //    paramarr.Add(new ReportParameter("trCashPaid", AppSettings.resourcemanagerreport.GetString("trCashPaid")));
        //    paramarr.Add(new ReportParameter("trUnPaid", AppSettings.resourcemanagerreport.GetString("trUnPaid")));
        //    paramarr.Add(new ReportParameter("trUnPaidCash", AppSettings.resourcemanagerreport.GetString("trUnPaidCash")));
        //    paramarr.Add(new ReportParameter("trTheRemine", AppSettings.resourcemanagerreport.GetString("trTheRemine")));
        //    paramarr.Add(new ReportParameter("trSum", AppSettings.resourcemanagerreport.GetString("trSum")));
        //    paramarr.Add(new ReportParameter("trDiscount", AppSettings.resourcemanagerreport.GetString("trDiscount")));
        //    paramarr.Add(new ReportParameter("trTax", AppSettings.resourcemanagerreport.GetString("trTax")));
        //    paramarr.Add(new ReportParameter("trDelivery", AppSettings.resourcemanagerreport.GetString("trDelivery")));
        //    paramarr.Add(new ReportParameter("trDeserved", AppSettings.resourcemanagerreport.GetString("deserved")));
        //    paramarr.Add(new ReportParameter("trAddress", AppSettings.resourcemanagerreport.GetString("trAddress")));
        //    string sectb = AppSettings.resourcemanagerreport.GetString("trSection") + "/" + AppSettings.resourcemanagerreport.GetString("trTable");
        //    paramarr.Add(new ReportParameter("trTables", sectb));
        //    paramarr.Add(new ReportParameter("trWaiter", AppSettings.resourcemanagerreport.GetString("trWaiter")));
        //    paramarr.Add(new ReportParameter("Tables", (invoice.sectionTable == null || invoice.sectionTable == "") ? "" : invoice.sectionTable.Trim()));

        //    paramarr.Add(new ReportParameter("Waiter", (invoice.waiterName == null || invoice.waiterName == "") ? "" : invoice.waiterName.Trim()));
        //    paramarr.Add(new ReportParameter("carNumber", (invoice.carNumber == null || invoice.carNumber == "") ? "" : invoice.carNumber.Trim()));
        //    paramarr.Add(new ReportParameter("trCarNumber", AppSettings.resourcemanagerreport.GetString("carNumber")));

        //    string agentMobile = (invoice.agentMobile == null || invoice.agentMobile == "") ? "" : invoice.agentMobile;
        //    agentMobile = agentMobile.Length <= 7 ? "" : agentMobile;
        //    paramarr.Add(new ReportParameter("agentMobile", agentMobile));
        //    paramarr.Add(new ReportParameter("trAgentMobile", AppSettings.resourcemanagerreport.GetString("trMobile")));

        //    paramarr.Add(new ReportParameter("trReceiverName", AppSettings.resourcemanagerreport.GetString("receiverName")));
        //    paramarr.Add(new ReportParameter("trDepartment", AppSettings.resourcemanagerreport.GetString("salesDepartment")));


        //    if (lang == "both")
        //    {
        //        paramarr.Add(new ReportParameter("trDraftInvAr", AppSettings.resourcemanagerAr.GetString("trDraft")));
        //        paramarr.Add(new ReportParameter("trRefNoAr", AppSettings.resourcemanagerAr.GetString("trRefNo.")));
        //        paramarr.Add(new ReportParameter("trAddressAr", AppSettings.resourcemanagerAr.GetString("trAddress")));
        //        paramarr.Add(new ReportParameter("trItemAr", AppSettings.resourcemanagerAr.GetString("trDescription")));
        //        paramarr.Add(new ReportParameter("trQTRAr", AppSettings.resourcemanagerAr.GetString("trQTR")));
        //        paramarr.Add(new ReportParameter("trPriceAr", AppSettings.resourcemanagerAr.GetString("trPrice")));
        //        paramarr.Add(new ReportParameter("trTotalAr", AppSettings.resourcemanagerAr.GetString("trTotal")));
        //        paramarr.Add(new ReportParameter("cashTrAr", AppSettings.resourcemanagerAr.GetString("trCashType")));
        //        paramarr.Add(new ReportParameter("trOnDeliveryAr", AppSettings.resourcemanagerAr.GetString("OnDelivery")));
        //        paramarr.Add(new ReportParameter("trTheShippingCompanyAr", AppSettings.resourcemanagerAr.GetString("trTheShippingCompany")));
        //        paramarr.Add(new ReportParameter("trDeliveryManAr", AppSettings.resourcemanagerAr.GetString("trDeliveryMan")));
        //        paramarr.Add(new ReportParameter("trUpdatedInvoiceAr", AppSettings.resourcemanagerAr.GetString("UpdatedInvoice")));
        //        paramarr.Add(new ReportParameter("trinvoiceClassAr", AppSettings.resourcemanagerAr.GetString("invoiceClass")));
        //        paramarr.Add(new ReportParameter("trFreeAr", AppSettings.resourcemanagerAr.GetString("trFree")));
        //        //   
        //        paramarr.Add(new ReportParameter("trInvoiceCharpAr", AppSettings.resourcemanagerAr.GetString("trInvoiceCharp")));
        //        paramarr.Add(new ReportParameter("trDateAr", AppSettings.resourcemanagerAr.GetString("trDate")));
        //        paramarr.Add(new ReportParameter("trBranchAr", AppSettings.resourcemanagerAr.GetString("trBranch")));
        //        paramarr.Add(new ReportParameter("trToAr", AppSettings.resourcemanagerAr.GetString("trTo")));
        //        paramarr.Add(new ReportParameter("trPaymentsAr", AppSettings.resourcemanagerAr.GetString("trPayments")));
        //        paramarr.Add(new ReportParameter("trCashPaidAr", AppSettings.resourcemanagerAr.GetString("trCashPaid")));
        //        paramarr.Add(new ReportParameter("trUnPaidAr", AppSettings.resourcemanagerAr.GetString("trUnPaid")));
        //        paramarr.Add(new ReportParameter("trUnPaidCashAr", AppSettings.resourcemanagerAr.GetString("trUnPaidCash")));
        //        paramarr.Add(new ReportParameter("trTheRemineAr", AppSettings.resourcemanagerAr.GetString("trTheRemine")));
        //        paramarr.Add(new ReportParameter("trSumAr", AppSettings.resourcemanagerAr.GetString("trSum")));
        //        paramarr.Add(new ReportParameter("trDiscountAr", AppSettings.resourcemanagerAr.GetString("trDiscount")));
        //        paramarr.Add(new ReportParameter("trTaxAr", AppSettings.resourcemanagerAr.GetString("trTax")));
        //        paramarr.Add(new ReportParameter("trDeliveryAr", AppSettings.resourcemanagerAr.GetString("trDelivery")));
        //        paramarr.Add(new ReportParameter("trDeservedAr", AppSettings.resourcemanagerAr.GetString("deserved")));
        //        string sectbAr = AppSettings.resourcemanagerAr.GetString("trSection") + "/" + AppSettings.resourcemanagerAr.GetString("trTable");
        //        paramarr.Add(new ReportParameter("trTablesAr", sectbAr));
        //        paramarr.Add(new ReportParameter("trWaiterAr", AppSettings.resourcemanagerAr.GetString("trWaiter")));
        //        paramarr.Add(new ReportParameter("trCarNumberAr", AppSettings.resourcemanagerAr.GetString("carNumber")));

        //        paramarr.Add(new ReportParameter("trAgentMobileAr", AppSettings.resourcemanagerAr.GetString("trMobile")));
        //        paramarr.Add(new ReportParameter("trReceiverNameAr", AppSettings.resourcemanagerAr.GetString("receiverName")));
        //        paramarr.Add(new ReportParameter("trDepartmentAr", AppSettings.resourcemanagerAr.GetString("salesDepartment")));
        //        #region multitax
        //        paramarr.Add(new ReportParameter("trTotalNotaxAr", AppSettings.resourcemanagerAr.GetString("TAXABLEAMT")));
        //        paramarr.Add(new ReportParameter("trTotalTaxAr", AppSettings.resourcemanagerAr.GetString("TAXAMT")));
        //        paramarr.Add(new ReportParameter("trTotalTaxRateAr", AppSettings.resourcemanagerAr.GetString("RATE")));
        //        #endregion
        //    }
        //    paramarr.Add(new ReportParameter("trUpdatedInvoice", AppSettings.resourcemanagerreport.GetString("UpdatedInvoice")));
        //    paramarr.Add(new ReportParameter("isPrePaid", invoice.isPrePaid.ToString()));
        //    // paramarr.Add(new ReportParameter("isShipPaid", invoice.isShipPaid.ToString()));
        //    paramarr.Add(new ReportParameter("trOfferOnRep", AppSettings.resourcemanagerreport.GetString("trOfferOnRep")));


        //    paramarr.Add(new ReportParameter("isUpdated", invoice.ChildInvoice == null ? (0).ToString() : (1).ToString()));
        //    paramarr.Add(new ReportParameter("trDiscountOffer", AppSettings.resourcemanagerreport.GetString("trDiscountOffer")));
        //    paramarr.Add(new ReportParameter("isFreeShip", invoice.isFreeShip.ToString()));
        //    paramarr.Add(new ReportParameter("trCode", AppSettings.resourcemanagerreport.GetString("trCode")));

        //    return paramarr;

        //}

        //public static List<ItemTransferInvoice> converter(List<ItemTransferInvoice> query)
        //{
        //    foreach (ItemTransferInvoice item in query)
        //    {
        //        if (item.invType == "p")
        //        {
        //            item.invType = AppSettings.resourcemanagerreport.GetString("trPurchaseInvoice");
        //        }
        //        else if (item.invType == "pw")
        //        {
        //            item.invType = AppSettings.resourcemanagerreport.GetString("trPurchaseInvoice");
        //        }
        //        else if (item.invType == "pb")
        //        {
        //            item.invType = AppSettings.resourcemanagerreport.GetString("trPurchaseReturnInvoice");
        //        }
        //        else if (item.invType == "pd")
        //        {
        //            item.invType = AppSettings.resourcemanagerreport.GetString("trDraftPurchaseBill");
        //        }
        //        else if (item.invType == "pbd")
        //        {
        //            item.invType = AppSettings.resourcemanagerreport.GetString("trPurchaseReturnDraft");
        //        }
        //    }
        //    return query;

        //}

        /////////
        ///
        public bool encodefile(string source, string dest)
        {
            try
            {

                byte[] arr = File.ReadAllBytes(source);

                arr = Encrypt(arr);

                File.WriteAllBytes(dest, arr);
                return true;
            }
            catch
            {
                return false;
            }

        }
        public static byte[] Encrypt(byte[] ordinary)
        {
            BitArray bits = ToBits(ordinary);
            BitArray LHH = SubBits(bits, 0, bits.Length / 2);
            BitArray RHH = SubBits(bits, bits.Length / 2, bits.Length / 2);
            BitArray XorH = LHH.Xor(RHH);
            RHH = RHH.Not();
            XorH = XorH.Not();
            BitArray encr = ConcateBits(XorH, RHH);
            byte[] b = new byte[encr.Length / 8];
            encr.CopyTo(b, 0);
            return b;
        }
        private static BitArray ToBits(byte[] Bytes)
        {
            BitArray bits = new BitArray(Bytes);
            return bits;
        }
        private static BitArray SubBits(BitArray Bits, int Start, int Length)
        {
            BitArray half = new BitArray(Length);
            for (int i = 0; i < half.Length; i++)
                half[i] = Bits[i + Start];
            return half;
        }
        private static BitArray ConcateBits(BitArray LHH, BitArray RHH)
        {
            BitArray bits = new BitArray(LHH.Length + RHH.Length);
            for (int i = 0; i < LHH.Length; i++)
                bits[i] = LHH[i];
            for (int i = 0; i < RHH.Length; i++)
                bits[i + LHH.Length] = RHH[i];
            return bits;
        }
        public void DelFile(string fileName)
        {

            bool inuse = false;

            inuse = IsFileInUse(fileName);
            if (inuse == false)
            {
                File.Delete(fileName);
            }






        }
        private bool IsFileInUse(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                //throw new ArgumentException("'path' cannot be null or empty.", "path");
                return true;
            }


            try
            {
                using (var stream = new FileStream(path, FileMode.Open, FileAccess.ReadWrite)) { }
            }
            catch (IOException)
            {
                return true;
            }

            return false;
        }
        public static bool encodestring(string sourcetext, string dest)
        {
            try
            {
                byte[] arr = ConvertToBytes(sourcetext);
                //  byte[] arr = File.ReadAllBytes(source);

                arr = Encrypt(arr);

                File.WriteAllBytes(dest, arr);
                return true;
            }
            catch
            {
                return false;
            }

        }
        private static byte[] ConvertToBytes(string text)
        {
            return System.Text.Encoding.Unicode.GetBytes(text);
        }
        public static string Decrypt(string EncryptedText)
        {
            byte[] b = ConvertToBytes(EncryptedText);
            b = Decrypt(b);
            return ConvertToText(b);
        }
        public static string DeCompressThenDecrypt(string text)
        {
            var bytes = Encoding.UTF8.GetBytes(text);
            text = Encoding.UTF8.GetString(bytes);

            return (Decrypt(text));
        }
        public static bool decodefile(string Source, string DestPath)
        {
            try
            {
                byte[] restorearr = File.ReadAllBytes(Source);

                restorearr = Decrypt(restorearr);
                File.WriteAllBytes(DestPath, restorearr);
                return true;

            }
            catch
            {
                return false;
            }
        }
        public static string decodetoString(string Source)
        {
            try
            {
                byte[] restorearr = File.ReadAllBytes(Source);

                restorearr = Decrypt(restorearr);
                return ConvertToText(restorearr);
                // File.WriteAllBytes(DestPath, restorearr);


            }
            catch
            {
                return "0";
            }
        }
        private static string ConvertToText(byte[] ByteAarry)
        {
            return System.Text.Encoding.Unicode.GetString(ByteAarry);
        }
        public static byte[] Decrypt(byte[] Encrypted)
        {
            BitArray enc = ToBits(Encrypted);
            BitArray XorH = SubBits(enc, 0, enc.Length / 2);
            XorH = XorH.Not();
            BitArray RHH = SubBits(enc, enc.Length / 2, enc.Length / 2);
            RHH = RHH.Not();
            BitArray LHH = XorH.Xor(RHH);
            BitArray bits = ConcateBits(LHH, RHH);
            byte[] decr = new byte[bits.Length / 8];
            bits.CopyTo(decr, 0);
            return decr;
        }

       
    }
}

