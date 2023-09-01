using AzureNaming.Tool.Models;
using Moq;
using System.Diagnostics;
using Xunit.Abstractions;

namespace AzureNaming.Tool.Services
{
    public class ResourceEnvironmentServiceTests
    {
        //private readonly ServiceResponse _expectedResourceEnvironmentServiceResponse;
        // readonly ServiceResponse _serviceResponse;
        private readonly Mock<IAdminLogService> _adminLogServiceMock;
        private readonly ResourceEnvironmentService _resourceEnvironmentService;
        //private ServiceResponse _expectedTestResourceEnvironmentServiceResponse;

        private readonly ITestOutputHelper output;       

        public ResourceEnvironmentServiceTests(ITestOutputHelper output)
        {                       

            _adminLogServiceMock = new Mock<IAdminLogService>();
            //_adminLogServiceMock.Setup(x => x.GetItems())
            //    .Returns(_serviceResponse);

            _resourceEnvironmentService = new ResourceEnvironmentService(_adminLogServiceMock.Object);
            this.output = output;
        }

        [Fact]
        public void ShouldReturnTestResourceEnvironment()
        {
            // Arrange
            // done in constructor
            ServiceResponse expectedTestResourceEnvironmentServiceResponse = new ServiceResponse()
            {
                Success = true,
                ResponseMessage = "",
                ResponseObject = new ResourceEnvironment() { Id = 6, Name = "Test", ShortName = "tst", SortOrder = 6 }
            };

            // Act
            ServiceResponse actualTestResourceEnvironmentServiceResponse = _resourceEnvironmentService.GetItem(6).Result;

            // Assert
            Assert.NotNull(actualTestResourceEnvironmentServiceResponse);
            Assert.Equal(expectedTestResourceEnvironmentServiceResponse.ResponseMessage, actualTestResourceEnvironmentServiceResponse.ResponseMessage);
            Assert.Equal(expectedTestResourceEnvironmentServiceResponse.Success, actualTestResourceEnvironmentServiceResponse.Success);
            Assert.IsType<ResourceEnvironment>(actualTestResourceEnvironmentServiceResponse.ResponseObject);
            //check all fields
            Assert.Equal(expectedTestResourceEnvironmentServiceResponse.ResponseObject.Id, actualTestResourceEnvironmentServiceResponse.ResponseObject.Id);
            Assert.Equal(expectedTestResourceEnvironmentServiceResponse.ResponseObject.Name, actualTestResourceEnvironmentServiceResponse.ResponseObject.Name);
            Assert.Equal(expectedTestResourceEnvironmentServiceResponse.ResponseObject.ShortName, actualTestResourceEnvironmentServiceResponse.ResponseObject.ShortName);
            Assert.Equal(expectedTestResourceEnvironmentServiceResponse.ResponseObject.SortOrder, actualTestResourceEnvironmentServiceResponse.ResponseObject.SortOrder);

        }

        [Fact]
        public void ShouldReturnResourceEnvironmentNotFoundError()
        {
            // Arrange
            // done in constructor
            ServiceResponse expectedTestResourceEnvironmentServiceResponse = new ServiceResponse()
            {
                Success = false,
                ResponseMessage = "",
                ResponseObject = "Resource Environment not found!"
            };

            // Act
            ServiceResponse actualTestResourceEnvironmentServiceResponse = _resourceEnvironmentService.GetItem(8).Result;

            // Assert
            Assert.NotNull(actualTestResourceEnvironmentServiceResponse);
            Assert.Equal(expectedTestResourceEnvironmentServiceResponse.ResponseMessage, actualTestResourceEnvironmentServiceResponse.ResponseMessage);
            Assert.Equal(expectedTestResourceEnvironmentServiceResponse.Success, actualTestResourceEnvironmentServiceResponse.Success);
            Assert.IsType<string>(actualTestResourceEnvironmentServiceResponse.ResponseObject);
            //check all fields
            Assert.Equal(expectedTestResourceEnvironmentServiceResponse.ResponseObject, actualTestResourceEnvironmentServiceResponse.ResponseObject);

        }

        [Fact]
        public void ShouldAddAResourceEnvironment()
        {
            // Arrange
            // done in constructor
            ServiceResponse expectedTestResourceEnvironmentServiceResponse = new ServiceResponse()
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
            Assert.Equal(expectedTestResourceEnvironmentServiceResponse.ResponseMessage, actualTestResourceEnvironmentServiceResponse.ResponseMessage);
            Assert.Equal(expectedTestResourceEnvironmentServiceResponse.Success, actualTestResourceEnvironmentServiceResponse.Success);
            Assert.IsType<string>(actualTestResourceEnvironmentServiceResponse.ResponseObject);
            //check all fields
            Assert.Equal(expectedTestResourceEnvironmentServiceResponse.ResponseObject, actualTestResourceEnvironmentServiceResponse.ResponseObject);

        }

        [Fact]
        public void ShouldAddMultipleResourceEnvironments()
        {
            // Arrange
            // done in constructor
            ServiceResponse expectedTestResourceEnvironmentServiceResponse = new ServiceResponse()
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
            Assert.Equal(expectedTestResourceEnvironmentServiceResponse.ResponseMessage, actualTestResourceEnvironmentServiceResponse.ResponseMessage);
            Assert.Equal(expectedTestResourceEnvironmentServiceResponse.Success, actualTestResourceEnvironmentServiceResponse.Success);
            Assert.Null(actualTestResourceEnvironmentServiceResponse.ResponseObject);

        }

        [Fact]
        public async Task ShouldDeleteAResourceEnvironmentAsync()
        {
            // Arrange
            // NOT done in constructor -- all local due to flakyness

            var adminLogServiceMock = new Mock<IAdminLogService>();
            
            var resourceEnvironmentService = new ResourceEnvironmentService(adminLogServiceMock.Object);

            ServiceResponse expectedTestResourceEnvironmentServiceResponse = new ServiceResponse()
            {
                Success = true,
                ResponseMessage = "",
                ResponseObject = null
            };

            // Act
            var allResourceEnvironments = resourceEnvironmentService.GetItems().Result;
            foreach (var item in allResourceEnvironments.ResponseObject)
            {
                Trace.WriteLine($"{item.Id}: {item.Name}");
            }
            if (allResourceEnvironments.ResponseObject.Count < 7)
            {
                //flakiness explained:
                //due to add multiple is 3 instead of 7
                //PostConfig re-writes the environments

                Trace.WriteLine("need to re-post config...");

                //re-post config
                var originalItems = new List<ResourceEnvironment>
                {
                    new ResourceEnvironment() { Id = 1, Name = "Development", ShortName = "dev", SortOrder = 1 },
                    new ResourceEnvironment() { Id = 2, Name = "Production", ShortName = "prd", SortOrder = 2 },
                    new ResourceEnvironment() { Id = 3, Name = "Sandbox", ShortName = "sbx", SortOrder = 3 },
                    new ResourceEnvironment() { Id = 4, Name = "Shared", ShortName = "shd", SortOrder = 4 },
                    new ResourceEnvironment() { Id = 5, Name = "Staging", ShortName = "stg", SortOrder = 5 },
                    new ResourceEnvironment() { Id = 6, Name = "Test", ShortName = "tst", SortOrder = 6 },
                    new ResourceEnvironment() { Id = 7, Name = "UAT", ShortName = "uat", SortOrder = 7 }
                };
                await resourceEnvironmentService.PostConfig(originalItems);
            } else
            {
                Trace.WriteLine("no need to repost");
            }

            ServiceResponse actualTestResourceEnvironmentServiceResponse = resourceEnvironmentService.DeleteItem(6).Result;

            // Assert
            Assert.NotNull(actualTestResourceEnvironmentServiceResponse);
            Assert.Equal(expectedTestResourceEnvironmentServiceResponse.ResponseMessage, actualTestResourceEnvironmentServiceResponse.ResponseMessage);
            Assert.Equal(expectedTestResourceEnvironmentServiceResponse.Success, actualTestResourceEnvironmentServiceResponse.Success);
            Assert.Null(actualTestResourceEnvironmentServiceResponse.ResponseObject);

        }

        [Fact]
        public void ShouldReturnAllDefaultResourceEnvironments()
        {
            // Arrange
            // done in constructor
            var expectedResourceEnvironmentServiceResponse = new ServiceResponse
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

            // Act
            ServiceResponse actualResourceEnvironmentServiceResponse = _resourceEnvironmentService.GetItems().Result;

            // Assert
            Assert.NotNull(actualResourceEnvironmentServiceResponse);
            Assert.Equal(expectedResourceEnvironmentServiceResponse.ResponseMessage, actualResourceEnvironmentServiceResponse.ResponseMessage);
            Assert.Equal(expectedResourceEnvironmentServiceResponse.Success, actualResourceEnvironmentServiceResponse.Success);
            Assert.IsType<List<ResourceEnvironment>>(actualResourceEnvironmentServiceResponse.ResponseObject);
            Assert.Equal(expectedResourceEnvironmentServiceResponse.ResponseObject.Count, actualResourceEnvironmentServiceResponse.ResponseObject.Count);
            for (int i = 0; i < expectedResourceEnvironmentServiceResponse.ResponseObject.Count; i++)
            {
                Assert.Equal(expectedResourceEnvironmentServiceResponse.ResponseObject[i].Id, actualResourceEnvironmentServiceResponse.ResponseObject[i].Id);
                Assert.Equal(expectedResourceEnvironmentServiceResponse.ResponseObject[i].Name, actualResourceEnvironmentServiceResponse.ResponseObject[i].Name);
                Assert.Equal(expectedResourceEnvironmentServiceResponse.ResponseObject[i].ShortName, actualResourceEnvironmentServiceResponse.ResponseObject[i].ShortName);
                Assert.Equal(expectedResourceEnvironmentServiceResponse.ResponseObject[i].SortOrder, actualResourceEnvironmentServiceResponse.ResponseObject[i].SortOrder);
            };

        }

    }
}
