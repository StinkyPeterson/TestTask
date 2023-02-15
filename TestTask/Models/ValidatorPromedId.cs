using System.ComponentModel.DataAnnotations;

namespace TestTask.Models
{
    public class ValidatorStationAdisId : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            DbContext dbContext = new DbContext();
            if (value != null)
            {
                if (dbContext.GetSubstations().Where(x => x.Station.StationAdisId == (int)value).Count() > 0)
                {
                    return new ValidationResult("Данный StationAdisId уже занят");
                }
                else
                {
                    return ValidationResult.Success;
                }
            }
            return ValidationResult.Success;
        }
        public class ValidatorSubstationId : ValidationAttribute
        {
            protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
            {
                DbContext dbContext = new DbContext();
                if (value != null)
                {
                    if (dbContext.GetSubstations().Where(x => x.SubstationId == (int)value).Count() > 0)
                    {
                        return new ValidationResult("Данный SubstationId уже занят");
                    }
                    else
                    {
                        return ValidationResult.Success;
                    }
                }
                return ValidationResult.Success;
            }
        }
    }
}
