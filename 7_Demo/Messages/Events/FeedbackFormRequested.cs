namespace Events
{
    public class FeedbackFormRequested
    {
        public FeedbackFormRequested(string speakerFullName, string topicName, string url)
        {
            SpeakerFullName = speakerFullName;
            TopicName = topicName;
            Url = url;
        }

        public string SpeakerFullName { get; }

        public string TopicName { get; }

        public string Url { get; }
    }
}