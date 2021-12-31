using Anabasis.MethodCache.Samples.Model;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anabasis.MethodCache.Samples
{
    class Program
    {

        static async Task Main(string[] _)
        {

            var ressourceService = new RessourceService();

            Console.WriteLine($"Call=> {nameof(ressourceService.GetAvailableRessourceForRessourceType)}");

            var availableDescriptors = await ressourceService.GetAvailableRessourceForRessourceType("someRessource");

            var keys = await CachingServices.Backend.GetKeys();

            Console.WriteLine($"keys=> {string.Join("-", keys)}");

            var availableDescriptorsTaskCachedValue = await CachingServices.Backend.GetValue<Task<AvailableRessourcesDescriptor>>(keys.First());

            Console.WriteLine($"cached task value=> IsCompleted: {availableDescriptorsTaskCachedValue.IsCompleted}");

            Console.WriteLine($"cached value=> {Environment.NewLine} {JsonConvert.SerializeObject(availableDescriptorsTaskCachedValue.Result)}");

            var getRessourceRequest = new GetRessourceRequest()
            {
                DateUtc = DateTime.UtcNow,
                RessourceId = "SOME RESSOURCE ID",
                RessourceType ="SOME RESSOURCE TYPE"
            };

            Console.WriteLine($"Call=> {nameof(ressourceService.GetRessource)}");

            var ressource = await ressourceService.GetRessource(getRessourceRequest);

            keys = await CachingServices.Backend.GetKeys();

            Console.WriteLine($"keys=> {string.Join("-", keys)}");

            var ressourceTaskCachedValue = await CachingServices.Backend.GetValue<Task<Ressource>>(keys.First(key=> key.Contains("GetRessource")));

            Console.WriteLine($"cached task value=> IsCompleted: {ressourceTaskCachedValue.IsCompleted}");

            Console.WriteLine($"cached value=> {Environment.NewLine} {JsonConvert.SerializeObject(ressourceTaskCachedValue.Result)}");

            Console.WriteLine($"clearing cache=> {nameof(CachingServices.Backend.Clear)}");

            await CachingServices.Backend.Clear();

            keys = await CachingServices.Backend.GetKeys();

            Console.WriteLine($"keys=> {string.Join("-", keys)}");
        }
    }
}
