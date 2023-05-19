using Castle.Core.Logging;
using Castle.DynamicProxy;
using ICP.Infrastructure.Core.Exceptions;
using ICP.Infrastructure.Abstractions;
using ICP.Infrastructure.Core.Models;
using ICP.Infrastructure.Core.Models.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Infrastructure.Core.Frameworks.AOP
{
    /// <summary>
    /// 攔截可驗證物件並自動進行驗證
    /// </summary>
    public class ValidatableObjectInterceptor : IInterceptor
    {
        public virtual void Intercept(IInvocation invocation)
        {
            if (invocation.Arguments != null)
            {
                foreach (var item in invocation.Arguments.Where(x => x != null))
                {
                    if (item is ValidatableObject validatableObject &&
                        !validatableObject.IsValid())
                    {
                        var validationResults = validatableObject.GetValidationResults();
                        throw new Exception(item.ToString() + " 資料模型驗證失敗", new ValidatableObjectException(validationResults));
                    }
                }
            }

            invocation.Proceed();
        }
    }
}
