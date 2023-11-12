using Aps8.Api.Models.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Aps8.Api.Configurations;

public static class DataBaseConfiguration
{
    public static void ConfigureServices(this IServiceCollection? services, IConfiguration configuration)
    {
        if (services == null) return;

        services.AddDbContext<CidadesContext>(options =>
            options.UseInMemoryDatabase("aps8_db"));

        services.AddMvc();
    }
}
