using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anabasis.MethodCache.Samples.Model
{
    public class GetDateUnboundRessourceRequest : BaseRessourceRequest
    {
        public override string GetDescription()
        {
            return $"{RessourceType} - {RessourceId}";
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = RessourceType.GetHashCode();
                if (null != RessourceId) hashCode = (hashCode * 397) ^ RessourceId.GetHashCode();

                return hashCode;
            }
        }

        public override bool Equals(object obj)
        {
            return obj is GetDateUnboundRessourceRequest && GetHashCode() == (obj as GetDateUnboundRessourceRequest).GetHashCode();
        }
    }
}
