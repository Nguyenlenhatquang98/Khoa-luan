using KhoaLuanTotNghiep.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KhoaLuanTotNghiep.Controllers
{
    public class HomeController : Controller
    {
        private Model1 model;
        public HomeController()
        {
            model = new Model1();
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Home()
        {
            TAIKHOAN account = model.TAIKHOANs.Where(t => t.USING == true).SingleOrDefault();
            if (account != null)
            {
                THONGTINTAIKHOAN accountInfo = model.THONGTINTAIKHOANs.Where(t => t.IDTK == account.IDTK).SingleOrDefault();
                return View(accountInfo);
            }
            else
            {
                ADMIN admin = model.ADMINs.Where(t => t.USING == true).SingleOrDefault();
                if(admin == null)
                {
                    THONGTINTAIKHOAN accountInfo = null;
                    return View(accountInfo);
                }
                else
                {
                    THONGTINADMIN admininfo = model.THONGTINADMINs.Where(t => t.IDTK == admin.IDTK).SingleOrDefault();
                    return RedirectToAction("Admin");
                }
                
            }
        }


        public ActionResult SignIn(string username, string password)
        {
            TAIKHOAN account = model.TAIKHOANs.Where(t => t.TENDN.ToLower().Equals(username.ToLower())).Where(t => t.MATKHAU.ToLower().Equals(password.ToLower())).SingleOrDefault();
            if (account != null)
            {
                THONGTINTAIKHOAN accountInfo = model.THONGTINTAIKHOANs.Where(t => t.IDTK == account.IDTK).SingleOrDefault();
                account.USING = true;
                model.SaveChanges();
                return View("Home", accountInfo);
            }
            else
            {
                ADMIN admin = model.ADMINs.Where(t => t.TENDN.ToLower().Equals(username.ToLower())).Where(t => t.MATKHAU.ToLower().Equals(password.ToLower())).SingleOrDefault();
                if(admin != null)
                {
                    THONGTINADMIN admininfo = model.THONGTINADMINs.Where(T => T.IDTK == admin.IDTK).SingleOrDefault();
                    admin.USING = true;
                    model.SaveChanges();
                    return RedirectToAction("Admin");
                }
                else
                {
                    ViewBag.message = "Username or password was wrong !";
                    THONGTINTAIKHOAN accountInfo = null;
                    return View("Home", accountInfo);
                }
            }
        }
        public ActionResult SignUp(string name,string username,string password,string day,string month, string year,string gender)
        {
            TAIKHOAN checkExist = model.TAIKHOANs.Where(t => t.TENDN.ToLower().Equals(username.ToLower())).SingleOrDefault();
            if (checkExist == null)
            {
                TAIKHOAN account = model.TAIKHOANs.Add(new TAIKHOAN(username, password,DateTime.Now ,true));
                DateTime ngaySinh = new DateTime(Int32.Parse(year), Int32.Parse(month), Int32.Parse(day));
                bool Gender;
                if (Int32.Parse(gender) == 1)
                {
                    Gender = true;
                }
                else
                {
                    Gender = false;
                }
                THONGTINTAIKHOAN accountInfo = model.THONGTINTAIKHOANs.Add(new THONGTINTAIKHOAN(name, ngaySinh, Gender, account.IDTK));
                model.SaveChanges();
                return View("Home", accountInfo);
            }
            else
            {
                ViewBag.checkExist = "Account was already exist . Please choose another username";
                THONGTINTAIKHOAN accountInfo = null;
                return View("Home", accountInfo);
            }
           
        }

        public ActionResult Logout()
        {
            TAIKHOAN account = model.TAIKHOANs.Where(t => t.USING == true).SingleOrDefault();
            if (account == null)
            {
                ADMIN admin = model.ADMINs.Where(t => t.USING == true).SingleOrDefault();
                admin.USING = false;
                model.SaveChanges();
            }
            else
            {
                account.USING = false;
                model.SaveChanges();
            }      
            return View("Home");
        }

        public ActionResult AdminInfo()
        {
            ADMIN admin = model.ADMINs.Where(t => t.USING == true).SingleOrDefault();
            THONGTINADMIN admininfo = model.THONGTINADMINs.Where(t => t.IDTK == admin.IDTK).SingleOrDefault();
            return View(admininfo);
        }

        public ActionResult AddImage(HttpPostedFileBase file, string id, string name, string password, string day, string month, string year, string gender, string email, string address, string phone)
        {
            int ID = Int32.Parse(id);
            THONGTINADMIN Admininfo = model.THONGTINADMINs.Where(t => t.IDTTTK == ID).SingleOrDefault();
            Admininfo.TEN = name;
            DateTime ngaySinh = new DateTime(Int32.Parse(year), Int32.Parse(month), Int32.Parse(day));
            Admininfo.NGAYSINH = ngaySinh;
            bool Gender;
            if (Int32.Parse(gender) == 1)
            {
                Gender = true;
            }
            else
            {
                Gender = false;
            }
            Admininfo.GIOITINH = Gender;
            Admininfo.EMAIL = email;
            Admininfo.DIACHI = address;
            Admininfo.SODT = phone;
            Admininfo.ADMIN.MATKHAU = password;
            if (file != null)
            {
                string filename = Path.GetFileName(file.FileName);
                string extension = Path.GetExtension(file.FileName);
                string path = Path.Combine(Server.MapPath("~/Img/"), filename);
                Admininfo.ANHACC = "~/Img/" + filename;
                if (extension.ToLower() == ".jpg" || extension.ToLower() == ".jpeg" || extension.ToLower() == ".png")
                {
                    if (file.ContentLength <= 1000000)
                    {

                        if (model.SaveChanges() > 0)
                        {
                            file.SaveAs(path);
                        }

                    }
                }
            }
            else
            {
                model.SaveChanges();
            }

            return View("AdminInfo",Admininfo);
        }

        public ActionResult Admin()
        {
            List<TAIKHOAN> accounts = model.TAIKHOANs.ToList();
            List<THONGTINTAIKHOAN> listaccount = new List<THONGTINTAIKHOAN>();
            foreach (var item in accounts)
            {
                var accountinfo = model.THONGTINTAIKHOANs.Where(t => t.IDTK == item.IDTK).SingleOrDefault();
                listaccount.Add(accountinfo);
            }
            return View(listaccount);
        }

        public ActionResult Practice()
        {
            List<PHONGLUYENTAP> listphongthi = model.PHONGLUYENTAPs.ToList();
            List<TAIKHOANPHONGLUYENTAP> tkpt = model.TAIKHOANPHONGLUYENTAPs.ToList();
            ViewData["tkpt"] = tkpt;
            return View(listphongthi) ;
        }

        public ActionResult TopicAdmin()
        {
            THONGTINADMIN admin = model.THONGTINADMINs.Where(t => t.ADMIN.USING == true).SingleOrDefault();
            List<CHUDE> listcd = model.CHUDEs.ToList();
            List<TUVUNGONTAP> listTV = model.TUVUNGONTAPs.ToList();
            Tuple<List<CHUDE>, List<TUVUNGONTAP>> tuple = new Tuple<List<CHUDE>, List<TUVUNGONTAP>>(listcd, listTV);
            ViewData["tuple"] = tuple;
            return View(admin);
        }

        public ActionResult details(int id, string searchWord)
        {
            ADMIN account = model.ADMINs.Where(t => t.USING == true).SingleOrDefault();
            THONGTINADMIN accountInfo = model.THONGTINADMINs.Where(t => t.IDTK == account.IDTK).SingleOrDefault(); ;
            

            List<TUVUNGONTAP> listTV = new List<TUVUNGONTAP>();
            if (searchWord == null)
            {
                listTV = model.TUVUNGONTAPs.Where(t => t.IDCD == id).ToList();
            }
            else
            {
                listTV = model.TUVUNGONTAPs.Where(t => t.IDCD == id).Where(t => t.TENTV.ToLower().Contains(searchWord.ToLower())).ToList();
            }
            Tuple<List<TUVUNGONTAP>, int> tuple = new Tuple<List<TUVUNGONTAP>, int>(listTV, id);
            ViewData["tuple"] = tuple;

            return View(accountInfo);
        }

        public ActionResult FixExam(int id)
        {
            THONGTINADMIN admin = model.THONGTINADMINs.Where(t => t.ADMIN.USING == true).SingleOrDefault();
            List<TUVUNGPHONGLUYENTAP> ListTv = model.TUVUNGPHONGLUYENTAPs.Where(t => t.IDPLT == id).ToList();
            Tuple<List<TUVUNGPHONGLUYENTAP>, int> tuple = new Tuple<List<TUVUNGPHONGLUYENTAP>, int>(ListTv, id);
            ViewData["tuple"] = tuple;
            return View(admin);
        }
    }
}