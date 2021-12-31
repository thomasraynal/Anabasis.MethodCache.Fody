﻿using System;
using System.Threading.Tasks;

namespace Anabasis.MethodCache.Samples.Model
{
    public class RessourceService : IRessourceService
    {
        [Cache(AbsoluteExpirationRelativeToNowInMilliseconds = 60*60*1000)]
        public Task<AvailableRessourcesDescriptor> GetAvailableRessourceIdsForRessourceType(string ressourceType)
        {
            var availableRessourcesDescriptor =

                new AvailableRessourcesDescriptor()
                {
                    Benchmarks = new[] { new BenchmarkDescriptor() { BenchmarkId = "CANDRIAM BONDS EURO CORPORATE" }, new BenchmarkDescriptor() { BenchmarkId = "CAC40 ETF" } },
                    Dates = new[] { DateTime.UtcNow, DateTime.UtcNow.AddDays(-1) },
                    Funds = new[]{new FundDescriptor() { BenchmarkId = "CANDRIAM BONDS EURO CORPORATE", FundId = "CANDRIAM LOW YIELD", FundType ="OBLIGATAIRE" },
                                   new FundDescriptor() { BenchmarkId = "CAC40 ETF", FundId = "CANDRIAM EURO EQUITIES", FundType ="ACTION" } }

                };

            return Task.FromResult(availableRessourcesDescriptor);
        }

        [Cache]
        public Task<string[]> GetAvailableRessourceTypes()
        {
            var ressourceTypes = new[] { "FundData", "BenchmarkData" };

            return Task.FromResult(ressourceTypes);
        }

        [Cache(SlidingExpirationInMilliseconds = 60 * 1000)]
        public Task<Ressource> GetDateUnboundRessource(GetDateUnboundRessourceRequest getDateUnboundRessourceRequest)
        {
            var ressource = new Ressource("CANDRIAM LOW YIELD", "BenchmarkData", null, Array.Empty<RessourceItem>());

            return Task.FromResult(ressource);
        }

        [Cache(SlidingExpirationInMilliseconds = 60 * 1000)]
        public Task<Ressource> GetRessource(GetRessourceRequest getRessourceRequest)
        {
            var ressource = new Ressource("CANDRIAM BONDS EURO CORPORATE", "FundData", DateTime.UtcNow, Array.Empty<RessourceItem>());

            return Task.FromResult(ressource);
        }

        [Cache]
        public Task<Ressource> GetRessourceRequestWithPredicate(GetRessourceRequestWithPredicateRequest getRessourceRequestWithPredicate)
        {
            var ressource = new Ressource("CANDRIAM EURO EQUITIES", "FundData", DateTime.UtcNow, Array.Empty<RessourceItem>());

            return Task.FromResult(ressource);
        }

        public Task<RessourceItem> PutRessourceItem(PutRessourceRequest putRessourceRequest)
        {
            var ressourceItem = new RessourceItem(new[]
            {
                new RessourceProperty("name","VOLKSWAGEN 5.375% 22/05/18"),
                new RessourceProperty("marketValue",3322993.97260274)
            });

            return Task.FromResult(ressourceItem);
        }
    }
}