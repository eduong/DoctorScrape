namespace DoctorScrape
{
    internal class Basics
    {
        public Basics(
            string name,
            string cpsoNumber)
        {
            Name = name;
            CpsoNumber = cpsoNumber;
        }

        public string Name { get; }
        public string CpsoNumber { get; }
    }
}
