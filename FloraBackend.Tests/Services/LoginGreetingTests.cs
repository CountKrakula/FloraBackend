using System.Security.Claims;
using FloraBackend.Models;
using FloraBackend.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace FloraBackend.Tests.Services;

public class LoginGreetingTests
{
    [Fact]
    public async Task OnSigningInAsync_MarksFirstLogin_WhenUserHasNeverSignedIn()
    {
        // Arrange
        var user = new User { Id = 1 };

        var userManagerMock = new Mock<UserManager<User>>(
            Mock.Of<IUserStore<User>>(),
            null!, null!, null!, null!,
            null!, null!, null!, null!
        );

        var principal = new ClaimsPrincipal(
            new ClaimsIdentity(
                new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                },
                IdentityConstants.ApplicationScheme
            )
        );

        userManagerMock
            .Setup(manager => manager.GetUserAsync(principal))
            .ReturnsAsync(user);

        userManagerMock
            .Setup(manager => manager.GetAuthenticationTokenAsync(
                user, "Flora", "HasSignedIn"))
            .ReturnsAsync((string?)null);

        userManagerMock
            .Setup(manager => manager.SetAuthenticationTokenAsync(
                user, "Flora", "HasSignedIn", "true"))
            .ReturnsAsync(IdentityResult.Success);

        using var services = new ServiceCollection()
            .AddSingleton(userManagerMock.Object)
            .BuildServiceProvider();

        var httpContext = new DefaultHttpContext
        {
            RequestServices = services
        };

        httpContext.Request.Method = HttpMethods.Post;
        httpContext.Request.Path = "/login";

        var scheme = new AuthenticationScheme(
            IdentityConstants.ApplicationScheme,
            null,
            typeof(CookieAuthenticationHandler)
        );

        var context = new CookieSigningInContext(
            httpContext,
            scheme,
            new CookieAuthenticationOptions(),
            principal,
            new AuthenticationProperties(),
            new CookieOptions()
        );

        // Act
        await LoginGreeting.OnSigningInAsync(context);

        // Assert
        Assert.True(
            context.Properties.Items.ContainsKey(LoginGreeting.FirstLoginKey)
        );

        userManagerMock.Verify(
            manager => manager.SetAuthenticationTokenAsync(
                user, "Flora", "HasSignedIn", "true"),
            Times.Once()
        );
    }

    [Fact]
    public async Task OnSigningInAsync_DoesNotMarkFirstLogin_WhenUserHasSignedInBefore()
    {
        // Arrange
        var user = new User { Id = 1 };

        var userManagerMock = new Mock<UserManager<User>>(
            Mock.Of<IUserStore<User>>(),
            null!, null!, null!, null!,
            null!, null!, null!, null!
        );

        var principal = new ClaimsPrincipal(
            new ClaimsIdentity(
                new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                },
                IdentityConstants.ApplicationScheme
            )
        );

        userManagerMock
            .Setup(manager => manager.GetUserAsync(principal))
            .ReturnsAsync(user);

        userManagerMock
            .Setup(manager => manager.GetAuthenticationTokenAsync(
                user, "Flora", "HasSignedIn"))
            .ReturnsAsync("true");

        userManagerMock
            .Setup(manager => manager.SetAuthenticationTokenAsync(
                user, "Flora", "HasSignedIn", "true"))
            .ReturnsAsync(IdentityResult.Success);

        using var services = new ServiceCollection()
            .AddSingleton(userManagerMock.Object)
            .BuildServiceProvider();

        var httpContext = new DefaultHttpContext
        {
            RequestServices = services
        };

        httpContext.Request.Method = HttpMethods.Post;
        httpContext.Request.Path = "/login";

        var scheme = new AuthenticationScheme(
            IdentityConstants.ApplicationScheme,
            null,
            typeof(CookieAuthenticationHandler)
        );

        var context = new CookieSigningInContext(
            httpContext,
            scheme,
            new CookieAuthenticationOptions(),
            principal,
            new AuthenticationProperties(),
            new CookieOptions()
        );

        // Act
        await LoginGreeting.OnSigningInAsync(context);

        // Assert
        Assert.False(
            context.Properties.Items.ContainsKey(LoginGreeting.FirstLoginKey)
        );

        userManagerMock.Verify(
            manager => manager.SetAuthenticationTokenAsync(
                user, "Flora", "HasSignedIn", "true"),
            Times.Never()
        );
    }
}
