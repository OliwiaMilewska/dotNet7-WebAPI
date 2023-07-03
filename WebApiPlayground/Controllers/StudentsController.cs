using Microsoft.AspNetCore.Mvc;

namespace WebApiPlayground.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAllStudents()
        {
            string[] students = new string[] { "Jacek", "Kuba", "Oliwia" };
            return Ok(students);
        }
    }
}
