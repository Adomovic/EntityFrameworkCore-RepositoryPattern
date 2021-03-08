namespace Web.DatabaseModel.Repository
{
    public class CategoryRepository : GenericRepository<Category>
    {
        public CategoryRepository(WebDataContext context) : base(context)
        {
        }
    }
}
