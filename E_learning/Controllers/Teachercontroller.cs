using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using E_learning;
using BCrypt.Net;

namespace E_learning.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherController : ControllerBase
    {
        public E_learningcontext Context = new E_learningcontext();

        // GET: api/Teacher
        [HttpGet]
        public async Task<ActionResult<List<Teacher>>> GetAll()
        {
            var teachers = await Context.Teachers.Include(t => t.Students).Include(t => t.Uploads).ToListAsync();
            if (teachers == null || !teachers.Any())
                return NotFound("No teachers found.");

            return Ok(teachers);
        }

        // GET: api/Teacher/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Teacher>> GetTeacher(int id)
        {
            var teacher = await Context.Teachers.Include(t => t.Students).Include(t => t.Uploads).FirstOrDefaultAsync(t => t.T_Id == id);
            if (teacher == null)
                return NotFound();

            return Ok(teacher);
        }

        // POST: api/Teacher/SignUp
        [HttpPost("SignUp")]
        public async Task<ActionResult<Teacher>> PostTeacher([FromBody] Teacher teacher)
        {
            if (teacher == null)
                return BadRequest("Teacher cannot be null.");

            if (string.IsNullOrWhiteSpace(teacher.UserName) ||
                string.IsNullOrWhiteSpace(teacher.Email) ||
                string.IsNullOrWhiteSpace(teacher.Password))
                return BadRequest("Make sure that all fields are provided.");

            var existingTeacher = await Context.Teachers.FirstOrDefaultAsync(t => t.Email == teacher.Email);
            if (existingTeacher != null)
                return BadRequest("An account with this email already exists.");


            teacher.Password = BCrypt.Net.BCrypt.HashPassword(teacher.Password);

            Context.Teachers.Add(teacher);
            await Context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTeacher), new { id = teacher.T_Id }, teacher);
        }

        // POST: api/Teacher/Login
        [HttpPost("Login")]
        public async Task<ActionResult> Login([FromBody] LoginModel login)
        {
            try
            {
                var teacher = await Context.Teachers.FirstOrDefaultAsync(t => t.Email == login.Username);
                if (teacher == null)
                    return Unauthorized("Invalid email or password.");

                // Verify password
                bool isPasswordValid = BCrypt.Net.BCrypt.Verify(login.Password, teacher.Password);
                if (!isPasswordValid)
                    return Unauthorized("Invalid email or password.");

                return Ok("Login successful.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/Teacher/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTeacher(int id, [FromBody] Teacher teacher)
        {
            if (teacher == null)
                return BadRequest("Update data cannot be null.");

            var existingTeacher = await Context.Teachers.FindAsync(id);
            if (existingTeacher == null)
                return NotFound();

            if (existingTeacher.T_Id != teacher.T_Id)
                return BadRequest("Cannot change ID!");

            existingTeacher.UserName = teacher.UserName ?? existingTeacher.UserName;
            existingTeacher.Email = teacher.Email ?? existingTeacher.Email;
            existingTeacher.Password = !string.IsNullOrEmpty(teacher.Password) ? BCrypt.Net.BCrypt.HashPassword(teacher.Password) : existingTeacher.Password;

            await Context.SaveChangesAsync();

            return Ok(existingTeacher);
        }

        // DELETE: api/Teacher/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTeacher(int id)
        {
            var teacher = await Context.Teachers.FindAsync(id);
            if (teacher == null)
                return NotFound();

            Context.Teachers.Remove(teacher);
            await Context.SaveChangesAsync();

            return NoContent();
        }

        // Paged: api/Teacher/5
        [HttpGet("Paged")]
        public async Task<ActionResult<List<Teacher>>> GetPagedData(int pageNum = 1, int pageSize = 10)
        {
            var data = Context.Teachers.ToList();
            var paged = data.Skip((pageNum - 1) * pageSize).Take(pageSize);
            return Ok(paged);
        }

    }


}
