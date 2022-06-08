using DesafioMbLabs.Models.CustomAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace DesafioMbLabs.Models
{
    /// <summary>
    /// Represents a event manager
    /// </summary>
    public class EventManager : User
    {
        [NotMapped]
        public override string Role { get { return nameof(EventManager); } }

        [Required]
        [MinLength(14)]
        [MaxLength(18)]
        [CustomValidationCnpj]
        public string Cnpj { get; set; }

        [Required]
        [MinLength(8)]
        [MaxLength(64)]
        public string OrganizationName { get; set; }

        [JsonIgnore]
        [IgnoreDataMember]
        public List<Event> Events { get; set; }

        /// <summary>
        /// Create a void manager User
        /// </summary>
        public EventManager() : base()
        {
            Events = new();
        }

        /// <summary>
        /// Create a Manager User
        /// </summary>
        /// <param name="email">User email</param>
        /// <param name="password">User password</param>
        /// <param name="cpf">User CPF</param>
        /// <param name="name">Username</param>
        /// <param name="cnpj">Organization CNPJ</param>
        /// <param name="organizationName">Organization name</param>
        public EventManager(string email, string password, string cpf, string name, string cnpj, string organizationName)
            : base(email, password, cpf, name)
        {
            Cnpj = cnpj;
            OrganizationName = organizationName;
            Events = new();
        }

        public static string FormatCnpj(string cnpj)
        {
            return cnpj.Trim().Replace(".", "").Replace("-", "").Replace("/", "");
        }

        /// <summary>
        /// Valid if is a valid cnpj
        /// </summary>
        /// <param name="cnpj">CNPJ</param>
        /// <returns></returns>
        public static bool IsCnpj(string cnpj)
        {
            cnpj = FormatCnpj(cnpj);

            if (cnpj.Length != 14)
                return false;

            string tempCnpj = cnpj[..12];
            int sum = 0;

            int[] multiplier1 = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            for (int i = 0; i < 12; i++)
                sum += int.Parse(tempCnpj[i].ToString()) * multiplier1[i];

            int rest = (sum % 11);

            if (rest < 2)
                rest = 0;
            else
                rest = 11 - rest;

            string digit = rest.ToString();
            tempCnpj += digit;
            sum = 0;

            int[] multiplier2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            for (int i = 0; i < 13; i++)
                sum += int.Parse(tempCnpj[i].ToString()) * multiplier2[i];

            rest = (sum % 11);

            if (rest < 2)
                rest = 0;
            else
                rest = 11 - rest;

            digit += rest.ToString();
            return cnpj.EndsWith(digit);
        }
    }
}
