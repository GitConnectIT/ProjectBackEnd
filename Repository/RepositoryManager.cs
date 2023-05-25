using Repository.Contracts;

namespace Repository;

public class RepositoryManager : IRepositoryManager
{
    private readonly RepositoryContext _repositoryContext;
    private readonly Lazy<IEmailTemplateRepository> _emailTemplateRepository;
    private readonly Lazy<IApplicationMenuRepository> _applicationMenuRepository;
    private readonly Lazy<IClientRepository> _clientRepository;

    public RepositoryManager(RepositoryContext repositoryContext)
    {
        _repositoryContext = repositoryContext;
        _emailTemplateRepository = new Lazy<IEmailTemplateRepository>(() => new EmailTemplateRepository(repositoryContext));
        _applicationMenuRepository = new Lazy<IApplicationMenuRepository>(() => new ApplicationMenuRepository(repositoryContext));
        _clientRepository = new Lazy<IClientRepository>(()=> new ClientRepository(repositoryContext));
    }

    public IEmailTemplateRepository EmailTemplateRepository => _emailTemplateRepository.Value;
    public IApplicationMenuRepository ApplicationMenuRepository => _applicationMenuRepository.Value;
    public IClientRepository ClientRepository=> _clientRepository.Value;
    public async Task SaveAsync()
    {
        _repositoryContext.ChangeTracker.AutoDetectChangesEnabled = false;
        await _repositoryContext.SaveChangesAsync();
    }
}