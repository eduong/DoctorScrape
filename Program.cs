using System;

namespace DoctorScrape
{
    public class Program
    {
        private const string ListPractitionersVerb = "ListPractitioners";
        private const string ListPractitionersDetailsVerb = "ListPractitionersDetails";

        static void Main(string[] args)
        {
            if (args.Length != 3)
            {
                Console.WriteLine("Arguments must be "+
                $"'{ListPractitionersVerb} <City> <practitioners CSV filepath>' or" +
                $"'{ListPractitionersDetailsVerb} <practitioners CSV filepath> <details CSV filepath>'.");
            }

            var verb = args[0];
            if (ListPractitionersVerb.Equals(verb, StringComparison.InvariantCultureIgnoreCase)) {
                var city = args[1];
                var filePath = args[2];

                Console.WriteLine($"Writing practitioners lists for {city} to {filePath}");
                ListPractitioners.Record(city, filePath);
            }
            else if (ListPractitionersDetailsVerb.Equals(verb, StringComparison.InvariantCultureIgnoreCase)) {
                var inFilePath = args[1];
                var outFilePath = args[2];

                Console.WriteLine($"Reading from practitioners lists {inFilePath}, writing details to {outFilePath}.");
                ListPractitionersDetails.Record(inFilePath, outFilePath);
            }
            else {
                Console.WriteLine($"First argument must be {ListPractitionersVerb} or {ListPractitionersDetailsVerb}");
            }
        }
    }
}
