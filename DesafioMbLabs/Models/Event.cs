using DesafioMbLabs.Models.AppExceptions;
using System;
using System.Collections.Generic;

namespace DesafioMbLabs.Models
{
    /// <summary>
    /// Represents a event
    /// </summary>
    public class Event
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int NumberOfTickets { get; set; }

        public double TicketPrice { get; set; }

        public List<Ticket> Tickets { get; set; }

        public EventManager Manager { get; set; }

        public DateTime StartDateAndTime { get; set; }

        public DateTime EndDateAndTime { get; set; }

        public DateTime StartDateToBuy { get; set; }

        public DateTime EndDateToBuy { get; set; }

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

            ValidateEventDateTimes();
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

            ValidateEventDateTimes();
        }

        public void ValidateEventDateTimes()
        {
            if (StartDateAndTime < DateTime.Today.AddDays(1))
                throw new AppException("Start date must be a day after today");

            if (StartDateToBuy < DateTime.Now)
                throw new AppException("Start date to buy must be after now");

            if (EndDateAndTime < StartDateAndTime)
                throw new AppException("End date must be after start date");

            if (EndDateToBuy < StartDateToBuy)
                throw new AppException("End date to buy must be after start date to buy");
        }

        /// <summary>
        /// Add a new ticket
        /// </summary>
        /// <param name="tickets">New tickets</param>
        /// <exception cref="AppException"></exception>
        public void AddTicket(List<Ticket> tickets)
        {
            if (NumberOfTickets > Tickets.Count + tickets.Count)
                throw new AppException("Maximum number of tickets has been exceeded");

            foreach (Ticket ticket in tickets)
            {
                if (ticket.TransactionData.BuyDateTime < StartDateToBuy)
                    throw new AppException("Ticket bought date must be greater than start date to buy");

                if (ticket.TransactionData.BuyDateTime >= EndDateToBuy)
                    throw new AppException("Ticket bought date must be less than end date to buy");
            }

            Tickets.AddRange(tickets);
        }
    }
}
