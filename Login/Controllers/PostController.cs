using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Login.Models;

namespace Login.Controllers
{
    public class PostController : Controller
    {
        // View
        public ActionResult Index()
        {
            return View();
        }
    }

    //Api
    [Route("api/[controller]")]
    public class PostsController : ControllerBase
    {
        internal AppDb Db { get; }

        public PostsController(AppDb db)
        {
            Db = db;
        }

        // GET: api/posts
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            await Db.Connection.OpenAsync();
            var query = new PostModel.PostQuery(Db);
            var result = await query.LatestPostsAsync();
            return new OkObjectResult(result);

        }

        // GET: api/posts/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOne(int id)
        {
            await Db.Connection.OpenAsync();
            var query = new PostModel.PostQuery(Db);
            var result = await query.FindOneAsync(id);
            if (result is null) return new NotFoundResult();
            return new OkObjectResult(result);
        }

        // POST api/posts

        //PUT api/blog/{id}
    }
}
