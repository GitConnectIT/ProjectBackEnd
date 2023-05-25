using AutoMapper;
using EmailService;
using Entities.Exceptions;
using Entities.Models;
using Repository.Contracts;
using Service.Contracts;
using Shared.DTC;
using Shared.DTO;
using Shared.RequestFeatures;
using Shared.ResponseFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service;

public class ClientService : IClientService
{
    private readonly ILoggerManager _logger;
    private readonly IMapper _mapper;
    private readonly IRepositoryManager _repositoryManager;
    private readonly IDapperRepository _dapperRepository;
    private readonly IEmailSender _emailSender;


    public ClientService(ILoggerManager logger, IMapper mapper, IRepositoryManager repositoryManager, IDapperRepository dapperRepository, IEmailSender emailSender)
    {
        _logger = logger;
        _mapper = mapper;
        _repositoryManager = repositoryManager;
        _dapperRepository = dapperRepository;
        _emailSender = emailSender;
    }
    public async Task<ClientListDTO> GetRecordById(int id)
    {
        try
        {
            var existingClient = await GetClientAndCheckIfExistsAsync(id);

            var existingClientDto = _mapper.Map<ClientListDTO>(existingClient);
            return existingClientDto;
        }
        catch (Exception ex)
        {
            _logger.LogError(string.Format("{0}: {1}", nameof(GetRecordById), ex.Message));
            throw new NotFoundException(ex.Message);
        }
    }
    public async Task<PagedListResponse<IEnumerable<ClientListDTO>>> GetAllRecords(LookupRepositoryDTO filter)
    {
        try
        {
            var columns = GetDataTableColumns();
            var clientsWithMetaData = await _dapperRepository.SearchClients(filter);

            PagedListResponse<IEnumerable<ClientListDTO>> response = new PagedListResponse<IEnumerable<ClientListDTO>>
            {
                //RowCount = emailTemplateWithMetaData.MetaData.TotalCount,
                //Page = emailTemplateWithMetaData.MetaData.CurrentPage,
                PageSize = clientsWithMetaData.MetaData.PageSize,
                Columns = columns,
                Rows = clientsWithMetaData,
                HasNext = clientsWithMetaData.MetaData.HasNext,
                HasPrevious = clientsWithMetaData.MetaData.HasPrevious
            };
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(string.Format("{0}: {1}", nameof(GetAllRecords), ex.Message));
            throw new BadRequestException(ex.Message);
        }
    }
    public async Task<bool> SendClientEmail(SendEmailDTO sendEmailDto)
    {
        try
        {
            var message = new Message(new string[] { sendEmailDto.ToEmail }, sendEmailDto.Subject, sendEmailDto.Body);
            await _emailSender.SendEmailAsync(message);
            return true;
        }
        catch(Exception ex)
        {
            _logger.LogError(string.Format("{0}: {1}", nameof(SendClientEmail), ex.Message));
            throw new BadRequestException(ex.Message);
        }

    }
    #region Private Methods

    private async Task<Clients> GetClientAndCheckIfExistsAsync(int id)
    {
        var existingClient = await _repositoryManager.ClientRepository.GetRecordByIdAsync(id);
        if (existingClient is null)
            throw new NotFoundException($"Klienti me Id:{id} nuk ekziston!");

        return existingClient;
    }

    private List<DataTableColumn> GetDataTableColumns()
    {
        // get the columns
        var columns = GenerateDataTableColumn<ClientColumn>.GetDataTableColumns();

        // return all columns
        return columns;
    }

    #endregion

}
