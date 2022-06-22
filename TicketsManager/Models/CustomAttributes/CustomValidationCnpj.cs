using System.ComponentModel.DataAnnotations;

namespace TicketsManager.Models.CustomAttributes
{
    public class CustomValidationCnpj : ValidationAttribute
    {
        /// <summary>
        /// Construtor
        /// </summary>
        public CustomValidationCnpj() { }

        public override bool IsValid(object value)
        {
            return EventManager.IsCnpj(value.ToString());
        }
    }
}
