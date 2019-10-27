using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Diagnostics;

namespace SudokuVisual.Class
{
    class ObservableArray : INotifyPropertyChanged
    {
        private string[] data;
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableArray(int sz)
        {
            data = new string[81];
        }

        private int _index;

        public int Index
        {
            get { return _index; }
            set { _index = value; OnPropertyChanged("Index"); }
        }


        public string this[int key]
        {
            get { return data[key]; }
            set { data[key] = value;
                Debug.WriteLine("Setting " + key + " to " + value);
                Index = key;
            }
        }

        // Create the OnPropertyChanged method to raise the event
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
