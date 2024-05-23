# Caching With MediatR Powerful Pipeline Behavior

# Prerequisites

Knowledge of CQRS, working with MediatR, and in-memory or Redis caching

## Installation

You need to install the following packages:

- Bogus
- MediatR
- Microsoft.Extensions.Caching.Abstractions
- Microsoft.Extensions.Logging.Abstractions
- Microsoft.Extensions.Options
- Newtonsoft.Json
- MediatR.Extensions.Microsoft.DependencyInjection

## Description

Bogus is a tool that generates fake data, meaning each time we use it, it creates a new set of information and does not give us duplicate data. We can use this tool to test the caching of information.

In the application layer, we have a common folder that contains the basic caching requirements:

- The `CacheBehavior` class, which we cannot explain in detail here. You can read about its functionality.
- The `CacheSettings` class, which pertains to the configuration settings in the `appsettings` file. The `CacheEnable` property is for enabling or disabling the cache, and the `SlidingExpiration` property is used to set the expiration time.

In the `appsettings` file, we have a section like below:
```json
"CacheSettings": {
    "CacheEnable": false,
    "SlidingExpiration": 2
}
```

In this project, we implement a caching mechanism for MediatR queries. We have an interface named `ICacheableMediatrQuery` that all queries requiring caching should inherit from. Below is the definition of this interface:

```csharp
public interface ICacheableMediatrQuery
{
    bool BypassCache { get; }
    string CacheKey { get; }
    TimeSpan? SlidingExpiration { get; }
}
```

## Interface Properties

When a query implements the `ICacheableMediatrQuery` interface, three properties are implemented:

1. **BypassCache**: This property indicates whether to use the cache for this request.
2. **CacheKey**: The `CacheKey` property is for creating the cache key for this query so that we can fetch data from the cache using this key each time.
3. **SlidingExpiration**: The `SlidingExpiration` property is for setting how long the data should be stored in the cache.

## Example Implementation

In the `GetUserByIdQuery`, you can see that in addition to inheriting from `IRequest` of MediatR, it also implements the `ICacheableMediatrQuery` interface:

```csharp
public class GetUserByIdQuery : IRequest<UserViewModel>, ICacheableMediatrQuery
{
    public int UserId { get; set; }
    public string CacheKey => $"getuser_cachekey_{UserId}";
    public TimeSpan? SlidingExpiration { get; set; }
    public bool BypassCache { get; set; }
}
```

As you can see, the `CacheKey` is constructed based on the `UserId`. Now in the handler for the `GetUserByIdQuery`, we see that using Bogus, a new set of data is generated each time the handler is called and returned to you. If we cache the data, this information can be repeated until the expiration time ends, and the request may not reach the handler. You can test this by enabling and disabling `CacheEnable` in the configuration.
```csharp

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserViewModel>
{
    public async Task<UserViewModel> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        return await Task.FromResult(GetUserByIdMock(request.UserId));
    }

    private UserViewModel GetUserByIdMock(int userId)
    {
        return new Faker<UserViewModel>()
            .RuleFor(bp => bp.UserId, f => userId)
            .RuleFor(bp => bp.UserName, f => f.Name.FindName())
            .RuleFor(bp => bp.FirstName, f => f.Name.FirstName())
            .RuleFor(bp => bp.LastName, f => f.Name.LastName())
            .Generate();
    }
}
```