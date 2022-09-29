using Microsoft.Extensions.DependencyInjection;

namespace Melberg.Application;

public interface IAppStartup 
{
    void ConfigureServices(IServiceCollection services);
}