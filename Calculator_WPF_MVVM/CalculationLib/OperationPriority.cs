
namespace CalculationLib;

/// <summary> Приоритеты операций. </summary>
internal record OperationPriority
{
    /// <summary> Словарь приоритетов операций вычисления. </summary>
    /// <remarks> Приоритет назначен по возрастающей, где "1" будет выполняться в последнюю очередь. </remarks>
    internal Dictionary<char, int> Priority { get; } = new()
    {
        { '+', 1 },
        { '-', 1 },
        { '*', 2 },
        { '/', 2 },
        { '^', 3 },
    };
}