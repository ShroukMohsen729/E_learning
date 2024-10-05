using System.ComponentModel.DataAnnotations;

namespace E_learning
{
    public class Uploaded
    {
        [Key]
        public int UploadedId { get; set; }  

        [Required(ErrorMessage = "File name is required")]
        public string FileName { get; set; }  
        public string FilePath { get; set; }  

    
        [Required(ErrorMessage = "File type is required")]
        public FileType Type { get; set; }

        [Required(ErrorMessage = "Status is required")]
        public FileStatus Status { get; set; }

      
        public int TeacherId { get; set; }  
         
 
    }

 
    public enum FileType
    {
        Video = 1,
        PDF = 2,
        Exam = 3
    }

    public enum FileStatus
    {
        Pending = 0,
        Active = 1,
        Inactive = 2,
        Archived = 3
    }
}
