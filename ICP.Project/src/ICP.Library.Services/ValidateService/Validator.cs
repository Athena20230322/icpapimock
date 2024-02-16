using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Library.Services.ValidateService
{
    public static class Validator
    {
        /// <summary>
        /// 要驗證的結構描述。
        /// </summary>
        /// <param name="source">主要驗證的元件。</param>
        /// <returns>驗證的訊息內容。</returns>
        public static IEnumerable<string> Validate(object source)
        {
            foreach (PropertyInfo propInfo in source.GetType().GetProperties())
            {
                string displayName = null;
                var attributeData = propInfo.CustomAttributes.Where(x => x.AttributeType.Name == "DisplayAttribute");
                if(attributeData.Count() > 0)
                {
                    displayName = attributeData?.First().NamedArguments[0].TypedValue.Value.ToString() ?? null;
                }

                object[] customAttributes = propInfo.GetCustomAttributes(typeof(ValidationAttribute), inherit: true);

                foreach (object customAttribute in customAttributes)
                {
                    bool isValid;
                    ValidAttribute(source, propInfo, customAttribute, out ValidationAttribute validationAttribute, out isValid);

                    if (!isValid)
                    {
                        yield return validationAttribute.FormatErrorMessage(displayName ?? propInfo.Name);
                    }
                }

            }
        }

        private static void ValidAttribute(object item, PropertyInfo detailPropInfo, object customAttribute, out ValidationAttribute validationAttribute, out bool isValid)
        {
            validationAttribute = (ValidationAttribute)customAttribute;

            isValid = false;
            // 預設驗證的 Attributes。
            if (validationAttribute.GetType() == typeof(RequiredAttribute) || validationAttribute.GetType() == typeof(RangeAttribute)
                || validationAttribute.GetType() == typeof(RegularExpressionAttribute) || validationAttribute.GetType() == typeof(StringLengthAttribute))
            {
                isValid = validationAttribute.IsValid(detailPropInfo.GetValue(item, BindingFlags.GetProperty, null, null, null));
            }

            // 自訂驗證的 Attributes。
            else
            {
                isValid = validationAttribute.IsValid(new object[] { detailPropInfo.Name, detailPropInfo.GetValue(item, BindingFlags.GetProperty, null, null, null), item });
            }
        }
    }
}
