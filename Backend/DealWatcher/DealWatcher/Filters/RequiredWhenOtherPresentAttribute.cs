using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DealWatcher.Filters
{
    [AttributeUsage(AttributeTargets.Property|AttributeTargets.Field)]
    public class RequiredWhenOtherPresentAttribute : ValidationAttribute
    {
        protected Predicate<object> ValuePresentPredicate { get; private set; } 
        protected string PartnerProperty { get; private set; }

        public RequiredWhenOtherPresentAttribute(String partnerProperty, String errorMessage = "Property was present without it's partner {0}")
        {
            PartnerProperty = partnerProperty;
            ValuePresentPredicate = (obj =>obj != null);
            ErrorMessage = errorMessage;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var result = true;
            if (ValuePresentPredicate != null && ValuePresentPredicate(value))
            {
                var partnerProperty = validationContext.ObjectType.GetProperty(PartnerProperty);
                var partnerPropertyVal = partnerProperty.GetValue(validationContext.ObjectType);
                result = ValuePresentPredicate(partnerPropertyVal);
            }

            return result
                ? ValidationResult.Success
                : new ValidationResult(String.Format(ErrorMessage, PartnerProperty));
        }
    }
}