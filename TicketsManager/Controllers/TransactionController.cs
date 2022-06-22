using TicketsManager.Models;
using TicketsManager.Models.AppExceptions;
using TicketsManager.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TicketsManager.Controllers
{
    [Route("api/transaction")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _tranService;

        private readonly IUserService _userService;

        public TransactionController(ITransactionService transactionService, IUserService userService)
        {
            _tranService = transactionService;
            _userService = userService;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<List<Transaction>>> GetUserTransactions()
        {
            var user = await _userService.GetUserAsync(User.Identity.Name);

            if (user == null)
                return BadRequest(new { message = $"User {user.Email} wasn't found in database" });

            return await _tranService.GetUserTransactionsAsync(user);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Transaction>> GetTransaction(int id)
        {
            var user = await _userService.GetUserAsync(User.Identity.Name);

            if (user == null)
                return BadRequest(new { message = $"User {user.Email} wasn't found in database" });

            Transaction transaction = await _tranService.GetTransactionAsync(id, user);

            if (transaction == null)
                return NotFound();

            return transaction;
        }

        [HttpPost]
        [Route("{id}/pay")]
        [Authorize]
        public async Task<IActionResult> PayTransaction(int id)
        {
            try
            {
                var user = await _userService.GetUserAsync(User.Identity.Name);

                if (user == null)
                    return BadRequest(new { message = $"User {user.Email} wasn't found in database" });

                Transaction transaction = await _tranService.GetTransactionAsync(id, user);

                if (transaction == null)
                    return NotFound();

                transaction.PayTransaction();

                await _tranService.UpdateTransactionAsync(transaction);

                return NoContent();
            }
            catch (AppException ae)
            {
                return BadRequest(new { message = ae.Message });
            }
        }

        [HttpPost]
        [Route("{id}/cancel")]
        [Authorize]
        public async Task<IActionResult> CancelTransaction(int id)
        {
            try
            {
                var user = await _userService.GetUserAsync(User.Identity.Name);

                if (user == null)
                    return BadRequest(new { message = $"User {user.Email} wasn't found in database" });

                Transaction transaction = await _tranService.GetTransactionAsync(id, user);

                if (transaction == null)
                    return NotFound();

                transaction.CancelTransaction();

                await _tranService.UpdateTransactionAsync(transaction);

                return NoContent();
            }
            catch (AppException ae)
            {
                return BadRequest(new { message = ae.Message });
            }
        }
    }
}
