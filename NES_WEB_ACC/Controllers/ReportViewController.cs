using Dapper;
using Microsoft.Reporting.WebForms;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;

namespace NES_WEB_ACC.Controllers
{
    public class ReportViewController : Controller
    {
        public string connectionString = ConfigurationManager.ConnectionStrings["NES_WEB_ACCConnectionString"].ConnectionString;
        private NES_WEB_ACCEntities _dbContext = new NES_WEB_ACCEntities();
        
        /// <summary>
        /// 測試View
        /// </summary>
        /// <returns></returns>
        public ActionResult AccReport()
        {
            AccReportASPX();
            return View();
        }
        /// <summary>
        /// 對應aspx的V1.0版，只傳單一資料表
        /// </summary>
        /// <param name="reportId"></param>
        /// <returns></returns>
        public ActionResult AccReportASPX()
        {
            // 報表資料設定_SQL查詢
            string sql = @"
                select CompNo,CompAbbr,AccGroupNo,AccGroupNameC 
                from NES_WEB_ACC.dbo.ACC_AccCode1 where CompId = '150615163202244'
            ";

            List<ACC_AccCode1> infodata = new List<ACC_AccCode1>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                infodata = conn.Query<ACC_AccCode1>(sql).ToList();
            }

            // 報表參數設定
            var reportParameters = new List<ReportParameter>
            {
                new ReportParameter("ReportMaker", "Chris")
            };

            // Session設定，給ReportViewer使用
            Session["ReportPath"] = Server.MapPath("~/Report/RDLC/AccCodeReport.rdlc");
            Session["ReportDataSource"] = new ReportDataSource("AccCodeRdlc", infodata);
            Session["ReportParameters"] = reportParameters;

            // 導向ReportViewer.aspx
            return Redirect("~/Report/ReportViewer.aspx");
        }
        /// <summary>
        /// 對應aspx的V2.0版，可傳多張資料表
        /// </summary>
        /// <param name="reportId"></param>
        /// <returns></returns>
        public ActionResult VoucherBillReport(string reportId)
        {
            string EmpNo = (string)Session["EmpNo"];            
            string sqlInfo = @"
                select BillNo,CONVERT(varchar(100), BillDate, 111) as BillDate,EmpNo,EmpNameC, StateBy, StateDate, ClosedBy, ClosedDate,Money21,Money22 from NES_WEB_ACC.dbo.ACC_VoucherInfo where WebId = @reportId
            ";
            string sqlDetail = @"
                select Linage, AccNo, AccNameC,DCTypeNo, CurrencyNo, Money, Rate1, CurrencySt, Money1  from NES_WEB_ACC.dbo.ACC_VoucherDetail where WebDocId = @reportId
            ";

            ACC_VoucherInfo voucherInfo;
            List<ACC_VoucherDetail> voucherDetail = new List<ACC_VoucherDetail>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                voucherInfo = conn.QueryFirstOrDefault<ACC_VoucherInfo>(sqlInfo, new { reportId });
                voucherDetail = conn.Query<ACC_VoucherDetail>(sqlDetail, new { reportId }).ToList();
            }                      

            // 報表參數設定
            var reportParameters = new List<ReportParameter>
            {
                new ReportParameter("ReportMaker", EmpNo),
                new ReportParameter("ReportId", reportId)
            };
            var reportDataSources = new Dictionary<string, ReportDataSource>
            {
                { "VoucherBillMain", new ReportDataSource("VoucherBillMain", new List<ACC_VoucherInfo> { voucherInfo }) },
                { "VoucherBillItem", new ReportDataSource("VoucherBillItem", voucherDetail) }
            };

            // Session設定，給ReportViewer使用
            Session["ReportPath"] = Server.MapPath("~/Report/RDLC/VoucherBillReport.rdlc");
            Session["ReportDataSources"] = reportDataSources;
            Session["ReportParameters"] = reportParameters;
            Session["ReportDocName"] = "VoucherBill_" + voucherInfo.BillNo;

            return Redirect("~/Report/ReportViewer.aspx");
        }
    }
}