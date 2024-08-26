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
        public List<ListViewModel> LogicalOperator { get; set; } //條件_判斷
        public List<ListViewModel> ColumnName { get; set; } //條件_欄位
        public List<ListViewModel> ComparisonOperator { get; set; } //條件_比對
    }
}