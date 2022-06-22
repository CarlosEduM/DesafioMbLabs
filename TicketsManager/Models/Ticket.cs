using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketsManager.Models
{
    /// <summary>
    /// Represents a event ticket
    /// </summary>
    [Table("Tickets")]
    public class Ticket
    {
        [Key]
        [Column("TicketId")]
        public long Id { get; set; }

        [Required]
        [ForeignKey("EventId")]
        public Event TicketEvent { get; set; }

        [Required]
        [ForeignKey("UserId")]
        public User Owner { get; set; }

        [Required]
        [ForeignKey("TransactionId")]
        public Transaction TransactionData { get; set; }

        /// <summary>
        /// Create a void ticket
        /// </summary>
        public Ticket()
        {

        }

        /// <summary>
        /// Create a new Event
        /// </summary>
        /// <param name="ticketEvent">Event to which the ticket belongs</param>
        public Ticket(Event ticketEvent) => TicketEvent = ticketEvent;

        /// <summary>
        /// Create a new Event
        /// </summary>
        /// <param name="id">Ticket id</param>
        /// <param name="ticketEvent">Event to which the ticket belongs</param>
        /// <param name="owner">Ticket owner</param>
        /// <param name="transactionData">Transaction where the ticket was bought</param>
        public Ticket(long id, Event ticketEvent, User owner, Transaction transactionData)
        {
            Id = id;
            TicketEvent = ticketEvent;
            Owner = owner;
            TransactionData = transactionData;
        }
    }
}
