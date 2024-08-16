using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NES_WEB_ACC.ViewModels.ForM
{
    public class ACC_VoucherDetail_ViewModel
    {
        public System.Guid WebId { get; set; }
        public string Id { get; set; }
        public System.Guid WebDocId { get; set; }
        public string DocId { get; set; }
        public Nullable<int> Linage { get; set; }
        public Nullable<System.Guid> AccNoWebId { get; set; }
        public string AccNoId { get; set; }
        public string AccNo { get; set; }
        public string AccName { get; set; } //多語系       
        public string Remark { get; set; }
        public string DCTypeNo { get; set; }
        public string DCTypeNameC { get; set; }
        public string DCTypeNameMX { get; set; }
        public string CurrencyNo { get; set; }
        public string CurrencySt { get; set; }
        public Nullable<decimal> Rate1 { get; set; }
        public Nullable<decimal> Rate2 { get; set; }
        public Nullable<decimal> Money { get; set; }
        public Nullable<decimal> Money1 { get; set; }
        public Nullable<decimal> Money2 { get; set; }
        public string AccProfitId { get; set; }
        public string AccProfitNo { get; set; }
        public string AccProfitName { get; set; }
        public string AccDeptId { get; set; }
        public string AccDeptNo { get; set; }
        public string AccDeptName { get; set; }
        public string PayDeptId { get; set; }
        public string PayDeptNo { get; set; }
        public string PayDeptName { get; set; }
        public string TargetType { get; set; }
        public string TargetId { get; set; }
        public string TargetNo { get; set; }
        public string TargetAbbr { get; set; }
        public string OffsetNo { get; set; }
        public string CaseBillId { get; set; }
        public string CaseBillNo { get; set; }
        public string SourceProjectId { get; set; }
        public string SourceDocSubType { get; set; }
        public string SourceDocSubTypeName { get; set; }
        public string SourceDocId { get; set; }
        public string SourceSeqId { get; set; }
        public string SourceNo { get; set; }
        public string InitialProjectId { get; set; }
        public string InitialDocSubType { get; set; }
        public string InitialDocSubTypeName { get; set; }
        public string InitialDocId { get; set; }
        public string InitialNo { get; set; }
        public string CheckType { get; set; }
        public string CheckId { get; set; }
        public string CheckNo { get; set; }
        public string EventType { get; set; }
        public string ActivityType { get; set; }
        public Nullable<bool> IsParty { get; set; }
        public Nullable<int> BillAddType { get; set; }
        public Nullable<int> Flag { get; set; }
        public Nullable<bool> IsState { get; set; }
        public Nullable<System.DateTime> StateDate { get; set; }
        public string StateBy { get; set; }
        public Nullable<bool> IsChecked { get; set; }
        public Nullable<System.DateTime> CheckDate { get; set; }
        public string CheckBy { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public string CreateBy { get; set; }
        public Nullable<int> ShowPage { get; set; }
    }
}