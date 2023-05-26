using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectBackEnd.Authorization;
using ProjectBackEnd.Utility;
using ProjectBackEnd.Validation;
using Service.Contracts;
using Shared.DTO;
using Shared.RequestFeatures;
using Shared.ResponseFeatures;
using Shared.Utility;

namespace ProjectBackEnd.Controllers;

[Route("api/clients")]
[ApiController]
[CustomAuthorize]
[Authorize(Roles = "Admin, User")]
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
    [HttpPost()]
    public async Task<IActionResult> CreateClient([FromBody] ClientListDTO createClient)
    {
        var result = await _service.ClientService.CreateRecord(createClient);

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
    public async Task<IActionResult> UpdateEmailTemplate(int id, [FromBody] ClientListDTO updateClient)
    {
        var result = await _service.ClientService.UpdateRecord(id, updateClient);

        var baseResponse = new BaseResponse<object, object>
        {
            Result = result,
            Data = "",
            Errors = "",
            StatusCode = StatusCodes.Status200OK
        };
        return Ok(baseResponse);

    }

    [HttpPost("delete")]
    public async Task<IActionResult> DeleteClient(int[] clientIds)
    {
        var result = await _service.ClientService.DeleteRecord(clientIds);
        var baseResponse = new BaseResponse<object, object>
        {
            Result = result,
            Data = "",
            Errors = "",
            StatusCode = StatusCodes.Status200OK
        };
        return Ok(baseResponse);
    }
    [HttpPost("send-email")]
    public async Task<IActionResult> SendEmail([FromBody] SendEmailDTO sendEmail)
    {
        var result = await _service.ClientService.SendClientEmail(sendEmail);
        var baseResponse = new BaseResponse<object, object>
        {
            Result = true,
            Data = result,
            Errors = "",
            StatusCode = StatusCodes.Status200OK
        };
        return Ok(baseResponse);
    }

}
