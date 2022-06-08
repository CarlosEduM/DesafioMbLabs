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

        public async Task<Event> GetEvent(int id)
        {
            var eventGetted = await _dbContext.Events
                .Include(e => e.Tickets.Where(t => t.TransactionData.PaymentStatus != PaymentStatus.Canceled))
                .Include(e => e.Manager)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (eventGetted != null)
                _dbContext.Entry(eventGetted).State = EntityState.Detached;

            return eventGetted;
        }

        public async Task<Event> GetEvent(string eventName)
        {
            return await _dbContext.Events.FirstOrDefaultAsync(e => e.Name == eventName);
        }

        public List<Event> GetEvents()
        {
            return _dbContext.Events.ToList();
        }

        public List<Event> GetEvents(string eventName)
        {
            return _dbContext.Events.Where(e => e.Name.Contains(eventName)).ToList();
        }

        public List<Event> GetUserEvents(User user)
        {
            return _dbContext.Events.Include(e => e.Tickets).Where(e => e.Manager.Id == user.Id).ToList();
        }

        public async Task RemoveEvent(Event eventToRemove)
        {
            _dbContext.Events.Remove(eventToRemove);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateEvent(Event newEvent)
        {
            _dbContext.Events.Update(newEvent).State = EntityState.Modified;

            await _dbContext.SaveChangesAsync();
        }
    }
}
