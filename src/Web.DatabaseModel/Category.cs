using System;
using System.Collections.Generic;
using System.Text;

namespace Web.DatabaseModel
{
    public class Category
    {
        public Guid CategoryId { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool? IsActive { get; set; }
    }
}
