using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NES_WEB_ACC.ViewModels.RDLC
{
    public class VoucherMainReportViewModel
    {
        public string BillNo { get; set; }
        public DateTime BillDate { get; set; }
        public string CreateBy { get; set; }
        public string CreateByName { get; set; }
        public DateTime CreateDate { get; set; }
        public string CheckBy { get; set; }
        public string CheckByName { get; set; }
        public DateTime CheckDate { get; set; }
        public string StateBy { get; set; }
        public string StateByName { get; set; }
        public DateTime StateDate { get; set; }
        public string ClosedBy { get; set; }
        public string ClosedByName { get; set; }
        public DateTime ClosedDate { get; set; }
        public string CurrencyNo { get; set; }
        public string CurrencySt { get; set; }
        public Decimal Money21 { get; set; }
        public Decimal Money22 { get; set; }
        public string Remark { get; set; }
    }
}