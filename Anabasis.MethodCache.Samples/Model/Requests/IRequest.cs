using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anabasis.MethodCache.Samples.Model
{
    public interface IRequest : IMemoryCacheFormattable
    {
        string GetDescription();
        string NormalizedRessourceType { get; }
    }


}
