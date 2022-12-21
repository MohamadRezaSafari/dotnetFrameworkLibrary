using Clinic.Controllers;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Providers
{
    public class UnixDateTimeAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            //try
            //{
            var propertyName = validationContext.ObjectType.GetProperty("StartDate");
            var x = value;
                if (value != null)
                {
                    DateTime dateTime = Requirements.UnixTimeStampToDateTime(Convert.ToDouble(value));
                   
                    validationContext
                        .ObjectType
                        .GetProperty(validationContext.MemberName)
                        .SetValue(validationContext.ObjectInstance, dateTime, null);                    
                }
                return ValidationResult.Success;
            //}
            //catch (Exception error)
            //{
            //    throw new Exception(error.Message);
            //}
        }
    }
}