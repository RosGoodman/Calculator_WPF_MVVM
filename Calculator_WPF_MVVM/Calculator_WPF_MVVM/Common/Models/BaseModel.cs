
using System;
using System.ComponentModel.DataAnnotations;

namespace Calculator_WPF_MVVM.Common.Models;

/// <summary> Базовый класс моделей. </summary>
public class BaseModel
{
    /// <summary> Id in DB. </summary>
    [Key]
    public Guid Id { get; set; }
}