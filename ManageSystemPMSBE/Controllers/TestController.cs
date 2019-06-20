using ManageSystemPMSBE.Models;
using System.Diagnostics;
using System.Web.Mvc;

namespace ManageSystemPMSBE.Controllers
{
    [DebuggerDisplay("id={id}, name={name}")]
    public class TestClass
    {
        public string id { get; set; }
        public string name { get; set; }
    }
    public class TestController : Controller
    {
        [Route("Test/{alias}.html")]
        [CustomerAuth(Roles = "TestMethod")]
        public ActionResult TestMethod(string alias)
        {
            Session["UserName"] = alias;
            ViewBag.test = alias;
            TestClass testClass = new TestClass()
            {
                id = "1",
                name = "124434567"
            };
            return View();
        }
        public ActionResult TestMethod2(string alias)
        {
            ViewBag.test = alias;
            return View();
        }
        public ActionResult TestMethod3()
        {
            return View();
        }
    }
}