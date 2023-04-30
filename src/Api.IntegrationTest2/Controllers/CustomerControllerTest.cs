using System.Net.Mime;
using System.Text;
using Api.IntegrationTest2.Attributes;
using Application.CQRS.Commands.Customer;
using Application.CQRS.Queries.Customer;
using Domain.Models;
using FluentAssertions;
using Newtonsoft.Json;
using Persistence;
using Xunit;

namespace Api.IntegrationTest2.Controllers
{
    [TestCaseOrderer("Api.IntegrationTest2.Attributes.PriorityOrderer", "Api.IntegrationTest2")]
    public class CustomerControllerTest : IntegrationTestBase
    {
        private static Guid _customerId = Guid.Empty;
        private static string _customerName = "Jittichai";
        private static int _customerAge = 35;

        public CustomerControllerTest(TestWebApplicationFactory<Program, DatabaseContext> factory)
            : base(factory)
        {
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
            var response = await Client.PostAsync("/api/customers", payloadStringContent).ConfigureAwait(false);
            var resultJson = await response.Content.ReadAsStringAsync();
            var resultObject = JsonConvert.DeserializeObject<Response<CreateCustomerResponse>>(resultJson);
            _customerId = resultObject?.Data?.Id ?? Guid.Empty;

            // Assert
            response.EnsureSuccessStatusCode();

            resultObject?.Status.Should().Be(EnumResponseStatus.Ok);
        }

        [Fact, TestPriority(2)]
        public async Task GetCustomerById_WithValidData_ShouldBeRetrieved()
        {
            // Arrange

            // Act
            var response = await Client.GetAsync($"/api/customers/{_customerId}").ConfigureAwait(false);
            var resultJson = await response.Content.ReadAsStringAsync();
            var resultObject = JsonConvert.DeserializeObject<Response<GetCustomerByIdResponse>>(resultJson);

            // Assert
            response.EnsureSuccessStatusCode();

            resultObject?.Status.Should().Be(EnumResponseStatus.Ok);
            resultObject?.Data.Should().NotBeNull();
            resultObject?.Data.Id.Should().Be(_customerId);
            resultObject?.Data.Name.Should().Be(_customerName);
            resultObject?.Data.Age.Should().Be(_customerAge);
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
            var response = await Client.PutAsync($"/api/customers/{_customerId}", payloadStringContent).ConfigureAwait(false);
            var resultJson = await response.Content.ReadAsStringAsync();
            var resultObject = JsonConvert.DeserializeObject<Response<UpdateCustomerResponse>>(resultJson);

            // Assert
            response.EnsureSuccessStatusCode();

            resultObject?.Status.Should().Be(EnumResponseStatus.Ok);
            resultObject?.Data.Should().NotBeNull();
            resultObject?.Data.Id.Should().Be(_customerId);
            resultObject?.Data.Name.Should().Be(_customerName);
            resultObject?.Data.Age.Should().Be(_customerAge);
        }

        [Fact, TestPriority(4)]
        public async Task DeleteCustomer_WithValidData_ShouldBeDeleted()
        {
            // Arrange

            // Act
            var response = await Client.DeleteAsync($"/api/customers/{_customerId}").ConfigureAwait(false);
            var resultJson = await response.Content.ReadAsStringAsync();
            var resultObject = JsonConvert.DeserializeObject<Response<DeleteCustomerResponse>>(resultJson);

            // Assert
            response.EnsureSuccessStatusCode();

            resultObject?.Status.Should().Be(EnumResponseStatus.Ok);
            resultObject?.Data.Should().NotBeNull();
            resultObject?.Data.Success.Should().Be(true);
        }

        [Fact, TestPriority(5)]
        public async Task GetCustomerById_WithInvalidData_ShouldBeReturnStatusError()
        {
            // Arrange

            // Act
            var response = await Client.GetAsync($"/api/customers/{_customerId}").ConfigureAwait(false);
            var resultJson = await response.Content.ReadAsStringAsync();
            var resultObject = JsonConvert.DeserializeObject<Response<GetCustomerByIdResponse>>(resultJson);

            // Assert
            response.EnsureSuccessStatusCode();

            resultObject?.Status.Should().Be(EnumResponseStatus.Error);
        }
    }
}