using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using sozluk123.Models;
using Newtonsoft.Json;

namespace sozluk123.Controllers
{
    public class yazarsController : Controller
    {
        private sozluk1Entities1 db = new sozluk1Entities1();

        // GET: yazars
        public ActionResult Index()
        {
            return View(db.yazar.ToList());
        }

        // GET: yazars/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            yazar yazar = db.yazar.Find(id);
            if (yazar == null)
            {
                return HttpNotFound();
            }
            return View(yazar);
        }

        // GET: yazars/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: yazars/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,yazar_ismi,yazar_yorum,kayit_tarih,active")] yazar yazar)
        {
            if (ModelState.IsValid)
            {
                yazar.ID = Guid.NewGuid();
                db.yazar.Add(yazar);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(yazar);
        }

        // GET: yazars/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            yazar yazar = db.yazar.Find(id);
            if (yazar == null)
            {
                return HttpNotFound();
            }
            return View(yazar);
        }

        // POST: yazars/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,yazar_ismi,yazar_yorum,kayit_tarih,active")] yazar yazar)
        {
            if (ModelState.IsValid)
            {
                db.Entry(yazar).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(yazar);
        }

        // GET: yazars/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            yazar yazar = db.yazar.Find(id);
            if (yazar == null)
            {
                return HttpNotFound();
            }
            return View(yazar);
        }

        // POST: yazars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            yazar yazar = db.yazar.Find(id);
            db.yazar.Remove(yazar);
            db.SaveChanges();
            return RedirectToAction("Index");
        }



        public ActionResult Index123() // multiple model views in one single page razor
        {
            try
            {
                var abc = db.yazar.ToList();

                return View(abc);

                //var mts = new lstentries1();
                //mts.yazars1 = db.yazar.ToList();
                //mts.entries1 = db.entry.ToList();
                //mts.baslik1 = db.baslik.ToList();
                //return View(mts);
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
                var val = db.yazar.ToList();

                if (searchval != null)
                {
                    val = val.Where(x => x.yazar_ismi.Contains(searchval) || x.yazar_yorum.Contains(searchval)).ToList();
                }

                return View(val);
            }
            catch (Exception)
            {

                throw;
            }




            //try
            //{
            //    var val = new lstentries1();
            //    val.yazars1 = db.yazar.ToList();
            //    val.entries1 = db.entry.ToList();
            //    val.baslik1 = db.baslik.ToList();

            //    if (searchval != null)
            //    {
            //        val.entries1 = val.entries1.Where(x => x.entry_ismi.Contains(searchval) || x.entry_icerik.Contains(searchval) || x.yazar.yazar_ismi.Contains(searchval)).ToList();
            //        val.baslik1 = val.baslik1.Where(x => x.baslik_ismi.Contains(searchval) || x.baslik_icerik.Contains(searchval) || x.yazar.yazar_ismi.Contains(searchval)).ToList();
            //        val.yazars1 = val.yazars1.Where(x => x.yazar_ismi.Contains(searchval) || x.yazar_yorum.Contains(searchval)).ToList();
            //    }

            //    return View(val);

            //}
            //catch (Exception)
            //{

            //    throw;
            //}


        }



        public ActionResult Index1()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetAllTeachers()
        {

            db.Configuration.ProxyCreationEnabled = false;

            //var q = db.yazar.ToList().Where(u => u.kayit_tarih > u.kayit_tarih );
            var q = db.yazar.OrderByDescending(s => s.kayit_tarih).ToList();

            return Json(q, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LoadEditTeacherPopup(Guid? TeacherId)
        {
            try
            {
                ViewBag.yazar_id = new SelectList(db.yazar, "ID", "yazar_ismi");
                var model = db.yazar.Where(a => a.ID == TeacherId).FirstOrDefault();
                return PartialView("_UpdateData1", model);
            }
            catch (Exception )
            {
                return PartialView("_UpdateData1");
            }
        }

        public ActionResult LoadaddTeacherPopup()
        {
            try
            {
                ViewBag.yazar_id = new SelectList(db.yazar, "ID", "yazar_ismi");
                return PartialView("_AddData1");
            }
            catch (Exception )
            {
                return PartialView("_AddData1");
            }
        }

        public JsonResult AddTeacher(yazar teachers)
        {
            string status = "success";
            try
            {
                teachers.kayit_tarih = null;
                //teachers.active = null;
                teachers.ID = Guid.NewGuid();
                db.yazar.Add(teachers);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                status = ex.Message;
            }

            return Json(status, JsonRequestBehavior.AllowGet);

        }
        public JsonResult UpdateTeacher(yazar teachers)
        {

            string status = "success";
            try
            {
                db.yazar.Attach(teachers);
                //ViewBag.yazar_id = new SelectList(db.yazar, "ID", "yazar_ismi");
                db.Entry(teachers).State = EntityState.Modified;
                db.SaveChanges();



            }
            catch (Exception ex)
            {
                status = ex.Message;

            }
            return Json(teachers, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteTeacher(Guid? TeacherId)
        {
            string status = "success";
            try
            {

                var pateint = db.yazar.Find(TeacherId);
                db.yazar.Remove(pateint);
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
