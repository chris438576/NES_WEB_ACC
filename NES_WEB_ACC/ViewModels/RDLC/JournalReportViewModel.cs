using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NES_WEB_ACC.ViewModels.RDLC
{
    public class JournalReportViewModel
    {
        public DateTime BillDate { get; set; }
        public string BillNo { get; set; }
        public string CurrencySt { get; set; }
        public bool IsChecked { get; set; }
        public bool IsState { get; set; }
        public bool IsClosed { get; set; }
        public string Linage { get; set; }
        public string AccNo { get; set; }
        public string AccName { get; set; }
        public string Remark { get; set; }
        public string DCTypeNo { get; set; }
        public decimal Money1 { get; set; }
    }
}