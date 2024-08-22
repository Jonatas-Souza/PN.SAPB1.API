using Microsoft.EntityFrameworkCore;
using PN.SAPB1.API.Models;

namespace PN.SAPB1.API.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<BusinessPartners> BusinessPartners { get; set; }
        public DbSet<Bpaddress> Bpaddresses { get; set; }
        public DbSet<Bpfiscaltaxidcollection> Bpfiscaltaxidcollections { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Bpaddress>()
                .HasKey(bpa => new { bpa.BPCode, bpa.AddressName });

            modelBuilder.Entity<Bpfiscaltaxidcollection>()
                .HasKey(bpf => new { bpf.BPCode, bpf.Address });

            modelBuilder.Entity<BusinessPartners>()
                .HasMany(x => x.BPAddresses)
                .WithOne(bpa => bpa.BusinessPartner)
                .HasForeignKey(bpa => bpa.BPCode)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<BusinessPartners>()
                .HasMany(bp => bp.BPFiscalTaxIDCollection)
                .WithOne(bpf => bpf.BusinessPartner)
                .HasForeignKey(bpf => bpf.BPCode)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }


    }
}
