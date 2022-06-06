using DesafioMbLabs.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace DesafioTestes
{
    [TestClass]
    public class ModelsTest
    {
        [TestMethod]
        public void TesteCriaçãoDeTicket()
        {
            EventManager user = new EventManager("aa@aa.com", "aaaaa", "007.428.201-81", "Carlos Eduardo", "98.640.072/0001-06", "Organization");

            Event newEvent = new Event("festa no meu app", "Festa no meu app", 1, 20, user, DateTime.Now.AddMonths(1),
                DateTime.Now.AddMonths(1).AddHours(6), DateTime.Now.AddMinutes(1), DateTime.Now.AddDays(15), "Meu app");

            user.Payments = new()
            {
                new PaymentForm(0, "Meu pagamento")
            };

            List<Ticket> tickets = new()
            {
                new(newEvent, user),
                new(newEvent, user)
            };

            Transaction tran = new(newEvent, tickets, user.Payments[0]);

            newEvent.AddTicket(tickets);
        }
    }
}
