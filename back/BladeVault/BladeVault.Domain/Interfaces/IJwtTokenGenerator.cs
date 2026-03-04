using BladeVault.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BladeVault.Domain.Interfaces
{
    public interface IJwtTokenGenerator
    {
        string Generate(User user);
    }
}
