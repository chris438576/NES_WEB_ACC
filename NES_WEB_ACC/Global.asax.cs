using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace NES_WEB_ACC
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            
        }
        protected void Application_BeginRequest()
        {
            ////本機測試
            //Application["GlobalUrl"] = "";
            //正式使用
            Application["GlobalUrl"] = @"http://" + HttpContext.Current.Request.Url.Authority + @"/WEB_ACC";
        }
        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            //// 測試
            //string username = "NES1492"; // 假設您要模擬的用戶名稱
            //string[] roles = { "User" }; // 假設您要模擬的用戶角色
            ////正式
            string username = User.Identity.Name;            
            string[] roles = RoleSetting(username);

            // 創建一個GenericIdentity對象，表示已驗證的用戶
            GenericIdentity id = new GenericIdentity(username);

            // 創建一個GenericPrincipal對象，將用戶名稱和角色分配給該用戶
            GenericPrincipal principal = new GenericPrincipal(id, roles);

            // 設置HttpContext的User屬性為模擬的用戶
            HttpContext.Current.User = principal;
        }

        /// <summary>
        /// 登入後使用者權限設定
        /// </summary>
        /// <param name="identityEmpNo"></param>
        /// <returns></returns>
        public string[] RoleSetting(string identityEmpNo)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["NES_WEB_ACCConnectionString"].ConnectionString;
                       
            string sql = @"SELECT SU.EmpNo,
                                SU.EmpNameC,    
                                SR.RoleName    
                            FROM [NES_WEB_ACC].[dbo].[SYS_Users] as SU 
                                left join [ESTAERPV2].[dbo].EmployeeInfo as EI on SU.EmpId = EI.Id
                                left join [NES_WEB_ACC].[dbo].LNK_UserRole as L1 on SU.[EmpId] = L1.[EmpId]
                                left join [NES_WEB_ACC].[dbo].SYS_Roles as SR on L1.[RoleId] = SR.[RoleId]                                     
                            where 1=1
                                and SU.Status = 1    --User是否啟用
                                and EI.JobType = '在職'
                                and L1.Status = 1    --User對應Role是否啟用
                                and SR.Status = 1    --Role是否啟用 
                                and SU.EmpNo = @EmpNo";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@EmpNo", identityEmpNo);
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        // 創建一個 List 來存放 RoleName
                        List<string> roleList = new List<string>();
                        // 將每一行的 RoleName 加入到 List 中
                        while (reader.Read())
                        {
                            string roleName = reader.GetString(reader.GetOrdinal("RoleName"));
                            roleList.Add(roleName);
                        }

                        if (roleList.Count > 0)
                        {             
                            return roleList.ToArray(); // 返回角色陣列
                        }
                        else
                        {                            
                            return new string[0]; // 返回空陣列
                        }
                    }
                }
            }
        }
    }
}
