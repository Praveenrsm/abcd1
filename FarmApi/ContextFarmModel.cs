using FarmTradeDataLayer;
using FarmTradeEntity;
using Microsoft.EntityFrameworkCore;
using FarmApi.Model;
namespace FarmApi
{
    public class ContextFarmModel : DbContext
    {
        public ContextFarmModel()
        {
        }

        public ContextFarmModel(DbContextOptions<ContextFarmModel> options) : base(options)
        {

        }
        public DbSet<Product1> products { get; set; }
        // SQL Connection:
        protected override void OnConfiguring(DbContextOptionsBuilder dbContextOptionsBuilder)
        {
            dbContextOptionsBuilder.UseSqlServer("Server=IN-6YYZFY3;Database=Products;User Id=sa;Password=Praveen0077$$$$;TrustServerCertificate=True;");
        }
    }
}