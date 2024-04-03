using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NES_WEB_ACC.Controllers
{
    [Authorize]  
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            // 20240403停用，測試新版
            //string identityEmpNo = User.Identity.Name;    //正式
            ////string identityEmpNo = "NES1492";       //本機測試

            //RoleSetting(identityEmpNo);

            //// 檢查TempData是否包含訊息
            //if (TempData["Message"] != null)
            //{
            //    ViewBag.Message = TempData["Message"].ToString();
            //}

            return View();
        }
        /// <summary>
        /// 登入後只用者權限設定_20240403停用，測試新版
        /// </summary>
        /// <param name="identityEmpNo"></param>
        /// <returns></returns>
        //public ActionResult RoleSetting(string identityEmpNo)
        //{
        //    string connectionString = ConfigurationManager.ConnectionStrings["NES_WEB_ACCConnectionString"].ConnectionString;

        //    // SQL 查詢語句-1：系統中是否有該使用者
        //    string sqlQuery1 = @"SELECT 
        //                                SU.EmpId,
        //                                SU.EmpNo,
	       //                             SU.EmpNameC,
        //                                SU.Status
        //                            FROM [NES_WEB_ACC].[dbo].[SYS_Users] as SU 
	       //                               left join [ESTAERPV2].[dbo].EmployeeInfo as EI on SU.EmpId = EI.Id
        //                            where 1=1
	       //                             and SU.Status = 1	--User是否啟用
	       //                             and EI.JobType = '在職'  --ERP在職員工
        //                                and SU.EmpNo = @EmpNo";
        //    using (SqlConnection connection = new SqlConnection(connectionString))
        //    {
        //        using (SqlCommand command = new SqlCommand(sqlQuery1, connection))
        //        {
        //            command.Parameters.AddWithValue("@EmpNo", identityEmpNo);
        //            connection.Open();

        //            using (SqlDataReader reader = command.ExecuteReader())
        //            {
        //                if (reader.Read())
        //                {
        //                    if (reader.GetBoolean(reader.GetOrdinal("Status")))
        //                    {
        //                        // 將資料庫資料寫入Session：EmpId、EmpNo、EmpNameC
        //                        Session["EmpId"] = reader.GetInt64(reader.GetOrdinal("EmpId"));
        //                        Session["EmpNo"] = reader.GetString(reader.GetOrdinal("EmpNo"));
        //                        Session["EmpNameC"] = reader.GetString(reader.GetOrdinal("EmpNameC"));
        //                    }
        //                    else
        //                    {
        //                        TempData["Message"] = "您的帳號或已被停用。";
        //                        return RedirectToAction("Index");
        //                    }
        //                }
        //                else
        //                {
        //                    TempData["Message"] = "此系統無您的資料，請聯絡系統管理員。";
        //                    return RedirectToAction("Index");
        //                }
        //            }
        //        }
        //    }
        //    // SQL 查詢語句-2：該使用者有哪些角色
        //    string sqlQuery2 = @"SELECT SU.EmpNo,
	       //                             SU.EmpNameC,	
	       //                             SR.RoleName	
        //                            FROM [NES_WEB_ACC].[dbo].[SYS_Users] as SU 
	       //                               left join [ESTAERPV2].[dbo].EmployeeInfo as EI on SU.EmpId = EI.Id
	       //                               left join [NES_WEB_ACC].[dbo].LNK_UserRole as L1 on SU.[EmpId] = L1.[EmpId]
	       //                               left join [NES_WEB_ACC].[dbo].SYS_Roles as SR on L1.[RoleId] = SR.[RoleId]	                                     
        //                            where 1=1
	       //                             and SU.Status = 1	--User是否啟用
	       //                             and EI.JobType = '在職'
	       //                             and L1.Status = 1	--User對應Role是否啟用
	       //                             and SR.Status = 1	--Role是否啟用 
        //                                and SU.EmpNo = @EmpNo";
        //    using (SqlConnection connection = new SqlConnection(connectionString))
        //    {
        //        using (SqlCommand command = new SqlCommand(sqlQuery2, connection))
        //        {
        //            command.Parameters.AddWithValue("@EmpNo", identityEmpNo);
        //            connection.Open();

        //            using (SqlDataReader reader = command.ExecuteReader())
        //            {
        //                // 創建一個 List 來存放 RoleName
        //                List<string> roleList = new List<string>();
        //                // 將每一行的 RoleName 加入到 List 中
        //                while (reader.Read())
        //                {
        //                    string roleName = reader.GetString(reader.GetOrdinal("RoleName"));
        //                    roleList.Add(roleName);
        //                }

        //                if (roleList.Count > 0)
        //                {
        //                    ViewBag.RoleList = roleList;
        //                    // 將資料庫資料寫入Session：Role
        //                    Session["RoleList"] = roleList;
        //                }
        //                else
        //                {
        //                    // 沒有找到符合條件的資料，可以進行相應的處理
        //                    TempData["Message"] = "目前無角色，請聯絡系統管理員。";
        //                    return RedirectToAction("Index");
        //                }
        //            }
        //        }
        //    }
        //    TempData["Message"] = "角色Session設定完成。";
        //    return RedirectToAction("Index");
        //}

        /// <summary>
		/// 導覽圖介面
		/// </summary>
		/// <returns></returns>
		public ActionResult _AccStatePartial()
        {
            return PartialView();
        }
    }
}