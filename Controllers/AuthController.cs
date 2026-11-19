using BookwormAPI.Data;
using BookwormAPI.Dtos;
using BookwormAPI.Services;
using CredGuard.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookwormAPI.Controllers
{
    [Route("api/v1/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly AppDbContext _context;

        public AuthController(IUserService userService, IConfiguration configuration, IRefreshTokenService refreshTokenService, AppDbContext context)
        {
            _userService = userService;
            _configuration = configuration;
            _context = context;
            _refreshTokenService = refreshTokenService;
        }

        [HttpPost("signup")]
        public async Task<ActionResult<SignupResponse>> SignUp([FromBody] SignupDto request)
        {
            if (!request.Password.Equals(request.ConfirmPassword))
            {
                return BadRequest("Passwords do not match.");
            }

            if (!request.Role.Equals(UserRoles.Admin) && !request.Role.Equals(UserRoles.User))
            {
                return BadRequest("You must provide either ADMIN or USER roles");
            }

            var newUser = await _userService.CreateUserAsync(request);

            var response = new SignupResponse
            {
                User = new UserDto
                {
                    Id = newUser.Id,
                    FirstName = newUser.FirstName,
                    LastName = newUser.LastName,
                    Email = newUser.Email,
                    Role = newUser.Role,
                    CreatedAt = newUser.CreatedAt,
                    UpdatedAt = newUser.UpdatedAt
                },

            };

            return StatusCode(StatusCodes.Status201Created, response);
        }

        [HttpPost("signin")]
        public async Task<ActionResult<SigninResponse>> SignIn([FromBody] LoginDto request)
        {
            var existingUser = await _userService.GetUserByEmailAsync(request.Email);

            if (existingUser is null) return Unauthorized("Invalid email or password.");

            var isPasswordValid = HashingService.VerifyPassword(request.Password, existingUser.PasswordHash);

            if (!isPasswordValid) return Unauthorized("Invalid email or password.");

            var tokenService = new TokenService(_configuration);

            var (accessToken, accessTokenExpiration) = tokenService.GenerateToken(existingUser.FirstName, existingUser.Email, existingUser.Role.ToString());

            var (refreshToken, refreshTokenExpiration) = await _refreshTokenService.GenerateRefreshToken(existingUser);

            var response = new SigninResponse
            {
                Data = new AuthResponse
                {
                    User = new UserDto
                    {
                        Id = existingUser.Id,
                        FirstName = existingUser.FirstName,
                        LastName = existingUser.LastName,
                        Email = existingUser.Email,
                        Role = existingUser.Role,
                        CreatedAt = existingUser.CreatedAt,
                        UpdatedAt = existingUser.UpdatedAt
                    },
                    Tokens = new TokenDto
                    {
                        AccessToken = accessToken,
                        RefreshToken = refreshToken,
                        AccessTokenExpirationTime = accessTokenExpiration
                    }
                }
            };

            Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = false, // Use false only if testing over plain HTTP without localhost exception
                // SameSite = SameSiteMode.None, // Required for cross-origin localhost:5173 -> localhost:5252
                SameSite = SameSiteMode.Lax,
                Expires = refreshTokenExpiration,
            });

            /*
            Local HTTP vs. HTTPS cookie rules: In your SignIn action, you set Secure = true and SameSite = SameSiteMode.None. Chrome blocks SameSite=None; Secure cookies over plain http://localhost:5252. Either switch baseURL to your API's HTTPS port (https://localhost:7xxx/api/v1) or set Secure = false, SameSite = SameSiteMode.Lax during local HTTP development.
            */
            return StatusCode(StatusCodes.Status201Created, response);
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<TokenDto>> RefreshToken()
        {
            if (!Request.Cookies.TryGetValue("refreshToken", out var cookieToken) || string.IsNullOrEmpty(cookieToken))
            {
                return Unauthorized("Refresh token cookie is missing.");
            }

            var storedToken = await _context.RefreshTokens.Include(rt => rt.User).FirstOrDefaultAsync(rt => rt.Token == cookieToken);

            if (storedToken is null || storedToken.IsRevoked || storedToken.ExpiresAt <= DateTime.UtcNow)
            {
                return Unauthorized("Invalid refresh token");
            }

            var tokenService = new TokenService(_configuration);
            var isRefreshTokenValid = await tokenService.ValidateRefreshToken(cookieToken);
            if (!isRefreshTokenValid) return Unauthorized("Invalid refresh token");

            storedToken.IsRevoked = true;
            var user = storedToken.User;

            var (accessToken, accessTokenExpiration) = tokenService.GenerateToken(user.FirstName, user.Email, user.Role);
            var (newRefreshToken, refreshTokenExpiration) = await _refreshTokenService.GenerateRefreshToken(user);

            Response.Cookies.Append("refreshToken", newRefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = false,
                SameSite = SameSiteMode.Lax,
                Expires = refreshTokenExpiration
            });

            var response = new SigninResponse
            {
                Data = new AuthResponse
                {
                    User = new UserDto
                    {
                        Id = user.Id,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                        Role = user.Role,
                        CreatedAt = user.CreatedAt,
                        UpdatedAt = user.UpdatedAt
                    },
                    Tokens = new TokenDto
                    {
                        AccessToken = accessToken,
                        RefreshToken = newRefreshToken,
                        AccessTokenExpirationTime = accessTokenExpiration
                    }
                }
            };

            return Ok(response);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            if (Request.Cookies.TryGetValue("refreshToken", out var cookieToken) && !string.IsNullOrEmpty(cookieToken))
            {
                var storedToken = await _context.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == cookieToken);
                if (storedToken is not null)
                {
                    storedToken.IsRevoked = true;
                    await _context.SaveChangesAsync();
                }
            }

            Response.Cookies.Delete("refreshToken", new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None
            });

            return NoContent();
        }
    }
}
