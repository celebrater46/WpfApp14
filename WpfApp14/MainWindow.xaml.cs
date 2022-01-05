using System;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Shell;

namespace WpfApp14
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public ObservableCollection<ZipRecord> ZipRecords { get; set; }
        
        public MainWindow()
        {
            InitializeComponent();
            
            this.ZipRecords = new ObservableCollection<ZipRecord>();
            listView.DataContext = this.ZipRecords;
            
            BindingOperations.EnableCollectionSynchronization(this.ZipRecords, new object());
        }

        private void ReadCsv(string filePath)
        {
            this.ZipRecords.Clear();

            var parser = new Microsoft.VisualBasic.FileIO.TextFieldParser(filePath, Encoding.Default);
            
            using(parser)
            {
                parser.TextFieldType = Microsoft.VisualBasic.FileIO.FieldType.Delimited;
                parser.SetDelimiters(",");

                try
                {
                    // Until next file is nothing
                    while (parser.EndOfData == false)
                    {
                        string[] buf = parser.ReadFields();

                        this.ZipRecords.Add(new ZipRecord
                        {
                            Code = buf[0],
                            OldZip = buf[1],
                            Zip = buf[2],
                            StateKana = buf[3],
                            CityKana = buf[4],
                            TownKana = buf[5],
                            State = buf[6],
                            City = buf[7],
                            Town = buf[8],
                            Flag1 = buf[9],
                            Flag2 = buf[10],
                            Flag3 = buf[11],
                            Flag4 = buf[12],
                            Flag5 = buf[13],
                            Flag6 = buf[14],
                        });
                    }
                }
                catch
                {
                    throw new Exception("Error to read CSV.");
                }
            }
        }

        // private void OpenBtClick(object sender, RoutedEventArgs e)
        private async void OpenBtClick(object sender, RoutedEventArgs e)
        {
            var dlg = new Microsoft.Win32.OpenFileDialog();

            dlg.Filter = "CSV file(*.csv)|*.csv|Text file(*.txt)|*.txt|Other file(*.*)|*.*";
            dlg.FilterIndex = 1;

            if (dlg.ShowDialog() == true)
            {
                // this.IsEnabled = false; 
                // ReadCsv(dlg.FileName);
                // this.IsEnabled = true;
                SetLoadingUi(true);
                await ReadCsvTask(dlg.FileName);
                SetLoadingUi(false);
            }
        }

        private void CloseBtClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ClearBtClick(object sender, RoutedEventArgs e)
        {
            this.ZipRecords.Clear();
        }

        private void Test_Click(object sender, RoutedEventArgs e)
        {
            this.IsEnabled = false;
        }

        private void SetLoadingUi(bool loading)
        {
            if (loading)
            {
                // Make the display invalid
                this.IsEnabled = !loading;
                loadingText.Visibility = Visibility.Visible;
                listView.Visibility = Visibility.Collapsed;
                taskbarInfo.ProgressState = TaskbarItemProgressState.Indeterminate;
            }
            else
            {
                this.IsEnabled = !loading;
                loadingText.Visibility = Visibility.Collapsed;
                listView.Visibility = Visibility.Visible;
                taskbarInfo.ProgressState = TaskbarItemProgressState.None;
            }
        }

        private Task ReadCsvTask(string filePath)
        {
            return Task.Run(() => { ReadCsv(filePath);});
        }
    }
}