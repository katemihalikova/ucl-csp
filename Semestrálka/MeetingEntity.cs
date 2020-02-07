using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semestrálka
{
    public abstract class MeetingEntity : INotifyPropertyChanged
    {
        protected MeetingEntity(string name, string code, string description)
        {
            Name = name;
            Code = code;
            Description = description;
        }

        protected MeetingEntity()
        {
        }

        private string name;
        private string code;
        private string description;

        public string Name {
            get => name;
            set
            {
                name = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Name"));
                OnPropertyChanged(new PropertyChangedEventArgs("FullName"));
            }
        }
        public string Code
        {
            get => code;
            set
            {
                code = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Code"));
                OnPropertyChanged(new PropertyChangedEventArgs("FullName"));
            }
        }
        public string Fullname
        {
            get => string.Format("{0} ({1})", Name, Code);
        }
        public string Description
        {
            get => description;
            set
            {
                description = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Description"));
            }
        }

        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
