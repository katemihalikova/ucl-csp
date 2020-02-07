using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semestrálka
{
    [Serializable]
    public class MeetingCentre : MeetingEntity
    {
        public MeetingCentre(): base()
        {
        }

        public MeetingCentre(string name, string code, string description): base(name, code, description)
        {
            MeetingRooms = new BindingList<MeetingRoom>();
        }

        public BindingList<MeetingRoom> MeetingRooms { get; set; }

        public void AddMeetingRoom(string name, string code, string description, int capacity, bool videoConference)
        {
            var meetingRoom = new MeetingRoom(name, code, description, capacity, videoConference);
            MeetingRooms.Add(meetingRoom);
        }
    }
}
