using System.Data.Entity;

namespace ARM_RZA_v._1._0
{
    public class MGTOContext : DbContext
    {
        public MGTOContext() : base("DefaultConnection")
        { }
        
        public DbSet<Device> Devices { get; set; }
        public DbSet<DevType> DevTypes { get; set; }
        public DbSet<Prisoed> Prisoeds { get; set; }
        public DbSet<PS> PS { get; set; }
        public DbSet<TO> TOes { get; set; }
        public DbSet<T2020> T2020 { get; set; }
        public DbSet<T2021> T2021 { get; set; }
        public DbSet<T2022> T2022 { get; set; }
        public DbSet<T2023> T2023 { get; set; }
        public DbSet<T2024> T2024 { get; set; }
        public DbSet<T2025> T2025 { get; set; }
        public DbSet<Mgto> Mgtoes { get; set; }

    }
}
