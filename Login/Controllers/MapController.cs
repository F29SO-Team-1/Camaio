using Login.Areas.Identity.Data;
using Login.Data;
using Login.Models.Map;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Login.Controllers
{
    public class MapController : Controller
    {
        private readonly string apiUrl = "https://www.google.com/maps/embed/v1/place?q=";
        private readonly string apiKey = "&key=AIzaSyBtN6zSCAPIyvKcVHC_NL3mvkq5w9zixqg";

        private readonly IThread _threadService;
        private readonly IApplicationUsers _userService;
        public MapController(IThread threadService, IApplicationUsers userService)
        {
            _threadService = threadService;
            _userService = userService;
        }

        //there is a issue that it needs a 20, in the url for some reason i dont know why

        [Route("/Thread/Map/{threadId}")]
        public ActionResult Index(int threadId)
        {
            var thread = _threadService.GetById(threadId);
            
            if(thread.Lat == null || thread.Lng == null)
            {
                return RedirectToAction("Index", "Thread", new { id = threadId });
            }

            var model = new MapModel
            {
                Lat = thread.Lat,
                Lng = thread.Lng,
                Url = apiUrl + thread.Lat + "N%20" + thread.Lng + "W" + apiKey
            };

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [Route("Profile/{userName}/Map/")]
        public ActionResult UsersThreadsListMap(string userName)
        {
            LoginUser user = _userService.GetByUserName(userName);
            if (user == null) NotFound();
            var listOfCords = UsersPostsLocation(userName);
            var model = new MapModelList {Username= userName, MapCordsList = listOfCords };

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [Route("/map/api/{username}")]
        public JsonResult DataTest(string userName)
        {
            LoginUser user = _userService.GetByUserName(userName);
            if (user == null) NotFound();
            var listOfCords = UsersPostsLocation(userName);
            if (listOfCords.Count() == 0 )
            {
                return Json(null);
            }
            var model = new MapModelList {Username = userName, MapCordsList = listOfCords };

            return Json(model);
        }

        private IEnumerable<MapModel> UsersPostsLocation(string userName)
        {
            return _threadService.UserThreadsWithoutAlbum(userName).Select(thread => new MapModel
            {
                Lat = thread.Lat,
                Lng = thread.Lng

            })
                .Where(x=> x.Lat != null);
        }

    }
}
