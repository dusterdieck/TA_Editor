namespace TA_Editor
{
    public class Counter
    {
        public int successcounter { get; set; }
        public int outofrangecounter { get; set; }
        public Counter()
        {
            this.successcounter = 0;
            this.outofrangecounter = 0;
        }

        public void Merge(Counter other)
        {
            this.successcounter += other.successcounter;
            this.outofrangecounter += other.outofrangecounter;
        }
    }
}