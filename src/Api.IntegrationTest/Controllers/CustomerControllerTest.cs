using System;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Application.CQRS.Commands.Customer;
using Application.CQRS.Queries.Customer;
using Domain.Models;
using FluentAssertions;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Api.IntegrationTest.Controllers
{
    [TestFixture]
    public class CustomerControllerTest
    {
        private TestWebApplicationFactory _factory;
        private HttpClient _client;

        private Guid _customerId = Guid.Empty;
        private string _customerName = "Jittichai";
        private int _customerAge = 35;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _factory = new TestWebApplicationFactory();
            _client = _factory.CreateClient();

            _client.DefaultRequestHeaders.Add(HeaderNames.Authorization, TestAuthenticationHandler.TOKEN);
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _client.Dispose();
            _factory.Dispose();
        }

        [Test, Order(1)]
        public async Task CreateCustomer_WithValidData_ShouldBeCreated()
        {
            // Arrange
            var payload = new Domain.Models.Customer.CreateCustomer
            {
                Name = _customerName,
                Age = _customerAge
            };
            var payloadJson = JsonConvert.SerializeObject(payload);
            var payloadStringContent = new StringContent(payloadJson, Encoding.UTF8, MediaTypeNames.Application.Json);

            // Act
            var response = await _client.PostAsync("/api/customers", payloadStringContent).ConfigureAwait(false);
            var resultJson = await response.Content.ReadAsStringAsync();
            var resultObject = JsonConvert.DeserializeObject<Response<CreateCustomerResponse>>(resultJson);

            // Assert
            response.EnsureSuccessStatusCode();
            resultObject.Status.Should().Be(EnumResponseStatus.Ok);

            _customerId = resultObject.Data.Id;
        }

        [Test, Order(2)]
        public async Task GetCustomerById_WithValidData_ShouldBeRetrieved()
        {
            // Arrange

            // Act
            var response = await _client.GetAsync($"/api/customers/{_customerId}").ConfigureAwait(false);
            var resultJson = await response.Content.ReadAsStringAsync();
            var resultObject = JsonConvert.DeserializeObject<Response<GetCustomerByIdResponse>>(resultJson);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            resultObject.Status.Should().Be(EnumResponseStatus.Ok);

            resultObject.Data.Should().NotBeNull();
            resultObject.Data.Id.Should().Be(_customerId);
            resultObject.Data.Name.Should().Be(_customerName);
            resultObject.Data.Age.Should().Be(_customerAge);
        }

        [Test, Order(3)]
        public async Task UpdateCustomer_WithValidData_ShouldBeUpdated()
        {
            // Arrange
            _customerName = "Kittisak";
            _customerAge = 38;
            var payload = new Domain.Models.Customer.UpdateCustomer
            {
                Name = _customerName,
                Age = _customerAge
            };
            var payloadJson = JsonConvert.SerializeObject(payload);
            var payloadStringContent = new StringContent(payloadJson, Encoding.UTF8, MediaTypeNames.Application.Json);

            // Act
            var response = await _client.PutAsync($"/api/customers/{_customerId}", payloadStringContent).ConfigureAwait(false);
            var resultJson = await response.Content.ReadAsStringAsync();
            var resultObject = JsonConvert.DeserializeObject<Response<UpdateCustomerResponse>>(resultJson);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            resultObject.Status.Should().Be(EnumResponseStatus.Ok);

            resultObject.Data.Should().NotBeNull();
            resultObject.Data.Id.Should().Be(_customerId);
            resultObject.Data.Name.Should().Be(_customerName);
            resultObject.Data.Age.Should().Be(_customerAge);
        }

        [Test, Order(4)]
        public async Task DeleteCustomer_WithValidData_ShouldBeDeleted()
        {
            // Arrange

            // Act
            var response = await _client.DeleteAsync($"/api/customers/{_customerId}").ConfigureAwait(false);
            var resultJson = await response.Content.ReadAsStringAsync();
            var resultObject = JsonConvert.DeserializeObject<Response<DeleteCustomerResponse>>(resultJson);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            resultObject.Status.Should().Be(EnumResponseStatus.Ok);

            resultObject.Data.Should().NotBeNull();
            resultObject.Data.Success.Should().Be(true);
        }

        [Test, Order(5)]
        public async Task GetCustomerById_WithInvalidData_ShouldBeReturnStatusError()
        {
            // Arrange

            // Act
            var response = await _client.GetAsync($"/api/customers/{_customerId}").ConfigureAwait(false);
            var resultJson = await response.Content.ReadAsStringAsync();
            var resultObject = JsonConvert.DeserializeObject<Response<GetCustomerByIdResponse>>(resultJson);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            resultObject.Status.Should().Be(EnumResponseStatus.Error);
        }
    }
}