using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anabasis.MethodCache.Samples.Model
{
    public class GetRessourceRequestWithPredicateRequest : BaseRequest
    {
        public Dictionary<string, string> Predicates { get; set; }

        public override string GetDescription()
        {
            return $"{RessourceType} - {string.Join(",", Predicates.Select((kv) => $"[{kv.Key},{kv.Value}]").ToArray())}";
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = RessourceType.GetHashCode();
                if (null != Predicates) hashCode = (hashCode * 397) ^ Predicates.GetHashCode();

                return hashCode;
            }
        }

        public override bool Equals(object obj)
        {
            return obj is GetRessourceRequestWithPredicateRequest && GetHashCode() == (obj as GetRessourceRequestWithPredicateRequest).GetHashCode();
        }
    }
}
