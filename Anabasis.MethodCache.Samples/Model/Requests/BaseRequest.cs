using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anabasis.MethodCache.Samples.Model
{
    public abstract class BaseRequest : IRequest
    {
        public string RessourceType { get; set; }

        public string NormalizedRessourceType => RessourceType.ToLowerInvariant();

        public void Format(StringBuilder stringBuilder)
        {
            stringBuilder.Append(NormalizedRessourceType);
            stringBuilder.Append("-");
            stringBuilder.Append(GetHashCode());
        }

        public abstract string GetDescription();

        public override string ToString()
        {
            return GetDescription();
        }
    }
}
