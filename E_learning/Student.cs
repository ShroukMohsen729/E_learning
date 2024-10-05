using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace E_learning
{
    public class Student               // inhertance from user
    {
        [Key]  
        public int S_Id { get; set; }

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

        public int Level { get; set; }
        public string Photo { get; set; }        //for payment

        public bool confirmed { get; set; }

        public ICollection<Teacher> Teachers { get; set; } = new List<Teacher>();
    }
}
