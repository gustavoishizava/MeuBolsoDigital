using System.Collections.Generic;

namespace MBD.Transactions.Application.Response
{
    public class CategoryWithSubCategoriesResponse : CategoryResponse
    {
        public List<CategoryResponse> SubCategories { get; set; }
    }
}