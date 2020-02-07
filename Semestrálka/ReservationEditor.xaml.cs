using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Semestrálka
{
    /// <summary>
    /// Interakční logika pro ReservationEditor.xaml
    /// </summary>
    public partial class ReservationEditor : Window
    {
        public ReservationEditor(MeetingRoom meetingRoom, string date, IEnumerable<Reservation> otherReservations)
        {
            InitializeComponent();
            DataContext = data;

            data.MeetingRoom = meetingRoom;
            data.Date = date;
            data.OtherReservations = otherReservations;
        }

        public ReservationEditor(MeetingRoom meetingRoom, string date, IEnumerable<Reservation> otherReservations, string timeFrom, string timeTo, int expectedPersonsCount, string customer, bool videoConference, string note) : this(meetingRoom, date, otherReservations)
        {
            data.TimeFrom = timeFrom;
            data.TimeTo = timeTo;
            data.ExpectedPersonsCount = expectedPersonsCount;
            data.Customer = customer;
            data.VideoConference = videoConference;
            data.Note = note;
        }

        public ReservationEditorData data = new ReservationEditorData();
        public bool cancelled = false;

        public class ReservationEditorData
        {
            public MeetingRoom MeetingRoom { get; set; }
            public string Date { get; set; }
            public IEnumerable<Reservation> OtherReservations { get; set; }
            public string TimeFrom { get; set; }
            public string TimeTo { get; set; }
            public int ExpectedPersonsCount { get; set; }
            public string Customer { get; set; }
            public bool VideoConference { get; set; }
            public string Note { get; set; }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            cancelled = true;
            Close();
        }
    }
}
