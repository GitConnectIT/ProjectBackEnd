namespace Service.Contracts;

public interface IServiceManager
{
    IApplicationMenuService ApplicationMenuService { get; }
    IEmailTemplateService EmailTemplateService { get; }
    IAuthenticationService AuthenticationService { get; }
    IClientService ClientService { get; }
}