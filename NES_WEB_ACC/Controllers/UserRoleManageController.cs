using NES_WEB_ACC.Modules;
using NES_WEB_ACC.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NES_WEB_ACC.Controllers
{
    public class UserRoleManageController : Controller
    {
        public string connectionString = ConfigurationManager.ConnectionStrings["NES_WEB_ACCConnectionString"].ConnectionString;
        private NES_WEB_ACCEntities _dbContext = new NES_WEB_ACCEntities();

        /// <summary>
        /// UsersList權限列表-主畫面View
        /// </summary>
        /// <returns></returns>
        [CustomAuthorize(Roles = "Admin")]
        public ActionResult UsersList()
        {
            // 20240403停用，測試新版
            //if (!IsAction(new string[] { "Admin","User" }))
            //    return RedirectToAction("Permissions", "Error");

            List<UsersListViewModel> users = new List<UsersListViewModel>();
            string sqlQuery1 = @"	SELECT 
                                            SU.[EmpId],
                                            SU.[EmpNo],
                                            SU.[EmpNameC],
                                            SU.[DeptNo],
                                            SU.[DeptName],
                                            SU.Status,
                                            STUFF(
                                                (SELECT ',' + SR.RoleName
                                                 FROM  [NES_WEB_ACC].[dbo].LNK_UserRole L1
                                                 INNER JOIN  [NES_WEB_ACC].[dbo].SYS_Roles SR ON L1.RoleId = SR.RoleId
                                                 WHERE SU.EmpId = L1.EmpId and L1.Status = 1 and SR.Status=1
                                                 FOR XML PATH('')), 1, 1, '') AS RoleName
                                        FROM 
                                            [NES_WEB_ACC].[dbo].[SYS_Users] AS SU 
                                        order by SU.DeptNo,SU.EmpNo";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Execute the SQL query and retrieve data
                SqlCommand command = new SqlCommand(sqlQuery1, connection);
                SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                users = dataTable.AsEnumerable().Select(row => new UsersListViewModel
                {
                    EmpId = "id" + Convert.ToString(row["EmpId"]),
                    EmpNo = row.Field<string>("EmpNo"),
                    EmpNameC = row.Field<string>("EmpNameC"),
                    DeptNo = row.Field<string>("DeptNo"),
                    DeptName = row.Field<string>("DeptName"),
                    RoleName = row.Field<string>("RoleName"),
                    Status = row.Field<Boolean>("Status")
                }).ToList();
            }
            return View(users);
        }
        /// <summary>
        /// UsersList權限列表-Show[個人編輯]_已加入控制角色
        /// </summary>
        /// <param name="empId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetPersonalRolesList(string empId)
        {
            // 檢查使用者角色權限
            if (true)
            //if (IsAction("UserRoleManage", "UsersList"))
            {
                List<PersonalRolesListViewModel> users = new List<PersonalRolesListViewModel>();
                string sqlQuery1 = @"	 select SU.EmpNo,SU.EmpNameC,SR.RoleName,L1.Status as 'PersnalRoleStatus'
                                                ,L1.EmpId,L1.RoleId
                                          from [NES_WEB_ACC].[dbo].[SYS_Users] as SU
	                                        left join [NES_WEB_ACC].[dbo].LNK_UserRole as L1 on SU.[EmpId] = L1.[EmpId]
	                                        left join [NES_WEB_ACC].[dbo].SYS_Roles as SR on L1.RoleId = SR.RoleId
                                          where 1=1
	                                        and SU.Status = 1
	                                        and SR.Status = 1
	                                        and L1.EmpId = @empId
                                          ";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Execute the SQL query and retrieve data
                    SqlCommand command = new SqlCommand(sqlQuery1, connection);
                    command.Parameters.AddWithValue("@empId", empId);
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);

                    users = dataTable.AsEnumerable().Select(row => new PersonalRolesListViewModel
                    {
                        EmpId = row.Field<Int64>("EmpId"),
                        EmpNo = row.Field<string>("EmpNo"),
                        EmpNameC = row.Field<string>("EmpNameC"),
                        RoleId = row.Field<Guid>("RoleId").ToString(),
                        RoleName = row.Field<string>("RoleName"),
                        PersnalRoleStatus = row.Field<Boolean>("PersnalRoleStatus")
                    }).ToList();
                }
                return Json(users, JsonRequestBehavior.AllowGet);
            }
            else
            {
                // 使用者沒有足夠權限，可以導向一個錯誤頁面或執行其他適當的處理
                //return RedirectToAction("AccessDenied", "Error"); // 假設有一個 AccessDenied 的 Action 在 ErrorController 中
            }
        }
        /// <summary>
        /// UsersList權限列表-Show[個人編輯]_未已加入控制角色
        /// </summary>
        /// <param name="empId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetPersonalNotControllerRolesList(string empId)
        {
            // 檢查使用者角色權限
            if (true)
            //if (IsAction("UserRoleManage", "UsersList"))
            {
                List<SYS_Roles> users = new List<SYS_Roles>();
                string sqlQuery1 = @"--使用者(未停用)尚未加入控制角色且該角色未被停用
	                                    select RoleId,RoleName from SYS_Roles
	                                    where NOT EXISTS (
			                                        Select 1
			                                        from SYS_Users as SU
				                                        left join LNK_UserRole as L1 on Su.EmpId = L1.EmpId
				                                        left join SYS_Roles as SR on L1.RoleId = SR.RoleId
			                                        where 1=1
				                                        and SU.Status = 1
				                                        and SU.EmpId = @empId
				                                        AND SR.RoleName = SYS_Roles.RoleName 
			                                        )
			                                    and Status =1
                                          ";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Execute the SQL query and retrieve data
                    SqlCommand command = new SqlCommand(sqlQuery1, connection);
                    command.Parameters.AddWithValue("@empId", empId);
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);

                    users = dataTable.AsEnumerable().Select(row => new SYS_Roles
                    {
                        RoleId = row.Field<Guid>("RoleId"),
                        RoleName = row.Field<string>("RoleName")
                    }).ToList();
                }
                return Json(users, JsonRequestBehavior.AllowGet);
            }
            else
            {
                // 使用者沒有足夠權限，可以導向一個錯誤頁面或執行其他適當的處理
                //return RedirectToAction("AccessDenied", "Error"); // 假設有一個 AccessDenied 的 Action 在 ErrorController 中
            }
        }
        /// <summary>
        /// UsersList權限列表-Show[User匯入]_ERP在職員工
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetERPEmpList()
        {
            // SQL 查詢語句：ERP員工資料表(扣除系統已有員工)
            string sqlQuery1 = @"SELECT [Id] as EmpId
                                      ,[EmpNo]
                                      ,[EmpNameC]
                                      ,[DeptNo]
                                      ,[DeptName]
                                  FROM [ESTAERPV2].[dbo].[EmployeeInfo]
                                  where 1=1
	                                and CompId = '150615163202244'
	                                and JobType = '在職'
	                                and [Id]　not in　(select EmpId from [NES_WEB_ACC].[dbo].SYS_Users)";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Execute the SQL query and retrieve data
                SqlCommand command = new SqlCommand(sqlQuery1, connection);
                SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                // Convert DataTable to List of Dictionary
                var rows = new List<Dictionary<string, object>>();
                foreach (DataRow row in dataTable.Rows)
                {
                    var dict = new Dictionary<string, object>();
                    foreach (DataColumn col in dataTable.Columns)
                    {
                        dict[col.ColumnName] = row[col];
                    }
                    rows.Add(dict);
                }

                // Return JSON result
                return Json(new { rows = rows }, JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// UsersList權限列表-Action[主畫面]_更新使User狀態
        /// </summary>
        /// <param name="empNo"></param>
        /// <param name="isStatus"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UpdateUsersStatus(string empNo, bool isStatus)
        {
            //檢查權限
            //IsAction(); ---未完成
            var user = _dbContext.SYS_Users.FirstOrDefault(u => u.EmpNo == empNo);
            if (user != null)
            {
                // 設定[更新日期、更新人員]欄位資料
                DateTime updateDate = DateTime.Now;
                // string updateBy = Session["EmpId"]?.ToString();
                string updateBy = Session["EmpNo"]?.ToString();
                user.Status = isStatus;
                //user.UpdateEmpId = updaetEmpId;
                user.UpdateBy = updateBy;
                user.UpdateDate = updateDate;

                // 儲存變更到資料庫
                _dbContext.SaveChanges();

                //Log寫入---未完成

                // 返回JSON格式的成功訊息
                return Json(new { success = true, message = "Data updated successfully" });
            }
            return Json(new { success = true, message = "有問題" });
        }
        /// <summary>
        /// UsersList權限列表-Actio[個人編輯]_更新個人角色狀態
        /// </summary>
        /// <param name="empId"></param>
        /// <param name="roleId"></param>
        /// <param name="isStatus"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UpdateUserRoleStatus(string empId, string roleId, bool isStatus)
        {
            //檢查權限
            //IsAction(); ---未完成

            if (Guid.TryParse(roleId, out var roleIdGuid))
            {
                // 在這裡使用 roleIdGuid
                Int64 empIdInt64 = Convert.ToInt64(empId);
                var user = _dbContext.LNK_UserRole.FirstOrDefault(u => u.EmpId == empIdInt64 && u.RoleId == roleIdGuid);
                if (user != null)
                {
                    // 設定[更新日期、更新人員]欄位資料
                    DateTime updateDate = DateTime.Now;
                    string updateBy = Session["EmpNo"]?.ToString();
                    user.Status = isStatus;
                    user.UpdateBy = updateBy;
                    user.UpdateDate = updateDate;

                    // 儲存變更到資料庫
                    _dbContext.SaveChanges();

                    //Log寫入---未完成

                    // 返回JSON格式的成功訊息
                    return Json(new { success = true, message = "Data updated successfully" });
                }
                return Json(new { success = false, message = "寫入出現問題" });
            }
            else
            {
                // 處理轉換失敗的情況，例如拋出異常或進行其他處理
                return Json(new { success = false, message = "Guid轉換出現問題" });
            }
        }
        /// <summary>
        /// UsersList權限列表-Action[User匯入]_新增User資料列
        /// </summary>
        /// <param name="rows"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult InsertToSYS_Users(List<SYS_Users> rows)
        {
            //檢查權限
            //IsAction();
            try
            {
                if (rows != null && rows.Any())
                {
                    // 設定[]欄位資料
                    DateTime createDate = DateTime.Now;
                    string createdBy = Session["EmpNo"]?.ToString();

                    // 遍歷行，檢查 EmpNo 是否在資料庫中已存在
                    foreach (var row in rows)
                    {
                        if (_dbContext.SYS_Users.Any(u => u.EmpNo == row.EmpNo))
                        {
                            // 如果 EmpNo 已存在，可以採取適當的處理，例如返回失敗的 JSON 回應
                            return Json(new { success = false, message = $"EmpNo: {row.EmpNo} 已存在於資料庫中" });
                        }

                        // 如果 EmpNo 不存在，設置其他欄位值
                        row.CreateDate = createDate;
                        row.CreateBy = createdBy;
                        row.Status = false;
                    }

                    // 將接收到的資料新增到資料庫
                    _dbContext.SYS_Users.AddRange(rows);
                    _dbContext.SaveChanges();

                    return Json(new { success = true, message = "資料儲存成功" });
                }
                else
                {
                    return Json(new { success = false, message = "沒有要儲存的資料" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "資料儲存失敗：" + ex.Message });
            }
        }
        /// <summary>
        /// UsersList權限列表-Action[個人編輯]_使用者綁定角色
        /// </summary>
        /// <param name="empId"></param>
        /// <param name="rows"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult InsertToLNK_UserRole(string empId, List<LNK_UserRole> rows)
        {
            //檢查權限
            //IsAction();
            try
            {
                if (rows != null && rows.Any() && empId != "")
                {
                    // 設定[]欄位資料
                    DateTime createDate = DateTime.Now;
                    string createdBy = Session["EmpNo"]?.ToString();

                    // 遍歷行，檢查 EmpId、RoleId 是否在資料庫中已存在
                    foreach (var row in rows)
                    {
                        if (_dbContext.LNK_UserRole.Any(u => u.EmpId == row.EmpId && u.RoleId == row.RoleId))
                        {
                            // 如果 EmpNo 已存在，可以採取適當的處理，例如返回失敗的 JSON 回應
                            return Json(new { success = false, message = $"EmpId: {row.EmpId}、 RoleId: {row.RoleId}已存在於資料庫中" });
                        }
                        Int64 empIdInt64 = Convert.ToInt64(empId);
                        // 如果 EmpNo 不存在，設置其他欄位值
                        row.Id = Guid.NewGuid();
                        row.EmpId = empIdInt64;
                        row.CreateDate = createDate;
                        row.CreateBy = createdBy;
                        row.Status = false;
                    }

                    // 將接收到的資料新增到資料庫
                    _dbContext.LNK_UserRole.AddRange(rows);
                    _dbContext.SaveChanges();

                    return Json(new { success = true, message = "資料儲存成功" });
                }
                else
                {
                    return Json(new { success = false, message = "沒有要儲存的資料" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "資料儲存失敗：" + ex.Message });
            }
        }

        /// <summary>
        /// RolesList-權限列表-主畫面View
        /// </summary>
        /// <returns></returns>
        [CustomAuthorize(Roles = "Admin")]
        public ActionResult RolesList()
        {
            // 20240403停用，測試新版
            //if (!IsAction(new string[] { "Admin" }))
            //    return RedirectToAction("Permissions", "Error");

            string sql = @"select RoleId, RoleName , Status as 'RoleStatus' from NES_WEB_ACC.dbo.SYS_Roles
                                                ";
            List<RolesListViewModel> rolesList = new List<RolesListViewModel>();

            // 建立資料庫連線
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();               
                SqlCommand command = new SqlCommand(sql, connection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    RolesListViewModel role = new RolesListViewModel
                    {
                        RoleId = (Guid)reader["RoleId"],
                        RoleName = reader["RoleName"].ToString(),
                        RoleStatus = (Boolean?)reader["RoleStatus"]
                    };
                    rolesList.Add(role);
                }
                return View(rolesList);
            }
        }
        /// <summary>
        /// RolesList權限列表-Action[主畫面]_更新Role狀態
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="isStatus"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UpdateRolesStatus(string roleId, bool isStatus)
        {
            //檢查權限
            //IsAction(); ---未完成
            Guid roleIdGuid;
            if (Guid.TryParse(roleId, out roleIdGuid))
            {
                var role = _dbContext.SYS_Roles.FirstOrDefault(u => u.RoleId == roleIdGuid);
                if (role != null)
                {
                    // 設定[更新日期、更新人員]欄位資料
                    DateTime updateDate = DateTime.Now;
                    string updateBy = Session["EmpNo"]?.ToString();
                    role.Status = isStatus;
                    role.UpdateBy = updateBy;
                    role.UpdateDate = updateDate;

                    // 儲存變更到資料庫
                    _dbContext.SaveChanges();

                    //Log寫入---未完成

                    // 返回JSON格式的成功訊息
                    return Json(new { success = true, message = "Data updated successfully" });
                }
                else
                {
                    return Json(new { success = true, message = "Guid 轉換失敗" });
                }
            }
            else
            {
                return Json(new { success = true, message = "Data updated fales" });
            }

        }
        /// <summary>
        /// RolesList權限列表-Action[新增角色]_新增Role資料列
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult InsertToSYS_Roles(string roleName)
        {
            //檢查權限
            //IsAction(); ---未完成
            try
            {
                // 檢查資料庫中是否已存在相同的 RoleName
                if (_dbContext.SYS_Roles.Any(r => r.RoleName == roleName))
                {
                    // 如果存在，返回失敗的 JSON 回應
                    return Json(new { success = false, message = "儲存失敗：相同的角色名稱已存在" });
                }
                // 建立新的 SYS_Roles 物件
                SYS_Roles newRole = new SYS_Roles
                {
                    RoleId = Guid.NewGuid(),
                    RoleName = roleName,
                    Status = false,
                    CreateDate = DateTime.Now,
                    CreateBy = Session["EmpNo"].ToString(),
                };

                // 將新角色加入資料庫
                _dbContext.SYS_Roles.Add(newRole);
                _dbContext.SaveChanges();

                // 返回成功的 JSON 回應
                return Json(new { success = true, message = "儲存成功" });
            }
            catch (Exception ex)
            {
                // 返回失敗的 JSON 回應
                return Json(new { success = false, message = $"儲存失敗：{ex.Message}" });
            }
        }
        /// <summary>
        /// 權限檢查_20240403停用，測試新版
        /// </summary>
        /// <param name="roles"></param>
        /// <returns>回傳bool</returns>
        //public bool IsAction(string[] roles)
        //{
        //    // 檢查傳入的值是否為空
        //    if (roles != null && roles.Length > 0)
        //    {
        //        // 檢查 Session["RoleList"] 是否存在並且不為空
        //        if (HttpContext.Session["RoleList"] is List<string> userRoles && userRoles.Count > 0)
        //        {
        //            // 檢查傳入的角色是否有任何一個存在於 Session["RoleList"] 中
        //            return roles.Any(role => userRoles.Contains(role));
        //        }
        //    }
        //    return false;
        //}
    }
}