using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using E_learning;

namespace E_learning.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        public E_learningcontext Context = new E_learningcontext();

        // Teachers
        // GET: api/Admin/teachers
        [HttpGet("teachers")]
        public async Task<ActionResult<List<Teacher>>> GetAllTeachers()
        {
            var teachers = await Context.Teachers.Include(t => t.Students).ToListAsync();
            if (!teachers.Any())
                return NotFound("No teachers found.");

            return Ok(teachers);
        }

        // GET: api/Admin/teachers/5
        [HttpGet("teachers/{id}")]
        public async Task<ActionResult<Teacher>> GetTeacherById(int id)
        {
            var teacher = await Context.Teachers.Include(t => t.Students).FirstOrDefaultAsync(t => t.T_Id == id);

            if (teacher == null)
                return NotFound("Teacher not found.");

            return Ok(teacher);
        }

        // POST: api/Admin/teachers
        [HttpPost("teachers")]
        public async Task<ActionResult<Teacher>> AddTeacher([FromBody] Teacher teacher)
        {
            if (teacher == null)
                return BadRequest("Teacher cannot be null.");

            if (string.IsNullOrWhiteSpace(teacher.UserName) || string.IsNullOrWhiteSpace(teacher.Email) || string.IsNullOrWhiteSpace(teacher.Password))
                return BadRequest("Ensure all fields are valid.");

            var existingTeacher = await Context.Teachers.FirstOrDefaultAsync(t => t.Email == teacher.Email);
            if (existingTeacher != null)
                return BadRequest("An account with this email already exists.");

            Context.Teachers.Add(teacher);
            await Context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTeacherById), new { id = teacher.T_Id }, teacher);
        }

        // PUT: api/Admin/teachers/5
        [HttpPut("teachers/{id}")]
        public async Task<IActionResult> UpdateTeacher(int id, [FromBody] Teacher updatedTeacher)
        {
            var teacher = await Context.Teachers.FindAsync(id);

            if (teacher == null)
                return NotFound("Teacher not found.");

            teacher.UserName = updatedTeacher.UserName ?? teacher.UserName;
            teacher.Email = updatedTeacher.Email ?? teacher.Email;
            teacher.Password = updatedTeacher.Password ?? teacher.Password;
            teacher.Phone = updatedTeacher.Phone ?? teacher.Phone;
            teacher.Photo = updatedTeacher.Photo ?? teacher.Photo;

            await Context.SaveChangesAsync();
            return Ok(teacher);
        }

        // DELETE: api/Admin/teachers/5
        [HttpDelete("teachers/{id}")]
        public async Task<IActionResult> DeleteTeacher(int id)
        {
            var teacher = await Context.Teachers.FindAsync(id);
            if (teacher == null)
                return NotFound("Teacher not found.");

            Context.Teachers.Remove(teacher);
            await Context.SaveChangesAsync();
            return NoContent();
        }



        // Students
        // GET: api/Admin/students
        [HttpGet("students")]
        public async Task<ActionResult<List<Student>>> GetAllStudents()
        {
            var students = await Context.Students.Include(s => s.Teachers).ToListAsync();
            if (!students.Any())
                return NotFound("No students found.");

            return Ok(students);
        }

        // GET: api/Admin/students/5
        [HttpGet("students/{id}")]
        public async Task<ActionResult<Student>> GetStudentById(int id)
        {
            var student = await Context.Students.Include(s => s.Teachers).FirstOrDefaultAsync(s => s.S_Id == id);
            if (student == null)
                return NotFound("Student not found.");

            return Ok(student);
        }

        // POST: api/Admin/students
        [HttpPost("students")]
        public async Task<ActionResult<Student>> AddStudent([FromBody] Student student)
        {
            if (student == null)
                return BadRequest("Student cannot be null.");

            if (string.IsNullOrWhiteSpace(student.UserName) || string.IsNullOrWhiteSpace(student.Email) || string.IsNullOrWhiteSpace(student.Password))
                return BadRequest("Ensure all fields are valid.");

            var existingStudent = await Context.Students.FirstOrDefaultAsync(s => s.Email == student.Email);
            if (existingStudent != null)
                return BadRequest("An account with this email already exists.");

            Context.Students.Add(student);
            await Context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetStudentById), new { id = student.S_Id }, student);
        }

        // PUT: api/Admin/students/5
        [HttpPut("students/{id}")]
        public async Task<IActionResult> UpdateStudent(int id, [FromBody] Student updatedStudent)
        {
            var student = await Context.Students.FindAsync(id);
            if (student == null)
                return NotFound("Student not found.");

            student.UserName = updatedStudent.UserName ?? student.UserName;
            student.Email = updatedStudent.Email ?? student.Email;
            student.Password = updatedStudent.Password ?? student.Password;
            student.Phone = updatedStudent.Phone ?? student.Phone;
            student.Photo = updatedStudent.Photo ?? student.Photo;
            student.Level = updatedStudent.Level;

            await Context.SaveChangesAsync();
            return Ok(student);
        }

        // DELETE: api/Admin/students/5
        [HttpDelete("students/{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var student = await Context.Students.FindAsync(id);
            if (student == null)
                return NotFound("Student not found.");

            Context.Students.Remove(student);
            await Context.SaveChangesAsync();
            return NoContent();
        }



        // Uploaded Files
        // GET: api/Admin/uploads
        [HttpGet("uploads")]
        public async Task<ActionResult<List<Uploaded>>> GetAllUploads()
        {
            var uploads = await Context.Uploaded.ToListAsync();
            if (!uploads.Any())
                return NotFound("No uploads found.");

            return Ok(uploads);
        }

        // GET: api/Admin/uploads/5
        [HttpGet("uploads/{id}")]
        public async Task<ActionResult<Uploaded>> GetUploadById(int id)
        {
            var upload = await Context.Uploaded.FindAsync(id);
            if (upload == null)
                return NotFound("Upload not found.");

            return Ok(upload);
        }

        // POST: api/Admin/uploads
        [HttpPost("uploads")]
        public async Task<ActionResult<Uploaded>> AddUpload([FromBody] Uploaded upload)
        {
            if (upload == null)
                return BadRequest("Upload cannot be null.");

            Context.Uploaded.Add(upload);
            await Context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUploadById), new { id = upload.UploadedId }, upload);
        }

        // DELETE: api/Admin/uploads/5
        [HttpDelete("uploads/{id}")]
        public async Task<IActionResult> DeleteUpload(int id)
        {
            var upload = await Context.Uploaded.FindAsync(id);
            if (upload == null)
                return NotFound("Upload not found.");

            Context.Uploaded.Remove(upload);
            await Context.SaveChangesAsync();
            return NoContent();
        }





        // GET: api/Admin/StudentsPendingConfirmation
        [HttpGet("StudentsPendingConfirmation")]
        public async Task<ActionResult<List<Student>>> GetStudentsPendingConfirmation()
        {
            var students = await Context.Students
                .Where(s => !s.confirmed)               // Get students whose photos are not confirmed
                .ToListAsync();

            if (students == null || !students.Any())
                return NotFound("No students pending photo confirmation.");

            return Ok(students);
        }


        // PUT: api/Admin/ConfirmStudentPhoto/5
        [HttpPut("ConfirmStudentPhoto/{id}")]
        public async Task<IActionResult> ConfirmStudentPhoto(int id)
        {
            var student = await Context.Students.FindAsync(id);

            if (student == null)
                return NotFound("Student not found.");

            student.confirmed = true;    

            await Context.SaveChangesAsync();

            return Ok("Student photo confirmed successfully.");
        }
    }
}
