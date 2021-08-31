namespace TextClustering
{
    public class Word
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int NumberOfDocumentsWhereTheTermAppears { get; set; }
        public int Freq { get; set; }
    }
}