using E_Commerce.Application.DTOs.Identity;
using E_Commerce.Application.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_Commerce.API.Controllers;

public class AuthenticationController(IAuthenticationService authenticationService) : ApiBaseController
{
    // Login
    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto, CancellationToken ct = default)
    {
        var result = await authenticationService.LoginAsync(loginDto, ct);

        return ToActionResult(result);
    }

    // Register
    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto, CancellationToken ct = default)
    {
        var result = await authenticationService.RegisterAsync(registerDto, ct);

        return ToActionResult(result);
    }

    // Check Email Exists
    [HttpGet("emailExists/{email}")]
    public async Task<ActionResult<bool>> CheckEmail(string email, CancellationToken ct = default)
    {
        var result = await authenticationService.CheckEmailAsync(email, ct);

        return ToActionResult(result);
    }

    // Get Current User
    [HttpGet("currentUser")]
    [Authorize]
    public async Task<ActionResult<UserDto>> GetCurrentUser(CancellationToken ct = default)
    {
        // We will get Email from Token that user will send 
        var email = User.FindFirstValue(ClaimTypes.Email) ?? throw new UnauthorizedAccessException();

        var result = await authenticationService.GetCurrentUserAsync(email, ct);

        return ToActionResult(result);
    }

    // Get Current User Address
    [HttpGet("Address")]
    [Authorize]
    public async Task<ActionResult<AddressDto>> GetCurrentUserAddress(CancellationToken ct = default)
    {
        var email = User.FindFirstValue(ClaimTypes.Email) ?? throw new UnauthorizedAccessException();

        var result = await authenticationService.GetCurrentUserAddressAsync(email, ct);

        return ToActionResult<AddressDto>(result);
    }

    //Update Current User Address
    [HttpPut("Address")]
    [Authorize]
    public async Task<ActionResult<AddressDto>> UpdateCurrentUserAddress(AddressDto dto, CancellationToken ct = default)
    {
        var email = User.FindFirstValue(ClaimTypes.Email) ?? throw new UnauthorizedAccessException();

        var result = await authenticationService.UpdateCurrentUserAddressAsync(email, dto, ct);

        return ToActionResult<AddressDto>(result);
    }
}
