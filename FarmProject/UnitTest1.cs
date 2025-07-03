using FarmApi.Controllers;
using FarmBusiness.Services;
using FarmTradeDataLayer.Repository;
using FarmTradeEntity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
namespace FarmProject
{
    [TestFixture]
    public class Tests
    {
        private Mock<IAddressRepo> _mockRepo;
        private AddressService _addressService;
        private AddressController _addressController;

        [SetUp]
        public void Setup()
        {
            _mockRepo = new Mock<IAddressRepo>();
            _addressService = new AddressService(_mockRepo.Object);
            _addressController = new AddressController(_addressService);
        }

        [Test]
        public void AddAddress_ShouldReturnOk()
        {
            // Arrange
            var address = new Address
            {
                street = "123 Main St",
                city = "Chennai",
                state = "TN",
                pinCode = "600001",
                country = "India",
                Phone = "9999999999",
                userId = Guid.NewGuid()
            };

            // Act
            var result = _addressController.AddAddress(address);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var ok = result as OkObjectResult;
            Assert.AreEqual("Address Added successfully", ok.Value);
        }
        //int a = 9; int b=4;
        public static int calc(int a,int b) => a+b ;
        [Test]
        public void Test1()
        {
            Tests tests = new Tests();
            int result = calc(2, 3);
            Assert.AreEqual(5, result);
        }
    }
    [TestFixture]
    public class Test2
    {
        [Test]
        public void Test1()
        {
            Tests tests = new Tests();
            Assert.AreEqual(1, 3-2);
        }
    }

}