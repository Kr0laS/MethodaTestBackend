using MethodaTest.Database;
using Microsoft.AspNetCore.Mvc;

namespace MethodaTest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StatusController : ControllerBase
    {
        private readonly AppRepository _repo;

        public StatusController(AppRepository appRepository)
        {
            _repo = appRepository;
        }

        [HttpGet()]
        public List<Status> GetStatuses()
        {
            _repo.InferLabel();
            return _repo.Statuses;
        }

        [HttpPost("Add")]
        public IActionResult AddStatus([FromBody]string name)
        {
            if (string.IsNullOrEmpty(name)) return Ok("Bad Request");

            if(_repo.Statuses.Any(s => s.Name == name)) return Ok("Status Conflict");

            Status status = new()
            {
                Id = _repo.StatusId++,
                Name = name,
                IsFinal = true,
                IsOrphan = true,
                IsInitial = _repo.Statuses.Count == 0
            };

            _repo.AddStatus(status);

            _repo.InferLabel();

            return Ok();
        }

        [HttpPut("Update")]
        public IActionResult UpdateStatus([FromBody] Status dto)
        {
            if (dto is null)
                return Ok("Bad Request");

            var oldStatus = _repo.Statuses.FirstOrDefault(s => s.Id == dto.Id);

            if (oldStatus is null)
                return Ok("Status not found");

            if (!string.IsNullOrEmpty(dto.Name) && dto.Name != oldStatus.Name)
            {
                if (_repo.Statuses.Any(s => s.Name == dto.Name))
                    return Ok("Status with the updated name already exists");

                oldStatus.Name = dto.Name;
            }

            oldStatus.IsInitial = dto.IsInitial;
            oldStatus.IsFinal = dto.IsFinal;

            _repo.UpdateStatus(oldStatus);

            return Ok();
        }

        [HttpDelete("Delete/{statusId}")]
        public IActionResult DeleteStatus(int statusId)
        {
            var statusToDelete = _repo.Statuses.FirstOrDefault(s => s.Id == statusId);

            if (statusToDelete is null)
                return Ok("Status not found");

            _repo.DeleteStatus(statusId);

            _repo.InferLabel();

            return Ok();
        }


    }
}