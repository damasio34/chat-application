using ChatApplication.Core.Domain.Repositories;
using ChatApplication.Core.Domain.Services;
using ChatApplication.Core.Repositories;
using ChatApplication.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ChatApplication.API.Extensions
{
    public static class IoCExtension
    {
        public static IServiceCollection AddIoC(this IServiceCollection services)
        {
            return services
                .AddSingleton<ITokenService, TokenService>()
                .AddSingleton<IUserRepository, UserRepository>()
                .AddSingleton<IUserAppService, UserAppService>();
        }
    }
}
