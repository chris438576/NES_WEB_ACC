using Dapper;
using Microsoft.Reporting.WebForms;
using NES_WEB_ACC.ViewModels;
using NES_WEB_ACC.ViewModels.ForM;
using NES_WEB_ACC.ViewModels.RDLC;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;


namespace NES_WEB_ACC.Controllers
{
    public class ReportViewController : Controller
    {
        public string connectionString = ConfigurationManager.ConnectionStrings["NES_WEB_ACCConnectionString"].ConnectionString;
        private NES_WEB_ACCEntities _dbContext = new NES_WEB_ACCEntities();

        //------ 介面 ------// 
        /// <summary>
        /// 日記帳
        /// </summary>
        /// <returns></returns>
        public ActionResult Journal()
        {
            var result = GetCbxData("journal");
            var model = result.Data as CbxDataViewModel;
            return View(model);
        }
        /// <summary>
        /// 傳票報表
        /// </summary>
        /// <returns></returns>
        public ActionResult Voucher()
        {
            var result = GetCbxData("voucher");
            var model = result.Data as CbxDataViewModel;          
            
            return View(model);
        }
        /// <summary>
        /// 資產負債表
        /// </summary>
        /// <returns></returns>
        public ActionResult BalanceSheet()
        {
            var result = GetCbxData("balancesheet");
            var model = result.Data as CbxDataViewModel;
            return View(model);
        }
        /// <summary>
        /// 損益表
        /// </summary>
        /// <returns></returns>
        public ActionResult IncomeStatement()
        {
            var result = GetCbxData("incomestatement");
            var model = result.Data as CbxDataViewModel;
            return View(model);
        }

        //------ 其他 ------// 
        public string SqlSeache(SearchDateViewModel searche )
        {
            string sqlSeache = string.Empty;
            if (string.IsNullOrEmpty(searche.LogicalOperator) || string.IsNullOrEmpty(searche.ColumnName) 
                || string.IsNullOrEmpty(searche.ComparisonOperator) || string.IsNullOrEmpty(searche.ColumnValue))
            {
                sqlSeache = string.Empty;
            }
            else
            {
                sqlSeache = $" {searche.LogicalOperator} {searche.ColumnName} {searche.ComparisonOperator} '{searche.ColumnValue}'";
            }
            
            return sqlSeache;
        }
        public JsonResult GetCbxData(string type)
        {
            List<ListViewModel> columnName = null;
            switch (type)
            {
                case "journal":
                    columnName = new List<ListViewModel>
                    {
                        new ListViewModel { Id = "BillDate", Name = "傳票日期" },
                        new ListViewModel { Id = "BillNo", Name = "傳票編號" }
                    };
                    break;
                case "voucher":
                    columnName = new List<ListViewModel>
                    {
                        new ListViewModel { Id = "BillDate", Name = "傳票日期" },
                        new ListViewModel { Id = "BillNo", Name = "傳票編號" }
                    };
                    break;
                case "balancesheet":
                    columnName = new List<ListViewModel>
                    {
                        new ListViewModel { Id = "BillDate", Name = "傳票日期" },
                        new ListViewModel { Id = "BillNo", Name = "傳票編號" }
                    };
                    break;
                case "incomestatement":
                    columnName = new List<ListViewModel>
                    {
                        new ListViewModel { Id = "BillDate", Name = "傳票日期" },
                        new ListViewModel { Id = "BillNo", Name = "傳票編號" }
                    };
                    break;
                default:
                    break;
            }

            var cbxdata = new CbxDataViewModel
            {
                LogicalOperator = new List<ListViewModel>
                {
                    new ListViewModel { Id = "AND", Name = "AND" },
                    new ListViewModel { Id = "OR", Name = "OR" }
                },
                ColumnName = columnName,
                ComparisonOperator = new List<ListViewModel>
                {
                    new ListViewModel { Id = "=", Name = "=" },
                    new ListViewModel { Id = ">", Name = ">" },
                    new ListViewModel { Id = "<", Name = "<" },                    
                    new ListViewModel { Id = ">=", Name = ">=" },
                    new ListViewModel { Id = "<=", Name = "<=" },
                    new ListViewModel { Id = "!=", Name = "!=" },
                    new ListViewModel { Id = "like", Name = "like" },
                }
            };


            return Json(cbxdata, JsonRequestBehavior.AllowGet);
        }
        
        //------ Data to ASPX ------// 
        /// <summary>
        /// 傳票單據報表       
        /// </summary>
        /// <param name="reportId"></param>
        /// <returns></returns>
        public ActionResult VoucherBillReport(string reportId)
        {
            string EmpNo = (string)Session["EmpNo"];
            string sqlInfo = @"
                select 
	                a.BillNo
	                ,CONVERT(varchar(100), a.BillDate, 111) as BillDate
	                ,a.EmpNo as CreateBy ,a.EmpNameC as CreateByName	, a.CreateDate
	                ,a.CheckBy			, d.EmpNameC as CheckByName		, a.CheckDate
	                ,a.StateBy			, b.EmpNameC as StateByName		, a.StateDate
	                ,a.ClosedBy			, c.EmpNameC as ClosedByName	, a.ClosedDate
	                , a.CurrencyNo, a.CurrencySt ,a.Money21,a.Money22, a.Remark 
                from NES_WEB_ACC.dbo.ACC_VoucherInfo as a
                left join NES_WEB.dbo.NES_EmployeeInfo as b on b.EmpNo COLLATE Chinese_Taiwan_Stroke_CI_AS = a.StateBy 
                left join NES_WEB.dbo.NES_EmployeeInfo as c on c.EmpNo COLLATE Chinese_Taiwan_Stroke_CI_AS = a.ClosedBy
                left join NES_WEB.dbo.NES_EmployeeInfo as d on d.EmpNo COLLATE Chinese_Taiwan_Stroke_CI_AS = a.CheckBy
                where WebId = @reportId
            ";
            string sqlDetail = @"
                select Linage, AccNo, AccNameC as AccName,Remark,DCTypeNo, CurrencyNo, Money, Rate1, CurrencySt, Money1  from NES_WEB_ACC.dbo.ACC_VoucherDetail where WebDocId = @reportId order by Linage
            ";

            VoucherMainReportViewModel voucherInfo;
            List<ACC_VoucherDetail_ViewModel> voucherDetail = new List<ACC_VoucherDetail_ViewModel>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                voucherInfo = conn.QueryFirstOrDefault<VoucherMainReportViewModel>(sqlInfo, new { reportId });
                voucherDetail = conn.Query<ACC_VoucherDetail_ViewModel>(sqlDetail, new { reportId }).ToList();
            }

            // 報表參數設定
            var reportParameters = new List<ReportParameter>
            {
                new ReportParameter("ReportMaker", EmpNo),
                new ReportParameter("ReportId", reportId)
            };
            var reportDataSources = new Dictionary<string, ReportDataSource>
            {
                { "VoucherBillMain", new ReportDataSource("VoucherBillMain", new List<VoucherMainReportViewModel> { voucherInfo }) },
                { "VoucherBillItem", new ReportDataSource("VoucherBillItem", voucherDetail) }
            };

            // Session設定，給ReportViewer使用
            Session["ReportPath"] = Server.MapPath("~/Report/RDLC/VoucherBillReport.rdlc");
            Session["ReportDataSources"] = reportDataSources;
            Session["ReportParameters"] = reportParameters;
            Session["ReportDocName"] = "VoucherBill_" + voucherInfo.BillNo;

            return Redirect("~/Report/ReportViewer_V2.aspx");
        }
        /// <summary>
        /// 傳票報表
        /// </summary>
        /// <param name="sqlsearches"></param>
        /// <returns></returns>
        public ActionResult VoucherReport(string sqlSeache)
        {          
            string EmpNo = (string)Session["EmpNo"];
            string sqlInfo = $@"
				select * from (
					select 
						a.WebId,a.BillNo
						,CONVERT(varchar(100), a.BillDate, 111) as BillDate
						,a.EmpNo as CreateBy ,a.EmpNameC as CreateByName	, a.CreateDate
						,a.CheckBy			, d.EmpNameC as CheckByName		, a.CheckDate
						,a.StateBy			, b.EmpNameC as StateByName		, a.StateDate
						,a.ClosedBy			, c.EmpNameC as ClosedByName	, a.ClosedDate
						, a.CurrencyNo, a.CurrencySt ,a.Money21,a.Money22, a.Remark 
					from NES_WEB_ACC.dbo.ACC_VoucherInfo as a
					left join NES_WEB.dbo.NES_EmployeeInfo as b on b.EmpNo COLLATE Chinese_Taiwan_Stroke_CI_AS = a.StateBy 
					left join NES_WEB.dbo.NES_EmployeeInfo as c on c.EmpNo COLLATE Chinese_Taiwan_Stroke_CI_AS = a.ClosedBy
					left join NES_WEB.dbo.NES_EmployeeInfo as d on d.EmpNo COLLATE Chinese_Taiwan_Stroke_CI_AS = a.CheckBy
					where 1=1
						and  a.IsClosed = 1
				) as a
				where 1=1
                {sqlSeache}
            ";
            string sqlDetail = $@"
                IF OBJECT_ID ('tempdb..#Temp') IS NOT NULL
		            Drop Table #Temp
                select WebId into #Tmp from NES_WEB_ACC.dbo.ACC_VoucherInfo  where 1=1 {sqlSeache}           

                select WebDocId, Linage, AccNo, AccNameC as AccName,Remark,DCTypeNo, CurrencyNo, Money, Rate1, CurrencySt, Money1  from NES_WEB_ACC.dbo.ACC_VoucherDetail 
                where WebDocId in    (select * from #Tmp)  
                order by Linage

                IF OBJECT_ID ('tempdb..#Temp') IS NOT NULL
		            Drop Table #Temp
            ";            

            List<VoucherMainReportViewModel> voucherInfo = new List<VoucherMainReportViewModel>();
            List<ACC_VoucherDetail_ViewModel> voucherDetail = new List<ACC_VoucherDetail_ViewModel>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                voucherInfo = conn.Query<VoucherMainReportViewModel>(sqlInfo).ToList();
                voucherDetail = conn.Query<ACC_VoucherDetail_ViewModel>(sqlDetail).ToList();
            }

            // 報表參數設定
            var reportParameters = new List<ReportParameter>
            {
                new ReportParameter("ReportMaker", EmpNo),
                //new ReportParameter("ReportId", reportId)
            };
           
            // Session設定，給ReportViewer使用
            Session["ReportPath"] = Server.MapPath("~/Report/RDLC/VoucherReportMain.rdlc");
            Session["ReportDataSourceMain"] = new ReportDataSource("VoucherBillMain", voucherInfo);
            Session["ReportDataSourceItem"] = new ReportDataSource("VoucherBillItem", voucherDetail);
            Session["ReportParameters"] = reportParameters;
            Session["ReportDocName"] = "VoucherReport_" + System.DateTime.Now.ToString("yyyyMMdd");

            
            return Redirect("~/Report/ReportViewer_V3.aspx");
        }
               
    }
}