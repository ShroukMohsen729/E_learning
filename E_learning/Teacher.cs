using System.ComponentModel.DataAnnotations;

namespace E_learning
{
    public class Teacher   
    {
        [Key]
        public int T_Id { get; set; }

        [Required(ErrorMessage = "Username is required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long")]
        public string Password { get; set; }

        [Phone(ErrorMessage = "Invalid phone number format")]
        [Required(ErrorMessage = "Phone number is required")]
        public string Phone { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email address")]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }
        public string Photo { get; set; }      //for teacher
        public ICollection<Student> Students { get; set; } = new List<Student>();
        public ICollection<Uploaded> Uploads { get; set; } = new List<Uploaded>();

    }
}
