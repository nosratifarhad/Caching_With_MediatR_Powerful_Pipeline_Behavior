using Application.Common.CacheCommon;
using Application.Features.Users.ViewModel;
using MediatR;

namespace Application.Features.Users.Queries.GetAllUsers;

public class GetAllUsersQuery : IRequest<IEnumerable<UserBriefViewModel>>, ICacheableMediatrQuery
{
    public string CacheKey => $"getallusers_cachekey";
    public TimeSpan? SlidingExpiration { get; set; }
    public bool BypassCache { get; set; }

}
