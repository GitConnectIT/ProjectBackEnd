﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTO;

public class UserUpdateDTO
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? State { get; set; }
    public string? PhoneNumber { get; set; }    
}
