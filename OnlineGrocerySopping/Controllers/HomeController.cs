using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MySql.Data.MySqlClient;


namespace OnlineGrocerySopping.Controllers
{
    public class HomeController : Controller
    {
        string myConnectionString = "server=localhost;uid=root;" + "database=grocery_shopping";
        [HttpGet]
        public ActionResult Login()
        {
            ViewBag.Message = "Login.";
            return View();
        }

        [HttpPost]
        public ActionResult Login(Models.loginDetail obj)
        {
            string newProdID = "";
            try
            {
                using (MySql.Data.MySqlClient.MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection(myConnectionString))
                {
                    MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand("select Count(*) from consumers o where o.email=@userName and password=@password", conn);
                    cmd.Parameters.AddWithValue("@userName", obj.userName);
                    cmd.Parameters.AddWithValue("@password", obj.password);
                    try
                    {
                        conn.Open();
                        newProdID = Convert.ToString(cmd.ExecuteScalar());
                        if (Convert.ToInt32(newProdID) == 0)
                        {
                            return View("Login");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                newProdID = "";
            }
            Session["uid"] = obj.userName;
            Session["pwd"] = obj.password;
            return View("Index");
        }

        [HttpGet]
        public ActionResult UserRegistration()
        {
            ViewBag.Message = "User Registration.";
            return View();
        }

        [HttpPost]
        public ActionResult UserRegistration(Models.RegistrationDetails obj)
        {
            int count = 0;
            try
            {
                using (MySql.Data.MySqlClient.MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection(myConnectionString))
                {
                    MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand("insert into consumers(name,gender,email,mobile,password) values(@name,@gender,@email,@mobile,@password)", conn);
                    cmd.Parameters.AddWithValue("@name", obj.name);
                    cmd.Parameters.AddWithValue("@gender", obj.gender);
                    cmd.Parameters.AddWithValue("@email", obj.email);
                    cmd.Parameters.AddWithValue("@mobile", obj.mobile);
                    cmd.Parameters.AddWithValue("@password", obj.password);
                    try
                    {
                        conn.Open();
                        count = cmd.ExecuteNonQuery();
                        if (count == 0)
                        {
                            return View("UserRegistration");
                        }
                    }
                    catch (Exception ex)
                    {
                        return View("UserRegistration");
                    }
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                count = 0;
            }
            return View("Login");
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Order(string img, string Product, string ProductId, string Amt, string Status)
        {
           //ViewBag.Message = "Your application description page.";
           TempData["img"] = img;
           TempData["Product"] = Product;
           TempData["ProductId"] = ProductId;
           TempData["Amt"] = Amt;
           TempData["Status"] = Status;
           return View();
        }

        [HttpPost]
        public JsonResult Proceed(Models.PaymentDetail obj)
        {
            //int count = 0;
            string CustId = "";
            string OrdId = "";
            double TotAmt=(Convert.ToDouble(obj.Amount) * Convert.ToInt32(obj.Quantity));
            try
            {
                using (MySql.Data.MySqlClient.MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection(myConnectionString))
                {
                    MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand("select consumerid from consumers o where o.email=@userName and password=@password", conn);
                    cmd.Parameters.AddWithValue("@userName", Convert.ToString(Session["uid"]));
                    cmd.Parameters.AddWithValue("@password", Convert.ToString(Session["pwd"]));
                    try
                    {
                        conn.Open();
                        CustId = Convert.ToString(cmd.ExecuteScalar());
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    MySql.Data.MySqlClient.MySqlCommand cmd1 = new MySql.Data.MySqlClient.MySqlCommand("insert into orders(orderdate,consumerid,billamount,groceryname,billstatus) values(@orderdate,@consumerid,@billamount,@groceryname,@billstatus); SELECT LAST_INSERT_ID();", conn);
                    cmd1.Parameters.AddWithValue("@orderdate", (DateTime.Now).ToString("yyyy-MM-dd"));
                    cmd1.Parameters.AddWithValue("@consumerid", Convert.ToInt32(CustId));
                    cmd1.Parameters.AddWithValue("@billamount", Convert.ToDecimal(TotAmt));
                    cmd1.Parameters.AddWithValue("@groceryname", obj.Product);
                    cmd1.Parameters.AddWithValue("@billstatus", "NOT PAID");
                    try
                    {
                        OrdId = Convert.ToString(cmd1.ExecuteScalar());
                        if (OrdId == null)
                        {
                            return Json(0, JsonRequestBehavior.AllowGet); 
                        }
                    }
                    catch (Exception ex)
                    {
                        return Json(0, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
            Models.PaymentDetail obj1 = new Models.PaymentDetail();
            obj1.Amount = Convert.ToString(TotAmt);
            obj1.OrdId = Convert.ToString(OrdId);
            return Json(obj1, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ProceedPay(Models.PaymentDetail obj)
        {
            int count = 0;
            try
            {
                using (MySql.Data.MySqlClient.MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection(myConnectionString))
                {
                    MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand("Update orders set billstatus = 'PAID' Where ordernumber=@ordernumber", conn);
                    cmd.Parameters.AddWithValue("@ordernumber", obj.OrdId);
                    try
                    {
                        conn.Open();
                        count = cmd.ExecuteNonQuery();
                        if (count <= 0)
                        {
                            return Json("Failed", JsonRequestBehavior.AllowGet);
                        }
                    }
                    catch (Exception ex)
                    {
                        return Json("Failed", JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                return Json("Failed", JsonRequestBehavior.AllowGet);
            }
            return Json("Success", JsonRequestBehavior.AllowGet);
        }

        public ActionResult Logout()
        {
            Session["uid"] = null;
            Session["pwd"] = null;
            return RedirectToAction("Login");
        }
    }
}