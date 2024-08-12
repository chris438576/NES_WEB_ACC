using Dapper;
using NES_WEB_ACC.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace NES_WEB_ACC.Controllers
{
    public class AccSetController : Controller
    {
        public string connectionString = ConfigurationManager.ConnectionStrings["NES_WEB_ACCConnectionString"].ConnectionString;
        private NES_WEB_ACCEntities _dbContext = new NES_WEB_ACCEntities();
        
        /// <summary>
        /// 介面_會計科目
        /// </summary>
        /// <returns></returns>
        public ActionResult AccTitle(int? scroll, string msg)
        {
            ViewBag.Scroll = (scroll == null) ? 0 : scroll;
            ViewBag.Msg = (String.IsNullOrEmpty(msg)) ? null : msg;
            return View();
        }
        public ActionResult GetAccTitleNo()
        {
            string compid, compno, compabbr;
            try
            {
                compid = Session["CompId"].ToString();
                compno = Session["CompNo"].ToString();
                compabbr = Session["CompAbbr"].ToString();
            }
            catch (Exception e)
            {
                return Json(new { success = false, code = "C0001", err = e }, JsonRequestBehavior.AllowGet);
            }
           
            string sql = @"  
                select * from ACC_AccTitleNo_MX 
                where   1=1
                    and ([CompId] = @compid and [CompNo] = @compno and [CompAbbr] = @compabbr) 
                order by AccNo
            ";
            var param = new { compid, compno, compabbr };
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    List<ACC_AccTitleNo_MX> customerdata = conn.Query<ACC_AccTitleNo_MX>(sql, param).ToList();
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
                return Json(new { success = false, code = "C0004", err = e }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult UpdateIsState(string type, string webid)
        {
            if (string.IsNullOrEmpty(type) || string.IsNullOrEmpty(webid) || webid == "00000000-0000-0000-0000-000000000000")
            {
                return Json(new { success = false, code = "C0001"}, JsonRequestBehavior.AllowGet);
            }
            Guid guidId;
            if (!Guid.TryParse(webid, out guidId))
            {
                return Json(new { success = false, code = "C0001" }, JsonRequestBehavior.AllowGet);
            }
            if (type != "enable" && type != "disable")
            {
                return Json(new { success = false, code = "C0001" }, JsonRequestBehavior.AllowGet);
            }

            try
            {

                var existdata = _dbContext.ACC_AccTitleNo_MX.FirstOrDefault(x => x.WebId == guidId);
                if (existdata != null)
                {
                    switch (type)
                    {
                        case "enable":
                            existdata.IsState = true;
                            break;
                        case "disable":
                            existdata.IsState = false;
                            break;                       
                    }
                    existdata.StateBy = Session["EmpNo"].ToString();
                    existdata.StateDate = System.DateTime.Now;
                    _dbContext.SaveChanges();
                    return Json(new { success = true, code = "OK", data= existdata.AccNo }, JsonRequestBehavior.AllowGet);
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
        [HttpPost]
        public ActionResult EditData(string type, ACC_AccTitleNo_MX data)
        {
            if (string.IsNullOrEmpty(type) || data is null || data.WebId == null )
            {
                return Json(new { success = false, code = "C0001" }, JsonRequestBehavior.AllowGet);
            }
            if (type != "edit")
            {
                return Json(new { success = false, code = "C0001" }, JsonRequestBehavior.AllowGet);
            }
            try
            {
                var existdata = _dbContext.ACC_AccTitleNo_MX.FirstOrDefault(x => x.WebId == data.WebId);
                if (existdata != null)
                {
                    existdata.AccNo = data.AccNo;
                    existdata.AccNameC = data.AccNameC;
                    existdata.AccNameE = data.AccNameE;
                    existdata.AccNameMX = data.AccNameMX;
                    existdata.DCTypeNo = data.DCTypeNo;
                    existdata.StateBy = Session["EmpNo"].ToString();
                    existdata.StateDate = System.DateTime.Now;
                    _dbContext.SaveChanges();
                    return Json(new { success = true, code = "OK", data = existdata.AccNo }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false, code = "C0003" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(new { success = false, code = "C0004", err = e }, JsonRequestBehavior.AllowGet);
            }            
        }
        [HttpPost]
        public ActionResult AddData(string type, ACC_AccTitleNo_MX data)
        {
            if (string.IsNullOrEmpty(type) || data is null)
            {
                return Json(new { success = false, code = "C0001" }, JsonRequestBehavior.AllowGet);
            }
            if (type != "add")
            {
                return Json(new { success = false, code = "C0001" }, JsonRequestBehavior.AllowGet);
            }
            try
            {
                var existdata = _dbContext.ACC_AccTitleNo_MX.FirstOrDefault(x => x.AccNo == data.AccNo);
                if (existdata != null)
                {
                    return Json(new { success = false, code = "C0002" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var insertdata = new ACC_AccTitleNo_MX
                    {
                        WebId = Guid.NewGuid(),
                        CompId = Session["CompId"].ToString(),
                        CompNo = Session["CompNo"].ToString(),
                        CompAbbr = Session["CompAbbr"].ToString(),
                        AccNo = data.AccNo,
                        AccNameC = data.AccNameC,
                        AccNameE = data.AccNameE,
                        AccNameMX = data.AccNameMX,
                        IsState = true,
                        CreateBy = Session["EmpNo"].ToString(),
                        CreateDate = System.DateTime.Now
                    };
                    _dbContext.ACC_AccTitleNo_MX.Add(insertdata);
                    _dbContext.SaveChanges();
                    return Json(new { success = true, code = "OK", data = insertdata.AccNo }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(new { success = false, code = "C0004", err = e }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 介面_匯率
        /// </summary>
        /// <returns></returns>
        public ActionResult Rate(int? scroll, string msg)
        {
            var viewModel = new CbxDataViewModel
            {
                CurrencyNo = GetData(1),
                CurrencySt = GetData(2)
            };           

            var currentCulture = Thread.CurrentThread.CurrentCulture.Name;           
           
            ViewBag.CurrentCulture = currentCulture;
            ViewBag.Scroll = (scroll == null) ? 0 : scroll;
            ViewBag.Msg = (String.IsNullOrEmpty(msg)) ? null : msg;

            return View(viewModel);
        }
        private List<ListViewModel> GetData(int docType)
        {
            var result = new List<ListViewModel>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "select ElmVal as Id, ElmTxt as Name from NES_WEB_ACC.dbo.ACC_SysCode where DocType = @DocType";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@DocType", docType);
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(new ListViewModel
                            {
                                Id = reader["Id"].ToString(),
                                Name = reader["Name"].ToString()
                            });
                        }
                    }
                }
            }
            return result;
        }        
        public ActionResult GetRate(string currencyno, string currencyst, string ratedate)
        {
            string sql;
            object param = null;
            if (string.IsNullOrEmpty(currencyno) || string.IsNullOrEmpty(currencyst))
            {
                return Json(new { success = false, code = "C0001" }, JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(ratedate) || ratedate == "")
            {
                sql = @"
                    select * 
                        ,case ExchangeMonthFlag
                            WHEN 'B' THEN 1
                            WHEN 'M' THEN 2
                            WHEN 'E' THEN 3
                          END as ob
                    from NES_WEB_ACC.dbo.ACC_Rate 
                    where CurrencyNo = @currencyno and CurrencySt = @currencyst 
                    order by ExchangeYear desc ,ExchangeMonth desc,ob desc
                ";
                param = new { currencyno, currencyst };
            }
            else
            {
                sql = @"
                    select * 
                        ,case ExchangeMonthFlag
                            WHEN 'B' THEN 1
                            WHEN 'M' THEN 2
                            WHEN 'E' THEN 3
                          END as ob
                    from NES_WEB_ACC.dbo.ACC_Rate 
                    where CurrencyNo = @currencyno and CurrencySt = @currencyst and ExchangeYear = @exYear and ExchangeMonth = @exMonth and ExchangeMonthFlag = @exFlag 
                    order by ExchangeYear desc ,ExchangeMonth desc,ob desc
                ";
                string[] dateParts = ratedate.Split('/');
                int exYear = Convert.ToInt32(dateParts[0]);
                int exMonth = Convert.ToInt32(dateParts[1]);
                string exFlag = dateParts[2];
                param = new { currencyno, currencyst , exYear , exMonth, exFlag };
            }           

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    List<ACC_Rate> resultdata = conn.Query<ACC_Rate>(sql,param).ToList();
                    if (resultdata.Count > 0)
                    {
                        return Json(new { success = true, code = "OK", data = resultdata }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { success = false, code = "C0003" }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception e)
            {
                return Json(new { success = false, code = "C0004", err = e }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult UpdateRate(Guid webid, decimal rate)
        {
            if (webid == Guid.Empty || rate < 0)
            {
                return Json(new { success = false, code = "C0001"}, JsonRequestBehavior.AllowGet);
            }
            try
            {
                var existdata = _dbContext.ACC_Rate.FirstOrDefault(x => x.WebId == webid);
                if (existdata != null)
                {
                    existdata.Rate = rate;
                    existdata.UpdateEmpId = Session["EmpId"].ToString();
                    existdata.UpdateEmpNo = Session["EmpNo"].ToString();
                    existdata.UpdateDate = System.DateTime.Now;
                    _dbContext.SaveChanges();
                    return Json(new { success = true, code = "OK", data = $"{existdata.ExchangeYear}/{existdata.ExchangeMonth}/{existdata.ExchangeMonthFlag}" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false, code = "C0003" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e )
            {
                return Json(new { success = false, code = "C0004", err = e }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult AddRate(string currencyno, string currencyst, decimal rate, string ratedate)
        {
            if (string.IsNullOrEmpty(currencyno) || string.IsNullOrEmpty(currencyst) || string.IsNullOrEmpty(ratedate) || rate < 0)
            {
                return Json(new { success = false, code = "C0001" }, JsonRequestBehavior.AllowGet);
            }
            try
            {
                #region 參數設定
                string[] dateParts = ratedate.Split('/');
                int exYear = Convert.ToInt32(dateParts[0]);
                int exMonth = Convert.ToInt32(dateParts[1]);
                string exFlag = dateParts[2];
                #endregion
                var existdata = _dbContext.ACC_Rate.FirstOrDefault(x => x.CurrencyNo == currencyno && x.CurrencySt == currencyst && x.ExchangeYear == exYear && x.ExchangeMonth == exMonth && x.ExchangeMonthFlag == exFlag);
                if (existdata != null)
                {
                    return Json(new { success = false, code = "C0002" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var insrtdata = new ACC_Rate
                    {
                        WebId = Guid.NewGuid(),
                        CurrencyNo = currencyno,
                        CurrencySt = currencyst,
                        ExchangeYear = exYear,
                        ExchangeMonth = exMonth,
                        ExchangeMonthFlag = exFlag,
                        CreateEmpId = Session["EmpId"].ToString(),
                        CreateEmpNo = Session["EmpNo"].ToString(),
                        CreatEmpDate = System.DateTime.Now
                    };
                    _dbContext.ACC_Rate.Add(insrtdata);
                    return Json(new { success = true, code = "OK", data = $"{existdata.ExchangeYear}/{existdata.ExchangeMonth}/{existdata.ExchangeMonthFlag}" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(new { success = false, code = "C0004", err = e }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult SynRate()
        {
            string EmpId = Session["EmpId"].ToString();
            string EmpNo = Session["EmpNo"].ToString();
            if (string.IsNullOrEmpty(EmpId) || string.IsNullOrEmpty(EmpNo))
            {
                return Json(new { success = false, code = "C0001" }, JsonRequestBehavior.AllowGet);
            }
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_SynExchange", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@EmpId", EmpId);
                        command.Parameters.AddWithValue("@EmpNo", EmpNo);

                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }
                return Json(new { success = true, code = "OK" });
            }
            catch (Exception e)
            {
                return Json(new { success = false, code = "C0004", err = e }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 工具列介面
        /// </summary>
        /// <returns></returns>
        public ActionResult _ToolBar2Partial()
        {
            return PartialView();
        }
    }
}