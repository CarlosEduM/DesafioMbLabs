using System;
using System.Collections.Generic;

namespace DesafioMbLabs.Models
{
    /// <summary>
    /// Default user for the app
    /// </summary>
    public class User
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string Rule { get { return nameof(User); } }

        private string _cpf;

        public string Cpf
        {
            get { return _cpf; }
            set
            {
                if (!IsCpf(value))
                    throw new FormatException($"The string {value} is not a valid CPF");

                _cpf = value;
            }
        }

        public string Name { get; set; }

        public List<Transaction> Transactions { get; set; }

        public List<Ticket> Tickets { get; set; }

        public List<PaymentForm> Payments { get; set; }

        /// <summary>
        /// Create a void user
        /// </summary>
        public User()
        {
            Transactions = new List<Transaction>();
            Tickets = new List<Ticket>();
        }

        /// <summary>
        /// Create a new User
        /// </summary>
        /// <param name="email">User email</param>
        /// <param name="password">User password</param>
        /// <param name="cpf">User CPF</param>
        /// <param name="name">Username</param>
        public User(string email, string password, string cpf, string name)
        {
            Email = email;
            Password = password;
            Cpf = cpf;
            Name = name;
            Transactions = new();
            Tickets = new();
            Payments = new();
        }

        /// <summary>
        /// Create a new User
        /// </summary>
        /// <param name="id"></param>
        /// <param name="email">User email</param>
        /// <param name="password">User password</param>
        /// <param name="cpf">User CPF</param>
        /// <param name="name">Username</param>
        /// <param name="transactions">Transactions made by the user</param>
        /// <param name="tickets">User tickets</param>
        public User(int id, string email, string password, string cpf, string name,
            List<Transaction> transactions, List<Ticket> tickets, List<PaymentForm> payment)
        {
            Id = id;
            Email = email;
            Password = password;
            Cpf = cpf;
            Name = name;
            Transactions = transactions;
            Tickets = tickets;
            Payments = payment;
        }

        public void BuyTickets()
        {

        }

        /// <summary>
        /// Validates the CPF camp
        /// </summary>
        /// <param name="cpf">A CPF</param>
        /// <returns>true if the CPF is valid and false if not</returns>
        public static bool IsCpf(string cpf)
        {
            cpf = cpf.Trim();
            cpf = cpf.Replace(".", "").Replace("-", "");

            if (cpf.Length != 11)
                return false;

            string tempCpf = cpf.Substring(0, 9);
            int sum = 0;

            for (int i = 0; i < 9; i++)
                sum += int.Parse(tempCpf[i].ToString()) * (new int[] { 10, 9, 8, 7, 6, 5, 4, 3, 2 })[i];
            int rest = sum % 11;

            if (rest < 2)
                rest = 0;
            else
                rest = 11 - rest;

            string digit = rest.ToString();
            tempCpf += digit;
            sum = 0;

            int[] multiplier2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            for (int i = 0; i < 10; i++)
                sum += int.Parse(tempCpf[i].ToString()) * multiplier2[i];

            rest = sum % 11;

            if (rest < 2)
                rest = 0;
            else
                rest = 11 - rest;

            digit += rest.ToString();
            return cpf.EndsWith(digit);
        }
    }
}
