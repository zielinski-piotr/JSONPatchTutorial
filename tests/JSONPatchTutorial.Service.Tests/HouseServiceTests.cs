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
                .Returns(Houses.GetSeededHouses().AsQueryable().BuildMock().Object);

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
                .Returns(Houses.GetSeededHouses().AsQueryable().BuildMock().Object);

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
                .Returns(Houses.GetSeededHouses().AsQueryable().BuildMock().Object);

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
                .Returns(Houses.GetSeededHouses().AsQueryable().BuildMock().Object);

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
                .Returns(Houses.GetSeededHouses().AsQueryable().BuildMock().Object);

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
            UpdateHouse_Should_Trow_ArgumentNullException_When_Replacing_Value_On_Property_Of_Composed_Object_Which_Is_Null()
        {
            //Arrange
            _repository.Setup(x => x.Get<DataModelHouse>())
                .Returns(Houses.GetSeededHouses().AsQueryable().BuildMock().Object);

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
            UpdateHouse_Should_Throw_ArgumentNullException_When_Adding_Value_On_Property_Of_Composed_Object_Which_Is_Null()
        {
            //Arrange
            _repository.Setup(x => x.Get<DataModelHouse>())
                .Returns(Houses.GetSeededHouses().AsQueryable().BuildMock().Object);

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
    }
}