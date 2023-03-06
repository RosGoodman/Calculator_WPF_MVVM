
using System.Collections;
using System.Collections.Generic;

namespace Calculator_WPF_MVVM.Common.Repositories;

/// <summary> Интерфейс репозиториев. </summary>
/// <typeparam name="T"></typeparam>
public interface IRepository<T> where T : class
{
    /// <summary> Добавить экземпляр модели в БД. </summary>
    /// <param name="entity"> Добавляемый экземпляр. </param>
    public void Add(T entity);

    /// <summary> Получить список всех экземпляров. </summary>
    /// <returns> Список экземпляров. </returns>
    public List<T> GetAll();

    /// <summary> Удалить все экземпляры из таблицы. </summary>
    public void DeleteAll();
}
