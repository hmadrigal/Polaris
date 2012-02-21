using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Polaris.Cms.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        #region Fields
        #endregion

        #region Actions

        public ActionResult Index()
        {
            ViewData["Message"] = "Welcome to Polaris Content Management System site!";

            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        #endregion

        #region Methods
        #endregion
    }
}
