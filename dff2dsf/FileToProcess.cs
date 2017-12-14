using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dff2dsf
{
    public class FileToProcess : INotifyPropertyChanged
    {
        private string _srcFile;
        private string _destFile;
        private string _status;

        public string SrcFile
        {
            get => _srcFile;
            set
            {
                _srcFile = value;
                NotifyPropertyChanged("SrcFile");
            }
        }

        public string DestFile
        {
            get => _destFile;
            set
            {
                _destFile = value;
                NotifyPropertyChanged("DestFile");
            }
        }

        public string Status
        {
            get => _status;
            set
            {
                _status = value;
                NotifyPropertyChanged("Status");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string p)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(p));
        }

    }
}
