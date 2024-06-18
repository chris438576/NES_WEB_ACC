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

        public ActionResult AccReport()
        {
            AccReportASPX();
            return View();
        }
        public ActionResult AccReportASPX()
        {
            // 報表資料設定_SQL查詢
            string sql = @"select CompNo,CompAbbr,AccGroupNo,AccGroupNameC 
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
    }
}