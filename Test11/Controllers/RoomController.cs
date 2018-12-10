using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Test11.Models;
using Newtonsoft.Json;

namespace Test11.Controllers
{
    public class RoomController : Controller
    {
        ron_projectEntities db = new ron_projectEntities();
        //get room information according to room status selection
        public List<roominfo> GetRoominfos(string selectedStatus)
        {
            List<roominfo> rooms = db.roominfo.ToList();
            if (selectedStatus != "All" && selectedStatus != null)
            {
                int selected = (int)Enum.Parse(typeof(rStatus1), selectedStatus);
                rooms = rooms.Where(x => x.rStatus == selected).ToList();
            }
            return rooms;
        }
        //get all rooms' information
        public ActionResult RoomStatus(string selectedStatus = "All")
        {
            ViewBag.rooms = GetRoominfos(selectedStatus);
            return View(ViewBag.rooms);
        }
        //get the rooms' information according to room status selection and return to partial view
        public PartialViewResult GetRoomStatus(string selectedStatus = "All")
        {
            return PartialView(GetRoominfos(selectedStatus));
        }
        // get room information using Json
        public JsonResult GetRoomStatusJson(string selectedStatus = "All")
        {
            var data = GetRoominfos(selectedStatus).Select(p => new
            {
                rId = p.rId,
                rStatus = p.rStatus,
                hId = p.hId
            });
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}