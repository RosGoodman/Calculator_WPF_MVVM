#nullable disable

namespace Calculator_WPF_MVVM.Common.Models;

/// <summary> Модель истории вычислений. </summary>
public class OperationHistoryModel : BaseModel
{
    /// <summary> Вычисляемая строка. </summary>
    public string CalculatedString { get; set; }

    /// <summary> Результат вычисления. </summary>
    public string Result { get; set; }
}