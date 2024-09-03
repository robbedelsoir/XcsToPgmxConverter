using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace XcsToPgmxConverter.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private string file1;
        private string file2;

        public string File1
        {
            get => file1;
            set
            {
                file1 = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CanMerge));
            }
        }

        public string File2
        {
            get => file2;
            set
            {
                file2 = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CanMerge));
            }
        }

        public bool CanMerge => !string.IsNullOrEmpty(File1) && !string.IsNullOrEmpty(File2);

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
