using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using WebCrud.Data;
using WebCrud.DTOs;
using WebCrud.Entities;

namespace WebCrud.Controllers;
[ApiController]
[Route("[controller]")]
public class AccountController(DataContext context):BaseApiController
{
    [HttpPost("register")]
    public async Task<ActionResult<User>> Register(RegisterDto registerDto)
    {
        if (await UserExists(registerDto.Username)) return BadRequest("Username is taken");
        using var hmac = new HMACSHA512();
        var user = new User
        {
            UserName = registerDto.Username.ToLower(),
            Email = "abc@email.com",
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
            PasswordSalt = hmac.Key,

        };

        context.Users.Add(user);
        await context.SaveChangesAsync();
        return user;
    }
    [HttpPost("Login")]
    public async Task<ActionResult<User>> Login(LoginDto loginDto)
    {
        var user = await context.Users.FirstOrDefaultAsync(x =>
        x.UserName.ToLower() == loginDto.Username.ToLower()
        );
        if (user is null) return Unauthorized("Invalid Username");
        using var hmac = new HMACSHA512(user.PasswordSalt);
        var computerHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));
        for (int i = 0; i < computerHash.Length; i++)
        {
            if (computerHash[i] != user.PasswordHash[i])
                return Unauthorized("Invalid Password");
        }
        return user;
    }
    [HttpPut("ChangePassword")]
    public async Task<ActionResult<User>> UpdatePassword(UpdateUserPassDto updateUserDto)
    {
        var user = await context.Users.FirstOrDefaultAsync(x =>
        x.UserName.ToLower() == updateUserDto.UserName.ToLower());
        if (user is null) return Unauthorized("Invalid User");
        using var hmac = new HMACSHA512(user.PasswordSalt);
        var computerHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(updateUserDto.CurrPassword));
        for (int i = 0; i < computerHash.Length; i++)
        {
            if (computerHash[i] != user.PasswordHash[i])
                return Unauthorized("Invalid Password");
        }
        using var newhmac = new HMACSHA512();
        user.PasswordHash = newhmac.ComputeHash(Encoding.UTF8.GetBytes(updateUserDto.NewPassword));
        user.PasswordSalt = newhmac.Key;

        context.Users.Update(user);
        await context.SaveChangesAsync();

        return Ok(user);
    }
    [HttpPut("ChangeEmail")]
    public async Task<ActionResult<User>> ChangeUserEmail(UpdateUserEmailDto updateUserEmailDto)
    {
        var user = await context.Users.FirstOrDefaultAsync(x =>
        x.UserName.ToLower() == updateUserEmailDto.UserName.ToLower());
        if (user is null) return Unauthorized("Invalid User");
        using var hmac = new HMACSHA512(user.PasswordSalt);
        var computerHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(updateUserEmailDto.Password));
        for (int i = 0; i < computerHash.Length; i++)
        {
            if (computerHash[i] != user.PasswordHash[i])
                return Unauthorized("Invalid Password");
        }

        user.Email = updateUserEmailDto.Email;
        context.Users.Update(user);
        await context.SaveChangesAsync();
        return user;
    }
    [HttpDelete("DeleteByUsername")]
    public async Task<ActionResult<User>> DeleteUserByUserName(string username)
    {
        var user = await context.Users.FirstOrDefaultAsync(x =>
        x.UserName.ToLower() == username.ToLower());
        if (user is null) return Unauthorized("Not Found User");
        context.Users.Remove(user);
        await context.SaveChangesAsync();
        return user;
    }
    [HttpDelete("DeleteUserById")]
    public async Task<ActionResult<User>> DeleteUserById(int id)
    {
        var user = await context.Users.FindAsync(id);
        if (user is null) return Unauthorized("Not Found Id");
        context.Users.Remove(user);
        await context.SaveChangesAsync();
        return user;
    }
    private async Task<bool> UserExists(string username)
    {
        return await context.Users.AnyAsync(x => x.UserName.ToLower() == username.ToLower()); //Bob!=bob
    }
}

