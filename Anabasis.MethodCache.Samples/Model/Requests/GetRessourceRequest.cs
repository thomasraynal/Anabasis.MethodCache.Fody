using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anabasis.MethodCache.Samples.Model
{
    public class GetRessourceRequest : BaseRessourceRequest
    {

        public DateTime? DateUtc { get; set; }

        public override string ToString()
        {
            return GetDescription();
        }

        public override string GetDescription()
        {
            return $"{RessourceType} - {RessourceId} - {DateUtc:yyyy-MM-ddTHH\\:mm\\:ss.fffffffzzz}";
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = RessourceType.GetHashCode();
                if (null != RessourceId) hashCode = (hashCode * 397) ^ RessourceId.GetHashCode();
                if (null != DateUtc) hashCode = (hashCode * 397) ^ DateUtc.GetHashCode();

                return hashCode;
            }
        }

        public override bool Equals(object obj)
        {
            return obj is GetRessourceRequest && GetHashCode() == (obj as GetRessourceRequest).GetHashCode();
        }
    }
}
