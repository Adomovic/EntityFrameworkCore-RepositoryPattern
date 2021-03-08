# Entity Framework Core with Repository Pattern

This project illustrates how to use Repository Pattern with Entity Framework Core

## Installation

Download the project

## Project Structure
The project has two main projects:
- Web 
  - An Asp.net core Razorpages project
- Web.DatabaseModel
  - A .net Standard library project, which host all database models with the **Repository Pattern**

## Web.DatabaseModel/Repository/GenericRepository.cs interface

```c#
public interface IGenericRepository<T> where T : class
    {
        Task<T> FindAsync(object id);

        Task<List<T>> GetAsync(Expression<Func<T, bool>> predicate);

        Task<List<T>> GetAsync(Expression<Func<T, bool>> predicate,
            params Expression<Func<T, object>>[] includeProperties);

        Task<List<T>> GetAllAsync();

        Task<List<T>> GetAllAsync(params Expression<Func<T, object>>[] includeProperties);

        Task InsertAsync(T objectValue);

        Task UpdateAsync(T objectValue);

        Task DeleteAsync(object id);

        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, bool asNoTracking = true);

        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate,
            params Expression<Func<T, object>>[] includeProperties);

        Task<int> GetCountAsync(Expression<Func<T, bool>> predicate);

        Task<PagedRecords<T>> GetPaginated(Expression<Func<T, bool>> predicate = null,
            Expression<Func<T, object>> orderBy = null,
            Expression<Func<T, object>> orderByDescending = null,
            int page = 1, int pageSize = 20);
    }
```


## Web.DatabaseModel/Repository/GenericRepository.cs implementation

```c#
public class GenericRepository<T> : IGenericRepository<T>
        where T : class
    {
        protected readonly WebDataContext Context;

        public GenericRepository(WebDataContext context)
        {
            Context = context;
        }

        public async Task<T> FindAsync(object id)
        {
            return await Context.Set<T>().FindAsync(id);
        }

        public async Task<List<T>> GetAsync(Expression<Func<T, bool>> predicate)
        {
            return await Context.Set<T>().Where(predicate).AsNoTracking().ToListAsync();
        }

        public async Task<List<T>> GetAsync(Expression<Func<T, bool>> predicate,
            params Expression<Func<T, object>>[] includeProperties)
        {

            IQueryable<T> query = Context.Set<T>();
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            return await query.Where(predicate).AsNoTracking().ToListAsync();
        }

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, bool asNoTracking = true)
        {
            if(asNoTracking)
                return await Context.Set<T>().AsNoTracking().FirstOrDefaultAsync(predicate);

            return await Context.Set<T>().FirstOrDefaultAsync(predicate);
        }

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate,
            params Expression<Func<T, object>>[] includeProperties)
        {

            IQueryable<T> query = Context.Set<T>();
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            return await query.AsNoTracking().FirstOrDefaultAsync(predicate);
        }

        public async Task<int> GetCountAsync(Expression<Func<T, bool>> predicate)
        {
            return await Context.Set<T>().Where(predicate).AsNoTracking().CountAsync();
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await Context.Set<T>().AsNoTracking().ToListAsync();
        }

        public async Task<List<T>> GetAllAsync(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = Context.Set<T>();
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            return await query.AsNoTracking().ToListAsync();
        }

        public async Task InsertAsync(T objectValue)
        {
            Context.Set<T>().Add(objectValue);
            await Context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T objectValue)
        {
            Context.Entry(objectValue).State = EntityState.Modified;
            await Context.SaveChangesAsync();
        }

        public async Task DeleteAsync(object id)
        {
            var objectValue = await FindAsync(id);

            if (objectValue != null)
            {
                Context.Set<T>().Remove(objectValue);
                await Context.SaveChangesAsync();
            }
        }

        public async Task<PagedRecords<T>> GetPaginated(Expression<Func<T, bool>> predicate = null,
            Expression<Func<T, object>> orderBy = null,
            Expression<Func<T, object>> orderByDescending = null,
            int page = 1, int pageSize = 20)
        {
            IQueryable<T> query = Context.Set<T>();

            if (predicate != null)
                query = query.Where(predicate);
           

            var pageIndex = page - 1;
            var totalRecords = await query.CountAsync();
            var totalPages = Convert.ToInt32(Math.Ceiling((decimal)totalRecords / pageSize));
            List<T> list;
            if(orderBy != null)
            {
                list = await query
                .OrderBy(orderBy)
                .Skip(pageIndex * pageSize).Take(pageSize)
                .AsNoTracking()
                .ToListAsync();
            }
            else if(orderByDescending != null)
            {
                list = await query
                    .OrderByDescending(orderByDescending)
                    .Skip(pageIndex * pageSize).Take(pageSize)
                    .AsNoTracking()
                    .ToListAsync();
            }
            else
            {
                list =await  query
                    .Skip(pageIndex * pageSize).Take(pageSize)
                    .AsNoTracking()
                    .ToListAsync();
            }

            return new PagedRecords<T>
            {
                TotalPages = totalPages,
                Records = list,
                Page = page,
                PageSize = pageSize,
                TotalRecords = totalRecords
            };
        }
    }
```


## Model Class making use of GenericRepository
```c#
public class ProductRepository : GenericRepository<Product>
    {
        public ProductRepository(WebDataContext context) : base(context)
        {
        }
    }
```

## Usage of ProductRepository in a webpage
```c#
public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ProductRepository _productRepository;
        private int _pageSize = 20;

        public PagedRecords<Product> PagedProducts { get; set; }

        public IndexModel(ILogger<IndexModel> logger, ProductRepository productRepository)
        {
            _logger = logger;
            _productRepository = productRepository;
        }

        public async Task<IActionResult> OnGetAsync(int pageId)
        {
            PagedProducts = await _productRepository.GetPaginated(c => c.Type.ToLower() == "retail",
                c => c.Name,
                null,
                pageId,
                _pageSize);

            return Page();
        }
    }
```

