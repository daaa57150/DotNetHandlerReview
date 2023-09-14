namespace CodeReview
{
    public class Voucher
    {
        public int Id { get; set; }
        public object MemberAssigned { get; set; }
        public object Type { get; set; }
        public object Value { get; set; }
        public DateTime DateExpiring { get; set; }
    }
}