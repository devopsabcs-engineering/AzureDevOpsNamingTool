using AzureNaming.Tool.Models;
using Moq;

namespace AzureNaming.Tool.Services
{
    public class ResourceEnvironmentServiceTests
    {
        private readonly ServiceResponse _expectedResourceEnvironmentServiceResponse;
        // readonly ServiceResponse _serviceResponse;
        private readonly Mock<IAdminLogService> _adminLogServiceMock;
        private readonly ResourceEnvironmentService _resourceEnvironmentService;

        public ResourceEnvironmentServiceTests()
        {
            _expectedResourceEnvironmentServiceResponse = new ServiceResponse
            {
                ResponseMessage = "",
                Success = true,
                ResponseObject = new List<ResourceEnvironment>
                {
                    new ResourceEnvironment() { Id = 1, Name = "Development", ShortName = "dev", SortOrder = 1 },
                    new ResourceEnvironment() { Id = 2, Name = "Production", ShortName = "prd", SortOrder = 2 },
                    new ResourceEnvironment() { Id = 3, Name = "Sandbox", ShortName = "sbx", SortOrder = 3 },
                    new ResourceEnvironment() { Id = 4, Name = "Shared", ShortName = "shd", SortOrder = 4 },
                    new ResourceEnvironment() { Id = 5, Name = "Staging", ShortName = "stg", SortOrder = 5 },
                    new ResourceEnvironment() { Id = 6, Name = "Test", ShortName = "tst", SortOrder = 6 },
                    new ResourceEnvironment() { Id = 7, Name = "UAT", ShortName = "uat", SortOrder = 7 }
                }
            };

            //_serviceResponse = new ServiceResponse() {
            //    Success = true,
            //     ResponseMessage = "OK",
            //      ResponseObject = new List<AdminLogMessage> { new AdminLogMessage() { CreatedOn = new DateTime(2023,8,31), Id=5, Message="OK", Title="hello", Source="Mock" } }
            //};

            _adminLogServiceMock = new Mock<IAdminLogService>();
            //_adminLogServiceMock.Setup(x => x.GetItems())
            //    .Returns(_serviceResponse);

            _resourceEnvironmentService = new ResourceEnvironmentService(_adminLogServiceMock.Object);
        }

        [Fact]
        public void ShouldReturnAllDefaultResourceEnvironments()
        {
            // Arrange
            // done in constructor

            // Act
            ServiceResponse actualResourceEnvironmentServiceResponse = _resourceEnvironmentService.GetItems().Result;

            // Assert
            Assert.NotNull(actualResourceEnvironmentServiceResponse);
            Assert.Equal(_expectedResourceEnvironmentServiceResponse.ResponseMessage, actualResourceEnvironmentServiceResponse.ResponseMessage);
            Assert.Equal(_expectedResourceEnvironmentServiceResponse.Success, actualResourceEnvironmentServiceResponse.Success);
            Assert.IsType<List<ResourceEnvironment>>(actualResourceEnvironmentServiceResponse.ResponseObject);
            Assert.Equal(_expectedResourceEnvironmentServiceResponse.ResponseObject.Count, actualResourceEnvironmentServiceResponse.ResponseObject.Count);
            for (int i = 0; i < _expectedResourceEnvironmentServiceResponse.ResponseObject.Count; i++)
            {
                Assert.Equal(_expectedResourceEnvironmentServiceResponse.ResponseObject[i].Id, actualResourceEnvironmentServiceResponse.ResponseObject[i].Id);
                Assert.Equal(_expectedResourceEnvironmentServiceResponse.ResponseObject[i].Name, actualResourceEnvironmentServiceResponse.ResponseObject[i].Name);
                Assert.Equal(_expectedResourceEnvironmentServiceResponse.ResponseObject[i].ShortName, actualResourceEnvironmentServiceResponse.ResponseObject[i].ShortName);
                Assert.Equal(_expectedResourceEnvironmentServiceResponse.ResponseObject[i].SortOrder, actualResourceEnvironmentServiceResponse.ResponseObject[i].SortOrder);
            };            

        }

    }
}
