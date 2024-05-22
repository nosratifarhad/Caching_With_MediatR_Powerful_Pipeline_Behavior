using Application.Common.CacheCommon;
using Application.Features.Users.ViewModel;
using MediatR;

namespace Application.Features.Users.Queries.GetUserById;

public class GetUserByIdQuery : IRequest<UserViewModel>, ICacheableMediatrQuery
{
    public int UserId { get; set; }
    public string CacheKey => $"getuser_cachekey_{UserId}";
    public TimeSpan? SlidingExpiration { get; set; }
    public bool BypassCache { get; set; }
}
