using DesafioMbLabs.Models;
using DesafioMbLabs.Models.AppExceptions;
using DesafioMbLabs.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DesafioMbLabs.Controllers
{
    [Route("api/event")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IEventService _eventService;
        private readonly IUserService _userService;

        public EventController(IEventService eventService, IUserService userService)
        {
            _eventService = eventService;
            _userService = userService;
        }

        [HttpPost]
        [Authorize(Roles = "EventManager")]
        public async Task<IActionResult> NewEvent(Event newEvent)
        {
            if (await _eventService.GetEventAsync(newEvent.Name) != null)
                return BadRequest(new { message = "Event already exists" });
            
            try
            {
                newEvent.ValidateEventDateTimes();

                var user = await _userService.GetUserAsync(User.Identity.Name);

                if (user == null)
                    BadRequest(new { message = $"User {user} not found in database" });

                newEvent.Manager = (EventManager) user;

                await _eventService.AddEvent(newEvent);

                return CreatedAtAction(nameof(NewEvent), new { id = newEvent.Id });
            }
            catch (AppException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        [Route("userEvents")]
        [Authorize(Roles = "EventManager")]
        public async Task<ActionResult<List<Event>>> GetUserEvents()
        {
            var user = await _userService.GetUserAsync(User.Identity.Name);

            if (user == null)
                BadRequest(new { message = $"User {user} wasn't found in database" });

            var events = await _eventService.GetUserEventsAsync(user);

            foreach (var userEvent in events)
            {
                userEvent.Manager = null;
            }

            return events;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<dynamic>> GetEvent(int id)
        {
            var eventGetted = await _eventService.GetEventAsync(id);

            if (eventGetted == null)
                return NotFound();

            eventGetted.Manager = null;

            int soldTickets = eventGetted.Tickets.Count;

            if (!User.Identity.IsAuthenticated || User.Identity.Name != eventGetted.Manager.Name)
                eventGetted.Tickets = null;

            return new { EventGetted = eventGetted, SoldTickets = soldTickets };
        }

        [HttpGet]
        public async Task<ActionResult<List<Event>>> GetEventsAsync([FromQuery] string eventName)
        {
            if (string.IsNullOrEmpty(eventName))
                return await _eventService.GetEventsAsync();

            return await _eventService.GetEventsAsync(eventName);
        }

        [HttpPut]
        [Route("{id}")]
        [Authorize(Roles = "EventManager")]
        public async Task<IActionResult> UpdateEvent(int id, Event newEvent)
        {
            if (newEvent.Id != id)
                return BadRequest(new { message = "New event has a diferent id number" });

            var oldEvent = await _eventService.GetEventAsync(id);

            if (oldEvent == null)
                return NotFound();

            newEvent.Manager = null;
            newEvent.Tickets = null;

            await _eventService.UpdateEventAsync(newEvent);

            return NoContent();
        }

        [HttpDelete]
        [Route("{id}")]
        [Authorize(Roles = "EventManager")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var oldEvent = await _eventService.GetEventAsync(id);

            if (oldEvent == null)
                return NotFound();

            if (oldEvent.StartDateToBuy < DateTime.UtcNow)
                return BadRequest(new { message = "Ticket buy has already started"});

            await _eventService.RemoveEventAsync(oldEvent);

            return NoContent();
        }
    }
}
