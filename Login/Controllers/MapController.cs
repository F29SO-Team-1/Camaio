using Login.Areas.Identity.Data;
using Login.Data;
using Login.Models.Map;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Login.Controllers
{
    public class MapController : Controller
    {
        private readonly string apiUrl = "https://www.google.com/maps/embed/v1/place?q=";
        private readonly string apiKey = "&key=AIzaSyBtN6zSCAPIyvKcVHC_NL3mvkq5w9zixqg";

        string s = "55%C2%B056'34.4%22N%" +
                "203%C2%B009'58.4%22W";


        private readonly IThread _threadService;
        private readonly IApplicationUsers _userService;
        public MapController(IThread threadService, IApplicationUsers userService)
        {
            _threadService = threadService;
            _userService = userService;
        }

        //there is a issue that it needs a 20 for some stupid reason i dont know why

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
                Lat = thread.Lat + "N",
                Lng = thread.Lng + "W",
                Url = apiUrl + thread.Lat + "N%20" + thread.Lng + "W" + apiKey
            };

            return View(model);
        }

        [Route("Profile/{userName}/Map/")]
        public ActionResult UsersThreadsListMap(string userName)
        {
            LoginUser user = _userService.GetByUserName(userName);
            if (user == null) NotFound();



            return View();
        }


         
    }
}
