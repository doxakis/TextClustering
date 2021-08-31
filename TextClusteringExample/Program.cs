using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using TextClustering;

namespace TextClusteringExample
{
    internal static class Program
    {
        private static void Main()
        {
            // Dataset is from: https://www.kaggle.com/rmisra/news-category-dataset
            var lines = File.ReadLines("News_Category_Dataset_v2.json")
                .OrderBy(_ => Guid.NewGuid())
                .Take(1000);
            var news = lines.Select(JsonConvert.DeserializeObject<News>);

            var clusters = news.ClusterBy(x => x.Headline, options => options
                .WithMinClusterSize(5)
                .WithMinWordLength(5)
                .WithMaxPresencePercent(10)
                .UseCaching(true)
                .WithMaxDegreeOfParallelism(Environment.ProcessorCount)
                .WithLanguages(Language.English, Language.French)
            );

            Console.WriteLine("Unclassified: " + clusters.Unclassified.Count);
            Console.WriteLine("Number of clusters: " + clusters.Clusters.Count);
            
            Console.WriteLine();
            Console.WriteLine("Press enter to continue...");
            Console.ReadLine();

            var clusterId = 1;
            foreach (var cluster in clusters.Clusters)
            {
                Console.WriteLine("Cluster #" + clusterId);
                Console.WriteLine();
                Console.WriteLine("Categories: " + string.Join(", ",
                    cluster.GroupBy(x => x.Category).Select(x => x.Key + " (" + x.Count() + ")")));
                Console.WriteLine("Total: " + cluster.Count);
                Console.WriteLine();
                foreach (var item in cluster)
                    Console.WriteLine(item.Category + " | " + item.Headline);
                Console.WriteLine();
                Console.WriteLine("Press enter to continue...");
                Console.ReadLine();

                clusterId++;
            }

            Console.WriteLine("End of the program.");
            Console.WriteLine("Press enter to continue...");
            Console.ReadLine();
        }
    }
}