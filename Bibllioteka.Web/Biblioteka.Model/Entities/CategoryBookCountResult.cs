using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteka.Model.Entities
{
    public class CategoryBookCountResult
    {
        public string CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int BookCount { get; set; } = 0;
    }
}
