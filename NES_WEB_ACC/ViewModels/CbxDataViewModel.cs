using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NES_WEB_ACC.ViewModels
{
    public class CbxDataViewModel
    {
        public List<ListViewModel> CurrencyNo { get; set; } //交易幣別
        public List<ListViewModel> CurrencySt { get; set; } //記帳幣別
    }
}