namespace Main_Project.interfaces
{
    public interface IShoppingItem
    {
        public int Id { get; set; }
        public string Photo { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
    }
}