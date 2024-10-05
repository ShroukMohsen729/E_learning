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
    public class StudentController : ControllerBase
    {
        public E_learningcontext Context = new E_learningcontext();

        // GET: api/Student
        [HttpGet]
        public async Task<ActionResult<List<Student>>> GetAll()
        {
            var students = await Context.Students.Include(s => s.Teachers).ToListAsync();
            if (students == null || !students.Any())
                return NotFound("No students found.");

            return Ok(students);
        }

        // GET: api/Student/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> GetStudent(int id)
        {
            var student = await Context.Students.Include(s => s.Teachers).FirstOrDefaultAsync(s => s.S_Id == id);
            if (student == null)
                return NotFound();

            return Ok(student);
        }

        // POST: api/Student/SignUp
        [HttpPost("SignUp")]
        public async Task<ActionResult<Student>> PostStudent([FromBody] Student student)
        {
            if (student == null)
                return BadRequest("Student cannot be null.");

            if (string.IsNullOrWhiteSpace(student.UserName) ||
                string.IsNullOrWhiteSpace(student.Email) ||
                string.IsNullOrWhiteSpace(student.Password))
                return BadRequest("Make sure that all fields are provided.");

        
            var existingStudent = await Context.Students.FirstOrDefaultAsync(s => s.Email == student.Email);
            if (existingStudent != null)
                return BadRequest("An account with this email already exists.");

            student.Password = BCrypt.Net.BCrypt.HashPassword(student.Password);
            student.confirmed = false; 

            Context.Students.Add(student);
            await Context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetStudent), new { id = student.S_Id }, student);
        }

        // POST: api/Student/Login
        [HttpPost("Login")]
        public async Task<ActionResult> Login([FromBody] LoginModel login)
        {
            try
            {
                var student = await Context.Students.FirstOrDefaultAsync(s => s.Email == login.Username);
                if (student == null)
                    return Unauthorized("Invalid email or password.");


                bool isPasswordValid = BCrypt.Net.BCrypt.Verify(login.Password, student.Password);
                if (!isPasswordValid)
                    return Unauthorized("Invalid email or password.");

                if (!student.confirmed)
                    return BadRequest("Please confirm your email to log in.");

                return Ok("Login successful.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/Student/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudent(int id, [FromBody] Student student)
        {
            if (student == null)
                return BadRequest("Update data cannot be null.");

            var existingStudent = await Context.Students.FindAsync(id);
            if (existingStudent == null)
                return NotFound();

            if (existingStudent.S_Id != student.S_Id)
                return BadRequest("Cannot change ID!");

            existingStudent.UserName = student.UserName ?? existingStudent.UserName;
            existingStudent.Email = student.Email ?? existingStudent.Email;
            existingStudent.Phone = student.Phone ?? existingStudent.Phone;
            existingStudent.Password = !string.IsNullOrEmpty(student.Password) ? BCrypt.Net.BCrypt.HashPassword(student.Password) : existingStudent.Password;
            existingStudent.Level = student.Level;
            existingStudent.Photo = student.Photo ?? existingStudent.Photo;
            existingStudent.confirmed = student.confirmed;

            await Context.SaveChangesAsync();

            return Ok(existingStudent);
        }

        // PUT: api/Student/ConfirmEmail/5
        [HttpPut("ConfirmEmail/{id}")]
        public async Task<IActionResult> ConfirmEmail(int id)
        {
            var student = await Context.Students.FindAsync(id);
            if (student == null)
                return NotFound("Student not found.");

            student.confirmed = true;
            await Context.SaveChangesAsync();

            return Ok("Email confirmed successfully.");
        }

        // DELETE: api/Student/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var student = await Context.Students.FindAsync(id);
            if (student == null)
                return NotFound("Student not found.");

            Context.Students.Remove(student);
            await Context.SaveChangesAsync();

            return NoContent();
        }
    }
}
