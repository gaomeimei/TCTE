﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TCTE.Models;
using TCTE.Filters;

namespace TCTE.Controllers
{
    [CheckSessionState]
    public class CompanyController : Controller
    {
        private TCTEContext db = new TCTEContext();

        public SelectList Cities
        {
            get { return new SelectList(db.Cities.ToList(), "Id", "Name"); }
        }

        // GET: /Company/
        public ActionResult Index()
        {
            return View(db.Companies.OrderByDescending(c => c.Id).ToList());
        }

        // GET: /Company/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company company = db.Companies.Find(id);
            if (company == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            return View(company);
        }

        // GET: /Company/Create
        public ActionResult Create()
        {
            ViewBag.Cities = this.Cities;
            ViewBag.Title = "添加商家";
            return View();
        }

        // POST: /Company/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Company company)
        {
            if (ModelState.IsValid)
            {
                //创建Company
                company.CreatedDate = DateTime.Now;
                db.Companies.Add(company);
                db.SaveChanges();
                //生成Company.Code
                company.Code = string.Format("{0}{1}{2:000}", db.Cities.Find(company.CityId).Abbr, company.Abbr, company.Id);
                //生成CompanyAdmin
                var user = new User { CompanyId = company.Id, CreatedDate = DateTime.Now, UserName = company.Code, Password = Utility.EncryptHelper.MD5Encrypt("666666"), RoleId = 2 };
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Cities = this.Cities;
            ViewBag.Title = "添加商家";
            return View(company);
        }

        // GET: /Company/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company company = db.Companies.Find(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            ViewBag.Cities = new SelectList(db.Cities.ToList(), "Id", "Name", company.CityId);
            ViewBag.Title = "编辑商家";
            return View("Create", company);
        }

        // POST: /Company/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Company company)
        {
            if (ModelState.IsValid)
            {
                var entry = db.Entry(company);
                entry.State = EntityState.Modified;
                // Tell EF do not modify the property 'CreatedDate'
                entry.Property("CreatedDate").IsModified = false;
                // Tell EF do not modify the property 'Code'
                entry.Property("Code").IsModified = false;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(company);
        }

        // GET: /Company/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company company = db.Companies.Find(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            return View(company);
        }

        // POST: /Company/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                var user = db.Users.Where(u => u.CompanyId == id).SingleOrDefault();
                if (user != null)
                {
                    db.Users.Remove(user);
                }
                Company company = db.Companies.Find(id);
                db.Companies.Remove(company);
                db.SaveChanges();
                return Json("success");
            }
            catch(Exception ex)
            {
                return HttpNotFound();
            }
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
