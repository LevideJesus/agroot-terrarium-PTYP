using Microsoft.EntityFrameworkCore;

namespace AgrootTerrarium.Api
{
    public class AgrootDbContext : DbContext
    {
        public AgrootDbContext(DbContextOptions<AgrootDbContext> options)
            :base (options){}
       
    }
}