using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NES_WEB_ACC.ViewModels
{
    public class PersonalRolesListViewModel
    {
        public Int64 EmpId { get; set; }
        public string EmpNo { get; set; }
        public string EmpNameC { get; set; }
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public Boolean PersnalRoleStatus { get; set; }
    }
}