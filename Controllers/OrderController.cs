using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TCTE.Models;
using TCTE.ViewModel;

namespace TCTE.Controllers
{
    public class OrderController : Controller
    {
        private TCTEContext db = new TCTEContext();

        //获取违章信息
        public ActionResult GetPeccancyInfo(string PlateNumber, string VIN)
        {
            //模拟数据
            var peccancyInfos = new List<PeccancyInfo>
            {
                new PeccancyInfo  {   PlateNumber = "川A12568", Time = DateTime.Now, Address = "锦晖西十街", Behavior = "不按规定停车(1099)", Money = 50, Deduction=0 },
                new PeccancyInfo  {   PlateNumber = "川A12568", Time = DateTime.Now, Address = "锦晖西十街", Behavior = "不按规定停车(1099)", Money = 50, Deduction=0 },
                new PeccancyInfo  {   PlateNumber = "川A12568", Time = DateTime.Now, Address = "锦晖西十街", Behavior = "不按规定停车(1099)", Money = 50, Deduction=0 }
            };

            //todo:PlateNumber追加"川", 拉取违章信息 

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
    }
}