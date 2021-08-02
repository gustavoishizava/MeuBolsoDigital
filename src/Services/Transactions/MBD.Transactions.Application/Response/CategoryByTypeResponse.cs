using System.Collections.Generic;

namespace MBD.Transactions.Application.Response
{
    public class CategoryByTypeResponse
    {
        public List<CategoryWithSubCategoriesResponse> Income { get; set; }
        public List<CategoryWithSubCategoriesResponse> Expense { get; set; }

        public CategoryByTypeResponse(
            List<CategoryWithSubCategoriesResponse> income,
            List<CategoryWithSubCategoriesResponse> expense)
        {
            Income = income;
            Expense = expense;
        }
    }
}