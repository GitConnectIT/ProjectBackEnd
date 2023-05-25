using DocumentFormat.OpenXml.Spreadsheet;
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

namespace ProjectBackEnd.Controllers
{
    [Route("api/emailtemplate")]
    [ApiController]
    [Authorize(Roles = UserRole.Admin)]
    [CustomAuthorize]
    public class EmailTemplateController : ControllerBase
    {
        private readonly IServiceManager _service;

        public EmailTemplateController(IServiceManager service)
        {
            _service = service;
        }

        [HttpPost()]
        public async Task<IActionResult> CreateEmailTemplate([FromBody] EmailTemplateDTO createEmailTemplate)
        {
            CreateEmailTemplateDTOValidator validator = new CreateEmailTemplateDTOValidator();
            var validatorResult = validator.Validate(createEmailTemplate);
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
                var result = await _service.EmailTemplateService.CreateRecord(createEmailTemplate, ClaimsUtility.ReadCurrentUserId(User.Claims));

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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdEmailTemplate(int id)
        {
            var result = await _service.EmailTemplateService.GetRecordById(id);
            var baseResponse = new BaseResponse<EmailTemplateDTO, object>
            {
                Result = true,
                Data = result,
                Errors = "",
                StatusCode = StatusCodes.Status200OK
            };
            return Ok(baseResponse);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmailTemplate(int id, [FromBody] EmailTemplateDTO updateEmailTemplate)
        {
            UpdateEmailTemplateDTOValidator validator = new UpdateEmailTemplateDTOValidator();
            var validatorResult = validator.Validate(updateEmailTemplate);
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
                var result = await _service.EmailTemplateService.UpdateRecord(id, updateEmailTemplate, ClaimsUtility.ReadCurrentUserId(User.Claims));

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

        [HttpPost("delete")]
        public async Task<IActionResult> DeleteEmailTemplate(int[] emailTemplateIds)
        {
            var result = await _service.EmailTemplateService.DeleteRecord(emailTemplateIds);
            var baseResponse = new BaseResponse<object, object>
            {
                Result = result,
                Data = "",
                Errors = "",
                StatusCode = StatusCodes.Status200OK
            };
            return Ok(baseResponse);
        }

        [HttpPost("get-all")]
        public async Task<IActionResult> GetAllEmailTemplate([FromBody] LookupRepositoryDTO filter)
        {
            var result = await _service.EmailTemplateService.GetAllRecords(filter);
            var baseResponse = new BaseResponse<PagedListResponse<IEnumerable<EmailTemplateDTO>>, object>
            {
                Result = true,
                Data = result,
                Errors = "",
                StatusCode = StatusCodes.Status200OK
            };
            return Ok(baseResponse);
        }
       
    }
}
