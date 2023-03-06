
using Calculator_WPF_MVVM.Common.DataContext;
using Calculator_WPF_MVVM.Common.Models;
using System.Collections.Generic;
using System.Linq;

namespace Calculator_WPF_MVVM.Common.Repositories;

/// <summary> Интерфейс репозитория для <see cref="LogStringModelRepository"/>. </summary>
public interface ILogStringModelRepository : IRepository<LogModel> { }

/// <summary> Репозиторий для <see cref="LogModel"/>. </summary>
public class LogStringModelRepository : ILogStringModelRepository
{
    private readonly ContextDB _contextDB;

    /// <summary> Ctor. </summary>
    public LogStringModelRepository()
    {
        _contextDB = new ContextDB();
    }

    ///
    /// <inheritdoc cref="IRepository{T}.Add"/>
    public void Add(LogModel entity)
    {
        if (entity is null) return;
        _contextDB.Add(entity);
        _contextDB.SaveChanges();
    }

    ///
    /// <inheritdoc cref="IRepository{T}.DeleteAll"/>
    public void DeleteAll()
    {
        _contextDB.Logs.RemoveRange(_contextDB.Logs);
        _contextDB.SaveChanges();
    }

    ///
    /// <inheritdoc cref="IRepository{T}.GetAll"/>
    public List<LogModel> GetAll()
    {
        return _contextDB.Logs.ToList();
    }
}
