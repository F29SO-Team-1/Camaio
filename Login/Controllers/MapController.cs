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
        private readonly IThread _threadService;
        public MapController(IThread threadService)
        {
            _threadService = threadService;
        }
        [Route("/Thread/Map/{threadId}")]
        public ActionResult Index(int threadId)
        {
            var thread = _threadService.GetById(threadId);

            var model = new MapModel {  };
            return View(model);
        }

    }
}
