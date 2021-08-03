using System;
using MBD.Transactions.Domain.Enumerations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace MBD.Transactions.API.Configuration
{
    public class TransactionTypeRouteConstraint : IRouteConstraint
    {
        public bool Match(HttpContext httpContext, IRouter route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {
            var type = values[routeKey]?.ToString();
            return Enum.TryParse(type, out TransactionType transactionType);
        }
    }
}