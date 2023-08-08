using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;

namespace IO
{
    internal interface IWriter
    {
        void Write(IEnumerable<PractitionerCsv> practitionerCsvs);

        void Write(IEnumerable<PractitionerDetailsCsv> practitionerDetailsCsvs);
    }

    internal interface IReader
    {
        IList<PractitionerCsv> Read();
    }

    internal class CsvManager : IWriter, IReader
    {
        private readonly string _filePath;

        public CsvManager(string filePath)
        {
            _filePath = filePath;
        }

        public void Write(IEnumerable<PractitionerCsv> practitionerCsvs)
        {
            using (var writer = new StreamWriter(_filePath))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(practitionerCsvs);
            }
        }

        public void Write(IEnumerable<PractitionerDetailsCsv> practitionerDetailsCsvs)
        {
            using (var writer = new StreamWriter(_filePath))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(practitionerDetailsCsvs);
            }
        }

        public IList<PractitionerCsv> Read()
        {
            using (var reader = new StreamReader(_filePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                return csv.GetRecords<PractitionerCsv>().ToList();
            }
        }
    }
}
