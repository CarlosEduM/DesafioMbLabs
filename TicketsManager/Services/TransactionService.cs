using TicketsManager.Data;
using TicketsManager.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TicketsManager.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly SqlServerContext _dbContext;

        public TransactionService(SqlServerContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task NewTransactionAsync(Transaction newTransaction)
        {
            List<Ticket> tickets = new(newTransaction.Tickets);

            newTransaction.Tickets.Clear();
            _dbContext.Transactions.Add(newTransaction);

            await _dbContext.SaveChangesAsync();

            newTransaction.Tickets = tickets;
            _dbContext.Entry(newTransaction).State = EntityState.Modified;
            _dbContext.Entry(tickets.First().TicketEvent).State = EntityState.Modified;
            _dbContext.Entry(tickets.First().Owner).State = EntityState.Modified;

            foreach (var ticket in tickets)
            {
                _dbContext.Entry(ticket).State = EntityState.Added;
            }

            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Transaction>> GetUserTransactionsAsync(User user)
        {
            return await _dbContext.Transactions.Where(t => t.Tickets.First().Owner.Id == user.Id).ToListAsync();
        }

        public async Task<Transaction> GetTransactionAsync(int id, User user)
        {
            return await _dbContext.Transactions.Where(t => t.Tickets.First().Owner.Id == user.Id).FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task UpdateTransactionAsync(Transaction newTransaction)
        {
            _dbContext.Transactions.Update(newTransaction).State = EntityState.Modified;

            await _dbContext.SaveChangesAsync();
        }
    }
}
