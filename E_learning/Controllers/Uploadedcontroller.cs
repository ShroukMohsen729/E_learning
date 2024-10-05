using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using E_learning;

namespace E_learning.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadedController : ControllerBase
    {
        public E_learningcontext Context = new E_learningcontext();

        // GET: api/Uploaded
        [HttpGet]
        public async Task<ActionResult<List<Uploaded>>> GetAll()
        {
            var uploads = await Context.Uploaded.ToListAsync();
            if (uploads == null || !uploads.Any())
                return NotFound("No uploads found.");

            return Ok(uploads);
        }

        // GET: api/Uploaded/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Uploaded>> GetUpload(int id)
        {
            var upload = await Context.Uploaded.FindAsync(id);
            if (upload == null)
                return NotFound("No upload found with the given ID.");

            return Ok(upload);
        }

        // POST: api/Uploaded
        [HttpPost]
        public async Task<ActionResult<Uploaded>> PostUpload([FromBody] Uploaded uploaded)
        {
            if (uploaded == null)
                return BadRequest("Uploaded data cannot be null.");

            if (string.IsNullOrWhiteSpace(uploaded.FileName) || string.IsNullOrWhiteSpace(uploaded.FilePath))
                return BadRequest("File name and path are required.");

            Context.Uploaded.Add(uploaded);
            await Context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUpload), new { id = uploaded.UploadedId }, uploaded);
        }

        // PUT: api/Uploaded/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUpload(int id, [FromBody] Uploaded uploaded)
        {
            if (uploaded == null)
                return BadRequest("Uploaded data cannot be null.");

            var existingUpload = await Context.Uploaded.FindAsync(id);
            if (existingUpload == null)
                return NotFound("No upload found with the given ID.");

            existingUpload.FileName = uploaded.FileName ?? existingUpload.FileName;
            existingUpload.FilePath = uploaded.FilePath ?? existingUpload.FilePath;
            existingUpload.Type = uploaded.Type;
            existingUpload.Status = uploaded.Status;

            await Context.SaveChangesAsync();

            return Ok(existingUpload);
        }

        // DELETE: api/Uploaded/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUpload(int id)
        {
            var uploaded = await Context.Uploaded.FindAsync(id);
            if (uploaded == null)
                return NotFound("No upload found with the given ID.");

            Context.Uploaded.Remove(uploaded);
            await Context.SaveChangesAsync();

            return NoContent();
        }
    }
}
