using ICP.Infrastructure.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Infrastructure.Core.Extensions
{
    public static class ValidatableObjectExtensions
    {
        public static ValidationResult GetFirstValidationResult(this ValidatableObject validatableObject)
        {
            return validatableObject.GetValidationResults()?.FirstOrDefault();
        }

        public static string GetFirstErrorMessage(this ValidatableObject validatableObject)
        {
            return validatableObject.GetFirstValidationResult()?.ErrorMessage;
        }

        public static IEnumerable<string> GetErrorMessage(this ValidatableObject validatableObject)
        {
            return validatableObject.GetValidationResults()?.Select(x => x.ErrorMessage);
        }
    }
}
