using Newtonsoft.Json;

namespace TextClusteringExample
{
    internal class News
    {
        public string Category { get; set; }
        public string Headline { get; set; }
        public string Date { get; set; }

        [JsonProperty("short_description")] public string ShortDescription { get; set; }
    }
}