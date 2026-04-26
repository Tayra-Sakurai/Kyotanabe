using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kizu.Validators
{
    public class PastDateTimeAttribute : ValidationAttribute
    {
        private string? AssociatedProperty { get; }
        
        public PastDateTimeAttribute(string? AssociatedProperty = null)
        {
            this.AssociatedProperty = AssociatedProperty;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is DateTimeOffset date)
            {
                if (AssociatedProperty == null)
                {
                    if (date > DateTimeOffset.Now)
                        return new("The date cannot be a future date.");
                    return ValidationResult.Success;
                }
                object? associated = validationContext.ObjectInstance.GetType().GetProperty(AssociatedProperty)?.GetValue(validationContext.ObjectInstance);
                if (associated is TimeSpan time)
                {
                    DateTimeOffset dt = date.Add(time);
                    if (dt > DateTimeOffset.Now)
                        return new("The date cannot be a future date.");
                    return ValidationResult.Success;
                }
                if (date > DateTimeOffset.Now)
                    return new("The date must not be in the future.");
                return ValidationResult.Success;
            }

            if (value is TimeSpan timeSpan)
            {
                object instance = validationContext.ObjectInstance;
                object? associated1 = instance.GetType().GetProperty(AssociatedProperty!)?.GetValue(instance);

                if (associated1 is DateTimeOffset dt)
                {
                    DateTimeOffset dateTimeOffset = dt.Add(timeSpan);
                    if (dateTimeOffset > DateTimeOffset.Now)
                        return new("The time is in the future");
                    return ValidationResult.Success;
                }

                return ValidationResult.Success;
            }

            return new("The value is invalid.");
        }
    }
}
