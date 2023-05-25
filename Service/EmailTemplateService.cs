using AutoMapper;
using ClosedXML.Excel;
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

namespace Service
{
    public class EmailTemplateService : IEmailTemplateService
    {
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly IRepositoryManager _repositoryManager;
        private readonly IDapperRepository _dapperRepository;

        public EmailTemplateService(ILoggerManager logger, IMapper mapper, IRepositoryManager repositoryManager, IDapperRepository dapperRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _repositoryManager = repositoryManager;
            _dapperRepository = dapperRepository;
        }

        public async Task<bool> CreateRecord(EmailTemplateDTO createEmailTemplateDTO, int userId)
        {
            try
            {
                var checkEmailTemplateCode = await _repositoryManager.EmailTemplateRepository.GetRecordByCodeAsync(createEmailTemplateDTO.Code);
                if (checkEmailTemplateCode is not null)
                    throw new BadRequestException($"Kodi {createEmailTemplateDTO.Code} i email template ekziston! Vendosni një kod unik!");

                var emailTemplate = _mapper.Map<EmailTemplate>(createEmailTemplateDTO);

                emailTemplate.DateCreated = DateTime.Now;
                emailTemplate.CreatedBy = userId;

                _repositoryManager.EmailTemplateRepository.CreateRecord(emailTemplate);
                await _repositoryManager.SaveAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("{0}: {1}", nameof(CreateRecord), ex.Message));
                throw new BadRequestException(ex.Message);
            }
        }

        public async Task<bool> DeleteRecord(int[] emailTemplateIds)
        {
            try
            {
                await _dapperRepository.DeleteEmailTemplate(emailTemplateIds);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("{0}: {1}", nameof(DeleteRecord), ex.Message));
                throw new BadRequestException(ex.Message);
            }
        }

        public async Task<EmailTemplateDTO> GetRecordById(int id)
        {
            try
            {
                var existingEmailTemplate = await GetEmailTemplateAndCheckIfExistsAsync(id);

                var emailTemplateDto = _mapper.Map<EmailTemplateDTO>(existingEmailTemplate);
                return emailTemplateDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("{0}: {1}", nameof(GetRecordById), ex.Message));
                throw new NotFoundException(ex.Message);
            }
        }

        public async Task<bool> UpdateRecord(int id, EmailTemplateDTO updateEmailTemplateDto, int userId)
        {
            try
            {
                var existingEmailTemplate = await GetEmailTemplateAndCheckIfExistsAsync(id);

                if (existingEmailTemplate.Code != updateEmailTemplateDto.Code)
                {
                    var checkEmailTemplateCode = await _repositoryManager.EmailTemplateRepository.GetRecordByCodeAsync(updateEmailTemplateDto.Code);
                    if (checkEmailTemplateCode != null)
                        throw new BadRequestException($"Kodi {updateEmailTemplateDto.Code} i email template ekziston! Vendosni një kod unik!");
                }

                _mapper.Map(updateEmailTemplateDto, existingEmailTemplate);

                existingEmailTemplate.DateModified = DateTime.Now;
                existingEmailTemplate.ModifiedBy = userId;

                _repositoryManager.EmailTemplateRepository.UpdateRecord(existingEmailTemplate);
                await _repositoryManager.SaveAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("{0}: {1}", nameof(UpdateRecord), ex.Message));
                throw new BadRequestException(ex.Message);
            }
        }

        public async Task<PagedListResponse<IEnumerable<EmailTemplateDTO>>> GetAllRecords(LookupRepositoryDTO filter)
        {
            try
            {
                var columns = GetDataTableColumns();
                var emailTemplateWithMetaData = await _dapperRepository.SearchEmailTemplate(filter);

                PagedListResponse<IEnumerable<EmailTemplateDTO>> response = new PagedListResponse<IEnumerable<EmailTemplateDTO>>
                {
                    //RowCount = emailTemplateWithMetaData.MetaData.TotalCount,
                    //Page = emailTemplateWithMetaData.MetaData.CurrentPage,
                    PageSize = emailTemplateWithMetaData.MetaData.PageSize,
                    Columns = columns,
                    Rows = emailTemplateWithMetaData,
                    HasNext = emailTemplateWithMetaData.MetaData.HasNext,
                    HasPrevious = emailTemplateWithMetaData.MetaData.HasPrevious
                };
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("{0}: {1}", nameof(GetAllRecords), ex.Message));
                throw new BadRequestException(ex.Message);
            }
        }
        

        #region Private Methods

        private async Task<EmailTemplate> GetEmailTemplateAndCheckIfExistsAsync(int id)
        {
            var existingEmailTemplate = await _repositoryManager.EmailTemplateRepository.GetRecordByIdAsync(id);
            if (existingEmailTemplate is null)
                throw new NotFoundException($"Email Template me Id:{id} nuk ekziston!");

            return existingEmailTemplate;
        }

        private List<DataTableColumn> GetDataTableColumns()
        {
            // get the columns
            var columns = GenerateDataTableColumn<EmailTemplateColumn>.GetDataTableColumns();

            // return all columns
            return columns;
        }

        #endregion

    }

}
