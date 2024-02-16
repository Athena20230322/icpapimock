using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Infrastructure.Core.Utils
{
    public static class ValidateUtil
    {
        public static bool TryValidateObject(object obj, out IList<ValidationResult> validationResults)
        {
            validationResults = new List<ValidationResult>();
            return Validator.TryValidateObject(obj, new ValidationContext(obj, null, null), validationResults, true);
        }
    }
}
