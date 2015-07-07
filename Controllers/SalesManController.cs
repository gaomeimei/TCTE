using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TCTE.Models;

namespace TCTE.Controllers
{
    public class SalesManController : Controller
    {
        private TCTEContext db = new TCTEContext();

        // GET: /SalesMan/
        public ActionResult Index()
        {
            var user = Session["user"] as User;
            return View(db.SalesMen.Where(a => a.CompanyId ==user.CompanyId).OrderByDescending(s => s.Id).ToList().OrderBy(a => a.Code));
        }

        // GET: /SalesMan/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SalesMan salesman = db.SalesMen.Find(id);
            if (salesman == null)
            {
                return HttpNotFound();
            }
            return View(salesman);
        }

        // GET: /SalesMan/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /SalesMan/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SalesMan salesman)
        {
            //测试数据
            Session["user"] = db.Users.Find(2);
            var user = Session["user"] as User;

            salesman.CreatedDate = DateTime.Now;
            salesman.CompanyId = user.CompanyId.Value;
            if (ModelState.IsValid)
            {
                db.SalesMen.Add(salesman);
                db.SaveChanges();

                salesman.Code = string.Format("{0}{1:000}", db.Companies.Find(salesman.CompanyId).Code, salesman.Id);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.CompanyId = new SelectList(db.Companies, "Id", "Name", salesman.CompanyId);
            ViewBag.TerminalId = new SelectList(db.Terminals, "Id", "SerialNumber", salesman.TerminalId);
            return View(salesman);
        }

        // GET: /SalesMan/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SalesMan salesman = db.SalesMen.Find(id);
            if (salesman == null)
            {
                return HttpNotFound();
            }
            return View("Create", salesman);
        }

        // POST: /SalesMan/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SalesMan salesman)
        {
            if (ModelState.IsValid)
            {
                var entry = db.Entry(salesman);
                entry.State = EntityState.Modified;
                entry.Property(a => a.CreatedDate).IsModified = false;
                entry.Property(a => a.CompanyId).IsModified = false;
                entry.Property(a => a.IsLicenced).IsModified = false;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(salesman);
        }

        // GET: /SalesMan/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SalesMan salesman = db.SalesMen.Find(id);
            if (salesman == null)
            {
                return HttpNotFound();
            }
            return View(salesman);
        }

        // POST: /SalesMan/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            SalesMan salesman = db.SalesMen.Find(id);
            db.SalesMen.Remove(salesman);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult License(int id)
        {
            try
            {
                SalesMan salesman = db.SalesMen.Find(id);
                salesman.IsLicenced = !salesman.IsLicenced;
                db.SaveChanges();
                return Json("success");
            }
            catch(Exception)
            {
                return Json("error");
            }
        }
    }
}
