using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Contracts;

public interface IUserRepository
{
    void CreateRecord(ApplicationUser user);
    void UpdateRecord(ApplicationUser user);
    void DeleteRecord(ApplicationUser user);
    Task<ApplicationUser> GetRecordByIdAsync(int id);
}
