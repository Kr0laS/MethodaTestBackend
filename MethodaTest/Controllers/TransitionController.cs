using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MethodaTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransitionController : ControllerBase
    {
        private readonly AppRepository _repo;

        public TransitionController(AppRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public List<Transition> GetTransitions()
        {
            return _repo.Transitions;
        }

        [HttpPost("Add")]
        public IActionResult AddTransition([FromBody] CreateTransitionDto dto)
        {
            if (dto is null || dto.FromStatusId == dto.ToStatusId) return Ok("Bad Request");

            if (_repo.Transitions.Any(t => t.Name == dto.Name))
                return Ok("Transition Conflict");

            if (!_repo.Statuses.Any(s => s.Id == dto.FromStatusId) ||
                !_repo.Statuses.Any(s => s.Id == dto.ToStatusId))
            {
                return Ok("Invalid statuses.");
            }

            Transition transition = new() //could implament auto mapper, didnt because of time pressure.
            {
                Id = _repo.TransitionId++,
                Name = dto.Name,
                ToStatusId = dto.ToStatusId,
                FromStatusId = dto.FromStatusId
            };

            _repo.AddTransition(transition);

            _repo.InferLabel();

            return Ok();
        }

        [HttpDelete("Delete/{transitionId}")]
        public IActionResult DeleteTransition(int transitionId)
        {
            var transitionToDelete = _repo.Transitions.FirstOrDefault(t => t.Id == transitionId);

            if (transitionToDelete is null) return Ok("Transition not found");

            _repo.DeleteTransition(transitionId);

            _repo.InferLabel();

            return Ok();
        }
    }
}
