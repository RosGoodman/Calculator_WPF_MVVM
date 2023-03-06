
using Calculator_WPF_MVVM.Common.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Calculator_WPF_MVVM.Common.DataContext;

public class ContextDB : DbContext
{
    public DbSet<OperationHistoryModel> OperationHistory { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string connStr = "Filename=Database.db";

        optionsBuilder.UseSqlite(connStr, option =>
        {
            option.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
        });
        base.OnConfiguring(optionsBuilder);
    }
}