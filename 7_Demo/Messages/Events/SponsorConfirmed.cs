namespace Events
{
    public class SponsorConfirmed
    {
        public SponsorConfirmed(string name, string address)
        {
            Name = name;
            Address = address;
        }

        public string Name { get; }

        public string Address { get; }
    }
}