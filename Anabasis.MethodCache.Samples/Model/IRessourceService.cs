using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anabasis.MethodCache.Samples.Model
{
    public interface IRessourceService
    {

        Task<Ressource> GetRessource(GetRessourceRequest getRessourceRequest);
        Task<Ressource> GetDateUnboundRessource(GetDateUnboundRessourceRequest getDateUnboundRessourceRequest);
        Task<AvailableRessourcesDescriptor> GetAvailableRessourcesForRessourceType(string ressourceType);
        Task<string[]> GetAvailableRessourceTypes();
        Task<RessourceItem> PutRessourceItem(PutRessourceRequest putRessourceRequest, Guid? extraParameter);
        Task<Ressource> GetRessourceRequestWithPredicate(GetRessourceRequestWithPredicateRequest getRessourceRequestWithPredicate);
        IEnumerable<string> GetAvailableRessourceTypesSynchronous();
    }
}
