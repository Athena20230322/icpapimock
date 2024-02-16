using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Infrastructure.Core.Exceptions
{
    public class ValidatableObjectException : Exception
    {
        private readonly IList<ValidationResult> _validationResults = null;

        public ValidatableObjectException(IList<ValidationResult> validationResults)
        {
            _validationResults = validationResults;
        }

        public override string Message
        {
            get
            {
                return string.Join(", ", _validationResults.Select(x => x.ErrorMessage));
            }
        }
    }
}
