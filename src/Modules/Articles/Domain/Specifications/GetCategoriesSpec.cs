using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Specification;

namespace Astrum.Articles.Specifications
{
    public class GetCategoriesSpec : Specification<Aggregates.Category>
    {
        public GetCategoriesSpec() 
        {
            Query
                .OrderBy(category => category.Name);
        }
    }

    public class GetCategoryByIdSpec : Specification<Aggregates.Category>
    {
        public GetCategoryByIdSpec(Guid categoryId) 
        {
            Query
                .Where(category => category.Id == categoryId);
        }
    }

    public class GetOtherCategorySpec : Specification<Aggregates.Category>
    {
        public GetOtherCategorySpec()
        {
            Query
                .Where(category => category.Name == "Другое");
        }
    }
}
