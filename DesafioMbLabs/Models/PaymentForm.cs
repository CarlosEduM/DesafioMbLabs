namespace DesafioMbLabs.Models
{
    public class PaymentForm
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual PaymentMethod Method { get { return PaymentMethod.MyTypeOfPayment; } }

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
        public PaymentForm(string name) => Name = name;

        /// <summary>
        /// Create a new Payment
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        public PaymentForm(int id, string name) => (Id, Name) = (id, name);
    }
}
