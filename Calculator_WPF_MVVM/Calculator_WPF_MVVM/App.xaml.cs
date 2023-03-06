using NLog;
using PastingProductivityCalculationProgram.ViewModel.ViewModels.Windows;
using System.Windows;
using System.Data.SQLite;
using System.IO;

namespace Calculator_WPF_MVVM
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        static Logger _logger = LogManager.GetCurrentClassLogger();

        /// <summary> Кастомный стартап. </summary>
        /// <param name="e"></param>
        protected override void OnStartup(StartupEventArgs e)
        {
            

            if (!File.Exists("Log.db3"))
            {
                using (SQLiteConnection connection = new SQLiteConnection("Data Source = Log.db3; Version = 3;"))
                using (SQLiteCommand command = new SQLiteCommand(
                    "CREATE TABLE Log (Timestamp TEXT, Loglevel TEXT, Callsite TEXT, Message TEXT)",
                    connection))
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            

            _logger.Info("Cтарт программы.");

            MainWindow_VM _vaiwModel = new(_logger);
            Current.MainWindow = new MainWindow
            {
                DataContext = _vaiwModel
            };
            Current.MainWindow.Show();
        }
    }   
}
