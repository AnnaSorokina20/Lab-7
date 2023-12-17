using Lab7_Sorokina_program;
namespace TestProject_SalonService
{
    [TestClass]
    public class SalonServiceTest
    {
        [TestMethod]
        public void SetName_ValidName_SetsName()
        {
            // Arrange
            var service = new SalonService();
            var validName = "ValidName";

            // Act
            service.SetName(validName);

            // Assert
            Assert.AreEqual(validName, service.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SetName_InvalidName_ThrowsArgumentException()
        {
            // Arrange
            var service = new SalonService();
            var invalidName = "12"; // Ім'я закоротке та складається не  з латинських літер

            // Act
            service.SetName(invalidName);

            // Assert не потрібен так як ми очікуємо Exception
        }


        [TestMethod]
        public void UpdateServiceInfo_ValidPriceAndDate_UpdatesPriceAndDate()
        {
            // Arrange
            var service = new SalonService("Test", 100, ServiceType.Haircut, DateTime.Now, "Description");
            var newPrice = 150.0;
            var newDate = DateTime.Now.AddDays(1);

            // Act
            service.UpdateServiceInfo(newPrice, newDate);

            // Assert
            Assert.AreEqual(newPrice, service.Price);
            Assert.AreEqual(newDate, service.Date);
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void UpdateServiceInfo_InvalidPrice_ThrowsArgumentException()
        {
            // Arrange
            var service = new SalonService("Test", 100, ServiceType.Haircut, DateTime.Now, "Description");
            var invalidPrice = -50.0;

            // Act
            service.UpdateServiceInfo(invalidPrice, DateTime.Now);

            // Assert не потрібен так як ми очікуємо Exception
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "New date should be in the future.")]
        public void UpdateServiceInfo_InvalidDate_ThrowsArgumentException()
        {
            // Arrange
            var service = new SalonService("Test", 100, ServiceType.Haircut, DateTime.Now, "Description");
            var invalidDate = DateTime.Now.AddDays(-1);

            // Act
            service.UpdateServiceInfo(150.0, invalidDate);

            // Assert не потрібен так як ми очікуємо Exception
        }



        [TestMethod]
        public void Parse_ValidString_ReturnsSalonServiceObject()
        {
            // Arrange
            var validString = "Name,200,Haircut,01.01.2024,Description,AdditionalInfo";

            // Act
            var result = SalonService.Parse(validString);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Name", result.Name);
            Assert.AreEqual(200, result.Price);
            Assert.AreEqual(ServiceType.Haircut, result.Service);
            Assert.AreEqual(new DateTime(2024, 01, 01), result.Date);
            Assert.AreEqual("Description", result.Description);
            Assert.AreEqual("AdditionalInfo", result.AdditionalInfo);

        }

        [TestMethod]
        public void TryParse_InvalidString_ReturnsFalseAndNullObject()
        {
            // Arrange
            var invalidString = "Invalid";

            // Act
            var success = SalonService.TryParse(invalidString, out var result);

            // Assert
            Assert.IsFalse(success);
            Assert.IsNull(result);
        }


        [TestMethod]
        public void FindMostExpensiveService_WithValidList_ReturnsMostExpensiveService()
        {
            // Arrange
            var services = new List<SalonService>
            {
                new SalonService("Haircut", 30.00, ServiceType.Haircut, DateTime.Now),
                new SalonService("Manicure", 25.00, ServiceType.Manicure, DateTime.Now),
                new SalonService("Massage", 50.00, ServiceType.Massage, DateTime.Now)
            };

            // Act
            var mostExpensiveService = SalonService.FindMostExpensiveService(services);

            // Assert
            Assert.IsNotNull(mostExpensiveService);
            Assert.AreEqual(50.00, mostExpensiveService.Price);
        }
    }
}