using Microsoft.AspNetCore.Mvc;
using RASP_Redis.Services.MongoDB;
using RASP_Redis.Models;

namespace RASP_Redis.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectAController : ControllerBase
    {
        private readonly UsersService _usersService;
        private readonly MeetingsService _meetingsService;
        private readonly AttendeesService _attendeesService;

        public ProjectAController(UsersService usersService, MeetingsService meetingsService, AttendeesService attendeesService)
        {
            _usersService = usersService;
            _meetingsService = meetingsService;
            _attendeesService = attendeesService;
        }

        [HttpGet("users")]
        public async Task<List<User>> GetUsers() =>
            await _usersService.GetAsync();

        [HttpGet("meetings")]
        public async Task<List<Meeting>> GetMeetings() =>
            await _meetingsService.GetAsync();

        [HttpGet("attendees")]
        public async Task<List<Attendees>> GetAttendees() =>
            await _attendeesService.GetAsync();

        [HttpGet("usermeetings")]
        public async Task<List<Attendees>> GetUserMeetings() =>
            await _attendeesService.GetAsync();
    }
}
