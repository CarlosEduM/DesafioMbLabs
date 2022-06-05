namespace DesafioMbLabs.Models
{
    public class PaymentForm
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual PaymentMethod Method { get { return PaymentMethod.MyTypeOfPayment; } }
    }
}
