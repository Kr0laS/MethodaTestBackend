using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MethodaTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResetController : ControllerBase
    {

        private readonly AppRepository _repo;

        public ResetController(AppRepository appRepository)
        {
            _repo = appRepository;
        }

        [HttpDelete]
        public IActionResult Reset() 
        {
            _repo.Transitions.Clear();
            _repo.Statuses.Clear();

            _repo.TransitionId = 1;
            _repo.StatusId = 1;

            return Ok();
        }
    }
}
