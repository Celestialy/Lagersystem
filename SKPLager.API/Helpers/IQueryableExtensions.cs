using Microsoft.EntityFrameworkCore;
using SKPLager.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SKPLager.API.Helpers
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, string ordering, string direction)
        {
            direction = direction == "OrderBy" || direction == "OrderByDescending" ? direction : "OrderBy";
            var type = typeof(T);
            var property = type.GetProperty(ordering);
            var parameter = Expression.Parameter(type, "p");
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var orderByExp = Expression.Lambda(propertyAccess, parameter);
            MethodCallExpression resultExp = Expression.Call(typeof(Queryable), direction, new Type[] { type, property.PropertyType }, source.Expression, Expression.Quote(orderByExp));
            return source.Provider.CreateQuery<T>(resultExp);
        }

        public static IQueryable<T> TryOrderBy<T>(this IQueryable<T> source, string orderBy)
        {
            var orderByAfterSplit = orderBy.Split(',');
            if (orderByAfterSplit.Count() == 2)
            {
                for (int i = 0; i < orderByAfterSplit.Count(); i++)
                {
                    orderByAfterSplit[i] = orderByAfterSplit[i].Trim();
                }

                //var info = source.GetType().GetProperty(orderByAfterSplit[0]);
                var info = typeof(T).GetProperty(orderByAfterSplit[0]);
                if (info != null)
                {
                    if (orderByAfterSplit[1] == "OrderBy" || orderByAfterSplit[1] == "OrderByDescending")
                    {
                        return source.OrderBy(orderByAfterSplit[0], orderByAfterSplit[1]);
                    }
                }
                else
                {
                    info = typeof(InventoryItem).GetProperty("Item").PropertyType.GetProperty(orderByAfterSplit[0]);

                    if (info != null)
                    {
                        orderByAfterSplit[1] = orderByAfterSplit[1] == "OrderBy" || orderByAfterSplit[1] == "OrderByDescending" ? orderByAfterSplit[1] : "OrderBy";
                        var param = Expression.Parameter(typeof(InventoryItem), "x");
                        Expression body = param;
                        body = Expression.PropertyOrField(body, "Item");
                        body = Expression.PropertyOrField(body, orderByAfterSplit[0]);
                        var orderByExp = Expression.Lambda(body, param);
                        MethodCallExpression resultExp = Expression.Call(typeof(Queryable), orderByAfterSplit[1], new Type[] { typeof(T), info.PropertyType }, source.Expression, Expression.Quote(orderByExp));
                        return source.Provider.CreateQuery<T>(resultExp);
                    }
                }
            }
            return source;
        }


        public static IQueryable<InventoryItem> GetIncludes(this IQueryable<InventoryItem> source)
        {
            return source.Include(i => i.Item).ThenInclude(ic => ic.Category).Include(i => i.Item).ThenInclude(ii => ii.Image);
        }

        public static IQueryable<LoanItem> GetIncludes(this IQueryable<LoanItem> source)
        {
            return source.Include(i => i.Item).ThenInclude(ic => ic.Category).Include(i => i.Item).ThenInclude(ii => ii.Image);
        }

        public static IQueryable<ConsumptionItem> GetIncludes(this IQueryable<ConsumptionItem> source)
        {
            return source.Include(i => i.Item).ThenInclude(ic => ic.Category).Include(i => i.Item).ThenInclude(ii => ii.Image);
        }
    }
}
