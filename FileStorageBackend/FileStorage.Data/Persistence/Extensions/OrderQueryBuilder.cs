using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace FileStorage.Data.Persistence.Extensions
{
    public static class OrderQueryBuilder
    {
        public static string CreateOrderQuery<T>(this string orderByString)
        {
            var orderParams = orderByString.Trim().Split(',');
            var propertiesInfo = typeof(T).GetProperties(
                BindingFlags.Public | BindingFlags.Instance);

            var orderQueryBuilder = new StringBuilder();

            foreach (var param in orderParams)
            {
                if (string.IsNullOrWhiteSpace(param))
                    continue;

                var propertyFromQuery = param.Split(" ")[0];
                var objectProperty = propertiesInfo.FirstOrDefault(property =>
                    property.Name.Equals(propertyFromQuery,
                        System.StringComparison.InvariantCultureIgnoreCase));

                if (objectProperty == null)
                    continue;
                var direction = param.EndsWith(" desc") ? "descending" : "ascending";

                orderQueryBuilder.Append($"{objectProperty.Name.ToString()} {direction}, ");
            }

            var orderQuery = orderQueryBuilder.ToString().TrimEnd(',', ' ');

            return orderQuery;
        }
    }
}
