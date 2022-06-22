using TicketsManager.Models;
using TicketsManager.Models.AppExceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace DesafioTestes
{
    [TestClass]
    public class ModelsTest
    {
        [TestMethod]
        public void TesteCriacaoTransacao()
        {
            EventManager user = new("aa@aa.com", "aaaaa", "007.428.201-81", "Carlos Eduardo", "98.640.072/0001-06", "Organization");

            Event newEvent = new("festa no meu app", "Festa no meu app", 2, 20, user, DateTime.Now.AddMonths(1),
                DateTime.Now.AddMonths(1).AddHours(6), DateTime.Now.AddMinutes(1), DateTime.Now.AddDays(15), "Meu app");

            user.Payments = new()
            {
                new PaymentForm(0, "Meu pagamento")
            };

            user.BuyATicket(newEvent, user.Payments[0], 2);
        }

        [TestMethod]
        public void TesteCriacaoTransacao2()
        {
            try
            {
                EventManager user = new("aa@aa.com", "aaaaa", "007.428.201-81", "Carlos Eduardo", "98.640.072/0001-06", "Organization");

                Event newEvent = new("festa no meu app", "Festa no meu app", 1, 20, user, DateTime.Now.AddMonths(1),
                    DateTime.Now.AddMonths(1).AddHours(6), DateTime.Now.AddMinutes(1), DateTime.Now.AddDays(15), "Meu app");

                user.BuyATicket(newEvent, user.Payments[0], 2);

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
