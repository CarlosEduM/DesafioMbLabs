using DesafioMbLabs.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DesafioMbLabs.Services
{
    public interface IEventService
    {
        public Task AddEvent(Event newEvent);

        /// <summary>
        /// Remove a event
        /// </summary>
        /// <param name="eventToRemove">Event to be removed</param>
        public Task RemoveEvent(Event eventToRemove);

        /// <summary>
        /// Update a event
        /// </summary>
        /// <param name="newEvent">Event changed</param>
        public Task UpdateEvent(Event newEvent);

        /// <summary>
        /// Get all events
        /// </summary>
        /// <returns>A list of events</returns>
        public List<Event> GetEvents();

        /// <summary>
        /// Get all events that match the given event name
        /// </summary>
        /// <param name="eventName">Event name</param>
        /// <returns>A event</returns>
        public List<Event> GetEvents(string eventName);

        /// <summary>
        /// Get a event data by the id
        /// </summary>
        /// <param name="id">Event Id</param>
        /// <returns>A event</returns>
        public Task<Event> GetEvent(int id);
        
        /// <summary>
        /// Get a event by name to see if the event already exists a event with this name
        /// </summary>
        /// <param name="eventName">Event name</param>
        /// <returns>A event</returns>
        public Task<Event> GetEvent(string eventName);

        /// <summary>
        /// Get events from a user
        /// </summary>
        /// <param name="user"></param>
        /// <returns>List of events</returns>
        public List<Event> GetUserEvents(User user);
    }
}
