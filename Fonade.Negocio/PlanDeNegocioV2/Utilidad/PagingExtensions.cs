using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fonade.Negocio.PlanDeNegocioV2.Utilidad
{
    public static class PagingExtensions
    {
        //used by LINQ to SQL
        //public static IQueryable<TSource> Paginacion<TSource>(this IQueryable<TSource> source, int page, int pageSize)
        //{
        //    return source.Skip((page - 1) * pageSize).Take(pageSize);
        //}

        ////used by LINQ
        //public static IEnumerable<TSource> Paginacion<TSource>(this IEnumerable<TSource> source, int page, int pageSize)
        //{
        //    return source.Skip((page - 1) * pageSize).Take(pageSize);
        //}
    }
}
