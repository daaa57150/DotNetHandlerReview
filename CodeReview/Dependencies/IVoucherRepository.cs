using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeReview
{
    public interface IVoucherRepository
    {
        Task AddVoucherAsync(Voucher voucher, CancellationToken cancellationToken);
        Task<Member> GetMemberAsync(Guid memberId, CancellationToken cancellationToken);
        Task<VoucherType> GetVoucherTypeAsync(Guid voucherTypeId, CancellationToken cancellationToken);
    }
}
