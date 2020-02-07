using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Semestrálka
{
    class MainWindowData
    {
        public BindingList<MeetingCentre> Centres { get; set; }
        public MeetingCentre SelectedCentre { get; set; }
        public MeetingRoom SelectedRoom { get; set; }
        public DateTime? SelectedDate { get; set; }
        public BindingList<Reservation> ListOfReservations { get; set; }
        public Reservation SelectedReservation { get; set; }

        private const string fileName = "centres.xml";
        private bool pendingEdits = false;

        public MainWindowData()
        {
            ListOfReservations = new BindingList<Reservation>();
        }

        public void LoadData()
        {
            try
            {
                // otevření souboru s daty, pokud neexistuje, vyhozená výjimka se zpracuje níže
                var streamReader = new StreamReader(fileName);
                var xmlSerializer = new XmlSerializer(typeof(List<MeetingCentre>));

                try
                {
                    Centres = new BindingList<MeetingCentre>((List<MeetingCentre>)xmlSerializer.Deserialize(streamReader));
                }
                finally
                {
                    streamReader.Close();
                }
            }
            catch (Exception)
            {
                Centres = new BindingList<MeetingCentre>();
            }

            //if (Centres.Count > 0)
            //    Centres[0].MeetingRooms[0].AddReservation(DateTime.Parse("2020-01-01"), "10:00", "12:00", 4, "QWERTZ", false, "Some note...");
        }

        public void SaveData()
        {
            if (!pendingEdits)
                return;

            // vytvoření nového souboru (nebo přepsání) a zápis do něj
            var fileStream = new FileStream(fileName, FileMode.Create);
            var streamWriter = new StreamWriter(fileStream);
            var xmlSerializer = new XmlSerializer(typeof(BindingList<MeetingCentre>));

            try
            {
                xmlSerializer.Serialize(streamWriter, Centres);
            }
            finally
            {
                streamWriter.Close();
            }

            pendingEdits = false;
        }

        public void ImportData()
        {
            // nativní dialog výběru souboru
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() != true)
                return;
             
            // otevřít vybraný soubor
            var streamReader = new StreamReader(openFileDialog.FileName);

            try
            {
                // centra budou v Dictionary, aby nebylo třeba vyhledávat podle kódu
                var centres = new Dictionary<string, MeetingCentre>();
                // mode hlídá, jestli čteme centra nebo místnosti
                string mode = "Start";

                while (!streamReader.EndOfStream)
                {
                    var line = streamReader.ReadLine();
                    var values = line.Split(',');

                    if (values[0] == "MEETING_CENTRES")
                        mode = "Centres";
                    else if (values[0] == "MEETING_ROOMS")
                        mode = "Rooms";
                    else if (mode == "Centres")
                    {
                        var centre = new MeetingCentre(values[0], values[1], values[2]);
                        centres.Add(centre.Code, centre);
                    }
                    else
                    {
                        var centre = centres[values[5]];
                        centre.AddMeetingRoom(values[0], values[1], values[2], int.Parse(values[3]), values[4] == "YES");
                    }
                }

                // přehodit centra z Dictionary do BindingListu
                Centres.Clear();
                foreach (var entry in centres)
                {
                    Centres.Add(entry.Value);
                }
            }
            finally
            {
                streamReader.Close();
            }
                       
            pendingEdits = true;
        }

        public void ExportData()
        {
            // show dialog for selecting a new file, do not continue if cancelled
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.DefaultExt = "json";
            saveFileDialog.Filter = "JSON (*.json)|*.json";
            if (saveFileDialog.ShowDialog() != true)
                return;

            // prepare data for json using plain objects
            var output = new
            {
                schema = "PLUS4U.EBC.MCS.MeetingRoom_Schedule_1.0",
                uri = "ues:UCL-BT:UCL.INF/DEMO_REZERVACE:EBC.MCS.DEMO/MR001/SCHEDULE",
                data = new List<object>(),
            };

            foreach (var centre in Centres.OrderBy(centre => centre.Code))
            {
                foreach (var room in centre.MeetingRooms.OrderBy(room => room.Code))
                {
                    if (room.Reservations.Count == 0)
                        continue;

                    var roomOutput = new
                    {
                        meetingCentre = centre.Code,
                        meetingRoom = room.Code,
                        reservations = new Dictionary<string, List<object>>(),
                    };

                    foreach (var reservation in room.Reservations.OrderBy(reservation => reservation.Date).ThenBy(reservation => reservation.TimeFrom))
                    {
                        if (!roomOutput.reservations.ContainsKey(reservation.Date))
                            roomOutput.reservations.Add(reservation.Date, new List<object>());

                        var reservationOutput = new
                        {
                            from = reservation.TimeFrom,
                            to = reservation.TimeTo,
                            expectedPersonsCount = reservation.ExpectedPersonsCount,
                            customer = reservation.Customer,
                            videoConference = reservation.VideoConference,
                            note = reservation.Note,
                        };
                        roomOutput.reservations[reservation.Date].Add(reservationOutput);
                    }

                    output.data.Add(roomOutput);
                }
            }

            // save json
            var json = JsonConvert.SerializeObject(output);
            File.WriteAllText(saveFileDialog.FileName, json);
        }

        public bool IsEditPending()
        {
            return pendingEdits;
        }

        public void AddCentre(MeetingCentreEditor.MeetingCentreEditorData data)
        {
            var centre = new MeetingCentre(data.Name, data.Code, data.Description);
            Centres.Add(centre);
            pendingEdits = true;
        }

        public void EditCentre(MeetingCentreEditor.MeetingCentreEditorData data)
        {
            if (SelectedCentre.Name != data.Name)
            {
                SelectedCentre.Name = data.Name;
                pendingEdits = true;
            }
            if (SelectedCentre.Code != data.Code)
            {
                SelectedCentre.Code = data.Code;
                pendingEdits = true;
            }
            if (SelectedCentre.Description != data.Description)
            {
                SelectedCentre.Description = data.Description;
                pendingEdits = true;
            }
        }

        public void DeleteCentre()
        {
            Centres.Remove(SelectedCentre);
            SelectedCentre = null;
            pendingEdits = true;
        }

        public void AddRoom(MeetingRoomEditor.MeetingRoomEditorData data)
        {
            SelectedCentre.AddMeetingRoom(data.Name, data.Code, data.Description, data.Capacity, data.VideoConference);
            pendingEdits = true;
        }

        public void EditRoom(MeetingRoomEditor.MeetingRoomEditorData data)
        {
            if (SelectedRoom.Name != data.Name)
            {
                SelectedRoom.Name = data.Name;
                pendingEdits = true;
            }
            if (SelectedRoom.Code != data.Code)
            {
                SelectedRoom.Code = data.Code;
                pendingEdits = true;
            }
            if (SelectedRoom.Description != data.Description)
            {
                SelectedRoom.Description = data.Description;
                pendingEdits = true;
            }
            if (SelectedRoom.Capacity != data.Capacity)
            {
                SelectedRoom.Capacity = data.Capacity;
                pendingEdits = true;
            }
            if (SelectedRoom.VideoConference != data.VideoConference)
            {
                SelectedRoom.VideoConference = data.VideoConference;
                pendingEdits = true;
            }
        }

        public void DeleteRoom()
        {
            SelectedCentre.MeetingRooms.Remove(SelectedRoom);
            SelectedRoom = null;
            pendingEdits = true;
        }

        public void AddReservation(ReservationEditor.ReservationEditorData data)
        {
            SelectedRoom.AddReservation(SelectedDate.GetValueOrDefault(), data.TimeFrom, data.TimeTo, data.ExpectedPersonsCount, data.Customer, data.VideoConference, data.Note);
            PickReservations();
            pendingEdits = true;
        }

        public void EditReservation(ReservationEditor.ReservationEditorData data)
        {
            if (SelectedReservation.TimeFrom != data.TimeFrom)
            {
                SelectedReservation.TimeFrom = data.TimeFrom;
                pendingEdits = true;
            }
            if (SelectedReservation.TimeTo != data.TimeTo)
            {
                SelectedReservation.TimeTo = data.TimeTo;
                pendingEdits = true;
            }
            if (SelectedReservation.ExpectedPersonsCount != data.ExpectedPersonsCount)
            {
                SelectedReservation.ExpectedPersonsCount = data.ExpectedPersonsCount;
                pendingEdits = true;
            }
            if (SelectedReservation.Customer != data.Customer)
            {
                SelectedReservation.Customer = data.Customer;
                pendingEdits = true;
            }
            if (SelectedReservation.VideoConference != data.VideoConference)
            {
                SelectedReservation.VideoConference = data.VideoConference;
                pendingEdits = true;
            }
            if (SelectedReservation.Note != data.Note)
            {
                SelectedReservation.Note = data.Note;
                pendingEdits = true;
            }
        }

        public void DeleteReservation()
        {
            SelectedRoom.Reservations.Remove(SelectedReservation);
            ListOfReservations.Remove(SelectedReservation);
            SelectedReservation = null;
            pendingEdits = true;
        }

        public void PickReservations()
        {
            ListOfReservations.Clear();
            var dateString = SelectedDate?.ToString("yyyy-MM-dd");
            if (dateString != null && SelectedRoom != null)
            {
                var reservations = SelectedRoom.Reservations
                    .Where(reservation => reservation.Date == dateString)
                    .OrderBy(reservation => reservation.TimeFrom);
                foreach (var reservation in reservations)
                    ListOfReservations.Add(reservation);
            }
        }
    }
}
