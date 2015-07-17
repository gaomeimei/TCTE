using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TCTE.Models;
using TCTE.ViewModel;
using System.Data.Entity;
using TCTE.Filters;
using TCTE.Utility;

namespace TCTE.Controllers
{
    [CheckSessionState]
    public class OrderController : Controller
    {
        private TCTEContext db = new TCTEContext();

        //获取违章信息
        
        public ActionResult GetPeccancyInfo(string PlateNumber, string VIN)
        {
            if(string.IsNullOrEmpty(PlateNumber) || string.IsNullOrEmpty(VIN))
            {
                return RedirectToAction("Index", "Home");
            }

            PlateNumber = PlateNumber.ToUpper();
            //查违章信息
            var peccancyInfos = PeccancyHelper.GetPeccancyInfo(PlateNumber, VIN);

            //回传页面
            ViewBag.PlateNumber = PlateNumber;
            ViewBag.VIN = VIN;

            return View(peccancyInfos);
        }

        //生成订单
        [HttpPost]
        
        public ActionResult Create(string PlateNumber, string VIN)
        {
            var user = Session["user"] as User;
            //处于上岗状态且和终端绑定的业务员
            var salesMen = db.SalesMen.Where(s => s.CompanyId == user.CompanyId && s.IsLicenced && s.TerminalId != null).ToList();
            ViewBag.SalesMen = new SelectList(salesMen, "Id", "Name");
            //回传页面
            ViewBag.PlateNumber = PlateNumber;
            ViewBag.VIN = VIN;
            return View();
        }
        //生成订单
        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public ActionResult CreateOK([Bind(Include = "PlateNumber,VIN,Name,Phone,Address,Comment,SalesManId")]Order order)
        {
            var user = Session["user"] as User;
            order.CompanyId = user.CompanyId.Value;
            order.CreatedDate = DateTime.Now;
            order.Status = Models.SystemType.OrderStatus.Created;

            var salesman = db.SalesMen.SingleOrDefault(s => s.Id == order.SalesManId);
            if (salesman != null)
                order.TerminalId = salesman.TerminalId;

            if (ModelState.IsValid)
            {
                db.Orders.Add(order);
                db.SaveChanges();
                //生成订单Code
                order.Code = string.Format("{0:yyyyMMdd}{1}", order.CreatedDate, order.Id);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        //订单列表
        
        public ActionResult Index()
        {
            var orders = db.Orders.Include(o => o.SalesMan);
            var user = Session["user"] as User;
            if (RoleHelper.IsInRole(SystemRole.COMPANY_ADMIN))
            {
                orders = from o in orders where o.CompanyId == user.CompanyId 
                         orderby o.Status ascending, o.Id descending 
                         select o;
            }
            else if (RoleHelper.IsInRole(SystemRole.SUPER_ADMIN))
            {
                orders = from o in orders
                         orderby o.Status ascending, o.Id descending
                         select o;
            }
            return View(orders.ToList());
        }

        //订单详情
        
        public ActionResult Details(int id)
        {
            var order = db.Orders.Include(o => o.OrderDetails).Include(o => o.SalesMan).SingleOrDefault(o => o.Id == id);
            return View(order);
        }
    }
}