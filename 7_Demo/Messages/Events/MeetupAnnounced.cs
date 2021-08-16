using System;

namespace Events
{
    public class MeetupAnnounced
    {
        public MeetupAnnounced(string topicName, string speakerInfo, string speakerFullName, string address,
            DateTime dateTime)
        {
            TopicName = topicName;
            SpeakerInfo = speakerInfo;
            SpeakerFullName = speakerFullName;
            Address = address;
            DateTime = dateTime;
        }

        public DateTime DateTime { get; }

        public string Address { get; }

        public string SpeakerFullName { get; }

        public string SpeakerInfo { get; }

        public string TopicName { get; }
    }
}