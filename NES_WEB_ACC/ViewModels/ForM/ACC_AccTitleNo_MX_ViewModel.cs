using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NES_WEB_ACC.ViewModels.ForM
{
    public class ACC_AccTitleNo_MX_ViewModel
    {
        public System.Guid WebId { get; set; }
        public string Id { get; set; }
        public string CompId { get; set; }
        public string CompNo { get; set; }
        public string CompAbbr { get; set; }
        public string AccGroupNoId { get; set; }
        public string AccGroupNo { get; set; }
        public string AccGroupNameC { get; set; }
        public string AccGroupNameE { get; set; }
        public string AccGroupNameMX { get; set; }
        public string AccNoById { get; set; }
        public string AccNoBy { get; set; }
        public string AccNoByNameC { get; set; }
        public string AccNoByNameE { get; set; }
        public string AccNoByNameMX { get; set; }
        public string AccNo { get; set; }
        public string AccName { get; set; }   //多語系取值    
        public string DCTypeNo { get; set; }
        public string DCTypeNameC { get; set; }
        public Nullable<bool> AccCode { get; set; }
        public Nullable<bool> IsControl { get; set; }
        public Nullable<bool> IsCancel { get; set; }
        public Nullable<bool> IsCash { get; set; }
        public Nullable<bool> IsParty { get; set; }
        public Nullable<bool> IsCurrency { get; set; }
        public Nullable<bool> IsTarget { get; set; }
        public Nullable<bool> IsDept { get; set; }
        public Nullable<bool> IsOffset { get; set; }
        public Nullable<bool> IsProject { get; set; }
        public string AccNoRemark { get; set; }
        public Nullable<bool> Status { get; set; }
        public Nullable<System.DateTime> StatuDate { get; set; }
        public string StatusBy { get; set; }
        public Nullable<int> Flag { get; set; }
        public Nullable<bool> IsState { get; set; }
        public Nullable<System.DateTime> StateDate { get; set; }
        public string StateBy { get; set; }
        public Nullable<bool> IsChecked { get; set; }
        public Nullable<System.DateTime> CheckDate { get; set; }
        public string CheckBy { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public string CreateBy { get; set; }
        public string AccSetting { get; set; }
    }
}