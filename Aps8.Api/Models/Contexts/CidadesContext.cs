using Microsoft.EntityFrameworkCore;

namespace Aps8.Api.Models.Contexts
{
    public class CidadesContext : DbContext
    {
        public CidadesContext(DbContextOptions<CidadesContext> options)
            : base(options)
        { }

        public DbSet<Cidade> Cidades { get; set; }
    }
}
