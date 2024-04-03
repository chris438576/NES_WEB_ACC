using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NES_WEB_ACC.Modules
{
    public class CustomAuthorize : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                // 如果用戶未驗證，則將其重定向到登錄頁面
                filterContext.Result = new RedirectResult(string.Format("{0}?ReturnUrl={1}", "/Account/Login", filterContext.HttpContext.Request.Url));
            }
            else
            {
                // 如果用戶已驗證，但沒有所需角色，則返回未授權的結果
                filterContext.Result = new HttpUnauthorizedResult();
            }
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (!httpContext.User.Identity.IsAuthenticated)
            {
                return false;
            }

            // 檢查用戶是否擁有指定的任一角色
            foreach (string role in Roles.Split(','))
            {
                if (httpContext.User.IsInRole(role.Trim()))
                {
                    return true;
                }
            }

            return false;
        }
    }
}