using AzureNaming.Tool.Models;
using Moq;

namespace AzureNaming.Tool.Services
{
    public class ResourceTypeServiceTests
    {
        private readonly ServiceResponse _expectedResourceTypeServiceResponse;
        private readonly Mock<IAdminLogService> _adminLogServiceMock;
        private readonly Mock<IResourceComponentByIdService> _resourceComponentByIdServiceMock;
        private readonly Mock<IResourceDelimiterService> _resourceDelimiterServiceMock;
        private readonly IResourceTypeService _resourceTypeService;

        public ResourceTypeServiceTests()
        {
            _expectedResourceTypeServiceResponse = new ServiceResponse
            {
                ResponseMessage = "",
                Success = true,
                ResponseObject = Helpers.GeneralTestHelper.DefaultResourceTypes
            };

            _adminLogServiceMock = new Mock<IAdminLogService>();
            _resourceComponentByIdServiceMock = new Mock<IResourceComponentByIdService>();
            _resourceDelimiterServiceMock = new Mock<IResourceDelimiterService>();

            _resourceTypeService = new ResourceTypeService(_adminLogServiceMock.Object, _resourceComponentByIdServiceMock.Object, _resourceDelimiterServiceMock.Object);
        }

        [Fact]
        public void ShouldReturnAllDefaultResourceTypes()
        {
            // Arrange
            // done in constructor

            bool admin = false;
            //
            // Act
            ServiceResponse actualResourceTypeServiceResponse = _resourceTypeService.GetItems(admin).Result;

            // Assert
            Assert.NotNull(actualResourceTypeServiceResponse);
            Assert.Equal(_expectedResourceTypeServiceResponse.ResponseMessage, actualResourceTypeServiceResponse.ResponseMessage);
            Assert.Equal(_expectedResourceTypeServiceResponse.Success, actualResourceTypeServiceResponse.Success);
            Assert.IsType<List<ResourceType>>(actualResourceTypeServiceResponse.ResponseObject);
            Assert.Equal(_expectedResourceTypeServiceResponse.ResponseObject.Count, actualResourceTypeServiceResponse.ResponseObject.Count);
            for (int i = 0; i < _expectedResourceTypeServiceResponse.ResponseObject.Count; i++)
            {
                Assert.Equal(_expectedResourceTypeServiceResponse.ResponseObject[i].Id, actualResourceTypeServiceResponse.ResponseObject[i].Id);
                Assert.Equal(_expectedResourceTypeServiceResponse.ResponseObject[i].Resource, actualResourceTypeServiceResponse.ResponseObject[i].Resource);
                Assert.Equal(_expectedResourceTypeServiceResponse.ResponseObject[i].Optional, actualResourceTypeServiceResponse.ResponseObject[i].Optional);
                Assert.Equal(_expectedResourceTypeServiceResponse.ResponseObject[i].Exclude, actualResourceTypeServiceResponse.ResponseObject[i].Exclude);
                Assert.Equal(_expectedResourceTypeServiceResponse.ResponseObject[i].Property, actualResourceTypeServiceResponse.ResponseObject[i].Property);
                Assert.Equal(_expectedResourceTypeServiceResponse.ResponseObject[i].ShortName, actualResourceTypeServiceResponse.ResponseObject[i].ShortName);
                Assert.Equal(_expectedResourceTypeServiceResponse.ResponseObject[i].Scope, actualResourceTypeServiceResponse.ResponseObject[i].Scope);
                Assert.Equal(_expectedResourceTypeServiceResponse.ResponseObject[i].LengthMin, actualResourceTypeServiceResponse.ResponseObject[i].LengthMin);
                Assert.Equal(_expectedResourceTypeServiceResponse.ResponseObject[i].LengthMax, actualResourceTypeServiceResponse.ResponseObject[i].LengthMax);
                Assert.Equal(_expectedResourceTypeServiceResponse.ResponseObject[i].ValidText, actualResourceTypeServiceResponse.ResponseObject[i].ValidText);
                Assert.Equal(_expectedResourceTypeServiceResponse.ResponseObject[i].InvalidText, actualResourceTypeServiceResponse.ResponseObject[i].InvalidText);
                Assert.Equal(_expectedResourceTypeServiceResponse.ResponseObject[i].InvalidCharacters, actualResourceTypeServiceResponse.ResponseObject[i].InvalidCharacters);
                Assert.Equal(_expectedResourceTypeServiceResponse.ResponseObject[i].InvalidCharactersStart, actualResourceTypeServiceResponse.ResponseObject[i].InvalidCharactersStart);
                Assert.Equal(_expectedResourceTypeServiceResponse.ResponseObject[i].InvalidCharactersEnd, actualResourceTypeServiceResponse.ResponseObject[i].InvalidCharactersEnd);
                Assert.Equal(_expectedResourceTypeServiceResponse.ResponseObject[i].InvalidCharactersConsecutive, actualResourceTypeServiceResponse.ResponseObject[i].InvalidCharactersConsecutive);
                Assert.Equal(_expectedResourceTypeServiceResponse.ResponseObject[i].Regx, actualResourceTypeServiceResponse.ResponseObject[i].Regx);
                Assert.Equal(_expectedResourceTypeServiceResponse.ResponseObject[i].StaticValues, actualResourceTypeServiceResponse.ResponseObject[i].StaticValues);
                Assert.Equal(_expectedResourceTypeServiceResponse.ResponseObject[i].Enabled, actualResourceTypeServiceResponse.ResponseObject[i].Enabled);
            };

        }
    }
}
