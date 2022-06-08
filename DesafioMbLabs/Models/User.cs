using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DesafioMbLabs.Models.CustomAttributes;
using Microsoft.EntityFrameworkCore;

namespace DesafioMbLabs.Models
{
    /// <summary>
    /// Default user for the app
    /// </summary>
    [Table("Users")]
    public class User : UserBase
    {
        [NotMapped]
        public virtual string Role { get { return nameof(User); } }

        [Required]
        [MinLength(11)]
        [MaxLength(13)]
        [CustomValidationCpf]
        public string Cpf { get; set; }

        [Required]
        [MinLength(8)]
        [MaxLength(64)]
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
            Payments = new List<PaymentForm>();
        }

        /// <summary>
        /// Create a new User
        /// </summary>
        /// <param name="email">User email</param>
        /// <param name="password">User password</param>
        /// <param name="cpf">User CPF</param>
        /// <param name="name">Username</param>
        public User(string email, string password, string cpf, string name) : base(email, password)
        {
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
            List<Transaction> transactions, List<Ticket> tickets, List<PaymentForm> payment) : base(id, email, password)
        {
            Cpf = cpf;
            Name = name;
            Transactions = transactions;
            Tickets = tickets;
            Payments = payment;
        }

        /// <summary>
        /// Formats a CPF
        /// </summary>
        /// <param name="cpf">The CPF to format</param>
        /// <returns>A formated CPF</returns>
        public static string FormatCpf(string cpf)
        {
            return cpf.Trim().Replace(".", "").Replace("-", "");
        }

        /// <summary>
        /// Validates the CPF camp
        /// </summary>
        /// <param name="cpf">A CPF</param>
        /// <returns>true if the CPF is valid and false if not</returns>
        public static bool IsCpf(string cpf)
        {
            cpf = FormatCpf(cpf);

            if (cpf.Length != 11)
                return false;

            string tempCpf = cpf[..9];
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
