using KhoaLuanTotNghiep.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Speech;
using System.Speech.Synthesis;
namespace KhoaLuanTotNghiep.Controllers
{
    public class VocabularyController : Controller
    {
        // GET: Vocabulary
        private Model1 model;
        SpeechSynthesizer reader = new SpeechSynthesizer();
        public VocabularyController()
        {
            model = new Model1();
           
        }
        public ActionResult Index(string id)
        {

            TAIKHOAN account = model.TAIKHOANs.Where(t => t.USING == true).SingleOrDefault();
            THONGTINTAIKHOAN accountInfo = null;
            List<CHUDE> listChude = new List<CHUDE>();
            if (id == null)
            {
              listChude  = model.CHUDEs.ToList();
            }
            else
            {
                listChude = model.CHUDEs.Where(t => t.TENCD.ToLower().Contains(id.ToLower())).ToList();
            }
            //List<TUVUNGONTAP> tUVUNGONTAPs = model.TUVUNGONTAPs.Join(model.CHUDEs, tv => tv.IDCD, cd => cd.IDCD, (tv, cd) => new { tv, cd }).Where(t => t.cd.IDTK == account.IDTK).Select(t => t.tv).ToList();
            List<TUVUNGONTAP> listTv = new List<TUVUNGONTAP>();
            foreach(var item in listChude)
            {
                var tvcd = model.TUVUNGONTAPs.Where(t => t.IDCD == item.IDCD).ToList();
                foreach(var item1 in tvcd)
                {
                    listTv.Add(item1);
                }
            }
            if (account != null)
            {
                accountInfo = model.THONGTINTAIKHOANs.Where(t => t.IDTK == account.IDTK).SingleOrDefault();
            }
            Tuple<List<CHUDE>, List<TUVUNGONTAP>> tuple = new Tuple<List<CHUDE>, List<TUVUNGONTAP>>(listChude, listTv);
            ViewData["tuple"] = tuple;
            return View(accountInfo);

        }

        public ActionResult Topic(string topic)
        {
            TAIKHOAN account = model.TAIKHOANs.Where(t => t.USING == true).SingleOrDefault();
            model.CHUDEs.Add(new CHUDE(topic, account.IDTK));
            model.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult deleteTopic(int id)
        {
            CHUDE chude = model.CHUDEs.Where(t => t.IDCD == id).SingleOrDefault();
            List<TUVUNGONTAP> listTV = model.TUVUNGONTAPs.Where(t => t.IDCD == id).ToList();
            if(listTV.Count > 0)
            {
                foreach(var item in listTV)
                {
                    model.TUVUNGONTAPs.Remove(item);
                    model.SaveChanges();
                }
            }
            model.CHUDEs.Remove(chude);
            model.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult details(int id,string searchWord)
        {
            TAIKHOAN account = model.TAIKHOANs.Where(t => t.USING == true).SingleOrDefault();
            THONGTINTAIKHOAN accountInfo = null;
            if (account != null)
            {
                accountInfo = model.THONGTINTAIKHOANs.Where(t => t.IDTK == account.IDTK).SingleOrDefault();
            }
          
            List<TUVUNGONTAP> listTV = new List<TUVUNGONTAP>();
            if(searchWord == null)
            {
                listTV = model.TUVUNGONTAPs.Where(t => t.IDCD == id).ToList();
            }
            else
            {
                listTV = model.TUVUNGONTAPs.Where(t => t.IDCD == id).Where(t=>t.TENTV.ToLower().Contains(searchWord.ToLower())).ToList();
            }
            
            ViewData["IDCD"] = id;
            ViewData["ListTV"] = listTV;
            return View(accountInfo);
        }
        public PartialViewResult Flashcard(int data1)
        {
            TUVUNGONTAP tuvung = model.TUVUNGONTAPs.Where(t => t.IDTV == data1).SingleOrDefault();
            return PartialView(tuvung);
        }
       

        [HttpPost]
        public ActionResult EditVocab(HttpPostedFileBase file,string id,string name, string meaning, string example,string idCD)
        {
            TAIKHOAN account = model.TAIKHOANs.Where(t => t.USING == true).SingleOrDefault();
            THONGTINTAIKHOAN accountinfo = model.THONGTINTAIKHOANs.Where(t => t.IDTK == account.IDTK).SingleOrDefault();
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
        public ActionResult DeleteTV(int id,int idCD)
        {
            TAIKHOAN account = model.TAIKHOANs.Where(t => t.USING == true).SingleOrDefault();
            THONGTINTAIKHOAN accountinfo = model.THONGTINTAIKHOANs.Where(t => t.IDTK == account.IDTK).SingleOrDefault();
            TUVUNGONTAP tuvung = model.TUVUNGONTAPs.Where(t => t.IDTV == id).SingleOrDefault();
            CHUDE chude = model.CHUDEs.Where(t => t.IDCD == idCD).SingleOrDefault();
            model.TUVUNGONTAPs.Remove(tuvung);
            model.SaveChanges();
            return RedirectToAction("details",new {id = idCD });
        }
        public PartialViewResult FlashcardCreate(int data1)
        {
            CHUDE chude = model.CHUDEs.Where(t => t.IDCD == data1).SingleOrDefault();
            return PartialView(chude);
        }

        public ActionResult CreateTV(HttpPostedFileBase file, string name, string meaning, string example, int idCD)
        {
            TAIKHOAN account = model.TAIKHOANs.Where(t => t.USING == true).SingleOrDefault();
            THONGTINTAIKHOAN accountinfo = model.THONGTINTAIKHOANs.Where(t => t.IDTK == account.IDTK).SingleOrDefault();
            //int IDCD = Int32.Parse(idCD);

            if (file != null)
            {
                string filename = Path.GetFileName(file.FileName);
                string extension = Path.GetExtension(file.FileName);
                string path = Path.Combine(Server.MapPath("/Img/"), filename);
                model.TUVUNGONTAPs.Add(new TUVUNGONTAP(name, meaning, null,example,"/Img/" + filename, idCD));
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
                model.TUVUNGONTAPs.Add(new TUVUNGONTAP(name, meaning, null, example ,null, idCD));
                model.SaveChanges();
            }
            List<TUVUNGONTAP> listTV = model.TUVUNGONTAPs.Where(t => t.IDCD == idCD).ToList();
            return RedirectToAction("details", new { id = idCD });
        }

        public ActionResult SearchTopic(string searchTopic)
        {
            return RedirectToAction("Index", new { id = searchTopic });
        }

        public ActionResult SearchWord(string searchWord,int idcd)
        {

            return RedirectToAction("details", new { searchWord = searchWord, id = idcd });
        }

        public ActionResult TopicEdit(string topic , string topicID)
        {
            int TopicID = Int32.Parse(topicID);
            CHUDE chude = model.CHUDEs.Where(t => t.IDCD == TopicID).SingleOrDefault();
            chude.TENCD = topic;
            model.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Practice(int id)
        {
            TAIKHOAN account = model.TAIKHOANs.Where(t => t.USING == true).SingleOrDefault();
            THONGTINTAIKHOAN accountinfo = model.THONGTINTAIKHOANs.Where(t => t.IDTK == account.IDTK).SingleOrDefault();
            ViewData["id"] = id;
            return View(accountinfo);
        }

        public ActionResult match(int id)
        {
            var rnd = new Random();

            TAIKHOAN account = model.TAIKHOANs.Where(t => t.USING == true).SingleOrDefault();
            THONGTINTAIKHOAN accountinfo = model.THONGTINTAIKHOANs.Where(t => t.IDTK == account.IDTK).SingleOrDefault();
            List<TUVUNGONTAP> listTV = model.TUVUNGONTAPs.Where(t => t.CHUDE.IDCD == id).ToList();
            List<TUVUNGONTAP> listTV1 = new List<TUVUNGONTAP>();
            if (listTV.Count > 7)
            {
                 listTV1 = listTV.OrderBy(t => rnd.Next()).Take(7).ToList();
            }
            else
            {
                listTV1 = listTV.OrderBy(t => rnd.Next()).ToList();
            }
            List<TUVUNGONTAP> listTV2 = listTV1.OrderBy(t => rnd.Next()).ToList();
           

            Tuple<List<TUVUNGONTAP>, List<TUVUNGONTAP>> tuple = new Tuple<List<TUVUNGONTAP>, List<TUVUNGONTAP>>(listTV1, listTV2);
            ViewData["tuple"] = tuple;
            return View(accountinfo);
        }

        public ActionResult ShareTV(int idnhom,int idchude, int idtuvung)
        {
            TUVUNGONTAP tvot = model.TUVUNGONTAPs.Where(t => t.IDTV == idtuvung).SingleOrDefault();
            TUVUNGNHOM tvn = new TUVUNGNHOM(tvot.TENTV, tvot.NGHIATV, tvot.VDTV, tvot.ANHTUVUNG, idnhom);
            int checktt = model.TUVUNGNHOMs.Where(t=>t.IDN == idnhom).Where(t => t.TENTV.ToLower().Equals(tvn.TENTV.ToLower())).ToList().Count();
            if(checktt == 0)
            {
                model.TUVUNGNHOMs.Add(tvn);
                model.SaveChanges();
            }

            return RedirectToAction("details",idchude);
        }
        public ActionResult wordCorrect(int id)
        {
            TAIKHOAN account = model.TAIKHOANs.Where(t => t.USING == true).SingleOrDefault();
            THONGTINTAIKHOAN accountinfo = model.THONGTINTAIKHOANs.Where(t => t.IDTK == account.IDTK).SingleOrDefault();
            List<TUVUNGONTAP> listtv = model.TUVUNGONTAPs.Where(t => t.IDCD == id).ToList();
            Tuple<List<TUVUNGONTAP>, int> tuple = new Tuple<List<TUVUNGONTAP>, int>(listtv, id);
            ViewData["tuple"] = tuple;
            return View(accountinfo);
        }

        public ActionResult learnCard(int id)
        {
            List<TUVUNGONTAP> listTuVung = model.TUVUNGONTAPs.Where(t => t.IDCD == id).ToList();
            THONGTINTAIKHOAN accountinfo = model.THONGTINTAIKHOANs.Where(t => t.TAIKHOAN.USING == true).SingleOrDefault();
            Tuple<List<TUVUNGONTAP>, int> tuple = new Tuple<List<TUVUNGONTAP>, int>(listTuVung, id);
            ViewData["tuple"] = tuple;
            
            return View(accountinfo);
        }

        public ActionResult addWord(int idtv , int idcd)
        {
            TUVUNGONTAP tuvung = model.TUVUNGONTAPs.Where(t => t.IDTV == idtv).SingleOrDefault();
            TUVUNGONTAP tuvungadd = new TUVUNGONTAP(tuvung.TENTV, tuvung.NGHIATV, tuvung.PHIENAM, tuvung.VDTV, tuvung.ANHTUVUNG, 2);
            model.TUVUNGONTAPs.Add(tuvungadd);
            model.SaveChanges();
            return RedirectToAction("learnCard", new { id = idcd });
        }

        public ActionResult removeWord(int idtv, int idcd)
        {
            TUVUNGONTAP tuvung = model.TUVUNGONTAPs.Where(t => t.IDTV == idtv).SingleOrDefault();
            TUVUNGONTAP tuvungremove = model.TUVUNGONTAPs.Where(t => t.IDCD == 2).Where(t => t.TENTV.ToLower().Equals(tuvung.TENTV.ToLower())).SingleOrDefault();
            model.TUVUNGONTAPs.Remove(tuvungremove);
            model.SaveChanges();
            if (idcd != 2)
            {
                return RedirectToAction("learnCard", new { id = idcd });
            }
            else
            {
                return RedirectToAction("index");
            }
            
        }

        public ActionResult pictureMeaning(int id)
        {
            TAIKHOAN account = model.TAIKHOANs.Where(t => t.USING == true).SingleOrDefault();
            THONGTINTAIKHOAN accountinfo = model.THONGTINTAIKHOANs.Where(t => t.IDTK == account.IDTK).SingleOrDefault();
            List<TUVUNGONTAP> listtv = model.TUVUNGONTAPs.Where(t => t.IDCD == id).ToList();
            Tuple<List<TUVUNGONTAP>, int> tuple = new Tuple<List<TUVUNGONTAP>, int>(listtv, id);
            ViewData["tuple"] = tuple;
            return View(accountinfo);
        }


    }
}