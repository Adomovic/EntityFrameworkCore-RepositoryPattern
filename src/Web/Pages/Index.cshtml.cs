using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.DatabaseModel;
using Web.DatabaseModel.Repository;

namespace Web.Pages
{
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
}
