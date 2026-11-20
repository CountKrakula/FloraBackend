using System.Security.Claims;
using FloraBackend.Controllers;
using FloraBackend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace FloraBackend.Tests.Controllers;

public class UserControllerTests
{
    private readonly Mock<UserManager<User>> _userManagerMock;
    private readonly Mock<SignInManager<User>> _signInManagerMock;
    private readonly UserController _controller;

    public UserControllerTests()
    {
        var store = new Mock<IUserStore<User>>();
        _userManagerMock = new Mock<UserManager<User>>(
            store.Object, null!, null!, null!, null!, null!, null!, null!, null!);

        _signInManagerMock = new Mock<SignInManager<User>>(
            _userManagerMock.Object,
            new Mock<IHttpContextAccessor>().Object,
            new Mock<IUserClaimsPrincipalFactory<User>>().Object,
            null!, null!, null!, null!);

        _controller = new UserController(_userManagerMock.Object, _signInManagerMock.Object);

        // Simulates logged-in user (Id = 1)
        var user = new ClaimsPrincipal(new ClaimsIdentity([new Claim(ClaimTypes.NameIdentifier, "1")]));
        _controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user } };
    }

    [Fact]
    public async Task DeleteAccount_ReturnsOk_WhenPasswordIsValid()
    {
        // Arrange
        var mockUser = new User { Id = 1 };
        _userManagerMock.Setup(x => x.FindByIdAsync("1")).ReturnsAsync(mockUser);
        _userManagerMock.Setup(x => x.CheckPasswordAsync(mockUser, "rättLösenord")).ReturnsAsync(true);
        _userManagerMock.Setup(x => x.DeleteAsync(mockUser)).ReturnsAsync(IdentityResult.Success);

        // Act
        var result = await _controller.DeleteAccount(new UserController.DeleteAccountDto { Password = "rättLösenord" });

        // Assert
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task DeleteAccount_ReturnsBadRequest_WhenPasswordIsWrong()
    {
        // Arrange
        var mockUser = new User { Id = 1 };
        _userManagerMock.Setup(x => x.FindByIdAsync("1")).ReturnsAsync(mockUser);
        _userManagerMock.Setup(x => x.CheckPasswordAsync(mockUser, "felLösenord")).ReturnsAsync(false);

        // Act
        var result = await _controller.DeleteAccount(new UserController.DeleteAccountDto { Password = "felLösenord" });

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }
}