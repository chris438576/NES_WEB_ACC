using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NES_WEB_ACC.ViewModels
{
    public class SearchDateViewModel
    {
        public string LogicalOperator { get; set; }
        public string ColumnName { get; set; }
        public string ComparisonOperator { get; set; }
        public string ColumnValue { get; set; }       
    }
}