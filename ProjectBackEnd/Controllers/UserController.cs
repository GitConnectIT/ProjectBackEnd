using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectBackEnd.Authorization;
using Service.Contracts;
using Shared.DTO;
using Shared.ResponseFeatures;
using Shared.Utility;

namespace ProjectBackEnd.Controllers;

[Route("api/user")]
[ApiController]
[CustomAuthorize]
[Authorize(Roles = UserRole.Admin)]
public class UserController : ControllerBase
{
    private readonly IServiceManager _service;

    public UserController(IServiceManager service)
    {
        _service = service;
    }
    [HttpPost("delete")]
    public async Task<IActionResult> DeleteUser(int[] userIds)
    {
        var result = await _service.UserService.DeleteRecord(userIds);
        var baseResponse = new BaseResponse<object, object>
        {
            Result = result,
            Data = "",
            Errors = "",
            StatusCode = StatusCodes.Status200OK
        };
        return Ok(baseResponse);
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEmailTemplate(int id, [FromBody] UserUpdateDTO updateUser)
    {
        var result = await _service.UserService.UpdateRecord(id, updateUser);

        var baseResponse = new BaseResponse<object, object>
        {
            Result = result,
            Data = "",
            Errors = "",
            StatusCode = StatusCodes.Status200OK
        };
        return Ok(baseResponse);

    }
}