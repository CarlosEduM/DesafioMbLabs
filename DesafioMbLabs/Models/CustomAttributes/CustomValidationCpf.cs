using System.ComponentModel.DataAnnotations;

namespace DesafioMbLabs.Models.CustomAttributes
{
    public class CustomValidationCpf : ValidationAttribute
    {
        /// <summary>
        /// Construtor
        /// </summary>
        public CustomValidationCpf() { }

        public override bool IsValid(object value)
        {
            return User.IsCpf(value.ToString());
        }
    }
}
