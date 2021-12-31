using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anabasis.MethodCache.Samples.Model
{
    public class Ressource
    {
 
        internal Ressource()
        {
        }

        public string RessourceId { get; }
        public string RessourceType { get; }
        public DateTime? RessourceTimestamp { get; }

        public Ressource(RessourceItem[] ressourceObjects)
        {
            RessourceObjects = ressourceObjects;
        }

        public Ressource(string ressourceId, string ressourceType, DateTime? ressourceDate, RessourceItem[] ressourceObjects)
        {
            RessourceId = ressourceId;
            RessourceType = ressourceType;
            RessourceObjects = ressourceObjects;
            RessourceTimestamp = ressourceDate;
        }

        [Required]
        public RessourceItem[] RessourceObjects { get; set; }

    }

    public class RessourceItem
    {

        internal RessourceItem()
        {
        }

        public RessourceItem(RessourceProperty[] properties)
        {
            Properties = properties;
        }

        public string Id => Properties.FirstOrDefault(property => property.Key == "uniqueId")?.Value as string;

        public string Name => Properties.FirstOrDefault(property => property.Key == "name")?.Value as string;

        [Required]
        public RessourceProperty[] Properties { get; set; }
    }

    public class RessourceProperty
    {
        internal RessourceProperty()
        {
        }

        public RessourceProperty(string key, object value)
        {
            Key = key;
            Value = value;
        }

        [Required]
        public string Key { get; set; }

        [Required]
        public object Value { get; set; }
    }

    public class BenchmarkDescriptor
    {
        public string BenchmarkId { get; set; }
    }

    public class FundDescriptor
    {
        public string FundId { get; set; }
        public string BenchmarkId { get; set; }
        public string FundType { get; set; }
    }

    public class AvailableRessourcesDescriptor
    {
        public FundDescriptor[] Funds { get; set; }
        public BenchmarkDescriptor[] Benchmarks { get; set; }
        public DateTime[] Dates { get; set; }
    }
}
