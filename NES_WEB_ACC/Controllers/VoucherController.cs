using Dapper;
using Microsoft.Reporting.WebForms;
using NES_WEB_ACC.Modules;
using NES_WEB_ACC.Report;
using NES_WEB_ACC.Report.RDLC;
using NES_WEB_ACC.ViewModels;
using Newtonsoft.Json.Linq; 
using Newtonsoft.Json; 
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Web.Mvc;
using NES_WEB_ACC.ViewModels.ForM;

namespace NES_WEB_ACC.Controllers
{
    [Authorize]
    public class VoucherController : Controller
    {
        public string connectionString = ConfigurationManager.ConnectionStrings["NES_WEB_ACCConnectionString"].ConnectionString;
        private NES_WEB_ACCEntities _dbContext = new NES_WEB_ACCEntities();
        private string currentCulture = Thread.CurrentThread.CurrentCulture.Name;
        //------ 介面 ------//        
        /// <summary>
        /// 介面_傳票建立
        /// </summary>
        /// <returns></returns>
        [CustomAuthorize(Roles = "Admin,AccManager,AccPm,AccUser")]
        public ActionResult VoucherCreate(string billno, string msg)
        {
            var createInfo = new CreateInfoViewModel
            {
                EmpId = Session["EmpId"].ToString(),
                EmpNo = (string)Session["EmpNo"],
                EmpNameC = (string)Session["EmpNameC"],
                DeptNo = (string)Session["DeptNo"],
                DeptName = (string)Session["DeptName"],
                CompNo = (string)Session["CompNo"],
                CurrencySt = (string)Session["CurrencySt"]
            };          
            var data = GetEditTableCode3("0");
            ViewData["CurrencyNo"] = data.Data; 

            ViewBag.BillNo = (String.IsNullOrEmpty(billno)) ? null : billno;
            ViewBag.Msg = (String.IsNullOrEmpty(msg)) ? null : msg;
            ViewBag.CurrentCulture = currentCulture;
            return View(createInfo);
        }
        /// <summary>
        /// 介面_主管審核
        /// </summary>
        /// <returns></returns>
        [CustomAuthorize(Roles = "Admin,AccManager,AccPm")]
        public ActionResult VoucherCheck(string billno, string msg)
        {
            ViewBag.BillNo = (String.IsNullOrEmpty(billno)) ? null : billno;
            ViewBag.Msg = (String.IsNullOrEmpty(msg)) ? null : msg;
            return View();
        }
        /// <summary>
        /// 介面_傳票結案
        /// </summary>
        /// <returns></returns>
        [CustomAuthorize(Roles = "Admin,AccManager,AccPm")]
        public ActionResult VoucherClose(string billno, string msg)
        {
            ViewBag.BillNo = (String.IsNullOrEmpty(billno)) ? null : billno;
            ViewBag.Msg = (String.IsNullOrEmpty(msg)) ? null : msg;
            return View();
        }
        /// <summary>
        /// 介面_期初傳票
        /// </summary>
        /// <returns></returns>
        [CustomAuthorize(Roles = "Admin,AccManager")]
        public ActionResult VoucherBeginning(string billno, string msg)
        {
            var createInfo = new CreateInfoViewModel
            {
                EmpId = Session["EmpId"].ToString(),
                EmpNo = (string)Session["EmpNo"],
                EmpNameC = (string)Session["EmpNameC"],
                DeptNo = (string)Session["DeptNo"],
                DeptName = (string)Session["DeptName"],
                CompNo = (string)Session["CompNo"],
                CurrencySt = (string)Session["CurrencySt"]
            };
            var data = GetEditTableCode3("0");
            ViewData["CurrencyNo"] = data.Data;

            ViewBag.BillNo = (String.IsNullOrEmpty(billno)) ? null : billno;
            ViewBag.Msg = (String.IsNullOrEmpty(msg)) ? null : msg;
            ViewBag.CurrentCulture = currentCulture;
            return View(createInfo);
        }
        
        //------ 資料讀取 ------//
        public ActionResult GetVoucherInfo(string type,bool dosearch = false, SearchString search = null)
        {
            var roles = ((GenericPrincipal)User).Identity as GenericIdentity;
            string sql,sqlwhere,sqlsearch = "";
            var param = new DynamicParameters();

            if (string.IsNullOrEmpty(type))
            {
                return Json(new { success = false, code = "C0001" }, JsonRequestBehavior.AllowGet);
            }
            if (dosearch)
            {                
                sqlsearch += (!string.IsNullOrEmpty(search.SearchField1)) ? " " + search.Logical1 + " " + search.SearchField1 + " " + search.Comparison1 + " @searchdata1 " : "";
                sqlsearch += (!string.IsNullOrEmpty(search.SearchField2)) ? " " + search.Logical2 + " " + search.SearchField2 + " " + search.Comparison2 + " @searchdata2 " : "";
                sqlsearch += (!string.IsNullOrEmpty(search.SearchField3)) ? " " + search.Logical3 + " " + search.SearchField3 + " " + search.Comparison3 + " @searchdata3 " : "";

                if (!string.IsNullOrEmpty(search.SearchField1)) param.Add("searchdata1", search.SearchData1);
                if (!string.IsNullOrEmpty(search.SearchField2)) param.Add("searchdata2", search.SearchData2);
                if (!string.IsNullOrEmpty(search.SearchField3)) param.Add("searchdata3", search.SearchData3);
            }
            if (roles != null)
            {
                var userRoles = ((GenericPrincipal)User).Claims
                    .Where(c => c.Type == ClaimTypes.Role)
                    .Select(c => c.Value)
                    .ToArray();

                sql = @"
                            select * from NES_WEB_ACC.dbo.ACC_VoucherInfo 
                            where 1=1      
                                --and CompId = @compid
                                --and CompNo = @compno
                                --amd CompAbbr = @compabbr                            
                ";
              
                switch (type)
                {
                    case "create":
                        sqlwhere = @"  
                            and DocType = 'V1'
                            and DocSubType = 'A'
                            and Isnull(IsClosed,0) = 0    --未結案   
                            --and BillStatus in ('0','1','2','4','5')
                        ";
                        sql = sql + sqlwhere ;
                        if (!userRoles.Contains("Admin"))
                        {
                            sql = sql + " and CreateBy = @craeteby ";
                            string craeteby = roles.Name;
                            param.Add("craeteby", craeteby);                           
                        }
                        sql += (dosearch) ? sqlsearch : "";
                        break;
                    case "check":
                        sqlwhere = @"
                            and DocType = 'V1'
                            and DocSubType = 'A'
                            and Isnull(IsChecked,0) = 1   --已覆核
                            and Isnull(IsState,0) = 0     --主管未審核
                            and Isnull(IsClosed,0) = 0    --未結案                            
                        ";
                        sql = sql + sqlwhere;
                        sql += (dosearch) ? sqlsearch : "";
                        break;
                    case "close":
                        sqlwhere = @" 
                            and DocType = 'V1'
                            and DocSubType = 'A'
                            and Isnull(IsChecked,0) = 1   --已覆核
                            and Isnull(IsState,0) = 1     --主管已審核
                            --and Isnull(IsClosed,0) = 0    --未結案
                        ";
                        sql = sql + sqlwhere;
                        sql += (dosearch) ? sqlsearch : "";
                        break;
                    case "begin":
                        sqlwhere = @"
                            and DocType = 'V2'
                            and DocSubType = 'B'
                        ";
                        sql = sql + sqlwhere;
                        sql += (dosearch) ? sqlsearch : "";
                        break;
                    default:
                        return Json(new { success = false, code = "C0001" }, JsonRequestBehavior.AllowGet);                        
                }
            }
            else
            {
                return Json (new { success = false, code = "C0001" }, JsonRequestBehavior.AllowGet);
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    List<ACC_VoucherInfo> resultdata;
                    sql += " order by BillNo";
                    //if (param == null)
                    //{
                    //    resultdata = conn.Query<ACC_VoucherInfo>(sql).ToList();
                    //}
                    //else
                    //{
                    //    resultdata = conn.Query<ACC_VoucherInfo>(sql, param).ToList();
                    //}
                    resultdata = conn.Query<ACC_VoucherInfo>(sql, param).ToList();
                    if (resultdata.Count > 0)
                    {                        
                        return Json(new { success = true, code = "OK",data = resultdata }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { success = false, code = "C0003"}, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception e)
            {
                return Json(new { success = false, message = "C0004",err = e }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult GetVoucherItem(string webdocid)
        {
            if (string.IsNullOrEmpty(webdocid))
            {              
                return Json(new { success = false, code = "C0001" }, JsonRequestBehavior.AllowGet);
            }
            string sql;
            switch (currentCulture)
            {                
                case "zh-TW":
                    sql = @"select *,AccNameC as AccName from ACC_VoucherDetail where WebDocId = @webdocid order by Linage";
                    break;
                case "es-MX":
                    sql = @"select *,AccNameMX as AccName from ACC_VoucherDetail where WebDocId = @webdocid order by Linage";
                    break;
                case "en":
                default:
                    sql = @"select *,AccNameE as AccName from ACC_VoucherDetail where WebDocId = @webdocid order by Linage";
                    break;
            }
           
            var param = new { webdocid };
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    List<ACC_VoucherDetail_ViewModel> customerdata = conn.Query<ACC_VoucherDetail_ViewModel>(sql, param).ToList();
                    if (customerdata.Count > 0)
                    {                       
                        return Json(new { success = true, code = "OK" , data = customerdata }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {                       
                        return Json(new { success = false, code = "C0003" }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception e)
            {              
                return Json(new { success = false, code = "C0004" , err = e}, JsonRequestBehavior.AllowGet);
            }
        }        
        /// <summary>
        /// 編輯_會計科目
        /// </summary>
        /// <returns></returns>
        public ActionResult GetEditTableCode()
        {
            string compid = Session["CompId"].ToString();
            string compno = Session["CompNo"].ToString();
            string compabbr = Session["CompAbbr"].ToString();
            string sql;
            switch (currentCulture)
            {                
                case "zh-TW":
                    sql = @"
                        select AccNo,AccNameC as AccName ,AccNoBy,AccNoByNameC,AccGroupNo,AccGroupNameC,DCTypeNo,DCTypeNameC  
                        from NES_WEB_ACC.dbo.ACC_AccTitleNo_MX 
                        where 1=1
                            and CompId = @compid 
                            and CompNo = @compno
                            and CompAbbr = @compabbr
                    ";
                    break;
                case "es-MX":
                    sql = @"
                        select AccNo,AccNameMX as AccName ,AccNoBy,AccNoByNameC,AccGroupNo,AccGroupNameC,DCTypeNo,DCTypeNameC  
                        from NES_WEB_ACC.dbo.ACC_AccTitleNo_MX 
                        where 1=1
                            and CompId = @compid 
                            and CompNo = @compno
                            and CompAbbr = @compabbr
                    ";
                    break;
                case "en":
                default:
                    sql = @"
                        select AccNo,AccNameE as AccName ,AccNoBy,AccNoByNameC,AccGroupNo,AccGroupNameC,DCTypeNo,DCTypeNameC  
                        from NES_WEB_ACC.dbo.ACC_AccTitleNo_MX 
                        where 1=1
                            and CompId = @compid 
                            and CompNo = @compno
                            and CompAbbr = @compabbr
                    ";
                    break;
            }
            
            var param = new { compid, compno,compabbr };
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    List<ACC_AccTitleNo_MX_ViewModel> customerdata = conn.Query<ACC_AccTitleNo_MX_ViewModel>(sql, param).ToList();
                    if (customerdata.Count > 0)
                    {
                        return Json(new { success = true, code = "OK" , data = customerdata }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {                       
                        return Json(new { success = false, code = "C0003" }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception e)
            {               
                return Json(new { success = false, code = "C0004" , err = e }, JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// 編輯_對象別
        /// </summary>
        /// <returns></returns>
        public ActionResult GetEditTableCode2()
        {
            List<object> dataList;
            switch (currentCulture)
            {                
                case "zh-TW":
                    dataList = new List<object>
                    {
                        new { TargetType = "15", TargetTypeName = "廠商" },
                        new { TargetType = "14", TargetTypeName = "客戶" },
                        new { TargetType = "12", TargetTypeName = "員工" },
                        new { TargetType = "65", TargetTypeName = "銀行" },
                    };
                    break;
                case "es-MX":
                    dataList = new List<object>
                    {
                        new { TargetType = "15", TargetTypeName = "Fabricante" },
                        new { TargetType = "14", TargetTypeName = "Cliente" },
                        new { TargetType = "12", TargetTypeName = "Empleada" },
                        new { TargetType = "65", TargetTypeName = "Banco" },
                    };
                    break;
                case "en":
                default:
                    dataList = new List<object>
                    {
                        new { TargetType = "15", TargetTypeName = "Manufacturer" },
                        new { TargetType = "14", TargetTypeName = "Customer" },
                        new { TargetType = "12", TargetTypeName = "Employee" },
                        new { TargetType = "65", TargetTypeName = "Bank" },
                    };
                    break;
            }          
            
            return Json(new { success = true, code = "OK", data = dataList }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 編輯_幣別&匯率
        /// </summary>
        /// <returns></returns>
        public JsonResult GetEditTableCode3(string askfrom)
        {
            string currencyst = (string)Session["CurrencySt"];
            string sql = @"
               --依當前日期取的平均匯率
                select a.CurrencyNo , a.Rate
                from NES_WEB_ACC.dbo.ACC_Rate as a
                    inner join (	                 
					select * from (
	                    select 
		                    CurrencyNo
		                    ,CurrencySt
		                    ,CONCAT([ExchangeYear], '/',  RIGHT('0' + CAST([ExchangeMonth] AS varchar(2)), 2),'/',  case 
			                    when ExchangeMonthFlag = 'B' then  '01' 
			                    when ExchangeMonthFlag = 'M' then '11' 
			                    when ExchangeMonthFlag = 'E' then '21' 
		                    end ) AS ExchangeDate
		                    ,Rate
						from NES_WEB_ACC.dbo.ACC_Rate) as a 
						where ExchangeDate = 
							CASE 
								WHEN DAY(GETDATE()) BETWEEN 1 AND 10 THEN CONVERT(varchar(100), DATEADD(day, 1 - DAY(GETDATE()), GETDATE()), 111)  -- 1號
								WHEN DAY(GETDATE()) BETWEEN 11 AND 20 THEN CONVERT(varchar(100), DATEADD(day, 11 - DAY(GETDATE()), GETDATE()), 111) -- 11號
								ELSE CONVERT(varchar(100), DATEADD(day, 21 - DAY(GETDATE()), GETDATE()), 111) -- 21號
							END	               
                    ) as b on a.CurrencyNo = b.CurrencyNo
	                    and a.CurrencySt = b.CurrencySt
	                    and a.ExchangeYear = LEFT(b.[ExchangeDate], CHARINDEX('/', b.[ExchangeDate]) - 1)
	                    and a.ExchangeMonth = SUBSTRING(b.[ExchangeDate], CHARINDEX('/', b.[ExchangeDate]) + 1, CHARINDEX('/', b.[ExchangeDate], CHARINDEX('/', b.[ExchangeDate]) + 1) - CHARINDEX('/', b.[ExchangeDate]) - 1)
	                    and a.ExchangeMonthFlag =CASE
                            WHEN RIGHT(b.[ExchangeDate], 2) = '01' THEN 'B'
                            WHEN RIGHT(b.[ExchangeDate], 2) = '11' THEN 'M'
                            WHEN RIGHT(b.[ExchangeDate], 2) = '21' THEN 'E'
                        END
                where a.CurrencySt = @currencyst  --'MXN'
            ";
            var param = new { currencyst };
            List<ACC_Rate> resultdata;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                resultdata = conn.Query<ACC_Rate>(sql, param).ToList();
            }
            if (askfrom == "0")
            {
                return Json(resultdata, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = true, code = "OK", data = resultdata }, JsonRequestBehavior.AllowGet);
            }
            
        }
        /// <summary>
        /// 編輯_表身資訊_對象編號
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public ActionResult GetEditTableCode4(string type)
        {
            if (string.IsNullOrEmpty(type))
            {               
                return Json(new { success = false, code = "C0001" }, JsonRequestBehavior.AllowGet);
            }
            string sql;
            Type viewModelType = null;
            switch (type)
            {
                case "15":
                    sql = @"
                        SELECT DISTINCT 
                            T1.Id ,T1.PartnerNo ,T1.PartnerAbbr ,T1.PartnerName ,T5.EmpNo ,T5.EmpNameC ,T1.Tel1 ,T1.Tel2 ,T2.VatNo ,T1.AreaName ,T1.ClassName ,T1.Grade ,T1.Addr ,T1.AgentNo ,T1.AgentAbbr ,T1.ConsigneeNo ,T1.ConsigneeAbbr ,T1.AccChargeNo ,T1.AccChargeAbbr ,T1.IsStatus
                        FROM ESTAERPV2.dbo.PartnerInfo as T1 
                            LEFT JOIN ESTAERPV2.dbo.PartnerAcc as T2 ON T1.Id = T2.DocId and T2.Type = 1
                            LEFT JOIN ESTAERPV2.dbo.PartnerByComp as T3 on T1.Id = T3.DocId
                            LEFT JOIN ESTAERPV2.dbo.CompanyInfo as T4 on T3.CompId = T4.Id
                            OUTER APPLY (select TOP 1 EmpNo, EmpNameC from ESTAERPV2.dbo.PartnerByComp where DocId = T1.Id and CompNo = T3.CompNo) as T5                    
                        WHERE T1.Type = 15 and T3.CompId in ( -1 , 150615163202244 ) and T1.IsChecked = 1 and T1.IsState = 0 and T1.IsStatus = 0
                        ORDER BY  T1.PartnerNo
                    ";
                    viewModelType = typeof(VoucherCreateEditTargetNo1ViewModel);
                    break;
                case "14":
                    sql = @"
                        SELECT  DISTINCT 
                            T1.Id ,T1.PartnerNo ,T1.PartnerAbbr ,T1.PartnerName ,T3.EmpNo ,T3.EmpNameC ,T1.Tel1 ,T1.Tel2 ,T2.VatNo ,T1.AreaName ,T1.ClassName ,T1.Grade ,T1.Addr ,T1.AgentNo ,T1.AgentAbbr ,T1.ConsigneeNo ,T1.ConsigneeAbbr ,T1.AccChargeNo ,T1.AccChargeAbbr ,T1.IsStatus
                        FROM ESTAERPV2.dbo.PartnerInfo as T1 
                            LEFT JOIN ESTAERPV2.dbo.PartnerAcc as T2 ON T1.Id = T2.DocId and T2.Type = 1
                            LEFT JOIN ESTAERPV2.dbo.PartnerByComp  as T3   on  T1.Id  =  T3.DocId
                            LEFT JOIN ESTAERPV2.dbo.CompanyInfo  as T4   on  T3.CompId  =  T4.Id                        
                        WHERE T1.Type = 14 and T3.CompId in ( -1 , 150615163202244 ) and T1.IsChecked = 1 and T1.IsState = 0 and T1.IsStatus = 0
                        ORDER BY  T1.PartnerNo
                    ";
                    viewModelType = typeof(VoucherCreateEditTargetNo2ViewModel);
                    break;
                case "12":
                    sql = @"
                        SELECT 
                            T1.Id ,T1.DeptId ,T1.EmpNo ,T1.EmpNameC ,T1.EmpNameE ,T1.DeptNo ,T1.DeptName ,T1.Title ,T1.JobType ,T1.ClassId ,T1.ClassCode ,T1.ClassName ,T1.EmpCard ,T1.Post ,T1.Grade ,T1.Rank
                        FROM ESTAERPV2.dbo.EmployeeInfo as T1  
                        WHERE  (JobType = N'在職'  or JobType = N'試用') and ISNULL(IsErp,0) = 1 and CompNo = N'A'
                        ORDER BY  T1.DeptNo, T1.EmpNo
                    ";
                    viewModelType = typeof(VoucherCreateEditTargetNo3ViewModel);
                    break;
                case "65":
                    sql = @"
                        SELECT DISTINCT 
                            T1.Id ,T1.Code ,T1.BankCode ,T1.BankAbbr ,T1.AccountNo ,T1.AccountNameC ,T1.AccountNameE ,T1.AccNoAbbr ,T1.PettyCashExpenses ,T1.AccId ,T1.AccNo ,T1.AccNameC ,T1.AccNameE
                        FROM ESTAERPV2.dbo.CompanyBankInfo as T1
                            left join ESTAERPV2.dbo.CompanyBankCurrency as T2 on (T1.Id = T2.DocId)                               
                        WHERE T1.CompId in ( -1 , 150615163202244 ) And T1.CompNo = N'A' 
                        ORDER BY  T1.Code
                    ";
                    viewModelType = typeof(VoucherCreateEditTargetNo4ViewModel);
                    break;
                default:                    
                    return Json(new { success = false, code = "C0001" }, JsonRequestBehavior.AllowGet); 
            }
             
            //var param = new { docid };
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    var customerdata = conn.Query(viewModelType, sql).ToList();
                    if (customerdata.Count > 0)
                    {
                        return Json(new { success = true, code = "OK", data = customerdata }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {                        
                        return Json(new { success = false, code = "C0003" }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception e)
            {               
                return Json(new { success = false, code = "C0004" , err = e }, JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// 編輯_表身資訊_成本中心
        /// </summary>
        /// <returns></returns>
        public ActionResult GetEditTableCode5()
        {
            string sql = @"
              SELECT 
                 CAST(T1.DocId AS VARCHAR(20)) AS DocId, CAST(T1.Id AS VARCHAR(20)) AS Id,T1.DeptNo ,T1.DeptNameC ,T1.DeptNameE ,T1.CostLevel
                 FROM ESTAERPV2.dbo.CompanyDept AS T1 
                 WHERE  1=1 and DocId = '150615163202244'
                                and DocId in (select CompId from ESTAERPV2.dbo.UserComp where DocId = 5118025658776861062)
                                and Id in (-1,150615180215382,150615180246474,150615180336289,150615180351569,150615180413894,150615180435147,150615180501009,150615180526337,150616100815608,150616100859069,150616100933569,150616101312619,150616104415125,150616105520376,150616105631046,150616105654696,150616105757566,150616105833717,150616105913867,160818175502176,4619767610192552604,4701615448641564065,4763770088711818952,4865408518691831955,4879809568801835176,4932464689311258781,4960905404041071669,5053810030989480689,5585776709352846340)
            
                 ORDER BY DeptNo
            ";
            //var param = new { docid };
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    List<VoucherCreateEditAccDeptViewModel> customerdata = conn.Query<VoucherCreateEditAccDeptViewModel>(sql).ToList();
                    if (customerdata.Count > 0)
                    {
                        return Json(new { success = true, code = "OK", data = customerdata }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {                        
                        return Json(new { success = false, code = "C0003" }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception e)
            {              
                return Json(new { success = false, code = "C0004" , err = e }, JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// 編輯_表身資訊_支付部門
        /// </summary>
        /// <returns></returns>
        public ActionResult GetEditTableCode6()
        {
            string sql = @"
                 SELECT 
                    CAST(T1.DocId AS VARCHAR(20)) AS DocId ,CAST(T1.Id AS VARCHAR(20)) AS Id ,T1.DeptNo ,T1.TargetNameC ,T1.TargetNameE ,T1.DeptNameC ,T1.DeptNameE
                 FROM  ESTAERPV2.dbo.CompanyDept AS T1 
                 WHERE  1 = 1  and T1.DocId = 150615163202244 
                 ORDER BY  T1.DeptNo
            ";
            //var param = new { docid };
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    List<VoucherCreateEditPayDeptViewModel> customerdata = conn.Query<VoucherCreateEditPayDeptViewModel>(sql).ToList();
                    if (customerdata.Count > 0)
                    {
                        return Json(new { success = true, code = "OK" , data = customerdata }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {                       
                        return Json(new { success = false, code = "C0003" }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception e)
            {
                return Json(new { success = false, code = "C0004", err= e }, JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// 編輯_活動類別
        /// </summary>
        /// <returns></returns>
        public ActionResult GetEditTableCode7() {
            var dataList = new List<object>
            {
                new { id = "1", ActivityType = "營業活動" },
                new { id = "2", ActivityType = "投資活動" },
                new { id = "3", ActivityType = "融資活動" },
                new { id = "4", ActivityType = "匯率變動" },
                new { id = "5", ActivityType = "所的稅" },
                new { id = "6", ActivityType = "停業單位" },
                new { id = "7", ActivityType = "權益" },
            };
            return Json(dataList, JsonRequestBehavior.AllowGet);
            //return Json(new { success = true, code = "OK", data = dataList }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 新增_表頭資訊_單據類別
        /// </summary>
        /// <returns></returns>
        public ActionResult GetAddInfoCode()
        {
            List<object> dataList;
            switch (currentCulture)
            {               
                case "zh-TW":
                    dataList = new List<object>
                    {
                        new { DocSubType = "A", DocSubTypeName = "一般傳票" },
                        new { DocSubType = "B", DocSubTypeName = "特殊傳票" },
                        //new { DocSubType = "C", DocSubTypeName = "其他傳票" },
                        //new { DocSubType = "E", DocSubTypeName = "年結傳票" },
                    };
                    break;
                case "es-MX":
                    dataList = new List<object>
                    {
                        new { DocSubType = "A", DocSubTypeName = "Citación" },
                        new { DocSubType = "B", DocSubTypeName = "Especial Citación" },
                        //new { DocSubType = "C", DocSubTypeName = "Otra Citación" },
                        //new { DocSubType = "E", DocSubTypeName = "Año Citación" },
                    };
                    break;
                case "en":                   
                default:
                    dataList = new List<object>
                    {
                        new { DocSubType = "A", DocSubTypeName = "Voucher" },
                        new { DocSubType = "B", DocSubTypeName = "Special Voucher" },
                        //new { DocSubType = "C", DocSubTypeName = "Other Voucher" },
                        //new { DocSubType = "E", DocSubTypeName = "Year Voucher" },
                    };
                    break;
            }
            //return Json(dataList, JsonRequestBehavior.AllowGet);
            return Json(new { success = true, code = "OK", data = dataList }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 新增_表頭資訊_公司編號
        /// </summary>
        /// <returns></returns>
        public ActionResult GetAddInfoCode2()
        {
            var dataList = new[]
            {
                new { CompId = "150615163202244", CompNo = "A", CompAbbr = "NES" },
                new { CompId = "5354520274329583096", CompNo = "B", CompAbbr = "RNES" },
                new { CompId = "5563551763276641505", CompNo = "C", CompAbbr = "RUIS SERVICES" },
                new { CompId = "4844350623996401600", CompNo = "F", CompAbbr = "瑞師福委" }
            };                      
            var compNo = Session["CompNo"]?.ToString();                        
            var filteredCompanies = dataList.Where(c => c.CompNo == compNo).ToList();

            return Json(filteredCompanies, JsonRequestBehavior.AllowGet);
            //return Json(new { success = true, code = "OK", data = filteredCompanies }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 新增_表頭資訊_傳票類別
        /// </summary>
        /// <returns></returns>
        public ActionResult GetAddInfoCode3()
        {
            List<object> dataList;

            switch (currentCulture)
            {
                case "zh-TW":
                    dataList = new List<object>
                    {
                        new { VoucherType = "1", VoucherName = "轉帳傳票" }                       
                    };
                    break;
                case "es-MX":
                    dataList = new List<object>
                    {
                        new { VoucherType = "1", VoucherName = "Transferir Citación" }                       
                    };
                    break;
                case "en":
                default:
                    dataList = new List<object>
                    {
                        new { VoucherType = "1", VoucherName = "Transfer Voucher" }                       
                    };
                    break;
            }
            //return Json(dataList, JsonRequestBehavior.AllowGet);
            return Json(new { success = true, code = "OK", data = dataList }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 新增_表頭資訊_交易幣別
        /// </summary>
        /// <returns></returns>
        public ActionResult GetAddInfoCode4()
        {
            string currencyst = (string)Session["CurrencySt"];
            string sql = @"
                select a.CurrencyNo , a.Rate
                from NES_WEB_ACC.dbo.ACC_Rate as a
                    inner join (
	                    select  
	                    [CurrencyNo]
	                    ,[CurrencySt]
	                    ,MAX(ExchangeDate) as ExchangeDate
	                    ,MAX(Rate) as Rate
                    from (
	                    select 
		                    CurrencyNo
		                    ,CurrencySt
		                    ,CONCAT([ExchangeYear], '/', [ExchangeMonth],'/',  case 
			                    when ExchangeMonthFlag = 'B' then  '01' 
			                    when ExchangeMonthFlag = 'M' then '11' 
			                    when ExchangeMonthFlag = 'E' then '21' 
		                    end ) AS ExchangeDate
		                    ,Rate
		                    from NES_WEB_ACC.dbo.ACC_Rate 
	                    ) as a group by a.CurrencyNo,a.CurrencySt
                    ) as b on a.CurrencyNo = b.CurrencyNo
	                    and a.CurrencySt = b.CurrencySt
	                    and a.ExchangeYear = LEFT(b.[ExchangeDate], CHARINDEX('/', b.[ExchangeDate]) - 1)
	                    and a.ExchangeMonth = SUBSTRING(b.[ExchangeDate], CHARINDEX('/', b.[ExchangeDate]) + 1, CHARINDEX('/', b.[ExchangeDate], CHARINDEX('/', b.[ExchangeDate]) + 1) - CHARINDEX('/', b.[ExchangeDate]) - 1)
	                    and a.ExchangeMonthFlag =CASE
                            WHEN RIGHT(b.[ExchangeDate], 2) = '01' THEN 'B'
                            WHEN RIGHT(b.[ExchangeDate], 2) = '11' THEN 'M'
                            WHEN RIGHT(b.[ExchangeDate], 2) = '21' THEN 'E'
                        END
                where a.CurrencySt = @currencyst  --'MXN'
            ";
            var param = new { currencyst };
            List<ACC_Rate> resultdata;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {               
                resultdata = conn.Query<ACC_Rate>(sql,param).ToList();
            }            
           
            return Json(resultdata, JsonRequestBehavior.AllowGet);
            //return Json(new { success = true, code = "OK", data = dataList }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 查詢欄位下拉
        /// </summary>
        /// <returns></returns>
        public ActionResult GetSeacheField() {
            List<object> dataList;
            switch (currentCulture)
            {
                case "zh-TW":
                    dataList = new List<object>
                    {
                        new { Value = "BillNo", Name = "傳票編號" },
                        new { Value = "BillDate", Name = "傳票日期" },
                        new { Value = "EmpNo", Name = "負責會計" },                           
                    };
                    break;
                case "es-MX":
                    dataList = new List<object>
                    {
                        new { Value = "BillNo", Name = "Número de citación" },
                        new { Value = "BillDate", Name = "Fecha de citación" },
                        new { Value = "EmpNo", Name = "No. Contadora" },
                    };
                    break;
                case "en":
                default:
                    dataList = new List<object>
                    {
                        new { Value = "BillNo", Name = "Voucher No" },
                        new { Value = "BillDate", Name = "Voucher Date" },
                        new { Value = "EmpNo", Name = "Accountant No" },
                    };
                    break;
            }
            return Json(dataList, JsonRequestBehavior.AllowGet);
            //return Json(new { success = true, code = "OK", data = dataList }, JsonRequestBehavior.AllowGet);
        }


        //------ 資料寫入 ------//
        /// <summary>
        /// 新增傳票
        /// </summary>
        /// <param name="data"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpPost]
        [CustomAuthorize(Roles = "Admin,AccManager,AccPm,AccUser")]
        public ActionResult AddData(VoucherDataViewModel data, string type)
        {
            if (type == "begin")
            {
                if (!User.IsInRole("Admin") && !User.IsInRole("AccManager"))               
                {
                    return Json(new { success = false, code = "C0005" }, JsonRequestBehavior.AllowGet);
                }
            }
            string voucherDocType = "";
            if (data == null)
            {               
                return Json(new { success = false, code = "C0001" }, JsonRequestBehavior.AllowGet);
            }
            voucherDocType = (type == "begin") ? (data.Maindata.DocSubType == "B") ? "V2" : "false" : (data.Maindata.DocSubType == "B") ? "false" : "V1";
            if (voucherDocType == "false")
            {
                return Json(new { success = false, code = "C0005" }, JsonRequestBehavior.AllowGet);
            }
            try
            {
                #region 參數值設定
                DateTime dateTime = Convert.ToDateTime(data.Maindata.BillDate);
                var billNoCode = _dbContext.ACC_VoucherInfo.Where(v => v.BillDate == dateTime).GroupBy(v => v.BillDate).Select(g => g.Max(v => v.Code2)).FirstOrDefault();
                string str_Code2 = string.IsNullOrEmpty(billNoCode) ? "001"  : (Convert.ToInt64(billNoCode )+ 1).ToString("D3");
                #endregion

                #region 主檔寫入
                var mainData = new ACC_VoucherInfo
                {
                    WebId = Guid.NewGuid(),
                    CompId = data.Maindata.CompId,
                    CompNo = data.Maindata.CompNo,
                    CompAbbr = data.Maindata.CompAbbr,
                    DocType = voucherDocType,
                    DocSubType = data.Maindata.DocSubType,
                    DocSubTypeName = data.Maindata.DocSubTypeName,
                    BillDate = dateTime,
                    Code1 = "V" + data.Maindata.CompNo + dateTime.ToString("yyMMdd"),
                    Code2 = str_Code2,
                    BillNo = "V" + data.Maindata.CompNo + dateTime.ToString("yyMMdd") + str_Code2,
                    DcType = "轉帳",
                    VoucherType = data.Maindata.VoucherType,
                    VoucherNameC = data.Maindata.VoucherNameC,
                    EmpId = Session["EmpId"].ToString(),
                    EmpNo = Session["EmpNo"].ToString(),
                    EmpNameC = Session["EmpNameC"].ToString(),
                    //DeptId = Session["DeptId"].ToString(),
                    DeptNo = Session["DeptNo"].ToString(),
                    DeptName = Session["DeptName"].ToString(),
                    CurrencyNo = data.Maindata.CurrencyNo,
                    CurrencySt = data.Maindata.CurrencySt,
                    Rate1 = Convert.ToDecimal(data.Maindata.Rate1),
                    Rate2 = Convert.ToDecimal(data.Maindata.Rate1),
                    Remark = data.Maindata.Remark,
                    Money11 = data.Dcdata.Money11,
                    Money12 = data.Dcdata.Money12,
                    Money21 = data.Dcdata.Money21,
                    Money22 = data.Dcdata.Money22,
                    Money1Dc = data.Dcdata.Money1Dc,
                    Money2Dc = data.Dcdata.Money2Dc,
                    CreateDate = System.DateTime.Now,
                    CreateBy = Session["EmpNo"].ToString(),
                    IsClosed = (type == "begin") ? true : false,
                    BillStatus = 0,
                };
                var billNo = _dbContext.ACC_VoucherInfo.FirstOrDefault(x => x.BillNo == mainData.BillNo);
                if (billNo != null) return Json(new { success = false, code = "C0002" }, JsonRequestBehavior.AllowGet);
                _dbContext.ACC_VoucherInfo.Add(mainData);
                Guid mnid = Guid.NewGuid();
                MnDataSave(mainData,"1", mnid,"1");
                #endregion
                #region 明細寫入
                foreach (var item in data.Infodata)
                {
                    //取會計科目的資訊
                    var accTitle = _dbContext.ACC_AccTitleNo_MX.FirstOrDefault(x => x.AccNo == item.AccNo);
                    var infoData = new ACC_VoucherDetail
                    {
                        WebId = Guid.NewGuid(),
                        WebDocId = mainData.WebId,
                        Linage = Convert.ToInt32(item.Linage),
                        AccNoWebId = accTitle.WebId,
                        AccNoId = accTitle.Id,
                        AccNo = accTitle.AccNo,
                        AccNameC = accTitle.AccNameC,
                        AccNameE = accTitle.AccNameE,
                        AccNameMX = accTitle.AccNameMX,
                        Remark = item.Remark,
                        DCTypeNo = item.DCTypeNo,
                        DCTypeNameC = item.DCTypeNameC,
                        DCTypeNameMX = (item.DCTypeNo == "D") ? "Débito" : "Crédito",
                        CurrencyNo = item.CurrencyNo,
                        CurrencySt = item.CurrencySt,
                        Rate1 = Convert.ToDecimal(item.Rate1),
                        Rate2 = Convert.ToDecimal(item.Rate1),
                        Money = item.Money,
                        Money1 = item.Money1,
                        Money2 = item.Money1,
                        AccProfitId = item.AccProfitId,
                        AccProfitNo = item.AccProfitNo,
                        AccProfitName = item.AccProfitName,
                        CreateDate = System.DateTime.Now,
                        CreateBy = Session["EmpNo"].ToString(),
                    };
                    _dbContext.ACC_VoucherDetail.Add(infoData);
                    MnDataSave(infoData, "1", mnid, "1");
                }
                #endregion
                _dbContext.SaveChanges();
                return Json(new { success = true, code = "OK", data = mainData.BillNo }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {               
                return Json(new { success = false, code = "C0004" , err = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// 編輯傳票
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [CustomAuthorize(Roles = "Admin,AccManager,AccPm,AccUser")]
        public ActionResult EditData(VoucherDataViewModel data, string type) {
            if (data == null)
            {                
                return Json(new { success = false, code = "C0001" }, JsonRequestBehavior.AllowGet);
            }
            Guid guidId;
            if (!Guid.TryParse(data.Maindata.WebId, out guidId))
            {
                return Json(new { success = false, code = "C0001" }, JsonRequestBehavior.AllowGet);
            }
            try
            {
                Guid mnid = Guid.NewGuid();                
                var existdata = _dbContext.ACC_VoucherInfo.FirstOrDefault(x => x.WebId == guidId);
                if (existdata != null)
                {
                    existdata.Money11 = data.Dcdata.Money11;
                    existdata.Money12 = data.Dcdata.Money12;
                    existdata.Money21 = data.Dcdata.Money21;
                    existdata.Money22 = data.Dcdata.Money22;
                    existdata.Money1Dc = data.Dcdata.Money1Dc;
                    existdata.Money2Dc = data.Dcdata.Money2Dc;
                    MnDataSave(existdata, "2", mnid, "2");
                    var itemToDelete = _dbContext.ACC_VoucherDetail.Where(x => x.WebDocId == existdata.WebId).ToList();                                       
                    foreach (var item in itemToDelete)
                    {
                        _dbContext.ACC_VoucherDetail.Remove(item);
                        MnDataSave(item, "3", mnid, "3");
                    }
                    foreach (var item in data.Infodata)
                    {
                        //取會計科目的資訊
                        var accTitle = _dbContext.ACC_AccTitleNo_MX.FirstOrDefault(x => x.AccNo == item.AccNo);
                        var infoData = new ACC_VoucherDetail
                        {
                            WebId = Guid.NewGuid(),
                            WebDocId = existdata.WebId,
                            Linage = Convert.ToInt32(item.Linage),
                            AccNoWebId = accTitle.WebId,
                            AccNoId = accTitle.Id,
                            AccNo = accTitle.AccNo,
                            AccNameC = accTitle.AccNameC,
                            AccNameE = accTitle.AccNameE,
                            AccNameMX = accTitle.AccNameMX,
                            Remark = item.Remark,
                            DCTypeNo = item.DCTypeNo,
                            DCTypeNameC = (item.DCTypeNo == "D") ? "借方" : "貸方",
                            DCTypeNameMX = (item.DCTypeNo == "D") ? "Débito" : "Crédito",
                            CurrencyNo = item.CurrencyNo,
                            CurrencySt = item.CurrencySt,
                            Rate1 = Convert.ToDecimal(item.Rate1),
                            Rate2 = Convert.ToDecimal(item.Rate1),
                            Money = item.Money,
                            Money1 = item.Money1,
                            Money2 = item.Money1,
                            AccProfitId = item.AccProfitId,
                            AccProfitNo = item.AccProfitNo,
                            AccProfitName = item.AccProfitName,
                            CreateDate = System.DateTime.Now,
                            CreateBy = Session["EmpNo"].ToString(),
                        };
                        _dbContext.ACC_VoucherDetail.Add(infoData);
                        MnDataSave(infoData, "2", mnid, "2");
                    }
                    _dbContext.SaveChanges();
                    return Json(new { success = true, code = "OK", data = existdata.BillNo }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false, code = "C0003" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {                
                return Json(new { success = false, code = "C0004" , err = e }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        [CustomAuthorize(Roles = "Admin,AccManager,AccPm,AccUser")]
        public ActionResult VoucherStatus(string webid, string type)
        {
            if (String.IsNullOrEmpty(webid))
            {
                return Json(new { success = false, code = "C0001" }, JsonRequestBehavior.AllowGet);
            }
            Guid guidId;
            if (!Guid.TryParse(webid, out guidId))
            {
                return Json(new { success = false, code = "C0001" }, JsonRequestBehavior.AllowGet);
            }

            try
            {
                Guid mnid = Guid.NewGuid();
                var existdata = _dbContext.ACC_VoucherInfo.FirstOrDefault(x => x.WebId == guidId);
                if (existdata != null)
                {
                    switch (type)
                    {
                        case "lock":
                            existdata.IsChecked = true;
                            existdata.CheckDate = System.DateTime.Now;
                            existdata.CheckBy = Session["EmpNo"].ToString();
                            existdata.BillStatus = 1;
                            break;
                        case "check":
                            if (User.IsInRole("Admin") || User.IsInRole("AccManager") || User.IsInRole("AccPm")) {
                                existdata.IsState = true;
                                existdata.StateDate = System.DateTime.Now;
                                existdata.StateBy = Session["EmpNo"].ToString();
                                existdata.BillStatus = 2;
                            } else
                            {
                                return Json(new { success = false, code = "C0005" }, JsonRequestBehavior.AllowGet);
                            }
                            break;
                        case "close":
                            if (User.IsInRole("Admin") || User.IsInRole("AccManager"))
                            {
                                existdata.IsClosed = true;
                                existdata.ClosedDate = System.DateTime.Now;
                                existdata.ClosedBy = Session["EmpNo"].ToString();
                                existdata.BillStatus = 3;
                            }
                            else
                            {
                                return Json(new { success = false, code = "C0005" }, JsonRequestBehavior.AllowGet);
                            }                            
                            break;
                        case "reject":
                            if (User.IsInRole("Admin") || User.IsInRole("AccManager") || User.IsInRole("AccPm"))
                            {
                                existdata.IsChecked = false;
                                existdata.BillStatus = 4;
                            }
                            else
                            {
                                return Json(new { success = false, code = "C0005" }, JsonRequestBehavior.AllowGet);
                            }                            
                            break;
                        case "reject2":
                            if (User.IsInRole("Admin") || User.IsInRole("AccManager"))
                            {
                                existdata.IsChecked = false;
                                existdata.IsState = false;
                                existdata.BillStatus = 5;
                            }
                            else
                            {
                                return Json(new { success = false, code = "C0005" }, JsonRequestBehavior.AllowGet);
                            }                           
                            break;
                        case "scrap":
                            existdata.IsClosed = true;
                            existdata.ClosedDate = System.DateTime.Now;
                            existdata.ClosedBy = Session["EmpNo"].ToString();
                            existdata.BillStatus = 9;
                            break;
                    }
                    existdata.SignDate = System.DateTime.Now;
                    existdata.SignBy = Session["EmpNo"].ToString();
                    MnDataSave(existdata, "2", mnid, "2");
                    _dbContext.SaveChanges();
                    return Json(new { success = true, code = "OK", data = existdata.BillNo }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false, code = "C0003"}, JsonRequestBehavior.AllowGet);
                }                
            }
            catch (Exception e)
            {
                return Json(new { success = false, code = "C0004", err = e }, JsonRequestBehavior.AllowGet);
            }
        }        
        [HttpDelete]
        [CustomAuthorize(Roles = "Admin,AccManager,AccPm,AccUser")]
        public ActionResult DeleteData(string webid)
        {
            if (String.IsNullOrEmpty(webid))
            {
                return Json(new { success = false, code = "C0001" }, JsonRequestBehavior.AllowGet);
            }
            Guid guidId;
            if (!Guid.TryParse(webid, out guidId))
            {
                return Json(new { success = false, code = "C0001" }, JsonRequestBehavior.AllowGet);
            }
            try
            {
                Guid mnid = Guid.NewGuid();
                var existdataInfo = _dbContext.ACC_VoucherInfo.FirstOrDefault(x => x.WebId == guidId);
                var existdataDetail = _dbContext.ACC_VoucherDetail.Where(x => x.WebDocId == guidId).ToList(); ;
                if (existdataInfo == null || existdataDetail == null)
                {
                    return Json(new { success = false, code = "C0003" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    _dbContext.ACC_VoucherInfo.Remove(existdataInfo);
                    MnDataSave(existdataInfo, "3", mnid, "3");
                    foreach (var item in existdataDetail)
                    {
                        _dbContext.ACC_VoucherDetail.Remove(item);
                        MnDataSave(item, "3", mnid, "3");
                    }
                    _dbContext.SaveChanges();
                    return Json(new { success = true, code = "OK", data = existdataInfo.BillNo }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception e)
            {
                return Json(new { success = false, code = "C0004", err = e }, JsonRequestBehavior.AllowGet);
            }
        }


        //------ 部分檢視 ------//
        /// <summary>
        /// 部分檢視_表頭欄位
        /// </summary>
        /// <returns></returns>
        public ActionResult _VoucherTableInfoPartial()
        {
            return PartialView();
        }
        /// <summary>
        /// 部分檢視_表頭欄位_進階資料
        /// </summary>
        /// <returns></returns>
        public ActionResult _VoucherTableAdvPartial()
        {
            return PartialView();
        }
        /// <summary>
        /// 部分檢視_借貸平衡
        /// </summary>
        /// <returns></returns>
        public ActionResult _VoucherDCShowPartial()
        {
            return PartialView();
        }
        /// <summary>
        /// 工具列介面
        /// </summary>
        /// <returns></returns>
        public ActionResult _ToolBarPartial()
        {
            return PartialView();
        }
        /// <summary>
        /// 查詢介面
        /// </summary>
        /// <returns></returns>
        public ActionResult _SearchPartial()
        {
            return PartialView();
        }

        /// <summary>
        /// 異動記錄
        /// </summary>
        public void MnDataSave(object list, string mntype, Guid mnid, string modeltype)
        {
            Type modelname;
            string tablename = "";
            modelname = list.GetType();
            tablename = modelname.Name.ToString();
            SYS_OperationHistory mndata = new SYS_OperationHistory();
            string EntityColumns = "", EntityData = "", mnstatus = "";
            foreach (var prop in modelname.GetProperties())
            {
                EntityColumns += "✏" + prop.Name;
                if (prop.GetValue(list) != null)
                {
                    EntityData += "✏" + prop.GetValue(list).ToString();
                }
                else
                {
                    EntityData += "✏" + " ";
                }
            }
            switch (mntype)
            {
                case "1":
                    mnstatus = "ADD";
                    break;
                case "2":
                    mnstatus = "EDIT";
                    break;
                case "3":
                    mnstatus = "DEL";
                    break;
            }
            mndata.MnId = mnid;
            mndata.OperationType = mntype;
            mndata.OperationStatus = mnstatus;
            mndata.TableName = tablename;
            mndata.EntityColumns = EntityColumns;
            mndata.EntityData = EntityData;
            mndata.EditEmpId = Convert.ToInt64(Session["EmpId"].ToString());
            mndata.EditBy = Session["EmpNo"].ToString();
            mndata.EditDate = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            _dbContext.SYS_OperationHistory.Add(mndata);
        }
    }
}