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
        private ServiceResponse _expectedTestResourceEnvironmentServiceResponse;

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

            _expectedTestResourceEnvironmentServiceResponse = new ServiceResponse()
            {
                Success = true,
                ResponseMessage = "",
                ResponseObject = new ResourceEnvironment() { Id = 6, Name = "Test", ShortName = "tst", SortOrder = 6 }
            };

            _adminLogServiceMock = new Mock<IAdminLogService>();
            //_adminLogServiceMock.Setup(x => x.GetItems())
            //    .Returns(_serviceResponse);

            _resourceEnvironmentService = new ResourceEnvironmentService(_adminLogServiceMock.Object);
        }

        [Fact]
        public void ShouldReturnTestResourceEnvironment()
        {
            // Arrange
            // done in constructor


            // Act
            ServiceResponse actualTestResourceEnvironmentServiceResponse = _resourceEnvironmentService.GetItem(6).Result;

            // Assert
            Assert.NotNull(actualTestResourceEnvironmentServiceResponse);
            Assert.Equal(_expectedTestResourceEnvironmentServiceResponse.ResponseMessage, actualTestResourceEnvironmentServiceResponse.ResponseMessage);
            Assert.Equal(_expectedTestResourceEnvironmentServiceResponse.Success, actualTestResourceEnvironmentServiceResponse.Success);
            Assert.IsType<ResourceEnvironment>(actualTestResourceEnvironmentServiceResponse.ResponseObject);
            //check all fields
            Assert.Equal(_expectedTestResourceEnvironmentServiceResponse.ResponseObject.Id, actualTestResourceEnvironmentServiceResponse.ResponseObject.Id);
            Assert.Equal(_expectedTestResourceEnvironmentServiceResponse.ResponseObject.Name, actualTestResourceEnvironmentServiceResponse.ResponseObject.Name);
            Assert.Equal(_expectedTestResourceEnvironmentServiceResponse.ResponseObject.ShortName, actualTestResourceEnvironmentServiceResponse.ResponseObject.ShortName);
            Assert.Equal(_expectedTestResourceEnvironmentServiceResponse.ResponseObject.SortOrder, actualTestResourceEnvironmentServiceResponse.ResponseObject.SortOrder);

        }

        [Fact]
        public void ShouldReturnResourceEnvironmentNotFoundError()
        {
            // Arrange
            // done in constructor
            _expectedTestResourceEnvironmentServiceResponse = new ServiceResponse()
            {
                Success = false,
                ResponseMessage = "",
                ResponseObject = "Resource Environment not found!"
            };

            // Act
            ServiceResponse actualTestResourceEnvironmentServiceResponse = _resourceEnvironmentService.GetItem(8).Result;

            // Assert
            Assert.NotNull(actualTestResourceEnvironmentServiceResponse);
            Assert.Equal(_expectedTestResourceEnvironmentServiceResponse.ResponseMessage, actualTestResourceEnvironmentServiceResponse.ResponseMessage);
            Assert.Equal(_expectedTestResourceEnvironmentServiceResponse.Success, actualTestResourceEnvironmentServiceResponse.Success);
            Assert.IsType<string>(actualTestResourceEnvironmentServiceResponse.ResponseObject);
            //check all fields
            Assert.Equal(_expectedTestResourceEnvironmentServiceResponse.ResponseObject, actualTestResourceEnvironmentServiceResponse.ResponseObject);

        }

        [Fact]
        public void ShouldAddAResourceEnvironment()
        {
            // Arrange
            // done in constructor
            _expectedTestResourceEnvironmentServiceResponse = new ServiceResponse()
            {
                Success = true,
                ResponseMessage = "",
                ResponseObject = "Resource Environment added/updated!"
            };

            ResourceEnvironment item = new ResourceEnvironment()
            {
                //Id = 0,
                Name = "Mock",
                ShortName = "mck",
                //SortOrder = 0
            };

            // Act
            ServiceResponse actualTestResourceEnvironmentServiceResponse = _resourceEnvironmentService.PostItem(item).Result;

            // Assert
            Assert.NotNull(actualTestResourceEnvironmentServiceResponse);
            Assert.Equal(_expectedTestResourceEnvironmentServiceResponse.ResponseMessage, actualTestResourceEnvironmentServiceResponse.ResponseMessage);
            Assert.Equal(_expectedTestResourceEnvironmentServiceResponse.Success, actualTestResourceEnvironmentServiceResponse.Success);
            Assert.IsType<string>(actualTestResourceEnvironmentServiceResponse.ResponseObject);
            //check all fields
            Assert.Equal(_expectedTestResourceEnvironmentServiceResponse.ResponseObject, actualTestResourceEnvironmentServiceResponse.ResponseObject);

        }

        [Fact]
        public void ShouldAddMultipleResourceEnvironments()
        {
            // Arrange
            // done in constructor
            _expectedTestResourceEnvironmentServiceResponse = new ServiceResponse()
            {
                Success = true,
                ResponseMessage = "",
                ResponseObject = null
            };

            List<ResourceEnvironment> items = new List<ResourceEnvironment> {
                new ResourceEnvironment() { Name = "Mock1", ShortName = "mck1" },
                new ResourceEnvironment() { Name = "Mock2", ShortName = "mck2" },
                new ResourceEnvironment() { Name = "Mock3", ShortName = "mck3" }
            };

            // Act
            ServiceResponse actualTestResourceEnvironmentServiceResponse = _resourceEnvironmentService.PostConfig(items).Result;

            // Assert
            Assert.NotNull(actualTestResourceEnvironmentServiceResponse);
            Assert.Equal(_expectedTestResourceEnvironmentServiceResponse.ResponseMessage, actualTestResourceEnvironmentServiceResponse.ResponseMessage);
            Assert.Equal(_expectedTestResourceEnvironmentServiceResponse.Success, actualTestResourceEnvironmentServiceResponse.Success);
            Assert.Null(actualTestResourceEnvironmentServiceResponse.ResponseObject);

        }

        [Fact]
        public void ShouldDeleteAResourceEnvironment()
        {
            // Arrange
            // done in constructor
            _expectedTestResourceEnvironmentServiceResponse = new ServiceResponse()
            {
                Success = true,
                ResponseMessage = "",
                ResponseObject = null
            };

            // Act
            ServiceResponse actualTestResourceEnvironmentServiceResponse = _resourceEnvironmentService.DeleteItem(6).Result;

            // Assert
            Assert.NotNull(actualTestResourceEnvironmentServiceResponse);
            Assert.Equal(_expectedTestResourceEnvironmentServiceResponse.ResponseMessage, actualTestResourceEnvironmentServiceResponse.ResponseMessage);
            Assert.Equal(_expectedTestResourceEnvironmentServiceResponse.Success, actualTestResourceEnvironmentServiceResponse.Success);
            Assert.Null(actualTestResourceEnvironmentServiceResponse.ResponseObject);

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
