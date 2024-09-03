using Microsoft.Win32;
using System.Diagnostics;
using System.Windows;
using XcsToPgmxConverter.ViewModels;

namespace XcsToPgmxConverter
{
    public partial class MainWindow : Window
    {
        public MainViewModel ViewModel { get; }

        public MainWindow()
        {
            InitializeComponent(); // This line should be here
            ViewModel = new MainViewModel();
            DataContext = ViewModel;
            FilesDataGrid.DataContext = ViewModel;
        }


        private void SelectFiles_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "XCS files (*.xcs)|*.xcs",
                Multiselect = true
            };

            if (openFileDialog.ShowDialog() == true)
            {
                var files = openFileDialog.FileNames;

                if (files.Length >= 1) ViewModel.File1 = files[0];
                if (files.Length >= 2) ViewModel.File2 = files[1];
            }

            
        }
        private void MergeFiles_Click(object sender, RoutedEventArgs e)
        {
            string file1 = ViewModel.File1;
            string file2 = ViewModel.File2;

            // Retrieve the filename from the TextBox
            string filename = FilenameTextBox.Text.Trim();

            // Default to "Merge" if no filename is provided
            if (string.IsNullOrWhiteSpace(filename))
            {
                filename = "Merge";
            }

            string downloadsFolder = GetDownloadsFolderPath();
            string outputFile = System.IO.Path.Combine(downloadsFolder, $"{filename}.pgmx");
            string converterPath = @"C:\Program Files\SCM Group\Maestro\XConverter.exe";
            string tlgxPath = @"C:\Program Files\SCM Group\Maestro\tlgx\def.tlgx";

            try
            {
                string arguments = $"-s -m 12 -i \"{file1}\" \"{file2}\" -t \"{tlgxPath}\" -o \"{outputFile}\"";

                ProcessStartInfo processStartInfo = new ProcessStartInfo
                {
                    FileName = converterPath,
                    Arguments = arguments,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                };

                using (Process process = Process.Start(processStartInfo))
                {
                    string output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();

                    process.WaitForExit();

                    if (process.ExitCode != 0)
                    {
                        MessageBox.Show($"Error: {error}", "Merge Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        MessageBox.Show("Merge completed successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Exception: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private string GetDownloadsFolderPath()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Downloads";
        }

        private void FilesDataGrid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }
    }
}
