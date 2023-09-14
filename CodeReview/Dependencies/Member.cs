namespace CodeReview
{
    public class Member
    {
        public object Email { get; internal set; }
        public List<Voucher> AssignedVouchers { get; internal set; }
        public int NumberOfActiveOffers { get; internal set; }
    }
}
