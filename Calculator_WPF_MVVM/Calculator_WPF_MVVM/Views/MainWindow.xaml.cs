using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Calculator_WPF_MVVM;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly StringBuilder _sb = new();

    public MainWindow()
    {
        InitializeComponent();
    }

    /*НЕМНОГО О КОДЕ В CODEBEHIDE (знаю, что это многие не любят):
     *  
     * обработка нажатия кнопок сделана тут т.к. создавать команды во ViewModel бессмысленно
     * в данном варианте программы для VM важна вычисляемая строка целиком, а не привязывать каждую кнопку*/

    /// <summary> Удалить последний символ. </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void RemoveSymbol_Click(object sender, RoutedEventArgs e)
    {
        _sb.Length--;
        MainTextBox.Text = _sb.ToString();
    }

    /// <summary> Добавить символ. </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Button_Click(object sender, RoutedEventArgs e)
    {
        _sb.Append(((Button)sender).CommandParameter);
        MainTextBox.Text = _sb.ToString();
    }

    /// <summary> Обработка ввода с клавиатуры. </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void SetNewNumb_TextChanged(object sender, TextChangedEventArgs e)
    {
        _sb.Clear();
        _sb.Append(MainTextBox.Text);
    }

    /// <summary> Полностью очистить вычисляемую строку. </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void CleanAll_Click(object sender, RoutedEventArgs e)
    {
        _sb.Clear();
        MainTextBox.Text = string.Empty;
    }
}
