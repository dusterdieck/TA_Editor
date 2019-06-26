namespace TA_Editor
{
    public class Counter
    {
        public int SuccessCount { get; set; }
        public int OutOfRangeCount { get; set; }
        public Counter()
        {
            this.SuccessCount = 0;
            this.OutOfRangeCount = 0;
        }

        public void Merge(Counter other)
        {
            this.SuccessCount += other.SuccessCount;
            this.OutOfRangeCount += other.OutOfRangeCount;
        }
    }
}