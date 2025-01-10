using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
namespace LabaONITSasha.Models
{
    public class CatContext : DbContext
    {
        public DbSet<CatModel> Cats { get; set; }

        public CatContext(DbContextOptions<CatContext> options) : base(options)
        {
        }
    }


}
