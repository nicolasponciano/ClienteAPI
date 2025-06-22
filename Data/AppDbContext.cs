using Microsoft.EntityFrameworkCore;
using ClienteAPI_.Models;

namespace ClienteAPI_.Context
{
    public class AppDbContext : DbContext
    {
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Contato> Contatos { get; set; }
        public DbSet<Endereco> Enderecos { get; set; }
        public DbSet<ViaCep> T_VIACEP { get; set; } 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cliente>()
                .ToTable("T_CLIENTES"); 

            modelBuilder.Entity<Cliente>()
                .HasKey(c => c.Id);

            modelBuilder.Entity<Cliente>()
                .Property(c => c.Id)
                .HasColumnName("CLI_ID") 
                .ValueGeneratedOnAdd(); 

            modelBuilder.Entity<Cliente>()
                .Property(c => c.Nome).HasColumnName("CLI_NOME");

            modelBuilder.Entity<Cliente>()
                .Property(c => c.DataCadastro).HasColumnName("CLI_DATA_CADASTRO");

            modelBuilder.Entity<Cliente>()
                .HasOne(c => c.Contato)
                .WithOne(co => co.ContatoCliente) 
                .HasForeignKey<Contato>(co => co.ClienteId)
                .OnDelete(DeleteBehavior.Cascade); 

            modelBuilder.Entity<Cliente>()
                .HasOne(c => c.Endereco)
                .WithOne(e => e.EnderecoCliente) 
                .HasForeignKey<Endereco>(e => e.ClienteId)
                .OnDelete(DeleteBehavior.Cascade); 

            modelBuilder.Entity<Contato>()
                .ToTable("T_CONTATO"); 

            modelBuilder.Entity<Contato>()
                .HasKey(co => co.Id); 

            modelBuilder.Entity<Contato>()
                .Property(co => co.Id)
                .HasColumnName("CTT_ID") 
                .ValueGeneratedOnAdd(); 

            modelBuilder.Entity<Contato>()
                .Property(co => co.Tipo).HasColumnName("CTT_TIPO");

            modelBuilder.Entity<Contato>()
                .Property(co => co.Texto).HasColumnName("CTT_TEXTO");

            modelBuilder.Entity<Contato>()
                .Property(co => co.ClienteId).HasColumnName("CTT_ID_CLIENTE"); 

            modelBuilder.Entity<Endereco>()
                .ToTable("T_ENDERECO"); 

            modelBuilder.Entity<Endereco>()
                .HasKey(e => e.Id); 

            modelBuilder.Entity<Endereco>()
                .Property(e => e.Id)
                .HasColumnName("END_ID") 
                .ValueGeneratedOnAdd(); 

            modelBuilder.Entity<Endereco>()
                .Property(e => e.Cep).HasColumnName("END_CEP");

            modelBuilder.Entity<Endereco>()
                .Property(e => e.Logradouro).HasColumnName("END_LOGRADOURO");

            modelBuilder.Entity<Endereco>()
                .Property(e => e.Cidade).HasColumnName("END_CIDADE");

            modelBuilder.Entity<Endereco>()
                .Property(e => e.Numero).HasColumnName("END_NUMERO");

            modelBuilder.Entity<Endereco>()
                .Property(e => e.Complemento).HasColumnName("END_COMPLEMENTO");

            modelBuilder.Entity<Endereco>()
                .Property(e => e.ClienteId).HasColumnName("END_ID_CLIENTE");

            modelBuilder.Entity<ViaCep>()
                .ToTable("T_VIACEP"); 

            modelBuilder.Entity<ViaCep>()
                .HasKey(v => v.Cep); 

            modelBuilder.Entity<ViaCep>()
                .Property(v => v.Cep)
                .HasColumnName("VIA_CEP")
                .HasMaxLength(9) 
                .IsRequired();

            modelBuilder.Entity<ViaCep>()
                .Property(v => v.Logradouro).HasColumnName("VIA_LOGRADOURO");

            modelBuilder.Entity<ViaCep>()
                .Property(v => v.Complemento).HasColumnName("VIA_COMPLEMENTO");

            modelBuilder.Entity<ViaCep>()
                .Property(v => v.Bairro).HasColumnName("VIA_BAIRRO");

            modelBuilder.Entity<ViaCep>()
                .Property(v => v.Localidade).HasColumnName("VIA_LOCALIDADE");

            modelBuilder.Entity<ViaCep>()
                .Property(v => v.Uf).HasColumnName("VIA_UF");

            modelBuilder.Entity<ViaCep>()
                .Property(v => v.Ibge).HasColumnName("VIA_IBGE");

            modelBuilder.Entity<ViaCep>()
                .Property(v => v.Gia).HasColumnName("VIA_GIA");

            modelBuilder.Entity<ViaCep>()
                .Property(v => v.Ddd).HasColumnName("VIA_DDD");

            modelBuilder.Entity<ViaCep>()
                .Property(v => v.Siafi).HasColumnName("VIA_SIAFI");
        }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    }
}