using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TCTE.Models;
using TCTE.ViewModel;
using System.Data.Entity;
using System.Web.Security;
using TCTE.Filters;

namespace TCTE.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Login(string ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string ReturnUrl)
        {
            model.Password = Utility.EncryptHelper.MD5Encrypt(model.Password);
            using (TCTEContext db = new TCTEContext())
            {
                var user = db.Users.Include(u => u.Role).Include(u => u.Role.Functions)
                    .Where(u => u.UserName.ToLower() == model.UserName.ToLower() && u.Password == model.Password)
                    .FirstOrDefault();
                if (user == null)
                {
                    ModelState.AddModelError("", "用户名或密码错误");
                    return View();
                }
                FormsAuthentication.SetAuthCookie(user.UserName, false);
                Session["user"] = user;
                if (string.IsNullOrEmpty(ReturnUrl))
                    return RedirectToAction("Index");
                return Redirect(ReturnUrl);
            }
        }

        [HttpGet]
        [CheckSessionState]
        public ActionResult ModifyPwd()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CheckSessionState]
        public ActionResult ModifyPwd(ModifyPwdModel model)
        {
            if (ModelState.IsValid)
            {
                //加密
                model.NewPwd = model.NewPwdConfirm = Utility.EncryptHelper.MD5Encrypt(model.NewPwd);
                model.OldPwd = Utility.EncryptHelper.MD5Encrypt(model.OldPwd);
                //取得当前用户
                var sessionUser = Session["user"] as User;
                using (TCTEContext db = new TCTEContext())
                {
                    var user = db.Users.Find(sessionUser.Id);
                    //校验原密码
                    if(user.Password != model.OldPwd)
                    {
                        ModelState.AddModelError("", "原密码不正确");
                        return View();
                    }
                    //修改密码
                    user.Password = model.NewPwdConfirm;
                    db.SaveChanges();
                    return RedirectToAction("LogOut");
                }
            }
            return View();
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
