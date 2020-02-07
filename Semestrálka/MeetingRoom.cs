using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Semestrálka
{
    [Serializable]
    public class MeetingRoom : MeetingEntity
    {
        public MeetingRoom() : base()
        {
        }

        public MeetingRoom(string name, string code, string description, int capacity, bool videoConference) : base(name, code, description)
        {
            Capacity = capacity;
            VideoConference = videoConference;
            Reservations = new BindingList<Reservation>();
        }

        private int capacity;
        private bool videoConference;

        public int Capacity
        {
            get => capacity;
            set
            {
                capacity = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Capacity"));
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

        public BindingList<Reservation> Reservations { get; set; }

        public void AddReservation(DateTime date, string timeFrom, string timeTo, int expectedPersonsCount, string customer, bool videoConference, string note)
        {
            var stringDate = date.ToString("yyyy-MM-dd");
            var reservation = new Reservation(stringDate, timeFrom, timeTo, expectedPersonsCount, customer, videoConference, note);
            Reservations.Add(reservation);
        }
    }
}
