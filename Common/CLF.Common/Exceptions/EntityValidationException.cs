using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CLF.Common.Exceptions
{
    public class EntityValidationException : Exception
    {
        public EntityValidationException() { }

        public EntityValidationException(IEnumerable<ValidationException> exceptions)
        {
            this.ValidationErrors = exceptions;
        }
        public IEnumerable<ValidationException> ValidationErrors { get; }
    }
}
