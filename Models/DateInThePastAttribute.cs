using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Project.Models
{

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class DateInThePastAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            var futureDate = value as DateTime?;
            var memberNames = new List<string>() { context.MemberName };

            if (futureDate != null)
            {
                if (futureDate.Value.Date > DateTime.UtcNow.Date)
                {
                    return new ValidationResult("This must be a date in the past", memberNames);
                }
            }

            return ValidationResult.Success;
        }
    }
}

