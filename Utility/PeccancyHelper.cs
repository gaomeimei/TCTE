using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using TCTE.ViewModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Configuration;

namespace TCTE.Utility
{
    public class PeccancyHelper
    {
        private static readonly string PHP_GET_PECCANCYINFO = ConfigurationManager.AppSettings["PHP_GET_PECCANCYINFO"];
        private static readonly string PHP_GET_PERSONFO = ConfigurationManager.AppSettings["PHP_GET_PERSONFO"];

        private static string RequestPeccancyJson(string PlateNumber, string VIN)
        {
            string url = string.Format(PHP_GET_PECCANCYINFO + "?hphm={0}&cjh={1}", PlateNumber, VIN);
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            string json = "";
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                json = reader.ReadToEnd();
            }
            return json;
        }

        private static string RequestDriverJson(string personNo, string archiveId)
        {
            string url = string.Format(PHP_GET_PERSONFO + "?sfzhm={0}&dah={1}", personNo, archiveId);
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            string json = "";
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                json = reader.ReadToEnd();
            }
            return json;
        }
        /// <summary>
        /// 根据车牌号和车架号查询违章信息
        /// </summary>
        /// <param name="PlateNumber">车牌号, 不带"川字"</param>
        /// <param name="VIN">车架号</param>
        /// <returns></returns>
        public static List<PeccancyInfo> GetPeccancyInfo(string PlateNumber, string VIN)
        {
            if(!string.IsNullOrEmpty(PlateNumber) && PlateNumber[0] == '川')
            {
                PlateNumber = PlateNumber.Substring(1);
            }

            string json = RequestPeccancyJson(PlateNumber, VIN);
            List<PeccancyInfo> list = new List<PeccancyInfo>();

            try
            {
                JObject obj = JObject.Parse(json);
                if (Convert.ToString(obj["state"]) == "0")
                {
                    var datazt = Convert.ToString(obj["datazt"]);
                    if (datazt == "1")
                    {
                        var illegal = (from o in obj["data"][0]["illegal"] select o).ToList();
                        foreach (var item in illegal)
                        {
                            list.Add(new PeccancyInfo
                            {
                                PlateNumber = PlateNumber,
                                Time = Convert.ToDateTime(item["wfsj"].ToString()),
                                Address = Convert.ToString(item["wfdz"].ToString()),
                                Behavior = Convert.ToString(item["wfxw"].ToString()),
                                Money = Convert.ToDecimal(item["fkje"].ToString()),
                                Deduction = Convert.ToInt32(item["jf"].ToString())
                            });
                        }
                    }
                }
            }
            catch(Exception)
            {

            }

            return list;
        }

        /// <summary>
        /// 根据车牌号和车架号查询违章信息
        /// </summary>
        /// <param name="PlateNumber">车牌号, 不带"川字"</param>
        /// <param name="VIN">车架号</param>
        /// <returns></returns>
        public static Car GetPeccancyInfo2(string PlateNumber, string VIN)
        {
            if (!string.IsNullOrEmpty(PlateNumber) && PlateNumber[0] == '川')
            {
                PlateNumber = PlateNumber.Substring(1);
            }

            string json = RequestPeccancyJson(PlateNumber, VIN);

            Car car = new Car();
            try
            {
                JObject obj = JObject.Parse(json);
                if (Convert.ToString(obj["state"]) == "0")
                {
                    var datazt = Convert.ToString(obj["datazt"]);
                    if (datazt == "1")
                    {
                        //car info
                        car.Type = Convert.ToString(obj["clxx"]["hpzl"].ToString());
                        car.PlatNumber = Convert.ToString(obj["clxx"]["hphm"].ToString());
                        car.Purpose = Convert.ToString(obj["clxx"]["syxz"].ToString());
                        car.Owner = Convert.ToString(obj["clxx"]["syr"].ToString());
                        car.EndDate1 = Convert.ToDateTime(obj["clxx"]["yxqz"].ToString());
                        car.EndDate2 = Convert.ToDateTime(obj["clxx"]["qzbfqz"].ToString());
                        car.PhoneNumber = Convert.ToString(obj["clxx"]["syrsjhm"].ToString());
                        car.Status = Convert.ToString(obj["clxx"]["zt"].ToString());
                        //PeccanyInfo
                        List<PeccancyInfo> list = new List<PeccancyInfo>();
                        var illegal = (from o in obj["data"][0]["illegal"] select o).ToList();
                        foreach (var item in illegal)
                        {
                            list.Add(new PeccancyInfo
                            {
                                PlateNumber = PlateNumber,
                                Time = Convert.ToDateTime(item["wfsj"].ToString()),
                                Address = Convert.ToString(item["wfdz"].ToString()),
                                Behavior = Convert.ToString(item["wfxw"].ToString()),
                                Money = Convert.ToDecimal(item["fkje"].ToString()),
                                Deduction = Convert.ToInt32(item["jf"].ToString())
                            });
                        }
                        car.PeccancyInfos = list;
                    }
                }
            }
            catch (Exception)
            {

            }

            return car;
        }
        /// <summary>
        /// 根据身份证号码和档案编号获取驾驶员信息
        /// </summary>
        /// <param name="personNo"></param>
        /// <param name="archiveId"></param>
        /// <returns></returns>
        public static Driver GetDriverInfo(string personNo, string archiveId)
        {
            if (string.IsNullOrEmpty(personNo) || string.IsNullOrEmpty(archiveId))
            {
                return null;
            }

            string json = RequestDriverJson(personNo, archiveId);

            try
            {
                JObject obj = JObject.Parse(json);
                Driver driver = new Driver();
                if (Convert.ToString(obj["state"]) == "0")
                {
                    driver.Name = obj["data"]["xm"].ToString();
                    driver.Level = obj["data"]["zjcx"].ToString();
                    driver.Organization = obj["data"]["fzjg"].ToString();
                    driver.Status = obj["data"]["zt"].ToString();
                    driver.Integral = obj["data"]["ljjf"].ToString();
                    driver.StartDate = obj["data"]["cclzrq"].ToString();
                    driver.Phone = obj["data"]["sjhm"].ToString();
                    return driver;
                }
            }
            catch (Exception)
            {

            }
            return null;
        }
    }
}