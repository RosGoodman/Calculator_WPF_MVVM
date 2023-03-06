using Calculator_WPF_MVVM.Common.DataContext;
using Calculator_WPF_MVVM.Common.Models;
using System.Collections.Generic;
using System.Linq;

namespace Calculator_WPF_MVVM.Common.Repositories;

/// <summary> Интерфейс репозитория для <see cref="ScanListModel"/>. </summary>
public interface IOperationHistoryModelRepository : IRepository<OperationHistoryModel> { }

/// <summary> Репозиторий для <see cref="OperationHistoryModel"/>. </summary>
public class OperationHistoryModelRepository : IOperationHistoryModelRepository
{
    private readonly ContextDB _contextDB;

    /// <summary> Ctor. </summary>
    public OperationHistoryModelRepository()
    {
        _contextDB = new ContextDB();
    }

    ///
    /// <inheritdoc cref="IRepository{T}.Add"/>
    public void Add(OperationHistoryModel entity)
    {
        if (entity is null) return;
        _contextDB.Add(entity);
        _contextDB.SaveChanges();
    }

    ///
    /// <inheritdoc cref="IRepository{T}.DeleteAll"/>
    public void DeleteAll()
    {
        _contextDB.OperationHistory.RemoveRange(_contextDB.OperationHistory);
        _contextDB.SaveChanges();
    }

    ///
    /// <inheritdoc cref="IRepository{T}.GetAll"/>
    public List<OperationHistoryModel> GetAll()
    {
        return _contextDB.OperationHistory.ToList();
    }
}
