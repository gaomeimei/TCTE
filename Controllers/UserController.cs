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
    [CheckSessionState]
    public class UserController : Controller
    {
        //
        // GET: /User/
        public ActionResult Index()
        {
            using (TCTEContext db = new TCTEContext())
            {
                var users = db.Users.Include(u => u.Role).Include(u => u.Company).OrderByDescending(u => u.Id).ToList();
                return View(users);
            }
        }
        public ActionResult ModifyPwd()
        {
            if (Request.QueryString["userId"] == null)
            {
                throw new Exception("请传递正确的参数");
            }
            int userId = int.Parse(Request.QueryString["userId"]);
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
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
                    int userId = int.Parse(Request.QueryString["userId"]);
                    var user = db.Users.Where(u => u.Id == userId).SingleOrDefault();
                    //校验原密码
                    if (user.Password != model.OldPwd)
                    {
                        ModelState.AddModelError("", "原密码不正确");
                        return View();
                    }
                    //修改密码
                    user.Password = model.NewPwdConfirm;
                    db.SaveChanges();
                    return Redirect("/User/index");
                }
            }
            return View();
        }

    }
}