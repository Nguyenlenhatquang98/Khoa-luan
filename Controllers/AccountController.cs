using KhoaLuanTotNghiep.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KhoaLuanTotNghiep.Controllers
{
    public class AccountController : Controller
    {
        private Model1 model;
        public AccountController()
        {
            model = new Model1();
        }
        
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AccountInfo(string id)
        {
            var idtk = Int32.Parse(id);
            THONGTINTAIKHOAN accountinfo = model.THONGTINTAIKHOANs.Where(t => t.IDTK == idtk).SingleOrDefault();
            return View(accountinfo);
        }

        [HttpPost]
        public ActionResult AddImage(HttpPostedFileBase file,string id,string name,string password,string day,string month,string year,string gender,string email,string address,string phone)
        {
            int ID = Int32.Parse(id);
            THONGTINTAIKHOAN accountInfo = model.THONGTINTAIKHOANs.Where(t => t.IDTTTK == ID).SingleOrDefault();
            accountInfo.TEN = name;
            DateTime ngaySinh = new DateTime(Int32.Parse(year), Int32.Parse(month), Int32.Parse(day));
            accountInfo.NGAYSINH = ngaySinh;
            bool Gender;
            if (Int32.Parse(gender) == 1)
            {
                Gender = true;
            }
            else
            {
                Gender = false;
            }
            accountInfo.GIOITINH = Gender;
            accountInfo.EMAIL = email;
            accountInfo.DIACHI = address;
            accountInfo.SODT = phone;
            accountInfo.TAIKHOAN.MATKHAU = password;
            if (file != null)
            {
                string filename = Path.GetFileName(file.FileName);
                string extension = Path.GetExtension(file.FileName);
                string path = Path.Combine(Server.MapPath("~/Img/"), filename);
                accountInfo.ANHACC = "~/Img/" + filename;
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

            
           
            return View("AccountInfo",accountInfo);
        }
        public ActionResult Group(string searchGroup)
        {
            TAIKHOAN account = model.TAIKHOANs.Where(t => t.USING == true).SingleOrDefault();
            THONGTINTAIKHOAN accountinfo = model.THONGTINTAIKHOANs.Where(t => t.IDTK == account.IDTK).SingleOrDefault();
            List<NHOM> listnhom = new List<NHOM>();
            List<NHOMTAIKHOAN> listnhomaccount = new List<NHOMTAIKHOAN>();
            List<TUVUNGNHOM> listtvnhom = new List<TUVUNGNHOM>();
            var nhomtaikhoan = model.NHOMTAIKHOANs.Select(t => new { t.IDTK, t.IDN }).Where(t=>t.IDTK == account.IDTK).Distinct().ToList();
            
            for(int i = 0; i < nhomtaikhoan.Count(); i++)
            {
                var idNhom = nhomtaikhoan[i].IDN;
                
                NHOM nhom = new NHOM();
                if (searchGroup == null)
                {
                    nhom = model.NHOMs.Where(t => t.IDN == idNhom).SingleOrDefault();
                }
                else
                {
                    nhom = model.NHOMs.Where(t => t.IDN == idNhom).Where(t => t.TENNHOM.ToLower().Contains(searchGroup.ToLower())).SingleOrDefault();
                }
                if(nhom != null)
                {
                    var listtvn = model.TUVUNGNHOMs.Where(t => t.IDN == nhom.IDN).ToList();
                    foreach(var item in listtvn)
                    {
                        listtvnhom.Add(item);
                    }
                    listnhom.Add(nhom);
                }
                
                var nhomtaikhoan2 = model.NHOMTAIKHOANs.Select(t => new { t.IDTK, t.IDN }).Where(t => t.IDN == idNhom).Distinct().ToList();
                for(int j = 0; j < nhomtaikhoan2.Count(); j++)
                {
                    var idTK = nhomtaikhoan2[j].IDTK;
                    NHOMTAIKHOAN nhomaccount1 = model.NHOMTAIKHOANs.Where(t => t.IDTK == idTK).Where(t=>t.IDN==idNhom).SingleOrDefault();
                    listnhomaccount.Add(nhomaccount1);
                }
            }
            Tuple<List<NHOM>, List<NHOMTAIKHOAN>,List<TUVUNGNHOM>> tuple = new Tuple<List<NHOM>, List<NHOMTAIKHOAN>,List<TUVUNGNHOM>>(listnhom, listnhomaccount,listtvnhom);
            ViewData["tuple"] = tuple;
            return View(accountinfo);
        }

        public ActionResult CreateGroup(string group)
        {
            TAIKHOAN account = model.TAIKHOANs.Where(t => t.USING == true).SingleOrDefault();
            model.NHOMs.Add(new NHOM(group));
            model.SaveChanges();
            NHOM nhom = model.NHOMs.Where(t => t.TENNHOM.ToLower().Equals(group.ToLower())).SingleOrDefault();
            model.NHOMTAIKHOANs.Add(new NHOMTAIKHOAN(nhom.IDN, account.IDTK));
            model.SaveChanges();

            return RedirectToAction("Group");
        }

        public  ActionResult GroupDetails(int id,string searchWord)
        {
            TAIKHOAN account = model.TAIKHOANs.Where(t => t.USING == true).SingleOrDefault();
            THONGTINTAIKHOAN accountinfo = model.THONGTINTAIKHOANs.Where(t => t.IDTK == account.IDTK).SingleOrDefault();
            List<TUVUNGNHOM> listVocab = new List<TUVUNGNHOM>();
            if (searchWord == null)
            {
                listVocab = model.TUVUNGNHOMs.Where(t => t.IDN == id).ToList();
            }
            else
            {
                listVocab = model.TUVUNGNHOMs.Where(t => t.IDN == id).Where(t=>t.TENTV.ToLower().Contains(searchWord.ToLower())).ToList();
            }
            List<THONGTINTAIKHOAN> accountInfos = new List<THONGTINTAIKHOAN>();
            List<NHOMTAIKHOAN> nhomtaikhoan = model.NHOMTAIKHOANs.Where(t => t.IDN == id).ToList();
            Tuple<List<TUVUNGNHOM>, List<THONGTINTAIKHOAN>,int> tuple = new Tuple<List<TUVUNGNHOM>, List<THONGTINTAIKHOAN>,int>(listVocab, accountInfos,id);
            foreach(var std in nhomtaikhoan)
            {
                TAIKHOAN account1 = model.TAIKHOANs.Where(t => t.IDTK == std.IDTK).SingleOrDefault();
                THONGTINTAIKHOAN accountinfo1 = model.THONGTINTAIKHOANs.Where(t => t.IDTK == account1.IDTK).SingleOrDefault();
                accountInfos.Add(accountinfo1);
            }
            ViewData["tuple"] = tuple;
            return View(accountinfo);
        }

        public ActionResult AddAccountToGroup(string idTK,string idNhom)
        {
            TAIKHOAN account = model.TAIKHOANs.Where(t => t.USING == true).SingleOrDefault();
            THONGTINTAIKHOAN accountinfo = model.THONGTINTAIKHOANs.Where(t => t.IDTK == account.IDTK).SingleOrDefault();
            int IDTK = Int32.Parse(idTK);
            int IDNHOM = Int32.Parse(idNhom);
            int checktt = model.NHOMTAIKHOANs.Where(t => t.IDTK == IDTK).Where(t => t.IDN == IDNHOM).ToList().Count();
            if(checktt > 0)
            {
                ViewBag.Message = "Thành viên đã tồn tại trong nhóm";
            }
            else
            {
                int checktt2 = model.TAIKHOANs.Where(t => t.IDTK == IDTK).ToList().Count();
                if(checktt2 > 0)
                {
                    model.NHOMTAIKHOANs.Add(new NHOMTAIKHOAN(IDNHOM, IDTK));
                    model.SaveChanges();
                }
                else
                {
                    ViewBag.Message = "Thành viên không tồn tại ";
                }
                
            }
            return RedirectToAction("GroupDetails", new { id = IDNHOM });
        }


        public ActionResult OutGroup(int idnhom)
        {
            TAIKHOAN account = model.TAIKHOANs.Where(t => t.USING == true).SingleOrDefault();
            NHOMTAIKHOAN ntk = model.NHOMTAIKHOANs.Where(t => t.IDN == idnhom).Where(t => t.IDTK == account.IDTK).SingleOrDefault();
            model.NHOMTAIKHOANs.Remove(ntk);
            model.SaveChanges();
            int sluong = model.NHOMTAIKHOANs.Where(t => t.IDN == idnhom).ToList().Count();
            if(sluong == 0)
            {
                NHOM nhom = model.NHOMs.Where(t => t.IDN == idnhom).SingleOrDefault();
                List<TUVUNGNHOM> checktv = model.TUVUNGNHOMs.Where(t => t.IDN == nhom.IDN).ToList();
                if (checktv.Count() == 0)
                {
                    model.NHOMs.Remove(nhom);
                    model.SaveChanges();
                }
                else
                {
                    foreach(var item in checktv)
                    {
                        model.TUVUNGNHOMs.Remove(item);
                        model.SaveChanges();
                    }
                    model.NHOMs.Remove(nhom);
                    model.SaveChanges();
                }
            }
            return RedirectToAction("Group");
        }

        public ActionResult GetTV(int idtv1 , int idnhom1 ,int idchude1)
        {
            TUVUNGNHOM tv = model.TUVUNGNHOMs.Where(t => t.IDTV == idtv1).SingleOrDefault();
            TUVUNGONTAP tvot = new TUVUNGONTAP(tv.TENTV, tv.NGHIATV,null , tv.VDTV, tv.ANHTUVUNG, idchude1);
            int chectt = model.TUVUNGONTAPs.Where(t=>t.IDCD==idchude1).Where(t => t.TENTV.ToLower().Equals(tvot.TENTV.ToLower())).ToList().Count();
            if (chectt == 0)
            {
                model.TUVUNGONTAPs.Add(tvot);
                model.SaveChanges();
            }

            return RedirectToAction("GroupDetails", new { id = idnhom1 });
        }

        public ActionResult DeleteTV(string idtv1, string idnhom1)
        {
            int IDTV = Int32.Parse(idtv1);
            int IDN = Int32.Parse(idnhom1);
            TUVUNGNHOM tv = model.TUVUNGNHOMs.Where(t => t.IDTV == IDTV).SingleOrDefault();
            model.TUVUNGNHOMs.Remove(tv);
            model.SaveChanges();
            return RedirectToAction("GroupDetails", new { id = IDN });
        }

        public ActionResult SearchWord(string searchWord1, int idn)
        {
            return RedirectToAction("GroupDetails", new { id = idn, searchWord = searchWord1 });
        }


        public ActionResult searchGroup(string searchGroup1)
        {
            return RedirectToAction("Group", new { searchGroup = searchGroup1 });
        }

        public ActionResult learnCard(int id)
        {
            return View(id);

        }

    }
}