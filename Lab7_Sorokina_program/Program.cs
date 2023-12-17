using Lab7_Sorokina_program;
using System.Diagnostics;
using System.Dynamic;
using System.Text;
using System.Text.Json;
using System.Xml.Linq;

public class Program
{

    static string DemonstrateBehavior(List<SalonService> salon, int index, double newPrice)
    {
        if (index >= 0 && index < salon.Count)
        {
            var selectedService = salon[index];

            string result = "Selected object:\n";
            result += selectedService.ToString() + "\n";

            selectedService.Price = newPrice;

            result += "Updated object:\n";
            result += selectedService.ToString();

            return result;
        }
        else
        {
            return "Invalid index selected.";
        }
    }

    public static void AddSalonService(SalonService newService, string name, double price, ServiceType serviceType, DateTime date, string description, string additionalInfo)//переделаный
    {
        try
        {
            newService.SetName(name, allowEmpty: true);
            newService.Price = price;
            newService.Service = serviceType;
            newService.Date = date;
            newService.Description = description;
            newService.AdditionalInfo = additionalInfo;
        }
        catch (ArgumentException ex)
        {

        }
    }

    public static SalonService AddObject(string name, double price, ServiceType serviceType, DateTime date, string description, string additionalInfo)//переделаный
    {
        SalonService newService = new SalonService(name, price, serviceType, date, description, additionalInfo);
        return newService;
    }


    public static SalonService ParseSalonService(string input)//переделаный
    {
        if (SalonService.TryParse(input, out SalonService newService))
        {
            return newService;
        }
        else
        {
            throw new ArgumentException("Failed to parse the string.");
        }
    }





    public static string DisplayObjects(List<SalonService> salonList)
    {
        StringBuilder result = new StringBuilder();
        result.AppendLine($"Total services created: {SalonService.ObjectCount}");
        result.AppendLine("Salon Objects:");
        result.AppendLine("name,price,service,date,description,additionalInfo");
        foreach (SalonService salon in salonList)
        {
            result.AppendLine(salon.ToString());
        }

        return result.ToString();
    }




    public static List<SalonService> FindObject(List<SalonService> salonList, int searchBy, string searchQuery)//переделаный
    {
        switch (searchBy)
        {
            case 1: // Поиск по имени
                return salonList.FindAll(s => s.Name.Contains(searchQuery, StringComparison.OrdinalIgnoreCase));
            case 2: // Поиск по цене
                if (double.TryParse(searchQuery, out double maxPrice))
                {
                    return salonList.FindAll(s => s.Price <= maxPrice);
                }
                break;
            case 3: // Поиск по типу услуги
                if (Enum.TryParse(searchQuery, out ServiceType serviceType))
                {
                    return salonList.FindAll(s => s.Service == serviceType);
                }
                break;
            case 4: // Поиск по дате
                if (DateTime.TryParseExact(searchQuery, "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime searchDate))
                {
                    return salonList.FindAll(s => s.Date.Date == searchDate.Date);
                }
                break;
        }
        return new List<SalonService>(); // Возвращаем пустой список, если критерии не совпадают
    }


    public static void DisplaySearchResults(List<SalonService> results)
    {
        if (results.Count > 0)
        {
            Console.WriteLine("Search Results:");
            foreach (var service in results)
            {
                Console.WriteLine(service.ToString());
            }
        }
        else
        {
            Console.WriteLine("No matching services found.");
        }
    }

    public static void DeleteObject(List<SalonService> salonList, int deleteBy, int index = -1, string nameToDelete = null)//переделаный
    {
        if (deleteBy == 1)
        {
            if (index >= 0 && index < salonList.Count)
            {
                salonList.RemoveAt(index);
            }
        }
        else if (deleteBy == 2)
        {
            SalonService objectToDelete = salonList.Find(s => s.Name == nameToDelete);
            if (objectToDelete != null)
            {
                salonList.Remove(objectToDelete);
            }
        }
    }

    static void DemonstrateStaticMethods(List<SalonService> salonList)
    {
        // Демонстрация статических методов
        Console.WriteLine(SalonService.GetServiceInfo());

        // Пример использования Parse и TryParse
        string input = "Anna,30,Haircut,01.01.2026,Short,red";
        if (SalonService.TryParse(input, out SalonService parsedService))
        {
            Console.WriteLine("Parsed service: " + parsedService.ToString());
        }
        else
        {
            Console.WriteLine("Failed to parse service.");
        }


        SalonService mostExpensiveService = SalonService.FindMostExpensiveService(salonList);
        if (mostExpensiveService != null)
        {
            Console.WriteLine("Most Expensive Service:" + mostExpensiveService.Name + " - " + mostExpensiveService.Price.ToString());

            Console.WriteLine(mostExpensiveService.ToString());
        }
        else
        {
            Console.WriteLine("No services available to analyze.");
        }
        SalonService.deletedObjectCount();
    }

   

    public static void SaveToCsv(List<SalonService> services, string filePath)
    {
        var lines = new List<string>();

        // Добавляем заголовок для столбцов, если нужно
        lines.Add("Name,Price,Service,Date,Description,AdditionalInfo");

        // Формируем каждую строку для объекта
        foreach (var service in services)
        {
            var line = $"{service.Name},{service.Price},{service.Service},{service.Date:dd.MM.yyyy},{service.Description},{service.AdditionalInfo}";
            lines.Add(line);
        }

        // Записываем все строки в файл
        File.WriteAllLines(filePath, lines);
    }

   

    public static List<SalonService> ReadFromCsv(string filePath)
    {
        var lines = File.ReadAllLines(filePath);
        var services = new List<SalonService>();

        // Пропускаем первую строку с заголовками
        for (int i = 1; i < lines.Length; i++)
        {
            if (SalonService.TryParse(lines[i], out SalonService service))
            {
                services.Add(service);
            }
        }

        return services;
    }





    public static void SaveToJson(List<SalonService> services, string filePath)
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true, 
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };
        var jsonString = JsonSerializer.Serialize(services, options);
        File.WriteAllText(filePath, jsonString);
    }

    public static List<SalonService> ReadFromJson(string filePath)
    {
        var jsonString = File.ReadAllText(filePath);
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };

        var jsonElements = JsonSerializer.Deserialize<List<JsonElement>>(jsonString, options);

        var services = new List<SalonService>();
        if (jsonElements != null)
        {
            foreach (var element in jsonElements)
            {
                try
                {
                    var service = JsonSerializer.Deserialize<SalonService>(element.GetRawText(), options);
                    if (service != null)
                    {
                        services.Add(service);
                        SalonService.IncrementObjectCount(); // Увеличиваем счетчик только при успешном добавлении объекта
                    }
                }
                catch
                {
                    // Некорректный элемент игнорируем
                }
            
            }
        }

        return services;
    }


    static void Main()
    {
        List<SalonService> salon = new List<SalonService>();
        string input;

        int maxObjects;
        while (true)
        {
            Console.Write("Enter the maximum number of objects: ");
            if (int.TryParse(Console.ReadLine(), out maxObjects) && maxObjects > 0)
            {
                break;
            }
            else
            {
                Console.WriteLine("Invalid value for the maximum number of objects. Please enter a number greater than 0.");
            }
        }

        while (true)
        {
            Console.WriteLine("Menu:");
            Console.WriteLine("1 - Add an object");
            Console.WriteLine("2 - Display objects");
            Console.WriteLine("3 - Find an object");
            Console.WriteLine("4 - Delete an object");
            Console.WriteLine("5 - Demonstrate behavior");
            Console.WriteLine("6 - Demonstrate static methods");
            Console.WriteLine("7 - Save a collection of objects to a file");
            Console.WriteLine("8 - Read a collection of objects from a file");
            Console.WriteLine("9 - Clear the collection of objects");
            Console.WriteLine("0 - Exit");

            int choice;
            if (!int.TryParse(Console.ReadLine(), out choice))
            {
                Console.WriteLine("Invalid choice. Please select a valid option.");
                continue;
            }
            switch (choice)
            {
                case 1:
                    Console.WriteLine("Choose method to add a salon service:");
                    Console.WriteLine("1 - Add by entering properties manually");
                    Console.WriteLine("2 - Add by parsing a string");

                    int addMethod;
                    if (!int.TryParse(Console.ReadLine(), out addMethod) || (addMethod != 1 && addMethod != 2))
                    {
                        Console.WriteLine("Invalid method selected.");
                        break;
                    }
                    if (addMethod == 1)
                    {
                        string name;
                        double price;
                        ServiceType serviceType;
                        DateTime date;
                        string description;
                        string additionalInfo;

                        while (true)
                        {
                            Console.Write("Enter the name (Latin characters only): ");
                            name = Console.ReadLine();
                            if (!string.IsNullOrEmpty(name) && name.Length >= 3 && name.All(c => char.IsLetter(c) && (c >= 'a' && c <= 'z' || c >= 'A' && c <= 'Z')))
                            {
                                break; // Имя соответствует
                            }
                            Console.WriteLine("Invalid name. It should be at least 3 characters long and contain only Latin characters.");
                        }

                        // Ввод цены
                        while (true)
                        {
                            Console.Write("Enter the price: ");
                            if (double.TryParse(Console.ReadLine(), out price) && price > 0) break;
                            Console.WriteLine("Invalid input. Price must be a positive number.");
                        }

                        // Ввод типа услуги
                        while (true)
                        {
                            Console.WriteLine("Enter the service type (0 - Haircut, 1 - Manicure, 2 - Pedicure, 3 - Facial, 4 - Massage):");
                            if (Enum.TryParse(Console.ReadLine(), out serviceType) && Enum.IsDefined(typeof(ServiceType), serviceType)) break;
                            Console.WriteLine("Invalid input. Please select a valid service type.");
                        }

                        // Ввод даты
                        while (true)
                        {
                            Console.WriteLine("Enter the date of the appointment (dd.MM.yyyy):");
                            if (DateTime.TryParseExact(Console.ReadLine(), "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None, out date) && date >= DateTime.Now) break;
                            Console.WriteLine("Invalid date format or past date. Please use dd.MM.yyyy format and ensure the date is not in the past.");
                        }

                        // Ввод описания
                        Console.WriteLine("Enter a description for the service:");
                        description = Console.ReadLine();

                        // Ввод дополнительной информации
                        Console.WriteLine("Enter additional info for the service:");
                        additionalInfo = Console.ReadLine();

                        // Добавление объекта
                        try
                        {
                            SalonService newService = AddObject(name, price, serviceType, date, description, additionalInfo);
                            salon.Add(newService);
                            Console.WriteLine("Object added successfully.");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error: {ex.Message}");
                        }
                    }
                    else if (addMethod == 2)
                    {
                        while (true)
                        {
                            Console.WriteLine("Enter a string to parse (Format: 'Name,Price,ServiceType,Date,Description,AdditionalInfo'):");
                            input = Console.ReadLine();
                            try
                            {
                                SalonService newService = ParseSalonService(input);
                                salon.Add(newService);
                                Console.WriteLine("Object added successfully by parsing.");
                                break;
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error parsing input: {ex.Message}. Please ensure the format is correct and try again.");
                            }
                        }
                    }
                    break;
                case 2:
                    string displayResult = DisplayObjects(salon);
                    Console.WriteLine(displayResult);
                    break;
                case 3:
                    //FindObject(salon);
                    Console.WriteLine("Search by (1 - Name, 2 - Price, 3 - Service, 4 - Date):");
                    int searchBy = int.Parse(Console.ReadLine());
                    Console.WriteLine("Enter search query:");
                    string searchQuery = Console.ReadLine();

                    List<SalonService> foundServices = FindObject(salon, searchBy, searchQuery);
                    DisplaySearchResults(foundServices); 
                    break;
                case 4:
                   
                    Console.WriteLine("Delete by (1 - Index, 2 - Name):");
                    int deleteBy;
                    if (!int.TryParse(Console.ReadLine(), out deleteBy) || (deleteBy != 1 && deleteBy != 2))
                    {
                        Console.WriteLine("Invalid input. Please select a valid delete option.");
                        return;
                    }

                    int index = -1;
                    string nameToDelete = null;

                    if (deleteBy == 1)
                    {
                      
                        Console.WriteLine("Enter the index to delete:");
                        if (!int.TryParse(Console.ReadLine(), out index) || index < 0)
                        {
                            Console.WriteLine("Invalid index. Please enter a valid index.");
                            return;
                        }
                    }
                    else if (deleteBy == 2)
                    {
                       
                        Console.WriteLine("Enter the name to delete:");
                        nameToDelete = Console.ReadLine();
                    }

                 
                    DeleteObject(salon, deleteBy, index, nameToDelete);

                    // Виведення результатів в Main
                    Console.WriteLine("Object deleted successfully."); break;
                case 5:
                    Console.WriteLine("Select an index of the object to demonstrate behavior:");
                    for (int i = 0; i < salon.Count; i++)
                    {
                        Console.WriteLine($"Index {i}: {salon[i].Name}");
                    }

                    index = int.Parse(Console.ReadLine());
                    Console.WriteLine("Changing price of the service. Enter new price:");
                    double newPrice = double.Parse(Console.ReadLine());

                    string behaviorResult = DemonstrateBehavior(salon, index, newPrice);
                    Console.WriteLine(behaviorResult);
                    //DemonstrateBehavior(salon);
                    break;
                case 6:
                    DemonstrateStaticMethods(salon);
                    
                    break;
                case 7:
                    Console.WriteLine("Choose the format to save:");
                    Console.WriteLine("1 - Save to CSV");
                    Console.WriteLine("2 - Save to JSON");

                    int saveChoice = int.Parse(Console.ReadLine());
                    string saveFilePath;

                    switch (saveChoice)
                    {
                        case 1:
                            Console.WriteLine("Enter CSV file name:");
                            saveFilePath = Console.ReadLine();
                            SaveToCsv(salon, saveFilePath);
                            Console.WriteLine("Data saved to CSV file.");
                            break;
                        case 2:
                            Console.WriteLine("Enter JSON file name:");
                            saveFilePath = Console.ReadLine();
                            SaveToJson(salon, saveFilePath);
                            Console.WriteLine("Data saved to JSON file.");
                            break;
                        default:
                            Console.WriteLine("Invalid option.");
                            break;
                    }
                    break;
                case 8:
                    Console.WriteLine("Choose the format to read:");
                    Console.WriteLine("1 - Read from CSV");
                    Console.WriteLine("2 - Read from JSON");

                    int readChoice = int.Parse(Console.ReadLine());
                    string readFilePath;

                    switch (readChoice)
                    {
                        case 1:
                            Console.WriteLine("Enter CSV file name:");
                            readFilePath = Console.ReadLine();
                            salon.AddRange(ReadFromCsv(readFilePath));
                            Console.WriteLine("Data read from CSV file.");
                            break;
                        case 2:
                            Console.WriteLine("Enter JSON file name:");
                            readFilePath = Console.ReadLine();
                            salon.AddRange(ReadFromJson(readFilePath));
                            Console.WriteLine("Data read from JSON file.");
                            break;
                        default:
                            Console.WriteLine("Invalid option.");
                            break;
                    }
                    break;
                //case 9:

                //    Console.WriteLine("Choose the format to clear:");
                //    Console.WriteLine("1 - Clear CSV file");
                //    Console.WriteLine("2 - Clear JSON file");
                //    Console.WriteLine("3 - Clear Collection");

                //    int clearChoice = int.Parse(Console.ReadLine());
                //    string clearFilePath;

                //    switch (clearChoice)
                //    {
                //        case 1:
                //            Console.WriteLine("Enter CSV file name to clear:");
                //            clearFilePath = Console.ReadLine();
                //            if (File.Exists(clearFilePath))
                //            {
                //                File.WriteAllText(clearFilePath, string.Empty); // Очистка содержимого файла
                //                Console.WriteLine("CSV file cleared.");
                //            }
                //            else
                //            {
                //                Console.WriteLine("File does not exist.");
                //            }
                //            break;
                //        case 2:
                //            Console.WriteLine("Enter JSON file name to clear:");
                //            clearFilePath = Console.ReadLine();
                //            if (File.Exists(clearFilePath))
                //            {
                //                File.WriteAllText(clearFilePath, "[]"); // Установка содержимого файла в пустой JSON массив
                //                Console.WriteLine("JSON file cleared.");
                //            }
                //            else
                //            {
                //                Console.WriteLine("File does not exist.");
                //            }
                //            break;

                //        case 3:
                //            salon.Clear();
                //            Console.WriteLine("Collection cleared.");
                //            break;
                //        default:
                //            Console.WriteLine("Invalid option.");
                //            break;
                //    }
                //    break;

                case 0:
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please select a valid option.");
                    break;
            }
        }
    }
}
