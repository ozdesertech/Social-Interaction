using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using sozluk123.Models;
using PagedList.Mvc;
using PagedList;
using System.Runtime.InteropServices;
using System.Collections.Specialized;
using System.ComponentModel;

namespace sozluk123.Controllers
{

    static class Butter
    {
        public static Guid counter;

        public static Guid entryval;

        public static Guid entryid1;


    }



    public class entriesController : Controller
    {
        private sozluk1Entities1 db = new sozluk1Entities1();


        //[Authorize]
        public ActionResult Index123() // multiple model views in one single page razor
        {
            try
            {

                //var abc = db.entry.ToList();
                var mts = new lstentries1();
               // mts.entries1 = db.entry.ToList();
                mts.yazars1 = db.yazar.ToList();
                mts.baslik1 = db.baslik.ToList();
                mts.entries1 = db.entry.OrderByDescending(abc => abc.kayit_tarih).ToList();
                //mts.entry1yazar1 = db.entry_yazar.ToList();
                mts.entry1yazar1 = db.entry_yazar.OrderByDescending(abc => abc.kayit_tarih).ToList();
                return View(mts);

            }
            catch (Exception)
            {

                throw;
            }

        }

        
        //POST: Index123
        [HttpPost]
        public ActionResult Index123(string searchval)
        {

            try
            {
                var test = new lstentries1();
                //test.entries1 = db.entry.ToList();
                test.yazars1 = db.yazar.ToList();
                test.baslik1 = db.baslik.ToList();
                //test.entry1yazar1 = db.entry_yazar.ToList();
                test.entries1 = db.entry.OrderByDescending(abc => abc.kayit_tarih).ToList();
                test.entry1yazar1 = db.entry_yazar.OrderByDescending(abc => abc.kayit_tarih).ToList();

                if (searchval != null)
                {
                    test.entries1 = test.entries1.Where(x => x.entry_ismi.Contains(searchval) || x.entry_icerik.Contains(searchval) || x.yazar.yazar_ismi.Contains(searchval)).ToList();
                
                }

                return View(test);
            }
            catch (Exception)
            {

                throw;
            }


        }
        
        public ActionResult setvalue(Guid id)
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
               

               
                //ViewBag.yazar_id = new SelectList(db.yazar, "ID", "yazar_ismi");
                
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
                //teachers.active = null;
                teachers.ID = Guid.NewGuid();
                db.yazar.Add(teachers);
                db.SaveChanges();
                Butter.counter = teachers.ID;
                
            }
            catch (Exception ex)
            {
                status = ex.Message;
            }

            return Json(status, JsonRequestBehavior.AllowGet);

        }

        
        public JsonResult Updateauthordetails(yazar teachers)
        {
            string status = "success";
            try
            {
                ViewBag.yazar_id = new SelectList(db.yazar, "ID", "yazar_ismi");
                
                teachers.ID = Butter.counter;
                //db.yazar.Add(teachers);
                db.yazar.Attach(teachers);
                db.Entry(teachers).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch (Exception ex)
            {

                status = ex.Message;
            }
           
            return Json(teachers, JsonRequestBehavior.AllowGet);
        }
 
        public ActionResult LoadaddTeacherPopup1()
        {
            try
            {
               
                ViewBag.yazar_id = new SelectList(db.yazar, "ID", "yazar_ismi");
                return PartialView("_AddDataFirstPage");
            }
            catch (Exception )
            {
                return PartialView("_AddDataFirstPage");
            }
        }

        public ActionResult LoadEditTeacherPopup1(Guid? TeacherId)
        {
            try
            {
                ViewBag.yazar_id = new SelectList(db.yazar, "ID", "yazar_ismi");
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
               
                ViewBag.yazar_id = new SelectList(db.yazar, "ID", "yazar_ismi");
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
                ViewBag.entry_id = new SelectList(db.entry, "ID", "entry_ismi");
                ViewBag.yazar_id = new SelectList(db.yazar, "ID", "yazar_ismi");
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

                ViewBag.yazar_id = new SelectList(db.yazar, "ID", "yazar_ismi");
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

                ViewBag.yazar_id = new SelectList(db.yazar, "ID", "yazar_ismi");
                return PartialView("_AddDataComment");
            }
            catch (Exception)
            {
                return PartialView("_AddDataComment");
            }
        }

        public ActionResult listingloaddata(Guid? TeacherId)
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

                ViewBag.yazar_id = new SelectList(db.yazar, "ID", "yazar_ismi");
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
                //teachers.active = null;
                teachers.ID = Guid.NewGuid();
                db.entry.Add(teachers);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                status = ex.Message;
            }
            
            return Json(status, JsonRequestBehavior.AllowGet);

        }

        public ActionResult Like(Guid id)
        {
            entry ent = db.entry.ToList().Find(u => u.ID == id);
            //ent.kayit_tarih = null;
            ent.post_like += 1;
            db.SaveChanges();


            return RedirectToAction("Index123");
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

       
        public ActionResult Like1(Guid? id )
        {
           

          
            var ent = db.entry.Where(x => x.ID == id).ToList();
            ent.FirstOrDefault().post_like += 1;           
            db.SaveChanges();      
            return PartialView(ent) ;
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
            }
            catch (Exception ex)
            {
                status = ex.Message;
            }

            return Json(status, JsonRequestBehavior.AllowGet);

        }

        public JsonResult UpdateTeacher(entry teacher)
        {

            // some changes and db.SaveChanges();

            

            string status = "success";
            try
            {
               
                db.entry.Attach(teacher);                         
                ViewBag.yazar_id = new SelectList(db.yazar, "ID", "yazar_ismi");
                db.Entry(teacher).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                status = ex.Message;

            }
            return Json(teacher, JsonRequestBehavior.AllowGet);
        }
        public JsonResult UpdateTeacher151(entry_yazar teacher)
        {
            string status = "success";
            try
            {
                db.entry_yazar.Attach(teacher);
                ViewBag.entry_id = new SelectList(db.entry, "ID", "entry_ismi");
                ViewBag.yazar_id = new SelectList(db.yazar, "ID", "yazar_ismi");
                db.Entry(teacher).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                status = ex.Message;

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
