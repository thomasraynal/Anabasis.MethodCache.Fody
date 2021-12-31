using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anabasis.MethodCache.Samples.Model
{
    public class PutRessourceRequest : BaseRessourceRequest
    {
        public RessourceProperty[] Properties { get; set; }

        public override string GetDescription()
        {
            return $"{RessourceType} - {RessourceId}";
        }
    }
}
