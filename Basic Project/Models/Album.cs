using Basic_Project.interfaces;

namespace Basic_Project.Models
{
    public class Album : IShoppingItem
    {
        public int Id { get; set; }
        public string Photo { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
    }
}