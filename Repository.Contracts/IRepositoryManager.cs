namespace Repository.Contracts;

public interface IRepositoryManager
{
    IEmailTemplateRepository EmailTemplateRepository { get; }
    IApplicationMenuRepository ApplicationMenuRepository { get; }
    IClientRepository ClientRepository { get; } 
    IUserRepository UserRepository { get; }
    Task SaveAsync();
}