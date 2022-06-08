using DesafioMbLabs.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DesafioMbLabs.Services
{
    public interface ITransactionService
    {
        public Task NewTransactionAsync(Transaction newTransaction);

        public Task<List<Transaction>> GetUserTransactionsAsync(User user);

        public Task<Transaction> GetTransactionAsync(int id);

        public Task UpdateTransactionAsync(Transaction newTransaction);
    }
}
