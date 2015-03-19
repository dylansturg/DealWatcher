﻿using System.Web.Http.OData;
using System.Web.Http.OData.Query;

namespace DealWatcher.Filters
{
    public class EnableRestrictedQuery : EnableQueryAttribute
    {
        public EnableRestrictedQuery()
        {
            AllowedFunctions = AllowedFunctions.None;
            AllowedArithmeticOperators = AllowedArithmeticOperators.None;
            AllowedLogicalOperators = AllowedLogicalOperators.None;
            AllowedQueryOptions = AllowedQueryOptions.OrderBy | AllowedQueryOptions.Skip | AllowedQueryOptions.Top |
                                  AllowedQueryOptions.Select;
        }
    }
}