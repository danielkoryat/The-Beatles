using Main_Project.Validations;

namespace Main_Project.Models
{
    public class Purchase
    {
        public int Id { get; set; }

        [FullNameValidationAttribute]
        public string FullName { get; set; } = string.Empty;

        [GreaterThanZeroAttribute(ErrorMessage = "The cart is empty")]
        public double Amount { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;
    }
}