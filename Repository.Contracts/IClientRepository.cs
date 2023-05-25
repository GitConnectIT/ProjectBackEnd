using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Contracts;

public interface IClientRepository
{
    void CreateRecord(Clients clients);
    Task<Clients> GetRecordByIdAsync(int id);
    void UpdateRecord(Clients clients);
    void DeleteRecord(Clients clients);
}
