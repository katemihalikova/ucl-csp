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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;

namespace Semestrálka
{
    /// <summary>
    /// Interakční logika pro MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainWindowData data = new MainWindowData();

        public MainWindow()
        {
            InitializeComponent();

            DataContext = data;

            data.LoadData();
        }

        private void CentreNew_Click(object sender, RoutedEventArgs e)
        {
            var editor = new MeetingCentreEditor();
            editor.ShowDialog();
            if (!editor.cancelled)
            {
                data.AddCentre(editor.data);
            }
        }

        private void CentreEdit_Click(object sender, RoutedEventArgs e)
        {
            var editor = new MeetingCentreEditor(data.SelectedCentre.Name, data.SelectedCentre.Code, data.SelectedCentre.Description);
            editor.ShowDialog();
            if (!editor.cancelled)
            {
                data.EditCentre(editor.data);
            }
        }

        private void CentreDelete_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show($"Do you really want to delete centre {data.SelectedCentre.Name}?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                data.DeleteCentre();
            }
        }

        private void RoomNew_Click(object sender, RoutedEventArgs e)
        {
            var editor = new MeetingRoomEditor();
            editor.ShowDialog();
            if (!editor.cancelled)
            {
                data.AddRoom(editor.data);
            }
        }

        private void RoomEdit_Click(object sender, RoutedEventArgs e)
        {
            var editor = new MeetingRoomEditor(data.SelectedRoom.Name, data.SelectedRoom.Code, data.SelectedRoom.Description, data.SelectedRoom.Capacity, data.SelectedRoom.VideoConference);
            editor.ShowDialog();
            if (!editor.cancelled)
            {
                data.EditRoom(editor.data);
            }
        }

        private void RoomDelete_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show($"Do you really want to delete room {data.SelectedRoom.Name}?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                data.DeleteRoom();
            }
        }

        private void ReservationNew_Click(object sender, RoutedEventArgs e)
        {
            var editor = new ReservationEditor(data.SelectedRoom, data.SelectedDate.GetValueOrDefault().ToString("dd.MM.yyyy"), data.ListOfReservations);
            editor.ShowDialog();
            if (!editor.cancelled)
            {
                data.AddReservation(editor.data);
            }
        }

        private void ReservationEdit_Click(object sender, RoutedEventArgs e)
        {
            var otherReservations = data.ListOfReservations.Where(reservation => reservation != data.SelectedReservation);
            var editor = new ReservationEditor(data.SelectedRoom, data.SelectedDate.GetValueOrDefault().ToString("dd.MM.yyyy"), otherReservations, data.SelectedReservation.TimeFrom, data.SelectedReservation.TimeTo, data.SelectedReservation.ExpectedPersonsCount, data.SelectedReservation.Customer, data.SelectedReservation.VideoConference, data.SelectedReservation.Note);
            editor.ShowDialog();
            if (!editor.cancelled)
            {
                data.EditReservation(editor.data);
            }
        }

        private void ReservationDelete_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show($"Do you really want to delete reservation {data.SelectedReservation.Date} {data.SelectedReservation.Fullname}?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                data.DeleteReservation();
            }
        }

        private void Export_Click(object sender, RoutedEventArgs e)
        {
            data.ExportData();
        }

        private void Import_Click(object sender, RoutedEventArgs e)
        {
            data.ImportData();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            data.SaveData();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (data.IsEditPending())
            {
                var result = MessageBox.Show("There are unsaved changes.\nDo you want to save them before leaving?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    data.SaveData();
                }
            }
        }

        private void RoomsComboBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            data.PickReservations();
        }
        private void DatePicker_SelectedDateChanged(object sender, RoutedEventArgs e)
        {
            data.PickReservations();
        }
    }
}
