using System;
using System.Collections.Generic;
using System.Text;

namespace BladeVault.Application.Common.Exceptions
{
    public class ValidationException : Exception
    {
        public IDictionary<string, string[]> Errors { get; }

        public ValidationException(IDictionary<string, string[]> errors)
            : base("Помилка валідації")
        {
            Errors = errors;
        }
    }
}
