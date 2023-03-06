
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PastingProductivityCalculationProgram.ViewModel.ViewModels.Windows;

/// <summary> Базовый класс для ViewModels. </summary>
public class ViewModelBase
{
    public event PropertyChangedEventHandler? PropertyChanged;

    public void OnPropertyChanged([CallerMemberName] string prop = "")
    {
        if (PropertyChanged != null)
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}