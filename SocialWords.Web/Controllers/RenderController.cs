using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SocialWords.Web.Controllers
{
    public class RenderController : Controller
    {
        //
        // GET: /Render/
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Q(String id)
        {
            ViewBag.Message = id;
            return View("Q");
        }
        public ActionResult D(String id)
        {
            ViewBag.Message = id;
            return View("D");
        }
        public ActionResult T(String id)
        {
            ViewBag.Message = id;
            return View("T");
        }
    }
}