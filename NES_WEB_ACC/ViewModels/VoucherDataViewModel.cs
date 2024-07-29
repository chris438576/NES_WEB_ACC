using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NES_WEB_ACC.ViewModels
{
    public class MainData
    {
        public string BillDate { get; set; }
        public string DocSubType { get; set; }
        public string DocSubTypeName { get; set; }
        public string CompId { get; set; }
        public string CompNo { get; set; }
        public string CompAbbr { get; set; }
        public string VoucherType { get; set; }
        public string VoucherNameC { get; set; }
        public string EmpNo { get; set; }
        public string EmpNameC { get; set; }
        public string DeptNo { get; set; }
        public string DeptName { get; set; }
        public string BillNo { get; set; }
        public string CurrencyNo { get; set; }
        public string CurrencyTw1 { get; set; }
        public string Rate1 { get; set; }
        public string CurrencyTw2 { get; set; }
        public string Rate2 { get; set; }
        public string TargetNo { get; set; }
    }
    public class InfoData
    {
        public string Id { get; set; }
        public string Linage { get; set; }
        public string AccNo { get; set; }
        public string AccNameC { get; set; }
        public string DCTypeNo { get; set; }
        public string DCTypeNameC { get; set; }
        public string CurrencyNo { get; set; }
        public string CurrencyNoNote { get; set; }
        public string Rate1 { get; set; }
        public decimal Money { get; set; }
        public decimal Money1 { get; set; }
        public string TargetId { get; set; }
        public string TargetType { get; set; }
        public string TargetNo { get; set; }
        public string TargetAbbr { get; set; }
        public string AccProfitId { get; set; }
        public string AccProfitNo { get; set; }
        public string AccProfitName { get; set; }
        public string AccDeptId { get; set; }
        public string AccDeptNo { get; set; }
        public string AccDeptName { get; set; }
        public string PayDeptId { get; set; }
        public string PayDeptNo { get; set; }
        public string PayDeptName { get; set; }
        public string CheckType { get; set; }
        public string CheckId { get; set; }
        public string CheckNo { get; set; }
        public string OffsetNo { get; set; }
        public string SourceDocId { get; set; }
        public string SourceSeqId { get; set; }
        public string SourceDocSubTypeName { get; set; }
        public string SourceNo { get; set; }
        public string InitialProjectId { get; set; }
        public string InitialDocSubType { get; set; }
        public string InitialDocId { get; set; }
        public string InitialDocSubTypeName { get; set; }
        public string InitialNo { get; set; }
        public string ActivityType { get; set; }
        public string Remark { get; set; }
        public string CaseBillNo { get; set; }
    }
    public class VoucherDataViewModel
    {
        public MainData Maindata { get; set; }
        public List<InfoData> Infodata { get; set; }
    }
}