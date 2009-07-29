using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Polaris.Pal.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        #region Fields
        #endregion

        #region Actions

        // GET: /
        public ActionResult Index()
        {
            ViewData["Message"] = "Welcome to Polaris Gamming Experiment!";

            return View();
        }

        // GET: /About
        public ActionResult About()
        {
            return View();
        }

        #endregion

        #region Methods
        #endregion
    }
}
