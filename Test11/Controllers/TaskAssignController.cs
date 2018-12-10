using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Test11.Models;
using MySql.Data.MySqlClient;
namespace Test11.Controllers
{
    public class TaskAssignController : Controller
    {
        ron_projectEntities db = new ron_projectEntities();
        // GET: TaskAssign
        public ActionResult Index()
        {
            return View();
        }
        public List<roominfo> SearchToDoRoom()
        {
            List<roominfo> roomInfo = db.roominfo.Where(x => x.rStatus == 0).ToList();
            return roomInfo;
        }
        public List<housekeeperinfo> SearchVacantHousekeeper()
        {
            List<housekeeperinfo> houseKeepers = db.housekeeperinfo.Where(x => x.hStatus == 1).ToList();
            return houseKeepers;
        }
        public List<int> GetVacantRoomId()
        {
            List<int> vacantRoomId = new List<int>();
            foreach (roominfo r in SearchToDoRoom())
            {
                vacantRoomId.Add(r.rId);
            }
            return vacantRoomId;
        }
        public ActionResult VacantHouseKeeper()
        {
            var tuple = new Tuple<List<housekeeperinfo>, List<roominfo>>(SearchVacantHousekeeper(), SearchToDoRoom());
            //ViewData["vacantRoomId"] = GetVacantRoomId();
            return View(tuple);
        }
        public PartialViewResult PartVacantHouseKeeper()
        {
            var tuple = new Tuple<List<housekeeperinfo>, List<roominfo>>(SearchVacantHousekeeper(), SearchToDoRoom());
            return PartialView(tuple);
        }
        public ActionResult GetVacantHouseKeeper()
        {
            ViewBag.tuple = new Tuple<List<housekeeperinfo>, List<roominfo>>(SearchVacantHousekeeper(), SearchToDoRoom());
            return View(ViewBag.tuple);      
        }
        public ActionResult SearchVacantHouseKeeper()
        {
            return RedirectToRoute(new { controller = "TaskAssign", action = "GetVacantHouseKeeper" });
        }
        public JsonResult TaskAssign(string roomId, string houseKeeperId)
        {
            int rmId = Convert.ToInt32(roomId);
            int hkId = Convert.ToInt32(houseKeeperId);
            List<roominfo> roomInfo = db.roominfo.Where(x => x.rId == rmId).ToList();
            List<housekeeperinfo> housekeeperInfo = db.housekeeperinfo.Where(x => x.hId == hkId).ToList();
            string connectionString = @"Server=localhost;Database=ron_project;Uid=root;Pwd=root";
            using (MySqlConnection sqlCon = new MySqlConnection(connectionString))
            {
                sqlCon.Open();
                string query2 = "UPDATE houseKeeperInfo SET rId = " + rmId + ", hStatus = 0 where hId = "+ hkId;
                string query = "UPDATE roomInfo SET hId = "+ hkId + ", rStatus = 1 where rId = " + rmId;
                using (MySqlCommand cmd = new MySqlCommand(query))
                {
                    cmd.Connection = sqlCon;

                    using (MySqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            roomInfo.Add(new roominfo
                            {
                                rId = Convert.ToInt16(sdr["rId"]),
                                hId = Convert.ToInt16(sdr["hId"]),
                                rStatus = Convert.ToInt16(sdr["rStatus"])
                            });
                        }
                    }

                }
                using (MySqlCommand cmd = new MySqlCommand(query2))
                {
                    cmd.Connection = sqlCon;

                    using (MySqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            housekeeperInfo.Add(new housekeeperinfo
                            {
                                rId = Convert.ToInt16(sdr["rId"]),
                                hId = Convert.ToInt16(sdr["hId"]),
                                hStatus = Convert.ToInt16(sdr["hStatus"])
                            });
                        }
                    }

                }
                //}
                sqlCon.Close();
            }
            var data = new Tuple<List<housekeeperinfo>, List<roominfo>>(SearchVacantHousekeeper(), SearchToDoRoom());
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}