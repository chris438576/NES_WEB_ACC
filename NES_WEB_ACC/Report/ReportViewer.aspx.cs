using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NES_WEB_ACC.Report
{
    public partial class ReportViewer : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["ReportPath"] != null && Session["ReportDataSource"] != null && Session["ReportParameters"] != null)
                {
                    string reportPath = Session["ReportPath"] as string;
                    ReportDataSource reportDataSource = Session["ReportDataSource"] as ReportDataSource;
                    List<ReportParameter> reportParameters = Session["ReportParameters"] as List<ReportParameter>;

                    RptViewer.ProcessingMode = ProcessingMode.Local;
                    RptViewer.LocalReport.ReportPath = reportPath;
                    RptViewer.LocalReport.DataSources.Clear();
                    RptViewer.LocalReport.DataSources.Add(reportDataSource);
                    RptViewer.LocalReport.SetParameters(reportParameters);
                    RptViewer.LocalReport.Refresh();

                    // 数据使用完后清空会话变量
                    Session["ReportPath"] = null;
                    Session["ReportDataSource"] = null;
                    Session["ReportParameters"] = null;
                }
            }
        }

    }
}