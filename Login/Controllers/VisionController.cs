using Login.Data;
using Login.Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using System.Threading.Tasks;

namespace Login.Controllers
{
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class VisionController : Controller
    {
        //azure vision api key and endpoint
        static string subscriptionKey = "5d7d56109a794e2b9532bdde2185755d";
        static string endpoint = "https://camaioai.cognitiveservices.azure.com/";
        //injections
        private readonly IThread _threadService;
        private readonly IVision _service;

        public VisionController(IVision service, IThread threadService)
        {
            _service = service;
            _threadService = threadService;
        }
        /*
         * AUTHENTICATE
         * Creates a Computer Vision client used by each example.
         */
        private static ComputerVisionClient Authenticate(string endpoint, string key)
        {
            ComputerVisionClient client =
              new ComputerVisionClient(new ApiKeyServiceClientCredentials(key))
              { Endpoint = endpoint };
            return client;
        }

        [Route("AI/{id}")]
        //[HttpGet("{id}")]
        public async Task<IActionResult> AI(int? id)
        {
            var thread = _threadService.GetById(id);

            if (thread == null)
            {
                return BadRequest();
            }
            string imageUri = thread.Image;

            // Create a client
            ComputerVisionClient client = Authenticate(endpoint, subscriptionKey);

            var r = await _service.AnalyzeImageUrl(client, imageUri);

            _service.Faces(r);
            _service.Objects(r);
            _service.Tags(r);

            var boolHuman = _service.Description(r);

            //return RedirectToAction("Index", "Thread", new { @id = thread.ID });
            return Json(boolHuman);
        }

        [Route("AIFULL/{id}")]
        [HttpGet]
        public async Task<IActionResult> FullAPI(int? id)
        {
            var thread = _threadService.GetById(id);
            if (thread == null)
            {
                return BadRequest();
            }
            string imageUri = thread.Image;

            // Create a client
            ComputerVisionClient client = Authenticate(endpoint, subscriptionKey);

            var r = await _service.AnalyzeImageUrl(client, imageUri);

            _service.Faces(r);
            _service.Objects(r);
            _service.Tags(r);

            return Json(r);
        }
    }
}
