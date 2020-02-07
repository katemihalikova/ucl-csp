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
    /// Interakční logika pro MeetingRoomEditor.xaml
    /// </summary>
    public partial class MeetingRoomEditor : Window
    {
        public MeetingRoomEditor()
        {
            InitializeComponent();
            DataContext = data;
        }

        public MeetingRoomEditor(string name, string code, string description, int capacity, bool videoConference) : this()
        {
            data.Name = name;
            data.Code = code;
            data.Description = description;
            data.Capacity = capacity;
            data.VideoConference = videoConference;
        }

        public MeetingRoomEditorData data = new MeetingRoomEditorData();
        public bool cancelled = false;

        public class MeetingRoomEditorData
        {
            public string Name { get; set; }
            public string Code { get; set; }
            public string Description { get; set; }
            public int Capacity { get; set; }
            public bool VideoConference { get; set; }
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
