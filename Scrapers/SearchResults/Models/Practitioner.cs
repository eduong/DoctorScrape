namespace DoctorScrape
{
    internal class Practitioner
    {
        public Practitioner(string detailsUrl)
        {
            DetailsUrl = detailsUrl;
        }

        public string DetailsUrl { get; }
    }
}