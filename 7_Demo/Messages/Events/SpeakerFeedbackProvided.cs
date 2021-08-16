namespace Events
{
    public class SpeakerFeedbackProvided
    {
        public SpeakerFeedbackProvided(string speakerFullName, string notes)
        {
            SpeakerFullName = speakerFullName;
            Notes = notes;
        }

        public string SpeakerFullName { get; }

        public string Notes { get; }
    }
}