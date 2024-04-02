using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NES_WEB_ACC.ViewModels
{
    public class RolesListViewModel
    {
        public Guid RoleId { get; set; }
        public string RoleName { get; set; }
        public Boolean? RoleStatus { get; set; }
       
    }     
}