using DesafioMbLabs.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DesafioMbLabs.Services
{
    public interface IEventService
    {
        public Task AddEvent(Event newEvent);

        public Task RemoveEvent(Event eventToRemove);

        public Task UpdateEvent(Event newEvent);

        public Event GetEvent(int id);

        public List<Event> GetUserEvents(User user);

        public List<Event> SearchForAEvent(string eventname);
    }
}
