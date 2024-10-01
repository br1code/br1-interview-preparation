using Microsoft.AspNetCore.Mvc;

namespace Br1InterviewPreparation.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        [HttpGet]
        public string Test()
        {
            return "Test";
        }
    }
}
