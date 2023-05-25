
using AutoMapper;
using Cryptography;
using EmailService;
using Entities.Configuration;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Repository.Contracts;
using Service.Contracts;
using Shared.Utility;

namespace Service;

public class ServiceManager : IServiceManager
{
    private readonly Lazy<IApplicationMenuService> _applicationMenuService;
    private readonly Lazy<IAuthenticationService> _authenticationService;
    private readonly Lazy<IEmailTemplateService> _emailTemplateService;
    private readonly Lazy<IClientService> _clientService;
    private readonly IEmailSender _emailSender;

    public ServiceManager(IRepositoryManager repositoryManager
        , IDapperRepository dapperRepository
        , ILoggerManager logger
        , IMapper mapper
        , UserManager<ApplicationUser> userManager
        , IOptions<JwtConfiguration> configuration
        , IEmailSender emailSender
        , DefaultConfiguration defaultConfig
        , SignInManager<ApplicationUser> signInManager
        , ICryptoUtils cryptoUtils
        , IEmailSender _emailSender
        )
    {
        _applicationMenuService = new Lazy<IApplicationMenuService>(() => new ApplicationMenuService(dapperRepository, logger, repositoryManager, mapper)); 
        _authenticationService = new Lazy<IAuthenticationService>(() => new AuthenticationService(logger, mapper, userManager, configuration, repositoryManager, signInManager, emailSender, cryptoUtils, defaultConfig));
        _emailTemplateService = new Lazy<IEmailTemplateService>(() => new EmailTemplateService(logger, mapper, repositoryManager, dapperRepository));
        _clientService = new Lazy<IClientService>(() => new ClientService(logger, mapper, repositoryManager, dapperRepository));

    }

    public IApplicationMenuService ApplicationMenuService => _applicationMenuService.Value;
    public IAuthenticationService AuthenticationService => _authenticationService.Value;
    public IEmailTemplateService EmailTemplateService => _emailTemplateService.Value;
    public IClientService ClientService => _clientService.Value;    

}