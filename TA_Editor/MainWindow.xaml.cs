using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace TA_Editor
{
    using System.Reflection;

    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();

        }

        public DataGrid DataGridTDF => this.m_DataGridTDF;

        public DataGrid DataGridFBI => this.m_DataGridFBI;

        void DataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex()).ToString();
        }

        private void OnInformationClick(object sender, RoutedEventArgs e)
        {
            var version = Assembly.GetExecutingAssembly().GetName().Version;
            var title = "About Total Annihilation Units and Weapons Editor";
            var message =
                $"Total Annihilation Units and Weapons Editor {version}"
                + "\r\n"
                + "\r\nThe original author of this program is Pascal Wauer. This fork contains modifications by Michael Heasell.";
            MessageBox.Show(message, title);
        }

        private void OnSelectionChangedFBI(object sender, SelectedCellsChangedEventArgs e)
        {
            this.DataGridTDF.SelectedCells.Clear();
        }

        private void OnSelectionChangedTDF(object sender, SelectedCellsChangedEventArgs e)
        {
            this.DataGridFBI.SelectedCells.Clear();
        }

        private void OnCellRightClickClick(object sender, MouseButtonEventArgs e)
        {
            TaCommands.OnCellRightClickClick.Execute(null, this);
        }
    }
}
