using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Sakura.AspNetCore;

namespace Astrum.SharedLib.Application.Extensions
{
    public static class PagedListExtension
    {
        public static IPagedList<TDestination> ToMappedPagedList<TSource, TDestination>(this IPagedList<TSource> list,
            IMapper mapper, int page, int pageSize)
        {
            var blazorModels = mapper.Map<List<TSource>, List<TDestination>>(list.ToList());

            IPagedList<TDestination> pagedList = 
                new PagedList<List<TDestination>, TDestination>
                    (blazorModels, blazorModels, pageSize, page, list.TotalCount, list.TotalPage);

            return pagedList;
        }
    }
}
