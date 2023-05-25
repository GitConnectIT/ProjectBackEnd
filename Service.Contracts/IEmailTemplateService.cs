using Shared.DTO;
using Shared.RequestFeatures;
using Shared.ResponseFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Contracts
{
    public interface IEmailTemplateService
    {
        Task<bool> CreateRecord(EmailTemplateDTO createEmailTemplateDTO, int userId);
        Task<EmailTemplateDTO> GetRecordById(int id);
        Task<bool> UpdateRecord(int id, EmailTemplateDTO updateEmailTemplateDto, int userId);
        Task<bool> DeleteRecord(int[] emailTemplateIds);
        Task<PagedListResponse<IEnumerable<EmailTemplateDTO>>> GetAllRecords(LookupRepositoryDTO filter);
    }
}
