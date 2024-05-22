using MediatR;
using Bogus;
using Application.Features.Users.ViewModel;

namespace Application.Features.Users.Queries.GetAllUsers;

public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, IEnumerable<UserBriefViewModel>>
{
    public async Task<IEnumerable<UserBriefViewModel>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        return await Task.FromResult(GetAllUsersMock());
    }

    private IEnumerable<UserBriefViewModel> GetAllUsersMock()
       => new Faker<UserBriefViewModel>()
         .RuleFor(bp => bp.UserId, f => f.IndexFaker)
         .RuleFor(bp => bp.UserName, f => f.Name.FindName()).Generate(100).ToList();
}
