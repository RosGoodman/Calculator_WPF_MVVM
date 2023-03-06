#nullable disable

using System;

namespace Calculator_WPF_MVVM.Common.Models;

/// <summary> Модель для записи ошибок в БД. </summary>
public class LogModel : BaseModel
{
    public DateTime Date { get; set; }

    public string Level { get; set; }

    public string Message { get; set; }

    public string Machinename { get; set; }

    public string Logger { get; set; }
}