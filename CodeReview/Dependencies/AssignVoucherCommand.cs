using MediatR;

namespace CodeReview
{
    public class AssignVoucherCommand : IRequest
    {
        public Guid MemberId { get; internal set; }
        public Guid VoucherTypeId { get; internal set; }
    }
}