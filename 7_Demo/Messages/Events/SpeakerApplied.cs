namespace Events
{
    public class SpeakerApplied
    {
        public SpeakerApplied(string speakerFullName, string draftTopicName)
        {
            SpeakerFullName = speakerFullName;
            DraftTopicName = draftTopicName;
        }

        public string SpeakerFullName { get; }

        public string DraftTopicName { get; }
    }
}