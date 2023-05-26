using AutoMapper;
using DocumentFormat.OpenXml.Office2010.Excel;
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
    public async Task<bool> CreateRecord(ClientListDTO createClientDto)
    {
        try
        {
            var client = _mapper.Map<Clients>(createClientDto);

            _repositoryManager.ClientRepository.CreateRecord(client);
            await _repositoryManager.SaveAsync();

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(string.Format("{0}: {1}", nameof(CreateRecord), ex.Message));
            throw new BadRequestException(ex.Message);
        }
    }

    public async Task<bool> DeleteRecord(int[] clientIds)
    {
        try
        {
            await _dapperRepository.DeleteClient(clientIds);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(string.Format("{0}: {1}", nameof(DeleteRecord), ex.Message));
            throw new BadRequestException(ex.Message);
        }
    }

    public async Task<bool> UpdateRecord(int id, ClientListDTO clientDto)
    {
        try
        {
            var exisitingClient = await GetClientAndCheckIfExistsAsync(id);

            _mapper.Map(clientDto, exisitingClient);

            _repositoryManager.ClientRepository.UpdateRecord(exisitingClient);
            await _repositoryManager.SaveAsync();

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(string.Format("{0}: {1}", nameof(UpdateRecord), ex.Message));
            throw new BadRequestException(ex.Message);
        }
    }
    public async Task<bool> SendClientEmail(SendEmailDTO sendEmailDto)
    {
        try
        {
            foreach(var clientId in sendEmailDto.ClientIds)
            {
                var existingClient = await _repositoryManager.ClientRepository.GetRecordByIdAsync(clientId);
                if (existingClient is not null)
                {
                    //var emailMessage = new MimeMessage();

                    //var bodyBuilder = new BodyBuilder();
                    //bodyBuilder.TextBody = sendEmailDto.Body;
                    //if(sendEmailDto.Attachments !=null && sendEmailDto.Attachments.Any())
                    //{
                    //    byte[] fileeBytes;
                    //    foreach(var attachment in sendEmailDto.Attachments)
                    //    {
                    //        using(var ms =new MemoryStream())
                    //        {
                    //            attachment.CopyTo(ms);
                    //            fileeBytes = ms.ToArray();
                    //        }
                    //        bodyBuilder.Attachments.Add(attachment.FileName, fileeBytes, ContentType.Parse(attachment.ContentType));
                    //    }
                        
                    //}
                    //emailMessage.Body = bodyBuilder.ToMessageBody();

                    var message = new Message(new string[] { existingClient.Email }, sendEmailDto.Subject, sendEmailDto.Body);
                    await _emailSender.SendEmailAsync(message);
                }
            }
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
