using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Test11.Models;


namespace Test11.Controllers
{
    public class HouseKeeperController : Controller
    {
        ron_projectEntities db = new ron_projectEntities();
        // GET: HouseKeeper
        public ActionResult Index()
        {
            return View();
        }
        public List<housekeeperinfo> GethousekeeperInfo(string selectedStatus)
        {

            List<housekeeperinfo> houseKeepers = db.housekeeperinfo.ToList();
            if (selectedStatus != "All" && selectedStatus != null)
            {
                int selected = (int)Enum.Parse(typeof(hStatus1), selectedStatus);
                houseKeepers = houseKeepers.Where(x => x.hStatus == selected).ToList();
            }
            return houseKeepers;
        }
        //get all rooms' information
        public ActionResult HouseKeeperStatus()
        {
            ViewBag.houseKeepers = GethousekeeperInfo(null);
            return View(ViewBag.houseKeepers);
        }
        //get the housekeepers' information according to room status selection and return to partial view
        public PartialViewResult GetHouseKeeperStatus(string selectedStatus = "All")
        {
            return PartialView(GethousekeeperInfo(selectedStatus));
        }
        // get room information using Json
        public JsonResult GetHouseKeeperStatusJson(string selectedStatus = "All")
        {
            //IEnumerable<Person> data = GetData(selectedRole);
            var data = GethousekeeperInfo(selectedStatus).Select(p => new
            {

                hId = p.hId,
                hName = p.hName,
                hStatus = p.hStatus,
                rId = p.rId
            });
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetVacantRoom(string selectedStatus)
        {
            return RedirectToRoute(new { controller="Room",action= "RoomStatus", selectedStatus });
        }
        public JsonResult GetHouseKeeperJson(string selection,string infox,string infoTexty)
        {
            string info;
            if (infox == null || infox == "")
            {
                info = infoTexty;
            }
            else {
                info = infox;
            }
            var data = SearchHouseKeeper(selection,info).Select(p => new
            {

                hId = p.hId,
                hName = p.hName,
                hStatus = p.hStatus,
                rId = p.rId
            });
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public List<housekeeperinfo> SearchHouseKeeper(string selection,string info)
        {

            List<housekeeperinfo> houseKeepers = db.housekeeperinfo.ToList();
                if (selection != "All" && selection != null)
                {
                    int selected = (int)Enum.Parse(typeof(houseKeeperInfo1), selection);
                    switch (selected)
                    {
                        case 0:
                            houseKeepers = houseKeepers.Where(x => x.hId == Convert.ToInt32(info)).ToList();
                            break;
                        case 1:
                            houseKeepers = houseKeepers.Where(x => x.hName == info).ToList();
                            break;
                        case 2:
                            houseKeepers = houseKeepers.Where(x => x.hStatus == Convert.ToInt32(info)).ToList();
                            break;
                        case 3:
                            houseKeepers = houseKeepers.Where(x => x.rId == Convert.ToInt32(info)).ToList();
                            break;
                    }
                }
                return houseKeepers;
        }
    }
}