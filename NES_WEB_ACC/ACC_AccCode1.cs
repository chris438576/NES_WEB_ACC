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
    
    public partial class ACC_AccCode1
    {
        public long Id { get; set; }
        public Nullable<long> CompId { get; set; }
        public string CompNo { get; set; }
        public string CompAbbr { get; set; }
        public string AccGroupNo { get; set; }
        public string AccGroupNameC { get; set; }
        public string AccGroupNameE { get; set; }
        public string Remark { get; set; }
        public Nullable<int> SourceType { get; set; }
        public Nullable<int> Flag { get; set; }
        public Nullable<bool> IsState { get; set; }
        public Nullable<System.DateTime> StateDate { get; set; }
        public string StateBy { get; set; }
        public Nullable<bool> IsChecked { get; set; }
        public Nullable<System.DateTime> CheckDate { get; set; }
        public string CheckBy { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public string CreateBy { get; set; }
    }
}