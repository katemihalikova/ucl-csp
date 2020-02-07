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
    /// Interakční logika pro MeetingCentreEditor.xaml
    /// </summary>
    public partial class MeetingCentreEditor : Window
    {
        public MeetingCentreEditor()
        {
            InitializeComponent();
            DataContext = data;
        }

        public MeetingCentreEditor(string name, string code, string description) : this()
        {
            data.Name = name;
            data.Code = code;
            data.Description = description;
        }

        public MeetingCentreEditorData data = new MeetingCentreEditorData();
        public bool cancelled = false;

        public class MeetingCentreEditorData
        {
            public string Name { get; set; }
            public string Code { get; set; }
            public string Description { get; set; }
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
