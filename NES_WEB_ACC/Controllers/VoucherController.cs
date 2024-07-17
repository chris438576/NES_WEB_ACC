using Dapper;
using Microsoft.Reporting.WebForms;
using NES_WEB_ACC.Report;
using NES_WEB_ACC.Report.RDLC;
using NES_WEB_ACC.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Newtonsoft.Json;


namespace NES_WEB_ACC.Controllers
{
    public class VoucherController : Controller
    {
        public string connectionString = ConfigurationManager.ConnectionStrings["NES_WEB_ACCConnectionString"].ConnectionString;
        private NES_WEB_ACCEntities _dbContext = new NES_WEB_ACCEntities();
        
        public ActionResult VoucherCreate()
        {
            return View();
        }
       
        public ActionResult GetVoucherInfo()
        {
            string sql = @"
                            SELECT [Id]
                                  ,[CompId]
                                  ,[CompNo]
                                  ,[CompAbbr]
                                  ,[AccountBill]
                                  ,[DocType]
                                  ,[DocSubType]
                                  ,[DocSubTypeName]
                                  ,CONVERT(VARCHAR, BillDate, 111) as 'BillDate'
                                  ,[Code1]
                                  ,[Code2]
                                  ,[BillNo]
                                  ,[DcType]
                                  ,[VoucherType]
                                  ,[VoucherNameC]
                                  ,[EmpId]
                                  ,[EmpNo]
                                  ,[EmpNameC]
                                  ,[DeptId]
                                  ,[DeptNo]
                                  ,[DeptName]
                                  ,[CurrencyNo]
                                  ,[Rate1]
                                  ,[Rate2]
                                  ,[SourceCompId]
                                  ,[SourceProjectId]
                                  ,[SourceDocSubType]
                                  ,[SourceDocSubTypeName]
                                  ,[SourceDocId]
                                  ,[SourceNo]
                                  ,[BillAddType]
                                  ,[Remark]
                                  ,[ActivityType]
                                  ,[AccDocType]
                                  ,[Money11]
                                  ,[Money12]
                                  ,[Money21]
                                  ,[Money22]
                                  ,[Money1Dc]
                                  ,[Money2Dc]
                                  ,[Flag]
                                  ,[IsState]
                                  ,[StateDate]
                                  ,[StateBy]
                                  ,[IsChecked]
                                  ,[CheckDate]
                                  ,[CheckBy]
                                  ,[CreateDate]
                                  ,[CreateBy]
                                  ,[BillStatus]
                                  ,[SignDate]
                                  ,[SignBy]
                              FROM [ACC_VoucherInfo]
　                            where Id in(
	                        SELECT  DISTINCT top(200)
		                        T1.Id 
	                         FROM 
		                        ACC_VoucherInfo as T1 
		                        left join ACC_DataSet AS T7 on (T1. AccountBill = T7.Id and T7.GroupNo = N'AccountBill')                                     
	                        WHERE T1.CompId in ( -1 , 150615163202244 ) And T1.DocType = N'V1'  
	                        )
                                ";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    List<ACC_VoucherInfo> resultdata = conn.Query<ACC_VoucherInfo>(sql).ToList();
                    if (resultdata.Count > 0)
                    {
                        var formatData = resultdata.Select(item => new
                        {
                            Id = item.Id.ToString(), // 將Id轉換為字符串
                            CompId = item.CompId,
                            CompNo = item.CompNo,
                            CompAbbr = item.CompAbbr,
                            AccountBill = item.AccountBill,
                            DocType = item.DocType,
                            DocSubType = item.DocSubType,
                            DocSubTypeName = item.DocSubTypeName,
                            BillDate = item.BillDate,
                            Code1 = item.Code1,
                            Code2 = item.Code2,
                            BillNo = item.BillNo,
                            DcType = item.DcType,
                            VoucherType = item.VoucherType,
                            VoucherNameC = item.VoucherNameC,
                            EmpId = item.EmpId,
                            EmpNo = item.EmpNo,
                            EmpNameC = item.EmpNameC,
                            DeptId = item.DeptId,
                            DeptNo = item.DeptNo,
                            DeptName = item.DeptName,
                            CurrencyNo = item.CurrencyNo,
                            Rate1 = item.Rate1,
                            Rate2 = item.Rate2,
                            SourceCompId = item.SourceCompId,
                            SourceProjectId = item.SourceProjectId,
                            SourceDocSubType = item.SourceDocSubType,
                            SourceDocSubTypeName = item.SourceDocSubTypeName,
                            SourceDocId = item.SourceDocId,
                            SourceNo = item.SourceNo,
                            BillAddType = item.BillAddType,
                            Remark = item.Remark,
                            ActivityType = item.ActivityType,
                            AccDocType = item.AccDocType,
                            Money11 = item.Money11,
                            Money12 = item.Money12,
                            Money21 = item.Money21,
                            Money22 = item.Money22,
                            Money1Dc = item.Money1Dc,
                            Money2Dc = item.Money2Dc,
                            Flag = item.Flag,
                            IsState = item.IsState,
                            StateDate = item.StateDate,
                            StateBy = item.StateBy,
                            IsChecked = item.IsChecked,
                            CheckDate = item.CheckDate,
                            CheckBy = item.CheckBy,
                            CreateDate = item.CreateDate,
                            CreateBy = item.CreateBy,
                            BillStatus = item.BillStatus,
                            SignDate = item.SignDate,
                            SignBy = item.SignBy
                        }).ToList();
                        return Json(formatData, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        Response.StatusCode = (int)HttpStatusCode.OK;
                        return Json("C0003", JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("C0004", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult GetVoucherItem(string docid)
        {
            if (string.IsNullOrEmpty(docid))
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("C0001", JsonRequestBehavior.AllowGet);
            }
            string sql = @"select * from ACC_VoucherDetail where DocId = @docid";
            var param = new { docid };
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    List<ACC_VoucherDetail> customerdata = conn.Query<ACC_VoucherDetail>(sql, param).ToList();
                    if (customerdata.Count > 0)
                    {
                        var formatData = customerdata.Select(item => new {
                            Id = item.Id.ToString(), // 將Id轉換為字符串
                            DocId = item.DocId,
                            Linage = item.Linage,
                            AccNoId = item.AccNoId,
                            AccNo = item.AccNo,
                            AccNameC = item.AccNameC,
                            AccNameE = item.AccNameE,
                            Remark = item.Remark,
                            DCTypeNo = item.DCTypeNo,
                            DCTypeNameC = item.DCTypeNameC,
                            CurrencyNo = item.CurrencyNo,
                            Rate1 = item.Rate1,
                            Rate2 = item.Rate2,
                            Money = item.Money,
                            Money1 = item.Money1,
                            Money2 = item.Money2,
                            AccProfitId = item.AccProfitId,
                            AccProfitNo = item.AccProfitNo,
                            AccProfitName = item.AccProfitName,
                            AccDeptId = item.AccDeptId,
                            AccDeptNo = item.AccDeptNo,
                            AccDeptName = item.AccDeptName,
                            PayDeptId = item.PayDeptId,
                            PayDeptNo = item.PayDeptNo,
                            PayDeptName = item.PayDeptName,
                            TargetType = item.TargetType,
                            TargetId = item.TargetId,
                            TargetNo = item.TargetNo,
                            TargetAbbr = item.TargetAbbr,
                            OffsetNo = item.OffsetNo,
                            CaseBillId = item.CaseBillId,
                            CaseBillNo = item.CaseBillNo,
                            SourceProjectId = item.SourceProjectId,
                            SourceDocSubType = item.SourceDocSubType,
                            SourceDocSubTypeName = item.SourceDocSubTypeName,
                            SourceDocId = item.SourceDocId,
                            SourceSeqId = item.SourceSeqId,
                            SourceNo = item.SourceNo,
                            InitialProjectId = item.InitialProjectId,
                            InitialDocSubType = item.InitialDocSubType,
                            InitialDocSubTypeName = item.InitialDocSubTypeName,
                            InitialDocId = item.InitialDocId,
                            InitialNo = item.InitialNo,
                            CheckType = item.CheckType,
                            CheckId = item.CheckId,
                            CheckNo = item.CheckNo,
                            EventType = item.EventType,
                            ActivityType = item.ActivityType,
                            IsParty = item.IsParty,
                            BillAddType = item.BillAddType,
                            Flag = item.Flag,
                            IsState = item.IsState,
                            StateDate = item.StateDate,
                            StateBy = item.StateBy,
                            IsChecked = item.IsChecked,
                            CheckDate = item.CheckDate,
                            CheckBy = item.CheckBy,
                            CreateDate = item.CreateDate,
                            CreateBy = item.CreateBy,
                            ShowPage = item.ShowPage
                        }).ToList();
                        return Json(formatData, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        Response.StatusCode = (int)HttpStatusCode.OK;
                        return Json("C0003", JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("C0004", JsonRequestBehavior.AllowGet);
            }
        }        
        /// <summary>
        /// 會計科目
        /// </summary>
        /// <returns></returns>
        public ActionResult GetEditTableCode()
        {
            //if (string.IsNullOrEmpty(docid))
            //{
            //    Response.StatusCode = (int)HttpStatusCode.BadRequest;
            //    return Json("C0001", JsonRequestBehavior.AllowGet);
            //}
            string sql = @"select AccNo,AccNameC,AccNoBy,AccNoByNameC,AccGroupNo,AccGroupNameC,DCTypeNo,DCTypeNameC  from NES_WEB_ACC.dbo.ACC_AccTitleNo where CompId = '150615163202244' 
            ";
            //var param = new { docid };
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    List<ACC_AccTitleNo> customerdata = conn.Query<ACC_AccTitleNo>(sql).ToList();
                    if (customerdata.Count > 0)
                    {
                        return Json(customerdata, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        Response.StatusCode = (int)HttpStatusCode.OK;
                        return Json("C0003", JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("C0004", JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// 對象別
        /// </summary>
        /// <returns></returns>
        public ActionResult GetEditTableCode2()
        {
            // 創建一個包含數據的列表
            var targetTypes = new List<object>
            {
            new { TargetType = "15", TargetTypeName = "廠商" },
            new { TargetType = "14", TargetTypeName = "客戶" },
            new { TargetType = "12", TargetTypeName = "員工" },
            new { TargetType = "65", TargetTypeName = "銀行" },
            };
            
            return Json(targetTypes, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 幣別
        /// </summary>
        /// <returns></returns>
        public ActionResult GetEditTableCode3()
        {
            // 創建一個包含數據的列表
            var targetTypes = new List<object>
            {
            new { CurrencyNo = "NT$" },
            new { CurrencyNo = "USD" },
            new { CurrencyNo = "RMB" },
            new { CurrencyNo = "EUR" },
            new { CurrencyNo = "HKD" },
            new { CurrencyNo = "JPY" },
            new { CurrencyNo = "SGD" },
            new { CurrencyNo = "GBP" },
            new { CurrencyNo = "CAD" },
            new { CurrencyNo = "KRW" },
            new { CurrencyNo = "VND" },
            new { CurrencyNo = "AUD" },
            new { CurrencyNo = "PLN" },
            new { CurrencyNo = "CHF" },
            new { CurrencyNo = "MXN" },
            new { CurrencyNo = "CZK" },
            };

            return Json(targetTypes, JsonRequestBehavior.AllowGet);
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
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("C0001", JsonRequestBehavior.AllowGet);
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
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return Json("C0002", JsonRequestBehavior.AllowGet); 
            }
             
            //var param = new { docid };
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    var customerdata = conn.Query(viewModelType, sql).ToList();
                    if (customerdata.Count > 0)
                    {
                        return Json(customerdata, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        Response.StatusCode = (int)HttpStatusCode.OK;
                        return Json("C0003", JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("C0004", JsonRequestBehavior.AllowGet);
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
                        return Json(customerdata, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        Response.StatusCode = (int)HttpStatusCode.OK;
                        return Json("C0003", JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("C0004", JsonRequestBehavior.AllowGet);
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
                        return Json(customerdata, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        Response.StatusCode = (int)HttpStatusCode.OK;
                        return Json("C0003", JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("C0004", JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// 活動類別
        /// </summary>
        /// <returns></returns>
        public ActionResult GetEditTableCode7() {
            var docSubTypes = new List<object>
            {
                new { id = "1", ActivityType = "營業活動" },
                new { id = "2", ActivityType = "投資活動" },
                new { id = "3", ActivityType = "融資活動" },
                new { id = "4", ActivityType = "匯率變動" },
                new { id = "5", ActivityType = "所的稅" },
                new { id = "6", ActivityType = "停業單位" },
                new { id = "7", ActivityType = "權益" },
            };

            return Json(docSubTypes, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 新增_表頭資訊_單據類別
        /// </summary>
        /// <returns></returns>
        public ActionResult GetAddInfoCode()
        {
            var docSubTypes = new List<object>
            {
                new { DocSubType = "A", DocSubTypeName = "一般傳票" },
                new { DocSubType = "B", DocSubTypeName = "特殊傳票" },
                new { DocSubType = "C", DocSubTypeName = "其他傳票" },
                new { DocSubType = "E", DocSubTypeName = "年結傳票" },
            };

            return Json(docSubTypes, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 新增_表頭資訊_公司編號
        /// </summary>
        /// <returns></returns>
        public ActionResult GetAddInfoCode2()
        {
            var companies = new List<object>
            {
                new { CompId = "150615163202244", CompNo = "A", CompAbbr = "NES" },
                new { CompId = "5354520274329583096", CompNo = "B", CompAbbr = "RNES" },
                new { CompId = "5563551763276641505", CompNo = "C", CompAbbr = "RUIS SERVICES" },
                new { CompId = "4844350623996401600", CompNo = "F", CompAbbr = "瑞師福委" }
            };

            return Json(companies, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 新增_表頭資訊_傳票類別
        /// </summary>
        /// <returns></returns>
        public ActionResult GetAddInfoCode3()
        {
            var voucherTypes = new List<object>
            {
                new { VoucherType = "1", VoucherNameC = "轉帳傳票" }
            };
            return Json(voucherTypes, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 新增_表頭資訊_交易幣別
        /// </summary>
        /// <returns></returns>
        public ActionResult GetAddInfoCode4()
        {          
            var currencies = new List<object>
            {
                new { CurrencyNo = "NT$", Rate1 = "1" },
                new { CurrencyNo = "RMB", Rate1 = "4.28760" },
                new { CurrencyNo = "CAD", Rate1 = "23.23800" },
                new { CurrencyNo = "USD", Rate1 = "31.84000" },
                new { CurrencyNo = "CHF", Rate1 = "30.06000" },
                new { CurrencyNo = "EUR", Rate1 = "31.16500" }
            };
            return Json(currencies, JsonRequestBehavior.AllowGet);
        }       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult PostAddData(VoucherDataViewModel data)
        {
            if (data == null)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("C0001", JsonRequestBehavior.AllowGet);
            }
            try
            {             

                return Json(new { success = true, message = "資料已成功儲存." }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { success = false, message = "資料處理時發生錯誤." }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult PostEditData(VoucherDataViewModel data) {
            if (data == null)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("C0001", JsonRequestBehavior.AllowGet);
            }
            try
            {

                return Json(new { success = true, message = "資料已成功修改." }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { success = false, message = "資料處理時發生錯誤." }, JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult VoucherCheck()
        {
            return View();
        }
     

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
        /// 
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
    }
}