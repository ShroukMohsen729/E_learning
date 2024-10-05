using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace E_learning
{
    public class Admin
    {
        [Key]
        public int AdminId { get; set; }  

        [Required(ErrorMessage = "Username is required")]
        public string UserName { get; set; } 

        [Required(ErrorMessage = "Password is required")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long")]
        public string Password { get; set; }  

        [EmailAddress(ErrorMessage = "Invalid email address")]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }  

        public string Phone { get; set; }      // for support

        public ICollection<Teacher> ManagedTeachers { get; set; } = new List<Teacher>();  
        public ICollection<Student> ManagedStudents { get; set; } = new List<Student>();  
    }
}
