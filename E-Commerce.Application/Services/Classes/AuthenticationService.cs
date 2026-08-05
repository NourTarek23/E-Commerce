using E_Commerce.Application.Common;
using E_Commerce.Application.DTOs.Identity;
using E_Commerce.Application.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Services.Classes;

public class AuthenticationService(IIdentityService identityService, ITokenService tokenService) : IAuthenticationService
{

    public async Task<Result<UserDto>> LoginAsync(LoginDto loginDto, CancellationToken ct = default)
    {
        // check Email
        var userResult = await identityService.FindUserByEmailAsync(loginDto.Email, ct);

        if (!userResult.IsSuccess)
            return Result<UserDto>.Fail(userResult.Errors);

        // check password
        var passwordResult = await identityService.CheckPasswordAsync(loginDto.Email, loginDto.Password, ct);

        if (!passwordResult.IsSuccess)
            return Result<UserDto>.Fail(passwordResult.Errors);

        // return userDto
        var user = userResult.Data;

        var rolesResult = await identityService.GetUserRolesAsync(user.Email, ct);

        var token = await tokenService.CreateTokenAsync(user.Id, user.Email, user.UserName, rolesResult.Data);

        return Result<UserDto>.Ok(new UserDto()
        {
            Email = user.Email,
            Token = token,
            DisplayName = user.DisplayName,
        });
    }

    public async Task<Result<UserDto>> RegisterAsync(RegisterDto registerDto, CancellationToken ct = default)
    {
        //create user
        var createResult = await identityService.CreateUserAsync(registerDto, ct);

        if (!createResult.IsSuccess)
        {
            return Result<UserDto>.Fail(createResult.Errors);
        }

        var user = createResult.Data;

        var rolesResult = await identityService.GetUserRolesAsync(user.Email, ct);

        var token = await tokenService.CreateTokenAsync(user.Id, user.Email, user.UserName, rolesResult.Data);

        return Result<UserDto>.Ok(new UserDto()
        {
            Email = user.Email,
            Token = token,
            DisplayName = user.DisplayName,
        });
    }

    public async Task<Result<bool>> CheckEmailAsync(string email, CancellationToken ct = default)
    {
        var result = await identityService.FindUserByEmailAsync(email, ct);
        if (!result.IsSuccess)
        {
            return Result<bool>.Fail(result.Errors);
        }

        return Result<bool>.Ok(true);
    }

    public async Task<Result<UserDto>> GetCurrentUserAsync(string email, CancellationToken ct = default)
    {
        var userResult = await identityService.FindUserByEmailAsync(email, ct);

        var user = userResult.Data;

        var rolesResult = await identityService.GetUserRolesAsync(user.Email, ct);

        var token = await tokenService.CreateTokenAsync(user.Id, user.Email, user.UserName, rolesResult.Data);

        if (!userResult.IsSuccess)
        {
            return Result<UserDto>.Fail(userResult.Errors);
        }

        return Result<UserDto>.Ok(new UserDto()
        {
            DisplayName = user.DisplayName,
            Email = user.Email,
            Token = token
        });
    }

    public async Task<Result<AddressDto>> GetCurrentUserAddressAsync(string email, CancellationToken ct = default)
    {
        return await identityService.GetCurrentUserAddressAsync(email, ct);

    }

    public async Task<Result<AddressDto>> UpdateCurrentUserAddressAsync(string email, AddressDto dto, CancellationToken ct = default)
    {
        return await identityService.UpdateCurrentUserAddressAsync(email, dto, ct);
    }
}
