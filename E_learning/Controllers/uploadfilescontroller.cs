using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace E_learning.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class uploadfilescontroller: ControllerBase
    {
       
            [HttpPost("upload")]
            public async Task<ActionResult<string>> UploadFile(IFormFile file)
            {
                if (file == null || file.Length == 0)
                    return BadRequest("No File Uploaded");

                var filePath = Path.Combine("root/uploadfiles", file.FileName);

                var filePathReturned = Path.Combine("root/uploadfiles", file.FileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                    await file.CopyToAsync(stream);

                return Ok(new { filePathReturned });

            }
    }
}
