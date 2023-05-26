using Shared.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Contracts;

public interface IUserService
{
    Task<bool> UpdateRecord(int id, UserUpdateDTO userUpdateDto);
    Task<bool> DeleteRecord(int[] clientIds);
}
