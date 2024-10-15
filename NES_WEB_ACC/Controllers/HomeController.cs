using Dapper;
using Microsoft.ReportingServices.Diagnostics.Internal;
using NES_WEB_ACC.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace NES_WEB_ACC.Controllers
{
    [Authorize]  
    public class HomeController : Controller
    {
        public string connectionString = ConfigurationManager.ConnectionStrings["NES_WEB_ACCConnectionString"].ConnectionString;
        private string currentCulture = Thread.CurrentThread.CurrentCulture.Name;
        /// <summary>
        /// 介面_儀錶板
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {           
            string identityEmpNo = ControllerContext.HttpContext.User.Identity.Name;
            CompanySessionSetting();
            UserSessionSetting(identityEmpNo);           

            // 檢查TempData是否包含訊息
            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"].ToString();
            }

            //儀錶板資料
            string connectionString = ConfigurationManager.ConnectionStrings["NES_WEB_ACCConnectionString"].ConnectionString;
            string sql = @"
                select  
	                (select count(*) from NES_WEB_ACC.dbo.ACC_VoucherInfo where EmpNo = @identityEmpNo and DocSubType = 'A') as 'Total'
	                ,(select count(*) from NES_WEB_ACC.dbo.ACC_VoucherInfo where EmpNo = @identityEmpNo and DocSubType = 'A' and Isnull(IsChecked,0) <> 1) as 'Uncheck'
	                ,(select count(*) from NES_WEB_ACC.dbo.ACC_VoucherInfo where EmpNo = @identityEmpNo and DocSubType = 'A' and Isnull(IsChecked,0) = 1 and ISNULL(IsState,0) <> 1) as 'UnRecheck'
	                ,(select count(*) from NES_WEB_ACC.dbo.ACC_VoucherInfo where EmpNo = @identityEmpNo and DocSubType = 'A' and Isnull(IsChecked,0) = 1 and ISNULL(IsState,0) = 1 and ISNULL(IsClosed,0) <> 1) as 'Unclose'
            ";
            var param = new { identityEmpNo };
            HomeBillInfoViewModel resultdata = new HomeBillInfoViewModel();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                resultdata = conn.QueryFirstOrDefault<HomeBillInfoViewModel>(sql,param);
            }

            return View(resultdata);
        }      
        /// <summary>
        /// 介面_作業流程圖
        /// </summary>
        /// <returns></returns>
        public ActionResult Workflow()
        {
            return View();
        }

        public ActionResult UserSessionSetting(string identityEmpNo)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["NES_WEB_ACCConnectionString"].ConnectionString;

            // SQL 查詢語句-1：系統中是否有該使用者
            string sqlQuery1 = @"SELECT 
                                            SU.EmpId,
                                            SU.EmpNo,
                                            SU.EmpNameC,
                                            SU.Status,
                                            EI.DeptNo,
                                            EI.DeptName
                                        FROM [NES_WEB_ACC].[dbo].[SYS_Users] as SU 
                                          left join [NES_WEB].[dbo].NES_EmployeeInfo as EI on SU.EmpId = EI.Id
                                        where 1=1
                                         and SU.Status = 1	--User是否啟用
                                         and EI.IsStatus = 0  --ERP在職員工
                                            and SU.EmpNo = @EmpNo";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(sqlQuery1, connection))
                {
                    command.Parameters.AddWithValue("@EmpNo", identityEmpNo);
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            if (reader.GetBoolean(reader.GetOrdinal("Status")))
                            {
                                // 將資料庫資料寫入Session：EmpId、EmpNo、EmpNameC
                                Session["EmpId"] = reader.GetInt64(reader.GetOrdinal("EmpId"));
                                Session["EmpNo"] = reader.GetString(reader.GetOrdinal("EmpNo"));
                                Session["EmpNameC"] = reader.GetString(reader.GetOrdinal("EmpNameC"));
                                Session["DeptNo"] = reader.GetString(reader.GetOrdinal("DeptNo"));
                                Session["DeptName"] = reader.GetString(reader.GetOrdinal("DeptName"));                                
                                TempData["Message"] = "角色Session設定完成。";
                                return RedirectToAction("Index");
                            }
                            else
                            {
                                TempData["Message"] = "您的帳號或已被停用。";
                                return RedirectToAction("Index");
                            }
                        }
                        else
                        {
                            TempData["Message"] = "此系統無您的資料，請聯絡系統管理員。";
                            return RedirectToAction("Index");
                        }
                    }
                }
            }
        }
        public void CompanySessionSetting()
        {
            Session["CompId"] = "5563551763276641505";
            Session["CompNo"] = "C";
            Session["CompAbbr"] = "RUIS SERVICES";
            Session["CurrencySt"] = "MXN";
        }

        public ActionResult TimeOut()
        {
            //清除登入資訊
            FormsAuthentication.SignOut();
            Session.Abandon();
            string url = @"http://" + Request.Url.Authority + @"/NES_Login.aspx";
            //Response.Redirect(url);
            return RedirectPermanent(url);
        }

        /// <summary>
		/// 導覽圖介面
		/// </summary>
		/// <returns></returns>
		public ActionResult _AccStatePartial()
        {
            return PartialView();
        }

        /// <summary>
        /// 多語系，並建立Cookie紀錄
        /// </summary>
        /// <param name="lang"></param>
        /// <returns></returns>
        public ActionResult ChangeLanguage(string lang)
        {               
            // Set cookie
            HttpCookie langCookie = new HttpCookie("lang", lang);
            langCookie.Expires = DateTime.Now.AddYears(1); // Cookie 有效期設置為一年
            Response.Cookies.Add(langCookie);
            Session["lang"] = lang;
            return RedirectToAction("Index", "Home", new { language = lang });
        }

        public JsonResult GetEchartData()
        {
            var sql = @"
                IF	OBJECT_ID('tempdb..#TmpBasic') IS NOT NULL
	                Drop Table #TmpBasic

                declare @currencyst nvarchar(10);
                set @currencyst = 'MXN'  --@currentCulture

                select * into #TmpBasic from (
	                select top(9) 
		                ExchangeYear, ExchangeMonth, ExchangeMonthFlag
		                ,CAST(ExchangeYear AS varchar) + '\' + CAST(ExchangeMonth AS varchar) + '\' + ExchangeMonthFlag as 'xAxisData'
		                ,ROW_NUMBER() OVER (ORDER BY ExchangeYear DESC, ExchangeMonth DESC, FlagOrder DESC) AS 'RowNumber'
	                from ( 
		                select *,
			                case 
				                when ExchangeMonthFlag = 'B' then 1
				                when ExchangeMonthFlag = 'M' then 2
				                when ExchangeMonthFlag = 'E' then 3
			                end as FlagOrder 
		                from NES_WEB_ACC.dbo.ACC_Rate
	                ) as a
	                group by ExchangeYear, ExchangeMonth, ExchangeMonthFlag, FlagOrder
	                order by ExchangeYear desc, ExchangeMonth desc, FlagOrder desc
                ) a
                select xAxisData from #TmpBasic order by RowNumber desc
                select Rate as 'Series1' from NES_WEB_ACC.dbo.ACC_Rate as a
                inner join #TmpBasic as b on a.ExchangeYear = b.ExchangeYear and a.ExchangeMonth = b.ExchangeMonth and a.ExchangeMonthFlag = b.ExchangeMonthFlag
                where CurrencyNo = 'TWD' and CurrencySt = @currencyst
                order by RowNumber desc
                select Rate as 'Series2'  from NES_WEB_ACC.dbo.ACC_Rate as a
                inner join #TmpBasic as b on a.ExchangeYear = b.ExchangeYear and a.ExchangeMonth = b.ExchangeMonth and a.ExchangeMonthFlag = b.ExchangeMonthFlag
                where CurrencyNo = 'USD' and CurrencySt = @currencyst
                order by RowNumber desc
                select Rate as 'Series3'  from NES_WEB_ACC.dbo.ACC_Rate as a
                inner join #TmpBasic as b on a.ExchangeYear = b.ExchangeYear and a.ExchangeMonth = b.ExchangeMonth and a.ExchangeMonthFlag = b.ExchangeMonthFlag
                where CurrencyNo = 'MXN' and CurrencySt = @currencyst
                order by RowNumber desc
                IF	OBJECT_ID('tempdb..#TmpBasic') IS NOT NULL
	                Drop Table #TmpBasic    
            ";             
            var param = new { currentCulture };

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                var result = conn.QueryMultiple(sql, param);

                var xAxisData = result.Read<string>().ToList();
                var series1 = result.Read<decimal>().ToList();
                var series2 = result.Read<decimal>().ToList();
                var series3 = result.Read<decimal>().ToList();

                var chartData = new
                {
                    xAxis = xAxisData,
                    series1 = series1,
                    series2 = series2,
                    series3 = series3
                };

                return Json(chartData, JsonRequestBehavior.AllowGet);
            }            
        }
    }
}