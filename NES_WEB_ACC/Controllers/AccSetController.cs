using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
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
        public ActionResult Rate()
        {
            return View();
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