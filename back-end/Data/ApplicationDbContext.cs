using Microsoft.EntityFrameworkCore;
using back_end.Models;

namespace back_end.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options
    ) : base(options)
    {
    }

    public DbSet<Usuario> Usuarios { get; set; }

    public DbSet<Tarefa> Tarefas { get; set; }
}