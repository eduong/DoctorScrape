namespace DoctorScrape
{
    internal class Summary
    {
        private Summary(
            string formerNames,
            string gender,
            string languagesSpoken,
            string education)
        {
            FormerNames = formerNames;
            Gender = gender;
            LanguagesSpoken = languagesSpoken;
            Education = education;
        }

        public string FormerNames { get; }
        public string Gender { get; }
        public string LanguagesSpoken { get; }
        public string Education { get; }

        internal class SummaryBuilder
        {
            private string _formerNames;
            private string _gender;
            private string _languagesSpoken;
            private string _education;
            public void FormerNames(string formerNames) { _formerNames = formerNames; }
            public void Gender(string gender) { _gender = gender; }
            public void LanguagesSpoken(string languagesSpoken) { _languagesSpoken = languagesSpoken; }
            public void Education(string education) { _education = education; }
            public Summary Build() { return new Summary(_formerNames, _gender, _languagesSpoken, _education); }
        }
    }
}
