using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using sozluk123.Models;

namespace sozluk123.Controllers
{
    public class basliksController : Controller
    {
        private sozluk1Entities1 db = new sozluk1Entities1();

        // GET: basliks

       
        public ActionResult Index()
        {
            var baslik = db.baslik.Include(b => b.yazar);
            return View(baslik.ToList());
        }

        // GET: basliks/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            baslik baslik = db.baslik.Find(id);
            if (baslik == null)
            {
                return HttpNotFound();
            }
            return View(baslik);
        }

        // GET: basliks/Create
        public ActionResult Create()
        {
            ViewBag.yazar_id = new SelectList(db.yazar, "ID", "yazar_ismi");
            return View();
        }

        // POST: basliks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,baslik_ismi,yazar_id,kayit_tarih,active")] baslik baslik)
        {
            if (ModelState.IsValid)
            {
                baslik.ID = Guid.NewGuid();
                db.baslik.Add(baslik);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.yazar_id = new SelectList(db.yazar, "ID", "yazar_ismi", baslik.yazar_id);
            return View(baslik);
        }

        // GET: basliks/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            baslik baslik = db.baslik.Find(id);
            if (baslik == null)
            {
                return HttpNotFound();
            }
            ViewBag.yazar_id = new SelectList(db.yazar, "ID", "yazar_ismi", baslik.yazar_id);
            return View(baslik);
        }

        // POST: basliks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,baslik_ismi,yazar_id,kayit_tarih,active")] baslik baslik)
        {
            if (ModelState.IsValid)
            {
                db.Entry(baslik).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.yazar_id = new SelectList(db.yazar, "ID", "yazar_ismi", baslik.yazar_id);
            return View(baslik);
        }

        // GET: basliks/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            baslik baslik = db.baslik.Find(id);
            if (baslik == null)
            {
                return HttpNotFound();
            }
            return View(baslik);
        }

        // POST: basliks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            baslik baslik = db.baslik.Find(id);
            db.baslik.Remove(baslik);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Index123() // multiple model views in one single page razor
        {
            try
            {
                var abc = db.baslik.ToList();

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
                var val = db.baslik.ToList();

                if (searchval != null)
                {
                    val = val.Where(x => x.baslik_ismi.Contains(searchval) || x.baslik_icerik.Contains(searchval)).ToList();
                }

                return View(val);
            }
            catch (Exception)
            {

                throw;
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

            var q = (from pd in db.baslik
                     join od in db.yazar on pd.yazar_id equals od.ID

                     orderby pd.kayit_tarih descending
                     select new
                     {
                         pd.ID,
                         pd.baslik_ismi,
                         pd.baslik_icerik,
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
                var model = db.baslik.Where(a => a.ID == TeacherId).FirstOrDefault();
                return PartialView("_UpdateData2", model);
            }
            catch (Exception )
            {
                return PartialView("_UpdateData2");
            }
        }

        public ActionResult LoadaddTeacherPopup()
        {
            try
            {
                ViewBag.yazar_id = new SelectList(db.yazar, "ID", "yazar_ismi");
                return PartialView("_AddData2");
            }
            catch (Exception )
            {
                return PartialView("_AddData2");
            }
        }

        public JsonResult AddTeacher(baslik teachers)
        {
            string status = "success";
            try
            {
                teachers.kayit_tarih = null;
                //teachers.active = null;
                teachers.ID = Guid.NewGuid();
                db.baslik.Add(teachers);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                status = ex.Message;
            }

            return Json(status, JsonRequestBehavior.AllowGet);

        }
        public JsonResult UpdateTeacher(baslik teacher)
        {

            string status = "success";
            try
            {
                db.baslik.Attach(teacher);
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

                var pateint = db.baslik.Find(TeacherId);
                db.baslik.Remove(pateint);
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
