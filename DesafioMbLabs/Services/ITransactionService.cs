using DesafioMbLabs.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DesafioMbLabs.Services
{
    public interface ITransactionService
    {
        /// <summary>
        /// Create a new transaction
        /// </summary>
        /// <param name="newTransaction">The new transaction to add</param>
        public Task NewTransactionAsync(Transaction newTransaction);

        /// <summary>
        /// Get the transaction of a user
        /// </summary>
        /// <param name="user">Tickets owner</param>
        /// <returns>Transactions List</returns>
        public Task<List<Transaction>> GetUserTransactionsAsync(User user);

        /// <summary>
        /// Get one transaction
        /// </summary>
        /// <param name="id">Transactoin id</param>
        /// <param name="user">Transaction owner</param>
        /// <returns>The transaction</returns>
        public Task<Transaction> GetTransactionAsync(int id, User user);

        /// <summary>
        /// Updates a transaction
        /// </summary>
        /// <param name="newTransaction">Transaction upadted</param>
        public Task UpdateTransactionAsync(Transaction newTransaction);
    }
}
