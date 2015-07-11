using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TCTE.Filters;
using TCTE.Models;

namespace TCTE.Controllers
{
    public class ClientController : Controller
    {
        private TCTEContext db = new TCTEContext();

        // GET: /Client/
        [CheckSessionState]
        public ActionResult Index()
        {
            var user = Session["user"] as User;
            var clients = db.Clients.Where(c => c.CompanyId == user.CompanyId).OrderByDescending(c => c.Id).ToList();
            return View(clients);
        }

        // GET: /Client/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client client = db.Clients.Find(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
        }

        // GET: /Client/Create
        public ActionResult Create()
        {
            ViewBag.Cities = new SelectList(db.Cities.ToList(), "Id", "Name");
            ViewBag.Title = "添加客户";
            return View();
        }

        // POST: /Client/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CheckSessionState]
        public ActionResult Create([Bind(Include="Id,Name,Gender,PlateNumber,VIN,Phone,Address,Source,Comment,CityId")] Client client)
        {
            var user = Session["user"] as User;
            client.CompanyId = user.CompanyId.Value;

            if (ModelState.IsValid)
            {
                //保存Client
                db.Clients.Add(client);
                db.SaveChanges();

                //生成Client.Code
                client.Code = string.Format("{0}{1:000}", db.Companies.Find(client.CompanyId).Code, client.Id);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.Cities = new SelectList(db.Cities.ToList(), "Id", "Name", client.CityId);
            return View(client);
        }

        // GET: /Client/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client client = db.Clients.Find(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            ViewBag.Cities = new SelectList(db.Cities.ToList(), "Id", "Name", client.CityId);
            ViewBag.Title = "编辑客户";
            return View("Create", client);
        }

        // POST: /Client/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CheckSessionState]
        public ActionResult Edit([Bind(Include="Id,Name,Gender,PlateNumber,VIN,Phone,Address,Source,Comment,CityId")] Client client)
        {
            var user = Session["user"] as User;
            client.CompanyId = user.CompanyId.Value;
            if (ModelState.IsValid)
            {
                var entry = db.Entry(client);
                entry.State = EntityState.Modified;
                //更新时排除Code
                entry.Property("Code").IsModified = false;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Cities = new SelectList(db.Cities.ToList(), "Id", "Name", client.CityId);
            return View(client);
        }

        // GET: /Client/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client client = db.Clients.Find(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
        }

        // POST: /Client/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Client client = db.Clients.Find(id);
            db.Clients.Remove(client);
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
    }
}
