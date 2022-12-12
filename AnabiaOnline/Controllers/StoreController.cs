using AnabiaOnline.Models;
using Rotativa;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace AnabiaOnline.Controllers
{
    public class StoreController : Controller
    {
        db_AnabiaEntities db = new db_AnabiaEntities();
        List<addtocard> li = new List<addtocard>();

        #region Home
        [HttpGet]
        [Route("Home/{Store?}")]
        public ActionResult Home(string Store)
        {
            if (Store == null && Session["StoreID"] == null)
            {
                return RedirectToAction("Shops", "SuperAdmin");
            }

            else
            {


                if (Store != null)
                {
                    string dmoin = Store;

                    var Shopid = db.Stores.Where(e => e.DomainName == Store).Select(e => e.StoreID).FirstOrDefault();

                    Session["PID"] = Convert.ToInt32(Shopid);

                    Session["tbl_Categories"] = db.ProductCategories.Where(x => x.StoreID == Shopid).ToList();

                    Session["Store"] = db.Stores.Where(x => x.StoreID == Shopid).ToList();

                    Session["StoreID"] = Shopid;

                    var data = db.Stores.Include("Package1").Include("ProductCategories").Include("ProductImages").Include("Products").Include("StoreWebs").Where(x => x.StoreID == Shopid).ToList();
                    var list = db.logoes.Where(x => x.StoreID == Shopid).ToList();
                    Session["Logo"] = list;
                    return View(data);
                }

                int id = Convert.ToInt32(Session["StoreID"]);
                var getalldata = db.Stores.Include("Package1").Include("ProductCategories").Include("ProductImages").Include("Products").Include("StoreWebs").Where(x => x.StoreID == id).ToList();


                return View(getalldata);

            }
        }
        [HttpPost]
        [Route("Home/{id?}")]
        public ActionResult Home(int id)
        {

            Session["PID"] = id;
            return RedirectToAction("Details", "Store");
        }
        #endregion

        #region Shop

        [HttpGet]
        public ActionResult Shop()
        {
            if (Session["StoreID"] != null)
            {
                int id = Convert.ToInt32(Session["StoreID"]);
                var list = db.Stores.Include("Package1").Include("ProductCategories").Include("ProductImages").Include("Products").Include("StoreWebs").Where(x => x.StoreID == id).ToList();

                return View(list);
            }
            else
            {
                return RedirectToAction("Home");
            }
        }

        #endregion

        #region Contact page
        [HttpGet]
        public ActionResult Contact()
        {
            Session["Country"] = db.Countries.ToList();
            if (Session["StoreID"] != null)
            {
                int id = Convert.ToInt32(Session["StoreID"]);
                var contact = db.Stores.Include("Package1").Include("ProductCategories").Include("ProductImages").Include("Products").Include("StoreWebs").Where(x => x.StoreID == id).ToList();


                return View(contact);
            }
            return RedirectToAction("Shops", "SuperAdmin");

        }

        #endregion

        #region Login
        [HttpGet]
        public ActionResult User_Login()
        {
            
            if (Session["StoreloginID"] != null && Session["StoreBussniessName"] != null)
            {
                return View();
            }

            else
            {
                return RedirectToAction("Dashboard", "SuperAdmin");
            }
        }

        [HttpPost]
        public ActionResult User_Login(string user, string pass)
        {
            try
            {
                if (Session["StoreloginID"] == null && Session["StoreBussniessName"] == null)
                {

              
                var list = db.Stores.Where(x => x.LoginID == user && x.Password == pass).FirstOrDefault();
                if (list != null)
                {
                    Session["StoreloginID"] = list.StoreID;
                    Session["StoreBussniessName"] = list.BusinessName;
                    Session["StoreID"] = list.StoreID;
                    Session["StoreName"] = list.BusinessName;

                    return RedirectToAction("Dashboard", "SuperAdmin");


                }
                else
                {
                    ViewBag.massages = "error";

                }
                return View();
                }
                return RedirectToAction("Dashboard", "SuperAdmin");
            }
            catch (Exception)
            {
                return RedirectToAction("Error");
                throw;
            }

        }

        public ActionResult Signout()
        {
            try
            {
                if (Session["StoreloginID"] != null && Session["StoreBussniessName"] != null)
                {
                    //FormsAuthentication.SignOut();
                    Session["StoreloginID"] = null;
                    Session["StoreBussniessName"] = null;
                    return RedirectToAction("User_Login");
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

        #region Category
        [HttpGet]
        public ActionResult Productbycatid(int id)
        {
            ViewBag.ProductImages = db.Products.Where(x => x.CategoryID == id).ToList();
            return View();
        }
        [HttpGet]
        public ActionResult CategoryList()
        {
            try
            {
                if (Session["StoreloginID"] != null && Session["StoreBussniessName"] != null)
                {
                    string id = Session["StoreID"].ToString();
                    ViewBag.list = db.ProductCategories.Where(x => x.StoreID.ToString() == id).ToList();
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

            }

        }
        [HttpGet]
        public ActionResult AddCategory()
        {
            if (Session["StoreloginID"] != null && Session["StoreBussniessName"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("User_Login");
            }
            
        }
        [HttpPost]
        public ActionResult AddCategory(ProductCategory category)
        {
            try
            {
                int id = Convert.ToInt32(Session["StoreID"]);

                var packageid = db.Stores.Where(x => x.StoreID == id).FirstOrDefault();
                var package = packageid.Package;
                if (package == 1)
                {
                    var list = db.ProductCategories.Where(x => x.StoreID == id).ToList();
                    if (list.Count == 3)
                    {
                        TempData["package"] = "Can Not Allow Add More Category must update your package";
                        return RedirectToAction("CategoryList");
                    }
                    category.StoreID = Convert.ToInt32(id);
                    category.CreationDate = DateTime.Now.ToString("MM/dd/yyyy");
                    db.ProductCategories.Add(category);
                    db.SaveChanges();
                    TempData["AddCategory"] = "massage";
                    return RedirectToAction("CategoryList");
                }
                if (package == 2)
                {
                    var list = db.ProductCategories.Where(x => x.StoreID == id).ToList();
                    if (list.Count == 5)
                    {
                        TempData["package"] = "Con Not Allow Add More Category must update your package";
                        return RedirectToAction("CategoryList");
                    }
                    category.StoreID = Convert.ToInt32(id);
                    category.CreationDate = DateTime.Now.ToString("MM/dd/yyyy");
                    db.ProductCategories.Add(category);
                    db.SaveChanges();
                    TempData["AddCategory"] = "massage";
                    return RedirectToAction("CategoryList");
                }
                category.StoreID = Convert.ToInt32(id);
                category.CreationDate = DateTime.Now.ToString("MM/dd/yyyy");
                db.ProductCategories.Add(category);
                db.SaveChanges();
                TempData["AddCategory"] = "massage";
                return RedirectToAction("CategoryList");

            }
            catch (Exception)
            {
                return RedirectToAction("Error");

                throw;
            }

        }
        [HttpGet]
        public ActionResult UpdateCategory(int id)
        {
            try
            {
                if (Session["StoreloginID"] != null && Session["StoreBussniessName"] != null)
                {
                    ProductCategory category = new ProductCategory();
                    var sid = Session["StoreID"];
                    category.StoreID = Convert.ToInt32(sid);
                    category = db.ProductCategories.Where(x => x.CategoryID == id && x.StoreID == category.StoreID).FirstOrDefault();
                    return View(category);
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
        [HttpPost]
        public ActionResult UpdateCategory(ProductCategory category)
        {
            try
            {
                var list = db.ProductCategories.Find(category.CategoryID);
                list.CategoryName = category.CategoryName;
                list.CreationDate = category.CreationDate;
                var id = Session["StoreID"];
                list.StoreID = Convert.ToInt32(id);
                db.SaveChanges();
                return RedirectToAction("CategoryList");
            }
            catch (Exception)
            {
                return RedirectToAction("Error");

                throw;
            }

        }

        [HttpGet]
        public ActionResult DeleteCategory(int id)
        {
            try
            {
                if (Session["StoreloginID"] != null && Session["StoreBussniessName"] != null)
                {
                    var cat = db.ProductCategories.Find(id);
                    db.ProductCategories.Remove(cat);
                    db.SaveChanges();
                    return RedirectToAction("CategoryList");
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

        #region Product
        [HttpGet]
        public ActionResult ProductList()
        {
            try
            {
                if (Session["StoreloginID"] != null && Session["StoreBussniessName"] != null)
                {
                    string id = Session["StoreID"].ToString();
                    var list = db.Products.Where(x => x.StoreID.ToString() == id).ToList();
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

            }

        }
        [HttpGet]
        public ActionResult AddProduct()
        {
            try
            {
                if (Session["StoreloginID"] != null && Session["StoreBussniessName"] != null)
                {
                    string id = Session["StoreID"].ToString();
                    ViewBag.list = db.ProductCategories.Where(x => x.StoreID.ToString() == id).ToList();
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

            }

        }
        [HttpPost]
        public ActionResult AddProduct(Product product, HttpPostedFileBase proimg)
        {
            try
            {
                Random rd = new Random();
                string extension1 = Path.GetExtension(proimg.FileName);
                if ((extension1 == ".png" || extension1 == ".jpg"))
                {

                    string _fileName = "files_" + rd.Next(100, 100000) + DateTime.Now.ToString("HHmmssddMMYYYY") + Path.GetExtension(proimg.FileName);
                    string _Path = Path.Combine(Server.MapPath("~/UserAsset/img/ProductImages"), _fileName);
                    proimg.SaveAs(_Path);
                    product.ProductImage = _fileName;
                    var id = Session["StoreID"];
                    product.StoreID = Convert.ToInt32(id);
                    product.CreationDate = DateTime.Now.ToString("MM/dd/yyyy");
                    db.Products.Add(product);
                    db.SaveChanges();
                    TempData["AddProduct"] = "massage";

                    return RedirectToAction("ProductList");

                }
                else
                {
                    TempData["filetype"] = "error";
                    return RedirectToAction("ProductList");

                }
            }
            catch (Exception)
            {
                return RedirectToAction("Error");

                throw;
            }

        }
        [HttpGet]
        public ActionResult EditProduct(int id)
        {
            try
            {
                if (Session["StoreloginID"] != null && Session["StoreBussniessName"] != null)
                {
                    Product product = new Product();
                    string pid = Session["StoreID"].ToString();
                    var list = db.Products.Where(x => x.ProductID == id && x.StoreID.ToString() == pid).FirstOrDefault();
                    ViewBag.catlist = db.ProductCategories.Where(x => x.StoreID.ToString() == pid).ToList();
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

            }

        }
        [HttpPost]
        public ActionResult EditProduct(Product product)
        {
            try
            {
                Product product1 = db.Products.Find(product.ProductID);
                product1.CategoryID = product.CategoryID;
                product1.DeliveryCharges = product.DeliveryCharges;
                product1.OrderDetails = product.OrderDetails;
                product1.Price = product.Price;
                product1.ProductDescription = product.ProductDescription;
                product1.ProductName = product.ProductName;

                var id = Session["StoreID"];
                product1.StoreID = Convert.ToInt32(id);
                product1.CreationDate = DateTime.Now.ToString("MM/dd/yyyy");
                db.SaveChanges();
                return RedirectToAction("ProductList");
            }
            catch (Exception)
            {
                return RedirectToAction("Error");

                throw;
            }

        }
        [HttpGet]
        public ActionResult ProductStatus(int id)
        {
            if (Session["StoreloginID"] != null && Session["StoreBussniessName"] != null)
            {
                var list = db.Products.Where(x => x.ProductID == id).FirstOrDefault();

                if (list.ProductStatus == 1)
                {
                    list.ProductStatus = 0;
                    db.SaveChanges();
                }

                else
                {
                    list.ProductStatus = 1;
                    db.SaveChanges();
                }

                return RedirectToAction("ProductList");
            }
            else
            {
                return RedirectToAction("User_Login");
            }
            
        }
        [HttpGet]
        public ActionResult ProductStockStatus(int id)
        {
            if (Session["StoreloginID"] != null && Session["StoreBussniessName"] != null)
            {
                var list = db.Products.Where(x => x.ProductID == id).FirstOrDefault();

                if (list.ProductStockStatus == 1)
                {
                    list.ProductStockStatus = 0;
                    db.SaveChanges();
                }

                else
                {
                    list.ProductStockStatus = 1;
                    db.SaveChanges();
                }

                return RedirectToAction("ProductList");
            }
            else
            {
                return RedirectToAction("User_Login");
            }

        }


        #endregion

        #region Product Images

        [HttpGet]
        public ActionResult ProductImageDelete(int id)
        {
            try
            {
                if (Session["StoreloginID"] != null && Session["StoreBussniessName"] != null)
                {
                    ProductImage proimg = new ProductImage();
                    string sid = Session["StoreID"].ToString();
                    proimg = db.ProductImages.Where(x => x.StoreID.ToString() == sid && x.ProductImagesID == id).FirstOrDefault();
                    db.ProductImages.Remove(proimg);
                    db.SaveChanges();
                    return RedirectToAction("ProductList");
                }
                else
                {
                    return RedirectToAction("User_Login");
                }
             
            }
            catch (Exception)
            {
                return RedirectToAction("Error");

            }

        }
        [HttpGet]
        public ActionResult ProductImagesList(int id)
        {
            try
            {
                if (Session["StoreloginID"] != null && Session["StoreBussniessName"] != null)
                {
                    string sid = Session["StoreID"].ToString();
                    var list = db.ProductImages.Where(x => x.StoreID.ToString() == sid && x.ProductID == id).ToList();
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

            }

        }


        [HttpGet]
        public ActionResult AddProductImages(int? id)
        {
            if (Session["StoreloginID"] != null && Session["StoreBussniessName"] != null)
            {
                ViewBag.id = id;
                return View();
            }
            else
            {
                return RedirectToAction("User_Login");
            }
            
        }

        [HttpPost]
        public ActionResult AddProductImages(ProductImage image, HttpPostedFileBase proimg)
        {
            var list = db.ProductImages.Where(x => x.ProductID == image.ProductID).ToList();
            if (list.Count > 3)
            {
                TempData["error"] = "error";
                return RedirectToAction("ProductList");
            }
            else
            {


                try
                {



                    Random rd = new Random();
                    string extension1 = Path.GetExtension(proimg.FileName);
                    if ((extension1 == ".png" || extension1 == ".jpg"))
                    {
                        string sid = Session["StoreID"].ToString();
                        string _fileName = "files_" + rd.Next(100, 100000) + DateTime.Now.ToString("HHmmssddMMYYYY") + Path.GetExtension(proimg.FileName);
                        string _Path = Path.Combine(Server.MapPath("~/UserAsset/img/ProductImages"), _fileName);
                        proimg.SaveAs(_Path);
                        image.ProductImage1 = _fileName;
                        image.StoreID = Convert.ToInt32(sid);
                        db.ProductImages.Add(image);
                        db.SaveChanges();
                        return RedirectToAction("ProductImagesList", new
                        {
                            id = image.ProductID
                        });
                    }
                    else
                    {
                        TempData["filetype"] = "error";
                        return RedirectToAction("ProductList");

                    }

                }

                catch (Exception)
                {
                    return RedirectToAction("Error");

                }
            }

        }

        #endregion

        #region Webstore
        [HttpGet]
        public ActionResult WebstoreIndex()
        {
            try
            {
                if (Session["StoreloginID"] != null && Session["StoreBussniessName"] != null)
                {
                    string id = Session["StoreID"].ToString();
                    var list = db.StoreWebs.Where(x => x.StoreID.ToString() == id).ToList();
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

            }

        }
        [HttpGet]
        public ActionResult WebstoreCreate()
        {
            if (Session["StoreloginID"] != null && Session["StoreBussniessName"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("User_Login");
            }
            
        }

        [HttpPost]
        public ActionResult WebstoreCreate(StoreWeb storeWeb, HttpPostedFileBase StoreBanner1, HttpPostedFileBase StoreBanner2, HttpPostedFileBase StoreBanner3)
        {
            try
            {
                int sids = Convert.ToInt32(Session["StoreID"]);
            List<StoreWeb> storeWeb1 = new List<StoreWeb>();
            storeWeb1 = db.StoreWebs.Where(x => x.StoreID == sids).ToList();
            if (storeWeb1.Count == 0)
            {
                Random rd = new Random();
                string extension1 = Path.GetExtension(StoreBanner1.FileName);
                string extension2 = Path.GetExtension(StoreBanner2.FileName);
                string extension3 = Path.GetExtension(StoreBanner3.FileName);


                if ((extension1 == ".png" || extension1 == ".jpg") && (extension2 == ".png" || extension2 == ".jpg") && (extension3 == ".png" || extension3 == ".jpg"))
                {

                    string sid = Session["StoreID"].ToString();

                    string _fileName = "files_" + rd.Next(100, 100000) + DateTime.Now.ToString("HHmmssddMMYYYY") + Path.GetExtension(StoreBanner1.FileName);
                    string _Path = Path.Combine(Server.MapPath("~/UserAsset/img/ProductImages"), _fileName);
                    StoreBanner1.SaveAs(_Path);

                    string _fileName2 = "files_" + rd.Next(100, 100000) + DateTime.Now.ToString("HHmmssddMMYYYY") + Path.GetExtension(StoreBanner2.FileName);
                    string _Path2 = Path.Combine(Server.MapPath("~/UserAsset/img/ProductImages"), _fileName2);
                    StoreBanner2.SaveAs(_Path2);

                    string _fileName3 = "files_" + rd.Next(100, 100000) + DateTime.Now.ToString("HHmmssddMMYYYY") + Path.GetExtension(StoreBanner3.FileName);
                    string _Path3 = Path.Combine(Server.MapPath("~/UserAsset/img/ProductImages"), _fileName3);
                    StoreBanner3.SaveAs(_Path3);




                    storeWeb.StoreBanner1 = _fileName;
                    storeWeb.StoreBanner2 = _fileName2;
                    storeWeb.StoreBanner3 = _fileName3;
                    storeWeb.StoreID = Convert.ToInt32(sid);
                    storeWeb.CreationDate = DateTime.Now.ToString("MM/dd/yyyy");
                    db.StoreWebs.Add(storeWeb);
                    db.SaveChanges();
                        TempData["Add"] = "Message";
                        return RedirectToAction("WebstoreIndex");

                }
                else
                {
                    TempData["filetype"] = "error";
                    return RedirectToAction("WebstoreIndex");

                }
            }

            else
            {
                TempData["already"] = "error";
                return RedirectToAction("WebstoreIndex");
            }
            }

            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return RedirectToAction("Error");

            }

        }
        [HttpGet]
        public ActionResult WebstoreEdit(int id)
        {
            if (Session["StoreloginID"] != null && Session["StoreBussniessName"] != null)
            {
                StoreWeb list = new StoreWeb();
                list = db.StoreWebs.Where(x => x.StoreWebID == id).FirstOrDefault();
                return View(list);
            }
            else
            {
                return RedirectToAction("User_Login");
            }
            
        }

        [HttpPost]
        public ActionResult WebstoreEdit(StoreWeb storeWebs, HttpPostedFileBase StoreBanner1, HttpPostedFileBase StoreBanner2, HttpPostedFileBase StoreBanner3)
        {
            try
            {
                Random rd = new Random();
                var storeWeb = db.StoreWebs.Where(x => x.StoreWebID == storeWebs.StoreWebID).FirstOrDefault();
                string extension1 = Path.GetExtension(StoreBanner1.FileName);
                string extension2 = Path.GetExtension(StoreBanner2.FileName);
                string extension3 = Path.GetExtension(StoreBanner3.FileName);

                if ((extension1 == ".png" || extension1 == ".jpg") && (extension2 == ".png" || extension2 == ".jpg") && (extension3 == ".png" || extension3 == ".jpg"))
                {

                    string sid = Session["StoreID"].ToString();

                    string _fileName = "files_" + rd.Next(100, 100000) + DateTime.Now.ToString("HHmmssddMMYYYY") + Path.GetExtension(StoreBanner1.FileName);
                    string _Path = Path.Combine(Server.MapPath("~/UserAsset/img/ProductImages"), _fileName);
                    StoreBanner1.SaveAs(_Path);

                    string _fileName2 = "files_" + rd.Next(100, 100000) + DateTime.Now.ToString("HHmmssddMMYYYY") + Path.GetExtension(StoreBanner2.FileName);
                    string _Path2 = Path.Combine(Server.MapPath("~/UserAsset/img/ProductImages"), _fileName2);
                    StoreBanner2.SaveAs(_Path2);

                    string _fileName3 = "files_" + rd.Next(100, 100000) + DateTime.Now.ToString("HHmmssddMMYYYY") + Path.GetExtension(StoreBanner3.FileName);
                    string _Path3 = Path.Combine(Server.MapPath("~/UserAsset/img/ProductImages"), _fileName3);
                    StoreBanner3.SaveAs(_Path3);




                    storeWeb.StoreBanner1 = _fileName;
                    storeWeb.StoreBanner2 = _fileName2;
                    storeWeb.StoreBanner3 = _fileName3;

                    storeWeb.StoreID = Convert.ToInt32(sid);
                    storeWeb.CreationDate = DateTime.Now.ToString("MM/dd/yyyy");
                    db.SaveChanges();
                }
                else
                {
                    TempData["filetype"] = "error";
                    return RedirectToAction("ProductList");

                }

            }

            catch (Exception)
            {
                return RedirectToAction("Error");

            }
            return View();
        }

        #endregion

        #region Addtocart

        [HttpGet]
        public ActionResult Addtocart()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Addtocart(int qty)
        {
            int id = Convert.ToInt16(Session["PID"]);


            var p = db.Products.Where(x => x.ProductID == id).SingleOrDefault();


            addtocard c = new addtocard();
            c.Pro_Id = p.ProductID;
            c.Pro_Name = p.ProductName;
            c.Pro_Price = Convert.ToDecimal(p.Price);
            c.Ord_Quantity = qty;
            c.Ord_Bill = Convert.ToDecimal(qty) * Convert.ToDecimal(p.Price);
            if (Session["cart"] == null)
            {
                li.Add(c);
                Session["cart"] = li;

            }

            else
            {
                List<addtocard> li2 = Session["cart"] as List<addtocard>;
                int flag = 0;
                foreach (var item in li2)
                {
                    if (item.Pro_Id == c.Pro_Id)
                    {
                        item.Ord_Quantity += c.Ord_Quantity;
                        item.Ord_Bill += c.Ord_Bill;
                        flag = 1;
                    }

                    else
                    {
                        li.Add(c);
                        Session["cart"] = li;
                    }
                }
                if (flag == 0)
                {
                    li2.Add(c);
                }
                Session["cart"] = li2;
            }


            if (Session["cart"] != null)
            {
                decimal? SubTotal = 0;
                List<addtocard> li2 = Session["cart"] as List<addtocard>;
                foreach (var item in li2)
                {
                    SubTotal += item.Ord_Bill;
                }
                var Shipping = SubTotal * 5 / 100;
                var Total = SubTotal + Shipping;
                Session["SubTotal"] = SubTotal;
                Session["Shipping"] = Shipping;
                Session["Total"] = Total;
            }


            TempData.Keep();


            return View();
        }

        [HttpPost]
        public ActionResult RemoveCart(int pid, int qty)
        {
            TempData["Productid"] = pid;
            if (Session["cart"] == null)
            {
                Session["SubTotal"] = null;
                Session["Shipping"] = null;
                Session["Total"] = null;
            }
            else
            {

                List<addtocard> li2 = Session["cart"] as List<addtocard>;
                addtocard c = li2.Where(x => x.Pro_Id == pid).FirstOrDefault();
                li2.Remove(c);
                decimal? s = 0;
                decimal? Dis = 0;
                foreach (var item in li2)
                {
                    s += item.Ord_Bill;
                }
                foreach (var item in li2)
                {
                    Dis += item.DeliveryCharges;
                }
                Session["SubTotal"] = s;
                var Shipping = Dis;
                Session["Shipping"] = Shipping;
                var Total = s + Shipping;
                Session["Total"] = Total;
            }
            return RedirectToAction("Addtocart");
        }

        #endregion

        #region Details
        [HttpGet]
        public ActionResult Details()
        {

            int id = Convert.ToInt16(Session["PID"]);
            int ids = Convert.ToInt16(Session["StoreID"]);

            Product pro = new Product();

            pro = db.Products.Include("ProductImages").Where(x => x.ProductID == id && x.StoreID == ids).FirstOrDefault();
            return View(pro);
        }
        [HttpPost]
        public ActionResult Details(int proid, int qty)
        {
            Session["PID"] = proid;


            var p = db.Products.Where(x => x.ProductID == proid).SingleOrDefault();


            addtocard c = new addtocard();
            c.Pro_Id = p.ProductID;
            c.Pro_Name = p.ProductName;
            c.Pro_Price = Convert.ToDecimal(p.Price);
            c.Ord_Quantity = qty;
            c.DeliveryCharges = Convert.ToDecimal(p.DeliveryCharges);
            c.Ord_Bill = Convert.ToDecimal(qty) * Convert.ToDecimal(p.Price);
            if (Session["cart"] == null)
            {
                li.Add(c);
                Session["cart"] = li;

            }

            else
            {
                List<addtocard> li2 = Session["cart"] as List<addtocard>;
                int flag = 0;
                foreach (var item in li2)
                {
                    if (item.Pro_Id == c.Pro_Id)
                    {
                        item.Ord_Quantity += c.Ord_Quantity;
                        item.Ord_Bill += c.Ord_Bill;

                        flag = 1;
                    }

                    else
                    {
                        li.Add(c);
                        Session["cart"] = li;
                    }
                }
                if (flag == 0)
                {
                    li2.Add(c);
                }
                Session["cart"] = li2;
            }


            if (Session["cart"] != null)
            {
                decimal? SubTotal = 0;
                decimal? DeliveryCharges = 0;
                List<addtocard> li2 = Session["cart"] as List<addtocard>;
                foreach (var item in li2)
                {
                    SubTotal += item.Ord_Bill;
                }
                foreach (var item in li2)
                {
                    DeliveryCharges += item.DeliveryCharges;
                }

                var Shipping = DeliveryCharges;
                var Total = SubTotal + Shipping;
                Session["SubTotal"] = SubTotal;
                Session["Shipping"] = Shipping;
                Session["Total"] = Total;
            }


            TempData.Keep();

            return RedirectToAction("Addtocart");
        }

        #endregion


        #region Checkout
        public ActionResult Checkout()
        {

            ViewBag.Countrylist = db.Countries.ToList();
            var list = db.Cities.ToList();
            ViewBag.Citylist = list;

            return View();
        }

        #endregion

        #region Orderplace

        [HttpGet]
        public ActionResult OrderList()
        {
            Session["City"] = db.Cities.ToList();
            Session["Country"] = db.Countries.ToList();
            int ids = Convert.ToInt16(Session["StoreID"]);
            var ord = db.Orders.Include("OrderDetails").Where(x => x.StoreID == ids && x.OrderStatus == 0).ToList();

            return View(ord);
        }
        [HttpGet]
        public ActionResult Dispatch(int id)
        {
            var ord = db.Orders.Include("OrderDetails").Where(x => x.OrderID == id).FirstOrDefault();
            ord.DeliveryDate = DateTime.Today.AddDays(10).ToString("yyyy-MM-dd");
            ord.DispatchDate = DateTime.Now.ToString("yyyy-MM-dd");
            ord.OrderDispatchedBy = Session["StoreID"].ToString();
            ord.OrderDeliverBy = Session["StoreID"].ToString();
            ord.OrderStatus = 1;
            db.SaveChanges();
            TempData["Massages"] = "massage";
            return RedirectToAction("OrderList");

        }

        [HttpGet]
        public ActionResult DispatchList()
        {
            Session["City"] = db.Cities.ToList();
            Session["Country"] = db.Countries.ToList();
            var list = db.Orders.Where(x => x.OrderStatus == 1).ToList();
            return View(list);


        }




        [HttpPost]
        public ActionResult Pleaseorder(string CustomerName, int ContactNumber, string Address, int Country, int city)
        {
            Order ord = new Order();
            ord.CustomerName = CustomerName;
            ord.ContactNumber = Convert.ToString(ContactNumber);
            ord.Address = Address;
            ord.Country = Country;
            ord.City = city;
            ord.OrderDate = DateTime.Now.ToString("yyyy-MM-dd");

            ord.DispatchDate = DateTime.Now.ToString("yyyy-MM-dd");

            ord.OrderDispatchedBy = Session["StoreID"].ToString();
            ord.StoreID = Convert.ToInt16(Session["StoreID"]);
            ord.DeliveryDate = 0.ToString();

            ord.OrderDeliverBy = Session["StoreID"].ToString();

            ord.OrderStatus = 0;

            db.Orders.Add(ord);
            db.SaveChanges();
            OrderDetail();
            TempData["Massages"] = "massage";
            return RedirectToAction("Home");
        }


        public ActionResult OrderDetails(int id)
        {
            List<OrderDetail> list = new List<OrderDetail>();
            list = db.OrderDetails.Where(x => x.OrderID == id).ToList();

            return View(list);
        }

        public ActionResult Getall(int id)
        {
            var q = new ActionAsPdf("OrderDetails/" + id);
            return q;


        }

        public ActionResult OrderDetail()
        {
            List<addtocard> li2 = Session["cart"] as List<addtocard>;
            foreach (var item in li2)
            {
                List<Product> list = new List<Product>();
                var lists = db.Products.Where(x => x.ProductID == item.Pro_Id).FirstOrDefault();
                var ordid = db.Orders.OrderBy(x => x.OrderID).ToList();
                var tempFileName = db.Orders.OrderByDescending(x => x.OrderID).Take(1).Select(x => x.OrderID).FirstOrDefault();
                OrderDetail ordDetl = new OrderDetail();
                ordDetl.CategoryID = lists.CategoryID;
                ordDetl.DeliveryCharges = lists.DeliveryCharges;
                ordDetl.OrderID = tempFileName;
                ordDetl.Price = lists.Price;
                ordDetl.ProductID = lists.ProductID;
                ordDetl.Quantity = item.Ord_Quantity;
                ordDetl.StoreID = Convert.ToInt16(Session["StoreID"]);
                ordDetl.TotalAmount = lists.Price * item.Ord_Quantity;
                db.OrderDetails.Add(ordDetl);

                db.SaveChanges();


            }
            Session.Remove("cart");
            Session.Remove("Total");
            Session.Remove("Shipping");
            Session.Remove("SubTotal");
            return RedirectToAction("Home");





        }
        #endregion

        #region Error
        [HttpGet]
        public ActionResult Error()
        {
            ViewBag.massage = "Maintenance are going on in this page";
            return View();
        }

        #endregion

        #region city
        public ActionResult GetCity(int CountryId)
        {


            var list = db.Cities.Where(x => x.countryid == CountryId).ToList();
            TempData["list"] = list;
            return RedirectToAction("Checkout");
        }

        #endregion

        #region Logo

        [HttpGet]
        public ActionResult LogoIndex()
        {
            var list = db.logoes.ToList();

            return View(list);
        }
        [HttpGet]
        public ActionResult LogoDelete(int id)
        {
            var list = db.logoes.Where(x => x.LogoId == id).FirstOrDefault();
            db.logoes.Remove(list);
            db.SaveChanges();
            TempData["DeleteBrand"] = "Message";
            return RedirectToAction("LogoIndex");

        }
        [HttpGet]
        public ActionResult LogoAdd()
        {


            return View();
        }
        [HttpPost]
        public ActionResult LogoAdd(logo logos, HttpPostedFileBase file)
        {
            try
            {



                Random rd = new Random();
                string extension1 = Path.GetExtension(file.FileName);
                if ((extension1 == ".png" || extension1 == ".jpg"))
                {
                    string sid = Session["StoreID"].ToString();
                    string _fileName = "files_" + rd.Next(100, 100000) + DateTime.Now.ToString("HHmmssddMMYYYY") + Path.GetExtension(file.FileName);
                    string _Path = Path.Combine(Server.MapPath("~/UserAsset/img/ProductImages"), _fileName);
                    file.SaveAs(_Path);
                    logos.Logo1 = _fileName;
                    logos.StoreID = Convert.ToInt32(sid);
                    db.logoes.Add(logos);
                    db.SaveChanges();
                    TempData["AddBrand"] = "message";
                    return RedirectToAction("LogoIndex");
                }
                else
                {
                    TempData["filetype"] = "error";
                    return RedirectToAction("LogoIndex");

                }

            }

            catch (Exception)
            {
                return RedirectToAction("Error");

            }

        }
        #endregion

    }
}