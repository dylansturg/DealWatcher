using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DealWatcher.Models;

namespace DealWatcher.ProductSearch.ProductSource
{
    public interface IApiProduct
    {
        String UniqueIdentifier { get; }
    }
}
