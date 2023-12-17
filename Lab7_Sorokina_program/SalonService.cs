using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Lab7_Sorokina_program
{
    public class SalonService
    {
        private static int objectCount = 0; // Счетчик объектов
        public static int ObjectCount => objectCount; // Публичный доступ к счетчику

        private string name;
        private double price;
        private ServiceType service;
        private DateTime date;
        public string Description { get; set; } = "No description available";
        public string AdditionalInfo { get; set; } = "No additional info available";

        // Конструкторы
        public SalonService(string name, double price, ServiceType service, DateTime date, string description, string additionalInfo) : this(name, price, service, date, description)
        {
            AdditionalInfo = additionalInfo;
            objectCount++;
        }

        public SalonService(string name, double price, ServiceType service, DateTime date, string description) : this(name, price, service, date)
        {
            Description = description;
        }

        public SalonService(string name, double price, ServiceType service, DateTime date)
        {
            SetName(name, allowEmpty: false);
            Price = price;
            Service = service;
            Date = date;

        }

        public SalonService()
        {
            Name = null;
            Price = 0.01;
            Service = ServiceType.Unknown;
            Date = DateTime.Now;
            //objectCount++;
        }

        // Свойства
        public string Name
        {
            get => name;
            set => name = string.IsNullOrEmpty(value) ? "Default Name" : value;
        }

        public double Price
        {
            get { return price; }
            set
            {
                if (value > 0)
                {
                    price = value;
                }
                else
                {
                    throw new ArgumentException("Price should be a positive number.");
                }
            }
        }

        public ServiceType Service
        {
            get { return service; }
            set { service = value; }
        }

        public DateTime Date
        {
            get { return date; }
            set
            {
                if (value >= new DateTime(2023, 11, 1))
                {
                    date = value;
                }
                else
                {
                    throw new ArgumentException("Date of the appointment should be from 01.11.2023 or later.");
                }
            }
        }

        public string ServiceAndPrice => $"{Service} - {Price}";

        // Методы
        public void SetName(string value, bool allowEmpty = false)
        {
            if ((allowEmpty || !string.IsNullOrEmpty(value)) && value.Length >= 3 && IsLatinCharacters(value))
            {
                Name = value;
            }
            else
            {
                throw new ArgumentException("Invalid name. It should be at least 3 characters long and contain only Latin characters.");
            }
        }

        public void SetName(string value, int minLength)
        {
            if (!string.IsNullOrEmpty(value) && value.Length >= minLength && IsLatinCharacters(value))
            {
                Name = value;
            }
            else
            {
                throw new ArgumentException($"Invalid name. It should be at least {minLength} characters long and contain only Latin characters.");
            }
        }

        private bool IsLatinCharacters(string text)
        {
            foreach (char c in text)
            {
                if (!((c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z')))
                {
                    return false;
                }
            }
            return true;
        }

        public void UpdateServiceInfo(double newPrice, DateTime newDate)
        {
            if (newPrice <= 0)
            {
                throw new ArgumentException("New price should be a positive number.");
            }

            if (newDate < DateTime.Now)
            {
                throw new ArgumentException("New date should be in the future.");
            }

            Price = newPrice;
            Date = newDate;
        }



        public void UpdateServiceInfo(double newPrice, DateTime newDate, string newDescription)
        {
            UpdateServiceInfo(newPrice, newDate);
            Description = newDescription;
        }

        public void UpdateServiceInfo(double newPrice, DateTime newDate, string newDescription, string newAdditionalInfo)
        {
            UpdateServiceInfo(newPrice, newDate, newDescription);
            AdditionalInfo = newAdditionalInfo;
        }


        public static void IncrementObjectCount()
        {
            objectCount++;
        }


       
        public static SalonService Parse(string s)
        {
            string[] parts = s.Split(',');
            if (parts.Length < 6)
            {
                throw new FormatException("String format is not correct.");
            }

            string name = parts[0].Trim();
            if (!double.TryParse(parts[1].Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out double price))
            {
                throw new FormatException("Price format is not correct.");
            }

            if (!Enum.TryParse(parts[2].Trim(), out ServiceType service))
            {
                throw new FormatException("Service type format is not correct.");
            }

            if (!DateTime.TryParseExact(parts[3].Trim(), "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
            {
                throw new FormatException("Date format is not correct.");
            }

            string description = parts[4].Trim();
            string additionalInfo = parts[5].Trim();

            return new SalonService(name, price, service, date, description, additionalInfo);
        }



        public static bool TryParse(string s, out SalonService obj)
        {
            try
            {
                obj = Parse(s);
                return true;
            }
            catch
            {
                obj = null;
                return false;
            }
        }

        public static bool TryParseFromJson(string json, out SalonService service)
        {
            try
            {
                service = JsonSerializer.Deserialize<SalonService>(json);
                return service != null;
            }
            catch
            {
                service = null;
                return false;
            }
        }


        public override string ToString()
        {
            CultureInfo culture = new CultureInfo("en-US");
            return $"{Name}, {Price.ToString("N2", culture)}, {Service}, {Date:dd.MM.yyyy}, {Description}, {AdditionalInfo}";
        }

      
        public static string GetServiceInfo()
        {
            return $"Total services created: {ObjectCount}";
        }

        public static void deletedObjectCount()
        {
            objectCount--;
        }

        public static SalonService FindMostExpensiveService(List<SalonService> services)
        {
            if (services == null || services.Count == 0)
            {
                return null;
            }

            return services.OrderByDescending(s => s.Price).FirstOrDefault();
        }

        [JsonIgnore] 
        public string PriceFormatted
        {
            get { return $"{Price:0.00} ₽"; }
            
        }

    }
}
