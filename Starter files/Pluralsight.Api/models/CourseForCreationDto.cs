using System.ComponentModel.DataAnnotations;
using CourseLibrary.API.ValidationAttributes;
using System;
using System.Collections.Generic;

namespace CourseLibrary.API.Models{
    
    [CourseTitleMustBeDifferentFromDescription]
    public class CourseForCreationDto{
        
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
        [MaxLength(1500)]
        public string Description { get; set; }

        // public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        // {
        //     if (Title == Description){
        //         yield return new ValidationResult("The description should be different from the title.", new[] { "CourseForCreationDto" });
        //     }
        // }
    }
}