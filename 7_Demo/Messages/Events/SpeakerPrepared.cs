namespace Events
{
    public class SpeakerPrepared
    {
        public SpeakerPrepared(string speakerFullName, string topicName, string speakerInfo)
        {
            SpeakerFullName = speakerFullName;
            TopicName = topicName;
            SpeakerInfo = speakerInfo;
        }

        public string SpeakerFullName { get; }

        public string SpeakerInfo { get; }

        public string TopicName { get; }
    }
}