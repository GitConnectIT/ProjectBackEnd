
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectBackEnd.Utility;
using ProjectBackEnd.Validation;
using Service.Contracts;
using Shared.DTO;
using Shared.ResponseFeatures;
using Shared.Utility;

namespace ProjectBackEnd.Controllers;

[Route("api/authentication")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly IServiceManager _service;

    public AuthenticationController(IServiceManager service)
    {
        _service = service;
    }

    [HttpPost("register")]
    [Authorize(Roles = UserRole.Admin)]
    public async Task<IActionResult> Register([FromBody] UserRegisterDTO userRegister)
    {
        UserRegisterDTOValidator validator = new UserRegisterDTOValidator();
        var validatorResult = validator.Validate(userRegister);
        if (!validatorResult.IsValid)
        {
            var baseResponse = new BaseResponse<object, List<ValidationFailure>>
            {
                Result = false,
                Data = "",
                Errors = validatorResult.Errors,
                StatusCode = StatusCodes.Status400BadRequest
            };
            return Ok(baseResponse);
        }
        else
        {
            var result = await _service.AuthenticationService.CreateUserAsync(userRegister, UserRole.User);

            if (!result.Succeeded)
            {
                var errorDetailsStr = string.Join("|", result.Errors.Select(x => x.Description));
                var baseResponse = new BaseResponse<object, string>
                {
                    Result = false,
                    Data = "",
                    Errors = errorDetailsStr,
                    StatusCode = StatusCodes.Status400BadRequest
                };
                return Ok(baseResponse);
            }
            else
            {
                var baseResponse = new BaseResponse<object, object>
                {
                    Result = true,
                    Data = "",
                    Errors = "",
                    StatusCode = StatusCodes.Status200OK
                };
                return Ok(baseResponse);
            }

        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginDTO userLogin)
    {
        UserLoginDTOValidator validator = new UserLoginDTOValidator();
        var validatorResult = validator.Validate(userLogin);
        if (!validatorResult.IsValid)
        {
            var baseResponse = new BaseResponse<object, List<ValidationFailure>>
            {
                Result = false,
                Data = "",
                Errors = validatorResult.Errors,
                StatusCode = StatusCodes.Status400BadRequest
            };
            return Ok(baseResponse);
        }
        else
        {
            var tokenDto = await _service.AuthenticationService.ValidateUserAndCreateToken(userLogin);
            var baseResponse = new BaseResponse<TokenDTO, object>
            {
                Result = true,
                Data = tokenDto,
                Errors = "",
                StatusCode = StatusCodes.Status200OK
            };
            return Ok(baseResponse);
        }
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] TokenDTO tokenDto)
    {
        var tokenDtoToReturn = await _service.AuthenticationService.RefreshToken(tokenDto);
        var baseResponse = new BaseResponse<TokenDTO, object>
        {
            Result = true,
            Data = tokenDtoToReturn,
            Errors = "",
            StatusCode = StatusCodes.Status200OK
        };
        return Ok(baseResponse);
    }
    [HttpPut("confirmation-email")]
    public async Task<IActionResult> ConfirmationEmail([FromBody] ConfirmationEmailDTO confirmationEmail)
    {
        var result = await _service.AuthenticationService.ConfirmEmail(confirmationEmail);
        var baseResponse = new BaseResponse<object, object>
        {
            Result = true,
            Data = result,
            Errors = "",
            StatusCode = StatusCodes.Status200OK
        };
        return Ok(baseResponse);
    }
    [HttpGet("userById")]
    [Authorize]
    public async Task<IActionResult> GetLoggedUser()
    {
        var result = await _service.AuthenticationService.GetRecordById(ClaimsUtility.ReadCurrentUserId(User.Claims));
        var baseResponse = new BaseResponse<UserListDTO, object>
        {
            Result = true,
            Data = result,
            Errors = "",
            StatusCode = StatusCodes.Status200OK
        };
        return Ok(baseResponse);
    }
}