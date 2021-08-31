using System;
using System.Collections.Generic;
using System.Linq;
using HdbscanSharp.Distance;
using HdbscanSharp.Runner;
using StopWord;

namespace TextClustering
{
    public class ClusteringResult<T>
    {
        public List<List<T>> Clusters { get; set; }
        public List<T> Unclassified { get; set; }
    }
    
    public static class TextClustering
    {
        public static ClusteringResult<T> ClusterBy<T>(this IEnumerable<T> items,
            Func<T, string> getTextFunc, Func<ClusterConfig, ClusterConfig> configFunc = null)
        {
            var config = new ClusterConfig();
            if (configFunc != null)
                _ = configFunc(config);

            var list = items as List<T> ?? items.ToList();
            var knownWords = ExtractExpressions(list.Select(getTextFunc), config);
            var vectors = list.Select(item => GenerateExpressionVector(knownWords, getTextFunc(item), config));

            var result = HdbscanRunner.Run(new HdbscanParameters<Dictionary<int, int>>
            {
                DataSet = vectors.ToArray(),
                MinPoints = config.MinClusterSize,
                MinClusterSize = config.MinClusterSize,
                DistanceFunction = new CosineSimilarity(config.CanUseCaching, config.MaxDegreeOfParallelism is > 1),
                MaxDegreeOfParallelism = config.MaxDegreeOfParallelism,
                CacheDistance = config.CanUseCaching
            });

            // Read results.
            var labels = result.Labels;
            var n = labels.Max();

            var clusteringResult = new ClusteringResult<T>
            {
                Clusters = new List<List<T>>(),
                Unclassified = new List<T>()
            };

            for (var iCluster = 0; iCluster <= n; iCluster++)
            {
                List<T> clustersItems = null;
                for (var i = 0; i < labels.Length; i++)
                {
                    if (labels[i] == iCluster)
                    {
                        var item = list[i];
                        if (clustersItems == null)
                            clustersItems = new List<T>();
                        clustersItems.Add(item);
                    }
                }

                if (clustersItems == null)
                    continue;
                
                if (iCluster == 0)
                    clusteringResult.Unclassified.AddRange(clustersItems);
                else
                    clusteringResult.Clusters.Add(clustersItems);
            }

            return clusteringResult;
        }

        private static Dictionary<string, Word> ExtractExpressions(IEnumerable<string> textContents,
            ClusterConfig config)
        {
            var knownWords = new Dictionary<string, Word>();
            var wordId = 1;
            var textContentsCount = 0;

            foreach (var textContent in textContents)
            {
                textContentsCount++;

                var wordsFreq = new Dictionary<string, int>();
                var words = textContent.Split(config.WordsSeparator, StringSplitOptions.RemoveEmptyEntries);

                foreach (var word in words)
                {
                    var key = word.ToLower();

                    if (key.Length < config.MinWordLength)
                        continue;

                    if (wordsFreq.ContainsKey(key))
                        wordsFreq[key]++;
                    else
                        wordsFreq.Add(key, 1);
                }

                foreach (var wordFreq in wordsFreq)
                {
                    if (knownWords.ContainsKey(wordFreq.Key))
                    {
                        knownWords[wordFreq.Key].NumberOfDocumentsWhereTheTermAppears++;
                        knownWords[wordFreq.Key].Freq += wordFreq.Value;
                    }
                    else
                        knownWords.Add(wordFreq.Key, new Word
                        {
                            Id = wordId++,
                            Text = wordFreq.Key,
                            NumberOfDocumentsWhereTheTermAppears = 1,
                            Freq = wordFreq.Value
                        });
                }
            }

            var maxPresenceCount = config.MaxPresencePercent * textContentsCount / 100;

            var stopWordList = GetStopWordList(config);

            knownWords = knownWords
                .Where(m => m.Value.NumberOfDocumentsWhereTheTermAppears >= 5)
                .Where(m => m.Value.NumberOfDocumentsWhereTheTermAppears <= maxPresenceCount)
                .Where(m => !stopWordList.Contains(m.Key))
                .OrderBy(x => x.Value.NumberOfDocumentsWhereTheTermAppears)
                .Take(config.MaxWordsUsed)
                .ToDictionary(x => x.Key, x => x.Value);

            return knownWords;
        }

        private static HashSet<string> GetStopWordList(ClusterConfig config)
        {
            if (config.Languages == null)
                return new HashSet<string>();

            var stopWordList = config.Languages
                .SelectMany(language => StopWords.GetStopWords(language.GetShortCode()).Select(x => x.ToLower()))
                .Distinct();
            
            return new HashSet<string>(stopWordList);
        }

        private static Dictionary<int, int> GenerateExpressionVector(Dictionary<string, Word> knownWords,
            string textContent, ClusterConfig config)
        {
            var freq = new Dictionary<string, int>();
            var words = textContent.Split(config.WordsSeparator, StringSplitOptions.RemoveEmptyEntries);

            foreach (var word in words)
            {
                var key = word.ToLower();

                if (!knownWords.ContainsKey(key))
                    continue;

                if (freq.ContainsKey(key))
                    freq[key]++;
                else
                    freq.Add(key, 1);
            }

            var vector = freq.ToDictionary(x => knownWords[x.Key].Id, x => x.Value);
            return vector;
        }
    }
}