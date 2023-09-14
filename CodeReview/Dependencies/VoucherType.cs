namespace CodeReview
{
    public class VoucherType
    {
        public object Name { get; internal set; }
        public ExpirationType ExpirationType { get; internal set; }
        public double DaysValid { get; internal set; }
        public DateTime? BeginDate { get; internal set; }
    }
}
