﻿using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NES_WEB_ACC.Report
{
    public partial class ReportViewer_V2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["ReportPath"] != null && Session["ReportDataSources"] != null && Session["ReportParameters"] != null)
                {      
                    //V2.0版本_可傳入多張資料表
                    string reportPath = (string)Session["ReportPath"];
                    string reportDocName = (string)Session["ReportDocName"];
                    var reportDataSources = (Dictionary<string, ReportDataSource>)Session["ReportDataSources"];
                    List<ReportParameter> reportParameters = Session["ReportParameters"] as List<ReportParameter>;

                    RptViewer.ProcessingMode = ProcessingMode.Local;
                    RptViewer.LocalReport.ReportPath = reportPath;
                    RptViewer.LocalReport.DataSources.Clear();                    
                    foreach (var dataSource in reportDataSources)
                    {
                        RptViewer.LocalReport.DataSources.Add(dataSource.Value);
                    }
                    RptViewer.LocalReport.SetParameters(reportParameters);
                    RptViewer.LocalReport.Refresh();
                    RptViewer.LocalReport.DisplayName = reportDocName;

                    Session["ReportPath"] = null;
                    Session["ReportDataSources"] = null;
                    Session["ReportParameters"] = null;
                    Session["ReportDocName"] = null;
                }                
            }
        }      
    }
}