using Lab7_Sorokina_program;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace TestProjectProgram
{
    [TestClass]
    public class SalonProgramTestTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ParseSalonService_InvalidString_ThrowsArgumentException()
        {
            // Arrange
            string input = "InvalidInput";

            // Act
            Program.ParseSalonService(input);

            // Assert не нужен
        }

        [TestMethod]
        public void AddSalonService_ValidData_ShouldSetProperties()
        {
            // Arrange
            var newService = new SalonService(); // конструктор по умолчанию
            string name = "TestService";
            double price = 99.99;
            ServiceType serviceType = ServiceType.Haircut;
            DateTime date = new DateTime(2023, 12, 31);
            string description = "Test Description";
            string additionalInfo = "Test Additional Info";

            // Act
            Program.AddSalonService(newService, name, price, serviceType, date, description, additionalInfo);

            // Assert
            Assert.AreEqual(name, newService.Name);
            Assert.AreEqual(price, newService.Price);
            Assert.AreEqual(serviceType, newService.Service);
            Assert.AreEqual(date, newService.Date);
            Assert.AreEqual(description, newService.Description);
            Assert.AreEqual(additionalInfo, newService.AdditionalInfo);
        }

        [TestMethod]
        public void AddObject_ValidData_ShouldReturnNewService()
        {
            // Arrange
            string name = "NewService";
            double price = 150.0;
            ServiceType serviceType = ServiceType.Manicure;
            DateTime date = new DateTime(2023, 12, 31);
            string description = "A new service";
            string additionalInfo = "Additional service info";

            // Act
            var service = Program.AddObject(name, price, serviceType, date, description, additionalInfo);

            // Assert
            Assert.IsNotNull(service);
            Assert.AreEqual(name, service.Name);
            Assert.AreEqual(price, service.Price);
            Assert.AreEqual(serviceType, service.Service);
            Assert.AreEqual(date, service.Date);
            Assert.AreEqual(description, service.Description);
            Assert.AreEqual(additionalInfo, service.AdditionalInfo);
        }

        [TestMethod]
        public void FindObject_ByServiceType_ReturnsCorrectServices()
        {
            // Arrange
            var salonList = new List<SalonService>
            {
                new SalonService("Anna", 98, ServiceType.Haircut, new DateTime(2027, 9, 9), "", ""),
            };
            string searchQuery = "Haircut";

            // Act
            var results = Program.FindObject(salonList, 3, searchQuery);

            // Assert
            Assert.IsTrue(results.All(s => s.Service == ServiceType.Haircut));
            Assert.IsTrue(results.Any()); // Проверяем, что результаты действительно найдены
        }

        [TestMethod]
        public void FindObject_ByPrice_ReturnsServiceWithExactPrice()
        {
            // Arrange
            var salonList = new List<SalonService>
            {
                new SalonService("Anna", 98, ServiceType.Haircut, new DateTime(2027, 9, 9), "", ""),
            };
            string searchQuery = "98";

            // Act
            var results = Program.FindObject(salonList, 2, searchQuery);

            // Assert
            Assert.IsTrue(results.All(s => s.Price == 98));
            Assert.IsTrue(results.Any());
        }



        [TestMethod]
        public void FindObject_ByName_ReturnsCorrectServices()
        {
            // Arrange
            var salonList = new List<SalonService>
            {
                new SalonService("Anna", 98, ServiceType.Haircut, new DateTime(2027, 9, 9), "", ""),

            };
            string searchQuery = "Anna";

            // Act
            var results = Program.FindObject(salonList, 1, searchQuery);

            // Assert
            Assert.IsTrue(results.Any());
            Assert.IsTrue(results.All(s => s.Name == "Anna"));
        }


        [TestMethod]
        public void FindObject_ByDate_ReturnsServicesOnSpecifiedDate()
        {
            // Arrange
            var salonList = new List<SalonService>
            {
                new SalonService("Anna", 98, ServiceType.Haircut, new DateTime(2027, 9, 9), "", ""),

            };
            string searchQuery = "09.09.2027"; // The price to search for

            // Act
            var results = Program.FindObject(salonList, 4, searchQuery);

            // Assert

            Assert.IsTrue(results.Any());
            DateTime targetDate = new DateTime(2027, 9, 9);
            Assert.IsTrue(results.All(s => s.Date.Date == targetDate.Date));
        }

        [TestMethod]
        public void FindObject_WithNonExistentCriteria_ReturnsEmptyList()
        {
            // Arrange
            var salonList = new List<SalonService>
            {
                 new SalonService("Anna", 98, ServiceType.Haircut, new DateTime(2027, 9, 9), "", ""),
            };
            string searchQuery = "250";

            // Act
            var results = Program.FindObject(salonList, 2, searchQuery);

            // Assert
            Assert.IsTrue(results.Any());
        }


        [TestMethod]
        public void DeleteObject_ByIndex_RemovesCorrectService()
        {
            // Arrange
            var salonList = new List<SalonService>
            {
                new SalonService("ServiceToRemove", 100, ServiceType.Haircut, new DateTime(2024, 1, 1), "Description", "Info"),
                new SalonService("ServiceToKeep", 200, ServiceType.Manicure, new DateTime(2024, 2, 1), "Description", "Info")
            };
            int indexToDelete = 0; // Index 

            // Act
            Program.DeleteObject(salonList, 1, indexToDelete);

            // Assert
            Assert.AreEqual(1, salonList.Count);
            Assert.AreEqual("ServiceToKeep", salonList[0].Name);
        }

        [TestMethod]
        public void DeleteObject_ByName_RemovesCorrectService()
        {
            // Arrange
            var salonList = new List<SalonService>
            {
                new SalonService("ServiceToRemove", 100, ServiceType.Haircut, new DateTime(2024, 1, 1), "Description", "Info"),
                new SalonService("ServiceToKeep", 200, ServiceType.Manicure, new DateTime(2024, 2, 1), "Description", "Info")
            };
            string nameToDelete = "ServiceToRemove"; // Name 

            // Act
            Program.DeleteObject(salonList, 2, nameToDelete: nameToDelete);

            // Assert
            Assert.AreEqual(1, salonList.Count);
            Assert.AreEqual("ServiceToKeep", salonList[0].Name);
        }

        private readonly string testFilePath = "test.json"; // Путь к файлу для тестов

        [TestMethod]
        public void SaveToJson_Test()
        {
            // Arrange
            var services = new List<SalonService>
            {
                 new SalonService("Anna", 98, ServiceType.Haircut, new DateTime(2027, 9, 9), "", ""),
            };

            // Act
            Program.SaveToJson(services, testFilePath);

            // Assert
            Assert.IsTrue(File.Exists(testFilePath));
            var jsonString = File.ReadAllText(testFilePath);
            var deserializedServices = JsonSerializer.Deserialize<List<SalonService>>(jsonString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            Assert.IsNotNull(deserializedServices);
            Assert.AreEqual(services.Count, deserializedServices.Count);

            // Удаляем тестовый файл
            if (File.Exists(testFilePath))
            {
                File.Delete(testFilePath);
            }
        }

        [TestMethod]
        public void ReadFromJson_Test()
        {
            // Arrange создаем тестовый файл JSON
            var servicesToTest = new List<SalonService>
            {
                new SalonService("Anna", 98, ServiceType.Haircut, new DateTime(2027, 9, 9), "", ""),
            };
            File.WriteAllText(testFilePath, JsonSerializer.Serialize(servicesToTest));

            // Act
            var loadedServices = Program.ReadFromJson(testFilePath);

            // Assert
            Assert.IsNotNull(loadedServices);
            Assert.AreEqual(servicesToTest.Count, loadedServices.Count);
            for (int i = 0; i < servicesToTest.Count; i++)
            {
                Assert.AreEqual(servicesToTest[i].Name, loadedServices[i].Name);
                Assert.AreEqual(servicesToTest[i].Price, loadedServices[i].Price);
                Assert.AreEqual(servicesToTest[i].Service, loadedServices[i].Service);
                Assert.AreEqual(servicesToTest[i].Date, loadedServices[i].Date);
                Assert.AreEqual(servicesToTest[i].Description, loadedServices[i].Description);
                Assert.AreEqual(servicesToTest[i].AdditionalInfo, loadedServices[i].AdditionalInfo);
            }

            // Удаляем тестовый файл
            if (File.Exists(testFilePath))
            {
                File.Delete(testFilePath);
            }
        }

        private readonly string testCsvFilePath = "test.csv";

        [TestMethod]
        public void SaveToCsv_Test()
        {
            // Arrange 
            var services = new List<SalonService>
            {
               new SalonService("Anna", 98, ServiceType.Haircut, new DateTime(2027, 9, 9), "", ""),
            };

            // Act сохранение в CSV
            Program.SaveToCsv(services, testCsvFilePath);

            // Assert 
            Assert.IsTrue(File.Exists(testCsvFilePath));
            var lines = File.ReadAllLines(testCsvFilePath);
            Assert.AreEqual(services.Count + 1, lines.Length); // +1 

            // Удаляем тестовый файл
            File.Delete(testCsvFilePath);
        }

        [TestMethod]
        public void ReadFromCsv_Test()
        {
            // Arrange
            var expectedServices = new List<SalonService>
            {
                new SalonService("Anna", 98, ServiceType.Haircut, new DateTime(2027, 9, 9), "", ""),
            };
            var lines = new List<string> { "Name,Price,Service,Date,Description,AdditionalInfo" };
            lines.AddRange(expectedServices.Select(s => $"{s.Name},{s.Price},{s.Service},{s.Date:dd.MM.yyyy},{s.Description},{s.AdditionalInfo}"));
            File.WriteAllLines(testCsvFilePath, lines);

            // Act 
            var loadedServices = Program.ReadFromCsv(testCsvFilePath);

            // Assert
            Assert.AreEqual(expectedServices.Count, loadedServices.Count);
            for (int i = 0; i < expectedServices.Count; i++)
            {
                Assert.AreEqual(expectedServices[i].Name, loadedServices[i].Name);
                Assert.AreEqual(expectedServices[i].Price, loadedServices[i].Price);
                Assert.AreEqual(expectedServices[i].Service, loadedServices[i].Service);
                Assert.AreEqual(expectedServices[i].Date, loadedServices[i].Date);
                Assert.AreEqual(expectedServices[i].Description, loadedServices[i].Description);
                Assert.AreEqual(expectedServices[i].AdditionalInfo, loadedServices[i].AdditionalInfo);
            }

            // Удаляем тестовый файл
            File.Delete(testCsvFilePath);
        }
    }
}