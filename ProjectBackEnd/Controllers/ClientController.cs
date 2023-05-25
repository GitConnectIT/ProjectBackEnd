using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectBackEnd.Authorization;
using ProjectBackEnd.Utility;
using Service.Contracts;
using Shared.DTO;
using Shared.RequestFeatures;
using Shared.ResponseFeatures;
using Shared.Utility;

namespace ProjectBackEnd.Controllers;

[Route("api/clients")]
[ApiController]
[CustomAuthorize]
public class ClientController : ControllerBase
{
    private readonly IServiceManager _service;

    public ClientController(IServiceManager service)
    {
        _service = service;
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdClient(int id)
    {
        var result = await _service.ClientService.GetRecordById(id);
        var baseResponse = new BaseResponse<ClientListDTO, object>
        {
            Result = true,
            Data = result,
            Errors = "",
            StatusCode = StatusCodes.Status200OK
        };
        return Ok(baseResponse);
    }
    [HttpPost("get-all")]
    public async Task<IActionResult> GetAllClients([FromBody] LookupRepositoryDTO filter)
    {
        var result = await _service.ClientService.GetAllRecords(filter);
        var baseResponse = new BaseResponse<PagedListResponse<IEnumerable<ClientListDTO>>, object>
        {
            Result = true,
            Data = result,
            Errors = "",
            StatusCode = StatusCodes.Status200OK
        };
        return Ok(baseResponse);
    }

}
