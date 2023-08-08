using System.Collections.Generic;

namespace DoctorScrape
{
    internal class Specialities
    {
        public Specialities(IList<Speciality> specialities)
        {
            SpecialityList = specialities.AsReadOnly();
        }

        public IReadOnlyList<Speciality> SpecialityList { get; }
    }

    internal class Speciality
    {
        private Speciality(string specialtyName, string issuedOn, string type)
        {
            SpecialtyName = specialtyName;
            IssuedOn = issuedOn;
            Type = type;
        }

        public string SpecialtyName { get; }
        public string IssuedOn { get; }
        public string Type { get; }

        internal class SpecialityBuilder
        {
            private string _specialtyName;
            public string _issuedOn;
            public string _type;
            public void SpecialtyName(string specialtyName) { _specialtyName = specialtyName; }
            public void IssuedOn(string issuedOn) { _issuedOn = issuedOn; }
            public void Type(string type) { _type = type; }
            public Speciality Build() { return new Speciality(_specialtyName, _issuedOn, _type); }
        }
    }
}
