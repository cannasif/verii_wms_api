using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Reflection;
using WMS_WEBAPI.DTOs;
 
namespace WMS_WEBAPI.Services
{
    public static class QueryHelper
    {
        public static readonly string[] CommonSearchableColumns =
        {
            "Name",
            "Title",
            "Description",
            "Code",
            "CustomerCode",
            "CustomerName",
            "TaxOffice",
            "TaxNumber",
            "TcknNumber",
            "SalesRepCode",
            "GroupCode",
            "Email",
            "Website",
            "Phone1",
            "Phone2",
            "Address",
            "City",
            "District",
            "CountryCode",
            "ErpStockCode",
            "StockName",
            "Unit",
            "UreticiKodu",
            "GrupKodu",
            "GrupAdi",
            "Kod1",
            "Kod1Adi",
            "Kod2",
            "Kod2Adi",
            "Kod3",
            "Kod3Adi",
            "Kod4",
            "Kod4Adi",
            "Kod5",
            "Kod5Adi",
            "Username",
            "Email",
            "FirstName",
            "LastName",
            "FullName",
            "RoleNavigation.Title"
        };

        private static string ResolveColumnName(string column, IReadOnlyDictionary<string, string>? columnMapping)
        {
            if (columnMapping == null) return column;
            var mappingKey = columnMapping.Keys.FirstOrDefault(k => string.Equals(k, column, StringComparison.OrdinalIgnoreCase));
            return mappingKey != null ? columnMapping[mappingKey] : column;
        }

        private static (Expression expression, PropertyInfo property)? ResolvePropertyPath(Expression param, Type rootType, string path)
        {
            var parts = path.Split('.');
            Expression current = param;
            PropertyInfo? prop = null;
            Type currentType = rootType;

            foreach (var part in parts)
            {
                prop = currentType.GetProperty(part, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (prop == null) return null;
                current = Expression.Property(current, prop);
                currentType = prop.PropertyType;
            }

            return prop == null ? null : (current, prop);
        }

        public static IQueryable<T> ApplySearch<T>(
            this IQueryable<T> query,
            string? search,
            params string[] searchableColumns)
        {
            if (string.IsNullOrWhiteSpace(search) || searchableColumns.Length == 0)
            {
                return query;
            }

            var terms = search
                .Trim()
                .ToLowerInvariant()
                .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            if (terms.Length == 0)
            {
                return query;
            }

            var parameter = Expression.Parameter(typeof(T), "x");
            Expression? searchPredicate = null;

            foreach (var term in terms)
            {
                Expression? termPredicate = null;
                var searchValue = Expression.Constant(term);

                foreach (var column in searchableColumns)
                {
                    var resolved = ResolvePropertyPath(parameter, typeof(T), column);
                    if (resolved == null || resolved.Value.property.PropertyType != typeof(string))
                    {
                        continue;
                    }

                    var member = resolved.Value.expression;
                    var notNull = Expression.NotEqual(member, Expression.Constant(null, typeof(string)));
                    var toLower = Expression.Call(member, typeof(string).GetMethod(nameof(string.ToLower), Type.EmptyTypes)!);
                    var contains = Expression.Call(
                        toLower,
                        typeof(string).GetMethod(nameof(string.Contains), new[] { typeof(string) })!,
                        searchValue);
                    var currentPredicate = Expression.AndAlso(notNull, contains);

                    termPredicate = termPredicate == null
                        ? currentPredicate
                        : Expression.OrElse(termPredicate, currentPredicate);
                }

                if (termPredicate == null)
                {
                    continue;
                }

                searchPredicate = searchPredicate == null
                    ? termPredicate
                    : Expression.AndAlso(searchPredicate, termPredicate);
            }

            if (searchPredicate == null)
            {
                return query;
            }

            var lambda = Expression.Lambda<Func<T, bool>>(searchPredicate, parameter);
            return query.Where(lambda);
        }

        public static IQueryable<T> ApplyFilters<T>(
            this IQueryable<T> query,
            List<Filter>? filters,
            string filterLogic = "and",
            IReadOnlyDictionary<string, string>? columnMapping = null)
        {
            ParameterExpression param = Expression.Parameter(typeof(T), "x");
            Expression? basePredicate = null;

            var isDeletedProperty = typeof(T).GetProperty("IsDeleted");
            if (isDeletedProperty != null && (isDeletedProperty.PropertyType == typeof(bool) || isDeletedProperty.PropertyType == typeof(bool?)))
            {
                var isDeletedLeft = Expression.Property(param, isDeletedProperty);
                basePredicate = Expression.Equal(isDeletedLeft, Expression.Constant(false));
            }

            if (filters == null || filters.Count == 0)
            {
                if (basePredicate == null) return query;
                var defaultLambda = Expression.Lambda<Func<T, bool>>(basePredicate, param);
                return query.Where(defaultLambda);
            }

            bool useOr = string.Equals(filterLogic, "or", StringComparison.OrdinalIgnoreCase);
            Expression? filterPredicate = null;

            foreach (var filter in filters)
            {
                if (string.IsNullOrEmpty(filter.Value)) continue;

                var columnName = ResolveColumnName(filter.Column, columnMapping);
                var resolved = ResolvePropertyPath(param, typeof(T), columnName);
                if (resolved == null) continue;

                var (left, property) = resolved.Value;
                Expression? exp = null;
                var operatorLower = filter.Operator.ToLowerInvariant();

                if (property.PropertyType == typeof(string))
                {
                    var notNull = Expression.NotEqual(left, Expression.Constant(null, typeof(string)));
                    var method = operatorLower switch
                    {
                        "contains" => typeof(string).GetMethod("Contains", new[] { typeof(string) }),
                        "startswith" => typeof(string).GetMethod("StartsWith", new[] { typeof(string) }),
                        "endswith" => typeof(string).GetMethod("EndsWith", new[] { typeof(string) }),
                        _ => null
                    };
                    if (method != null)
                    {
                        exp = Expression.AndAlso(notNull, Expression.Call(left, method, Expression.Constant(filter.Value)));
                    }
                    else
                    {
                        exp = Expression.Equal(left, Expression.Constant(filter.Value));
                    }
                }
                else if (property.PropertyType == typeof(int) || property.PropertyType == typeof(int?))
                {
                    if (int.TryParse(filter.Value, out int val))
                    {
                        exp = operatorLower switch
                        {
                            ">" or "gt" => Expression.GreaterThan(left, Expression.Constant(val)),
                            ">=" or "gte" => Expression.GreaterThanOrEqual(left, Expression.Constant(val)),
                            "<" or "lt" => Expression.LessThan(left, Expression.Constant(val)),
                            "<=" or "lte" => Expression.LessThanOrEqual(left, Expression.Constant(val)),
                            _ => Expression.Equal(left, Expression.Constant(val))
                        };
                    }
                }
                else if (property.PropertyType == typeof(long) || property.PropertyType == typeof(long?))
                {
                    if (long.TryParse(filter.Value, out long val))
                    {
                        exp = operatorLower switch
                        {
                            ">" or "gt" => Expression.GreaterThan(left, Expression.Constant(val)),
                            ">=" or "gte" => Expression.GreaterThanOrEqual(left, Expression.Constant(val)),
                            "<" or "lt" => Expression.LessThan(left, Expression.Constant(val)),
                            "<=" or "lte" => Expression.LessThanOrEqual(left, Expression.Constant(val)),
                            _ => Expression.Equal(left, Expression.Constant(val))
                        };
                    }
                }
                else if (property.PropertyType == typeof(decimal) || property.PropertyType == typeof(decimal?))
                {
                    if (decimal.TryParse(filter.Value, out decimal val))
                    {
                        exp = operatorLower switch
                        {
                            ">" or "gt" => Expression.GreaterThan(left, Expression.Constant(val)),
                            ">=" or "gte" => Expression.GreaterThanOrEqual(left, Expression.Constant(val)),
                            "<" or "lt" => Expression.LessThan(left, Expression.Constant(val)),
                            "<=" or "lte" => Expression.LessThanOrEqual(left, Expression.Constant(val)),
                            _ => Expression.Equal(left, Expression.Constant(val))
                        };
                    }
                }
                else if (property.PropertyType == typeof(DateTime) || property.PropertyType == typeof(DateTime?))
                {
                    if (DateTime.TryParse(filter.Value, out DateTime val))
                    {
                        exp = operatorLower switch
                        {
                            ">" or "gt" => Expression.GreaterThan(left, Expression.Constant(val)),
                            ">=" or "gte" => Expression.GreaterThanOrEqual(left, Expression.Constant(val)),
                            "<" or "lt" => Expression.LessThan(left, Expression.Constant(val)),
                            "<=" or "lte" => Expression.LessThanOrEqual(left, Expression.Constant(val)),
                            _ => Expression.Equal(left, Expression.Constant(val))
                        };
                    }
                }
                else if (property.PropertyType == typeof(bool) || property.PropertyType == typeof(bool?))
                {
                    if (bool.TryParse(filter.Value, out bool val))
                    {
                        exp = Expression.Equal(left, Expression.Constant(val));
                    }
                }
                else if (property.PropertyType.IsEnum)
                {
                    if (Enum.TryParse(property.PropertyType, filter.Value, true, out var enumVal))
                    {
                        exp = Expression.Equal(left, Expression.Constant(enumVal));
                    }
                }

                if (exp != null)
                {
                    filterPredicate = filterPredicate == null
                        ? exp
                        : useOr
                            ? Expression.OrElse(filterPredicate, exp)
                            : Expression.AndAlso(filterPredicate, exp);
                }
            }

            Expression? finalPredicate;
            if (basePredicate != null && filterPredicate != null)
            {
                finalPredicate = Expression.AndAlso(basePredicate, filterPredicate);
            }
            else
            {
                finalPredicate = basePredicate ?? filterPredicate;
            }

            if (finalPredicate == null) return query;
            var lambda = Expression.Lambda<Func<T, bool>>(finalPredicate, param);
            return query.Where(lambda);
        }

        public static IQueryable<T> ApplySorting<T>(
            this IQueryable<T> query,
            string sortBy,
            bool desc,
            IReadOnlyDictionary<string, string>? columnMapping = null)
        {
            if (string.IsNullOrWhiteSpace(sortBy))
            {
                sortBy = "Id";
            }

            sortBy = ResolveColumnName(sortBy, columnMapping);
            var parameter = Expression.Parameter(typeof(T), "x");
            var resolved = ResolvePropertyPath(parameter, typeof(T), sortBy);
            if (resolved == null)
            {
                resolved = ResolvePropertyPath(parameter, typeof(T), "Id");
                if (resolved == null) return query;
            }

            var (member, _) = resolved.Value;
            var keySelector = Expression.Lambda(
                typeof(Func<,>).MakeGenericType(typeof(T), member.Type),
                member,
                parameter
            );
            var methodName = desc ? "OrderByDescending" : "OrderBy";
            var call = Expression.Call(
                typeof(Queryable),
                methodName,
                new[] { typeof(T), member.Type },
                query.Expression,
                keySelector
            );
            return query.Provider.CreateQuery<T>(call);
        }

        public static IQueryable<T> ApplyPagination<T>(this IQueryable<T> query, int page, int pageSize)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 20;
            int skip = (page - 1) * pageSize;
            return query.Skip(skip).Take(pageSize);
        }

        public static IQueryable<T> ApplyPagedRequest<T>(this IQueryable<T> query, PagedRequest request, IReadOnlyDictionary<string, string>? columnMapping = null)
        {
            if (request == null) return query;

            query = query.ApplySearch(request.Search, CommonSearchableColumns);
            query = query.ApplyFilters(request.Filters, request.FilterLogic, columnMapping);
            bool desc = string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase)
                || string.Equals(request.SortDirection, "descending", StringComparison.OrdinalIgnoreCase);
            query = query.ApplySorting(request.SortBy ?? "Id", desc, columnMapping);
            query = query.ApplyPagination(request.PageNumber, request.PageSize);
            return query;
        }

        public static ApiResponse<PagedResponse<T>> ToPagedResponse<T>(
            this ApiResponse<IEnumerable<T>> source,
            PagedRequest? request,
            params string[] searchableColumns)
        {
            if (!source.Success)
            {
                return ApiResponse<PagedResponse<T>>.ErrorResult(
                    source.Message,
                    source.ExceptionMessage,
                    source.StatusCode,
                    source.Errors.FirstOrDefault());
            }

            request ??= new PagedRequest();
            request.Filters ??= new List<Filter>();

            var query = (source.Data ?? Enumerable.Empty<T>());
            var columns = searchableColumns.Length > 0
                ? searchableColumns
                : typeof(T)
                    .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(property => property.PropertyType == typeof(string))
                    .Select(property => property.Name)
                    .ToArray();

            query = query
            .AsQueryable()
                .ApplySearch(request.Search, columns)
                .ApplyFilters(request.Filters, request.FilterLogic);

            bool desc = string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase)
                || string.Equals(request.SortDirection, "descending", StringComparison.OrdinalIgnoreCase);

            query = query.AsQueryable().ApplySorting(request.SortBy ?? "Id", desc);

            var pageNumber = Math.Max(request.PageNumber, 0);
            var pageSize = request.PageSize <= 0 ? 20 : request.PageSize;
            var totalCount = query.Count();
            var items = query
                .Skip(pageNumber * pageSize)
                .Take(pageSize)
                .ToList();

            return ApiResponse<PagedResponse<T>>.SuccessResult(
                new PagedResponse<T>(items, totalCount, pageNumber, pageSize),
                source.Message);
        }
    }
}
