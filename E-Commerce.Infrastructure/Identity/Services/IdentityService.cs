using E_Commerce.Application.Common;
using E_Commerce.Application.DTOs.Identity;
using E_Commerce.Application.Services.Contracts;
using E_Commerce.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Infrastructure.Identity.Services;

public class IdentityService(UserManager<ApplicationUser> userManager) : IIdentityService
{
    public async Task<Result<bool>> CheckPasswordAsync(string email, string password, CancellationToken ct = default)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user is null)
            return Result<bool>.Fail(Error.InvalidCredentials("User.InvalidCredentials" ,"Email or Password Invalid"));

        var result = await userManager.CheckPasswordAsync(user ,password);

        return result ?
            Result<bool>.Ok(result)
            :
            Result<bool>.Fail(Error.InvalidCredentials("user.InvalidCredentials" , "Email or Password Invalid"));
    }

    public async Task<Result<IdentityUserResult>> FindUserByEmailAsync(string email, CancellationToken ct = default)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user is null)
            return Result<IdentityUserResult>.Fail(Error.NotFound("User.NotFound"));

        return Result<IdentityUserResult>.Ok(new IdentityUserResult
        (
            user.Id,
            user.DisplayName,
            user.Email,
            user.UserName
        ));
    }

    public async Task<Result<IdentityUserResult>> CreateUserAsync(RegisterDto registerDto, CancellationToken ct = default)
    {
        var user = new ApplicationUser()
        {
            Email = registerDto.Email,
            PhoneNumber = registerDto.PhoneNumber,
            DisplayName = registerDto.DisplayName,
            UserName = registerDto.UserName,
        };

        var createResult = await userManager.CreateAsync(user, registerDto.Password);
        if (!createResult.Succeeded)
        {
            var errors = createResult.Errors.Select(e => new Error(e.Code, e.Description)).ToList();
            return Result<IdentityUserResult>.Fail(errors);
        }

        return Result<IdentityUserResult>.Ok(new IdentityUserResult(user.Id, user.DisplayName, user.Email, user.UserName));
    }

    public async Task<Result<IReadOnlyList<string>>> GetUserRolesAsync(string email, CancellationToken ct = default)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user is null)
            return Result<IReadOnlyList<string>>.Fail(Error.NotFound("User.NotFound"));

        var roles = await userManager.GetRolesAsync(user);

        return Result<IReadOnlyList<string>>.Ok(roles.ToList());
    }

    public async Task<Result<AddressDto>> GetCurrentUserAddressAsync(string email, CancellationToken ct = default)
    {
        var user = await userManager.Users.Include(U => U.Address).Where(U => U.Email == email).FirstOrDefaultAsync(ct);
        
        if (user is null)
            return Result<AddressDto>.Fail(Error.NotFound("User.NotFound"));

        if (user.Address is null)
            return Result<AddressDto>.Fail(Error.NotFound("User.NotFound", "User with this Email doesn't have Address"));

        return Result<AddressDto>.Ok(new AddressDto()
        {
            Street = user.Address.Street,
            City = user.Address.City,
            Country = user.Address.Country,
            FirstName = user.Address.FirstName,
            LastName = user.Address.LastName,
        });

    }

    public async Task<Result<AddressDto>> UpdateCurrentUserAddressAsync(string email, AddressDto dto, CancellationToken ct = default)
    {
        var user = await userManager.Users.Include(U => U.Address).Where(U => U.Email == email).FirstOrDefaultAsync(ct);

        if (user is null)
            return Result<AddressDto>.Fail(Error.NotFound("User.NotFound"));

        if (user.Address is null)
        {
            // Add Address
            user.Address = new Address()
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Street = dto.Street,
                City = dto.City,
                Country = dto.Country
            };
        }
        else
        {
            // Update Address
            user.Address.FirstName = dto.FirstName;
            user.Address.LastName = dto.LastName;
            user.Address.Street = dto.Street;
            user.Address.City = dto.City;
            user.Address.Country = dto.Country;
        }

        var result = await userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            return Result<AddressDto>.Fail(Error.Failure("Failure", "Can not update or Create Address"));
        }


        return Result<AddressDto>.Ok(dto);
    }
}
