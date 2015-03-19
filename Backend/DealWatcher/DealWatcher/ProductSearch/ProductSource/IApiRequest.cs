using System.Collections.Generic;
using System.Threading.Tasks;

namespace DealWatcher.ProductSearch.ProductSource
{
    public interface IApiRequest
    {
        Task<IEnumerable<IApiResponse>> ExecuteAsync();
    }
}
