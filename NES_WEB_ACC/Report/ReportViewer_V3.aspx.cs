using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NES_WEB_ACC.Report
{
    public partial class ReportViewer_V3 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["ReportPath"] != null && Session["ReportDataSourceMain"] != null && Session["ReportParameters"] != null)
                {
                    //V3.0版本_使用Master-Detail，子報表
                    string reportPath = (string)Session["ReportPath"];
                    string reportDocName = (string)Session["ReportDocName"];
                    var reportDataSourceMain = (ReportDataSource)Session["ReportDataSourceMain"];
                    var reportDataSourceItem = (ReportDataSource)Session["ReportDataSourceItem"];
                    List<ReportParameter> reportParameters = Session["ReportParameters"] as List<ReportParameter>;

                    RptViewer.ProcessingMode = ProcessingMode.Local;
                    RptViewer.LocalReport.ReportPath = reportPath;
                    RptViewer.LocalReport.DataSources.Clear();
                  
                    RptViewer.LocalReport.DataSources.Add(reportDataSourceMain);
                    RptViewer.LocalReport.DataSources.Add(reportDataSourceItem);

                    RptViewer.LocalReport.SetParameters(reportParameters);
                    RptViewer.LocalReport.Refresh();
                    RptViewer.LocalReport.DisplayName = reportDocName;
                    RptViewer.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(SetSubDataSource);

                    Session["ReportPath"] = null;
                    Session["ReportDataSources"] = null;
                    Session["ReportParameters"] = null;
                    Session["ReportDocName"] = null;
                }                
            }
        }

        public void SetSubDataSource(object sender, SubreportProcessingEventArgs e)
        {           
            if (Session["ReportDataSourceItem"] != null)
            {                
                var reportDataSourceItem = (ReportDataSource)Session["ReportDataSourceItem"];
              
                e.DataSources.Add(reportDataSourceItem);
            }
        }
    }
}