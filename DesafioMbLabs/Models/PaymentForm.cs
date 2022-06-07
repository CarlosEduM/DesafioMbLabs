using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DesafioMbLabs.Models
{
    /// <summary>
    /// Type of payment
    /// </summary>
    [Table("PaymentForms")]
    public class PaymentForm
    {
        [Key]
        [Column("PaymentFormId")]
        public int Id { get; set; }

        [Required]
        [MinLength(5)]
        [MaxLength(64)]
        public string Name { get; set; }

        [Required]
        [EnumDataType(typeof(PaymentMethod))]
        public virtual PaymentMethod Method { get { return PaymentMethod.MyTypeOfPayment; } }

        [ForeignKey("UserId")]
        public User Owner { get; set; }

        /// <summary>
        /// Create a void payment form
        /// </summary>
        public PaymentForm()
        {

        }

        /// <summary>
        /// Create a new Payment
        /// </summary>
        /// <param name="name"></param>
        public PaymentForm(string name, User owner) => (Name, Owner) = (name, owner);

        /// <summary>
        /// Create a new Payment
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        public PaymentForm(int id, string name, User owner) => (Id, Name, Owner) = (id, name, owner);
    }
}
