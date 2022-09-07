using System;
using Application.Repositories.Base;
using AutoFixture;
using Microsoft.Extensions.Logging;
using Moq;

namespace Service.UnitTest.Services
{
    public class ServiceTestBase<T> where T : class
    {
        protected Mock<ILogger<T>> _mockLogger;
        protected Mock<IUnitOfWork> _mockUnitOfWork;
        protected Fixture _fixture;

        public ServiceTestBase()
        {
            _mockLogger = new Mock<ILogger<T>>(MockBehavior.Strict);
            _mockLogger.Setup(
                l => l.Log(
                    It.IsAny<LogLevel>(),
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()));

            _mockUnitOfWork = new Mock<IUnitOfWork>(MockBehavior.Strict);
            _fixture = new Fixture();
        }
    }
}