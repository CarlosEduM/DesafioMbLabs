using TicketsManager.Models.AppExceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace TicketsManager.Models
{
    /// <summary>
    /// Represents a university or enterprise event
    /// </summary>
    [Table("Events")]
    public class Event
    {
        [Key]
        [Column("TicketId")]
        public int Id { get; set; }

        [Required]
        [MinLength(5)]
        [MaxLength(64)]
        public string Name { get; set; }

        [Required]
        [MinLength(16)]
        [MaxLength(256)]
        public string Description { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int NumberOfTickets { get; set; }

        [Required]
        [Column(TypeName = "money")]
        public double TicketPrice { get; set; }

        [JsonIgnore]
        [IgnoreDataMember]
        public List<Ticket> Tickets { get; set; }

        [ForeignKey("ManagerId")]
        public EventManager Manager { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime StartDateAndTime { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime EndDateAndTime { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime StartDateToBuy { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime EndDateToBuy { get; set; }

        [Required]
        [MinLength(16)]
        [MaxLength(256)]
        public string Location { get; set; }

        /// <summary>
        /// Create a void event
        /// </summary>
        public Event()
        {

        }

        /// <summary>
        /// Create a new event
        /// </summary>
        /// <param name="name">Event name</param>
        /// <param name="description">Event description</param>
        /// <param name="numberOfTickets">Maximum number of tickets to sell</param>
        /// <param name="ticketPrice">Price of tickets</param>
        /// <param name="manager">Event manager user</param>
        /// <param name="startDateAndTime">Start date of event</param>
        /// <param name="endDateAndTime">End date of event</param>
        /// <param name="startDateToBuy">Start date to buy tickets</param>
        /// <param name="endDateToBuy">End date to buy tickets</param>
        /// <param name="location">Where the event will happen</param>
        public Event(string name, string description, int numberOfTickets, double ticketPrice,
            EventManager manager, DateTime startDateAndTime, DateTime endDateAndTime, DateTime startDateToBuy,
            DateTime endDateToBuy, string location)
        {
            Name = name;
            Description = description;
            NumberOfTickets = numberOfTickets;
            TicketPrice = ticketPrice;
            Manager = manager;
            StartDateAndTime = startDateAndTime;
            EndDateAndTime = endDateAndTime;
            StartDateToBuy = startDateToBuy;
            EndDateToBuy = endDateToBuy;
            Location = location;
            Tickets = new();
        }

        /// <summary>
        /// Create a new event
        /// </summary>
        /// <param name="id">Event id</param>
        /// <param name="name">Event name</param>
        /// <param name="description">Event description</param>
        /// <param name="numberOfTickets">Maximum number of tickets to sell</param>
        /// <param name="ticketPrice">Price of tickets</param>
        /// <param name="tickets">Sold tickets</param>
        /// <param name="manager">Event manager user</param>
        /// <param name="startDateAndTime">Start date of event</param>
        /// <param name="endDateAndTime">End date of event</param>
        /// <param name="startDateToBuy">Start date to buy tickets</param>
        /// <param name="endDateToBuy">End date to buy tickets</param>
        /// <param name="location">Where the event will happen</param>
        public Event(int id, string name, string description, int numberOfTickets, double ticketPrice,
            List<Ticket> tickets, EventManager manager, DateTime startDateAndTime, DateTime endDateAndTime,
            DateTime startDateToBuy, DateTime endDateToBuy, string location)
        {
            Id = id;
            Name = name;
            Description = description;
            NumberOfTickets = numberOfTickets;
            TicketPrice = ticketPrice;
            Tickets = tickets;
            Manager = manager;
            StartDateAndTime = startDateAndTime;
            EndDateAndTime = endDateAndTime;
            StartDateToBuy = startDateToBuy;
            EndDateToBuy = endDateToBuy;
            Location = location;
        }

        /// <summary>
        /// Create a new ticket to the event
        /// </summary>
        /// <returns></returns>
        /// <exception cref="AppException"></exception>
        public Ticket GetANewTicket()
        {
            if (Tickets.Count > NumberOfTickets)
                throw new AppException("Maximum number of tickets has been exceeded");

            Ticket t = new(this);

            Tickets.Add(t);

            return t;
        }

        /// <summary>
        /// Validate all date properties
        /// </summary>
        /// <exception cref="AppException"></exception>
        public void ValidateEventDateTimes()
        {
            if (StartDateAndTime < DateTime.UtcNow.AddDays(1))
                throw new AppException("Start date must be a day after today");

            if (StartDateToBuy < DateTime.UtcNow)
                throw new AppException("Start date to buy must be after now");

            if (EndDateAndTime < StartDateAndTime)
                throw new AppException("End date must be after start date");

            if (EndDateToBuy < StartDateToBuy)
                throw new AppException("End date to buy must be after start date to buy");
        }
    }
}
