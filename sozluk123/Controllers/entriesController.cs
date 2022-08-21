using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using sozluk123.Models;

namespace sozluk123.Controllers
{
    public class EntriesController : Controller
    {
        static class Butter
        {
            public static Guid counter;

            public static Guid entryval;

            public static Guid entryid1;
        }

        public static List<string> ttr = new List<string>();

        public static Dictionary<string, List<string>> dict = new Dictionary<string, List<string>>();

        private readonly sozluk1Entities1 db = new sozluk1Entities1();
        
        public ActionResult Index123() 
        {
                return View();
        }
        public ActionResult SetValue(Guid id)
        {
                Butter.entryval = id;
                return View();
        }
        public JsonResult AddComment(entry_yazar ent)
        {
            string status = "success";
            try
            {
                var yazarid = (from u in db.yazar from i in db.Users where ((u.yazar_ismi == i.Username) && (u.yazar_ismi == User.Identity.Name)) select u.ID).FirstOrDefault();
                ent.kayit_tarih = DateTime.Now;
                ent.ID = Guid.NewGuid();
                ent.yazar_ID = yazarid;
                ent.entry_ID = Butter.entryval;
                db.entry_yazar.Add(ent);
                db.SaveChanges();
                EntriesHub.BroadcastData();
            }
            catch (Exception ex)
            {
                status = ex.Message;
            }
           
            return Json(status, JsonRequestBehavior.AllowGet);
        }
        public JsonResult AddTeacher1(yazar teachers)
        {
            string status = "success";
            try
            {
                teachers.kayit_tarih = null;
                teachers.ID = Guid.NewGuid();
                db.yazar.Add(teachers);
                db.SaveChanges();
                Butter.counter = teachers.ID;
                EntriesHub.BroadcastData();
            }
            catch (Exception ex)
            {
                status = ex.Message;
            }

            return Json(status, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Updateauthordetails(yazar teachers)
        {
            try
            {
                teachers.ID = Butter.counter;
                db.yazar.Attach(teachers);
                db.Entry(teachers).State = EntityState.Modified;
                db.SaveChanges();
                EntriesHub.BroadcastData();
            }
            catch (Exception ex)
            {
                _ = ex.Message;
            }
           
            return Json(teachers, JsonRequestBehavior.AllowGet);
        }
        public ActionResult LoadaddTeacherPopup1()
        {
            try
            {
                return PartialView("_AddDataFirstPage");
            }
            catch (Exception)
            {

                throw;
            }
            
        }
        public ActionResult LoadEditTeacherPopup1(Guid? TeacherId)
        {
            try
            {
                var model = db.yazar.Where(a => a.ID == TeacherId).FirstOrDefault();
                return PartialView("_UpdateData12345", model);
            }
            catch (Exception )
            {
                return PartialView("_UpdateData12345");
            }
        }

        public ActionResult Index1()
        {
            return View();
        }
        
        [HttpGet]
        public JsonResult GetAllTeachers()
        {
            
            db.Configuration.ProxyCreationEnabled = false;

            var q = (from pd in db.entry
                     join od in db.yazar on pd.yazar_id equals od.ID
                     
                     orderby pd.kayit_tarih descending
                     select new
                     {
                         pd.ID,
                         pd.entry_ismi,
                         pd.entry_icerik,
                         pd.yazar.yazar_ismi,
                         pd.kayit_tarih,
                         pd.active,
                     }).ToList();

            return Json(q, JsonRequestBehavior.AllowGet);
        }
       
        public ActionResult LoadEditTeacherPopup(Guid? TeacherId)
        {
            try
            {
                var model = db.entry.Where(a => a.ID == TeacherId).FirstOrDefault();
                return PartialView("_UpdateData12345", model);
            }
            catch (Exception )
            {
                return PartialView("_UpdateData12345");
            }
        }
        
        public ActionResult LoadEditTeacherPopup15(Guid? TeacherId)
        {
            try
            {
                var model = db.entry_yazar.Where(a => a.ID == TeacherId).FirstOrDefault();
                return PartialView("_UpdateData12345rest", model);
            }
            catch (Exception )
            {
                return PartialView("_UpdateData12345rest");
            }
        }

       
        public ActionResult LoadaddTeacherPopup()
        {
            try
            {
                ViewBag.yazar_id = new SelectList(db.yazar, "ID", "yazar_ismi");
                return PartialView("_AddData");
            }
            catch (Exception )
            {
                return PartialView("_AddData");
            }
        }
        
        public ActionResult LoadaddTeacherPopup1234()
        {
            try
            {
                return PartialView("_AddData1234");
            }
            catch (Exception )
            {
                return PartialView("_AddData1234");
            }
        }

        public ActionResult LoadaddTeacherPopupComment1(Guid TeacherId)
        {
            Butter.entryid1 = TeacherId;
            try
            {
                return PartialView("_AddDataComment");
            }
            catch (Exception)
            {
                return PartialView("_AddDataComment");
            }
        }
        public ActionResult GetEmployeeData()
        {
            try
            {
                var mts1 = new lstentries1();
                mts1.yazars1 = db.yazar.ToList();
                mts1.baslik1 = db.baslik.ToList();
                mts1.entries1 = db.entry.OrderByDescending(abc => abc.kayit_tarih).ToList();
                mts1.entry1yazar1 = db.entry_yazar.OrderByDescending(abc => abc.kayit_tarih).ToList();

                return PartialView("_EmployeeData", mts1);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult Listingloaddata(Guid? TeacherId)
        {
            var mts = new lstentries1();
            mts.yazars1 = db.yazar.ToList();           
            mts.entry1yazar1 = db.entry_yazar.Where(x => x.entry_ID == TeacherId).OrderByDescending(x => x.kayit_tarih).ToList();
            try
            {
                return PartialView("_AddDataL", mts);
            }
            catch (Exception )
            {
                return PartialView("_AddDataL");
            }

        }
        public ActionResult LoadaddTeacherPopup12345()
        {        
            try
            {
                return PartialView("_AddData12345");
            }
            catch (Exception )
            {
                return PartialView("_AddData12345");
            }
        }
        public JsonResult AddTeacher(entry teachers)
        {
            string status = "success";
            try
            {
                teachers.post_like = 0;
                teachers.kayit_tarih = null;
                teachers.ID = Guid.NewGuid();
                db.entry.Add(teachers);
                db.SaveChanges();
                EntriesHub.BroadcastData();
            }
            catch (Exception ex)
            {
                status = ex.Message;
            }
            
            return Json(status, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CountRecords(Guid? id1)
        {
            ViewBag.q = (from i in db.entry_yazar
                         group i by i.entry_ID into pg
                         let count = pg.Count()
                         join y in db.entry on pg.FirstOrDefault().entry_ID equals y.ID
                         orderby y.kayit_tarih descending
                         where pg.Key == id1
                         select count); 
                        
            return PartialView(ViewBag.q);
        }
        public JsonResult Like1(Guid? id )
        {
            db.Configuration.ProxyCreationEnabled = false;
            var ent = db.entry.Where(x => x.ID == id).ToList();

            ttr.Add(User.Identity.Name);

            foreach (var line123 in ttr)
            {
                if (!dict.ContainsKey(line123.ToString()))
                {
                    dict.Add(line123.ToString(), new List<string>());
                }
            }

            List<string> aList = dict[User.Identity.Name.ToString()];

            var guid = User.Identity.GetHashCode();

            List<Guid> guids1 = new List<Guid> {
                    Guid.Empty
            };

            if (User.Identity.IsAuthenticated)
            {
                string guid1 = id.ToString();
                if (aList.Contains(guid1))
                {
                    ent.FirstOrDefault().post_like -= 1;
                    
                    aList.Remove(guid1);
                }
                else if (!aList.Contains(guid1))
                {
                    ent.FirstOrDefault().post_like += 1;
                    
                    aList.Add(guid1);
                }
            }
            db.SaveChanges();
            EntriesHub.BroadcastData();
            return Json(ent, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddTeacher1234(entry teachers)
        {
            string status = "success";
            try
            {
                var yazarid = (from u in db.yazar from i in db.Users where ((u.yazar_ismi == i.Username) && (u.yazar_ismi == User.Identity.Name)) select u.ID).FirstOrDefault();
                teachers.post_like = 0;
                teachers.kayit_tarih = null;             
                teachers.ID = Guid.NewGuid();
                teachers.yazar_id = yazarid;
                db.entry.Add(teachers);
                db.SaveChanges();
                EntriesHub.BroadcastData();
            }
            catch (Exception ex)
            {
                status = ex.Message;
            }

            return Json(status, JsonRequestBehavior.AllowGet);

        }
        public JsonResult AddTeacherComment(entry_yazar teachers)
        {
            string status = "success";
            try
            {
                var yazarid = (from u in db.yazar from i in db.Users where ((u.yazar_ismi == i.Username) && (u.yazar_ismi == User.Identity.Name)) select u.ID).FirstOrDefault();
                teachers.kayit_tarih = DateTime.Now;
                teachers.ID = Guid.NewGuid();
                teachers.entry_ID = Butter.entryid1;
                teachers.yazar_ID = yazarid;
                db.entry_yazar.Add(teachers);
                db.SaveChanges();
                EntriesHub.BroadcastData();
            }
            catch (Exception ex)
            {
                status = ex.Message;
            }

            return Json(status, JsonRequestBehavior.AllowGet);
        }
        public JsonResult UpdateTeacher(entry teacher)
        {
            try
            {
                db.entry.Attach(teacher);                         
                ViewBag.yazar_id = new SelectList(db.yazar, "ID", "yazar_ismi");
                db.Entry(teacher).State = EntityState.Modified;
                db.SaveChanges();
                EntriesHub.BroadcastData();
            }
            catch (Exception ex)
            {
                _ = ex.Message;
            }
            return Json(teacher, JsonRequestBehavior.AllowGet);
        }
        public JsonResult UpdateTeacher151(entry_yazar teacher)
        {
            try
            {
                db.entry_yazar.Attach(teacher);
                ViewBag.entry_id = new SelectList(db.entry, "ID", "entry_ismi");
                ViewBag.yazar_id = new SelectList(db.yazar, "ID", "yazar_ismi");
                db.Entry(teacher).State = EntityState.Modified;
                db.SaveChanges();
                EntriesHub.BroadcastData();
            }
            catch (Exception ex)
            {
                _ = ex.Message;
            }
            return Json(teacher, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteTeacher(Guid? TeacherId)
        {
            string status = "success";
            try
            {
                var pateint = db.entry.Find(TeacherId);
                db.entry.Remove(pateint);
                db.SaveChanges();
                EntriesHub.BroadcastData();
            }
            catch (Exception ex)
            {
                status = ex.Message;
            }
            return Json(status, JsonRequestBehavior.AllowGet);
        }
        
        public JsonResult DeleteTeacher15(Guid? TeacherId)
        {
            string status = "success";
            try
            {
                var pateint = db.entry_yazar.Find(TeacherId);
                db.entry_yazar.Remove(pateint);
                db.SaveChanges();
                EntriesHub.BroadcastData();
            }
            catch (Exception ex)
            {
                status = ex.Message;
            }
            return Json(status, JsonRequestBehavior.AllowGet);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
