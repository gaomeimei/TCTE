using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TCTE.Models;
using TCTE.Models.SystemType;
using TCTE.Utility;

namespace TCTE.Controllers
{
    public class TerminalController : Controller
    {
        private TCTEContext db = new TCTEContext();
        
        //查看激活请求
        public ActionResult Register()
        {
            return View(db.RegistrationRequests.Where(a => a.Status == RegistrationRequestStatus.WaitingApprove).OrderByDescending(a => a.Id).ToList());
        }

        //激设备
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(int id)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var regReq = db.RegistrationRequests.Find(id);
                    regReq.AccessToken = Guid.NewGuid().ToString();                    
                    regReq.Status = RegistrationRequestStatus.Approved;
                    regReq.ApproveDate = DateTime.Now;
                    var terminal = new Terminal { Status = TerminalStatus.NotInitialized, AccessToken = regReq.AccessToken, CreateDate = DateTime.Now };
                    db.Terminals.Add(terminal);
                    db.SaveChanges();
                    trans.Commit();
                    return RedirectToAction("AssignToCompany", new { id = terminal.Id });
                }
                catch(Exception)
                {
                    trans.Rollback();
                    return View();
                }
            }
        }

        //授权设备给商家
        public ActionResult AssignToCompany(int id)
        {
            ViewBag.Companies = new SelectList(db.Companies.ToList(), "Id", "Name");
            return View();
        }

        //授权设备给商家
        [HttpPost]
        public ActionResult AssignToCompany(int id, int? CompanyId)
        {
            if(CompanyId == null)
            {
                ModelState.AddModelError("CompanyId", "授权商家 字段是必选的");
                ViewBag.Companies = new SelectList(db.Companies.ToList(), "Id", "Name");
                return View();
            }
            var terminal = db.Terminals.Find(id);
            terminal.CompanyId = CompanyId;
            var company = db.Companies.Where(c => c.Id == CompanyId.Value).SingleOrDefault();
            terminal.Code = string.Format("{0}{1:000}", company.Code, terminal.Id);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //设备列表
        public ActionResult Index()
        {
            if (RoleHelper.IsInRole(SystemRole.COMPANY_ADMIN))
            {
                var user = Session["user"] as User;
                var terminals = db.Terminals.Include(t => t.SalesMan).Where(t => t.CompanyId == user.CompanyId).OrderByDescending(t=>t.Id);
                return View("IndexCompanyView", terminals.ToList());
            }
            else if (RoleHelper.IsInRole(SystemRole.SUPER_ADMIN))
            {
                return View(db.Terminals.Include(t => t.Company).OrderByDescending(t => t.Id).ToList());
            }
            return HttpNotFound();
        }
        

        // 删除设备
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Terminal terminal = db.Terminals.Find(id);
            if (terminal == null)
            {
                return HttpNotFound();
            }
            return View(terminal);
        }

        // 删除设备
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Terminal terminal = db.Terminals.Find(id);
            db.Terminals.Remove(terminal);
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
