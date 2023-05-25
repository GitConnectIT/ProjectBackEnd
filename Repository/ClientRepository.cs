using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository;

public class ClientRepository : RepositoryBase<Clients>, IClientRepository
{
    public ClientRepository(RepositoryContext repositoryContext)
        : base(repositoryContext)
    {
    }
    public void CreateRecord(Clients clients) => Create(clients);
    public void DeleteRecord(Clients clients) => Delete(clients);
    public async Task<Clients> GetRecordByIdAsync(int id) =>
        await FindByCondition(c => c.Id.Equals(id))
          .SingleOrDefaultAsync();
    public void UpdateRecord(Clients clients) => Update(clients);
}
