using ICP.Infrastructure.Core.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Infrastructure.Core.Models
{
    /// <summary>
    /// 可驗證的資料模型
    /// </summary>
    public class ValidatableObject
    {
        /// <summary>
        /// 是否驗證成功
        /// </summary>
        /// <returns></returns>
        public virtual bool IsValid(string[] ignore = null)
        {
            if (ignore == null || ignore.Length == 0)
            {
                return GetValidationResults().Count == 0;
            }

            return GetValidationResults(ignore).Count == 0;
        }

        private ValidationResult Ignore(ValidationResult validationResult, string[] fields)
        {
            if (!(validationResult is ValidationAttributes.CompositeValidationResult))
            {
                int errorCount = validationResult.MemberNames.Where(t => !fields.Contains(t)).Count();

                if (errorCount == 0) return null;

                return validationResult;
            }
            else
            {
                var compositeValidationResult = (ValidationAttributes.CompositeValidationResult)validationResult;

                var ignoreFields = new List<ValidationResult>();

                compositeValidationResult.Results.ToList().ForEach(result =>
                {
                    var result2 = Ignore(result, fields);
                    if (result2 == null)
                    {
                        ignoreFields.Add(result);
                    }
                });

                ignoreFields.ForEach(result =>
                {
                    compositeValidationResult.RemoveResult(result);
                });

                if (compositeValidationResult.Results.Count() == 0) return null;

                return validationResult;
            }
        }

        /// <summary>
        /// 取得驗證結果
        /// </summary>
        /// <returns></returns>
        public IList<ValidationResult> GetValidationResults()
        {
            ValidateUtil.TryValidateObject(this, out IList<ValidationResult> validationResults);
            return validationResults;
        }

        /// <summary>
        /// 取得驗證結果
        /// </summary>
        /// <returns></returns>
        public IList<ValidationResult> GetValidationResults(string[] ignore = null)
        {
            var ignoreFields = new List<ValidationResult>();
            var validationResults = GetValidationResults().ToList();
            validationResults.ForEach(result =>
            {
                var result2 = Ignore(result, ignore);
                if (result2 == null)
                {
                    ignoreFields.Add(result);
                }
            });

            ignoreFields.ForEach(result =>
            {
                validationResults.Remove(result);
            });

            return validationResults;
        }
    }
}
