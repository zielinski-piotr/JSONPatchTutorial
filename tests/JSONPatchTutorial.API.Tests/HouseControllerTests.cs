using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JsonPatchTutorial.API.Controllers;
using JSONPatchTutorial.Contract.Responses;
using JSONPatchTutorial.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace JSONPatchTutorial.API.Tests;

public class HouseControllerTests
{
    private readonly Mock<IHouseService> _houseServiceMock;
    private readonly HouseController _houseController;

    public HouseControllerTests()
    {
        _houseServiceMock = new Mock<IHouseService>();
        var loggerMock = new Mock<ILogger<HouseController>>();

        _houseController = new HouseController(_houseServiceMock.Object, loggerMock.Object);
    }

    #region GetById_Should

    [Fact]
    public async Task GetById_Should_Return_200OK_OnSuccess()
    {
        // Arrange
        _houseServiceMock.Setup(x => x.GetHouseById(It.IsAny<Guid>()))
            .ReturnsAsync(new House.Response());

        // Act
        var response = await _houseController.GetHouseById(Guid.NewGuid());

        var result = response as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(StatusCodes.Status200OK, result.StatusCode!.Value);

        _houseServiceMock.Verify(x => x.GetHouseById(It.IsAny<Guid>()), Times.Once);
    }

    [Fact]
    public async Task GetById_Should_Return_400BadRequest_OnArgumentException()
    {
        // Arrange
        _houseServiceMock.Setup(x => x.GetHouseById(It.IsAny<Guid>()))
            .ThrowsAsync(new ArgumentException(string.Empty));

        // Act
        var response = await _houseController.GetHouseById(Guid.NewGuid());

        var result = response as BadRequestResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);

        _houseServiceMock.Verify(x => x.GetHouseById(It.IsAny<Guid>()), Times.Once);
    }

    [Fact]
    public async Task GetById_Should_Return_404NotFound_OnKeyNotFoundException()
    {
        // Arrange
        _houseServiceMock.Setup(x => x.GetHouseById(It.IsAny<Guid>()))
            .ThrowsAsync(new KeyNotFoundException());

        // Act
        var response = await _houseController.GetHouseById(Guid.NewGuid());

        var result = response as NotFoundResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(StatusCodes.Status404NotFound, result.StatusCode);

        _houseServiceMock.Verify(x => x.GetHouseById(It.IsAny<Guid>()), Times.Once);
    }

    [Fact]
    public async Task GetById_Should_Return_500InternalServerError_OnException()
    {
        // Arrange
        _houseServiceMock.Setup(x => x.GetHouseById(It.IsAny<Guid>()))
            .ThrowsAsync(new Exception());

        // Act
        var response = await _houseController.GetHouseById(Guid.NewGuid());

        var result = response as StatusCodeResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, result.StatusCode);

        _houseServiceMock.Verify(x => x.GetHouseById(It.IsAny<Guid>()), Times.Once);
    }

    #endregion

    #region GetHouses_Should

    [Fact]
    public async Task GetHouses_Should_Return_200OK_OnSuccess()
    {
        // Arrange
        _houseServiceMock.Setup(x => x.GetHouses())
            .ReturnsAsync(new[] {new House.ListItem()});

        // Act
        var response = await _houseController.GetHouses();

        var result = response as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(StatusCodes.Status200OK, result.StatusCode!.Value);

        _houseServiceMock.Verify(x => x.GetHouses(), Times.Once);
    }

    [Fact]
    public async Task GetHouses_Should_Return_500InternalServerError_OnException()
    {
        // Arrange
        _houseServiceMock.Setup(x => x.GetHouses())
            .ThrowsAsync(new Exception());

        // Act
        var response = await _houseController.GetHouses();

        var result = response as StatusCodeResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, result.StatusCode);

        _houseServiceMock.Verify(x => x.GetHouses(), Times.Once);
    }

    #endregion

    #region PatchHouse_Should

    [Fact]
    public async Task PatchHouse_Should_Return_202Accepted_OnSuccess()
    {
        // Act
        var response = await _houseController.PatchHouse(
            new JsonPatchDocument<Contract.Requests.House.Patch>(),
            Guid.NewGuid());

        var result = response as AcceptedResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(StatusCodes.Status202Accepted, result.StatusCode);

        _houseServiceMock.Verify(x => x.UpdateHouse(
                It.IsAny<JsonPatchDocument<Contract.Requests.House.Patch>>(),
                It.IsAny<Guid>()),
            Times.Once);
    }

    [Fact]
    public async Task PatchHouse_Should_Return_400BadRequest_OnArgumentException()
    {
        // Arrange
        _houseServiceMock.Setup(x => x.UpdateHouse(
                It.IsAny<JsonPatchDocument<Contract.Requests.House.Patch>>(),
                It.IsAny<Guid>()))
            .ThrowsAsync(new ArgumentException(string.Empty));

        // Act
        var response = await _houseController.PatchHouse(
            new JsonPatchDocument<Contract.Requests.House.Patch>(),
            Guid.NewGuid());

        var result = response as BadRequestResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);

        _houseServiceMock.Verify(x => x.UpdateHouse(
                It.IsAny<JsonPatchDocument<Contract.Requests.House.Patch>>(),
                It.IsAny<Guid>()),
            Times.Once);
    }

    [Fact]
    public async Task PatchHouse_Should_Return_404BadRequest_OnKeyNotFoundException()
    {
        // Arrange
        _houseServiceMock.Setup(x => x.UpdateHouse(
                It.IsAny<JsonPatchDocument<Contract.Requests.House.Patch>>(),
                It.IsAny<Guid>()))
            .ThrowsAsync(new KeyNotFoundException());

        // Act
        var response = await _houseController.PatchHouse(
            new JsonPatchDocument<Contract.Requests.House.Patch>(),
            Guid.NewGuid());

        var result = response as NotFoundResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(StatusCodes.Status404NotFound, result.StatusCode);

        _houseServiceMock.Verify(x => x.UpdateHouse(
                It.IsAny<JsonPatchDocument<Contract.Requests.House.Patch>>(),
                It.IsAny<Guid>()),
            Times.Once);
    }

    [Fact]
    public async Task PatchHouse_Should_Return_Status422UnprocessableEntity_OnJsonPatchException()
    {
        // Arrange
        _houseServiceMock.Setup(x => x.UpdateHouse(
                It.IsAny<JsonPatchDocument<Contract.Requests.House.Patch>>(),
                It.IsAny<Guid>()))
            .ThrowsAsync(new JsonPatchException());

        // Act
        var response = await _houseController.PatchHouse(
            new JsonPatchDocument<Contract.Requests.House.Patch>(),
            Guid.NewGuid());

        var result = response as UnprocessableEntityResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(StatusCodes.Status422UnprocessableEntity, result.StatusCode);

        _houseServiceMock.Verify(x => x.UpdateHouse(
                It.IsAny<JsonPatchDocument<Contract.Requests.House.Patch>>(),
                It.IsAny<Guid>()),
            Times.Once);
    }

    [Fact]
    public async Task PatchHouse_Should_Return_Status500InternalServerError_OnException()
    {
        // Arrange
        _houseServiceMock.Setup(x => x.UpdateHouse(
                It.IsAny<JsonPatchDocument<Contract.Requests.House.Patch>>(),
                It.IsAny<Guid>()))
            .ThrowsAsync(new Exception());

        // Act
        var response = await _houseController.PatchHouse(
            new JsonPatchDocument<Contract.Requests.House.Patch>(),
            Guid.NewGuid());

        var result = response as StatusCodeResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, result.StatusCode);

        _houseServiceMock.Verify(x => x.UpdateHouse(
                It.IsAny<JsonPatchDocument<Contract.Requests.House.Patch>>(),
                It.IsAny<Guid>()),
            Times.Once);
    }

    #endregion

    #region UpdateHouse_Should

    [Fact]
    public async Task UpdateHouse_Should_Return_202Accepted_OnSuccess()
    {
        // Act
        var response = await _houseController.UpdateHouse(
            new Contract.Requests.House.Update(),
            Guid.NewGuid());

        var result = response as AcceptedResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(StatusCodes.Status202Accepted, result.StatusCode);

        _houseServiceMock.Verify(x => x.UpdateHouse(
                It.IsAny<Contract.Requests.House.Update>(),
                It.IsAny<Guid>()),
            Times.Once);
    }

    [Fact]
    public async Task UpdateHouse_Should_Return_400BadRequest_OnArgumentException()
    {
        // Arrange
        _houseServiceMock.Setup(x => x.UpdateHouse(
                It.IsAny<Contract.Requests.House.Update>(),
                It.IsAny<Guid>()))
            .ThrowsAsync(new ArgumentException(string.Empty));

        // Act
        var response = await _houseController.UpdateHouse(
            new Contract.Requests.House.Update(),
            Guid.NewGuid());

        var result = response as BadRequestResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);

        _houseServiceMock.Verify(x => x.UpdateHouse(
                It.IsAny<Contract.Requests.House.Update>(),
                It.IsAny<Guid>()),
            Times.Once);
    }

    [Fact]
    public async Task UpdateHouse_Should_Return_404BadRequest_OnKeyNotFoundException()
    {
        // Arrange
        _houseServiceMock.Setup(x => x.UpdateHouse(
                It.IsAny<Contract.Requests.House.Update>(),
                It.IsAny<Guid>()))
            .ThrowsAsync(new KeyNotFoundException());

        // Act
        var response = await _houseController.UpdateHouse(
            new Contract.Requests.House.Update(),
            Guid.NewGuid());

        var result = response as NotFoundResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(StatusCodes.Status404NotFound, result.StatusCode);

        _houseServiceMock.Verify(x => x.UpdateHouse(
                It.IsAny<Contract.Requests.House.Update>(),
                It.IsAny<Guid>()),
            Times.Once);
    }

    [Fact]
    public async Task UpdateHouse_Should_Return_Status500InternalServerError_OnException()
    {
        // Arrange
        _houseServiceMock.Setup(x => x.UpdateHouse(
                It.IsAny<Contract.Requests.House.Update>(),
                It.IsAny<Guid>()))
            .ThrowsAsync(new Exception());

        // Act
        var response = await _houseController.UpdateHouse(
            new Contract.Requests.House.Update(),
            Guid.NewGuid());

        var result = response as StatusCodeResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, result.StatusCode);

        _houseServiceMock.Verify(x => x.UpdateHouse(
                It.IsAny<Contract.Requests.House.Update>(),
                It.IsAny<Guid>()),
            Times.Once);
    }

    #endregion

    #region RemoveHouse_Should

    [Fact]
    public async Task RemoveHouse_Should_Return202AcceptedStatus_OnSuccess()
    {
        // Act
        var response = await _houseController.RemoveHouse(Guid.NewGuid());

        var result = response as AcceptedResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(StatusCodes.Status202Accepted, result.StatusCode);

        _houseServiceMock.Verify(x => x.RemoveHouseById(It.IsAny<Guid>()), Times.Once);
    }

    [Fact]
    public async Task RemoveHouse_Should_Return400BadRequestStatus_OnArgumentException()
    {
        // Arrange
        _houseServiceMock.Setup(x => x.RemoveHouseById(It.IsAny<Guid>()))
            .ThrowsAsync(new ArgumentException(string.Empty));

        // Act
        var response = await _houseController.RemoveHouse(Guid.NewGuid());

        var result = response as BadRequestResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);

        _houseServiceMock.Verify(x => x.RemoveHouseById(It.IsAny<Guid>()), Times.Once);
    }

    [Fact]
    public async Task RemoveHouse_Should_Return404NotFoundStatus_OnKeyNotFoundException()
    {
        // Arrange
        _houseServiceMock.Setup(x => x.RemoveHouseById(It.IsAny<Guid>()))
            .ThrowsAsync(new KeyNotFoundException());

        // Act
        var response = await _houseController.RemoveHouse(Guid.NewGuid());

        var result = response as NotFoundResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(StatusCodes.Status404NotFound, result.StatusCode);

        _houseServiceMock.Verify(x => x.RemoveHouseById(It.IsAny<Guid>()), Times.Once);
    }

    [Fact]
    public async Task RemoveHouse_Should_Return500InternalServerErrorStatus_OnException()
    {
        // Arrange
        _houseServiceMock.Setup(x => x.RemoveHouseById(It.IsAny<Guid>()))
            .ThrowsAsync(new Exception());

        // Act
        var response = await _houseController.RemoveHouse(Guid.NewGuid());

        var result = response as StatusCodeResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, result.StatusCode);

        _houseServiceMock.Verify(x => x.RemoveHouseById(It.IsAny<Guid>()), Times.Once);
    }

    #endregion

    #region CreateHouse_Should

    [Fact]
    public async Task CreateHouse_Should_Return201CreatedStatus_OnSuccess()
    {
        // Arrange
        _houseServiceMock.Setup(x => x.CreateHouse(It.IsAny<Contract.Requests.House.Request>()))
            .ReturnsAsync(new House.Response());

        // Act
        var response = await _houseController.CreateHouse(new Contract.Requests.House.Request());

        var result = response as CreatedAtActionResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(StatusCodes.Status201Created, result.StatusCode);

        _houseServiceMock.Verify(x => x.CreateHouse(It.IsAny<Contract.Requests.House.Request>()), Times.Once);
    }

    [Fact]
    public async Task CreateHouse_Should_Return400BadRequestStatus_OnArgumentException()
    {
        // Arrange
        _houseServiceMock.Setup(x => x.CreateHouse(It.IsAny<Contract.Requests.House.Request>()))
            .ThrowsAsync(new ArgumentException(string.Empty));

        // Act
        var response = await _houseController.CreateHouse(new Contract.Requests.House.Request());

        var result = response as BadRequestResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);

        _houseServiceMock.Verify(x => x.CreateHouse(It.IsAny<Contract.Requests.House.Request>()), Times.Once);
    }

    [Fact]
    public async Task CreateHouse_Should_Return500InternalServerErrorStatus_OnException()
    {
        // Arrange
        _houseServiceMock.Setup(x => x.CreateHouse(It.IsAny<Contract.Requests.House.Request>()))
            .ThrowsAsync(new Exception());

        // Act
        var response = await _houseController.CreateHouse(new Contract.Requests.House.Request());

        var result = response as StatusCodeResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, result.StatusCode);

        _houseServiceMock.Verify(x => x.CreateHouse(It.IsAny<Contract.Requests.House.Request>()), Times.Once);
    }

    #endregion
}