using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NES_WEB_ACC.Report
{
    public partial class ReportViewer_V1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["ReportPath"] != null && Session["ReportDataSource"] != null && Session["ReportParameters"] != null)
                {
                    //V1.0版本_只針對單一資料表
                    string reportPath = Session["ReportPath"] as string;
                    string reportDocName = (string)Session["ReportDocName"];
                    ReportDataSource reportDataSource = Session["ReportDataSource"] as ReportDataSource;
                    List<ReportParameter> reportParameters = Session["ReportParameters"] as List<ReportParameter>;

                    RptViewer.ProcessingMode = ProcessingMode.Local;
                    RptViewer.LocalReport.ReportPath = reportPath;
                    RptViewer.LocalReport.DataSources.Clear();
                    RptViewer.LocalReport.DataSources.Add(reportDataSource);
                    RptViewer.LocalReport.SetParameters(reportParameters);
                    RptViewer.LocalReport.Refresh();
                    RptViewer.LocalReport.DisplayName = reportDocName;

                    Session["ReportPath"] = null;
                    Session["ReportDataSource"] = null;
                    Session["ReportParameters"] = null;
                    Session["ReportDocName"] = null;
                }                
            }
        }      
    }
}