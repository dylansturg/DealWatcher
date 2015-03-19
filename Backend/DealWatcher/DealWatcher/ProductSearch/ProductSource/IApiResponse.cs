using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DealWatcher.ProductSearch.ProductSource
{
    public interface IApiResponse
    {
        IEnumerable<IApiProduct> ParsedProducts { get; }
        Task ParseResultsAsync(String responseMessagea);
    }
}
