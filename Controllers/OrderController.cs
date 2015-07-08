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
        //获取违章信息
        public ActionResult GetPeccancyInfo(string PlateNumber, string VIN)
        {
            //模拟数据
            var peccancyInfo = new PeccancyInfo 
            { 
                PlateNumber = "川A12568", Time = DateTime.Now, Address = "锦晖西十街", Behavior = "不按规定停车(1099)", Money = 50, Deduction=0
            };
            return View(peccancyInfo);
        }

        //创建订单
        public ActionResult Create(Order order)
        {
            return View();
        }
	}
}