# DoctorScrape
Scrapes public data from College of Physicians and Surgeons of Ontario website.

# Framework & Libraries
net7.0, Selenium and CsvHelper

# Building (with Visual Studio Code)
- Visual Studio Code
- Microsoft .NET 7 SDK
- Visual Studio Code extensions: C#, C# Dev Kit

# Running
.vscode/Launch.json contains argument examples for runnign the console app in VS Code. The program supports two verbs: ListPractitioners and ListPractitionersDetails.

# ListPractitioners
Program.exe ListPractitioners <city> <filePath_to_csv_out> performs an advanced search on a <city> then writes to <filePath_to_csv_out> the practitioner details URLs of all search results.

The <filePath_to_csv_out> has the structure: DetailsUrl

# ListPractitionersDetails
Program.exe ListPractitionersDetails <filePath_to_csv_in> <filePath_to_csv_out> loads the details URLs of all practitioners in <filePath_to_csv_in>, typically the output of ListPractitioners, pulls the details on that page and writes it to <filePath_to_csv_out>.

The <filePath_to_csv_out> has the structure: Name,CpsoNumber,FormerNames,Gender,LanguagesSpoken,Education,PracticeLocation,Specialities
