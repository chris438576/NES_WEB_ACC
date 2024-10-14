using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NES_WEB_ACC.Controllers
{
    public class HistoryController : Controller
    {
        //------ 介面 ------//    
        public ActionResult VoucherInfoReview()
        {
            return View();
        }

        //------ 部分檢視 ------//
        /// <summary>
        /// 工具列介面
        /// </summary>
        /// <returns></returns>
        public ActionResult _ToolBarPartial()
        {
            return PartialView();
        }
        /// <summary>
        /// 查詢介面
        /// </summary>
        /// <returns></returns>
        public ActionResult _SearchPartial()
        {
            return PartialView();
        }
    }
}