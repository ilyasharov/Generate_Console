using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using static System.Console;
using static System.Environment;

public class Program{

    static void Main(){

        // Step 1: Create a collection of randomly generated objects
        List<Person> persons = GenerateRandomPersons(10000);

        // Step 2: Serialize the collection to JSON
        string json = SerializeToJson(persons);

        // Step 3: Write the serialization result to the current user desktop directory
        string desktopPath = GetFolderPath(SpecialFolder.Desktop);
        string filePath = Path.Combine(desktopPath, "Persons.json");
        WriteToFile(filePath, json);

        // Step 4: Clear the in-memory collection
        persons.Clear();

        // Step 5: Read objects from the file
        List<Person> deserializedPersons = ReadFromFile(filePath);

        // Step 6: Display information in the console
        DisplayConsoleInformation(deserializedPersons);

    }
    
    static List<Person> GenerateRandomPersons(int count){
            
            List<Person> persons = new List<Person>();

            Random random = new Random();

            for (int i = 0; i < count; i++)
            {
                Person person = new Person
                {
                    Id = i + 1,
                    TransportId = Guid.NewGuid(),
                    FirstName = GenerateRandomName("FirstName"),
                    LastName = GenerateRandomName("LastName"),
                    SequenceId = random.Next(1000, 9999),
                    CreditCardNumbers = GenerateRandomCreditCardNumbers(random.Next(1, 4)),
                    Age = random.Next(18, 60),
                    Phones = GenerateRandomPhoneNumbers(random.Next(1, 3)),
                    BirthDate = GenerateRandomAge(18, 60),
                    Salary = random.NextDouble() * 100,
                    IsMarred = random.Next(2) == 0,
                    Gender = (Gender)random.Next(2),
                    Children = GenerateRandomChildren(random.Next(1, 4))
                };

                persons.Add(person);
            }

            return persons;
    }

    static string GenerateRandomName(string prefix){
        Random random = new Random();
        return $"{prefix}_{random.Next(1000, 9999)}";
    }

    static string[] GenerateRandomCreditCardNumbers(int count){
        
            string[] creditCardNumbers = new string[count];
            Random random = new Random();

            for (int i = 0; i < count; i++)
            {
                creditCardNumbers[i] = random.Next(1000, 9999).ToString("D4") +
                                       random.Next(1000, 9999).ToString("D4") +
                                       random.Next(1000, 9999).ToString("D4") +
                                       random.Next(1000, 9999).ToString("D4");
            }

            return creditCardNumbers;
    }

    static string[] GenerateRandomPhoneNumbers(int count){

            string[] phoneNumbers = new string[count];
            Random random = new Random();

            for (int i = 0; i < count; i++)
            {
                phoneNumbers[i] = "555-" +
                                  random.Next(100, 999).ToString("D3") +
                                  "-" + random.Next(1000, 9999).ToString("D4");
            }

            return phoneNumbers;
    }
    
    static long GenerateRandomAge(int minAge, int maxAge){
        
        if (minAge > maxAge)
        {
            throw new ArgumentException("minAge should be less than or equal to maxAge");
        }

        Random random = new Random();
        int age = random.Next(minAge, maxAge + 1);
        long ageAsInt64 = Convert.ToInt64(age);

        return ageAsInt64;
    }

    static Child[] GenerateRandomChildren(int count){
            
            Child[] children = new Child[count];
            Random random = new Random();

            for (int i = 0; i < count; i++)
            {
                Child child = new Child
                {
                    Id = i + 1,
                    FirstName = GenerateRandomName("ChildFirstName"),
                    LastName = GenerateRandomName("ChildLastName"),
                    BirthDate = GenerateRandomAge(0, 18),
                    Gender = (Gender)random.Next(2)
                };

                children[i] = child;
            }

            return children;
    }

    static int CalculateAge(long birthDateTicks){
        
        DateTime birthDate = new DateTime(birthDateTicks);
        DateTime currentDate = DateTime.Now;
        int age = currentDate.Year - birthDate.Year;

        if (currentDate < birthDate.AddYears(age))
        {
            age--;
        }

        return age;
    }

    static string SerializeToJson(List<Person> persons){
            
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                Formatting = Formatting.Indented,
                ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()
            };

            return JsonConvert.SerializeObject(persons, settings);
    }

    static void WriteToFile(string filePath, string content){
            File.WriteAllText(filePath, content);
    }

    static List<Person> ReadFromFile(string filePath){
            string json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<List<Person>>(json);
    }

    static double CalculateAverageChildAge(Person person)
    {
        double sumAverage = person.Children.Sum(child => child.BirthDate);

        return sumAverage / person.Children.Length;
    }

    static void DisplayConsoleInformation(List<Person> persons){
        
        WriteLine($"Persons count: {persons.Count}");

        foreach (Person person in persons)
        {
            WriteLine($"Person №{person.Id}:");
            WriteLine($"- Age: {person.BirthDate}");
            WriteLine($"- Credit card count: {person.CreditCardNumbers.Length}");
            WriteLine($"- Average child age: {CalculateAverageChildAge(person):F2}");

            WriteLine();
        }

        ReadLine();
    }
}
