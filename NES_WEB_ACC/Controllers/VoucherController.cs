using Dapper;
using NES_WEB_ACC.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

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
                    List<ACC_VoucherInfo> customerdata = conn.Query<ACC_VoucherInfo>(sql).ToList();
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
        public ActionResult GetVoucherItem1(string docid)
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
        /// 部分檢視_表頭欄位
        /// </summary>
        /// <returns></returns>
        public ActionResult _VoucherTableInfoPartial()
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