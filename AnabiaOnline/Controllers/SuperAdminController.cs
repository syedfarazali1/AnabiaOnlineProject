using AnabiaOnline.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace AnabiaOnline.Controllers
{
    public class SuperAdminController : Controller
    {
        Random rd = new Random();

        db_AnabiaEntities db = new db_AnabiaEntities();
        #region Login

        [HttpGet]
        public ActionResult User_Login()
        {
            if (Session["Userid"] == null && Session["UserName"] == null)
            {
                return View();
            }
            return RedirectToAction("Dashboard", "SuperAdmin");
        }  
            
            [HttpPost]
        public ActionResult User_Login(string user, string pass)
        {
            try
            {
               
                var admin = db.Users.Where(x => x.LoginID == user && x.Password == pass).FirstOrDefault();
                 if (admin != null)
                {
                    Session["Userid"] = admin.UserID;
                    Session["UserName"] = admin.FirstName + " " + admin.LastName;
                    FormsAuthentication.SetAuthCookie(admin.LoginID.ToString(), false);
                    return RedirectToAction("Dashboard", "SuperAdmin");
                }
                else
                {
                    ViewBag.massages = "error";

                }
                return View();
            }
            catch (Exception)
            {
                return RedirectToAction("Error");
                throw;
            }
          
        }

        public ActionResult Signout()
        {
            FormsAuthentication.SignOut();
            Session["Userid"] = null;
            Session["UserName"] = null;
            return RedirectToAction("AnabiaOnline_Home");
        }
        #endregion

        //[Authorize (Roles = "Admin")]
        #region Packages

        public ActionResult Packages()
        {


            return View();
        }

        #endregion

        #region AnabiaHome and Dashboad
        [HttpGet]
        public ActionResult AnabiaOnline_Home()
        {
            return View();

        }
        [HttpGet]
        public ActionResult Dashboard()
        {
            if (Session["Userid"] != null )
            {

                return View();

            }
            else if (Session["StoreloginID"] != null)
            {
                return View();
            }

            else if (Session["Userid"] == null || Session["StoreloginID"] == null)
            {
                return RedirectToAction("AnabiaOnline_Home", "SuperAdmin");
            }
           
            return View();
        }

        #endregion

        #region Shops

        [HttpGet]
        public ActionResult Shops()
        {
            try
            {
                var list = db.Stores.Where(x => x.Status == 1).ToList();
                return View(list);
            }
            catch (Exception)
            {

                return RedirectToAction("Error");

                throw;
            }

        }
        [HttpPost]
        public ActionResult Shops(int StoreID)
        {
            try
            {
                Session["tbl_Categories"] = db.ProductCategories.Where(x => x.StoreID == StoreID).ToList();
                Session["Store"] = db.Stores.Where(x => x.StoreID == StoreID).ToList();
                Session["StoreID"] = StoreID;

                return RedirectToAction("Home", "Store");
            }
            catch (Exception)
            {

                return RedirectToAction("Error");

                throw;
            }

        }


        #endregion

        #region City and Country
        public ActionResult CityIndex()
        {

            return View(db.Cities.ToList());
        }
        public ActionResult CountryIndex()
        {

            return View(db.Countries.ToList());
        }

        #endregion

        #region Error
        [HttpGet]
        public ActionResult Error()
        {
            ViewBag.massage = "maintenance are going on in this page";
            return View();
        }

        #endregion

        #region Stores


        [HttpGet]
        public ActionResult Stores()
        {
            ViewBag.countrylist = db.Countries.ToList();
            ViewBag.Citieslist = db.Cities.ToList();
            ViewBag.Packageslist = db.Packages.ToList();
            return View();
        } 
       


        [HttpPost]
        public ActionResult Stores( Store store,HttpPostedFileBase BackCopyOfCnic,HttpPostedFileBase FrontCopyOfCnic,HttpPostedFileBase Store_Logo)
        { try
            {   string extension1 = Path.GetExtension(BackCopyOfCnic.FileName);
                string extension2 = Path.GetExtension(FrontCopyOfCnic.FileName);
                string extension3 = Path.GetExtension(Store_Logo.FileName);
                if ((extension3 == ".png"  || extension3 == ".jpg") &&(extension1 == ".png"  || extension1 == ".jpg") && (extension2 == ".png" || extension2 == ".jpg"))
                {
                    string _fileName4 = "files_" + rd.Next(100, 100000) + DateTime.Now.ToString("HHmmssddMMYYYY") + Path.GetExtension(Store_Logo.FileName);
                    string _Path4 = Path.Combine(Server.MapPath("~/files"), _fileName4);
                    Store_Logo.SaveAs(_Path4);
                    store.Store_Logo = _fileName4;

                    string _fileName = "files_" + rd.Next(100, 100000) + DateTime.Now.ToString("HHmmssddMMYYYY") + Path.GetExtension(BackCopyOfCnic.FileName);
                    string _Path = Path.Combine(Server.MapPath("~/files"), _fileName);
                    BackCopyOfCnic.SaveAs(_Path);
                    store.FrontCopyOfCnic = _fileName;
                    string _fileName2 = "files_" + rd.Next(100, 100000) + DateTime.Now.ToString("HHmmssddMMYYYY") + Path.GetExtension(FrontCopyOfCnic.FileName);
                    string _Path2 = Path.Combine(Server.MapPath("~/files"), _fileName2);
                    BackCopyOfCnic.SaveAs(_Path2);
                    store.BackCopyOfCnic = _fileName2;
                    string date = DateTime.Now.ToString("d");
                    store.RegistrationDate = date;
                    db.Stores.Add(store);
                    db.SaveChanges();
                    TempData["Massages"] = "massage";
                    return RedirectToAction("AnabiaOnline_Home");
                }
                ViewBag.countrylist = db.Countries.ToList();
                ViewBag.Citieslist = db.Cities.ToList();
                ViewBag.Packageslist = db.Packages.ToList();
                ViewBag.massage = "file type must png or jpg";
                return View();
              
            }
            catch (Exception )
            {
                return RedirectToAction("Error");
            }
           
        }
        

       
        [HttpGet]
        public ActionResult StoresListbyapproved()
        {
            try
            {
                if (Session["StoreloginID"] != null && Session["StoreBussniessName"] != null)
                {
                    var list = db.Stores.Where(x => x.Status == 1).ToList();
                    return View(list);
                }
                else
                {
                    return RedirectToAction("User_Login");
                }
                
            }
            catch (Exception )
            {
                
                  return  RedirectToAction("Error");
                
                throw;
            }
        } 
        [HttpGet]
        public ActionResult StoresListbyapprovedDetails(int id)
        {
            try
            {
                if (Session["StoreloginID"] != null && Session["StoreBussniessName"] != null)
                {
                    var list = db.Stores.Where(x => x.Status == 1 && x.StoreID == id).FirstOrDefault();
                    return View(list);
                }
                else
                {
                    return RedirectToAction("User_Login");
                }
                
            }
            catch (Exception )
            {
                
                  return  RedirectToAction("Error");
                
                throw;
            }
        } 
        [HttpGet]
        public ActionResult StoresListbypanding()
        {
            try
            {
                if (Session["StoreloginID"] != null && Session["StoreBussniessName"] != null)
                {
                    var list = db.Stores.Where(x => x.Status == 0).ToList();
                    return View(list);
                }
                else
                {
                    return RedirectToAction("User_Login");
                }
               
            }
            catch (Exception)
            {
                return RedirectToAction("Error");

                throw;
            }
           
        }
        [HttpGet]
        public ActionResult StoresListbypandingDetails(int id)
        {
            try
            {
                if (Session["StoreloginID"] != null && Session["StoreBussniessName"] != null)
                {
                    var list = db.Stores.Where(x => x.Status == 0 && x.StoreID == id).FirstOrDefault();
                    return View(list);
                }
                else
                {
                    return RedirectToAction("User_Login");
                }
                
            }
            catch (Exception)
            {
                return RedirectToAction("Error");

                throw;
            }
           
        }
        [HttpGet]
        public ActionResult Statuschange(int storeid , int status)
        {
            try
            {
                if (Session["StoreloginID"] != null && Session["StoreBussniessName"] != null)
                {
                    if (status == 0)
                    {
                        var pan = db.Stores.Where(x => x.StoreID == storeid).FirstOrDefault();
                        pan.Status = 1;
                        var a = Session["Userid"];
                        pan.ApprovedBy = Convert.ToInt16(a);
                        db.SaveChanges();
                        return RedirectToAction("StoresListbyapproved");
                    }
                    else if (status == 1)
                    {
                        var pan = db.Stores.Where(x => x.StoreID == storeid).FirstOrDefault();
                        pan.Status = 0;
                        var a = Session["Userid"];
                        pan.ApprovedBy = Convert.ToInt16(a);
                        db.SaveChanges();
                        return RedirectToAction("StoresListbypanding");
                    }

                    return View();
                }
                else
                {
                    return RedirectToAction("User_Login");
                }
               
            }
            catch (Exception)
            {
                return RedirectToAction("Error");

                throw;
            }
           
        }

        #endregion
        
        #region checkdatafromdatabase

        public JsonResult CheckDomaim(string Domain)
        {
            System.Threading.Thread.Sleep(200);
            var SeachData = db.Stores.Where(x => x.DomainName == Domain).SingleOrDefault();
            if (SeachData != null)
            {
                return Json(1);
            }
            else
            {
                return Json(0);

            }
        }
        public JsonResult CheckBussname(string bussiness)
        {
            System.Threading.Thread.Sleep(200);
            var SeachData = db.Stores.Where(x => x.BusinessName == bussiness).SingleOrDefault();
            if (SeachData != null)
            {
                return Json(1);
            }
            else
            {
                return Json(0);

            }
        }
        #endregion


        #region Contact

        public ActionResult Contact()
        {


            return View();
        }

        #endregion
    }
}