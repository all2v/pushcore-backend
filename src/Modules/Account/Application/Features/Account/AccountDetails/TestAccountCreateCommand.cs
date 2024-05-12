using Astrum.SharedLib.Common.CQS.Implementations;

namespace Astrum.Account.Features.Account.AccountDetails;

public class TestAccountCreateCommand : CommandResult
{
}
//
// public sealed class TestAccountCreateCommandHandler : CommandHandler<TestAccountCreateCommand>
// {
//     private readonly IAccountEntityRepository _accountEntityRepository;
//     private readonly IApplicationUserAccessor _applicationUserAccessor;
//     private readonly UserManager<ApplicationUser> _userManager;
//
//     public TestAccountCreateCommandHandler(
//         UserManager<ApplicationUser> userManager, IAccountEntityRepository accountEntityRepository,
//         IApplicationUserAccessor applicationUserAccessor
//     )
//     {
//         _userManager = userManager;
//         _accountEntityRepository = accountEntityRepository;
//         _applicationUserAccessor = applicationUserAccessor;
//     }
//
//     public override async Task<Result> Handle(TestAccountCreateCommand command,
//         CancellationToken cancellationToken = default)
//     {
//         var guidExample = Guid.NewGuid().ToString();
//         var user = new User(guidExample, guidExample + "@email.com", guidExample)
//         {
//             EmailConfirmed = true,
//             Culture = "rus"
//         };
//         user.ActivateUser();
//         var alice = new ApplicationUser
//         {
//             Name = Guid.NewGuid().ToString(),
//             UserName = Guid.NewGuid().ToString(),
//             Email = Guid.NewGuid() + "@email.com",
//             EmailConfirmed = true,
//             Culture = "rus",
//             IsActive = true
//         };
//         var result = await _userManager.CreateAsync(alice, "Pass123$");
//         // await _signInManager.SignInAsync(alice, false);
//         // var user = await _userManager.GetUserAsync();
//
//         var account = new AccountEntity
//         {
//             UserId = alice.Id,
//             CreatedBy = alice.UserName,
//             AvatarUrl = "sadasdsa"
//         };
//         await _accountEntityRepository.AddAsync(account, cancellationToken);
//         await _accountEntityRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
//
//         // var a = await _accountEntityRepository.FindAsync(account.Id,cancellationToken);
//         // var b = await _dbContext.Accounts.Where(x => x.Id == account.Id)
//         // .Include(c => c.User)
//         // .FirstOrDefaultAsync(cancellationToken);
//         return Result.Success();
//     }
// }