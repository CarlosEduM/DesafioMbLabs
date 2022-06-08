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

        public async Task<Event> GetEventAsync(int id)
        {
            var eventGetted = await _dbContext.Events
                .Include(e => e.Tickets.Where(t => t.TransactionData.PaymentStatus != PaymentStatus.Canceled))
                .Include(e => e.Manager)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (eventGetted != null)
                _dbContext.Entry(eventGetted).State = EntityState.Detached;

            return eventGetted;
        }

        public async Task<Event> GetEventAsync(string eventName)
        {
            return await _dbContext.Events.FirstOrDefaultAsync(e => e.Name == eventName);
        }

        public async Task<List<Event>> GetEventsAsync()
        {
            return await _dbContext.Events.ToListAsync();
        }

        public async Task<List<Event>> GetEventsAsync(string eventName)
        {
            return await _dbContext.Events.Where(e => e.Name.Contains(eventName)).ToListAsync();
        }

        public async Task<List<Event>> GetUserEventsAsync(User user)
        {
            return await _dbContext.Events.Include(e => e.Tickets).Where(e => e.Manager.Id == user.Id).ToListAsync();
        }

        public async Task RemoveEventAsync(Event eventToRemove)
        {
            _dbContext.Events.Remove(eventToRemove);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateEventAsync(Event newEvent)
        {
            _dbContext.Events.Update(newEvent).State = EntityState.Modified;

            await _dbContext.SaveChangesAsync();
        }
    }
}
