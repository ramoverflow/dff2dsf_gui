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

        [DisplayName("Source File")]
        public string SrcFile
        {
            get => _srcFile;
            set
            {
                _srcFile = value;
                NotifyPropertyChanged("SrcFile");
            }
        }

        [DisplayName("Preview File")]
        public string DestFile
        {
            get => _destFile;
            set
            {
                _destFile = value;
                NotifyPropertyChanged("DestFile");
            }
        }

        [DisplayName("Result")]
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
