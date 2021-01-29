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
using Microsoft.EntityFrameworkCore;

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

    // Api/posts
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly PostContent _context;

        public PostsController(PostContent content)
        {
            _context = content;
        }
        private bool PostsExists(int id)
        {
            return _context.Posts.Any(e => e.Post_id == id);
        }

        // GET: api/posts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Post>>> GetPosts()
        {
            return await _context.Posts.ToListAsync();
        }

        // GET: api/posts/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Post>> GetOne(int id)
        {
            var posts = await _context.Posts.FindAsync(id);
            if (posts == null) return NotFound();
            return posts;
        }

        // POST: api/posts
        // Used to send data to server to create/update
        // Use this more than PUT
        [HttpPost]
        public async Task<ActionResult<Post>> PostaPost(Post post)
        {
            _context.Posts.Add(post);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPosts), new { id = post.Post_id }, post);
        }

        // PUT api/posts/{id}
        // Create/update
        [HttpPut("{id}")]
        public async Task<IActionResult> PostEdit(int id, Post post)
        {
            if (id != post.Post_id) return BadRequest();
            
            _context.Entry(post).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PostsExists(id)) return NotFound(); 
                else throw; 
            }
            return NoContent();
        }

        // DELETE api/blog/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult<Post>> DeletePost(int id)
        {
            var todoItem = await _context.Posts.FindAsync(id);
            if (todoItem == null) return NotFound();
            _context.Posts.Remove(todoItem);
            await _context.SaveChangesAsync();
            return todoItem;
        }
    }
}
