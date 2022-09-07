using System;
using System.Threading.Tasks;
using Application.Repositories.Base;
using Application.Resources;
using AutoFixture;
using FluentAssertions;
using FluentValidation;
using Moq;
using NUnit.Framework;
using Service.Services;

namespace Service.UnitTest.Services
{
    [TestFixture]
    public class CustomerServiceTest : ServiceTestBase<CustomerService>
    {
        private Domain.Models.Customer.CreateCustomer _validModel = null;
        private Domain.Models.Customer.CreateCustomer _invalidModel = null;
        private Domain.Entities.Customer _validEntity = null;

        public CustomerServiceTest()
            : base()
        {
        }

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var validId = Guid.NewGuid();
            var validName = "test";
            var validAge = Random.Shared.Next(1, 150);
            _validModel = _fixture
                .Build<Domain.Models.Customer.CreateCustomer>()
                .With(x => x.Name, validName)
                .With(x => x.Age, validAge)
                .Create();
            _validEntity = _fixture
                .Build<Domain.Entities.Customer>()
                .With(x => x.Id, validId)
                .With(x => x.Name, validName)
                .With(x => x.Age, validAge)
                .Create();

            var invalidId = Guid.NewGuid();
            string invalidName = null;
            var invalidAge = Random.Shared.Next(151, 99999);
            _invalidModel = _fixture
                .Build<Domain.Models.Customer.CreateCustomer>()
                .With(x => x.Name, invalidName)
                .With(x => x.Age, invalidAge)
                .Create();
        }

        [SetUp]
        public void SetUp()
        {
            _mockUnitOfWork.Reset();
        }

        [Test, Order(1)]
        public async Task GetByIdAsync_WithKnownId_ShouldReturnCustomerData()
        {
            // Arrange
            _mockUnitOfWork.Setup(x => x.Customers.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(_validEntity);

            var customerService = new CustomerService(_mockLogger.Object, _mockUnitOfWork.Object);

            // Act
            var result = await customerService.GetByIdAsync(_validEntity.Id);

            // Assert
            _mockUnitOfWork.Verify(x => x.Customers.GetByIdAsync(_validEntity.Id), Times.Once);

            Assert.True(result.Id == _validEntity.Id);
            Assert.True(result.Name == _validEntity.Name);
            Assert.True(result.Age == _validEntity.Age);
        }

        [Test, Order(2)]
        public async Task GetByIdAsync_WithUnknownId_ShouldThrowException()
        {
            // Arrange
            Domain.Entities.Customer model = null;

            _mockUnitOfWork.Setup(x => x.Customers.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(model);

            var customerService = new CustomerService(_mockLogger.Object, _mockUnitOfWork.Object);

            // Act
            var mockCustomerId = _fixture
                .Create<Guid>();
            Func<Task> func = () => customerService.GetByIdAsync(mockCustomerId);

            // Assert
            await func.Should()
                .ThrowAsync<Exception>()
                .WithMessage(ErrorMessage._00001);
        }

        [Test, Order(3)]
        public async Task CreateAsync_WithExistName_ShouldThrowException()
        {
            // Arrange
            _mockUnitOfWork.Setup(x => x.Customers.GetByUniqueKeyAsync(It.IsAny<string>()))
                .ReturnsAsync(_validEntity);

            var customerService = new CustomerService(_mockLogger.Object, _mockUnitOfWork.Object);

            // Act
            Func<Task> func = () => customerService.CreateAsync(_validModel);

            // Assert
            await func.Should()
                .ThrowAsync<Exception>()
                .WithMessage(ErrorMessage._00002);

            _mockUnitOfWork.Verify(x => x.Customers.GetByUniqueKeyAsync(It.IsAny<string>()), Times.Once);
            _mockUnitOfWork.Verify(x => x.Customers.AddAsync(It.IsAny<Domain.Entities.Customer>()), Times.Never);
        }

        [Test, Order(5)]
        public async Task CreateAsync_WithInvalidData_ShouldThrowException()
        {
            // Arrange
            Domain.Entities.Customer model = null;

            _mockUnitOfWork.Setup(x => x.Customers.GetByUniqueKeyAsync(It.IsAny<string>()))
                .ReturnsAsync(model);
            _mockUnitOfWork.Setup(x => x.Customers.AddAsync(It.IsAny<Domain.Entities.Customer>()))
                .ReturnsAsync(true);

            var customerService = new CustomerService(_mockLogger.Object, _mockUnitOfWork.Object);

            // Act
            Func<Task> func = () => customerService.CreateAsync(_invalidModel);

            // Assert
            await func.Should()
                .ThrowAsync<ValidationException>();

            _mockUnitOfWork.Verify(x => x.Customers.GetByUniqueKeyAsync(_invalidModel.Name), Times.Never);
            _mockUnitOfWork.Verify(x => x.Customers.AddAsync(It.IsAny<Domain.Entities.Customer>()), Times.Never);
        }

        [Test, Order(5)]
        public async Task CreateAsync_WithValidData_ShouldReturnCustomerId()
        {
            // Arrange
            Domain.Entities.Customer model = null;

            _mockUnitOfWork.Setup(x => x.Customers.GetByUniqueKeyAsync(It.IsAny<string>()))
                .ReturnsAsync(model);
            _mockUnitOfWork.Setup(x => x.Customers.AddAsync(It.IsAny<Domain.Entities.Customer>()))
                .ReturnsAsync(true);

            var customerService = new CustomerService(_mockLogger.Object, _mockUnitOfWork.Object);

            // Act
            var result = await customerService.CreateAsync(_validModel);

            // Assert
            _mockUnitOfWork.Verify(x => x.Customers.GetByUniqueKeyAsync(_validModel.Name), Times.Once);
            _mockUnitOfWork.Verify(x => x.Customers.AddAsync(It.IsAny<Domain.Entities.Customer>()), Times.Once);

            Assert.True(result != Guid.Empty);
        }

        [Test, Order(6)]
        public async Task CreateAsync_WithUnsuccessful_ShouldThrowException()
        {
            // Arrange
            Domain.Entities.Customer model = null;

            _mockUnitOfWork.Setup(x => x.Customers.GetByUniqueKeyAsync(It.IsAny<string>()))
                .ReturnsAsync(model);
            _mockUnitOfWork.Setup(x => x.Customers.AddAsync(It.IsAny<Domain.Entities.Customer>()))
                .ReturnsAsync(false);

            var customerService = new CustomerService(_mockLogger.Object, _mockUnitOfWork.Object);

            // Act
            Func<Task> func = () => customerService.CreateAsync(_validModel);

            // Assert
            await func.Should()
                .ThrowAsync<Exception>()
                .WithMessage(ErrorMessage._99999);

            _mockUnitOfWork.Verify(x => x.Customers.GetByUniqueKeyAsync(_validModel.Name), Times.Once);
            _mockUnitOfWork.Verify(x => x.Customers.AddAsync(It.IsAny<Domain.Entities.Customer>()), Times.Once);
        }
    }
}