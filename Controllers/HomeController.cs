using KhoaLuanTotNghiep.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Excel = Microsoft.Office.Interop.Excel;

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
                    ViewBag.message = "Tên đăng nhập hoặc mật khẩu không chính xác, vui lòng thử lại !";
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
                CHUDE chude = new CHUDE("Yêu thích", null, "/Img/favourite/star.jpg", account.IDTK);
                model.CHUDEs.Add(chude);
                model.SaveChanges();
                return View("Home", accountInfo);
            }
            else
            {
                ViewBag.checkExist = "Tên tài khoản đã tồn tại, vui lòng chọn tên tài khoản khác!";
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

        public ActionResult Admin(string month, string year, string accountName)
        {
            List<TAIKHOAN> accounts  = model.TAIKHOANs.ToList();
            List<THONGTINTAIKHOAN> listaccount = new List<THONGTINTAIKHOAN>();
            if (accountName == null)
            {
                foreach (var item in accounts)
                {
                    if (month == null && year == null)
                    {
                        var accountinfo = model.THONGTINTAIKHOANs.Where(t => t.IDTK == item.IDTK).SingleOrDefault();
                        listaccount.Add(accountinfo);
                    }
                    else
                    {
                        int month1 = Convert.ToInt32(month);
                        int year1 = Convert.ToInt32(year);
                        if (month1 == 0 && year1 == 0)
                        {
                            var accountinfo = model.THONGTINTAIKHOANs.Where(t => t.IDTK == item.IDTK).SingleOrDefault();
                            listaccount.Add(accountinfo);
                        }
                        if (month1 != 0 && year1 == 0)
                        {
                            if (item.THOIGIANDANGKY.Value.Month == month1)
                            {
                                var accountinfo = model.THONGTINTAIKHOANs.Where(t => t.IDTK == item.IDTK).SingleOrDefault();
                                listaccount.Add(accountinfo);
                            }
                        }
                        if (month1 == 0 && year1 != 0)
                        {
                            if (item.THOIGIANDANGKY.Value.Year == year1)
                            {
                                var accountinfo = model.THONGTINTAIKHOANs.Where(t => t.IDTK == item.IDTK).SingleOrDefault();
                                listaccount.Add(accountinfo);
                            }
                        }
                        else
                        {

                            if (item.THOIGIANDANGKY.Value.Year == year1 && item.THOIGIANDANGKY.Value.Month == month1)
                            {
                                var accountinfo = model.THONGTINTAIKHOANs.Where(t => t.IDTK == item.IDTK).SingleOrDefault();
                                listaccount.Add(accountinfo);
                            }
                        }
                    }
            
            
            
                }
                
            }
            else
            {
                foreach (var item in accounts)
                {
                    if (month == null && year == null)
                    {
                        var accountinfo = model.THONGTINTAIKHOANs.Where(t => t.IDTK == item.IDTK).SingleOrDefault();
                        listaccount.Add(accountinfo);
                    }
                    else
                    {
                        int month1 = Convert.ToInt32(month);
                        int year1 = Convert.ToInt32(year);
                        if (month1 == 0 && year1 == 0)
                        {
                            var accountinfo = model.THONGTINTAIKHOANs.Where(t => t.IDTK == item.IDTK).SingleOrDefault();
                            listaccount.Add(accountinfo);
                        }
                        if (month1 != 0 && year1 == 0)
                        {
                            if (item.THOIGIANDANGKY.Value.Month == month1)
                            {
                                var accountinfo = model.THONGTINTAIKHOANs.Where(t => t.IDTK == item.IDTK).SingleOrDefault();
                                listaccount.Add(accountinfo);
                            }
                        }
                        if (month1 == 0 && year1 != 0)
                        {
                            if (item.THOIGIANDANGKY.Value.Year == year1)
                            {
                                var accountinfo = model.THONGTINTAIKHOANs.Where(t => t.IDTK == item.IDTK).SingleOrDefault();
                                listaccount.Add(accountinfo);
                            }
                        }
                        else
                        {

                            if (item.THOIGIANDANGKY.Value.Year == year1 && item.THOIGIANDANGKY.Value.Month == month1)
                            {
                                var accountinfo = model.THONGTINTAIKHOANs.Where(t => t.IDTK == item.IDTK).SingleOrDefault();
                                listaccount.Add(accountinfo);
                            }
                        }
                    }

                    listaccount = listaccount.Where(t => t.TEN.ToLower().Contains(accountName.ToLower())).ToList();

                }
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

        public ActionResult TopicAdmin( string searchTopic)
        {
            THONGTINADMIN admin = model.THONGTINADMINs.Where(t => t.ADMIN.USING == true).SingleOrDefault();
            List<CHUDE> listcd = new List<CHUDE>();
            if (searchTopic == null)
            {
                listcd = model.CHUDEs.Where(t=>t.IDTK==admin.IDTK).ToList();
            }
            else
            {
                listcd = model.CHUDEs.Where(t => t.IDTK == admin.IDTK).Where(t => t.TENCD.ToLower().Contains(searchTopic.ToLower())).ToList();
            }
            
            ViewData["listcd"] = listcd;
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
            PHONGLUYENTAP plt = model.PHONGLUYENTAPs.Where(t => t.IDPLT == id).SingleOrDefault();
            Tuple<List<TUVUNGPHONGLUYENTAP>, int, int> tuple = new Tuple<List<TUVUNGPHONGLUYENTAP>, int,int>(ListTv, id,Convert.ToInt32(plt.SOCAUHOI));
            ViewData["tuple"] = tuple;
            return View(admin);
        }

        public PartialViewResult InfoAccount(int data1)
        {
            var accountinfo = model.THONGTINTAIKHOANs.Where(t => t.IDTK == data1).SingleOrDefault();
            return PartialView(accountinfo);
        }

        public ActionResult StatisticAccount(string month,string year)
        {
            return RedirectToAction("Admin", new { month = month, year = year });
        }

        public ActionResult findAccount(string accountName)
        {
            return RedirectToAction("Admin", new { accountName= accountName});
        }

        public ActionResult SearchTopic(string searchTopic)
        {
            return RedirectToAction("TopicAdmin", new { searchTopic = searchTopic });
        }

        public ActionResult TopicAddAdmin(HttpPostedFileBase file,string topic)
        {
            CHUDE chude;
            ADMIN admin = model.ADMINs.Where(t => t.USING == true).SingleOrDefault();
            if(file == null)
            {
                chude = new CHUDE(topic, admin.IDTK, null,null);
                model.CHUDEs.Add(chude);
                model.SaveChanges();
            }
            else
            {
                string filename = Path.GetFileName(file.FileName);
                string extension = Path.GetExtension(file.FileName);
                string path = Path.Combine(Server.MapPath("/Img/"), filename);
                model.CHUDEs.Add(new CHUDE(topic,admin.IDTK,"/Img/" + filename,null));
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
            return RedirectToAction("TopicAdmin");
        }

        public ActionResult TopicEditAdmin(string topic, string topicID, HttpPostedFileBase file)
        {
            int IDTopic = Convert.ToInt32(topicID);
            CHUDE chude = model.CHUDEs.Where(t => t.IDCD == IDTopic).SingleOrDefault();
            chude.TENCD = topic;
            if (file != null)
            {
                string filename = Path.GetFileName(file.FileName);
                string extension = Path.GetExtension(file.FileName);
                string path = Path.Combine(Server.MapPath("/Img/"), filename);
                chude.ANHCD = "/Img/" + filename;
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
            return RedirectToAction("TopicAdmin");
        }

        public ActionResult deleteTopic(int TopicID)
        {
            CHUDE chude = model.CHUDEs.Where(t => t.IDCD == TopicID).SingleOrDefault();
            List<TUVUNGONTAP> listTV = model.TUVUNGONTAPs.Where(t => t.IDCD == chude.IDCD).ToList();
            if (listTV.Count() == 0)
            {
                model.CHUDEs.Remove(chude);
                model.SaveChanges();
            }
            else
            {
                foreach(var item in listTV)
                {
                    model.TUVUNGONTAPs.Remove(item);
                    model.SaveChanges();
                }
                model.CHUDEs.Remove(chude);
                model.SaveChanges();
            }
 
            return RedirectToAction("TopicAdmin");
        }

        public ActionResult SearchWord(string idcd,string searchWord)
        {
            int IDCD = Convert.ToInt32(idcd);
            return RedirectToAction("details", new { id = IDCD, searchWord = searchWord });
        }
        [HttpPost]
        public ActionResult EditVocab(HttpPostedFileBase file, string id, string name, string meaning, string example, string idCD)
        {
            var account = model.ADMINs.Where(t => t.USING == true).SingleOrDefault();
            var accountinfo = model.THONGTINADMINs.Where(t => t.IDTK == account.IDTK).SingleOrDefault();
            int ID = Int32.Parse(id);
            int IDCD = Int32.Parse(idCD);
            TUVUNGONTAP tuvung = model.TUVUNGONTAPs.Where(t => t.IDTV == ID).SingleOrDefault();
            tuvung.TENTV = name;
            tuvung.NGHIATV = meaning;
            tuvung.VDTV = example;
            if (file != null)
            {
                string filename = Path.GetFileName(file.FileName);
                string extension = Path.GetExtension(file.FileName);
                string path = Path.Combine(Server.MapPath("/Img/"), filename);
                tuvung.ANHTUVUNG = "/Img/" + filename;
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
            return RedirectToAction("details", new { id = IDCD });
        }

        public PartialViewResult Flashcard(int data1)
        {
            TUVUNGONTAP tuvung = model.TUVUNGONTAPs.Where(t => t.IDTV == data1).SingleOrDefault();
            return PartialView(tuvung);
        }

        public PartialViewResult FlashcardCreate(int data1)
        {
            CHUDE chude = model.CHUDEs.Where(t => t.IDCD == data1).SingleOrDefault();
            return PartialView(chude);
        }

        public ActionResult DeleteTV(int id, int idCD)
        {
            var account = model.ADMINs.Where(t => t.USING == true).SingleOrDefault();
            var accountinfo = model.THONGTINADMINs.Where(t => t.IDTK == account.IDTK).SingleOrDefault();
            TUVUNGONTAP tuvung = model.TUVUNGONTAPs.Where(t => t.IDTV == id).SingleOrDefault();
            CHUDE chude = model.CHUDEs.Where(t => t.IDCD == idCD).SingleOrDefault();
            model.TUVUNGONTAPs.Remove(tuvung);
            model.SaveChanges();
            return RedirectToAction("details", new { id = idCD });
        }

        public ActionResult CreateTV(HttpPostedFileBase file, string name, string meaning, string example, int idCD)
        {
            var account = model.ADMINs.Where(t => t.USING == true).SingleOrDefault();
            var accountinfo = model.THONGTINADMINs.Where(t => t.IDTK == account.IDTK).SingleOrDefault();
            //int IDCD = Int32.Parse(idCD);

            if (file != null)
            {
                string filename = Path.GetFileName(file.FileName);
                string extension = Path.GetExtension(file.FileName);
                string path = Path.Combine(Server.MapPath("/Img/"), filename);
                model.TUVUNGONTAPs.Add(new TUVUNGONTAP(name, meaning, null, example, "/Img/" + filename, idCD));
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
                model.TUVUNGONTAPs.Add(new TUVUNGONTAP(name, meaning, null, example, null, idCD));
                model.SaveChanges();
            }
            List<TUVUNGONTAP> listTV = model.TUVUNGONTAPs.Where(t => t.IDCD == idCD).ToList();
            return RedirectToAction("details", new { id = idCD });
        }


        public ActionResult addFile(HttpPostedFileBase excelfile, string topicID)
        {
            int idCD = Convert.ToInt32(topicID);
            if (excelfile == null || excelfile.ContentLength == 0)
            {
                return RedirectToAction("details", new { id = idCD });
            }
            else
            {
                if(excelfile.FileName.EndsWith("xls") || excelfile.FileName.EndsWith("xlsx"))
                {
                    string path = Server.MapPath("~/files/" + excelfile.FileName);
                    if (System.IO.File.Exists(path))
                        System.IO.File.Delete(path);
                    excelfile.SaveAs(path);
                    Excel.Application application = new Excel.Application();
                    Excel.Workbook workbook = application.Workbooks.Open(path);
                    Excel.Worksheet worksheet = workbook.ActiveSheet;
                    Excel.Range range = worksheet.UsedRange;
                    List<TUVUNGONTAP> listTV = new List<TUVUNGONTAP>();
                    for(int row = 2;row <= range.Rows.Count; row++)
                    {
                        TUVUNGONTAP tv = new TUVUNGONTAP();
                        tv.TENTV = ((Excel.Range)range.Cells[row, 1]).Text;
                        tv.NGHIATV = ((Excel.Range)range.Cells[row, 2]).Text;
                        tv.ANHTUVUNG = ((Excel.Range)range.Cells[row,3]).Text;
                        tv.IDCD = idCD;
                        model.TUVUNGONTAPs.Add(tv);
                        model.SaveChanges();
                    }
                    return RedirectToAction("details", new { id = idCD });
                }
                else
                {
                    return RedirectToAction("details", new { id = idCD });
                }
            }
            
        }


        public ActionResult PracticesCreate(string topic, int time)
        {
            ADMIN admin = model.ADMINs.Where(t => t.USING == true).SingleOrDefault();
            PHONGLUYENTAP plt = new PHONGLUYENTAP(topic, 0, time, admin.IDTK);
            model.PHONGLUYENTAPs.Add(plt);
            model.SaveChanges();
            return RedirectToAction("Practice");
        }

        public ActionResult DeleteExam(int id)
        {
            PHONGLUYENTAP plt = model.PHONGLUYENTAPs.Where(t => t.IDPLT == id).SingleOrDefault();
            List<TAIKHOANPHONGLUYENTAP> tkplt = model.TAIKHOANPHONGLUYENTAPs.Where(t => t.IDPLT == id).ToList();
            if(plt.TUVUNGPHONGLUYENTAPs.Count > 0)
            {
                foreach(var item in plt.TUVUNGPHONGLUYENTAPs.ToList())
                {
                    model.TUVUNGPHONGLUYENTAPs.Remove(item);
                    model.SaveChanges();
                }
                if(tkplt.Count() > 0)
                {
                    foreach(var item1 in tkplt)
                    {
                        model.TAIKHOANPHONGLUYENTAPs.Remove(item1);
                        model.SaveChanges();
                    }
                    model.PHONGLUYENTAPs.Remove(plt);
                    model.SaveChanges();
                }
                else
                {
                    model.PHONGLUYENTAPs.Remove(plt);
                    model.SaveChanges();
                }
            }
            else
            {
                if (tkplt.Count() > 0)
                {
                    foreach (var item1 in tkplt)
                    {
                        model.TAIKHOANPHONGLUYENTAPs.Remove(item1);
                        model.SaveChanges();
                    }
                    model.PHONGLUYENTAPs.Remove(plt);
                    model.SaveChanges();
                }
                else
                {
                    model.PHONGLUYENTAPs.Remove(plt);
                    model.SaveChanges();
                }
            }
            return RedirectToAction("Practice");
        }


        public ActionResult CreateVocabExam(string name,string meaning, HttpPostedFileBase file,string idplt)
        {

            int Id = Convert.ToInt32(idplt);
            PHONGLUYENTAP plt = model.PHONGLUYENTAPs.Where(t => t.IDPLT == Id).SingleOrDefault();
            if (file != null)
            {
                string filename = Path.GetFileName(file.FileName);
                string extension = Path.GetExtension(file.FileName);
                string path = Path.Combine(Server.MapPath("/Img/"), filename);
                model.TUVUNGPHONGLUYENTAPs.Add(new TUVUNGPHONGLUYENTAP(name, meaning, "/Img/" + filename, Id));
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
                model.TUVUNGPHONGLUYENTAPs.Add(new TUVUNGPHONGLUYENTAP(name, meaning, null, Id));
                model.SaveChanges();
            }
            if(plt.SOCAUHOI < plt.TUVUNGPHONGLUYENTAPs.Count)
            {
                plt.SOCAUHOI = plt.TUVUNGPHONGLUYENTAPs.Count();
                model.SaveChanges();
            }
            return RedirectToAction("FixExam",new { id = Id});
        }

        public ActionResult EditVocabExam(string name, string meaning, HttpPostedFileBase file, string idplt , string idtv)
        {

            int IDPLT = Convert.ToInt32(idplt);
            int IDTV = Convert.ToInt32(idtv);
            TUVUNGPHONGLUYENTAP tuvung = model.TUVUNGPHONGLUYENTAPs.Where(t => t.IDPLT == IDPLT).Where(t => t.IDTV == IDTV).SingleOrDefault();
            if (file != null)
            {
                string filename = Path.GetFileName(file.FileName);
                string extension = Path.GetExtension(file.FileName);
                string path = Path.Combine(Server.MapPath("/Img/"), filename);
                
                tuvung.ANHTUVUNG = "/Img/" + filename;
                tuvung.TENTV = name;
                tuvung.NGHIATV = meaning;
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
                tuvung.TENTV = name;
                tuvung.NGHIATV = meaning;
                model.SaveChanges();
            }
            return RedirectToAction("FixExam", new { id = IDPLT });
        }

        public ActionResult ModifyExam(int sotv,int thoigian,int idplt)
        {
            PHONGLUYENTAP plt = model.PHONGLUYENTAPs.Where(t => t.IDPLT == idplt).SingleOrDefault();
            plt.SOCAUHOI = sotv;
            plt.THOIGIAN = thoigian;
            model.SaveChanges();
            return RedirectToAction("FixExam", new { id = idplt });
        }



        public ActionResult addFileExam(HttpPostedFileBase excelfile, string pltID)
        {
            int idPLT = Convert.ToInt32(pltID);
            if (excelfile == null || excelfile.ContentLength == 0)
            {
                return RedirectToAction("FixExam", new { id = idPLT });
            }
            else
            {
                if (excelfile.FileName.EndsWith("xls") || excelfile.FileName.EndsWith("xlsx"))
                {
                    string path = Server.MapPath("~/files/" + excelfile.FileName);
                    if (System.IO.File.Exists(path))
                        System.IO.File.Delete(path);
                    excelfile.SaveAs(path);
                    Excel.Application application = new Excel.Application();
                    Excel.Workbook workbook = application.Workbooks.Open(path);
                    Excel.Worksheet worksheet = workbook.ActiveSheet;
                    Excel.Range range = worksheet.UsedRange;
                    List<TUVUNGPHONGLUYENTAP> listTV = new List<TUVUNGPHONGLUYENTAP>();
                    for (int row = 2; row <= range.Rows.Count; row++)
                    {
                        TUVUNGPHONGLUYENTAP tv = new TUVUNGPHONGLUYENTAP();
                        tv.TENTV = ((Excel.Range)range.Cells[row, 1]).Text;
                        tv.NGHIATV = ((Excel.Range)range.Cells[row, 2]).Text;
                        if(((Excel.Range)range.Cells[row, 3]).Text != "")
                        {
                            tv.ANHTUVUNG = ((Excel.Range)range.Cells[row, 3]).Text;
                        }
                        else
                        {
                            tv.ANHTUVUNG = null;
                        }
                        
                        tv.IDPLT = idPLT;
                        model.TUVUNGPHONGLUYENTAPs.Add(tv);
                        model.SaveChanges();
                        PHONGLUYENTAP plt = model.PHONGLUYENTAPs.Where(t => t.IDPLT == idPLT).SingleOrDefault();
                        plt.SOCAUHOI = model.TUVUNGPHONGLUYENTAPs.Where(t => t.IDPLT == idPLT).ToList().Count();
                        model.SaveChanges();

                    }
                    return RedirectToAction("FixExam", new { id = idPLT });
                }
                else
                {
                    return RedirectToAction("FixExam", new { id = idPLT });
                }
            }

        }

        public ActionResult deleteVocabExam(int id, int idplt)
        {
            TUVUNGPHONGLUYENTAP tvplt = model.TUVUNGPHONGLUYENTAPs.Where(t => t.IDTV == id).SingleOrDefault();
            model.TUVUNGPHONGLUYENTAPs.Remove(tvplt);
            model.SaveChanges();
            PHONGLUYENTAP plt = model.PHONGLUYENTAPs.Where(t => t.IDPLT == idplt).SingleOrDefault();
            plt.SOCAUHOI = model.TUVUNGPHONGLUYENTAPs.Where(t => t.IDPLT == idplt).ToList().Count();
            model.SaveChanges();
            return RedirectToAction("FixExam", new { id = idplt });
        }
    }
}