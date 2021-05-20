using KhoaLuanTotNghiep.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KhoaLuanTotNghiep.Controllers
{
    public class ExamController : Controller
    {
        // GET: Exam
        private Model1 model;
        public ExamController()
        {
            model = new Model1();
        }
        public ActionResult Index(string findExam)
        {
            TAIKHOAN account = model.TAIKHOANs.Where(t => t.USING == true).SingleOrDefault();
            THONGTINTAIKHOAN accountinfo = model.THONGTINTAIKHOANs.Where(t => t.IDTK == account.IDTK).SingleOrDefault();
            List<PHONGLUYENTAP> listpt = new List<PHONGLUYENTAP>();
            if (findExam == null)
            {
                listpt = model.PHONGLUYENTAPs.Where(t=>t.SOCAUHOI>0).ToList();
            }
            else
            {
                listpt = model.PHONGLUYENTAPs.Where(t => t.SOCAUHOI > 0).Where(t => t.TENPHONG.ToLower().Contains(findExam.ToLower())).ToList();
            }
            List<TAIKHOANPHONGLUYENTAP> listtkpt = model.TAIKHOANPHONGLUYENTAPs.ToList();

            Tuple<List<PHONGLUYENTAP>, List<TAIKHOANPHONGLUYENTAP>> tuple = new Tuple<List<PHONGLUYENTAP>, List<TAIKHOANPHONGLUYENTAP>>(listpt, listtkpt);
            ViewData["Tuple"] = tuple;
            return View(accountinfo);
        }


        public ActionResult ExamRoom(int id)
        {
            Random rdn = new Random();
            TAIKHOAN account = model.TAIKHOANs.Where(t => t.USING == true).SingleOrDefault();
            TAIKHOANPHONGLUYENTAP tkplt = new TAIKHOANPHONGLUYENTAP(account.IDTK, id,0);
            int checktt = model.TAIKHOANPHONGLUYENTAPs.Where(t => t.IDTK == tkplt.IDTK).Where(t => t.IDPLT == tkplt.IDPLT).ToList().Count();
            PHONGLUYENTAP pt = model.PHONGLUYENTAPs.Where(t => t.IDPLT == id).SingleOrDefault();
            int socauhoi = Convert.ToInt32(pt.SOCAUHOI);
            if (checktt == 0)
            {
                model.TAIKHOANPHONGLUYENTAPs.Add(tkplt);
                model.SaveChanges();
            }
            List<TAIKHOANPHONGLUYENTAP> listtkpt = model.TAIKHOANPHONGLUYENTAPs.Where(t => t.IDPLT == id).ToList();
            List<TUVUNGPHONGLUYENTAP> listtv = model.TUVUNGPHONGLUYENTAPs.Where(t => t.IDPLT == id).ToList();
            var listtv1 = listtv.OrderBy(t => rdn.Next()).ToList();        
            Tuple<List<TAIKHOANPHONGLUYENTAP>, List<TUVUNGPHONGLUYENTAP>,int,int> tuple = new Tuple<List<TAIKHOANPHONGLUYENTAP>, List<TUVUNGPHONGLUYENTAP>,int,int>(listtkpt, listtv1,socauhoi,id);
            ViewData["tuple"] = tuple;
            return View(account.THONGTINTAIKHOANs.SingleOrDefault());
        }

        public ActionResult SaveResult(int idp, int scd )
        {
            TAIKHOAN account = model.TAIKHOANs.Where(t => t.USING == true).SingleOrDefault();
            TAIKHOANPHONGLUYENTAP tkpt = model.TAIKHOANPHONGLUYENTAPs.Where(t => t.IDTK == account.IDTK).Where(t => t.IDPLT == idp).SingleOrDefault();
            if(tkpt.DIEMTHI < scd)
            {
                tkpt.DIEMTHI = scd;
            }
            model.SaveChanges();
            return RedirectToAction("ExamRoom", new { id = idp });
        }

        public ActionResult addWord(int idcd,string tuvung, int idphong)
        {
            TUVUNGPHONGLUYENTAP tvpt = model.TUVUNGPHONGLUYENTAPs.Where(t => t.TENTV.ToLower().Equals(tuvung.ToLower())).SingleOrDefault();
            TUVUNGONTAP tvot = new TUVUNGONTAP(tvpt.TENTV, tvpt.NGHIATV,null, tvpt.VDTV, tvpt.ANHTUVUNG, idcd);
            model.TUVUNGONTAPs.Add(tvot);
            model.SaveChanges();
            return RedirectToAction("ExamRoom", new { id = idphong });
        }


        public ActionResult FinishExam(string diem, string phonglt)
        {
            int chuyendiem = Convert.ToInt32(diem);
            int chuyenphonglt = Convert.ToInt32(phonglt);
            TAIKHOAN taikhoan = model.TAIKHOANs.Where(t => t.USING == true).SingleOrDefault();
            TAIKHOANPHONGLUYENTAP tkplt = model.TAIKHOANPHONGLUYENTAPs.Where(t => t.IDPLT == chuyenphonglt).Where(t => t.IDTK == taikhoan.IDTK).SingleOrDefault();
            if(chuyendiem > tkplt.DIEMTHI)
            {
                tkplt.DIEMTHI = chuyendiem;
            }
            model.SaveChanges();
            return RedirectToAction("ExamRoom", new { id = chuyenphonglt });
        }

        public ActionResult searchExam(string searchExam)
        {
            return RedirectToAction("Index",new { findExam  = searchExam});
        }
    }
}