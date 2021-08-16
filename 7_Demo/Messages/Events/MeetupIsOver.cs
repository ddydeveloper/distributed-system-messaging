using System;

namespace Events
{
    public class MeetupIsOver
    {
        public MeetupIsOver(DateTime dateTime)
        {
            DateTime = dateTime;
        }

        public DateTime DateTime { get; }
    }
}