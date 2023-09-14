using MediatR;
using System.Text.Json;

namespace CodeReview
{

    public class AssignVoucherService : IRequestHandler<AssignVoucherCommand>
    {
        private readonly IVoucherRepository vrep;
        private readonly IUnitOfWork unitOfWork;
        private readonly HttpClient cli;

        public AssignVoucherService(IVoucherRepository vrep, IUnitOfWork unitOfWork, HttpClient cli)
        {
            this.vrep = vrep;
            this.unitOfWork = unitOfWork;
            this.cli = cli;
            this.cli.BaseAddress = new Uri("https://dummy.calculator.local/");
        }

        async Task IRequestHandler<AssignVoucherCommand>.Handle(AssignVoucherCommand command, CancellationToken cancellationToken)
        {
            var mb = await vrep.GetMemberAsync(command.MemberId, cancellationToken);
            var vt = await vrep.GetVoucherTypeAsync(command.VoucherTypeId, cancellationToken);

            // Calculate voucher value
            var response = await cli.GetAsync(
                $"/calculate-voucher-value?email={mb.Email}&voucherType={vt.Name}",
                cancellationToken);

            response.EnsureSuccessStatusCode();

            await using var responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
            var value = await JsonSerializer.DeserializeAsync<int>(responseStream, cancellationToken: cancellationToken);

            // Calculate expiration date
            DateTime dateExpiring;

            switch (vt.ExpirationType)
            {
                case ExpirationType.Assignment:
                    dateExpiring = DateTime.Today.AddDays(vt.DaysValid);
                    break;
                case ExpirationType.Fixed:
                    dateExpiring = vt.BeginDate?.AddDays(vt.DaysValid) ?? throw new InvalidOperationException();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            // Assign voucher
            var voucher = new Voucher
            {
                Id = 1234,
                MemberAssigned = mb,
                Type = vt,
                Value = value,
                DateExpiring = dateExpiring
            };

            mb.AssignedVouchers.Add(voucher);
            mb.NumberOfActiveOffers++;

            await this.vrep.AddVoucherAsync(voucher, cancellationToken);
            await this.unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
