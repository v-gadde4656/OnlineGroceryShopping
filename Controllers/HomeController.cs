using System;
using System.Collections.Generic;
using System.Data;
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
            string userRole = "";
            try
            {
                using (MySql.Data.MySqlClient.MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection(myConnectionString))
                {
                    MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand("select Role from consumers o where o.email=@userName and password=@password", conn);
                    cmd.Parameters.AddWithValue("@userName", obj.userName);
                    cmd.Parameters.AddWithValue("@password", obj.password);
                    try
                    {
                        conn.Open();
                        userRole = Convert.ToString(cmd.ExecuteScalar());
                        if (userRole == "")
                        {
                            ViewBag.Message = "User is not Valid.";
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
                userRole = "";
            }
            Session["UserRole"] = userRole;
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
            //string Role = "";
            try
            {
                using (MySql.Data.MySqlClient.MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection(myConnectionString))
                {
                    //-------------
                    MySql.Data.MySqlClient.MySqlCommand cmd1 = new MySql.Data.MySqlClient.MySqlCommand("select Count(*) from consumers o where o.email=@username and password=@password", conn);
                    cmd1.Parameters.AddWithValue("@username", obj.email);
                    cmd1.Parameters.AddWithValue("@password", obj.password);
                    try
                    {
                        conn.Open();
                        count = Convert.ToInt32(cmd1.ExecuteScalar()); 
                        if (count > 0)
                        {
                            ViewBag.Message = "User Name Already Exist.";
                            return View("UserRegistration");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    //-------------
                    MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand("insert into consumers(name,gender,email,mobile,password,Role) values(@name,@gender,@email,@mobile,@password,@Role)", conn);
                    cmd.Parameters.AddWithValue("@name", obj.name);
                    cmd.Parameters.AddWithValue("@gender", obj.gender);
                    cmd.Parameters.AddWithValue("@email", obj.email);
                    cmd.Parameters.AddWithValue("@mobile", obj.mobile);
                    cmd.Parameters.AddWithValue("@password", obj.password);
                    cmd.Parameters.AddWithValue("@Role", obj.Role);
                    try
                    {
                        //conn.Open();
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
           if ((Session["UserRole"]).ToString()== "Owner")
           { 
               return View("Index");
           }
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
            double TotAmt=(Convert.ToDouble(obj.Amount) * Convert.ToDouble(obj.Quantity));
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
                    MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand("Update orders set billstatus = 'PAID', PayMode=@PayMode, PayAmount=@PayAmount, DueAmount=@DueAmount  Where ordernumber=@ordernumber", conn);
                    cmd.Parameters.AddWithValue("@ordernumber", obj.OrdId);
                    cmd.Parameters.AddWithValue("@PayMode", obj.PayMode);
                    cmd.Parameters.AddWithValue("@PayAmount", obj.PayAmount);
                    cmd.Parameters.AddWithValue("@DueAmount", obj.DueAmount);
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

        [HttpGet]
        public ActionResult OrderDetail()
        {
            DataSet ds = new DataSet("OrderDetail");
            Models.OrderDetail OrderDetail = new Models.OrderDetail();
            try
            {
                string Query = "";
                if ((Session["UserRole"]).ToString() == "Owner")
                {
                    Query = "SELECT C.name,O.ordernumber, O.orderdate, O.billamount, O.groceryname, O.billstatus, O.PayMode, O.PayAmount, O.DueAmount FROM consumers C, orders O WHERE C.consumerid = O.consumerid  ORDER BY O.orderdate";
                }
                else
                {
                    Query = "SELECT C.name,O.ordernumber, O.orderdate, O.billamount, O.groceryname, O.billstatus, O.PayMode, O.PayAmount, O.DueAmount FROM consumers C, orders O WHERE C.consumerid = O.consumerid AND C.consumerid = (select consumerid from consumers o where o.email = '" + (Session["uid"]).ToString() + "' and password = '" + (Session["pwd"]).ToString() + "') ORDER BY O.orderdate";
                }
                using (MySql.Data.MySqlClient.MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection(myConnectionString))
                {
                    MySql.Data.MySqlClient.MySqlDataAdapter da = new MySql.Data.MySqlClient.MySqlDataAdapter(Query, conn);
                    da.Fill(ds, "OrderDetail");
                    OrderDetail.dt = ds.Tables[0];
                    
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
            }
            return View(OrderDetail);
        }
    }
}