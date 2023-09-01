using AzureNaming.Tool.Models;
using Moq;

namespace AzureNaming.Tool.Services
{
    public class ResourceComponentServiceTests
    {
        //private readonly ServiceResponse _expectedResourceComponentServiceResponse;
        //private readonly Task<ServiceResponse> _resourceTypeServiceResponse;
        private readonly Mock<IAdminLogService> _adminLogServiceMock;
        private readonly IResourceComponentService _resourceComponentService;
        //private ServiceResponse _expectedTestResourceComponentServiceResponse;
        private readonly Mock<IResourceTypeService> _resourceTypeServiceMock;

        private readonly Mock<IResourceComponentByIdService> _resourceComponentByIdServiceMock;
        private readonly Mock<IResourceDelimiterService> _resourceDelimiterServiceMock;
        private readonly IResourceTypeService _resourceTypeService; //really from Mock

        public ResourceComponentServiceTests()
        {

            _adminLogServiceMock = new Mock<IAdminLogService>();
            _resourceTypeServiceMock = new Mock<IResourceTypeService>();


            _resourceComponentByIdServiceMock = new Mock<IResourceComponentByIdService>();
            _resourceDelimiterServiceMock = new Mock<IResourceDelimiterService>();

            //built or composed from mock parameters
            _resourceTypeService = new ResourceTypeService(_adminLogServiceMock.Object, _resourceComponentByIdServiceMock.Object, _resourceDelimiterServiceMock.Object);

            //bool admin = true;

            //_resourceTypeServiceResponse = GetSampleResourceTypeServiceResponseAsTask();
            //_resourceTypeServiceMock.Setup(x => x.GetItems(It.IsAny<bool>()))
            //    .Returns(_resourceTypeServiceResponse);

            //TODO: compose mock objects
            _resourceComponentService = new ResourceComponentService(_adminLogServiceMock.Object, _resourceTypeService); // _resourceTypeServiceMock.Object);
        }

        [Fact]
        public void ShouldReturnAllDefaultResourceComponents()
        {
            // Arrange
            // done in constructor
            var expectedResourceComponentServiceResponse = new ServiceResponse
            {
                ResponseMessage = "",
                Success = true,
                ResponseObject = Helpers.GeneralTestHelper.DefaultResourceComponents
            };

            bool admin = false;
            //
            // Act
            ServiceResponse actualResourceComponentServiceResponse = _resourceComponentService.GetItems(admin).Result;

            // Assert
            Assert.NotNull(actualResourceComponentServiceResponse);
            Assert.Equal(expectedResourceComponentServiceResponse.ResponseMessage, actualResourceComponentServiceResponse.ResponseMessage);
            Assert.Equal(expectedResourceComponentServiceResponse.Success, actualResourceComponentServiceResponse.Success);
            Assert.IsType<List<ResourceComponent>>(actualResourceComponentServiceResponse.ResponseObject);
            Assert.Equal(expectedResourceComponentServiceResponse.ResponseObject.Count, actualResourceComponentServiceResponse.ResponseObject.Count);
            for (int i = 0; i < expectedResourceComponentServiceResponse.ResponseObject.Count; i++)
            {
                Assert.Equal(expectedResourceComponentServiceResponse.ResponseObject[i].Id, actualResourceComponentServiceResponse.ResponseObject[i].Id);
                Assert.Equal(expectedResourceComponentServiceResponse.ResponseObject[i].Name, actualResourceComponentServiceResponse.ResponseObject[i].Name);
                Assert.Equal(expectedResourceComponentServiceResponse.ResponseObject[i].DisplayName, actualResourceComponentServiceResponse.ResponseObject[i].DisplayName);
                Assert.Equal(expectedResourceComponentServiceResponse.ResponseObject[i].Enabled, actualResourceComponentServiceResponse.ResponseObject[i].Enabled);
                Assert.Equal(expectedResourceComponentServiceResponse.ResponseObject[i].IsCustom, actualResourceComponentServiceResponse.ResponseObject[i].IsCustom);
                Assert.Equal(expectedResourceComponentServiceResponse.ResponseObject[i].IsFreeText, actualResourceComponentServiceResponse.ResponseObject[i].IsFreeText);
                Assert.Equal(expectedResourceComponentServiceResponse.ResponseObject[i].MinLength, actualResourceComponentServiceResponse.ResponseObject[i].MinLength);
                Assert.Equal(expectedResourceComponentServiceResponse.ResponseObject[i].MaxLength, actualResourceComponentServiceResponse.ResponseObject[i].MaxLength);
                Assert.Equal(expectedResourceComponentServiceResponse.ResponseObject[i].SortOrder, actualResourceComponentServiceResponse.ResponseObject[i].SortOrder);
            };

        }

        [Fact]
        public void ShouldDeleteACustomResourceComponent()
        {
            // Arrange
            // done in constructor            

            var expectedTestResourceComponentServiceResponse = new ServiceResponse()
            {
                Success = true,
                ResponseMessage = "",
                ResponseObject = Helpers.GeneralTestHelper.DefaultResourceTypes
            };

            // Act
            ServiceResponse actualTestResourceComponentServiceResponse = _resourceComponentService.DeleteItem(10).Result; //must be custom component e.g. UniqueString

            // Assert
            Assert.NotNull(actualTestResourceComponentServiceResponse);
            Assert.Equal(expectedTestResourceComponentServiceResponse.ResponseMessage, actualTestResourceComponentServiceResponse.ResponseMessage);
            Assert.Equal(expectedTestResourceComponentServiceResponse.Success, actualTestResourceComponentServiceResponse.Success);
            Assert.IsType<List<ResourceType>>(actualTestResourceComponentServiceResponse.ResponseObject);
            Assert.Equal(expectedTestResourceComponentServiceResponse.ResponseObject.Count, actualTestResourceComponentServiceResponse.ResponseObject.Count);
            for (int i = 0; i < expectedTestResourceComponentServiceResponse.ResponseObject.Count; i++)
            {
                Assert.Equal(expectedTestResourceComponentServiceResponse.ResponseObject[i].Id, actualTestResourceComponentServiceResponse.ResponseObject[i].Id);
                Assert.Equal(expectedTestResourceComponentServiceResponse.ResponseObject[i].Resource, actualTestResourceComponentServiceResponse.ResponseObject[i].Resource);

                //this is what will change
                Assert.Equal(Helpers.GeneralTestHelper.RemoveComponentFromCsv(expectedTestResourceComponentServiceResponse.ResponseObject[i].Optional, "UniqueString"),
                    actualTestResourceComponentServiceResponse.ResponseObject[i].Optional);

                Assert.Equal(expectedTestResourceComponentServiceResponse.ResponseObject[i].Exclude, actualTestResourceComponentServiceResponse.ResponseObject[i].Exclude);
                Assert.Equal(expectedTestResourceComponentServiceResponse.ResponseObject[i].Property, actualTestResourceComponentServiceResponse.ResponseObject[i].Property);
                Assert.Equal(expectedTestResourceComponentServiceResponse.ResponseObject[i].ShortName, actualTestResourceComponentServiceResponse.ResponseObject[i].ShortName);
                Assert.Equal(expectedTestResourceComponentServiceResponse.ResponseObject[i].Scope, actualTestResourceComponentServiceResponse.ResponseObject[i].Scope);

                Assert.Equal(expectedTestResourceComponentServiceResponse.ResponseObject[i].LengthMin, actualTestResourceComponentServiceResponse.ResponseObject[i].LengthMin);
                Assert.Equal(expectedTestResourceComponentServiceResponse.ResponseObject[i].LengthMax, actualTestResourceComponentServiceResponse.ResponseObject[i].LengthMax);

                Assert.Equal(expectedTestResourceComponentServiceResponse.ResponseObject[i].ValidText, actualTestResourceComponentServiceResponse.ResponseObject[i].ValidText);
                Assert.Equal(expectedTestResourceComponentServiceResponse.ResponseObject[i].InvalidText, actualTestResourceComponentServiceResponse.ResponseObject[i].InvalidText);

                Assert.Equal(expectedTestResourceComponentServiceResponse.ResponseObject[i].InvalidCharacters, actualTestResourceComponentServiceResponse.ResponseObject[i].InvalidCharacters);
                Assert.Equal(expectedTestResourceComponentServiceResponse.ResponseObject[i].InvalidCharactersStart, actualTestResourceComponentServiceResponse.ResponseObject[i].InvalidCharactersStart);
                Assert.Equal(expectedTestResourceComponentServiceResponse.ResponseObject[i].InvalidCharactersEnd, actualTestResourceComponentServiceResponse.ResponseObject[i].InvalidCharactersEnd);
                Assert.Equal(expectedTestResourceComponentServiceResponse.ResponseObject[i].InvalidCharactersConsecutive, actualTestResourceComponentServiceResponse.ResponseObject[i].InvalidCharactersConsecutive);

                Assert.Equal(expectedTestResourceComponentServiceResponse.ResponseObject[i].Regx, actualTestResourceComponentServiceResponse.ResponseObject[i].Regx);
                Assert.Equal(expectedTestResourceComponentServiceResponse.ResponseObject[i].StaticValues, actualTestResourceComponentServiceResponse.ResponseObject[i].StaticValues);

                Assert.Equal(expectedTestResourceComponentServiceResponse.ResponseObject[i].Enabled, actualTestResourceComponentServiceResponse.ResponseObject[i].Enabled);
            };

        }


    }
}
