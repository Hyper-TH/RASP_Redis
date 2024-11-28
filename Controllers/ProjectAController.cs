using Microsoft.AspNetCore.Mvc;
using RASP_Redis.Services.MongoDB;
using RASP_Redis.Models;
using RASP_Redis.Services.Redis;

namespace RASP_Redis.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectAController : ControllerBase
    {
        private readonly UsersService _usersService;
        private readonly MeetingsService _meetingsService;
        private readonly AttendeesService _attendeesService;
        private readonly UserMeetingsService _userMeetingsService;
        private readonly ProjectARedisService _cache;

        public ProjectAController(UsersService usersService, MeetingsService meetingsService, 
                                    AttendeesService attendeesService, UserMeetingsService userMeetings,
                                    ProjectARedisService projectARedisService)
        {
            _usersService = usersService;
            _meetingsService = meetingsService;
            _attendeesService = attendeesService;
            _userMeetingsService = userMeetings;
            _cache = projectARedisService;
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

        [HttpPost("meeting")]
        public async Task<IActionResult> Post([FromBody] Meeting newMeeting)
        {
            if (newMeeting == null)
            {
                return BadRequest("Invalid meeting data");
            }

            try
            {
                var cachedDocId = await _cache.GetCachedDocIdAsync(newMeeting.mID);
                if (!string.IsNullOrEmpty(cachedDocId))
                {
                    return Conflict(new { Message = $"ID {newMeeting.mID} already exists." });
                }

                await _meetingsService.CreateAsync(newMeeting);
                await _userMeetingsService.AddMeetingAsync(newMeeting.Organizer, newMeeting.mID);
                await _cache.CacheIDAsync(newMeeting.mID, newMeeting.Id);

                return CreatedAtAction(nameof(GetMeetings), new { mID = newMeeting.mID }, newMeeting);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error creating Meeting: {ex.Message}", ex);

                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { Message = "An error occured while creating the meeting." });
            }
        }

        [HttpDelete("unregister/{uID}/{mID}")]
        public async Task<IActionResult> Delete(string uID, string mID)
        {
            try
            {
                var cachedDocId = await _cache.GetCachedDocIdAsync(mID);
                if (string.IsNullOrEmpty(cachedDocId))
                {
                    return Conflict(new { Message = $"ID {mID} does not exist." });
                }

                await _userMeetingsService.RemoveMeetingAsync(uID, mID);
                await _cache.CacheIDAsync(uID, mID);

                return Ok(new { Message = "Meeting removed successfully" });
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error creating Meeting: {ex.Message}", ex);

                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { Message = "An error occured while removing the meeting." });
            }
        }
    }
}
