using DesafioMbLabs.Data;
using DesafioMbLabs.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DesafioMbLabs.Services
{
    public class TransactionService
    {
        private readonly SqlServerContext _dbContext;

        public TransactionService(SqlServerContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task NewTransaction(Transaction newTransaction)
        {
            _dbContext.Transactions.Add(newTransaction);

            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Transaction>> GetUserTransactions(User user)
        {
            return await _dbContext.Transactions.Where(t => t.Tickets[0].Owner == user).ToListAsync();
        }

        public async Task<Transaction> GetTransactionAsync(int id)
        {
            return await _dbContext.Transactions.FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task UpdateTransaction(Transaction newTransaction)
        {
            _dbContext.Transactions.Update(newTransaction).State = EntityState.Modified;

            await _dbContext.SaveChangesAsync();
        }
    }
}
