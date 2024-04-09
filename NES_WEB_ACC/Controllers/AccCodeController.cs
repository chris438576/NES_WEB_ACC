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
            string sql = @"select * from NES_WEB_ACC.dbo.AccCode1 where ([CompId] = '150615163202244')";
            
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    List<AccCode1> customerdata = conn.Query<AccCode1>(sql).ToList();
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
            string sql = @"SELECT * FROM [NES_WEB_ACC].[dbo].[AccCode2] where ([CompId] = '150615163202244')　and DocId = @docid";
            var param = new { docid };
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    List<AccCode2> customerdata = conn.Query<AccCode2>(sql, param).ToList();
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
            string sql = @"  select * from [NES_WEB_ACC].[dbo].[AccTitleNo] where ([CompId] = '150615163202244') and AccNoById= @accnobyid order by AccNo";
            var param = new { accnobyid };
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    List<AccTitleNo> customerdata = conn.Query<AccTitleNo>(sql, param).ToList();
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
            string sql = @" select * from NES_WEB_ACC.dbo.[EquityInfo] where ([CompId] = '150615163202244') AND ([Type] = @type)";
            var param = new { type };
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    List<EquityInfo> customerdata = conn.Query<EquityInfo>(sql, param).ToList();
                    
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
            string sql = @" SELECT * FROM NES_WEB_ACC.dbo.[EquityItem] where  ([DocId] = @docid)";
            var param = new { docid };
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    List<EquityItem> customerdata = conn.Query<EquityItem>(sql, param).ToList();
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
            string sql = @" select * from NES_WEB_ACC.dbo.[SysDocSubType]　where  ([DocId] = '1019')";
            //var param = new { type };
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    //List<EquityInfo> customerdata = conn.Query<EquityInfo>(sql, param).ToList();
                    List<SysDocSubType> customerdata = conn.Query<SysDocSubType>(sql).ToList();
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
            string sql = @" select * from NES_WEB_ACC.dbo.[VoucherKind]　where  ([CompId] = @compid)　";
            var param = new { compid };
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    List<VoucherKind> customerdata = conn.Query<VoucherKind>(sql, param).ToList();                    
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

        /// <summary>
        /// 權限檢查_20240403停用，測試新版
        /// </summary>
        /// <param name="roles"></param>
        /// <returns></returns>
        //public bool IsAction(string[] roles)
        //{
        //    // 檢查傳入的值是否為空
        //    if (roles != null && roles.Length > 0)
        //    {
        //        // 檢查 Session["RoleList"] 是否存在並且不為空
        //        if (HttpContext.Session["RoleList"] is List<string> userRoles && userRoles.Count > 0)
        //        {
        //            // 檢查傳入的角色是否有任何一個存在於 Session["RoleList"] 中
        //            return roles.Any(role => userRoles.Contains(role));
        //        }
        //    }
        //    return false;
        //}
    }
}