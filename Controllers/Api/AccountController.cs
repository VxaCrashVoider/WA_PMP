using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WA_ProjectManagement.Data;
using WA_ProjectManagement.Models;
using WA_ProjectManagement.Models.EF;

namespace WA_ProjectManagement.Controllers.Api;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly ApplicationDbContext _db;

    public AccountController(ApplicationDbContext db)
    {
        _db = db;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest req)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        // check email exists
        var existing = await _db.Users.FirstOrDefaultAsync(u => u.Email == req.Email);
        if (existing != null)
            return Conflict(new { message = "Email already registered" });

        // simple password hashing (PBKDF2)
        byte[] salt = RandomNumberGenerator.GetBytes(16);
        var hash = Rfc2898DeriveBytes.Pbkdf2(req.Password, salt, 100_000, HashAlgorithmName.SHA256, 32);
        var passwordHash = new byte[salt.Length + hash.Length];
        Buffer.BlockCopy(salt, 0, passwordHash, 0, salt.Length);
        Buffer.BlockCopy(hash, 0, passwordHash, salt.Length, hash.Length);

        var user = new User
        {
            UserId = Guid.NewGuid(),
            Email = req.Email,
            DisplayName = req.DisplayName,
            PasswordHash = passwordHash,
            AvatarUrl = req.AvatarUrl,
            IsActive = true,
            IsDeleted = false,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        return CreatedAtAction(null, new { id = user.UserId }, new { user.UserId, user.Email, user.DisplayName });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest req)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == req.Email);
        if (user == null)
            return Unauthorized(new { message = "Invalid credentials" });

        if (user.PasswordHash == null || user.PasswordHash.Length < 48)
            return Unauthorized(new { message = "Invalid credentials" });

        // extract salt and hash
        var salt = new byte[16];
        Buffer.BlockCopy(user.PasswordHash, 0, salt, 0, 16);
        var storedHash = new byte[user.PasswordHash.Length - 16];
        Buffer.BlockCopy(user.PasswordHash, 16, storedHash, 0, storedHash.Length);

        var incomingHash = Rfc2898DeriveBytes.Pbkdf2(req.Password, salt, 100_000, HashAlgorithmName.SHA256, storedHash.Length);

        if (!CryptographicOperations.FixedTimeEquals(incomingHash, storedHash))
            return Unauthorized(new { message = "Invalid credentials" });

        // success - return minimal user info (you can return JWT here)
        return Ok(new { user.UserId, user.Email, user.DisplayName });
    }
}
