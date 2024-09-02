using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NES_WEB_ACC.ViewModels.RDLC
{
    public class SubsidiaryLedgerViewModel
    {
        public Int16 AccGroupNo { get; set; }
        public string AccNo { get; set; }
        public string AccName { get; set; }
        public string DCTypeNo { get; set; }
        public Decimal BeginMoney { get; set; }
        public DateTime BillDate { get; set; }
        public string BillNo { get; set; }
        public string BillMonth { get; set; }
        public string Remark { get; set; }
        public string BillDCTypeNo { get; set; }
        public Decimal Money1 { get; set; }
        public Decimal BillLast { get; set; }      
    }
}