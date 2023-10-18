using System.Net.Mime;
using System.Text;
using Api.IntegrationTest2.Attributes;
using Application.Handlers.Commands.Customer;
using Application.Handlers.Queries.Customer;
using Domain.Models;
using FluentAssertions;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using Persistence;
using Xunit;

namespace Api.IntegrationTest2.Controllers
{
    [Collection("IntegrationTestBaseCollection")]
    [TestCaseOrderer(
        ordererTypeName: "Api.IntegrationTest2.Attributes.PriorityOrderer",
        ordererAssemblyName: "Api.IntegrationTest2"
    )]
    public class CustomerControllerTest
    {
        private static Guid _customerId = Guid.Empty;
        private static string _customerName = "Jittichai";
        private static int _customerAge = 35;
        private readonly HttpClient _client;

        public CustomerControllerTest(TestWebApplicationFactory<Program, DatabaseContext> factory)
        {
            _client = factory.CreateClient();
            _client.DefaultRequestHeaders.Add(HeaderNames.Authorization, TestAuthenticationHandler.TOKEN);
        }

        [Fact, TestPriority(1)]
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

            // Assert
            response.EnsureSuccessStatusCode();

            if (response.IsSuccessStatusCode)
            {
                var resultJson = await response.Content.ReadAsStringAsync();
                var resultObject = JsonConvert.DeserializeObject<Response<CreateCustomerResponse>>(resultJson);

                resultObject?.Status.Should().Be(EnumResponseStatus.Ok);

                _customerId = resultObject?.Data?.Id ?? Guid.Empty;
            }
        }

        [Fact, TestPriority(2)]
        public async Task GetCustomerById_WithValidData_ShouldBeRetrieved()
        {
            // Arrange

            // Act
            var response = await _client.GetAsync($"/api/customers/{_customerId}").ConfigureAwait(false);

            // Assert
            response.EnsureSuccessStatusCode();

            if (response.IsSuccessStatusCode)
            {
                var resultJson = await response.Content.ReadAsStringAsync();
                var resultObject = JsonConvert.DeserializeObject<Response<GetCustomerByIdResponse>>(resultJson);

                resultObject?.Status.Should().Be(EnumResponseStatus.Ok);
                resultObject?.Data.Should().NotBeNull();
                resultObject?.Data.Id.Should().Be(_customerId);
                resultObject?.Data.Name.Should().Be(_customerName);
                resultObject?.Data.Age.Should().Be(_customerAge);
            }
        }

        [Fact, TestPriority(3)]
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

            // Assert
            response.EnsureSuccessStatusCode();

            if (response.IsSuccessStatusCode)
            {
                var resultJson = await response.Content.ReadAsStringAsync();
                var resultObject = JsonConvert.DeserializeObject<Response<UpdateCustomerResponse>>(resultJson);

                resultObject?.Status.Should().Be(EnumResponseStatus.Ok);
                resultObject?.Data.Should().NotBeNull();
                resultObject?.Data.Id.Should().Be(_customerId);
                resultObject?.Data.Name.Should().Be(_customerName);
                resultObject?.Data.Age.Should().Be(_customerAge);
            }
        }

        [Fact, TestPriority(4)]
        public async Task DeleteCustomer_WithValidData_ShouldBeDeleted()
        {
            // Arrange

            // Act
            var response = await _client.DeleteAsync($"/api/customers/{_customerId}").ConfigureAwait(false);

            // Assert
            response.EnsureSuccessStatusCode();

            if (response.IsSuccessStatusCode)
            {
                var resultJson = await response.Content.ReadAsStringAsync();
                var resultObject = JsonConvert.DeserializeObject<Response<DeleteCustomerResponse>>(resultJson);

                resultObject?.Status.Should().Be(EnumResponseStatus.Ok);
                resultObject?.Data.Should().NotBeNull();
                resultObject?.Data.Success.Should().Be(true);
            }
        }

        [Fact, TestPriority(5)]
        public async Task GetCustomerById_WithInvalidData_ShouldBeReturnStatusError()
        {
            // Arrange

            // Act
            var response = await _client.GetAsync($"/api/customers/{_customerId}").ConfigureAwait(false);

            // Assert
            Assert.False(response.IsSuccessStatusCode);

            if (!response.IsSuccessStatusCode)
            {
                var resultJson = await response.Content.ReadAsStringAsync();
                var resultObject = JsonConvert.DeserializeObject<Response<GetCustomerByIdResponse>>(resultJson);

                resultObject?.Status.Should().Be(EnumResponseStatus.Error);
            }
        }
    }
}