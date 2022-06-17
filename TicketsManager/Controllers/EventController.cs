using DesafioMbLabs.Models;
using DesafioMbLabs.Models.AppExceptions;
using DesafioMbLabs.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace DesafioMbLabs.Controllers
{
    [Route("api/event")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IEventService _eventService;

        private readonly IUserService _userService;

        private readonly ITransactionService _transactionService;

        public EventController(IEventService eventService,
                               IUserService userService,
                               ITransactionService transactionService)
        {
            _eventService = eventService;
            _userService = userService;
            _transactionService = transactionService;
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

                newEvent.Manager = (EventManager)user;

                await _eventService.AddEvent(newEvent);

                return CreatedAtAction(nameof(NewEvent), new { id = newEvent.Id });
            }
            catch (AppException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("userEvents")]
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

        [HttpGet("{id}")]
        public async Task<ActionResult<dynamic>> GetEvent(int id)
        {
            var eventGetted = await _eventService.GetEventAsync(id);

            if (eventGetted == null)
                return NotFound();

            int soldTickets = eventGetted.Tickets
                .Where(t => t.TransactionData.PaymentStatus != PaymentStatus.Canceled)
                .ToList().Count;

            if (!User.Identity.IsAuthenticated || User.Identity.Name != eventGetted.Manager.Name)
                eventGetted.Tickets = null;

            eventGetted.Manager = null;

            return new { EventGetted = eventGetted, SoldTickets = soldTickets };
        }

        [HttpGet("{eventId}/ticket")]
        [Authorize(Roles = "EventManager")]
        public async Task<ActionResult<dynamic>> GetEventTickets(int eventId)
        {
            var user = await _userService.GetUserAsync(User.Identity.Name);

            if (user == null)
                BadRequest(new { message = $"User {user} wasn't found in database" });

            var eventGetted = await _eventService.GetEventAsync(eventId, user);

            if (eventGetted == null)
                return NotFound();

            return eventGetted.Tickets;
        }

        [HttpGet("{eventId}/ticket/{ticketId}")]
        [Authorize(Roles = "EventManager")]
        public async Task<ActionResult<dynamic>> GetEventTicket(int eventId, int ticketId)
        {
            var user = await _userService.GetUserAsync(User.Identity.Name);

            if (user == null)
                BadRequest(new { message = $"User {user} wasn't found in database" });

            var eventGetted = await _eventService.GetEventAsync(eventId, user);

            if (eventGetted == null)
                return NotFound();

            var ticket = eventGetted.Tickets.FirstOrDefault(t => t.Id == ticketId);

            if (ticket == null)
                return NotFound();

            return ticket;
        }

        [HttpPost("{id}/buyTicket")]
        [Authorize]
        public async Task<IActionResult> BuyTicketToEvent([FromRoute] int id,
                                                          [FromBody] Dictionary<string, int> dataToBuy)
        {
            try
            {
                int numberOfTickets = 1;
                int paymentFormId = -1;

                foreach (var key in dataToBuy.Keys)
                {
                    switch (key)
                    {
                        case "numberOfTickets":
                            numberOfTickets = dataToBuy[key];
                            break;
                        case "paymentFormId":
                            paymentFormId = dataToBuy[key];
                            break;
                    }
                }

                if (paymentFormId == -1)
                    return BadRequest(new { message = "The camp paymentFormId don't cold be empty" });

                if (numberOfTickets < 1)
                    return BadRequest(new { message = "Tickets bought number must be greater than or egual to 1" });

                var user = await _userService.GetUserAsync(User.Identity.Name);

                if (user == null)
                    return BadRequest(new { message = $"User {user.Email} wasn't found in database" });

                var paymentForm = user.Payments.FirstOrDefault(pf => pf.Id == paymentFormId);

                if (paymentForm == null)
                    return BadRequest(new { message = $"Payment form id {paymentFormId} does't exists for user {user.Email} in database" });

                var eventGetted = await _eventService.GetEventAsync(id);

                if (eventGetted == null)
                    return NotFound();

                eventGetted.Tickets = eventGetted.Tickets.Where(t => t.TransactionData.PaymentStatus != PaymentStatus.Canceled).ToList();

                Transaction transaction = user.BuyATicket(eventGetted, paymentForm, numberOfTickets);

                await _transactionService.NewTransactionAsync(transaction);

                return CreatedAtAction(nameof(BuyTicketToEvent), transaction);
            }
            catch (AppException ae)
            {
                return BadRequest(new { message = ae.Message });
            }
            catch (JsonException)
            {
                return BadRequest(new { message = "The Payment Form is badly formated" });
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<Event>>> GetEventsAsync([FromQuery] string eventName)
        {
            if (string.IsNullOrEmpty(eventName))
                return await _eventService.GetEventsAsync();

            return await _eventService.GetEventsAsync(eventName);
        }

        [HttpPut("{id}")]
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

        [HttpDelete("{id}")]
        [Authorize(Roles = "EventManager")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var oldEvent = await _eventService.GetEventAsync(id);

            if (oldEvent == null)
                return NotFound();

            if (oldEvent.StartDateToBuy < DateTime.UtcNow)
                return BadRequest(new { message = "Ticket buy has already started" });

            await _eventService.RemoveEventAsync(oldEvent);

            return NoContent();
        }
    }
}
