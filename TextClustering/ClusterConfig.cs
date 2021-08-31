namespace TextClustering
{
    public class ClusterConfig
    {
        internal char[] WordsSeparator { get; private set; } =
        {
            ' ', '.', '?', '\n', '\r', '\t', ',', '!', '(', ')', ';', '"', ':', '-', '\\', '/', '[', ']', '{', '}', '0',
            '1', '2', '3', '4', '5', '6', '7', '8', '9', '\''
        };

        internal int MinWordLength { get; private set; } = 5;
        internal int MaxPresencePercent { get; private set; } = 25;
        internal int MinClusterSize { get; private set; } = 5;
        internal bool CanUseCaching { get; private set; } = true;
        internal int MaxDegreeOfParallelism { get; private set; } = 1;
        internal Language[] Languages { get; private set; } = {Language.English};
        public int MaxWordsUsed { get; private set; } = int.MaxValue;

        public ClusterConfig WithWordsSeparator(params char[] value)
        {
            WordsSeparator = value;
            return this;
        }

        public ClusterConfig WithMinWordLength(int value)
        {
            MinWordLength = value;
            return this;
        }

        public ClusterConfig WithMaxPresencePercent(int value)
        {
            MaxPresencePercent = value;
            return this;
        }

        public ClusterConfig WithMinClusterSize(int value)
        {
            MinClusterSize = value;
            return this;
        }

        public ClusterConfig UseCaching(bool value)
        {
            CanUseCaching = value;
            return this;
        }

        public ClusterConfig WithMaxDegreeOfParallelism(int value)
        {
            MaxDegreeOfParallelism = value;
            return this;
        }

        public ClusterConfig WithLanguages(params Language[] values)
        {
            Languages = values;
            return this;
        }

        public ClusterConfig WithMaxWordsUsed(int value)
        {
            MaxWordsUsed = value;
            return this;
        }
    }
}