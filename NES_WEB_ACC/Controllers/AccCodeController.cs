using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Dapper;
using NES_WEB_ACC.Modules;

namespace NES_WEB_ACC.Controllers
{
    [Authorize]
    [CustomAuthorize(Roles = "Admin")]
    public class AccCodeController : Controller
    {
        public string connectionString = ConfigurationManager.ConnectionStrings["NES_WEB_ACCConnectionString"].ConnectionString;
        private NES_WEB_ACCEntities _dbContext = new NES_WEB_ACCEntities();

        /// <summary>
        /// View_會計科目設定
        /// </summary>
        /// <returns></returns>
        public ActionResult AccCodeSet()
        {            
            return View();
        }
        /// <summary>
        /// Get_會計大類
        /// </summary>
        /// <returns></returns>
        public ActionResult GetAccCode1()
        {
            var lang = Session["lang"] as string;
            string sql;
            switch (lang)
            {
                case "en":
                    sql = @"select Id,AccGroupNo,AccGroupNameE as 'AccGroupNameC' from ACC_AccCode1 where ([CompId] = '150615163202244')";
                    break;
                case "zh-TW":
                    sql = @"select Id,AccGroupNo,AccGroupNameC as 'AccGroupNameC' from ACC_AccCode1 where ([CompId] = '150615163202244')";
                    break;
                case "es-MX":
                    sql = @"select Id,AccGroupNo,AccGroupNameMX as 'AccGroupNameC' from ACC_AccCode1 where ([CompId] = '150615163202244')";
                    break;
                default:
                    sql = @"select Id,AccGroupNo,AccGroupNameE as 'AccGroupNameC' from ACC_AccCode1 where ([CompId] = '150615163202244')";
                    break;
            }            
            
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    List<ACC_AccCode1> customerdata = conn.Query<ACC_AccCode1>(sql).ToList();
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
        /// Get_會計分類
        /// </summary>
        /// <param name="docid"></param>
        /// <returns></returns>
        public ActionResult GetAccCode2(string docid)
        {
            if (string.IsNullOrEmpty(docid))
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("C0001", JsonRequestBehavior.AllowGet);
            }
            var lang = Session["lang"] as string;
            string sql;
            switch (lang)
            {
                case "en":
                    sql = @"SELECT Id,AccNoBy,AccNoByNameE as 'AccNoByNameC',DCTypeNo,DCTypeNameC,AccNoType FROM ACC_AccCode2 where ([CompId] = '150615163202244')　and DocId = @docid";
                    break;
                case "zh-TW":
                    sql = @"SELECT Id,AccNoBy,AccNoByNameC as 'AccNoByNameC',DCTypeNo,DCTypeNameC,AccNoType FROM ACC_AccCode2 where ([CompId] = '150615163202244')　and DocId = @docid";
                    break;
                case "es-MX":
                    sql = @"SELECT Id,AccNoBy,AccNoByNameMX as 'AccNoByNameC',DCTypeNo,DCTypeNameC,AccNoType FROM ACC_AccCode2 where ([CompId] = '150615163202244')　and DocId = @docid";
                    break;
                default:
                    sql = @"SELECT Id,AccNoBy,AccNoByNameE as 'AccNoByNameC',DCTypeNo,DCTypeNameC,AccNoType FROM ACC_AccCode2 where ([CompId] = '150615163202244')　and DocId = @docid";
                    break;
            }
           
            var param = new { docid };
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    List<ACC_AccCode2> customerdata = conn.Query<ACC_AccCode2>(sql, param).ToList();
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
        /// Get_會計科目
        /// </summary>
        /// <param name="accnobyid"></param>
        /// <returns></returns>
        public ActionResult GetAccTitleNo(string accnobyid)
        {
            if (string.IsNullOrEmpty(accnobyid))
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("C0001", JsonRequestBehavior.AllowGet);
            }
            string sql = @"  select * from ACC_AccTitleNo where ([CompId] = '150615163202244') and AccNoById= @accnobyid order by AccNo";
            var param = new { accnobyid };
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    List<ACC_AccTitleNo> customerdata = conn.Query<ACC_AccTitleNo>(sql, param).ToList();
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
        /// View_科目類別設定
        /// </summary>
        /// <returns></returns>
        public ActionResult AccGroupNoSet()
        {
            return View();
        }

        /// <summary>
        /// View_權益科目設定
        /// </summary>
        /// <returns></returns>
        public ActionResult EquityCodeSet()
        {
            return View();
        }
        public ActionResult GetEquityInfo(string type)
        {
            if (string.IsNullOrEmpty(type))
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("C0001", JsonRequestBehavior.AllowGet);
            }
            string sql = @" select * from ACC_EquityInfo where ([CompId] = '150615163202244') AND ([Type] = @type)";
            var param = new { type };
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    List<ACC_EquityInfo> customerdata = conn.Query<ACC_EquityInfo>(sql, param).ToList();
                    
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
        public ActionResult GetEquityItem(string docid)
        {
            if (string.IsNullOrEmpty(docid))
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("C0001", JsonRequestBehavior.AllowGet);
            }
            string sql = @" SELECT * FROM ACC_EquityItem where  ([DocId] = @docid)";
            var param = new { docid };
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    List<ACC_EquityItem> customerdata = conn.Query<ACC_EquityItem>(sql, param).ToList();
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
        /// View_傳票類別設定
        /// </summary>
        /// <returns></returns>
        public ActionResult VoucherCodeSet()
        {
            return View();
        }
        public ActionResult GetSysDocSubType()
        {
            //if (string.IsNullOrEmpty(type))
            //{
            //    Response.StatusCode = (int)HttpStatusCode.BadRequest;
            //    return Json("C0001", JsonRequestBehavior.AllowGet);
            //}
            string sql = @" select * from ACC_SysDocSubType　where  ([DocId] = '1019')";
            //var param = new { type };
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    //List<EquityInfo> customerdata = conn.Query<EquityInfo>(sql, param).ToList();
                    List<ACC_SysDocSubType> customerdata = conn.Query<ACC_SysDocSubType>(sql).ToList();
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
        public ActionResult GetVoucherKind(string compid)
        {
            if (string.IsNullOrEmpty(compid))
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("C0001", JsonRequestBehavior.AllowGet);
            }
            string sql = @" select * from ACC_VoucherKind　where  ([CompId] = @compid)　";
            var param = new { compid };
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    List<ACC_VoucherKind> customerdata = conn.Query<ACC_VoucherKind>(sql, param).ToList();                    
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
        /// 工具列介面
        /// </summary>
        /// <returns></returns>
        public ActionResult _ToolBarPartial()
        {
            return PartialView();
        }                
    }
}