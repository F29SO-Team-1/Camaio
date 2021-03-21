using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using Login.Data.Interfaces;
using Login.Data;

namespace Login.Controllers
{
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
        /*[HttpPost]*/
        public async Task<IActionResult> CaptureAzure(int? id)
        {
            var thread = _threadService.GetById(id);
            string imageUri = thread.Image;

            // Create a client
            ComputerVisionClient client = Authenticate(endpoint, subscriptionKey);
            
            var r = await _service.AnalyzeImageUrl(client, imageUri);

            _service.Faces(r);
            _service.Objects(r);
            _service.Tags(r);

            //return RedirectToAction("Index", "Thread", new { @id = thread.ID });
            return Json(r);
        }         
    }
}
