using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TCTE.Models;
using System.Data.Entity;
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
    }
}