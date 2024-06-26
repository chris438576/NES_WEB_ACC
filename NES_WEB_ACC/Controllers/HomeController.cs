using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace NES_WEB_ACC.Controllers
{
    [Authorize]  
    public class HomeController : Controller
    {
        public ActionResult Index()
        {           
            string identityEmpNo = ControllerContext.HttpContext.User.Identity.Name;
            UserSessionSetting(identityEmpNo);

            // 檢查TempData是否包含訊息
            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"].ToString();
            }


            return View();
        }      

        public ActionResult UserSessionSetting(string identityEmpNo)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["NES_WEB_ACCConnectionString"].ConnectionString;

            // SQL 查詢語句-1：系統中是否有該使用者
            string sqlQuery1 = @"SELECT 
                                            SU.EmpId,
                                            SU.EmpNo,
                                            SU.EmpNameC,
                                            SU.Status,
                                            EI.DeptNo,
                                            EI.DeptName
                                        FROM [NES_WEB_ACC].[dbo].[SYS_Users] as SU 
                                          left join [NES_WEB].[dbo].NES_EmployeeInfo as EI on SU.EmpId = EI.Id
                                        where 1=1
                                         and SU.Status = 1	--User是否啟用
                                         and EI.IsStatus = 0  --ERP在職員工
                                            and SU.EmpNo = @EmpNo";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(sqlQuery1, connection))
                {
                    command.Parameters.AddWithValue("@EmpNo", identityEmpNo);
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            if (reader.GetBoolean(reader.GetOrdinal("Status")))
                            {
                                // 將資料庫資料寫入Session：EmpId、EmpNo、EmpNameC
                                Session["EmpId"] = reader.GetInt64(reader.GetOrdinal("EmpId"));
                                Session["EmpNo"] = reader.GetString(reader.GetOrdinal("EmpNo"));
                                Session["EmpNameC"] = reader.GetString(reader.GetOrdinal("EmpNameC"));
                                Session["DeptNo"] = reader.GetString(reader.GetOrdinal("DeptNo"));
                                Session["DeptName"] = reader.GetString(reader.GetOrdinal("DeptName"));
                                TempData["Message"] = "角色Session設定完成。";
                                return RedirectToAction("Index");
                            }
                            else
                            {
                                TempData["Message"] = "您的帳號或已被停用。";
                                return RedirectToAction("Index");
                            }
                        }
                        else
                        {
                            TempData["Message"] = "此系統無您的資料，請聯絡系統管理員。";
                            return RedirectToAction("Index");
                        }
                    }
                }
            }
        }

        public ActionResult TimeOut()
        {
            //清除登入資訊
            FormsAuthentication.SignOut();
            Session.Abandon();
            string url = @"http://" + Request.Url.Authority + @"/NES_Login.aspx";
            //Response.Redirect(url);
            return RedirectPermanent(url);
        }

        /// <summary>
		/// 導覽圖介面
		/// </summary>
		/// <returns></returns>
		public ActionResult _AccStatePartial()
        {
            return PartialView();
        }

        /// <summary>
        /// 多語系，並建立Cookie紀錄
        /// </summary>
        /// <param name="lang"></param>
        /// <returns></returns>
        public ActionResult ChangeLanguage(string lang)
        {               
            // Set cookie
            HttpCookie langCookie = new HttpCookie("lang", lang);
            langCookie.Expires = DateTime.Now.AddYears(1); // Cookie 有效期設置為一年
            Response.Cookies.Add(langCookie);
            Session["lang"] = lang;
            return RedirectToAction("Index", "Home", new { language = lang });
        }
    }
}