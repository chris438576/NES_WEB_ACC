using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NES_WEB_ACC.ViewModels
{
    public class HomeBillInfoViewModel
    {
        public int Total { get; set; }
        public int Uncheck { get; set; }
        public int UnRecheck { get; set; }
        public int Unclose { get; set; }
    }
}