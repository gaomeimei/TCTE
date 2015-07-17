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
    }
}