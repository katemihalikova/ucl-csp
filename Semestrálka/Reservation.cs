using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semestrálka
{
    [Serializable]
    public class Reservation : INotifyPropertyChanged
    {
        public Reservation()
        {
        }

        public Reservation(string date, string timeFrom, string timeTo, int expectedPersonsCount, string customer, bool videoConference, string note)
        {
            Date = date;
            TimeFrom = timeFrom;
            TimeTo = timeTo;
            ExpectedPersonsCount = expectedPersonsCount;
            Customer = customer;
            VideoConference = videoConference;
            Note = note;
        }

        private string date;
        private string timeFrom;
        private string timeTo;
        private int expectedPersonsCount;
        private string customer;
        private bool videoConference;
        private string note;

        public string Date
        {
            get => date;
            set
            {
                date = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Date"));
            }
        }
        public string TimeFrom
        {
            get => timeFrom;
            set
            {
                timeFrom = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TimeFrom"));
                OnPropertyChanged(new PropertyChangedEventArgs("Fullname"));
            }
        }
        public string TimeTo
        {
            get => timeTo;
            set
            {
                timeTo = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TimeTo"));
                OnPropertyChanged(new PropertyChangedEventArgs("Fullname"));
            }
        }
        public int ExpectedPersonsCount
        {
            get => expectedPersonsCount;
            set
            {
                expectedPersonsCount = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ExpectedPersonsCount"));
            }
        }
        public string Customer
        {
            get => customer;
            set
            {
                customer = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Customer"));
                OnPropertyChanged(new PropertyChangedEventArgs("Fullname"));
            }
        }
        public bool VideoConference
        {
            get => videoConference;
            set
            {
                videoConference = value;
                OnPropertyChanged(new PropertyChangedEventArgs("VideoConference"));
            }
        }
        public string Note
        {
            get => note;
            set
            {
                note = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Note"));
            }
        }

        public string Fullname
        {
            get => string.Format("{0} - {1}: {2}", TimeFrom, TimeTo, Customer);
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
