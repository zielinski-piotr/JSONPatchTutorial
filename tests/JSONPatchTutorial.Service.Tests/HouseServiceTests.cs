using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using JSONPatchTutorial.Contract.Requests;
using JSONPatchTutorial.Data;
using JSONPatchTutorial.Seeding;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Exceptions;
using Microsoft.AspNetCore.JsonPatch.Operations;
using MockQueryable.Moq;
using Moq;
using Xunit;
using DataModelHouse = JSONPatchTutorial.Domain.House;

namespace JSONPatchTutorial.Service.Tests
{
    public class HouseServiceTests
    {
        private readonly Mock<IRepository> _repository;
        private readonly IHouseService _houseService;

        private const string NewStreetName = "New Street";

        public HouseServiceTests()
        {
            _repository = new Mock<IRepository>();
            var mapper = new MapperConfiguration(mc => { mc.AddProfile(new MapperProfile()); }).CreateMapper();

            _houseService = new HouseService(_repository.Object, mapper);

            _repository.Setup(x => x.SaveChangesAsync());
        }

        [Fact]
        public async Task UpdateHouse_Should_UpdateEntity_Successfully()
        {
            //Arrange
            _repository.Setup(x => x.Get<DataModelHouse>())
                .Returns(Houses.GetSeededHouses().AsQueryable().BuildMock());

            _repository.Setup(x => x.Update(It.IsAny<DataModelHouse>()));

            var patch = new JsonPatchDocument<House.Patch>();
            patch.Operations.Add(new Operation<House.Patch>("replace", "/address/street", string.Empty, NewStreetName));

            //Act
            await _houseService.UpdateHouse(patch, Houses.House1.Id);

            //Assert
            _repository.Verify(x => x.Get<DataModelHouse>());
            _repository.Verify(x =>
                x.Update(It.Is<DataModelHouse>(house => house.Address.Street.Equals(NewStreetName))));
            _repository.Verify(x => x.SaveChangesAsync());
        }

        [Fact]
        public async Task UpdateHouse_Should_Throw_JsonPatchException_When_Patching_Not_Existing_Property()
        {
            //Arrange
            _repository.Setup(x => x.Get<DataModelHouse>())
                .Returns(Houses.GetSeededHouses().AsQueryable().BuildMock());

            _repository.Setup(x => x.Update(It.IsAny<DataModelHouse>()));

            var patch = new JsonPatchDocument<House.Patch>();
            patch.Operations.Add(new Operation<House.Patch>("replace", "/notExistingProperty", string.Empty,
                NewStreetName));

            //Act + Assert
            await Assert.ThrowsAsync<JsonPatchException>(async () =>
                await _houseService.UpdateHouse(patch, Houses.House1.Id));

            _repository.Verify(x => x.Get<DataModelHouse>());
            _repository.Verify(x =>
                x.Update(It.IsAny<DataModelHouse>()), Times.Never);
            _repository.Verify(x => x.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task UpdateHouse_Should_Throw_KeyNotFoundException_When_Patching_Not_Existing_Entity()
        {
            //Arrange
            _repository.Setup(x => x.Get<DataModelHouse>())
                .Returns(Houses.GetSeededHouses().AsQueryable().BuildMock());

            _repository.Setup(x => x.Update(It.IsAny<DataModelHouse>()));

            var patch = new JsonPatchDocument<House.Patch>();
            patch.Operations.Add(new Operation<House.Patch>("replace", "/address/street", string.Empty, NewStreetName));

            //Act + Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(async () =>
                await _houseService.UpdateHouse(patch, Guid.NewGuid()));

            _repository.Verify(x => x.Get<DataModelHouse>());
            _repository.Verify(x =>
                x.Update(It.IsAny<DataModelHouse>()), Times.Never);
            _repository.Verify(x => x.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task UpdateHouse_Should_Set_Property_Value_To_Null_When_Remove_Operation_Used_On_Reference_Type()
        {
            //Arrange
            _repository.Setup(x => x.Get<DataModelHouse>())
                .Returns(Houses.GetSeededHouses().AsQueryable().BuildMock());

            _repository.Setup(x => x.Update(It.IsAny<DataModelHouse>()));

            var patch = new JsonPatchDocument<House.Patch>();
            patch.Operations.Add(new Operation<House.Patch>("remove", "/address/street", string.Empty, string.Empty));

            //Act
            await _houseService.UpdateHouse(patch, Houses.House1.Id);

            //Assert
            _repository.Verify(x => x.Get<DataModelHouse>());
            _repository.Verify(x =>
                x.Update(It.Is<DataModelHouse>(house => house.Address.Street == null)));
            _repository.Verify(x => x.SaveChangesAsync());
        }

        [Fact]
        public async Task UpdateHouse_Should_Set_Property_Value_To_Default_When_Remove_Operation_Used_On_Value_Type()
        {
            //Arrange
            _repository.Setup(x => x.Get<DataModelHouse>())
                .Returns(Houses.GetSeededHouses().AsQueryable().BuildMock());

            _repository.Setup(x => x.Update(It.IsAny<DataModelHouse>()));

            var patch = new JsonPatchDocument<House.Patch>();
            patch.Operations.Add(new Operation<House.Patch>("remove", "/area", string.Empty, string.Empty));

            //Act
            await _houseService.UpdateHouse(patch, Houses.House1.Id);

            //Assert
            _repository.Verify(x => x.Get<DataModelHouse>());
            _repository.Verify(x =>
                x.Update(It.Is<DataModelHouse>(house => house.Area == default)));
            _repository.Verify(x => x.SaveChangesAsync());
        }

        [Fact]
        public async Task
            UpdateHouse_Should_Throw_JsonPatchException_When_Replacing_Value_On_Property_Of_Composed_Object_Which_Is_Null()
        {
            //Arrange
            _repository.Setup(x => x.Get<DataModelHouse>())
                .Returns(Houses.GetSeededHouses().AsQueryable().BuildMock());

            _repository.Setup(x => x.Update(It.IsAny<DataModelHouse>()));

            var patch = new JsonPatchDocument<House.Patch>();
            patch.Operations.Add(new Operation<House.Patch>("replace", "/address/street", string.Empty, NewStreetName));

            //Act
            await Assert.ThrowsAsync<JsonPatchException>(async () =>
                await _houseService.UpdateHouse(patch, Houses.House11.Id));

            //Assert
            _repository.Verify(x => x.Get<DataModelHouse>());
            _repository.Verify(x =>
                x.Update(It.IsAny<DataModelHouse>()), Times.Never);
            _repository.Verify(x => x.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task
            UpdateHouse_Should_Throw_JsonPatchException_When_Adding_Value_On_Property_Of_Composed_Object_Which_Is_Null()
        {
            //Arrange
            _repository.Setup(x => x.Get<DataModelHouse>())
                .Returns(Houses.GetSeededHouses().AsQueryable().BuildMock());

            _repository.Setup(x => x.Update(It.IsAny<DataModelHouse>()));

            var patch = new JsonPatchDocument<House.Patch>();
            patch.Operations.Add(new Operation<House.Patch>("add", "/address/street", string.Empty, NewStreetName));

            //Act
            await Assert.ThrowsAsync<JsonPatchException>(async () =>
                await _houseService.UpdateHouse(patch, Houses.House11.Id));

            //Assert
            _repository.Verify(x => x.Get<DataModelHouse>());
            _repository.Verify(x =>
                x.Update(It.IsAny<DataModelHouse>()), Times.Never);
            _repository.Verify(x => x.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task UpdateHouse_Should_Throw_KeyNotFoundException_When_Updating_Not_Existing_Entity()
        {
            //Arrange
            _repository.Setup(x => x.Get<DataModelHouse>())
                .Returns(Houses.GetSeededHouses().AsQueryable().BuildMock());

            _repository.Setup(x => x.Update(It.IsAny<DataModelHouse>()));

            var update = new House.Update()
            {
                Area = 5,
                Color = "Brown",
                Name = "SomeNewName"
            };

            //Act + Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(async () =>
                await _houseService.UpdateHouse(update, Guid.NewGuid()));

            _repository.Verify(x => x.Get<DataModelHouse>());
            _repository.Verify(x =>
                x.Update(It.IsAny<DataModelHouse>()), Times.Never);
            _repository.Verify(x => x.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task UpdateHouse_Should_Throw_ArgumentNullException_When_Update_Definition_Null()
        {
            //Arrange
            _repository.Setup(x => x.Get<DataModelHouse>())
                .Returns(Houses.GetSeededHouses().AsQueryable().BuildMock());

            _repository.Setup(x => x.Update(It.IsAny<DataModelHouse>()));

            //Act + Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await _houseService.UpdateHouse((House.Update)null, Guid.NewGuid()));

            _repository.Verify(x => x.Get<DataModelHouse>(), Times.Never);
            _repository.Verify(x =>
                x.Update(It.IsAny<DataModelHouse>()), Times.Never);
            _repository.Verify(x => x.SaveChangesAsync(), Times.Never);
        }
        
        [Fact]
        public async Task UpdateHouse_Should_Throw_ArgumentException_When_Id_Is_Empty()
        {
            //Arrange
            _repository.Setup(x => x.Get<DataModelHouse>())
                .Returns(Houses.GetSeededHouses().AsQueryable().BuildMock());

            _repository.Setup(x => x.Update(It.IsAny<DataModelHouse>()));

            var update = new House.Update()
            {
                Area = 5,
                Color = "Brown",
                Name = "SomeNewName"
            };
            
            //Act + Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
                await _houseService.UpdateHouse(update, Guid.Empty));

            _repository.Verify(x => x.Get<DataModelHouse>(), Times.Never);
            _repository.Verify(x =>
                x.Update(It.IsAny<DataModelHouse>()), Times.Never);
            _repository.Verify(x => x.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task UpdateHouse_Should_Update_Entity_When_Update_Definition_Is_Valid()
        {
            //Arrange
            _repository.Setup(x => x.Get<DataModelHouse>())
                .Returns(Houses.GetSeededHouses().AsQueryable().BuildMock());

            _repository.Setup(x => x.Update(It.IsAny<DataModelHouse>()));

            var update = new House.Update()
            {
                Area = 5,
                Color = "Brown",
                Name = "SomeNewName"
            };

            var seededHouse = Houses.House2;

            //Act
            await _houseService.UpdateHouse(update, seededHouse.Id);

            //Assert
            _repository.Verify(x => x.Get<DataModelHouse>(), Times.Once);
            _repository.Verify(x =>
                x.Update(It.Is<DataModelHouse>(house =>
                    house.Area == update.Area &&
                    house.Color == update.Color &&
                    house.Name == update.Name &&
                    seededHouse.Rooms.Count == house.Rooms.Count)), Times.Once);
            _repository.Verify(x => x.SaveChangesAsync(), Times.Once);
        }
        
        [Fact]
        public async Task RemoveHouseById_Should_Throw_ArgumentException_When_Id_Is_Empty()
        {
            //Arrange
            _repository.Setup(x => x.Get<DataModelHouse>())
                .Returns(Houses.GetSeededHouses().AsQueryable().BuildMock());

            _repository.Setup(x => x.Remove(It.IsAny<DataModelHouse>()));

            //Act + Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
                await _houseService.RemoveHouseById(Guid.Empty));

            _repository.Verify(x => x.Get<DataModelHouse>(), Times.Never);
            _repository.Verify(x =>
                x.Update(It.IsAny<DataModelHouse>()), Times.Never);
            _repository.Verify(x => x.SaveChangesAsync(), Times.Never);
        }
        
        [Fact]
        public async Task RemoveHouseById_Should_Throw_KeyNotFoundException_When_Entity_Not_Found()
        {
            //Arrange
            _repository.Setup(x => x.Get<DataModelHouse>())
                .Returns(Houses.GetSeededHouses().AsQueryable().BuildMock());

            _repository.Setup(x => x.Remove(It.IsAny<DataModelHouse>()));

            //Act + Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(async () =>
                await _houseService.RemoveHouseById(Guid.NewGuid()));

            _repository.Verify(x => x.Get<DataModelHouse>(), Times.Once);
            _repository.Verify(x =>
                x.Remove(It.IsAny<DataModelHouse>()), Times.Never);
            _repository.Verify(x => x.SaveChangesAsync(), Times.Never);
        }
        
        [Fact]
        public async Task RemoveHouseById_Should_Call_Remove_On_Repository_To_Remove_Entity()
        {
            //Arrange
            _repository.Setup(x => x.Get<DataModelHouse>())
                .Returns(Houses.GetSeededHouses().AsQueryable().BuildMock());

            _repository.Setup(x => x.Remove(It.IsAny<DataModelHouse>()));

            //Act + Assert
            await _houseService.RemoveHouseById(Houses.House1.Id);

            _repository.Verify(x => x.Get<DataModelHouse>(), Times.Once);
            _repository.Verify(x =>
                x.Remove(It.Is<DataModelHouse>(house=>house == Houses.House1)), Times.Once);
            _repository.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task CreateHouse_Should_Use_Repo_To_Store_Entity()
        {
            //Arrange
            _repository.Setup(x => x.Add(It.IsAny<DataModelHouse>()));
            var houseRequest = new House.Request()
            {
                Area = 5,
                Color = "Brown",
                Name = "SomeNewName"
            };
            
            //Act
            await _houseService.CreateHouse(houseRequest);

            //Assert
            _repository.Verify(x =>
                x.Add(It.Is<DataModelHouse>(house=>house.Area == houseRequest.Area &&
                                               house.Color == houseRequest.Color &&
                                               house.Name == houseRequest.Name)), Times.Once);
            _repository.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public void CreateHouse_Should_Throw_ArgumentNullException_When_Request_IsNull()
        {
            //Act + Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () => await _houseService.CreateHouse(null));
            
            _repository.Verify(x =>
                x.Add(It.IsAny<DataModelHouse>()),Times.Never);
            _repository.Verify(x => x.SaveChangesAsync(), Times.Never);
        }
    }
}