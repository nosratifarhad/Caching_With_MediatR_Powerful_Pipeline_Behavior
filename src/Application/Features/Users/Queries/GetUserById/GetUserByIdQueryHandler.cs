using Application.Features.Users.ViewModel;
using Bogus;
using MediatR;

namespace Application.Features.Users.Queries.GetUserById;

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserViewModel>
{
    public async Task<UserViewModel> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        return await Task.FromResult(GetUserByIdMock(request.UserId));
    }

    private UserViewModel GetUserByIdMock(int userId)
       => new Faker<UserViewModel>()
         .RuleFor(bp => bp.UserId, f => userId)
         .RuleFor(bp => bp.UserName, f => f.Name.FindName())
         .RuleFor(bp => bp.FirstName, f => f.Name.FirstName())
         .RuleFor(bp => bp.FirstName, f => f.Name.FullName());
}
