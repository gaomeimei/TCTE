using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TCTE.Filters;
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
                catch (Exception)
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
            if (CompanyId == null)
            {
                ModelState.AddModelError("CompanyId", "授权商家 字段是必选的");
                ViewBag.Companies = new SelectList(db.Companies.ToList(), "Id", "Name");
                return View();
            }
            var terminal = db.Terminals.Find(id);
            terminal.CompanyId = CompanyId;
            var company = db.Companies.Where(c => c.Id == CompanyId.Value).SingleOrDefault();
            //生成Terminal.Code
            terminal.Code = string.Format("{0}{1:000}", company.Code, terminal.Id);
            db.SaveChanges();
            return RedirectToAction("Register");
        }

        [CheckSessionState]
        public ActionResult AssignToSalesMan(int id)
        {
            var user = Session["user"] as User;
            ViewBag.SalesMen = new SelectList(db.SalesMen.Where(s => s.TerminalId == null && s.CompanyId == user.CompanyId).ToList(), "Id", "Name");
            return View();
        }

        [HttpPost]
        [CheckSessionState]
        public ActionResult AssignToSalesMan(int id, int? SalesManId)
        {
            if (SalesManId == null)
            {
                var user = Session["user"] as User;
                ModelState.AddModelError("SalesManId", "业务员 字段是必选的");
                ViewBag.SalesMen = new SelectList(db.SalesMen.Where(s => s.TerminalId == null && s.CompanyId == user.CompanyId).ToList(), "Id", "Name");
                return View();
            }
            var terminal = db.Terminals.Find(id);
            terminal.SalesManId = SalesManId;
            terminal.Status = TerminalStatus.Normal;
            db.SalesMen.Find(SalesManId).TerminalId = terminal.Id;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //设备列表
        [CheckSessionState]
        public ActionResult Index()
        {
            if (RoleHelper.IsInRole(SystemRole.COMPANY_ADMIN))
            {
                var user = Session["user"] as User;
                var terminals = db.Terminals.Include(t => t.SalesMan).Where(t => t.CompanyId == user.CompanyId).OrderByDescending(t => t.Id);
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

        // 重置
        public ActionResult Reset(int id)
        {
            var terminal = db.Terminals.Find(id);
            if (terminal.SalesMan != null)
            {
                terminal.SalesMan.TerminalId = null;
                terminal.SalesManId = null;
                terminal.Status = TerminalStatus.NotInitialized;
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        // 报修
        public ActionResult ReportMalfunction(int id)
        {
            var terminal = db.Terminals.Find(id);
            //与业务员解除绑定
            db.SalesMen.SingleOrDefault(s => s.TerminalId == id).TerminalId = null;
            terminal.SalesManId = null;
            //清除AccessToken
            terminal.AccessToken = null;            
            //改变状态
            terminal.Status = TerminalStatus.NotInitialized;
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
