namespace Main_Project.Models
{
    public class CartItem<T>
    {
        public int ItemId { get; set; }
        public int Quantity { get; set; }
        public T Item { get; set; }
    }
}