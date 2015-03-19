using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DealWatcher.ProductSearch.ProductSource
{
    public interface IApiRequest
    {
        Task<IEnumerable<IApiResponse>> ExecuteAsync();
    }
}
