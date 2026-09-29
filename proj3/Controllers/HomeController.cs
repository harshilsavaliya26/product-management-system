using AdminLTE.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using poj3.Models;
using System.Data;
using System.Diagnostics;


namespace poj3.Controllers
{
    public class HomeController : Controller
    {

        // Login Page
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        // Login Check
        [HttpPost]
        public IActionResult Index(RegisterModel rm)
        {
            if (string.IsNullOrWhiteSpace(rm.email))
            {
                ViewBag.msg = "Please enter email.";
                return View(rm);
            }

            if (string.IsNullOrWhiteSpace(rm.password))
            {
                ViewBag.msg = "Please enter password.";
                return View(rm);
            }

            DataSet ds = rm.Login(rm.email, rm.password);

            if (ds.Tables.Count > 0 &&
                ds.Tables[0].Rows.Count > 0)
            {
                int userId = Convert.ToInt32(ds.Tables[0].Rows[0]["id"]);
                HttpContext.Session.SetInt32("UserId", userId);
                HttpContext.Session.SetString("UserName", Convert.ToString(ds.Tables[0].Rows[0]["name"]) ?? "");
                HttpContext.Session.SetString("UserEmail", Convert.ToString(ds.Tables[0].Rows[0]["email"]) ?? "");

                return RedirectToAction("home", "User");
            }

            ViewBag.msg = "Invalid Email or Password";

            return View(rm);
        }
        // Register Page
        [HttpGet]
        public IActionResult register()
        {
            return View();
        }


        // Register Save
        [HttpPost]
        public IActionResult register(RegisterModel rm)
        {
            if (string.IsNullOrWhiteSpace(rm.name))
            {
                ViewBag.msg = "Please enter name.";
                return View(rm);
            }

            if (string.IsNullOrWhiteSpace(rm.email))
            {
                ViewBag.msg = "Please enter email.";
                return View(rm);
            }

            if (string.IsNullOrWhiteSpace(rm.password))
            {
                ViewBag.msg = "Please enter password.";
                return View(rm);
            }

            if (rm.EmailExists(rm.email))
            {
                ViewBag.msg = "This email is already registered.";
                return View(rm);
            }

            rm.Insert();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult password()
        {
            return View();
        }


        [HttpPost]
        public IActionResult password(RegisterModel model)
        {
            if (string.IsNullOrWhiteSpace(model.email))
            {
                ViewBag.msg = "Please enter email.";
                return View(model);
            }

            DataSet ds = model.CheckEmail(model.email);

            if (ds.Tables.Count > 0 &&
                ds.Tables[0].Rows.Count > 0)
            {
                TempData["email"] = model.email;

                return RedirectToAction("recover");
            }

            ViewBag.msg = "Email Not Found";

            return View(model);
        }
        // ================= RECOVER GET =================

        [HttpGet]
        public IActionResult recover()
        {
            RegisterModel model = new RegisterModel();

            if (TempData["email"] != null)
            {
                model.email = TempData["email"].ToString()!;
            }

            return View(model);
        }


        // ================= RECOVER POST =================

        [HttpPost]
        public IActionResult recover(RegisterModel model)
        {
            if (string.IsNullOrWhiteSpace(model.email))
            {
                ViewBag.msg = "Email is required.";
                return View(model);
            }

            if (string.IsNullOrWhiteSpace(model.password))
            {
                ViewBag.msg = "Password is required.";
                return View(model);
            }

            model.UpdatePassword(model.email, model.password);

            return RedirectToAction("Index");
        }
        // Dashboard
        public IActionResult Dashboard()
        {
            return View();
        }

        public IActionResult Dashboard2()
        {
            return View();
        }

        public IActionResult Dashboard3()
        {
            return View();
        }

        public IActionResult Widgets()
        {
            return View();
        }
        public IActionResult topnavigation()
        {
            return View();
        }
        public IActionResult topnavigationsidebar()
        {
            return View();
        }
        public IActionResult boxed()
        {
            return View();
        }
        public IActionResult fixsidebar ()
        {
            return View();
        }
        public IActionResult fixsidebarl()
        {
            return View();
        }
        public IActionResult fixednavbar()
        {
            return View();

        }
        public IActionResult fixedfooter()
        {
            return View();

        }
        public IActionResult collapsedsidebar()
        {
            return View();
        }
        public IActionResult chartjs()
        {
            return View();
        }
        public IActionResult flot()
        {
            return View();
        }
        public IActionResult inline()
        {
            return View();
        }
        public IActionResult uplot()
        {
            return View();
        }
        public IActionResult general()
        {

            return View();
        }
        public IActionResult icons()
        {
            return View();
        }
        public IActionResult buttons()
        {
            return View();
        }
        public IActionResult sliders()
        {
            return View();
        }
        public IActionResult modals()
        {
            return View();
        }
        public IActionResult tabs()
        {
            return View();
        }
        public IActionResult time()
        {
            return View();
        }
        public IActionResult ribbons()
        {
            return View();
        }   
        public IActionResult element()
        {
            return View();
        }
        public IActionResult advanced()
        {
            return View();
        }
        public IActionResult editors()
        {
            return View();
        }
        public IActionResult validation()
        {
            return View();
        }
        public IActionResult tables()
        {
            return View();
        }
        public IActionResult datatable()
        {
            return View();
        }
        public IActionResult jsgrid()
        {
            return View();
        }
        public IActionResult calendar()
        {
            return View();
        }

        public IActionResult gallery()
        {
            return View();
        }   
        public IActionResult kanbanboard()
        {
            return View();
        }
        public IActionResult inbox()
        {
            return View();
        }
        public IActionResult compose()
        {
            return View();
        }
        public IActionResult read()
        {
            return View();
        }
        public IActionResult invoice()
        {
            return View();
        }
        public IActionResult print()
        {
            return View();
        }
        public IActionResult profile()
        {
            return View();
        }
        public IActionResult ecommerce()
        {
            return View();
        }
        public IActionResult projects()
        {
            return View();
        }
        public IActionResult projectadd()
        {
            return View();
        }
        public IActionResult projectedit()
        {
            return View();
        }
        public IActionResult projectdetail()
        {
            return View();
        }
        public IActionResult contacts()
        {
            return View();
        }
        public IActionResult faq()
        {
            return View();
        }
        public IActionResult contactus()
        {
            return View();
        }
        public IActionResult Index2()
        {
            return View();
        }
        public IActionResult register2()
        {
            return View();
        }
        public IActionResult password2()
        {
            return View();
        }
        public IActionResult recover2()
        {
            return View();
        }
        public IActionResult locks()
        {
            return View();        
        }
        public IActionResult menu()
        {
            return View();
        }
        public IActionResult menul()
        {
            return View();
        }
        public IActionResult error404()
        {
            return View();
        }
        public IActionResult error500()
        {
            return View();
        }
        public IActionResult pace()
        {
            return View();
        }
        public IActionResult blank()
        {
            return View();
        }
        public IActionResult starter()
        {
            return View();
        }
        public IActionResult search()
        {
            return View();
        }
        public IActionResult searchr()
        {
            return View();
        }
        public IActionResult searche()
        {
            return View();
        }
        public IActionResult searcher()
        {
            return View();
        }
        public IActionResult tabbed()
        {
            return View();
        }
        public IActionResult documentation()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
