using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NES_WEB_ACC.ViewModels
{
    public class VoucherCreateViewModel
    {
        public string Id { get; set; }
        public string Flag { get; set; }
        public string BillDate { get; set; }
        public string BillNo { get; set; }
        public string VoucherNameC { get; set; }
        public string EmpNo { get; set; }
        public string EmpNameC { get; set; }
        public string Remark { get; set; }
        public string CompNo { get; set; }
        public string CompAbbr { get; set; }        
    }
}