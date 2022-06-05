namespace DesafioMbLabs.Models
{
    /// <summary>
    /// Represents a event ticket
    /// </summary>
    public class Ticket
    {
        public long Id { get; set; }

        public Event TicketEvent { get; set; }

        public User Owner { get; set; }

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
        /// <param name="owner">Ticket owner</param>
        public Ticket(Event ticketEvent, User owner)
        {
            TicketEvent = ticketEvent;
            Owner = owner;
        }

        /// <summary>
        /// Create a new Event
        /// </summary>
        /// <param name="id">Ticket id</param>
        /// <param name="ticketEvent">Event to which the ticket belongs</param>
        /// <param name="owner">Ticket owner</param>
        /// <param name="TransactionData">Transaction where the ticket was bought</param>
        public Ticket(long id, Event ticketEvent, User owner, Transaction transactionData)
        {
            Id = id;
            TicketEvent = ticketEvent;
            Owner = owner;
            TransactionData = transactionData;
        }
    }
}
