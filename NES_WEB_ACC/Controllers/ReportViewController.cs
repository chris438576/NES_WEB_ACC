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
using System.Threading;
using System.Web.Mvc;


namespace NES_WEB_ACC.Controllers
{
    public class ReportViewController : Controller
    {
        public string connectionString = ConfigurationManager.ConnectionStrings["NES_WEB_ACCConnectionString"].ConnectionString;
        private NES_WEB_ACCEntities _dbContext = new NES_WEB_ACCEntities();
        private string currentCulture = Thread.CurrentThread.CurrentCulture.Name;
        //------ 介面 ------// 
        /// <summary>
        /// 日記帳
        /// </summary>
        /// <returns></returns>
        public ActionResult Journal()
        {
            var result = GetCbxData("journal");
            var model = result.Data as CbxDataViewModel;
            ViewBag.CurrentCulture = currentCulture;
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
            ViewBag.CurrentCulture = currentCulture;
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
            ViewBag.CurrentCulture = currentCulture;
            return View(model);
        }
        /// <summary>
        /// 資產負債表-帳戶式
        /// </summary>
        /// <returns></returns>
        public ActionResult BalanceSheetAcc()
        {
            var result = GetCbxData("balancesheetacc");
            var model = result.Data as CbxDataViewModel;
            ViewBag.CurrentCulture = currentCulture;
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
            ViewBag.CurrentCulture = currentCulture;
            return View(model);
        }
        /// <summary>
        /// 明細分類帳
        /// </summary>
        /// <returns></returns>
        public ActionResult SubsidiaryLedger()
        {
            return View();
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
                case "balancesheetacc":
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
                select 
                    Linage, AccNo
                    ,case
                        when @currentCulture = 'en' then a.AccNameE
                        when @currentCulture = 'zh-TW' then a.AccNameC
                        when @currentCulture = 'es-MX' then a.AccNameMX
                        else a.AccNameE
                    end as AccName
                    ,Remark,DCTypeNo
                    ,CurrencyNo, Money, Rate1
                    ,CurrencySt, Money1  
                from NES_WEB_ACC.dbo.ACC_VoucherDetail 
                where WebDocId = @reportId order by Linage
            ";

            VoucherMainReportViewModel voucherInfo;
            List<ACC_VoucherDetail_ViewModel> voucherDetail = new List<ACC_VoucherDetail_ViewModel>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                voucherInfo = conn.QueryFirstOrDefault<VoucherMainReportViewModel>(sqlInfo, new { currentCulture, reportId });
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
        public ActionResult VoucherReport(string sqlseache)
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
                {sqlseache}
            ";
            string sqlDetail = $@"
                IF OBJECT_ID ('tempdb..#Temp') IS NOT NULL
		            Drop Table #Temp
                select WebId into #Tmp from NES_WEB_ACC.dbo.ACC_VoucherInfo  where 1=1 {sqlseache}           

                select 
                    WebDocId,Linage,AccNo
                    ,case
                        when @currentCulture = 'en' then AccNameE
                        when @currentCulture = 'zh-TW' then AccNameC
                        when @currentCulture = 'es-MX' then AccNameMX
                        else AccNameE
                    end as AccName
                    ,Remark,DCTypeNo
                    ,CurrencyNo
                    ,Money,Rate1
                    ,CurrencySt,Money1  
                from NES_WEB_ACC.dbo.ACC_VoucherDetail 
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
                voucherDetail = conn.Query<ACC_VoucherDetail_ViewModel>(sqlDetail,new { currentCulture }).ToList();
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
        /// <summary>
        /// 日記帳
        /// </summary>
        /// <param name="sqlSeache"></param>
        /// <returns></returns>
        public ActionResult JournalReport(string sqlseache, string startdate, string enddate)
        {
            string EmpNo = (string)Session["EmpNo"];
            string sql = @"
                select  
                 CONVERT(varchar(100), a.BillDate, 111) as BillDate
                 ,a.BillNo,a.CurrencySt,a.IsChecked,a.IsState,a.IsClosed
                 ,b.Linage,b.AccNo                
                 ,case
                    when @currentCulture = 'en' then b.AccNameE
                    when @currentCulture = 'zh-TW' then b.AccNameC
                    when @currentCulture = 'es-MX' then b.AccNameMX
                    else b.AccNameE
                 end as AccName
                 ,b.Remark,b.DCTypeNo,b.Money1

                from NES_WEB_ACC.dbo.ACC_VoucherDetail as b
                left join NES_WEB_ACC.dbo.ACC_VoucherInfo as a on a.WebId = b.WebDocId
                where 1=1
                 and a.IsChecked = 1
                 and a.IsState = 1
                 and a.IsClosed = 1
                and a.BillDate >= @startdate
                and a.BillDate <= @enddate
            ";
            var param = new { currentCulture,startdate, enddate };
            List<JournalReportViewModel> journalReport = new List<JournalReportViewModel>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                journalReport = conn.Query<JournalReportViewModel>(sql, param).ToList();
            }

            // 報表參數設定
            var reportParameters = new List<ReportParameter>
            {
                new ReportParameter("ReportMaker", EmpNo),
                new ReportParameter("StartDate", startdate),
                new ReportParameter("EndDate", enddate),
            };

            // Session設定，給ReportViewer使用
            Session["ReportPath"] = Server.MapPath("~/Report/RDLC/JournalReport.rdlc");            
            Session["ReportDataSource"] = new ReportDataSource("JournalRdlc", journalReport);           
            Session["ReportParameters"] = reportParameters;
            Session["ReportDocName"] = "JournalReport_" + System.DateTime.Now.ToString("yyyyMMdd");

            return Redirect("~/Report/ReportViewer_V1.aspx");
        }       
        /// <summary>
        /// 資產負債表
        /// </summary>
        /// <param name="sqlseache"></param>
        /// <param name="startdate"></param>
        /// <param name="enddate"></param>
        /// <returns></returns>
        public ActionResult BalanceSheetReport(string sqlseache, string startdate, string enddate)
        {
            string EmpNo = (string)Session["EmpNo"];
            string rdlcPath, rdlcDocName;
            switch (currentCulture)
            {
                case "en":
                    rdlcPath = "~/Report/RDLC/BalanceSheetReport_en.rdlc";
                    rdlcDocName = "BalanceSheetReport_" + System.DateTime.Now.ToString("yyyyMMdd"); ;
                    break;
                case "zh-TW":
                    rdlcPath = "~/Report/RDLC/BalanceSheetReport_zh-TW.rdlc";
                    rdlcDocName = "資產負債表_" + System.DateTime.Now.ToString("yyyyMMdd"); 
                    break;
                case "es-MX":
                    rdlcPath = "~/Report/RDLC/BalanceSheetReport_es-MX.rdlc";
                    rdlcDocName = "Informe de balance general_" + System.DateTime.Now.ToString("yyyyMMdd");
                    break;
                default:
                    rdlcPath = "~/Report/RDLC/BalanceSheetReport_en.rdlc";
                    rdlcDocName = "BalanceSheetReport_" + System.DateTime.Now.ToString("yyyyMMdd"); ;
                    break;
            }
            string sql = @"
               	IF OBJECT_ID ('tempdb..#TmpAcc') IS NOT NULL
		            Drop Table #TmpAcc
	            IF OBJECT_ID ('tempdb..#TmpVoucher') IS NOT NULL
		            Drop Table #TmpVoucher

                select * into #TmpAcc from (
	                select
		                a.AccGroupNo                        
                        ,case
                            when @currentCulture = 'en' then a.AccGroupNameE 
                            when @currentCulture = 'zh-TW' then a.AccGroupNameC 
                            when @currentCulture = 'es-MX' then a.AccGroupNameMX 
                            else a.AccGroupNameE 
                         end as AccGroupName
		                ,b.AccNo as AccControlNo                        
                        ,case
                            when @currentCulture = 'en' then b.AccNameE
                            when @currentCulture = 'zh-TW' then b.AccNameC
                            when @currentCulture = 'es-MX' then b.AccNameMX
                            else b.AccNameE
                         end as AccControlName
		                ,a.AccNo                       
                        ,case
                            when @currentCulture = 'en' then a.AccNameE
                            when @currentCulture = 'zh-TW' then a.AccNameC
                            when @currentCulture = 'es-MX' then a.AccNameMX
                            else a.AccNameE
                         end as AccName
                        
	                from NES_WEB_ACC.dbo.ACC_AccTitleNo_MX as a
	                left join NES_WEB_ACC.dbo.ACC_AccTitleNo_MX  as b on CONCAT(LEFT(a.AccNo, 3), '-00-000') = b.AccNo
	                where 1 = 1
		                and a.AccNo not like '%00-00-000'
		                and a.AccGroupNo in (1,2,3)
                ) as a

                /*** 串ACC_AccTitleNo_MX並且取該會科的借貸方向 ***/
                /*** 並依照借貸方向計算，如果是借方科目金額=借方總額-貸方總額；貸方科目金額=貸方總額-借方總額 ***/
                SELECT * into #TmpVoucher from (
	                select
		                a.[AccNo]
		                ,SUM(
                        CASE	
                            WHEN b.DCTypeNo = 'D' THEN
                                CASE 
                                    WHEN a.[DCTypeNo] = 'D' THEN [Money1]
                                    WHEN a.[DCTypeNo] = 'C' THEN -[Money1]
                                    ELSE 0
                                END
                            WHEN b.DCTypeNo = 'C' THEN
                                CASE 
                                    WHEN a.[DCTypeNo] = 'D' THEN -[Money1]
                                    WHEN a.[DCTypeNo] = 'C' THEN [Money1]
                                    ELSE 0
                                END
                            ELSE 0
                        END
                    ) AS TotalMoney
	                FROM 
		                NES_WEB_ACC.dbo.ACC_VoucherDetail as a	
                        left join NES_WEB_ACC.dbo.ACC_VoucherInfo as c on c.WebId = a.WebDocId
		                left join NES_WEB_ACC.dbo.ACC_AccTitleNo_MX as b on a.AccNo = b.AccNo 
                    where 1=1
                        and c.IsClosed = '1' --已結案   
                        and c.BillDate >= @startdate
                        and c.BillDate <= @enddate
	                GROUP BY 
		                a.[AccNo]
	                ) as a

                select a.*,ISNULL(b.TotalMoney,0) as TotalMoney from #TmpAcc as a
                left join #TmpVoucher as b on a.AccNo = b.AccNo
                order by a.AccGroupNo ,a.AccControlNo

	                IF OBJECT_ID ('tempdb..#TmpAcc') IS NOT NULL
		                Drop Table #TmpAcc
	                IF OBJECT_ID ('tempdb..#TmpVoucher') IS NOT NULL
		                Drop Table #TmpVoucher
            ";
            var param = new { currentCulture,startdate, enddate };
            List<BalanceSheetReportViewModel> balanceSheetReport = new List<BalanceSheetReportViewModel>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                balanceSheetReport = conn.Query<BalanceSheetReportViewModel>(sql, param).ToList();
            }

            // 報表參數設定
            var reportParameters = new List<ReportParameter>
            {
                new ReportParameter("ReportMaker", EmpNo),
                new ReportParameter("StartDate", startdate),
                new ReportParameter("EndDate", enddate),
                //new ReportParameter("StartDate", "2024/08/01"),
                //new ReportParameter("EndDate", "2024/08/31"),
            };

            // Session設定，給ReportViewer使用            
            Session["ReportPath"] = Server.MapPath(rdlcPath);
            Session["ReportDataSource"] = new ReportDataSource("BalanceSheetRdlc", balanceSheetReport);
            Session["ReportParameters"] = reportParameters;
            Session["ReportDocName"] = rdlcDocName;

            return Redirect("~/Report/ReportViewer_V1.aspx");
        }
        /// <summary>
       /// 資產負債表-帳戶式
       /// </summary>
       /// <param name="sqlseache"></param>
       /// <param name="startdate"></param>
       /// <param name="enddate"></param>
       /// <returns></returns>
        public ActionResult BalanceSheetAccReport(string sqlseache, string startdate, string enddate)
        {
            string EmpNo = (string)Session["EmpNo"];
            string rdlcPath, rdlcDocName;
            switch (currentCulture)
            {
                case "en":
                    rdlcPath = "~/Report/RDLC/BalanceSheetAccReport_en.rdlc";
                    rdlcDocName = "BalanceSheetAccReport_" + System.DateTime.Now.ToString("yyyyMMdd"); ;
                    break;
                case "zh-TW":
                    rdlcPath = "~/Report/RDLC/BalanceSheetAccReport_zh-TW.rdlc";
                    rdlcDocName = "資產負債表-帳戶式_" + System.DateTime.Now.ToString("yyyyMMdd");
                    break;
                case "es-MX":
                    rdlcPath = "~/Report/RDLC/BalanceSheetAccReport_es-MX.rdlc";
                    rdlcDocName = "Cuentas del balance general_" + System.DateTime.Now.ToString("yyyyMMdd");
                    break;
                default:
                    rdlcPath = "~/Report/RDLC/BalanceSheetAccReport_en.rdlc";
                    rdlcDocName = "BalanceSheetAccReport_" + System.DateTime.Now.ToString("yyyyMMdd"); ;
                    break;
            }
            string sql = @"
                --declare @currentCulture as nvarchar(50);
                --declare @startdate as nvarchar(10);
                --declare @enddate as nvarchar(10);
                --set @currentCulture='en'
                --set @startdate='2024-09-01'
                --set @enddate='2024-09-30'

                IF OBJECT_ID ('tempdb..#Temp') IS NOT NULL
	                Drop Table #Temp  
                IF OBJECT_ID ('tempdb..#TmpBeforeData') IS NOT NULL
	                Drop Table #TmpBeforeData
                IF OBJECT_ID ('tempdb..#TmpSearchData') IS NOT NULL
	                Drop Table #TmpSearchData


                CREATE TABLE #Temp (
                    AccGroupNo nvarchar(50),
                    AccGroupName NVARCHAR(255),
                    AccControlNo NVARCHAR(50),
                    AccControlName NVARCHAR(255),
                    AccNo NVARCHAR(50),
                    AccName NVARCHAR(255),
	                TotalAmount DECIMAL(18,3),
	                SearchAmount DECIMAL(18,3),
                );

                INSERT INTO #Temp (AccGroupNo, AccGroupName, AccControlNo, AccControlName, AccNo, AccName)
                select
	                a.AccGroupNo                        
                    ,case
                        when @currentCulture = 'en' then a.AccGroupNameE 
                        when @currentCulture = 'zh-TW' then a.AccGroupNameC 
                        when @currentCulture = 'es-MX' then a.AccGroupNameMX 
                        else a.AccGroupNameE 
                        end as AccGroupName
	                ,b.AccNo as AccControlNo                        
                    ,case
                        when @currentCulture = 'en' then b.AccNameE
                        when @currentCulture = 'zh-TW' then b.AccNameC
                        when @currentCulture = 'es-MX' then b.AccNameMX
                        else b.AccNameE
                        end as AccControlName
	                ,a.AccNo                       
                    ,case
                        when @currentCulture = 'en' then a.AccNameE
                        when @currentCulture = 'zh-TW' then a.AccNameC
                        when @currentCulture = 'es-MX' then a.AccNameMX
                        else a.AccNameE
                        end as AccName                        
                from NES_WEB_ACC.dbo.ACC_AccTitleNo_MX as a
                left join NES_WEB_ACC.dbo.ACC_AccTitleNo_MX  as b on CONCAT(LEFT(a.AccNo, 3), '-00-000') = b.AccNo
                where 1 = 1
	                and a.AccNo not like '%00-00-000'
	                and a.AccGroupNo in (1,2,3);

                SELECT * into #TmpBeforeData from (
	                select
		                a.[AccNo]
		                ,SUM(
		                CASE	
			                WHEN b.DCTypeNo = 'D' THEN
				                CASE 
					                WHEN a.[DCTypeNo] = 'D' THEN [Money1]
					                WHEN a.[DCTypeNo] = 'C' THEN -[Money1]
					                ELSE 0
				                END
			                WHEN b.DCTypeNo = 'C' THEN
				                CASE 
					                WHEN a.[DCTypeNo] = 'D' THEN -[Money1]
					                WHEN a.[DCTypeNo] = 'C' THEN [Money1]
					                ELSE 0
				                END
			                ELSE 0
		                END
	                ) AS TotalMoney
	                FROM 
		                NES_WEB_ACC.dbo.ACC_VoucherDetail as a	
		                left join NES_WEB_ACC.dbo.ACC_VoucherInfo as c on c.WebId = a.WebDocId
		                left join NES_WEB_ACC.dbo.ACC_AccTitleNo_MX as b on a.AccNo = b.AccNo 
	                where 1=1
		                and c.IsClosed = '1' --已結案   
		                and c.BillDate < @startdate
                       
	                GROUP BY 
		                a.[AccNo]
                ) as a
                SELECT * into #TmpSearchData from (
	                select
		                a.[AccNo]
		                ,SUM(
		                CASE	
			                WHEN b.DCTypeNo = 'D' THEN
				                CASE 
					                WHEN a.[DCTypeNo] = 'D' THEN [Money1]
					                WHEN a.[DCTypeNo] = 'C' THEN -[Money1]
					                ELSE 0
				                END
			                WHEN b.DCTypeNo = 'C' THEN
				                CASE 
					                WHEN a.[DCTypeNo] = 'D' THEN -[Money1]
					                WHEN a.[DCTypeNo] = 'C' THEN [Money1]
					                ELSE 0
				                END
			                ELSE 0
		                END
	                ) AS TotalMoney
	                FROM 
		                NES_WEB_ACC.dbo.ACC_VoucherDetail as a	
		                left join NES_WEB_ACC.dbo.ACC_VoucherInfo as c on c.WebId = a.WebDocId
		                left join NES_WEB_ACC.dbo.ACC_AccTitleNo_MX as b on a.AccNo = b.AccNo 
	                where 1=1
		                and c.IsClosed = '1' --已結案   
		                and c.BillDate >= @startdate
		                and c.BillDate <= @enddate
	                GROUP BY 
		                a.[AccNo]
                ) as a

                UPDATE T1
                SET 
                    T1.TotalAmount = T2.TotalMoney
	                --T1.SearchAmount = T3.TotalMoney
                FROM #Temp T1
                INNER JOIN #TmpBeforeData T2 ON T1.AccNo COLLATE Chinese_Taiwan_Stroke_CI_AS  = T2.AccNo
                --INNER JOIN #TmpSearchData T3 ON T1.AccNo COLLATE Chinese_Taiwan_Stroke_CI_AS  = T3.AccNo;

                UPDATE T1
                SET 
                    --T1.TotalAmount = T2.TotalMoney
	                T1.SearchAmount = T3.TotalMoney
                FROM #Temp T1
                --INNER JOIN #TmpBeforeData T2 ON T1.AccNo COLLATE Chinese_Taiwan_Stroke_CI_AS  = T2.AccNo
                INNER JOIN #TmpSearchData T3 ON T1.AccNo COLLATE Chinese_Taiwan_Stroke_CI_AS  = T3.AccNo;
                select  
	                AccGroupNo, AccGroupName, AccControlNo, AccControlName, AccNo, AccName 
	                ,case 
		                when TotalAmount is null then 0
		                else TotalAmount
	                end as TotalAmount
	                ,case 
		                when SearchAmount is null then 0
		                else SearchAmount
	                end as SearchAmount
                from #Temp

                IF OBJECT_ID ('tempdb..#Temp') IS NOT NULL
	                Drop Table #Temp  
                IF OBJECT_ID ('tempdb..#TmpBeforeData') IS NOT NULL
	                Drop Table #TmpBeforeData
                IF OBJECT_ID ('tempdb..#TmpSearchData') IS NOT NULL
	                Drop Table #TmpSearchData
            ";
            var param = new { currentCulture, startdate, enddate };
            List<BalanceSheetAccReportViewModel> balanceSheetReport = new List<BalanceSheetAccReportViewModel>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                balanceSheetReport = conn.Query<BalanceSheetAccReportViewModel>(sql, param).ToList();
            }

            // 報表參數設定
            var reportParameters = new List<ReportParameter>
            {
                new ReportParameter("ReportMaker", EmpNo),
                new ReportParameter("StartDate", startdate),
                new ReportParameter("EndDate", enddate),
                //new ReportParameter("StartDate", "2024/08/01"),
                //new ReportParameter("EndDate", "2024/08/31"),
            };

            // Session設定，給ReportViewer使用            
            Session["ReportPath"] = Server.MapPath(rdlcPath);
            Session["ReportDataSource"] = new ReportDataSource("BalanceSheetAccRdlc", balanceSheetReport);
            Session["ReportParameters"] = reportParameters;
            Session["ReportDocName"] = rdlcDocName;

            return Redirect("~/Report/ReportViewer_V1.aspx");
        }
        /// <summary>
        /// 損益表
        /// </summary>
        /// <param name="sqlseache"></param>
        /// <param name="startdate"></param>
        /// <param name="enddate"></param>
        /// <returns></returns>
        public ActionResult IncomeStatementReport(string sqlseache, string startdate, string enddate)
        {
            string EmpNo = (string)Session["EmpNo"];
            string rdlcPath, rdlcDocName;
            switch (currentCulture)
            {
                case "en":
                    rdlcPath = "~/Report/RDLC/IncomeStatementReport_en.rdlc";
                    rdlcDocName = "IncomeStatementReport_" + System.DateTime.Now.ToString("yyyyMMdd"); 
                    break;
                case "zh-TW":
                    rdlcPath = "~/Report/RDLC/IncomeStatementReport_zh-TW.rdlc";
                    rdlcDocName = "損益表_" + System.DateTime.Now.ToString("yyyyMMdd");
                    break;
                case "es-MX":
                    rdlcPath = "~/Report/RDLC/IncomeStatementReport_es-MX.rdlc";
                    rdlcDocName = "Informe de estado de resultados_" + System.DateTime.Now.ToString("yyyyMMdd");
                    break;
                default:
                    rdlcPath = "~/Report/RDLC/IncomeStatementReport_en.rdlc";
                    rdlcDocName = "IncomeStatementReport_" + System.DateTime.Now.ToString("yyyyMMdd");
                    break;
            }
            string sql = @"
               	IF OBJECT_ID ('tempdb..#TmpAcc') IS NOT NULL
		            Drop Table #TmpAcc
	            IF OBJECT_ID ('tempdb..#TmpVoucher') IS NOT NULL
		            Drop Table #TmpVoucher

                select * into #TmpAcc from (
	                select
		                a.AccGroupNo                       
                        ,case
                            when @currentCulture = 'en' then a.AccGroupNameE 
                            when @currentCulture = 'zh-TW' then a.AccGroupNameC 
                            when @currentCulture = 'es-MX' then a.AccGroupNameMX 
                            else a.AccGroupNameE 
                         end as AccGroupName
		                ,b.AccNo as AccControlNo                        
                        ,case
                            when @currentCulture = 'en' then b.AccNameE
                            when @currentCulture = 'zh-TW' then b.AccNameC
                            when @currentCulture = 'es-MX' then b.AccNameMX
                            else b.AccNameE
                         end as AccControlName
		                ,a.AccNo                       
                        ,case
                            when @currentCulture = 'en' then a.AccNameE
                            when @currentCulture = 'zh-TW' then a.AccNameC
                            when @currentCulture = 'es-MX' then a.AccNameMX
                            else a.AccNameE
                         end as AccName
	                from NES_WEB_ACC.dbo.ACC_AccTitleNo_MX as a
	                left join NES_WEB_ACC.dbo.ACC_AccTitleNo_MX  as b on CONCAT(LEFT(a.AccNo, 3), '-00-000') = b.AccNo
	                where 1 = 1
		                and a.AccNo not like '%00-00-000'
		                and a.AccGroupNo in (4,5,6,7,8)
                ) as a

                /*** 串ACC_AccTitleNo_MX並且取該會科的借貸方向 ***/
                /*** 並依照借貸方向計算，如果是借方科目金額=借方總額-貸方總額；貸方科目金額=貸方總額-借方總額 ***/
                SELECT * into #TmpVoucher from (
	                select
		                a.[AccNo]
		                ,SUM(
                        CASE	
                            WHEN b.DCTypeNo = 'D' THEN
                                CASE 
                                    WHEN a.[DCTypeNo] = 'D' THEN [Money1]
                                    WHEN a.[DCTypeNo] = 'C' THEN -[Money1]
                                    ELSE 0
                                END
                            WHEN b.DCTypeNo = 'C' THEN
                                CASE 
                                    WHEN a.[DCTypeNo] = 'D' THEN -[Money1]
                                    WHEN a.[DCTypeNo] = 'C' THEN [Money1]
                                    ELSE 0
                                END
                            ELSE 0
                        END
                    ) AS TotalMoney
	                FROM 
		                NES_WEB_ACC.dbo.ACC_VoucherDetail as a	 
                        left join NES_WEB_ACC.dbo.ACC_VoucherInfo as c on c.WebId = a.WebDocId
		                left join NES_WEB_ACC.dbo.ACC_AccTitleNo_MX as b on a.AccNo = b.AccNo 
                    where 1=1
                        and c.IsClosed = '1' --已結案   
                        and c.BillDate >= @startdate
                        and c.BillDate <= @enddate
	                GROUP BY 
		                a.[AccNo]
	                ) as a

                select a.*,ISNULL(b.TotalMoney,0) as TotalMoney from #TmpAcc as a
                left join #TmpVoucher as b on a.AccNo = b.AccNo
                order by a.AccGroupNo ,a.AccControlNo

	                IF OBJECT_ID ('tempdb..#TmpAcc') IS NOT NULL
		                Drop Table #TmpAcc
	                IF OBJECT_ID ('tempdb..#TmpVoucher') IS NOT NULL
		                Drop Table #TmpVoucher
            ";
            var param = new { currentCulture,startdate, enddate };
            List<IncomeStatementViewModel> balanceSheetReport = new List<IncomeStatementViewModel>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                balanceSheetReport = conn.Query<IncomeStatementViewModel>(sql, param).ToList();
            }

            // 報表參數設定
            var reportParameters = new List<ReportParameter>
            {
                new ReportParameter("ReportMaker", EmpNo),
                new ReportParameter("StartDate", startdate),
                new ReportParameter("EndDate", enddate),
                //new ReportParameter("StartDate", "2024/08/01"),
                //new ReportParameter("EndDate", "2024/08/31"),
            };

            // Session設定，給ReportViewer使用
            Session["ReportPath"] = Server.MapPath(rdlcPath);
            Session["ReportDataSource"] = new ReportDataSource("IncomeStatementRdlc", balanceSheetReport);
            Session["ReportParameters"] = reportParameters;
            Session["ReportDocName"] = rdlcDocName;

            return Redirect("~/Report/ReportViewer_V1.aspx");
        }
        /// <summary>
        /// 明細分類帳
        /// </summary>
        /// <param name="sqlseache"></param>
        /// <param name="startdate"></param>
        /// <param name="enddate"></param>
        /// <returns></returns>
        public ActionResult SubsidiaryLedgerReport(string sqlseache, string startdate, string enddate)
        {
            string EmpNo = (string)Session["EmpNo"];
            string rdlcPath, rdlcDocName;
            switch (currentCulture)
            {
                case "en":
                    rdlcPath = "~/Report/RDLC/SubsidiaryLedgerReport_en.rdlc";
                    rdlcDocName = "SubsidiaryLedgerReport_" + System.DateTime.Now.ToString("yyyyMMdd");
                    break;
                case "zh-TW":
                    rdlcPath = "~/Report/RDLC/SubsidiaryLedgerReport_zh-TW.rdlc";
                    rdlcDocName = "明細分類帳_" + System.DateTime.Now.ToString("yyyyMMdd");
                    break;
                case "es-MX":
                    rdlcPath = "~/Report/RDLC/SubsidiaryLedgerReport_es-MX.rdlc";
                    rdlcDocName = "Libro mayor subsidiario_" + System.DateTime.Now.ToString("yyyyMMdd");
                    break;
                default:
                    rdlcPath = "~/Report/RDLC/SubsidiaryLedgerReport_en.rdlc";
                    rdlcDocName = "SubsidiaryLedgerReport_" + System.DateTime.Now.ToString("yyyyMMdd");
                    break;
            }
            string sql = @"               
             	IF OBJECT_ID ('tempdb..#TmpBegin') IS NOT NULL
		             Drop Table #TmpBegin
	            IF OBJECT_ID ('tempdb..#TmpBegin2') IS NOT NULL
		            Drop Table #TmpBegin2
	            IF OBJECT_ID ('tempdb..#TmpVoucherBefore') IS NOT NULL
		            Drop Table #TmpVoucherBefore
	            IF OBJECT_ID ('tempdb..#TmpVoucher') IS NOT NULL
		            Drop Table #TmpVoucher
				IF OBJECT_ID ('tempdb..#TmpShow') IS NOT NULL
		            Drop Table #TmpShow

	            /****** 會計科目&期初傳票 ******/
	            select * into #TmpBegin from (
		            select
			             a.AccGroupNo,a.AccNo                        
                         ,case
                            when @currentCulture = 'en' then a.AccNameE
                            when @currentCulture = 'zh-TW' then a.AccNameC
                            when @currentCulture = 'es-MX' then a.AccNameMX
                            else a.AccNameE
                          end as AccName
                         ,a.DCTypeNo  
			             ,case	
				            when a.DCTypeNo = 'D' then
					            case 
						            when b.DCTypeNo = 'D' then NULLIF(b.Money1,0)
						            else 0-NULLIF(b.Money1,0)
					            end
				            else 
					            case	
						            when b.DCTypeNo = 'C' then NULLIF(b.Money1,0)
						            else 0-NULLIF(b.Money1,0)
					            end
			            end as BeginMoney
		            from NES_WEB_ACC.dbo.ACC_AccTitleNo_MX as a
		            left join (
			            select b.AccNo,b.DCTypeNo,b.Money1 
			            from NES_WEB_ACC.dbo.ACC_VoucherDetail as b
				            inner join (
					            select top(1) * from NES_WEB_ACC.dbo.ACC_VoucherInfo where DocType = 'V2' order by CreateDate
				            ) as a on a.WebId = b.WebDocId
			            ) as b  on a.AccNo = b.AccNo
	            ) as a
	            /****** 查詢區間前資料 ******/
	            select  * into #TmpVoucherBefore from (
		            select a.BillNo,b.AccNo,b.Remark,b.DCTypeNo,b.Money1 
		            from NES_WEB_ACC.dbo.ACC_VoucherDetail as b
			            left join NES_WEB_ACC.dbo.ACC_VoucherInfo as a on a.WebId = b.WebDocId
		            where 1=1	
			            and a.DocType = 'V1'
			            and a.BillNo is not null
                        and a.IsClosed = '1' --已結案
			            and a.BillDate < @startdate
			            --and a.BillDate < '2024/08/10'
	            ) as a;
	            /****** 查詢區間資料 ******/
	            select  * into #TmpVoucher from (
		            select a.BillNo,a.BillDate,b.AccNo,b.Remark,b.DCTypeNo,b.Money1 
		            from NES_WEB_ACC.dbo.ACC_VoucherDetail as b
			            left join NES_WEB_ACC.dbo.ACC_VoucherInfo as a on a.WebId = b.WebDocId
		            where 1=1	
			            and a.DocType = 'V1'
			            and a.BillNo is not null
                        and a.IsClosed = '1' --已結案
			            and a.BillDate >= @startdate
			            and a.BillDate <= @enddate
			            --and a.BillDate >= '2024/08/10'
			            --and a.BillDate <= '2024/08/31'
	            ) as a;
	            /****** 會計科目&期初傳票(含查詢區間前傳票) ******/
	            select * into #TmpBegin2 from (
		            select 
			            a.AccGroupNo,a.AccNo,a.AccName,a.DCTypeNo
			            ,case	
				            when a.DCTypeNo = 'D' then
					            sum( case when b.DCTypeNo = 'D' then b.Money1 else 0 end) -sum(case when b.DCTypeNo = 'C' then b.Money1 else 0 end)
				            else 
					             sum( case when b.DCTypeNo = 'C' then b.Money1 else 0 end) -sum(case when b.DCTypeNo = 'D' then b.Money1 else 0 end)
			            end as BeginMoney
		            from #TmpBegin as a
		            left join #TmpVoucherBefore as b on a.AccNo = b.AccNo
		            group by a.AccGroupNo,a.AccNo,a.AccName,a.DCTypeNo
	            ) as a;

	            /****** For Show ******/
				select * into #TmpShow from (				
		            select 
			            a.* 
			            ,b.BillDate
			            , FORMAT(b.BillDate, 'yyyy-MM') AS BillMonth
			            ,b.BillNo
			            ,b.Remark
			            ,b.DCTypeNo as BillDCTypeNo
			            ,b.Money1	 
		            from #TmpBegin2 as a
		            left join #TmpVoucher as b on a.AccNo = b.AccNo
				)a				
				/****** 顯示 ******/
				select 
					* 
					,case 
						when DCTypeNo = BillDCTypeNo 
						then BeginMoney + SUM(Money1) OVER (PARTITION BY AccNo ORDER BY BillDate, BillNo)
						else BeginMoney - SUM(Money1) OVER (PARTITION BY AccNo ORDER BY BillDate, BillNo)					
					end AS BillLast
				from #TmpShow
				order by 1,2
				


	            IF OBJECT_ID ('tempdb..#TmpBegin') IS NOT NULL
		             Drop Table #TmpBegin
	            IF OBJECT_ID ('tempdb..#TmpBegin2') IS NOT NULL
		            Drop Table #TmpBegin2
	            IF OBJECT_ID ('tempdb..#TmpVoucherBefore') IS NOT NULL
		            Drop Table #TmpVoucherBefore
	            IF OBJECT_ID ('tempdb..#TmpVoucher') IS NOT NULL
		            Drop Table #TmpVoucher
				IF OBJECT_ID ('tempdb..#TmpShow') IS NOT NULL
		            Drop Table #TmpShow
            ";
            var param = new { currentCulture, startdate, enddate };
            List<SubsidiaryLedgerViewModel> subsidiaryLedgerReport = new List<SubsidiaryLedgerViewModel>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                subsidiaryLedgerReport = conn.Query<SubsidiaryLedgerViewModel>(sql, param).ToList();
            }

            // 報表參數設定
            var reportParameters = new List<ReportParameter>
            {
                new ReportParameter("ReportMaker", EmpNo),
                new ReportParameter("StartDate", startdate),
                new ReportParameter("EndDate", enddate),               
            };

            // Session設定，給ReportViewer使用
            Session["ReportPath"] = Server.MapPath(rdlcPath);
            Session["ReportDataSource"] = new ReportDataSource("SubsidiaryLedgerRdlc", subsidiaryLedgerReport);
            Session["ReportParameters"] = reportParameters;
            Session["ReportDocName"] = rdlcDocName;

            return Redirect("~/Report/ReportViewer_V1.aspx");
        }
    }
}