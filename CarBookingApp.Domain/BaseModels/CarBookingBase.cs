using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CarBookingApp.Domain.Models
{
    public abstract class CarBookingBase : IValidatableObject
    {
        [Required]
        [StringLength(80)]
        public string FullName { get; set; }
        [Required]
        [StringLength(80)]
        public string Email { get; set; }
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
           if(Date<DateTime.Now.Date)
                {
                yield return new ValidationResult("Date Must be In Future", new[] { nameof(Date) });
            }

        }
    }
}
