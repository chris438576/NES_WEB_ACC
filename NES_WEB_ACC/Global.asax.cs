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
            ////��������
            //Application["GlobalUrl"] = "";
            //�����ϥ�
            Application["GlobalUrl"] = @"http://" + HttpContext.Current.Request.Url.Authority + @"/WEB_ACC";
        }
        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            //// ����
            //string username = "NES1492"; // ���]�z�n�������Τ�W��
            //string[] roles = { "User" }; // ���]�z�n�������Τᨤ��
            ////����
            string username = User.Identity.Name;            
            string[] roles = RoleSetting(username);

            // �Ыؤ@��GenericIdentity��H�A��ܤw���Ҫ��Τ�
            GenericIdentity id = new GenericIdentity(username);

            // �Ыؤ@��GenericPrincipal��H�A�N�Τ�W�٩M������t���ӥΤ�
            GenericPrincipal principal = new GenericPrincipal(id, roles);

            // �]�mHttpContext��User�ݩʬ��������Τ�
            HttpContext.Current.User = principal;
        }

        /// <summary>
        /// �n�J��ϥΪ��v���]�w
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
                                and SU.Status = 1    --User�O�_�ҥ�
                                and EI.JobType = '�b¾'
                                and L1.Status = 1    --User����Role�O�_�ҥ�
                                and SR.Status = 1    --Role�O�_�ҥ� 
                                and SU.EmpNo = @EmpNo";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@EmpNo", identityEmpNo);
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        // �Ыؤ@�� List �Ӧs�� RoleName
                        List<string> roleList = new List<string>();
                        // �N�C�@�檺 RoleName �[�J�� List ��
                        while (reader.Read())
                        {
                            string roleName = reader.GetString(reader.GetOrdinal("RoleName"));
                            roleList.Add(roleName);
                        }

                        if (roleList.Count > 0)
                        {             
                            return roleList.ToArray(); // ��^����}�C
                        }
                        else
                        {                            
                            return new string[0]; // ��^�Ű}�C
                        }
                    }
                }
            }
        }
    }
}
