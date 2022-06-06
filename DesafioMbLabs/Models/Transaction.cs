using DesafioMbLabs.Models.AppExceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DesafioMbLabs.Models
{
    /// <summary>
    /// Represents a transaction made by a user
    /// </summary>
    [Table("Transactions")]
    public class Transaction
    {
        [Key]
        [Column("TransactionId")]
        public int Id { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime BuyDateTime { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime PaymentDateTime { get; set; }

        private List<Ticket> _tickets;
        public List<Ticket> Tickets
        {
            get { return _tickets; }
            set
            {
                if (value.Count < 1)
                    throw new ArgumentException("Number of tickets bougth must be greater than 0");

                _tickets = value;
            }
        }

        [Required]
        [EnumDataType(typeof(PaymentStatus))]
        public PaymentStatus PaymentStatus { get; set; }

        [Required]
        [ForeignKey("PaymentFormId")]
        public PaymentForm PaymentForm { get; set; }

        [Required]
        [Column(TypeName = "decimal(5, 2)")]
        public double TotalPrice { get; set; }

        /// <summary>
        /// Create a void Transaction
        /// </summary>
        public Transaction()
        {

        }

        /// <summary>
        /// Create a new transaction
        /// </summary>
        /// <param name="ticketsEvent">transaction related tickets</param>
        /// <param name="tickets">Tickets to be bought</param>
        /// <param name="buyer">Buyer of tickets</param>
        /// <param name="paymentForm">Paymento form of transaction</param>
        /// <exception cref="ArgumentException"></exception>
        public Transaction(Event ticketsEvent, List<Ticket> tickets, PaymentForm paymentForm)
        {
            if (!tickets[0].Owner.Payments.Contains(paymentForm))
                throw new AppException($"This payment form doesn't exist to user {tickets[0].Owner}");

            BuyDateTime = DateTime.UtcNow;
            PaymentForm = paymentForm;
            TotalPrice = ticketsEvent.TicketPrice * tickets.Count;

            foreach (Ticket ticket in tickets)
            {
                ticket.TransactionData = this;
            }

            Tickets = tickets;
        }

        /// <summary>
        /// Create a new transaction
        /// </summary>
        /// <param name="id">Transaction id</param>
        /// <param name="buyDateTime">Date and time of buy</param>
        /// <param name="paymentDateTime">Date and time of payment</param>
        /// <param name="tickets">Tickets bought</param>
        /// <param name="paymentStatus">Payment status</param>
        /// <param name="buyer">Who bought the ticket</param>
        /// <param name="paymentForm">Payment form of transaction</param>
        /// <param name="totalPrice">Total price of transaction</param>
        public Transaction(int id, DateTime buyDateTime, DateTime paymentDateTime, List<Ticket> tickets,
            PaymentStatus paymentStatus, PaymentForm paymentForm, double totalPrice)
        {
            Id = id;
            BuyDateTime = buyDateTime;
            PaymentDateTime = paymentDateTime;
            Tickets = tickets;
            PaymentStatus = paymentStatus;
            PaymentForm = paymentForm;
            TotalPrice = totalPrice;
        }
    }
}