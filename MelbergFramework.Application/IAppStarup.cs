using Microsoft.Extensions.DependencyInjection;

namespace MelbergFramework.Application;

public interface IAppStartup 
{
    void ConfigureServices(IServiceCollection services);
}