﻿using Entities.Models;
using Shared.DTO;
using Shared.RequestFeatures;

namespace Repository.Contracts;

public interface IEmailTemplateRepository
{
    void CreateRecord(EmailTemplate emailTemplate);
    Task<EmailTemplate> GetRecordByIdAsync(int id);
    Task<EmailTemplate> GetRecordByCodeAsync(string code);
    void UpdateRecord(EmailTemplate emailTemplate);
    void DeleteRecord(EmailTemplate emailTemplate);
}