namespace Web.DatabaseModel.Repository
{
    public class ProductRepository : GenericRepository<Product>
    {
        public ProductRepository(WebDataContext context) : base(context)
        {
        }
    }
}
