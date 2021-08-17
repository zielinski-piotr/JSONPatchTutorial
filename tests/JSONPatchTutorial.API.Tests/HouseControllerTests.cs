using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using JSONPatchTutorial.API.Tests.Factories;
using JSONPatchTutorial.Contract.Responses;
using JSONPatchTutorial.Seeding;
using JSONPatchTutorial.Serialization.Helpers;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace JSONPatchTutorial.API.Tests
{
    public sealed class HouseControllerTests : IDisposable
    {
        private readonly HttpClient _client;

        public HouseControllerTests()
        {
            var factory = new JsonPatchTutorialApiFactory();

            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        [Fact]
        public async Task GetById_Should_Return_Valid_Entity()
        {
            //Arrange
            JsonSerializerOptions options = new(JsonSerializerDefaults.Web);

            //Act
            var httpResponse = await _client.GetAsync($"House/{Houses.House1.Id}");
            await using var stream = await httpResponse.Content.ReadAsStreamAsync();

            //Assert
            Assert.Equal(HttpStatusCode.OK, httpResponse.StatusCode);

            var house = await JsonSerializationHelper.DeserializeJsonFromStream<House.Response>(stream, options);

            Assert.Equal(Houses.House1.Id, house.Id);
            Assert.Equal(Houses.House1.Address.Street, house.Address.Street);
        }

        [Fact]
        public async Task Patch_Should_Update_House_Successfully()
        {
            //Arrange
            const string newStreetName = "New street name";
            const string patch = "[{\"op\": \"replace\", \"path\": \"/address/street\", \"value\" : \"" +
                                 newStreetName + "\"}]";
            var houseId = Houses.House1.Id;

            var requestContent = new ByteArrayContent(Encoding.UTF8.GetBytes(patch));
            requestContent.Headers.ContentType = new MediaTypeHeaderValue("application/json-patch+json");
            requestContent.Headers.ContentEncoding.Add("utf8");

            //Act
            var httpResponse = await _client.PatchAsync($"House/{houseId}", requestContent);

            //Assert response
            Assert.Equal(HttpStatusCode.Accepted, httpResponse.StatusCode);

            //Assert persisted entity after Patch 
            var houseAfterPatch = await GetHouseAfterPatch(houseId);

            Assert.Equal(newStreetName, houseAfterPatch.Address.Street);
        }

        [Theory]
        [InlineData("add")]
        [InlineData("remove")]
        [InlineData("replace")]
        public async Task Patch_Should_Return_UnprocessableEntity_When_Trying_To_Patch_Not_Existing_Property(
            string operation)
        {
            //Arrange
            const string newStreetName = "New street name";
            var patch = "[{\"op\": \"" + operation + "\", \"path\": \"/address/length\", \"value\" : \"" +
                        newStreetName + "\"}]";
            var houseId = Houses.House1.Id;

            var requestContent = new ByteArrayContent(Encoding.UTF8.GetBytes(patch));
            requestContent.Headers.ContentType = new MediaTypeHeaderValue("application/json-patch+json");
            requestContent.Headers.ContentEncoding.Add("utf8");

            //Act
            var httpResponse = await _client.PatchAsync($"House/{houseId}", requestContent);

            //Assert
            Assert.Equal(HttpStatusCode.UnprocessableEntity, httpResponse.StatusCode);
        }

        [Fact]
        public async Task Patch_Should_Add_Room_To_House_Successfully()
        {
            //Arrange
            const string patch =
                "[{\"op\": \"add\", \"path\": \"/rooms/-\", \"value\" : {\"name\" : \"Room1\", \"color\" : \"Red\", \"area\" : \"12.2\" }}]";
            var houseId = Houses.House1.Id;

            var requestContent = new ByteArrayContent(Encoding.UTF8.GetBytes(patch));
            requestContent.Headers.ContentType = new MediaTypeHeaderValue("application/json-patch+json");
            requestContent.Headers.ContentEncoding.Add("utf8");

            //Act
            var httpResponse = await _client.PatchAsync($"House/{houseId}", requestContent);

            //Assert
            Assert.Equal(HttpStatusCode.Accepted, httpResponse.StatusCode);

            var afterPatch = await GetHouseAfterPatch(houseId);
            Assert.Single(afterPatch.Rooms);
        }

        [Fact]
        public async Task Patch_Should_Replace_Rooms_In_House_Successfully()
        {
            //Arrange
            const string patch =
                "[{\"op\": \"replace\", \"path\": \"/rooms\", \"value\" :" +
                " [{\"name\" : \"Room1\", \"color\" : \"Red\", \"area\" : \"12.2\" }, {\"name\" : \"Room2\", \"color\" : \"Yellow\", \"area\" : \"24.2\" }]}]";
            var houseId = Houses.House3.Id;

            var requestContent = new ByteArrayContent(Encoding.UTF8.GetBytes(patch));
            requestContent.Headers.ContentType = new MediaTypeHeaderValue("application/json-patch+json");
            requestContent.Headers.ContentEncoding.Add("utf8");

            //Act
            var httpResponse = await _client.PatchAsync($"House/{houseId}", requestContent);

            //Assert
            Assert.Equal(HttpStatusCode.Accepted, httpResponse.StatusCode);

            var afterPatch = await GetHouseAfterPatch(houseId);
            Assert.Equal(2, afterPatch.Rooms.Count());
        }
        
        [Fact]
        public async Task Patch_Should_Return_UnprocessableEntity_When_Trying_To_Remove_Room_By_Id_Not_Index_In_Array()
        {
            //Arrange
            var houseId = Houses.House2.Id;
            var patch = "[{\"op\": \"remove\", \"path\": \"/rooms/" + Houses.House2.Rooms.Last().Id +"\", }]";

            var requestContent = new ByteArrayContent(Encoding.UTF8.GetBytes(patch));
            requestContent.Headers.ContentType = new MediaTypeHeaderValue("application/json-patch+json");
            requestContent.Headers.ContentEncoding.Add("utf8");

            //Act
            var httpResponse = await _client.PatchAsync($"House/{houseId}", requestContent);

            //Assert
            Assert.Equal(HttpStatusCode.UnprocessableEntity, httpResponse.StatusCode);
        }

        private async Task<House.Response> GetHouseAfterPatch(Guid id)
        {
            JsonSerializerOptions options = new(JsonSerializerDefaults.Web);

            var httpResponse = await _client.GetAsync($"House/{id}");
            await using var stream = await httpResponse.Content.ReadAsStreamAsync();

            return await JsonSerializationHelper.DeserializeJsonFromStream<House.Response>(stream, options);
        }

        public void Dispose() => _client?.Dispose();
    }
}