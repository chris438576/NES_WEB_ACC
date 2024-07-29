using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NES_WEB_ACC.Controllers
{
    public class AccCodeMXController : Controller
    {
        // GET: AccCodeMX
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 工具列介面
        /// </summary>
        /// <returns></returns>
        public ActionResult _ToolBarPartial()
        {
            return PartialView();
        }
    }
}