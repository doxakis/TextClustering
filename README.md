# TextClustering
A simple text clustering algorithm in c#.

It will add the extension method ClusterBy on IEnumerable<T>. You only need to specify which string property to use and some options.

# Supported framework
.NET Standard 2.0

# Install from Nuget
To get the latest version:

```
Install-Package TextClustering
```

# Example

Consider the following model:
```
public class Document
{
    public string Content { get; set; }
}
```

How to invoke it:
```
using TextClustering;

// ...

var documents = new List<Document>();
// Fill list of documents.

var result = documents.ClusterBy(document => document.Content, options => options
    .WithMinClusterSize(5) // The minimum cluster size (default value: 5, but you should change it)
    .WithMinWordLength(5) // The minimum word length
    .WithMaxPresencePercent(10) // The maximum overall presence in percent of one word among all text
    .UseCaching(true) // (optional, true by default. Will use more ram, but prevent redoing the same calculation multiple times)
    .WithMaxDegreeOfParallelism(Environment.ProcessorCount) // (optional, will use one thread by default)
    .WithLanguages(Language.English, Language.French) // (optional, will use English stop words if not specified) This is used to eliminate words that are so commonly used that they carry very little useful information.
);

// result.Unclassified // List<Document>
// result.Clusters // List<List<Document>>
```

**For more complete example, please see the project TextClusteringExample.**

# Copyright and license
Code released under the MIT license.