

using Shared.DTO;
using Shared.RequestFeatures;

namespace Repository.Contracts;

public interface IDapperRepository
{
    public  Task<IEnumerable<ApplicationMenuDTO>> GetChidlrenMenuAsync(string userRole, int userId, int parentId);
    public  Task<IEnumerable<ApplicationMenuDTO>> GetApplicationMenuAsync(string userRole, int userId);
    public  Task<IEnumerable<ApplicationMenuDTO>> AllMenuList();
    public  Task<ApplicationMenuDTO> GetMenuById(int id);


    Task<bool> DeleteEmailTemplate(int[] emailTemplateIds);

    //TABLE PAGGINATION

    public Task<PagedList<ClientListDTO>> SearchClients(LookupRepositoryDTO filter);
    
    public Task<IEnumerable<ClientListDTO>> ClientsTable(LookupRepositoryDTO filter);


    Task<IEnumerable<EmailTemplateDTO>> EmailTemplateTable(LookupRepositoryDTO filter);
    Task<PagedList<EmailTemplateDTO>> SearchEmailTemplate(LookupRepositoryDTO filter);
}