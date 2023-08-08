using System.Linq;
using IO;

namespace DoctorScrape {
    internal static class Converter {
        public static PractitionerCsv Convert(Practitioner practitioner) => new PractitionerCsv { DetailsUrl = practitioner.DetailsUrl };

        public static Practitioner Convert(PractitionerCsv practitionerCsv) => new Practitioner(practitionerCsv.DetailsUrl);

        public static PractitionerDetailsCsv Convert(
            Basics basics,
            Summary summary,
            PracticeLocation practiceLocation,
            Specialities specialities) =>
            new PractitionerDetailsCsv {
                Name = basics.Name,
                CpsoNumber = basics.CpsoNumber,
                FormerNames = summary.FormerNames,
                Gender = summary.Gender,
                LanguagesSpoken = summary.LanguagesSpoken,
                Education = summary.Education,
                PracticeLocation = practiceLocation.Text,
                Specialities = string.Join(',', specialities.SpecialityList.Select(s => $"({s.SpecialtyName},{s.IssuedOn},{s.Type})"))
            };
    }
}