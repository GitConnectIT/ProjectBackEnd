using AutoMapper;
using EmailService;
using Entities.Exceptions;
using Entities.Models;
using Repository.Contracts;
using Service.Contracts;
using Shared.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service;

public class UserService : IUserService
{
    private readonly ILoggerManager _logger;
    private readonly IMapper _mapper;
    private readonly IRepositoryManager _repositoryManager;
    private readonly IDapperRepository _dapperRepository;
    private readonly IEmailSender _emailSender;


    public UserService(ILoggerManager logger, IMapper mapper, IRepositoryManager repositoryManager, IDapperRepository dapperRepository, IEmailSender emailSender)
    {
        _logger = logger;
        _mapper = mapper;
        _repositoryManager = repositoryManager;
        _dapperRepository = dapperRepository;
        _emailSender = emailSender;
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

    public async Task<bool> UpdateRecord(int id, UserUpdateDTO userUpdateDto)
    {
        try
        {
            var existingUser = await GetUserAndCheckIfExistsAsync(id);

            _mapper.Map(userUpdateDto, existingUser);

            _repositoryManager.UserRepository.UpdateRecord(existingUser);
            await _repositoryManager.SaveAsync();

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(string.Format("{0}: {1}", nameof(UpdateRecord), ex.Message));
            throw new BadRequestException(ex.Message);
        }
    }
    #region Private Methods

    private async Task<ApplicationUser> GetUserAndCheckIfExistsAsync(int id)
    {
        var existingUser = await _repositoryManager.UserRepository.GetRecordByIdAsync(id);
        if (existingUser is null)
            throw new NotFoundException($"Pëdoruesi me Id:{id} nuk ekziston!");

        return existingUser;
    }
    #endregion

}
