﻿using Microsoft.AspNetCore.Mvc;
using RASP_Redis.Services.MongoDB;
using RASP_Redis.Services.Redis;
using RASP_Redis.Services.MongoDB.Utils;
using RASP_Redis.Models.ProjectA;
using RASP_Redis.Models.Auth;
using Microsoft.AspNetCore.Authorization;

namespace RASP_Redis.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectAController : ControllerBase
    {
        private readonly UserService _usersService;
        private readonly MeetingsService _meetingsService;
        private readonly AttendeesService _attendeesService;
        private readonly UserMeetingsService _userMeetingsService;
        private readonly UnregisterUsers _unregisterUsers;
        private readonly ProjectARedisService _cache;

        public ProjectAController(UserService usersService, MeetingsService meetingsService, 
                                    AttendeesService attendeesService, UserMeetingsService userMeetings,
                                    UnregisterUsers unregisterUsers, ProjectARedisService projectARedisService)
        {
            _usersService = usersService;
            _meetingsService = meetingsService;
            _attendeesService = attendeesService;
            _userMeetingsService = userMeetings;
            _unregisterUsers = unregisterUsers;
            _cache = projectARedisService;
        }

        [HttpGet("meetings")]
        public async Task<List<Meeting>> GetMeetings() =>
            await _meetingsService.GetAsync();

        [HttpGet("attendees")]
        public async Task<List<Attendees>> GetAttendees() =>
            await _attendeesService.GetAsync();

        [HttpGet("usermeetings")]
        public async Task<List<Attendees>> GetUserMeetings() =>
            await _attendeesService.GetAsync();

        //[Authorize]
        [HttpPost("meeting")]
        public async Task<IActionResult> Post([FromBody] Meeting newMeeting)
        {
            if (newMeeting == null)
            {
                return BadRequest("Invalid meeting data");
            }

            Console.WriteLine(newMeeting);

            try
            {
                string mID;
                do
                {
                    mID = Guid.NewGuid().ToString("N");
                    var cachedDocId = await _cache.GetCachedDocIdAsync(mID);

                    if (string.IsNullOrEmpty(cachedDocId))
                    {
                        newMeeting.mID = mID;
                        break;
                    }

                    Console.WriteLine($"Collision detected for mID: {mID}. Generating a new one.");
                }
                while (true);
                
                newMeeting.mID = mID;

                await _meetingsService.CreateAsync(newMeeting);
                // await _attendeesService.CreateAsync(newMeeting);
                await _userMeetingsService.AddMeetingAsync(newMeeting.Organizer, newMeeting.mID);    // Fails here
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

        [Authorize]
        [HttpPut("register/{uID}/{mID}")]
        public async Task<IActionResult> RegisterMeeting(string uID, string mID)
        {
            try
            {
                var cachedDocId = await _cache.GetCachedDocIdAsync(mID);
                if (string.IsNullOrEmpty(cachedDocId))
                {
                    return Conflict(new { Message = $"ID {mID} does not exist." });
                }

                await _userMeetingsService.AddMeetingAsync(uID, mID);
                await _attendeesService.AddUserToMeetingAsync(uID, mID);

                return Ok(new { Message = "Registered meeting successfully" });
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error registering Meeting: {ex.Message}", ex);

                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { Message = "An error occured while registering the meeting." });
            }
        }

        [Authorize]
        [HttpDelete("unregister/{uID}/{mID}")]
        public async Task<IActionResult> Unregister(string uID, string mID)
        {
            try
            {
                var cachedDocId = await _cache.GetCachedDocIdAsync(mID);
                if (string.IsNullOrEmpty(cachedDocId))
                {
                    return Conflict(new { Message = $"ID {mID} does not exist." });
                }

                await _userMeetingsService.RemoveMeetingAsync(uID, mID);
                await _attendeesService.RemoveOneAsync(uID, mID);

                return Ok(new { Message = "Unregistered meeting successfully" });
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error unregistering Meeting: {ex.Message}", ex);

                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { Message = "An error occured while unregistering the meeting." });
            }
        }

        [Authorize]
        // If the user is NOT the organizer, invoke the RemoveMeeting from UserMeetingsService AND update AttendeesService
        [HttpDelete("{uID}/{mID}")]
        public async Task<IActionResult> DeleteMeeting(string uID, string mID)
        {
            try
            {
                var cachedDocId = await _cache.GetCachedDocIdAsync(mID);
                if (string.IsNullOrEmpty(cachedDocId))
                {
                    return Conflict(new { Message = $"ID {mID} does not exist." });
                }

                await _unregisterUsers.UnregisterMeetingAsync(mID);
                await _userMeetingsService.RemoveMeetingAsync(uID, mID);
                await _cache.RemoveCachedIDAsync(mID);

                return Ok(new { Message = "Meeting removed successfully" });
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error removing Meeting: {ex.Message}", ex);

                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { Message = "An error occured while removing the meeting." });
            }
        }
    }
}
