using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NES_WEB_ACC.ViewModels
{
    public class UsersListViewModel
    {
        public string EmpId { get; set; }
        public string EmpNo { get; set; }
        public string EmpNameC { get; set; }
        public string RoleName { get; set; }
        public string DeptNo { get; set; }
        public string DeptName { get; set; }
        public Boolean Status { get; set; }
    }
}