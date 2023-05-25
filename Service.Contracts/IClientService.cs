using Shared.DTO;
using Shared.RequestFeatures;
using Shared.ResponseFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Contracts;

public interface IClientService
{
    Task<ClientListDTO> GetRecordById(int id);
    Task<PagedListResponse<IEnumerable<ClientListDTO>>> GetAllRecords(LookupRepositoryDTO filter);
}
