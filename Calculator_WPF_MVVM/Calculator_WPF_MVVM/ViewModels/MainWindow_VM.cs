
using CalculationLib;
using Calculator_WPF_MVVM.Commands;
using Calculator_WPF_MVVM.Common.DataContext;
using Calculator_WPF_MVVM.Common.Models;
using Calculator_WPF_MVVM.Common.Repositories;
using Serilog;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace PastingProductivityCalculationProgram.ViewModel.ViewModels.Windows;

/// <summary> ViewModel для главного окна. </summary>
public class MainWindow_VM : ViewModelBase, INotifyPropertyChanged
{
    //контейнеры в задании не упоминались, так же отсутствуют другие окна и страницы
    //по этому сделал агрегацию
    private readonly StackCalculation _stackCalculation = new ();
    private readonly ContextDB _contextDB = new();

    private readonly IOperationHistoryModelRepository _operationHistoryModelRepository;

    private string _calculatedString = string.Empty;
    private ObservableCollection<OperationHistoryModel> _historyCollection = new();

    #region Properties

    /// <summary> Lable. </summary>
    public string Lable { get; } = "Calculator";

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

    /// <summary> Коллекция истории вычислений. </summary>
    public ObservableCollection<OperationHistoryModel> HistoryCollection
    {
        get => _historyCollection;
        private set
        {
            _historyCollection = value;
            OnPropertyChanged(nameof(HistoryCollection));
        }
    }

    #endregion

    /////////////////////////////////////////////////////////////////////////////////

    #region CommandProperties

    /// <summary> Команда. Произвести вычисления. </summary>
    public RelayCommand CalculateCommand { get; private set; }

    /// <summary> Команда. Загрузить историю вычислений. </summary>
    public RelayCommand DownloadHistoryCommand { get; private set; }

    /// <summary> Команда. Удалить историю вычислений. </summary>
    public RelayCommand DeleteHistoryCommand { get; private set; }

    #endregion

    /////////////////////////////////////////////////////////////////////////////////

    /// <summary> Ctor. </summary>
    public MainWindow_VM()
    {
        _operationHistoryModelRepository = new OperationHistoryModelRepository();

        Log.Information($"Логгер встроен в {nameof(MainWindow_VM)}");

        CalculateCommand = new RelayCommand(CalculateCommand_Execute, CalculateCommand_CanExecute);
        DownloadHistoryCommand = new RelayCommand(DownloadHistoryCommand_Execute, DownloadHistoryCommand_CanExecute);
        DeleteHistoryCommand = new RelayCommand(DeleteHistoryCommand_Execute, DeleteHistoryCommand_CanExecute);
    }

    /////////////////////////////////////////////////////////////////////////////////

    #region CommandMethods




    /// <summary> Проверить возможность выполнить команду "Удалить историю вычислений". </summary>
    /// <param name="param"> Параметр. </param>
    /// <returns> Результат операции. </returns>
    private bool DeleteHistoryCommand_CanExecute(object param) => true;

    /// <summary> Выполнить команду "Удалить историю вычислений". </summary>
    /// <param name="param"> Параметр. </param>
    private void DeleteHistoryCommand_Execute(object param)
    {
        Log.Debug($"{nameof(DeleteHistoryCommand_Execute)}");
        _operationHistoryModelRepository.DeleteAll();
        HistoryCollection.Clear();
    }



    /// <summary> Проверить возможность выполнить команду "Загрузить историю вычислений". </summary>
    /// <param name="param"> Параметр. </param>
    /// <returns> Результат операции. </returns>
    private bool DownloadHistoryCommand_CanExecute(object param) => true;

    /// <summary> Выполнить команду "Загрузить историю вычислений". </summary>
    /// <param name="param"> Параметр. </param>
    private void DownloadHistoryCommand_Execute(object param)
    {
        Log.Debug($"{nameof(DownloadHistoryCommand_Execute)}");

        HistoryCollection = new ObservableCollection<OperationHistoryModel>(
            _operationHistoryModelRepository.GetAll());
    }



    /// <summary> Проверить возможность выполнить команду "Произвести вычисления". </summary>
    /// <param name="param"> Параметр. </param>
    /// <returns> Результат операции. </returns>
    private bool CalculateCommand_CanExecute(object param) => true;

    /// <summary> Выполнить команду "Произвести вычисления". </summary>
    /// <param name="param"> Параметр. </param>
    private void CalculateCommand_Execute(object param)
    {
        Log.Debug($"{nameof(CalculateCommand_Execute)}");

        var operationHistoryModel = new OperationHistoryModel()
        {
            CalculatedString = _calculatedString,
        };

        _calculatedString = _stackCalculation.Calculate(_calculatedString);
        operationHistoryModel.Result = CalculatedString;

        _operationHistoryModelRepository.Add(operationHistoryModel);
        OnPropertyChanged(nameof(CalculatedString));
    }

    #endregion

    /////////////////////////////////////////////////////////////////////////////////

    #region ViewModelMethods


    #endregion
}