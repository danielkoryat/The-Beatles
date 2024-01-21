using Basic_Project.interfaces;

namespace Basic_Project.Models
{
    public class Cart
    {
        public Dictionary<int, int> Items { get; set; } = new();
    }
}