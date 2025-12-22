using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AuthenticationSystem.Common;
using AuthenticationSystem.Entities;
using AuthenticationSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace AuthenticationSystem.Services;

public class AuthService(IConfiguration configuration, IUserService userService)
    : IAuthService
    {
        private readonly int AccessTokenExpirationInMinutes = 15;
        private readonly int RefreshTokenExpirationInDays = 7;
        public async Task<ApiResponse<TokenResponse>> LoginAsync(LoginRequest request)
        {
            var user = await userService.GetUserWithRolesByUserName(request.Username);
            if (user is null)
            {
                return ApiResponse<TokenResponse>.ErrorResponse("User not found");
            }
            // user.PasswordHash : FORMAT_MARKER:ALGORITHM_VERSION:ITERATION_COUNT:SALT:HASH_RESULT
            // extracts the salt, algorithm and hashed value
            // using that salt and algorithm, generate hash result for the password
            // compare and verify the hash result value
            if (new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, request.Password)
                == PasswordVerificationResult.Failed)
            {
                return ApiResponse<TokenResponse>.ErrorResponse("Incorrect Password");
            }

            var tokenResponse = await CreateTokenResponse(user);
            return new ApiResponse<TokenResponse>(tokenResponse);
        }

        public Task<ApiResponse<TokenResponse>> RefreshTokensAsync(RefreshTokenRequest request)
        {
            throw new NotImplementedException();
        }

        private async Task<TokenResponse> CreateTokenResponse(User user)
        {
            return new TokenResponse
            {
                AccessToken = CreateToken(user),
                RefreshToken = await GenerateAndSaveRefreshTokenAsync(user),
                AccessTokenExpiryInMinute = AccessTokenExpirationInMinutes,
                RefreshTokenExpiryInDays =  RefreshTokenExpirationInDays
            };
        }

        public async Task<ApiResponse<bool>> RegisterAsync(RegisterUserRequest request)
        {
            var user = await userService.GetUserByUserName(request.Username);
            if (user is not null)
            {
                return ApiResponse<bool>
                    .ErrorResponse("User already exists");
            }

            var newUser = new User
            {
                Username = request.Username,
                MobileNumber = request.MobileNumber
            };
            //
            // This string contains:
            // A format marker (algorithm version)
            // The salt value (random)
            // The actual hash result (can convert string to hash but not reverse)
            var hashedPassword = new PasswordHasher<User>()
                .HashPassword(newUser, request.Password);

            newUser.PasswordHash = hashedPassword;

            var roles = await userService.GetRolesByRoleName(request.Roles);

            await userService.CreateUserAsync(newUser, roles);
            return ApiResponse<bool>.SuccessResponse(true, "User registered successfully");
        }

        public async Task<ApiResponse<TokenResponse>> RefreshTokensAsync(string refreshToken)
        {
            var validateResponse = await ValidateRefreshTokenAsync(refreshToken);
            if (!validateResponse.Success)
                return ApiResponse<TokenResponse>.ErrorResponse(validateResponse.Title??"Unable to refresh token");

            var tokenResponse = await CreateTokenResponse(validateResponse.Data!);
            return new ApiResponse<TokenResponse>(tokenResponse);
        }

        private async Task<ApiResponse<User>> ValidateRefreshTokenAsync(string refreshToken)
        {
            var loginToken = await userService.GetUserLoginTokenByToken(refreshToken);
            if (loginToken is null)
                return ApiResponse<User>.ErrorResponse("Token does not exist. Invalid refresh token");
            if (loginToken.RefreshTokenExpiryTime <= DateTime.UtcNow )
                return ApiResponse<User>.ErrorResponse("Refresh token expired");
            var user = await userService.GetUserByUserId(loginToken.UserId);
            return ApiResponse<User>.SuccessResponse(user!,"Refresh token valid");
        }


        private static string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private async Task<string> GenerateAndSaveRefreshTokenAsync(User user)
        {
            var refreshToken = GenerateRefreshToken();
            await userService.SaveOrUpdateUserRefreshToken(user.UserId, refreshToken, RefreshTokenExpirationInDays);
            return refreshToken;
        }


        private string CreateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.MobilePhone, user.MobileNumber),
            };
            foreach (var role in user.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.Name));
            }

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(configuration.GetValue<string>("JwtConfigs:AccessTokenSecret")!));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var tokenDescriptor = new JwtSecurityToken(
                issuer: configuration.GetValue<string>("JwtConfigs:Issuer"),
                audience: configuration.GetValue<string>("JwtConfigs:Audience"),
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(AccessTokenExpirationInMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }

        public async Task LogoutUser(Guid userId)
        {
            await userService.RemoveUserLoginTokenByUserId(userId);
        }
    }