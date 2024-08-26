using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NES_WEB_ACC.ViewModels.RDLC
{
    public class VoucherReportViewModel
    {
        public string BillNo { get; set; }
        public string BillDate { get; set; }
        public string CreateBy { get; set; }
        public string CreateByName { get; set; }
        public string CreateDate { get; set; }
        public string CheckBy { get; set; }
        public string CheckByName { get; set; }
        public string CheckDate { get; set; }
        public string StateBy { get; set; }
        public string StateByName { get; set; }
        public string StateDate { get; set; }
        public string ClosedBy { get; set; }
        public string ClosedByName { get; set; }
        public string ClosedDate { get; set; }
        public string CurrencyNo { get; set; }
        public string CurrencySt { get; set; }
        public string Money21 { get; set; }
        public string Money22 { get; set; }
        public string BillRemark { get; set; }
        public string Linage { get; set; }
        public string AccNo { get; set; }
        public string AccName { get; set; }
        public string AccRemark { get; set; }
        public string DCTypeNo { get; set; }
        public string Money1 { get; set; }
    }
}