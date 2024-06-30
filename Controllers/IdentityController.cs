using Microsoft.AspNetCore.Mvc;
using RandomApp1.Dtos;
using RandomApp1.Enums;
using RandomApp1.Models;
using RandomApp1.Services;

namespace RandomApp1.Controllers
{
    [ApiController]
    public class IdentityController : BaseController
    {
        private readonly IIdentityService _identityService;
        private readonly ITokenService _tokenService;

        public IdentityController(IIdentityService identityService, ITokenService tokenService)
        {
            _identityService = identityService;
            _tokenService = tokenService;
        }

        [HttpPost]
        [Route("register")]
        public async Task<ActionResult<int>> RegisterUser(RegisterUserDto registerUserDto)

        {
            var exisitingUser = await _identityService.GetUserByEmail(registerUserDto.Email);

            if (exisitingUser is not null)
            {
                return BadRequest(ErrorCode.UserAlreadyExists);
            }
            var id = await _identityService.Register(registerUserDto);
            if (id < 0)
            {
                throw new InvalidOperationException();
            }

            return Ok(id);
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<LoginResponseDto>> Login(LoginDto loginDto)
        {
            var user = await _identityService.GetUserByEmail(loginDto.Email);
            if (user is null)
            {
                return BadRequest(ErrorCode.UserNotFound);
            }

            var authorized = _identityService.Login(loginDto, user);
            if (!authorized) return Unauthorized(ErrorCode.Unauthorized);

            LoginResponseDto response = await GetLoginReponse(user);

            return Ok(response);
        }

        [HttpPost]
        [Route("refreshToken")]
        public async Task<ActionResult<LoginResponseDto>> RefreshToken(RefreshTokenRequestDto refreshTokenReqDto)
        {
            var token = await _tokenService.GetRefreshToken(refreshTokenReqDto.refreshTokenString);
            if (token is null) return Unauthorized(ErrorCode.Unauthorized);
            await _tokenService.DeleteRefreshToken(refreshTokenReqDto.refreshTokenString);

            var user = await _identityService.GetUserById(token.UserId);
            if (user is null)
            {
                return BadRequest(ErrorCode.UserNotFound);
            }

            LoginResponseDto response = await GetLoginReponse(user);

            return Ok(response);
        }

        [HttpPost]
        [Route("revokeToken")]
        public async Task<ActionResult> RevokeToken(RefreshTokenRequestDto refreshTokenReqDto)
        {
            var token = await _tokenService.GetRefreshToken(refreshTokenReqDto.refreshTokenString);
            if (token is null) return Ok();
            await _tokenService.DeleteRefreshToken(refreshTokenReqDto.refreshTokenString);
            return Ok();
        }

        private async Task<LoginResponseDto> GetLoginReponse(IdentityUser user)
        {
            var accessTokenRes = _tokenService.GetToken(user);
            var refreshToken = _tokenService.GenerateRefreshToken();
            var refreshTokenId = await _tokenService.SaveRefreshToken(refreshToken, user.Id);
            if (refreshTokenId < 0)
            {
                throw new InvalidOperationException();
            }

            var response = new LoginResponseDto()
            {
                Username = user.Username,
                Email = user.Email,
                AccessToken = accessTokenRes.Token,
                AccessTokenExpiration = accessTokenRes.Expiration,
                RefreshToken = refreshToken,
            };
            return response;
        }
    }
}