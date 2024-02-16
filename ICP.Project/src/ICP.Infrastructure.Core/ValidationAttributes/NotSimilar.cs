using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Resources;

using System.Reflection;
using System.Globalization;

namespace ICP.Infrastructure.Core.ValidationAttributes
{
    /// <summary>
    /// 不可相似
    /// </summary>
    public class NotSimilar: ValidationAttribute
    {
        public NotSimilar(string otherPropertys): base("{0} 內容不可包含 {1}")
        {
            OtherPropertys = otherPropertys ?? throw new ArgumentNullException("otherProperty");
        }

        public string OtherPropertys { get; private set; }

        public string FormatErrorMessage(string name, string otherName)
        {
            return String.Format(CultureInfo.CurrentCulture, ErrorMessageString, name, otherName);
        }

        public override bool RequiresValidationContext
        {
            get
            {
                return true;
            }
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null) return null;

            Type tpeString = typeof(string);

            Type tpeValue = value.GetType();

            if (tpeValue != tpeString) return null;

            foreach (string OtherProperty in OtherPropertys.Split(','))
            {
                PropertyInfo otherPropertyInfo = validationContext.ObjectType.GetProperty(OtherProperty);
                if (otherPropertyInfo == null || otherPropertyInfo.PropertyType != tpeString)
                {
                    continue;
                }

                object otherPropertyValue = otherPropertyInfo.GetValue(validationContext.ObjectInstance, null);
                if (otherPropertyValue == null) continue;

                string sValue = value.ToString();
                string sValueOther = otherPropertyValue.ToString();

                if (sValue == string.Empty || sValueOther == string.Empty) continue;

                if (!sValue.Contains(sValueOther) && !sValueOther.Contains(sValue)) continue;

                string OtherPropertyDisplayName = GetDisplayNameForProperty(validationContext.ObjectType, OtherProperty);

                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName, OtherPropertyDisplayName));
            }

            return null;
        }

        private static string GetDisplayNameForProperty(Type containerType, string propertyName)
        {
            ICustomTypeDescriptor typeDescriptor = GetTypeDescriptor(containerType);
            PropertyDescriptor property = typeDescriptor.GetProperties().Find(propertyName, true);

            IEnumerable<Attribute> attributes = property.Attributes.Cast<Attribute>();
            DisplayAttribute display = attributes.OfType<DisplayAttribute>().FirstOrDefault();
            if (display != null)
            {
                return display.GetName();
            }
            DisplayNameAttribute displayName = attributes.OfType<DisplayNameAttribute>().FirstOrDefault();
            if (displayName != null)
            {
                return displayName.DisplayName;
            }
            return propertyName;
        }

        private static ICustomTypeDescriptor GetTypeDescriptor(Type type)
        {
            return new AssociatedMetadataTypeTypeDescriptionProvider(type).GetTypeDescriptor(type);
        }
    }
}
