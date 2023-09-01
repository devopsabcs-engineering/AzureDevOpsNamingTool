using AzureNaming.Tool.Models;
using Moq;
using System.Text.Json;

namespace AzureNaming.Tool.Services
{
    public class ResourceComponentServiceTests
    {
        private readonly ServiceResponse _expectedResourceComponentServiceResponse;
        //private readonly Task<ServiceResponse> _resourceTypeServiceResponse;
        private readonly Mock<IAdminLogService> _adminLogServiceMock;
        private readonly ResourceComponentService _resourceComponentService;
        private ServiceResponse _expectedTestResourceComponentServiceResponse;
        private readonly Mock<IResourceTypeService> _resourceTypeServiceMock;

        private readonly Mock<IResourceComponentByIdService> _resourceComponentByIdServiceMock;
        private readonly Mock<IResourceDelimiterService> _resourceDelimiterServiceMock;
        private readonly ResourceTypeService _resourceTypeService; //really from Mock

        public ResourceComponentServiceTests()
        {
            _expectedResourceComponentServiceResponse = new ServiceResponse
            {
                ResponseMessage = "",
                Success = true,
                ResponseObject = new List<ResourceComponent>
                {
                    new ResourceComponent
                    {
                        Id = 1,
                        Name = "ResourceType",
                        DisplayName = "Resource Types",
                        Enabled = true,
                        SortOrder = 1,
                        IsCustom = false,
                        IsFreeText = false,
                        MinLength = "1",
                        MaxLength = "10"
                    },
                    new ResourceComponent
                    {
                        Id = 2,
                        Name = "Prefix",
                        DisplayName = "Prefix",
                        Enabled = true,
                        SortOrder = 2,
                        IsCustom = true,
                        IsFreeText = true,
                        MinLength = "2",
                        MaxLength = "5"
                    },
                    new ResourceComponent
                    {
                        Id = 3,
                        Name = "PrefixPreDefined",
                        DisplayName = "PrefixPreDefined",
                        Enabled = true,
                        SortOrder = 3,
                        IsCustom = true,
                        IsFreeText = false,
                        MinLength = "2",
                        MaxLength = "5"
                    },
                    new ResourceComponent
                    {
                        Id = 4,
                        Name = "ResourceOrg",
                        DisplayName = "Orgs",
                        Enabled = true,
                        SortOrder = 4,
                        IsCustom = false,
                        IsFreeText = false,
                        MinLength = "1",
                        MaxLength = "5"
                    },
                    new ResourceComponent
                    {
                        Id = 5,
                        Name = "ResourceUnitDept",
                        DisplayName = "Units/Depts",
                        Enabled = true,
                        SortOrder = 5,
                        IsCustom = false,
                        IsFreeText = false,
                        MinLength = "1",
                        MaxLength = "5"
                    },
                    new ResourceComponent
                    {
                        Id = 6,
                        Name = "ResourceProjAppSvc",
                        DisplayName = "Projects/Apps/Services",
                        Enabled = true,
                        SortOrder = 6,
                        IsCustom = false,
                        IsFreeText = false,
                        MinLength = "1",
                        MaxLength = "3"
                    },
                    new ResourceComponent
                    {
                        Id = 7,
                        Name = "ResourceFunction",
                        DisplayName = "Functions",
                        Enabled = true,
                        SortOrder = 7,
                        IsCustom = false,
                        IsFreeText = false,
                        MinLength = "1",
                        MaxLength = "10"
                    },
                    new ResourceComponent
                    {
                        Id = 8,
                        Name = "ResourceEnvironment",
                        DisplayName = "Environments",
                        Enabled = true,
                        SortOrder = 8,
                        IsCustom = false,
                        IsFreeText = false,
                        MinLength = "1",
                        MaxLength = "5"
                    },
                    new ResourceComponent
                    {
                        Id = 9,
                        Name = "ResourceLocation",
                        DisplayName = "Locations",
                        Enabled = true,
                        SortOrder = 9,
                        IsCustom = false,
                        IsFreeText = false,
                        MinLength = "1",
                        MaxLength = "10"
                    },
                    new ResourceComponent
                    {
                        Id = 10,
                        Name = "UniqueString",
                        DisplayName = "UniqueString",
                        Enabled = true,
                        SortOrder = 10,
                        IsCustom = true,
                        IsFreeText = true,
                        MinLength = "13",
                        MaxLength = "13"
                    },
                    new ResourceComponent
                    {
                        Id = 11,
                        Name = "ResourceInstance",
                        DisplayName = "Instance",
                        Enabled = true,
                        SortOrder = 11,
                        IsCustom = false,
                        IsFreeText = false,
                        MinLength = "1",
                        MaxLength = "5"
                    }
                }
            };

            _expectedTestResourceComponentServiceResponse = new ServiceResponse()
            {
                Success = true,
                ResponseMessage = "",
                ResponseObject = new ResourceEnvironment() { Id = 6, Name = "Test", ShortName = "tst", SortOrder = 6 }
            };

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

            _resourceComponentService = new ResourceComponentService(_adminLogServiceMock.Object, _resourceTypeService); // _resourceTypeServiceMock.Object);
        }

        [Fact]
        public void ShouldReturnAllDefaultResourceComponents()
        {
            // Arrange
            // done in constructor

            bool admin = false;
            //
            // Act
            ServiceResponse actualResourceComponentServiceResponse = _resourceComponentService.GetItems(admin).Result;

            // Assert
            Assert.NotNull(actualResourceComponentServiceResponse);
            Assert.Equal(_expectedResourceComponentServiceResponse.ResponseMessage, actualResourceComponentServiceResponse.ResponseMessage);
            Assert.Equal(_expectedResourceComponentServiceResponse.Success, actualResourceComponentServiceResponse.Success);
            Assert.IsType<List<ResourceComponent>>(actualResourceComponentServiceResponse.ResponseObject);
            Assert.Equal(_expectedResourceComponentServiceResponse.ResponseObject.Count, actualResourceComponentServiceResponse.ResponseObject.Count);
            for (int i = 0; i < _expectedResourceComponentServiceResponse.ResponseObject.Count; i++)
            {
                Assert.Equal(_expectedResourceComponentServiceResponse.ResponseObject[i].Id, actualResourceComponentServiceResponse.ResponseObject[i].Id);
                Assert.Equal(_expectedResourceComponentServiceResponse.ResponseObject[i].Name, actualResourceComponentServiceResponse.ResponseObject[i].Name);
                Assert.Equal(_expectedResourceComponentServiceResponse.ResponseObject[i].DisplayName, actualResourceComponentServiceResponse.ResponseObject[i].DisplayName);

                Assert.Equal(_expectedResourceComponentServiceResponse.ResponseObject[i].Enabled, actualResourceComponentServiceResponse.ResponseObject[i].Enabled);
                Assert.Equal(_expectedResourceComponentServiceResponse.ResponseObject[i].IsCustom, actualResourceComponentServiceResponse.ResponseObject[i].IsCustom);
                Assert.Equal(_expectedResourceComponentServiceResponse.ResponseObject[i].IsFreeText, actualResourceComponentServiceResponse.ResponseObject[i].IsFreeText);
                Assert.Equal(_expectedResourceComponentServiceResponse.ResponseObject[i].MinLength, actualResourceComponentServiceResponse.ResponseObject[i].MinLength);
                Assert.Equal(_expectedResourceComponentServiceResponse.ResponseObject[i].MaxLength, actualResourceComponentServiceResponse.ResponseObject[i].MaxLength);
                Assert.Equal(_expectedResourceComponentServiceResponse.ResponseObject[i].Id, actualResourceComponentServiceResponse.ResponseObject[i].Id);

                Assert.Equal(_expectedResourceComponentServiceResponse.ResponseObject[i].SortOrder, actualResourceComponentServiceResponse.ResponseObject[i].SortOrder);
            };

        }

        [Fact]
        public void ShouldDeleteACustomResourceComponent()
        {
            // Arrange
            // done in constructor

            string fileName = "settings/resourcetypes.json";
            string jsonString = File.ReadAllText(fileName);
            List<ResourceType> allResourceTypes = JsonSerializer.Deserialize<List<ResourceType>>(jsonString)!;

            _expectedTestResourceComponentServiceResponse = new ServiceResponse()
            {
                Success = true,
                ResponseMessage = "",
                ResponseObject = allResourceTypes
            };

            // Act
            ServiceResponse actualTestResourceComponentServiceResponse = _resourceComponentService.DeleteItem(10).Result; //must be custom component e.g. UniqueString

            // Assert
            Assert.NotNull(actualTestResourceComponentServiceResponse);
            Assert.Equal(this._expectedTestResourceComponentServiceResponse.ResponseMessage, actualTestResourceComponentServiceResponse.ResponseMessage);
            Assert.Equal(this._expectedTestResourceComponentServiceResponse.Success, actualTestResourceComponentServiceResponse.Success);
            Assert.IsType<List<ResourceType>>(actualTestResourceComponentServiceResponse.ResponseObject);
            Assert.Equal(this._expectedTestResourceComponentServiceResponse.ResponseObject.Count, actualTestResourceComponentServiceResponse.ResponseObject.Count);
            for (int i = 0; i < _expectedTestResourceComponentServiceResponse.ResponseObject.Count; i++)
            {
                Assert.Equal(_expectedTestResourceComponentServiceResponse.ResponseObject[i].Id, actualTestResourceComponentServiceResponse.ResponseObject[i].Id);
                Assert.Equal(_expectedTestResourceComponentServiceResponse.ResponseObject[i].Resource, actualTestResourceComponentServiceResponse.ResponseObject[i].Resource);

                //this is what will change
                Assert.Equal(RemoveComponentFromCsv(_expectedTestResourceComponentServiceResponse.ResponseObject[i].Optional, "UniqueString"),
                    actualTestResourceComponentServiceResponse.ResponseObject[i].Optional);

                Assert.Equal(_expectedTestResourceComponentServiceResponse.ResponseObject[i].Exclude, actualTestResourceComponentServiceResponse.ResponseObject[i].Exclude);
                Assert.Equal(_expectedTestResourceComponentServiceResponse.ResponseObject[i].Property, actualTestResourceComponentServiceResponse.ResponseObject[i].Property);
                Assert.Equal(_expectedTestResourceComponentServiceResponse.ResponseObject[i].ShortName, actualTestResourceComponentServiceResponse.ResponseObject[i].ShortName);
                Assert.Equal(_expectedTestResourceComponentServiceResponse.ResponseObject[i].Scope, actualTestResourceComponentServiceResponse.ResponseObject[i].Scope);

                Assert.Equal(_expectedTestResourceComponentServiceResponse.ResponseObject[i].LengthMin, actualTestResourceComponentServiceResponse.ResponseObject[i].LengthMin);
                Assert.Equal(_expectedTestResourceComponentServiceResponse.ResponseObject[i].LengthMax, actualTestResourceComponentServiceResponse.ResponseObject[i].LengthMax);

                Assert.Equal(_expectedTestResourceComponentServiceResponse.ResponseObject[i].ValidText, actualTestResourceComponentServiceResponse.ResponseObject[i].ValidText);
                Assert.Equal(_expectedTestResourceComponentServiceResponse.ResponseObject[i].InvalidText, actualTestResourceComponentServiceResponse.ResponseObject[i].InvalidText);

                Assert.Equal(_expectedTestResourceComponentServiceResponse.ResponseObject[i].InvalidCharacters, actualTestResourceComponentServiceResponse.ResponseObject[i].InvalidCharacters);
                Assert.Equal(_expectedTestResourceComponentServiceResponse.ResponseObject[i].InvalidCharactersStart, actualTestResourceComponentServiceResponse.ResponseObject[i].InvalidCharactersStart);
                Assert.Equal(_expectedTestResourceComponentServiceResponse.ResponseObject[i].InvalidCharactersEnd, actualTestResourceComponentServiceResponse.ResponseObject[i].InvalidCharactersEnd);
                Assert.Equal(_expectedTestResourceComponentServiceResponse.ResponseObject[i].InvalidCharactersConsecutive, actualTestResourceComponentServiceResponse.ResponseObject[i].InvalidCharactersConsecutive);

                Assert.Equal(_expectedTestResourceComponentServiceResponse.ResponseObject[i].Regx, actualTestResourceComponentServiceResponse.ResponseObject[i].Regx);
                Assert.Equal(_expectedTestResourceComponentServiceResponse.ResponseObject[i].StaticValues, actualTestResourceComponentServiceResponse.ResponseObject[i].StaticValues);

                Assert.Equal(_expectedTestResourceComponentServiceResponse.ResponseObject[i].Enabled, actualTestResourceComponentServiceResponse.ResponseObject[i].Enabled);
            };

        }

        #region Helpers

        private string RemoveComponentFromCsv(string optionalCsv, string componentToRemove)
        {
            // essentially the same implementation in actual code
            string newOptionalCsv = optionalCsv;
            var currentvalues = new List<string>(optionalCsv.Split(','));
            if (currentvalues.Contains(componentToRemove))
            {
                currentvalues.Remove(componentToRemove);
                newOptionalCsv = String.Join(",", currentvalues.ToArray());
            }
            return newOptionalCsv;
        }

        private Task<ServiceResponse> GetSampleResourceTypeServiceResponseAsTask()
        {
            var serviceResponse = new ServiceResponse()
            {
                ResponseMessage = "",
                Success = true,
                ResponseObject = new List<ResourceType>()
                {
                    new ResourceType(){
                        Id = 1,
                        Resource = "AnalysisServices/servers",
                        Optional = "UnitDept,UniqueString",
                        Exclude = "Org,Function",
                        Property = "",
                        ShortName = "as",
                        Scope = "resource group",
                        LengthMin = "3",
                        LengthMax = "63",
                        ValidText = "Lowercase letters and numbers. Start with lowercase letter.",
                        InvalidText = "",
                        InvalidCharacters = "",
                        InvalidCharactersStart = "",
                        InvalidCharactersEnd = "",
                        InvalidCharactersConsecutive = "",
                        Regx = "^[a-z][a-z0-9]{2,62}$",
                        StaticValues = "",
                        Enabled = true
                    }
                }
            };

            // manually wrap the serviceResponse in a new Task
            return new Task<ServiceResponse>(() => { return serviceResponse; });
        }

        #endregion Helpers
    }
}
