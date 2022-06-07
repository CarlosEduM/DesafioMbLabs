using DesafioMbLabs.Models;
using DesafioMbLabs.Models.AppExceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace DesafioTestes
{
    [TestClass]
    public class ModelsTest
    {
        [TestMethod]
        public void TesteCriacaoTransacao()
        {
            EventManager user = new("aa@aa.com", "aaaaa", "007.428.201-81", "Carlos Eduardo", "98.640.072/0001-06", "Organization");

            Event newEvent = new("festa no meu app", "Festa no meu app", 1, 20, user, DateTime.Now.AddMonths(1),
                DateTime.Now.AddMonths(1).AddHours(6), DateTime.Now.AddMinutes(1), DateTime.Now.AddDays(15), "Meu app");

            user.Payments = new()
            {
                new PaymentForm(0, "Meu pagamento", user)
            };

            List<Ticket> tickets = new()
            {
                new(newEvent, user),
                new(newEvent, user)
            };

            if (user.Rule != nameof(EventManager))
                Assert.Fail(user.Rule);

            Transaction tran = new(newEvent, tickets, user.Payments[0]);

            newEvent.AddTicket(tickets);
        }

        [TestMethod]
        public void TesteCriacaoTransacao2()
        {
            try
            {
                EventManager user = new("aa@aa.com", "aaaaa", "007.428.201-81", "Carlos Eduardo", "98.640.072/0001-06", "Organization");

                Event newEvent = new("festa no meu app", "Festa no meu app", 1, 20, user, DateTime.Now.AddMonths(1),
                    DateTime.Now.AddMonths(1).AddHours(6), DateTime.Now.AddMinutes(1), DateTime.Now.AddDays(15), "Meu app");

                List<Ticket> tickets = new()
                {
                    new(newEvent, user),
                    new(newEvent, user)
                };

                if (user.Rule != nameof(EventManager))
                    Assert.Fail(user.Rule);
                Transaction tran = new(newEvent, tickets, new PaymentForm(0, "Meu pagamento", user));

                newEvent.AddTicket(tickets);

                Assert.Fail();
            }
            catch (AppException)
            {

            }
        }

        [TestMethod]
        public void TesteCriacaoUsuario()
        {
            try
            {
                User user = new("aa@aa.a", "sasd", "00742820182", "Carlos");

                Assert.Fail();
            }
            catch (FormatException)
            {

            }
        }

        [TestMethod]
        public void TesteCriacaoUsuario2()
        {
            try
            {
                EventManager user = new("aa@aa.a", "sasd", "00742820181", "Carlos", "98.640.072/0001-01", "Minha organização");

                Assert.Fail();
            }
            catch (FormatException)
            {

            }
        }
    }
}
