using AzureNaming.Tool.Models;
using AzureNaming.Tool.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace AzureNaming.Tool.Controllers
{
    public class ResourceComponentsControllerTests
    {

        private readonly Mock<IResourceComponentService> _resourceComponentServiceMock;
        private readonly Mock<IAdminLogService> _adminLogServiceMock;
        private readonly Mock<IResourceComponentByIdService> _resourceComponentByIdServiceMock;
        private readonly Mock<IResourceDelimiterService> _resourceDelimiterServiceMock;
        private readonly IResourceComponentService _resourceComponentService;
        private readonly ResourceComponentsController _resourceComponentsController;
        private readonly IResourceTypeService _resourceTypeService;

        public ResourceComponentsControllerTests()
        {
            _resourceComponentServiceMock = new Mock<IResourceComponentService>();
            _adminLogServiceMock = new Mock<IAdminLogService>();
            _resourceComponentByIdServiceMock = new Mock<IResourceComponentByIdService>();
            _resourceDelimiterServiceMock = new Mock<IResourceDelimiterService>();

            //built or composed from mock parameters
            _resourceTypeService = new ResourceTypeService(_adminLogServiceMock.Object, _resourceComponentByIdServiceMock.Object, _resourceDelimiterServiceMock.Object);

            //TODO: compose mock objects
            _resourceComponentService = new ResourceComponentService(_adminLogServiceMock.Object, _resourceTypeService); // _resourceTypeServiceMock.Object);

            //TODO: compose mock objects
            _resourceComponentsController = new ResourceComponentsController(
                _resourceComponentService, // _resourceComponentServiceMock.Object, 
                _adminLogServiceMock.Object,
                _resourceComponentByIdServiceMock.Object);

        }

        [Fact]
        public void ShouldReturnExpectedActionResult()
        {
            // Arrange
            // in constructor

            bool admin = false;
            // Act
            IActionResult actionResult = _resourceComponentsController.Get(admin).Result;

            Type expectedActionResultType = typeof(OkObjectResult);
            // Assert
            Assert.IsType(expectedActionResultType, actionResult);
        }

        [Fact]
        public void ShouldReturnAllDefaultResourceComponents()
        {
            // Arrange
            // in constructor

            var expectedResourceComponentServiceResponse =
                Helpers.GeneralTestHelper.DefaultResourceComponents;

            bool admin = false;
            // Act
            var actionResult = _resourceComponentsController.Get(admin).Result;

            Type expectedActionResultType = typeof(OkObjectResult);
            // Assert
            Assert.IsType(expectedActionResultType, actionResult);

            var objResultValue = ((OkObjectResult)actionResult).Value;
            Assert.NotNull(objResultValue);
            Assert.IsType<List<ResourceComponent>>(objResultValue);

            List<ResourceComponent> actualResourceComponents = objResultValue as List<ResourceComponent>;

            Assert.Equal(expectedResourceComponentServiceResponse.Count, actualResourceComponents.Count);
            for (int i = 0; i < expectedResourceComponentServiceResponse.Count; i++)
            {
                Assert.Equal(expectedResourceComponentServiceResponse[i].Id, actualResourceComponents[i].Id);
                Assert.Equal(expectedResourceComponentServiceResponse[i].Name, actualResourceComponents[i].Name);
                Assert.Equal(expectedResourceComponentServiceResponse[i].DisplayName, actualResourceComponents[i].DisplayName);

                Assert.Equal(expectedResourceComponentServiceResponse[i].Enabled, actualResourceComponents[i].Enabled);
                Assert.Equal(expectedResourceComponentServiceResponse[i].IsCustom, actualResourceComponents[i].IsCustom);
                Assert.Equal(expectedResourceComponentServiceResponse[i].IsFreeText, actualResourceComponents[i].IsFreeText);
                Assert.Equal(expectedResourceComponentServiceResponse[i].MinLength, actualResourceComponents[i].MinLength);
                Assert.Equal(expectedResourceComponentServiceResponse[i].MaxLength, actualResourceComponents[i].MaxLength);
                //Assert.Equal(expectedResourceComponentServiceResponse[i].Id, actualResourceComponents[i].Id);

                Assert.Equal(expectedResourceComponentServiceResponse[i].SortOrder, actualResourceComponents[i].SortOrder);
            };

        }
    }
}
