﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NES_WEB_ACC.ViewModels.RDLC
{
    public class BalanceSheetReportViewModel
    {
        public Int16 AccGroupNo { get; set; }
        public string AccGroupName { get; set; }
        public string AccControlNo { get; set; }
        public string AccControlName { get; set; }
        public string AccNo { get; set; }
        public string AccName { get; set; }
        public Decimal TotalMoney { get; set; }
    }
}