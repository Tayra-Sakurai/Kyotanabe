using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kizu.Validators
{
    public sealed class BalanceValidationAttribute : ValidationAttribute
    {
        public BalanceValidationAttribute(string propertyName)
        {
            PropertyName = propertyName;
        }

        private string PropertyName { get; }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            object instance = validationContext.ObjectInstance;
            double? otherValue = (double?)instance.GetType().GetProperty(PropertyName)?.GetValue(instance);
            double? valueAmount = (double?)value;

            if (otherValue is double amount && valueAmount is double thisAmount)
                return ((amount == 0 && thisAmount > 0) || (amount > 0 && thisAmount == 0)) ?
                    ValidationResult.Success : new("Invalid values. One must be 0 and the other must be greater than 0.");

            return new("Non-numeric value cannot be accepted.");
        }
    }
}
