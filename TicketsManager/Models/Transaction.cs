using DesafioMbLabs.Models.AppExceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

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

        [JsonIgnore]
        [IgnoreDataMember]
        public List<Ticket> Tickets { get; set; }

        [Required]
        [EnumDataType(typeof(PaymentStatus))]
        public PaymentStatus PaymentStatus { get; set; }

        [Required]
        [ForeignKey("PaymentFormId")]
        public PaymentForm PaymentForm { get; set; }

        [Required]
        [Column(TypeName = "money")]
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
        /// <param name="tickets">Tickets to be bought</param>
        /// <param name="paymentForm">Paymento form of transaction</param>
        public Transaction(PaymentForm paymentForm)
        {
            BuyDateTime = DateTime.UtcNow;
            PaymentForm = paymentForm;
            PaymentStatus = PaymentStatus.Pending;
            TotalPrice = 0;
            Tickets = new();
        }

        /// <summary>
        /// Create a new transaction
        /// </summary>
        /// <param name="id">Transaction id</param>
        /// <param name="buyDateTime">Date and time of buy</param>
        /// <param name="paymentDateTime">Date and time of payment</param>
        /// <param name="tickets">Tickets bought</param>
        /// <param name="paymentStatus">Payment status</param>
        /// <param name="paymentForm">Payment form of transaction</param>
        /// <param name="totalPrice">Total price of transaction</param>
        public Transaction(int id,
                           DateTime buyDateTime,
                           DateTime paymentDateTime,
                           List<Ticket> tickets,
                           PaymentStatus paymentStatus,
                           PaymentForm paymentForm,
                           double totalPrice)
        {
            Id = id;
            BuyDateTime = buyDateTime;
            PaymentDateTime = paymentDateTime;
            Tickets = tickets;
            PaymentStatus = paymentStatus;
            PaymentForm = paymentForm;
            TotalPrice = totalPrice;
        }

        /// <summary>
        /// Add a ticket for the transaction
        /// </summary>
        /// <param name="ticket">Ticket to add</param>
        /// <param name="ticketPrice">Ticket price</param>
        public void AddATicket(Ticket ticket, double ticketPrice)
        {
            Tickets.Add(ticket);

            TotalPrice += ticketPrice;
        }

        /// <summary>
        /// Change payment status to confirmed
        /// </summary>
        /// <exception cref="AppException"></exception>
        public void PayTransaction()
        {
            if (PaymentStatus == PaymentStatus.Confirmed)
                throw new AppException("Transaction already paid");

            if (PaymentStatus == PaymentStatus.Canceled)
                throw new AppException("Transaction was canceled");

            PaymentDateTime = DateTime.UtcNow;
            PaymentStatus = PaymentStatus.Confirmed;
        }

        /// <summary>
        /// Change payment status to canceled
        /// </summary>
        /// <exception cref="AppException"></exception>
        public void CancelTransaction()
        {
            if (PaymentStatus == PaymentStatus.Canceled)
                throw new AppException("Transaction already canceled");

            PaymentStatus = PaymentStatus.Canceled;
        }
    }
}