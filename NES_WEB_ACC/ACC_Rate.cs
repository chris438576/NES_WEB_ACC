//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace NES_WEB_ACC
{
    using System;
    using System.Collections.Generic;
    
    public partial class ACC_Rate
    {
        public System.Guid WebId { get; set; }
        public string CurrencyNo { get; set; }
        public string CurrencySt { get; set; }
        public int ExchangeYear { get; set; }
        public int ExchangeMonth { get; set; }
        public string ExchangeMonthFlag { get; set; }
        public decimal Rate { get; set; }
        public string CreateEmpId { get; set; }
        public string CreateEmpNo { get; set; }
        public Nullable<System.DateTime> CreatEmpDate { get; set; }
        public string UpdateEmpId { get; set; }
        public string UpdateEmpNo { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
    }
}