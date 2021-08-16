namespace Events
{
    public class OrganizerFeedbackProvided
    {
        public OrganizerFeedbackProvided(string notes)
        {
            Notes = notes;
        }

        public string Notes { get; }
    }
}