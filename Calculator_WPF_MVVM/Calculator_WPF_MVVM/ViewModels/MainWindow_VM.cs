
using CalculationLib;
using Calculator_WPF_MVVM.Commands;
using Calculator_WPF_MVVM.Common.DataContext;
using Calculator_WPF_MVVM.Common.Models;
using Calculator_WPF_MVVM.Common.Repositories;
using Microsoft.Extensions.Configuration;
using NLog;
using System;
using System.ComponentModel;
using System.IO;

namespace PastingProductivityCalculationProgram.ViewModel.ViewModels.Windows;

/// <summary> ViewModel для главного окна. </summary>
public class MainWindow_VM : ViewModelBase, INotifyPropertyChanged
{
    //контейнеры в задании не упоминались, так же отсутствуют другие окна и страницы
    //по этому сделал агрегацию
    private readonly StackCalculation _stackCalculation = new ();
    private readonly ContextDB _contextDB = new();
    private readonly ILogger _logger;

    private readonly IOperationHistoryModelRepository _operationHistoryModelRepository;
    private readonly ILogStringModelRepository _logStringModelRepository;

    private string _calculatedString = string.Empty;

    #region Properties

    public string Lable { get; set; } = "MyLable";

    /// <summary> Вычисляемая строка. </summary>
    public string CalculatedString
    {
        get => _calculatedString;
        set
        {
            _calculatedString = value;
            OnPropertyChanged(nameof(CalculatedString));
        }
    }

    #endregion

    /////////////////////////////////////////////////////////////////////////////////

    #region CommandProperties

    /// <summary> Команда. Произвести вычисления. </summary>
    public RelayCommand CalculateCommand { get; private set; }

    #endregion

    /////////////////////////////////////////////////////////////////////////////////

    /// <summary> Ctor. </summary>
    public MainWindow_VM(ILogger logger)
    {
        _operationHistoryModelRepository = new OperationHistoryModelRepository();
        _logStringModelRepository = new LogStringModelRepository();
        _logger = logger;
        _logger.Info($"Логгер встроен в {nameof(MainWindow_VM)}");

        CalculateCommand = new RelayCommand(CalculateCommand_Execute, CalculateCommand_CanExecute);
    }

    /////////////////////////////////////////////////////////////////////////////////

    #region CommandMethods


    /// <summary> Проверить возможность выполнить команду "Произвести вычисления". </summary>
    /// <param name="param"> Параметр. </param>
    /// <returns> Результат операции. </returns>
    private bool CalculateCommand_CanExecute(object param) => true;

    /// <summary> Выполнить команду "Произвести вычисления". </summary>
    /// <param name="param"> Параметр. </param>
    private void CalculateCommand_Execute(object param)
    {
        _logger.Info($"{nameof(CalculateCommand_Execute)}");
        var operationHistoryModel = new OperationHistoryModel()
        {
            CalculatedString = _calculatedString,
        };

        CalculatedString = _stackCalculation.Calculate(_calculatedString);
        operationHistoryModel.Result = CalculatedString;

        _operationHistoryModelRepository.Add(operationHistoryModel);
    }

    #endregion

    /////////////////////////////////////////////////////////////////////////////////

    #region ViewModelMethods


    #endregion
}