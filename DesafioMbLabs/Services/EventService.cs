using DesafioMbLabs.Data;
using DesafioMbLabs.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DesafioMbLabs.Services
{
    public class EventService : IEventService
    {
        private readonly SqlServerContext _dbContext;

        public EventService(SqlServerContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddEvent(Event newEnvent)
        {
            _dbContext.Events.Add(newEnvent);
            await _dbContext.SaveChangesAsync();
        }

        public Event GetEvent(int id)
        {
            return _dbContext.Events.FirstOrDefault(e => e.Id == id);
        }

        public List<Event> GetUserEvents(User user)
        {
            return _dbContext.Events.Where(e => e.Manager.Id == user.Id).ToList();
        }

        public async Task RemoveEvent(Event eventToRemove)
        {
            _dbContext.Events.Remove(eventToRemove);
            await _dbContext.SaveChangesAsync();
        }

        public List<Event> SearchForAEvent(string? eventname)
        {
            if (string.IsNullOrEmpty(eventname))
                return _dbContext.Events.ToList();

            return _dbContext.Events.Where(e => e.Name.Contains(eventname)).ToList();
        }

        public async Task UpdateEvent(Event newEvent)
        {
            _dbContext.Events.Update(newEvent).State = EntityState.Modified;

            await _dbContext.SaveChangesAsync();
        }
    }
}
