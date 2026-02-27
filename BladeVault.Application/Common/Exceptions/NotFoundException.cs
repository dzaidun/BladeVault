using System;
using System.Collections.Generic;
using System.Text;

namespace BladeVault.Application.Common.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string entityName, object key)
            : base($"{entityName} з ідентифікатором '{key}' не знайдено")
        {
        }
    }
}
