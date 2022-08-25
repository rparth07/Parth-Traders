namespace Parth_Traders.Domain.Entity.Admin
{
    public class Category
    {
        public long CategoryId { get; set; }

        public string CategoryName { get; set; }

        public string CategoryDescription { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
